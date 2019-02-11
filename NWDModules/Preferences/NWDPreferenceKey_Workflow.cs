//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDPreferenceKey : NWDBasis<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PREFERENCE_CHANGED_KEY = "K_PREFERENCE_CHANGED_KEY_8zQr95er"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKey()
        {
            //Debug.Log("NWDStatKey Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDStatKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDPreferenceKey), typeof(NWDAccountPreference), typeof(NWDUserPreference) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(NWDMultiType sValue)
        {
            switch (Domain)
            {
                case NWDPreferencesDomain.AccountPreference:
                    {
                        NWDAccountPreference.PreferencesForKey(Reference).AddEnter(sValue);
                    }
                    break;
                case NWDPreferencesDomain.UserPreference:
                    {
                        NWDUserPreference.PreferencesForKey(Reference).AddEnter(sValue);
                    }
                    break;
                case NWDPreferencesDomain.LocalPreference:
                    {
                        PlayerPrefs.SetString(Reference, sValue.Value);
                    }
                    break;
            }
            if (NotifyChange == true)
            {
                BTBNotificationManager.SharedInstance().PostNotification(this, K_PREFERENCE_CHANGED_KEY);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType GetEnter()
        {
            NWDMultiType rReturn = new NWDMultiType();
            rReturn.Value = Default.Value;
            switch (Domain)
            {
                case NWDPreferencesDomain.AccountPreference:
                    {
                        rReturn = NWDAccountPreference.PreferencesForKey(Reference).GetEnter();
                    }
                    break;
                case NWDPreferencesDomain.UserPreference:
                    {
                        rReturn =  NWDUserPreference.PreferencesForKey(Reference).GetEnter();
                    }
                    break;
                case NWDPreferencesDomain.LocalPreference:
                    {
                        rReturn.Value = PlayerPrefs.GetString(Reference, Default.Value);
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================