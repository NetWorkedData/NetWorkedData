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
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string SynchronizeKeyLastTimestamp = "last";
        public const string SynchronizeKeyInWaitingTimestamp = "waiting";
        //-------------------------------------------------------------------------------------------------------------
        public string SynchronizationPrefsKey(NWDAppEnvironment sEnvironment)
        {
            return ClassPrefBaseKey + SynchronizeKeyLastTimestamp;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public void New_SynchronizationResetTimestamp(NWDAppEnvironment sEnvironment)
        //{
        //    NWDBasisPreferences.SetInt(New_SynchronizationPrefsKey(sEnvironment), sEnvironment, sEnvironment.BuildTimestamp, kAccountDependent);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public int SynchronizationGetLastTimestamp(NWDAppEnvironment sEnvironment)
        {
            bool tAccountDependent = false;
            if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            {
                tAccountDependent = true;
            }
            int rReturn = NWDBasisPreferences.GetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment, 0, tAccountDependent);
            // Modified by the version of bundle
            if (tAccountDependent == false)
            {
                if (rReturn < 0 == false)
                {
                    int tTimestampMin = NWDAppConfiguration.SharedInstance().SelectedEnvironment().BuildTimestamp;
                    if (tTimestampMin > rReturn)
                    {
                        rReturn = tTimestampMin;
                        // the reccord will be do by the webservice answer
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void New_SynchronizationUpadteTimestamp()
        //{
        //    Debug.Log(ClassNamePHP + " must be reset the timestamp of last sync to the build tiemstamp");
        //    New_SynchronizationResetTimestamp(NWDAppEnvironment.SelectedEnvironment());
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationSetNewTimestamp(NWDAppEnvironment sEnvironment, int sNewTimestamp)
        {
            //NWDBasisPreferences.SetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment, sNewTimestamp, kAccountDependent);
            NWDBasisPreferences.SetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment, sNewTimestamp, TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
