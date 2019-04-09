//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountInfos : NWDBasis<NWDAccountInfos>
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
                kAccountIndex.InsertInIndex(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromAccountIndex()
        {
            // Remove from the actual indexation
            kAccountIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos FindFisrtByAccount(string sAccount, bool sOrCreate = true)
        {
            NWDAccountInfos rReturn = kAccountIndex.FindFirstByReference(sAccount);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.Account.SetReference(sAccount);
                rReturn.AccountType = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerStatut;
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos Current()
        {
            return FindFisrtByAccount(NWDAccount.CurrentReference(), true);
        }
            //-------------------------------------------------------------------------------------------------------------
        }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================