//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeFinder : NWDBasis<NWDUserTradeFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeFinder()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeFinder(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDTradePlace), typeof(NWDUserTradeRequest), typeof(NWDUserTradeProposition), typeof(NWDUserTradeFinder) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTradeRequest[] FindPropositionsWith(NWDTradePlace sTradePlace)
        {
            NWDUserTradeFinder[] tUserTradesFinder = FindDatas();
            foreach (NWDUserTradeFinder k in tUserTradesFinder)
            {
                if (k.TradePlace.GetReference().Equals(sTradePlace.Reference))
                {
                    return k.TradeRequestsList.GetObjectsAbsolute();
                }
            }

            CreateTradeFinderWith(sTradePlace);

            return new NWDUserTradeRequest[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTradeFinder GetTradeFinderWith(NWDTradePlace sTradePlace)
        {
            NWDUserTradeFinder[] tUserTradesFinder = FindDatas();
            foreach (NWDUserTradeFinder k in tUserTradesFinder)
            {
                if (k.TradePlace.GetReference().Equals(sTradePlace.Reference))
                {
                    return k;
                }
            }

            return CreateTradeFinderWith(sTradePlace);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDUserTradeFinder CreateTradeFinderWith(NWDTradePlace sTradePlace)
        {
            // No NWD Finder Object found, we create one
            NWDUserTradeFinder rFinder = NewData();
#if UNITY_EDITOR
            rFinder.InternalKey = NWDUserNickname.GetNickname() + " - " + sTradePlace.InternalKey;
#endif
            rFinder.Tag = NWDBasisTag.TagUserCreated;
            rFinder.TradePlace.SetObject(sTradePlace);
            rFinder.SaveData();

            return rFinder;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncTradeFinder()
        {
            // Clean the Trade Place Finder Result
            CleanResult();

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (tradeFinderBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    tradeFinderBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (tradeFinderBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    tradeFinderBlockDelegate(true, tResult);
                }
            };

            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        void CleanResult()
        {
            TradeRequestsList.Flush();
            SaveData();

            // Remove stranger data request
            NWDUserTradeRequest.PurgeTable();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================