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
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDBarterPlace), typeof(NWDUserBarterRequest), typeof(NWDUserBarterProposition), typeof(NWDUserBarterFinder) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterProposition CreateBarterProposalWith(NWDUserBarterRequest sRequest, Dictionary<string, int> sProposed)
        {
            // Create a new Proposal
            NWDUserBarterProposition rProposition = NewData();
            #if UNITY_EDITOR
            NWDBarterPlace tBarter = sRequest.BarterPlace.GetObject();
            rProposition.InternalKey = NWDAccountNickname.GetNickname() + " - " + tBarter.InternalKey;
            #endif
            rProposition.Tag = NWDBasisTag.TagUserCreated;
            rProposition.BarterPlace.SetObject(sRequest.BarterPlace.GetObject());
            rProposition.BarterRequest.SetObject(sRequest);
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
            NWDUserBarterProposition rProposition = NewData();
            #if UNITY_EDITOR
            NWDBarterPlace tBarter = sRequest.BarterPlace.GetObject();
            rProposition.InternalKey = NWDAccountNickname.GetNickname() + " - " + tBarter.InternalKey;
            #endif
            rProposition.Tag = NWDBasisTag.TagUserCreated;
            rProposition.BarterPlace.SetObject(sRequest.BarterPlace.GetObject());
            rProposition.BarterRequest.SetObject(sRequest);
            rProposition.BarterStatus = NWDTradeStatus.NoDeal;
            rProposition.BarterRequestHash = sRequest.BarterHash;
            rProposition.SaveData();


            return rProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetNumberOfProposalSentFor(NWDUserBarterRequest sRequest)
        {
            int rCpt = 0;
            foreach(NWDUserBarterProposition k in FindDatas())
            {
                NWDUserBarterRequest tBarterRequest = k.BarterRequest.GetObjectAbsolute();
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
            foreach (NWDUserBarterProposition k in FindDatas())
            {
                NWDBarterPlace tPlace = k.BarterPlace.GetObject();
                if (tPlace != null && tPlace.Equals(sBarterPlace))
                {
                    rUserBartersProposal.Add(k);
                }
            }

            return rUserBartersProposal;
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
                string tSellerReference = BarterRequest.GetObjectAbsolute().Account.GetReference();
                NWDUserInterMessage.SendMessage(Message, tSellerReference);
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
            BarterPlace = null;
            BarterRequest = null;
            ItemsProposed = null;
            BarterRequestHash = null;
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        void AddOrRemoveItems()
        {
            if (BarterStatus == NWDTradeStatus.Accepted)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
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
                Dictionary<NWDItem, int> tProposed = ItemsSend.GetObjectAndQuantity();
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================