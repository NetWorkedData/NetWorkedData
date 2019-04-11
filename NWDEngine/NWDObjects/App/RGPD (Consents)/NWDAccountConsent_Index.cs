//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using BasicToolBox;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountConsent : NWDBasis<NWDAccountConsent>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDConsent, NWDAccountConsent> kAccountIndex = new NWDIndex<NWDConsent, NWDAccountConsent>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInAccountIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                if (Consent.GetObject() != null)
                {
                    string tKey = Consent.GetReference() + NWDConstants.kFieldSeparatorA + Consent.GetObject().Version.ToString() + NWDConstants.kFieldSeparatorA + Account.GetReference();
                    kAccountIndex.InsertData(this, tKey);
                }
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
        public static NWDAccountConsent FindDataByConsent(NWDConsent sConsent, bool sOrCreate = true)
        {
            string tKey = sConsent.Reference + NWDConstants.kFieldSeparatorA + sConsent.Version.ToString() + NWDConstants.kFieldSeparatorA + NWDAccount.CurrentReference();
            NWDAccountConsent rReturn = kAccountIndex.RawFirstDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                //rReturn.Account.SetReference(NWDAccount.CurrentReference());
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.Consent.SetReference(sConsent.Reference);
                rReturn.Version.SetValue(sConsent.Version.GetValue());
                rReturn.Authorization = BTBSwitchState.Unknow;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================