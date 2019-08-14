//=====================================================================================================================
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
    public partial class NWDBasisPreferences : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        const string EDITOR = "EDITOR";
        const string PLAYING = "PLAYING";
        const string GENERAL = "GENERAL";
        //const string TOKEN = "TOKEN";
        //-------------------------------------------------------------------------------------------------------------
        static public NWDBasisPreferences SelectDataForEngine(string sKey, NWDAppEnvironment sEnvironment, string sStringDefault, int sIntDefault = 0, bool sLimitByAccount = true)
        {
            string tAccountReference = string.Empty;
            string tEnvironment = sEnvironment.Environment;
            if (sLimitByAccount)
            {
                tAccountReference = sEnvironment.PlayerAccountReference;
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                tAccountReference = EDITOR;
            }
#endif
            }
            else
            {
                tAccountReference = GENERAL;
            }
            //if (sKey == NWDAppEnvironment.kRequesTokenKey)
            //{
            //    tAccountReference = TOKEN;
            //}
            string tKey = tEnvironment + NWDConstants.kFieldSeparatorA + sKey + NWDConstants.kFieldSeparatorA + tAccountReference;
            // Debug.Log("NWDBasisPreferences SelectDataForEngine() tKey = " + tKey);
            NWDBasisPreferences rPref = NWDBasisHelper.GetRawDataByReference<NWDBasisPreferences>(tKey);
            if (rPref == null)
            {
                rPref = NWDBasisHelper.NewDataWithReference<NWDBasisPreferences>(tKey);
                rPref.DevSync = -1;
                rPref.PreprodSync = -1;
                rPref.ProdSync = -1;
                if (string.IsNullOrEmpty(sStringDefault))
                {
                    rPref.StringValue = string.Empty;
                }
                else
                {
                    rPref.StringValue = sStringDefault;
                }
                rPref.IntValue = sIntDefault;
#if UNITY_EDITOR
                if (tAccountReference.Equals(EDITOR))
                {
                    rPref.InternalKey = EDITOR + NWDConstants.kFieldSeparatorA + sKey;
                    rPref.Account = new NWDReferenceType<NWDAccount>();
                }
                else if (tAccountReference.Equals(GENERAL))
                {
                    rPref.InternalKey = GENERAL + NWDConstants.kFieldSeparatorA + sKey;
                    rPref.Account = new NWDReferenceType<NWDAccount>();
                }
                else
                {
                    rPref.InternalKey = PLAYING + NWDConstants.kFieldSeparatorA + sKey;
                }
                rPref.InternalDescription = tEnvironment;
                rPref.Tag = NWDBasisTag.TagAdminCreated;
                rPref.Environment = tEnvironment;
#else
                rPref.Tag = NWDBasisTag.TagUserCreated;
#endif
                rPref.SaveData();
            }
            return rPref;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
