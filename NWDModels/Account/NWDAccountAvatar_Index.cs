//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountAvatar : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDAccount, NWDAccountAvatar> kAccountIndex = new NWDIndex<NWDAccount, NWDAccountAvatar>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInAccountIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Account.GetReference();
                kAccountIndex.UpdateData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromAccountIndex()
        {
            // Remove from the actual indexation
            kAccountIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountAvatar FindFirstDataByAccount(string sAccountReference, bool sOrCreate = true)
        {
            NWDAccountAvatar rReturn = kAccountIndex.FirstRawDataByKey(sAccountReference);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDAccountAvatar>();
                rReturn.Account.SetReference(sAccountReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountAvatar CurrentData()
        {
            return FindFirstDataByAccount(NWDAccount.CurrentReference(), true);
        }
            //-------------------------------------------------------------------------------------------------------------
        }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================