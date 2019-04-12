// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:12
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        private NWDMessage Message;
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
        public static NWDUserBarterRequest CreateBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Proposal
            NWDUserBarterRequest rRequest = FindEmptySlot();

            #if UNITY_EDITOR
            rRequest.InternalKey = NWDUserNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
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

            // Create a new request
            NWDUserBarterRequest rRequest = FindEmptySlot();

            #if UNITY_EDITOR
            rRequest.InternalKey = NWDUserNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
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
        public static List<NWDUserBarterRequest> FindRequestsReceivedWith(NWDBarterPlace sBarterPlace)
        {
            List<NWDUserBarterRequest> rUserBartersRequest = new List<NWDUserBarterRequest>();
            NWDUserRelationship[] tRelationships = NWDUserRelationship.FindDatas();
            foreach (NWDUserRelationship k in tRelationships)
            {
                if (k.RelationshipStatus == NWDRelationshipStatus.Valid)
                {
                    List<NWDUserBarterRequest> tFound = FindRequestsSentWith(sBarterPlace, k.FriendAccount.GetReference(), k.FriendGameSave.GetObject());
                    foreach (NWDUserBarterRequest j in tFound)
                    {
                        rUserBartersRequest.Add(j);
                    }
                }
            }

            return rUserBartersRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserBarterRequest> FindRequestsSentWith(NWDBarterPlace sBarterPlace, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            List<NWDUserBarterRequest> rUserBartersRequest = new List<NWDUserBarterRequest>();
            foreach (NWDUserBarterRequest k in FindDatas(sAccountReference, sGameSave))
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    rUserBartersRequest.Add(k);
                }
            }

            return rUserBartersRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetNumberOfRequestSentFor(NWDBarterPlace sBarterPlace, NWDUserRelationship sRelationship = null)
        {
            int rCpt = 0;

            if (sRelationship == null)
            {
                // Count all Request sent by user
                foreach (NWDUserBarterRequest k in FindDatas())
                {
                    NWDBarterPlace tPlace = k.BarterPlace.GetObject();
                    if (tPlace != null && tPlace.Equals(sBarterPlace))
                    {
                        rCpt++;
                    }
                }
            }
            else
            {
                // Count all Request sent by user for a given relationship
                foreach (NWDUserBarterRequest k in FindDatas())
                {
                    NWDBarterPlace tPlace = k.BarterPlace.GetObject();
                    if (tPlace != null && tPlace.Equals(sBarterPlace) &&
                        k.UserRelationship.GetObject().Equals(sRelationship))
                    {
                        rCpt++;
                    }
                }
            }

            return rCpt;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RefreshAndSynchronizeDatas()
        {
            RefreshDatas();
            SynchronizeDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RefreshDatas()
        {
            foreach (NWDUserBarterRequest k in FindDatas())
            {
                if (k.BarterStatus == NWDTradeStatus.Waiting)
                {
                    k.BarterStatus = NWDTradeStatus.Refresh;
                    k.SaveData();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(true, tResult);
                }
            };

            // Sync NWDUserBarterRequest
            SynchronizationFromWebService(tSuccess, tFailed);
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
        public void AcceptRequest(NWDMessage sMessage = null, NWDUserBarterProposition sProposition = null)
        {
            NWDUserBarterProposition tProposition = sProposition;
            if (tProposition == null)
            {
                NWDUserBarterProposition[] tPropositions = Propositions.GetObjectsAbsolute();
                if (tPropositions.Length == 1)
                {
                    tProposition = tPropositions[0];
                }
            }

            // Check if proposition in not null
            if (tProposition == null)
            {
                if (barterRequestBlockDelegate != null)
                {
                    barterRequestBlockDelegate(false, NWDTradeStatus.None, null);
                }
                return;
            }

            BarterStatus = NWDTradeStatus.Deal;
            WinnerProposition.SetObject(tProposition);
            SaveData();

            // Keep Message for futur used
            Message = sMessage;

            // Sync NWDUserBarterRequest
            SynchronizationFromWebService(BarterRequestSuccessBlock, BarterRequestFailedBlock);
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
        public void RefuseRequest(NWDMessage sMessage = null)
        {
            BarterStatus = NWDTradeStatus.NoDeal;
            SaveData();

            // Keep Message for futur used
            Message = sMessage;

            // Sync NWDUserBarterRequest
            SynchronizationFromWebService(BarterRequestSuccessBlock, BarterRequestFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /*public int GetNumberOfPropositions()
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
        }*/
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

            // Notify the seller with an Inter Message
            if (Message != null)
            {
                NWDUserBarterProposition tWinner = WinnerProposition.GetObjectAbsolute();
                if (tWinner != null)
                {
                    NWDUserInterMessage tMessage = NWDUserInterMessage.CreateNewMessageWith(Message, tWinner.Account.GetReference());
                    tMessage.SendMessage();
                }
            }


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
            BarterPlace.Flush();
            ItemsProposed.Flush();
            LimitDayTime.Flush();
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        static NWDUserBarterRequest FindEmptySlot()
        {
            NWDUserBarterRequest rSlot = null;

            // Search for a empty NWDUserBarterRequest Slot
            foreach (NWDUserBarterRequest k in FindDatas())
            {
                if (k.BarterStatus == NWDTradeStatus.None)
                {
                    rSlot = k;
                    break;
                }
            }

            // Create a new Proposal if null
            if (rSlot == null)
            {
                rSlot = NewData();
            }

            return rSlot;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================