//=====================================================================================================================
//
//  ideMobi 2020©
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
    public partial class NWDBasisPreferences : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDBasisPreferences SelectDataForEngine(string sKey, NWDAppEnvironment sEnvironment, string sStringDefault, int sIntDefault = 0, bool sLimitByAccount = true)
        {
            string tAccountReference = string.Empty;
            if (sLimitByAccount)
            {
                switch (NWDLauncher.CompileAs())
                {
                    case NWDCompileType.Editor:
                        {
                            //tAccountReference = NWDCompileType.Editor.ToString() + NWDConstants.kFieldSeparatorA + sEnvironment.PlayerAccountReference;
                            tAccountReference = NWDCompileType.Editor.ToString();
                        }
                        break;
                    case NWDCompileType.PlayMode:
                        {
                            tAccountReference = NWDCompileType.PlayMode.ToString() + NWDConstants.kFieldSeparatorA + sEnvironment.PlayerAccountReference;
                        }
                        break;
                    case NWDCompileType.Runtime:
                        {
                            tAccountReference = NWDCompileType.Runtime.ToString() + NWDConstants.kFieldSeparatorA + sEnvironment.PlayerAccountReference;
                        }
                        break;
                }
            }
            else
            {
                tAccountReference = "General";
            }
            string tKey = sEnvironment.Environment + NWDConstants.kFieldSeparatorA + sKey + NWDConstants.kFieldSeparatorA + tAccountReference;
            Debug.Log("I need basis NWDBasisPreferences with reference " + tKey);
            NWDBasisPreferences rPref = NWDBasisHelper.GetRawDataByReference<NWDBasisPreferences>(tKey);
            if (rPref == null)
            {
                Debug.Log("I need create basis preferences with reference " + tKey);
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
                rPref.InternalKey = (sKey + " " + tAccountReference).Replace(NWDConstants.kFieldSeparatorA, " ").Replace("  ", " ");
                rPref.InternalDescription = sEnvironment.Environment;
                rPref.Tag = NWDBasisTag.TagAdminCreated;
#else
                rPref.Tag = NWDBasisTag.TagUserCreated;
#endif
                rPref.SaveData();
            }
            else
            {
                Debug.Log("basis preferences with reference " + tKey + " exists ... I return it");
            }
            return rPref;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
