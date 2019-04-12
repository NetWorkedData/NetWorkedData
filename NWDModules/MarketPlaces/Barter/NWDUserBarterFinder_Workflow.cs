// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:56
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

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterFinder : NWDBasis<NWDUserBarterFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void barterFinderBlock(bool result, NWDOperationResult infos);
        public barterFinderBlock barterFinderBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterFinder()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterFinder(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserBarterRequest> FindPropositionsWith(NWDBarterPlace sBarterPlace)
        {
            //TODO: use index
            NWDUserBarterFinder[] tUserBartersFinder = FindDatas();
            foreach (NWDUserBarterFinder k in tUserBartersFinder)
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    return k.BarterRequestsList.GetObjectsAbsoluteList();
                }
            }

            CreateBarterFinderWith(sBarterPlace);

            return new List<NWDUserBarterRequest>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterFinder GetBarterFinderWith(NWDBarterPlace sBarterPlace)
        {
            //TODO: use index
            NWDUserBarterFinder[] tUserBartersFinder = FindDatas();
            foreach (NWDUserBarterFinder k in tUserBartersFinder)
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    return k;
                }
            }

            return CreateBarterFinderWith(sBarterPlace);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDUserBarterFinder CreateBarterFinderWith(NWDBarterPlace sBarterPlace)
        {
            // No NWD Finder Object found, we create one
            NWDUserBarterFinder tFinder = NewData();
#if UNITY_EDITOR
            tFinder.InternalKey = NWDUserNickname.GetNickname();
#endif
            tFinder.Tag = NWDBasisTag.TagUserCreated;
            tFinder.BarterPlace.SetObject(sBarterPlace);
            tFinder.SaveData();

            return tFinder;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterFinder()
        {
            // Clean the Barter Place Finder Result
            CleanResult();

            List<Type> tLists = new List<Type>() {
                typeof(NWDUserBarterProposition),
                typeof(NWDUserBarterRequest),
                typeof(NWDUserBarterFinder),
                typeof(NWDBarterPlace),
            };
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterFinderBlockDelegate != null)
                {
                    barterFinderBlockDelegate(true, null);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterFinderBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterFinderBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CleanResult()
        {
            BarterRequestsList = null;
            SaveData();

            // Remove stranger data request
            NWDUserBarterRequest.BasisHelper().New_PurgeTable();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================