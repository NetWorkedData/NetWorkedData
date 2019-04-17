// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:30
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        static public NWDBasisPreferences SelectDataForEngine(string sKey, NWDAppEnvironment sEnvironment, string sStringDefault, int sIntDefault, bool sLimitByAccount)
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
#if UNITY_EDITOR
            if (Application.isPlaying==false)
            {
                tAccountReference = "EDITOR";
            }
#endif
            string tKey = sEnvironment.Environment + NWDConstants.kFieldSeparatorA + sKey + NWDConstants.kFieldSeparatorA + tAccountReference;

            rPref = NWDBasisPreferences.GetRawDataByReference(tKey);
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
