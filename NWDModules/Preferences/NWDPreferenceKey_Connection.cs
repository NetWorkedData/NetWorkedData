//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDPreferenceKeyConnection : NWDConnection<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void SetString(string sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType (sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetInt(int sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloat(float sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBool(bool sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString(string sNotExistValue = BTBConstants.K_EMPTY_STRING)
        {
            string rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetStringValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetInt(int sNotExistValue = 0)
        {
            int rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetIntValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloat(float sNotExistValue = 0.0F)
        {
            float rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetFloatValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetBoolValue();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ToogleBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
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