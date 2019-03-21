//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisPreferences : NWDBasis<NWDBasisPreferences>
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
        static public NWDBasisPreferences GetBasisPreferences(string sKey, NWDAppEnvironment sEnvironment, string sStringDefault, int sIntDefault, bool sLimitByAccount)
        {
            NWDBasisPreferences rPref = null;
            string tAccountReference = string.Empty;
            if (sLimitByAccount)
            {
                tAccountReference = sEnvironment.PlayerAccountReference;
            }
            else
            {
                tAccountReference = "";
            }
            string tKey = sEnvironment.Environment + "_" + sKey + "_" + tAccountReference;
            rPref = NWDBasisPreferences.GetDataByReference(tKey);
            //if (rPref != null)
            //{
                //Debug.Log("Already exists : " + tKey);
            //}
            if (rPref == null)
            {
                rPref = NWDBasisPreferences.NewDataWithReference(tKey);
                //rPref.Account.SetValue(tAccountReference);
                if (string.IsNullOrEmpty(sStringDefault))
                {
                    rPref.StringValue = string.Empty;
                }
                else
                {
                    rPref.StringValue = sStringDefault;
                }
                rPref.IntValue = sIntDefault;
                rPref.SaveData();

                //if (BasisHelper().DatasByInternalKey.ContainsKey(tKey) == true)
                //{
                //    Debug.Log("Insert success : " + tKey);
                //}
                //else
                //{
                //    Debug.Log("Insert FAILED : " + tKey);
                //}
            }
            return rPref;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey, NWDAppEnvironment sEnvironment, string sDefault = null, bool sLimitByAccount = true)
        {
            NWDBasisPreferences tPrefs = GetBasisPreferences(sKey, sEnvironment, sDefault, 0, sLimitByAccount);
            if (tPrefs != null)
            {
                return tPrefs.StringValue;
            }
            else
            {
                return sDefault;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey, NWDAppEnvironment sEnvironment, int sDefault = 0, bool sLimitByAccount = true)
        {
            NWDBasisPreferences tPrefs = GetBasisPreferences(sKey, sEnvironment, string.Empty, sDefault, sLimitByAccount);
            if (tPrefs != null)
            {
                return tPrefs.IntValue;
            }
            else
            {
                return sDefault;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetString(string sKey, NWDAppEnvironment sEnvironment, string sValue, bool sLimitByAccount = true)
        {
            NWDBasisPreferences tPrefs = GetBasisPreferences(sKey, sEnvironment, string.Empty, 0, sLimitByAccount);
            if (tPrefs != null)
            {
                tPrefs.StringValue = sValue;
                tPrefs.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetInt(string sKey, NWDAppEnvironment sEnvironment, int sValue, bool sLimitByAccount = true)
        {
            NWDBasisPreferences tPrefs = GetBasisPreferences(sKey, sEnvironment, string.Empty, 0, sLimitByAccount);
            if (tPrefs != null)
            {
                tPrefs.IntValue = sValue;
                tPrefs.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
