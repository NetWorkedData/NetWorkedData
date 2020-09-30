//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDPreferenceKey : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PREFERENCE_CHANGED_KEY = "K_PREFERENCE_CHANGED_KEY_8zQr95er"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKey()
        {   
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDPreferenceKey GetPrefKey(string sInternalKey, string sTitle, string sDescription, NWDPreferencesDomain sDomain, NWDMultiType sDefault, bool sNotifyChange)
        {
            string tReferenceKey = NWDBasisHelper.BasisHelper<NWDPreferenceKey>().ClassTrigramme + "-" + sInternalKey + "-999";
            NWDPreferenceKey rReturn = NWDBasisHelper.GetRawDataByReference<NWDPreferenceKey>(tReferenceKey);
            if (rReturn == null)
            {
                rReturn = NWDBasisHelper.NewDataWithReference<NWDPreferenceKey>(tReferenceKey);
                rReturn.InternalKey = sInternalKey;
                rReturn.Title.AddBaseString(sTitle);
                rReturn.Description.AddBaseString(sDescription);
                rReturn.Domain = sDomain;
                rReturn.Default = sDefault;
                rReturn.NotifyChange = sNotifyChange;
                rReturn.SaveData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(NWDMultiType sValue)
        {
            switch (Domain)
            {
                case NWDPreferencesDomain.AccountPreference:
                    {
                        NWDAccountPreference.FindDataByPreferenceKey(this).AddEnter(sValue);
                    }
                    break;
                case NWDPreferencesDomain.UserPreference:
                    {
                        NWDUserPreference.FindDataByPreferenceKey(this).AddEnter(sValue);
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
                NWENotificationManager.SharedInstance().PostNotification(this, K_PREFERENCE_CHANGED_KEY);
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
                        rReturn = NWDAccountPreference.FindDataByPreferenceKey(this).GetEnter();
                    }
                    break;
                case NWDPreferencesDomain.UserPreference:
                    {
                        rReturn =  NWDUserPreference.FindDataByPreferenceKey(this).GetEnter();
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
