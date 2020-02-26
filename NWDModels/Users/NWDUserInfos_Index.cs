//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        private static protected NWDIndex<NWDGameSave, NWDUserInfos> kIndex = new NWDIndex<NWDGameSave, NWDUserInfos>();
        //-------------------------------------------------------------------------------------------------------------
        private static NWDUserInfos kCurrent = null;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInTipKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kIndex.UpdateData(this, this.GameSave.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromTipKeyIndex()
        {
            // Remove from the actual indexation
            kIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos FindFirstDataByAccount(string sAccountReference, bool sOrCreate = true)
        {
            if (kCurrent != null)
            {
                if (kCurrent.Account.GetReference() != sAccountReference)
                {
                    kCurrent = null;
                }
            }

            if (kCurrent == null)
            {
                NWDUserInfos tUserInfos = NWDBasisHelper.GetCorporateFirstData<NWDUserInfos>(NWDAccount.CurrentReference());
                //NWDUserInfos tUserInfos = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
                if (tUserInfos == null && sOrCreate)
                {
                    tUserInfos = NWDBasisHelper.NewData<NWDUserInfos>();

#if UNITY_EDITOR
                    tUserInfos.InternalKey = NWDAccount.CurrentReference();
#endif

                    tUserInfos.GameSave.SetData(NWDGameSave.CurrentData());
                    tUserInfos.Account.SetReference(NWDAccount.CurrentReference());
                    tUserInfos.Tag = NWDBasisTag.TagUserCreated;
                    tUserInfos.SaveData();
                }
                kCurrent = tUserInfos;
            }
            return kCurrent;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDUserInfos CurrentData()
        //{
        //    return FindFirstDataByAccount(NWDAccount.CurrentReference(), true);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos CurrentData(bool sOrCreate = true)
        {
            NWDUserInfos rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserInfos));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                rReturn = kIndex.FirstRawDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserInfos>();
                    rReturn.GameSave.SetData(NWDGameSave.CurrentData());
                    rReturn.UpdateData();
                }
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDUserInfos CurrentData()
        {
            string tGameSaveReference = NWDGameSave.CurrentData().Reference;
            NWDUserInfos tInfos = NWDBasisHelper.GetRawDataByReference<NWDUserInfos>(tGameSaveReference);
            if (tInfos == null)
            {
                tInfos = NWDBasisHelper.NewDataWithReference<NWDUserInfos>(tGameSaveReference);
                tInfos.SaveData();
            }
            return tInfos;
        }

        //-------------------------------------------------------------------------------------------------------------
        /*public static NWDUserInfos CurrentData()
        {
            if (kCurrent != null)
            {
                if (kCurrent.Account.GetReference() != NWDAccount.CurrentReference())
                {
                    kCurrent = null;
                }
            }

            if (kCurrent == null)
            {
                NWDUserInfos tUserInfos = NWDBasisHelper.GetCorporateFirstData<NWDUserInfos>(NWDAccount.CurrentReference());
                //NWDUserInfos tUserInfos = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
                if (tUserInfos == null)
                {
                    tUserInfos = NWDBasisHelper.NewData<NWDUserInfos>();
                    
                    #if UNITY_EDITOR
                    tUserInfos.InternalKey = NWDAccount.CurrentReference();
                    #endif

                    tUserInfos.GameSave.SetData(NWDGameSave.CurrentData());
                    tUserInfos.Account.SetReference(NWDAccount.CurrentReference());
                    tUserInfos.Tag = NWDBasisTag.TagUserCreated;
                    tUserInfos.SaveData();
                }
                kCurrent = tUserInfos;
            }

            return kCurrent;
        }*/
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================