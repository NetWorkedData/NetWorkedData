//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;

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