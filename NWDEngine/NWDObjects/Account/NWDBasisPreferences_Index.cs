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
        static public NWDBasisPreferences FindData(string sKey, NWDAppEnvironment sEnvironment, string sStringDefault, int sIntDefault, bool sLimitByAccount)
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
            if (rPref == null)
            {
                rPref = NWDBasisPreferences.NewDataWithReference(tKey);
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
            }
            return rPref;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
