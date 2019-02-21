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
        public delegate void barterRequestBlock(bool error, NWDTradeStatus status, NWDOperationResult infos);
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
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDBarterPlace), typeof(NWDUserBarterRequest), typeof(NWDUserBarterProposition) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Request
            NWDUserBarterRequest tRequest = NewData();
            #if UNITY_EDITOR
            tRequest.InternalKey = NWDAccountNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
            #endif
            tRequest.Tag = NWDBasisTag.TagUserCreated;
            tRequest.BarterPlace.SetObject(sBarterPlace);
            tRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            tRequest.BarterStatus = NWDTradeStatus.Active;
            tRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            tRequest.SaveData();

            return tRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateFriendBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed, NWDUserRelationship sRelationship)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Request
            NWDUserBarterRequest tRequest = NewData();
            #if UNITY_EDITOR
            tRequest.InternalKey = NWDAccountNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
            #endif
            tRequest.Tag = NWDBasisTag.TagUserCreated;
            tRequest.BarterPlace.SetObject(sBarterPlace);
            tRequest.UserRelationship.SetObject(sRelationship);
            tRequest.ForRelationshipOnly = true;
            tRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            tRequest.BarterStatus = NWDTradeStatus.Active;
            tRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            tRequest.SaveData();

            return tRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest[] FindRequestsWith(NWDBarterPlace sBarterPlace)
        {
            List<NWDUserBarterRequest> tUserBartersRequest = new List<NWDUserBarterRequest>();
            foreach (NWDUserBarterRequest k in FindDatas())
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    tUserBartersRequest.Add(k);
                }
            }

            return tUserBartersRequest.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterRequest()
        {
            /*List<Type> tLists = new List<Type>() {
                typeof(NWDUserBarterProposition),
                typeof(NWDUserBarterRequest),
                typeof(NWDUserBarterFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                // Keep TradeStatus before Clean()
                NWDTradeStatus tBarterStatus = BarterStatus;

                // Do action with Items & Sync
                AddOrRemoveItems();
                
                if (barterRequestBlockDelegate != null)
                {
                    barterRequestBlockDelegate(false, tBarterStatus, null);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterRequestBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterRequestBlockDelegate(true, NWDTradeStatus.None, tInfos);
                }
            };*/

            // Sync NWDUserBarterRequest
            //NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
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
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserOwnership) });
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