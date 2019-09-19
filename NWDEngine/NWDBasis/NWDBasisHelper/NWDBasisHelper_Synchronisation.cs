//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:8
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

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
            int rReturn = NWDBasisPreferences.GetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment, 0, kAccountDependent);
            // Modified by the version of bundle
            if (kAccountDependent == false)
            {
                int tTimestampMin = NWDAppConfiguration.SharedInstance().SelectedEnvironment().BuildTimestamp;
                if (tTimestampMin > rReturn)
                {
                    rReturn = tTimestampMin;
                    // the reccord will be do by the webservice answer
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
        public void  SynchronizationSetNewTimestamp(NWDAppEnvironment sEnvironment, int sNewTimestamp)
        {
            NWDBasisPreferences.SetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment, sNewTimestamp, kAccountDependent);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================