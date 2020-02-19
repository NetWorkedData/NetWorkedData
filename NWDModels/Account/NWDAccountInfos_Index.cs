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
    public partial class NWDAccountInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDAccount, NWDAccountInfos> kAccountIndex = new NWDIndex<NWDAccount, NWDAccountInfos>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInAccountIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Account.GetReference();
                kAccountIndex.InsertData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromAccountIndex()
        {
            // Remove from the actual indexation
            kAccountIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos FindFirstDataByAccount(string sAccountReference, bool sOrCreate = true)
        {
            NWDAccountInfos rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                rReturn = kAccountIndex.RawFirstDataByKey(sAccountReference);
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDAccountInfos>();

#if UNITY_EDITOR
                    rReturn.InternalKey = sAccountReference;
#endif

                    rReturn.Account.SetReference(sAccountReference);
                    rReturn.Tag = NWDBasisTag.TagUserCreated;
                    rReturn.UpdateData();
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Used it to change and Temporary acount to a certified account.
        /// </summary>
        /// <param name="sOldAccount"></param>
        /// <param name="sNewAccount"></param>
        public static void ChangeCurrentData(string sOldAccount, string sNewAccount)
        {
            NWDAccountInfos tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(sOldAccount);
            if (tInfos != null)
            {
                tInfos.Reference = sNewAccount;
                tInfos.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos CurrentData()
        {
            //NWDAccountInfos tInfos = FindFirstDataByAccount(NWDAccount.CurrentReference(), true);
            NWDAccountInfos tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(NWDAccount.CurrentReference());
            if (tInfos == null)
            {
                tInfos = NWDBasisHelper.NewDataWithReference<NWDAccountInfos>(NWDAccount.CurrentReference());
                tInfos.SaveData();
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================