// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:57
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using SQLite.Attribute;
using UnityEngine;
using BasicToolBox;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeProposition : NWDBasis<NWDUserTradeProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeProposition()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeProposition(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTradeProposition CreateTradeProposalWith(NWDUserTradeRequest sRequest)
        {
            // Create a new Proposal
            NWDUserTradeProposition rProposition = FindEmptySlot();

            #if UNITY_EDITOR
            NWDTradePlace tTrade = sRequest.TradePlace.GetObject();
            rProposition.InternalKey = NWDUserNickname.GetNickname() + " - " + tTrade.InternalKey;
            #endif
            rProposition.Tag = NWDBasisTag.TagUserCreated;
            rProposition.TradePlace.SetObject(sRequest.TradePlace.GetObject());
            rProposition.TradeRequest.SetObject(sRequest);
            rProposition.ItemsProposed.SetReferenceAndQuantity(sRequest.ItemsProposed.GetReferenceAndQuantity());
            rProposition.ItemsAsked.SetReferenceAndQuantity(sRequest.ItemsAsked.GetReferenceAndQuantity());
            rProposition.TradeStatus = NWDTradeStatus.Submit;
            rProposition.TradeRequestHash = sRequest.TradeHash;
            rProposition.SaveData();

            return rProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncTradeProposal(NWDMessage sMessage = null)
        {
            // Keep Message for futur used
            Message = sMessage;

            // Sync NWDUserTradeProposal
            SynchronizationFromWebService(TradeProposalSuccessBlock, TradeProposalFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CancelProposal()
        {
            TradeStatus = NWDTradeStatus.Cancel;
            SaveData();

            // Sync NWDUserTradeProposal
            SynchronizationFromWebService(TradeProposalSuccessBlock, TradeProposalFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        void TradeProposalFailedBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (tradeProposalBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                tradeProposalBlockDelegate(true, NWDTradeStatus.None, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void TradeProposalSuccessBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            // Keep TradeStatus before Clean()
            NWDTradeStatus tTradeStatus = TradeStatus;

            // Notify the seller with an Inter Message
            if (Message != null)
            {
                NWDUserTradeRequest tTrade = TradeRequest.GetObjectAbsolute();
                if (tTrade != null)
                {
                    NWDUserInterMessage tMessage = NWDUserInterMessage.CreateNewMessageWith(Message, tTrade.Account.GetReference());
                    tMessage.SendMessage();
                }
            }

            // Do action with Items & Sync
            AddOrRemoveItems();

            // Notify Callback
            if (tradeProposalBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                tradeProposalBlockDelegate(false, tTradeStatus, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Clean()
        {
            TradePlace.Flush();
            TradeRequest.Flush();
            ItemsProposed.Flush();
            ItemsAsked.Flush();
            TradeRequestHash = string.Empty;
            TradeStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        void AddOrRemoveItems()
        {
            if (TradeStatus == NWDTradeStatus.Accepted)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                {
                    NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                }

                // Remove NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tAsked = ItemsAsked.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> pair in tAsked)
                {
                    NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                }

                // Set Trade Proposition to None, so we can reused an old slot for a new transaction
                Clean();

                // Sync NWDUserOwnership
                SynchronizationFromWebService();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static NWDUserTradeProposition FindEmptySlot()
        {
            NWDUserTradeProposition rSlot = null;

            // Search for a empty NWDUserTradeProposition Slot
            foreach (NWDUserTradeProposition k in FindDatas())
            {
                if (k.TradeStatus == NWDTradeStatus.None)
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