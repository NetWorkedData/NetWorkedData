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
    public partial class NWDAccountNickname : NWDBasis<NWDAccountNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDAccount, NWDAccountNickname> kAccountIndex = new NWDIndex<NWDAccount, NWDAccountNickname>();
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
        public static NWDAccountNickname FindFisrtByAccount(string sAccount, bool sOrCreate = true)
        {
            NWDAccountNickname rReturn = kAccountIndex.FindFirstByReference(sAccount);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.Account.SetReference(sAccount);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountNickname Current()
        {
            return FindFisrtByAccount(NWDAccount.CurrentReference(), true);
        }
            //-------------------------------------------------------------------------------------------------------------
        }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================