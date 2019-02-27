//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using SQLite.Attribute;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterRequest : NWDBasis<NWDUserBarterRequest>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void barterRequestBlock(bool error, NWDTradeStatus status, NWDOperationResult result);
        public barterRequestBlock barterRequestBlockDelegate;
        public delegate void synchronizeBlock(bool error, NWDOperationResult result);
        public static synchronizeBlock synchronizeBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterRequest()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterRequest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            ForRelationshipOnly = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDBarterPlace), typeof(NWDUserBarterRequest), typeof(NWDUserBarterProposition), typeof(NWDUserBarterFinder) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Request
            NWDUserBarterRequest rRequest = NewData();
            #if UNITY_EDITOR
            rRequest.InternalKey = NWDAccountNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
            #endif
            rRequest.Tag = NWDBasisTag.TagUserCreated;
            rRequest.BarterPlace.SetObject(sBarterPlace);
            rRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            rRequest.BarterStatus = NWDTradeStatus.Submit;
            rRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            rRequest.SaveData();

            return rRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateFriendBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed, NWDUserRelationship sRelationship)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Request
            NWDUserBarterRequest rRequest = NewData();
            #if UNITY_EDITOR
            rRequest.InternalKey = NWDAccountNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
            #endif
            rRequest.Tag = NWDBasisTag.TagUserCreated;
            rRequest.BarterPlace.SetObject(sBarterPlace);
            rRequest.UserRelationship.SetObject(sRelationship);
            rRequest.ForRelationshipOnly = true;
            rRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            rRequest.BarterStatus = NWDTradeStatus.Submit;
            rRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            rRequest.SaveData();

            return rRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest[] FindRequestsReceivedWith(NWDBarterPlace sBarterPlace)
        {
            List<NWDUserBarterRequest> rUserBartersRequest = new List<NWDUserBarterRequest>();
            NWDUserRelationship[] tRelationships = NWDUserRelationship.FindDatas();
            foreach (NWDUserRelationship k in tRelationships)
            {
                if (k.RelationshipStatus == NWDRelationshipStatus.Valid)
                {
                    NWDUserBarterRequest[] tFound = FindRequestsSentWith(sBarterPlace, k.FriendAccount.GetReference(), k.FriendGameSave.GetObject());
                    foreach (NWDUserBarterRequest j in tFound)
                    {
                        rUserBartersRequest.Add(j);
                    }
                }
            }

            return rUserBartersRequest.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest[] FindRequestsSentWith(NWDBarterPlace sBarterPlace, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            List<NWDUserBarterRequest> rUserBartersRequest = new List<NWDUserBarterRequest>();
            foreach (NWDUserBarterRequest k in FindDatas(sAccountReference, sGameSave))
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    rUserBartersRequest.Add(k);
                }
            }

            return rUserBartersRequest.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetNumberOfRequestSentFor(NWDBarterPlace sBarterPlace, NWDUserRelationship sRelationship)
        {
            int rCpt = 0;
            foreach (NWDUserBarterRequest k in FindDatas())
            {
                NWDBarterPlace tPlace = k.BarterPlace.GetObject();
                if (tPlace != null && tPlace.Equals(sBarterPlace) &&
                    k.UserRelationship.GetObject().Equals(sRelationship))
                {
                    rCpt++;
                }
            }

            return rCpt;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterRequest()
        {
            // Sync NWDUserBarterRequest
            SynchronizationFromWebService(BarterRequestSuccessBlock, BarterRequestFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddOrRemoveItems()
        {
            switch (BarterStatus)
            {
                case NWDTradeStatus.Waiting:
                    {
                        // Remove NWDItem from NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                        }
                    }
                    break;
                case NWDTradeStatus.Expired:
                    {
                        // Add NWDItem to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                case NWDTradeStatus.Accepted:
                    {
                        // Add NWDItem Ask to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsReceived.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                default:
                    break;
            }

            // Sync NWDUserOwnership
            SynchronizationFromWebService();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CancelRequest()
        {
            BarterStatus = NWDTradeStatus.Cancel;
            SaveData();

            // Sync NWDUserBarterRequest
            SynchronizationFromWebService(BarterRequestSuccessBlock, BarterRequestFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UserCanBuy()
        {
            bool rCanBuy = false;

            // Check Pack Cost
            foreach (KeyValuePair<NWDItem, int> pair in ItemsReceived.GetObjectAndQuantity())
            {
                // Get Item Cost data
                NWDItem tNWDItem = pair.Key;
                int tItemQte = pair.Value;

                rCanBuy = true;

                if (NWDUserOwnership.OwnershipForItemExists(tNWDItem))
                {
                    if (NWDUserOwnership.OwnershipForItem(tNWDItem).Quantity < tItemQte)
                    {
                        // User don't have enough item
                        rCanBuy = false;
                        break;
                    }
                }
                else
                {
                    // User don't have the selected item
                    rCanBuy = false;
                    break;
                }
            }

            return rCanBuy;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetNumberOfPropositions()
        {
            int rCpt = 0;

            if (PropositionsCounter > 0)
            {
                NWDUserBarterProposition[] tPropositions = NWDUserBarterProposition.FindDatas();
                foreach(NWDUserBarterProposition k in Propositions.GetObjectsAbsolute())
                {
                    foreach(NWDUserBarterProposition j in tPropositions)
                    {
                        if (k.Equals(j))
                        {
                            rCpt++;
                        }
                    }
                }
            }

            return rCpt;
        }
        //-------------------------------------------------------------------------------------------------------------
        void BarterRequestFailedBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (barterRequestBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                barterRequestBlockDelegate(true, NWDTradeStatus.None, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void BarterRequestSuccessBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            // Keep TradeStatus before Clean()
            NWDTradeStatus tBarterStatus = BarterStatus;

            // Do action with Items & Sync
            AddOrRemoveItems();

            if (barterRequestBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                barterRequestBlockDelegate(false, tBarterStatus, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Clean()
        {
            BarterPlace = null;
            ItemsProposed = null;
            LimitDayTime = null;
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================