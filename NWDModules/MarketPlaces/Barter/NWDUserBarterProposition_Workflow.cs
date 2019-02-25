﻿//=====================================================================================================================
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
            NWDUserBarterProposition tProposition = NewData();
            #if UNITY_EDITOR
            NWDBarterPlace tBarter = sRequest.BarterPlace.GetObject();
            tProposition.InternalKey = NWDAccountNickname.GetNickname() + " - " + tBarter.InternalKey;
            #endif
            tProposition.Tag = NWDBasisTag.TagUserCreated;
            tProposition.BarterPlace.SetObject(sRequest.BarterPlace.GetObject());
            tProposition.BarterRequest.SetObject(sRequest);
            tProposition.ItemsSend.SetReferenceAndQuantity(sProposed);
            tProposition.BarterStatus = NWDTradeStatus.Active;
            tProposition.BarterRequestHash = sRequest.BarterHash;
            tProposition.SaveData();
            
            return tProposition;
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
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================