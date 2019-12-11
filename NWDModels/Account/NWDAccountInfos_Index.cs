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
        public static NWDAccountInfos CurrentData()
        {
            return FindFirstDataByAccount(NWDAccount.CurrentReference(), true);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================