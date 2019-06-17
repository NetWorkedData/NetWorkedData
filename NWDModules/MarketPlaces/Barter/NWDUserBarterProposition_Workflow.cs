//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:6
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterProposition : NWDBasis<NWDUserBarterProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void barterProposalBlock(bool error, NWDTradeStatus status, NWDOperationResult result);
        public barterProposalBlock barterProposalBlockDelegate;
        public delegate void synchronizeBlock(bool error, NWDOperationResult result);
        public static synchronizeBlock synchronizeBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        private NWDMessage Message;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterProposition()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterProposition(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterProposition CreateBarterProposalWith(NWDUserBarterRequest sRequest, Dictionary<string, int> sProposed)
        {
            // Create a new Proposal
            NWDUserBarterProposition rProposition = FindEmptySlot();

            #if UNITY_EDITOR
            NWDBarterPlace tBarter = sRequest.BarterPlace.GetData();
            rProposition.InternalKey = NWDUserNickname.GetNickname() + " - " + tBarter.InternalKey;
            #endif
            rProposition.Tag = NWDBasisTag.TagUserCreated;
            rProposition.BarterPlace.SetData(sRequest.BarterPlace.GetData());
            rProposition.BarterRequest.SetData(sRequest);
            rProposition.ItemsSend.SetReferenceAndQuantity(sProposed);
            rProposition.BarterStatus = NWDTradeStatus.Submit;
            rProposition.BarterRequestHash = sRequest.BarterHash;
            rProposition.SaveData();
            
            return rProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterProposition CreateRefuseBarterProposalWith(NWDUserBarterRequest sRequest)
        {
            // Create a new Proposal
            NWDUserBarterProposition rProposition = FindEmptySlot();

            #if UNITY_EDITOR
            NWDBarterPlace tBarter = sRequest.BarterPlace.GetData();
            rProposition.InternalKey = NWDUserNickname.GetNickname() + " - " + tBarter.InternalKey;
            #endif
            rProposition.Tag = NWDBasisTag.TagUserCreated;
            rProposition.BarterPlace.SetData(sRequest.BarterPlace.GetData());
            rProposition.BarterRequest.SetData(sRequest);
            rProposition.BarterStatus = NWDTradeStatus.NoDeal;
            rProposition.BarterRequestHash = sRequest.BarterHash;
            rProposition.SaveData();

            return rProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetNumberOfProposalSentFor(NWDUserBarterRequest sRequest)
        {
            int rCpt = 0;
            foreach(NWDUserBarterProposition k in GetReachableDatas())
            {
                NWDUserBarterRequest tBarterRequest = k.BarterRequest.GetRawData();
                if (tBarterRequest != null && tBarterRequest.Equals(sRequest))
                {
                    rCpt++;
                }
            }

            return rCpt;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserBarterProposition> FindProposalWith(NWDBarterPlace sBarterPlace)
        {
            List<NWDUserBarterProposition> rUserBartersProposal = new List<NWDUserBarterProposition>();
            foreach (NWDUserBarterProposition k in GetReachableDatas())
            {
                NWDBarterPlace tPlace = k.BarterPlace.GetData();
                if (tPlace != null && tPlace.Equals(sBarterPlace))
                {
                    rUserBartersProposal.Add(k);
                }
            }


            return rUserBartersProposal;
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
            foreach (NWDUserBarterProposition k in GetReachableDatas())
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

            // Sync NWDUserBarterProposition
            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterProposal(NWDMessage sMessage = null)
        {
            // Keep Message for futur used
            Message = sMessage;

            // Sync NWDUserBarterProposal
            SynchronizationFromWebService(BarterProposalSuccessBlock, BarterProposalFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CancelProposal()
        {
            BarterStatus = NWDTradeStatus.Cancel;
            SaveData();

            SynchronizationFromWebService(BarterProposalSuccessBlock, BarterProposalFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        void BarterProposalFailedBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (barterProposalBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                barterProposalBlockDelegate(true, NWDTradeStatus.None, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void BarterProposalSuccessBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            // Keep TradeStatus before Clean()
            NWDTradeStatus tBarterStatus = BarterStatus;

            // Notify the seller with an Inter Message
            if (Message != null)
            {
                NWDUserBarterRequest tBarter = BarterRequest.GetRawData();
                if (tBarter != null)
                {
                    NWDUserInterMessage tMessage = NWDUserInterMessage.CreateNewMessageWith(Message, tBarter.Account.GetReference());
                    tMessage.SendMessage();
                }
            }

            // Do action with Items & Sync
            AddOrRemoveItems();

            if (barterProposalBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                barterProposalBlockDelegate(false, tBarterStatus, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Clean()
        {
            BarterPlace.Flush();
            BarterRequest.Flush();
            ItemsProposed.Flush();
            BarterRequestHash = string.Empty;
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        void AddOrRemoveItems()
        {
            if (BarterStatus == NWDTradeStatus.Accepted)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsProposed.GetReachableDatasAndQuantities();
                foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                {
                    NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                }

                // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                Clean();

                // Sync NWDUserOwnership
                SynchronizationFromWebService();
            }
            else if(BarterStatus == NWDTradeStatus.Expired)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsSend.GetReachableDatasAndQuantities();
                foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                {
                    NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                }

                // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                Clean();

                // Sync NWDUserOwnership
                SynchronizationFromWebService();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static NWDUserBarterProposition FindEmptySlot()
        {
            NWDUserBarterProposition rSlot = null;

            // Search for a empty NWDUserBarterProposal Slot
            foreach (NWDUserBarterProposition k in GetReachableDatas())
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