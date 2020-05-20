//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:31
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
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //public partial class NWDBasisPreferences : NWDBasisAccountUnsynchronize
    public partial class NWDBasisPreferences : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisPreferences()
        {
            //Debug.Log("NWDRequestToken Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisPreferences(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDRequestToken Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey, NWDAppEnvironment sEnvironment, string sDefault = null, bool sLimitByAccount = true)
        {
            //NWEBenchmark.Start();
            NWDBasisPreferences tPrefs = SelectDataForEngine(sKey, sEnvironment, sDefault, 0, sLimitByAccount);
            string rReturn;
            if (tPrefs != null)
            {
                rReturn = tPrefs.StringValue;
            }
            else
            {
                rReturn = sDefault;
                //Debug.Log("NWDBasisPreferences GetString for key <b>" + sKey + "</b> Pref NOT FOUND!");
            }
            //Debug.Log ("NWDBasisPreferences GetString for <b>" + sKey + "</b> Value is '<b>" + (string.IsNullOrEmpty(rReturn) ? "NULL" : rReturn) + "</b>'.");
            //NWEBenchmark.Finish(true, "for key <b>"+ sKey + "</b> Value is '<b>" + (string.IsNullOrEmpty(rReturn)? "NULL":rReturn)+"</b>'.");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey, NWDAppEnvironment sEnvironment, int sDefault = 0, bool sLimitByAccount = true)
        {
            //NWEBenchmark.Start();
            NWDBasisPreferences tPrefs = SelectDataForEngine(sKey, sEnvironment, string.Empty, sDefault, sLimitByAccount);
            int rReturn = sDefault;
            if (tPrefs != null)
            {
                rReturn = tPrefs.IntValue;
            }
            else
            {
                rReturn = sDefault;
                //Debug.Log("NWDBasisPreferences GetInt for key <b>" + sKey + "</b> Pref NOT FOUND!");
            }
            //Debug.Log("NWDBasisPreferences GetString for <b>" + sKey + "</b> Value is '<b>" + rReturn + "</b>'.");
            //NWEBenchmark.Finish(true, "for key <b>" + sKey + "</b> Value is '<b>" + rReturn + "</b>'.");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetString(string sKey, NWDAppEnvironment sEnvironment, string sValue, bool sLimitByAccount = true)
        {
            //NWEBenchmark.Start();
            NWDBasisPreferences tPrefs = SelectDataForEngine(sKey, sEnvironment, sValue, 0, sLimitByAccount);
            if (tPrefs != null)
            {
                tPrefs.StringValue = sValue;
                tPrefs.SaveData();
                //Debug.Log("NWDBasisPreferences SetString for <b>" + sKey + "</b> Value is '<b>" + sValue + "</b>'.");
            }
            else
            {
                //Debug.Log("NWDBasisPreferences SetString for key <b>" + sKey + "</b> Pref NOT FOUND!");
            }
            //NWEBenchmark.Finish(true, "for key <b>" + sKey + "</b> set Value '<b>" + sValue + "</b>'.");
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetInt(string sKey, NWDAppEnvironment sEnvironment, int sValue, bool sLimitByAccount = true)
        {
            //NWEBenchmark.Start();
            NWDBasisPreferences tPrefs = SelectDataForEngine(sKey, sEnvironment, string.Empty, sValue, sLimitByAccount);
            if (tPrefs != null)
            {
                tPrefs.IntValue = sValue;
                tPrefs.SaveData();
                //Debug.Log("NWDBasisPreferences SetInt for <b>" + sKey + "</b> Value is '<b>" + sValue + "</b>'.");
            }
            else
            {
                //Debug.Log("NWDBasisPreferences SetInt for key <b>" + sKey + "</b> Pref NOT FOUND!");
            }
            //NWEBenchmark.Finish(true, "for key <b>" + sKey + "</b> set Value is '<b>" + sValue + "</b>'.");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
