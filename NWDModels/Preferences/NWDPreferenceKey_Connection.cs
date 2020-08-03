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
#endif
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDPreferenceKeyConnection : NWDConnection<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void SetValue(string sValue)
        {
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetValue(int sValue)
        {
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetValue(float sValue)
        {
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetValue(bool sValue)
        {
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetStringValue(string sNotExistValue = NWEConstants.K_EMPTY_STRING)
        {
            string rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetStringValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetIntValue(int sNotExistValue = 0)
        {
            int rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetIntValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloatValue(float sNotExistValue = 0.0F)
        {
            float rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetFloatValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBoolValue(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetBoolValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ToggleBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetReachableData();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetBoolValue();
                rReturn = !rReturn;
                tPref.AddEnter(new NWDMultiType(rReturn));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================