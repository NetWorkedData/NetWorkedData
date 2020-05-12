//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:21
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
    public partial class NWDAccountInfos : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        //        static protected NWDIndex<NWDAccount, NWDAccountInfos> kAccountIndex = new NWDIndex<NWDAccount, NWDAccountInfos>();
        //        //-------------------------------------------------------------------------------------------------------------
        //        [NWDIndexInMemory]
        //        public void InsertInAccountIndex()
        //        {
        //            // Re-add to the actual indexation ?
        //            if (IsUsable())
        //            {
        //                // Re-add !
        //                string tKey = Account.GetReference();
        //                kAccountIndex.UpdateData(this, tKey);
        //            }
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        [NWDDeindexInMemory]
        //        public void RemoveFromAccountIndex()
        //        {
        //            // Remove from the actual indexation
        //            kAccountIndex.RemoveData(this);
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public static NWDAccountInfos FindFirstDataByAccount(string sAccountReference, bool sOrCreate = true)
        //        {
        //            NWDAccountInfos rReturn = null;
        //            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos));
        //            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
        //            {
        //                rReturn = kAccountIndex.FirstRawDataByKey(sAccountReference);
        //                if (rReturn == null && sOrCreate == true)
        //                {
        //                    rReturn = NWDBasisHelper.NewData<NWDAccountInfos>();

        //#if UNITY_EDITOR
        //                    rReturn.InternalKey = sAccountReference;
        //#endif

        //                    rReturn.Account.SetReference(sAccountReference);
        //                    rReturn.Tag = NWDBasisTag.TagUserCreated;
        //                    rReturn.UpdateData();
        //                }
        //            }
        //            return rReturn;
        //        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Used it to change and Temporary acount to a certified account.
        /// </summary>
        /// <param name="sOldAccount"></param>
        /// <param name="sNewAccount"></param>
        //public static void ChangeCurrentData(string sOldAccount, string sNewAccount)
        //{
        //    // look for old data , delete on database, change reference, reccord on database
        //    string tOldUniqueReference = NWDAccount.GetUniqueReference(sOldAccount, typeof(NWDAccountInfos));
        //    Debug.Log(" I WILL DELETE THE REFERENCE " + tOldUniqueReference);
        //    string tNewUniqueReference = NWDAccount.GetUniqueReference(sNewAccount, typeof(NWDAccountInfos));
        //    Debug.Log(" I WILL REPLACE BY THE REFERENCE " + tNewUniqueReference);
        //    NWDAccountInfos tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(sOldAccount);
        //    if (tInfos != null)
        //    {
        //        tInfos.DeleteData();
        //        NWDDataManager.SharedInstance().DataQueueExecute();
        //        tInfos.Reference = tNewUniqueReference;
        //        tInfos.SaveData();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos CurrentData()
        {
            Debug.Log("<color=red> ###### I NEED THE CURRENT DATA </color>");
            NWDAccountInfos tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
            {
                string tUniqueReference = NWDAccount.GetUniqueReferenceFromCurrentAccount<NWDAccountInfos>();
                tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
                if (tInfos == null && string.IsNullOrEmpty(tUniqueReference) == false)
                {
                    tInfos = NWDBasisHelper.NewDataWithReference<NWDAccountInfos>(tUniqueReference);
                    tInfos.SaveData();
                }
                Debug.Log("<color=red> ###### I NEED THE CURRENT DATA  I RETURN " + tUniqueReference + "</color>");
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos CurrentDataForAccount( string sAccountReference)
        {
            NWDAccountInfos tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
            {
                string tUniqueReference = NWDAccount.GetUniqueReference(sAccountReference, typeof(NWDAccountInfos));
                tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================