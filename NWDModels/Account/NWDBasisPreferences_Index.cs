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
                            tAccountReference = NWDCompileType.Editor.ToString();
                        }
                        break;
                    case NWDCompileType.PlayMode:
                        {
                            tAccountReference = NWDCompileType.PlayMode.ToString() + NWDConstants.kFieldSeparatorA + sEnvironment.GetAccountReference();
                        }
                        break;
                    case NWDCompileType.Runtime:
                        {
                            tAccountReference = NWDCompileType.Runtime.ToString() + NWDConstants.kFieldSeparatorA + sEnvironment.GetAccountReference();
                        }
                        break;
                }
            }
            else
            {
                tAccountReference = "General";
            }
            string tKey = sEnvironment.Environment + NWDConstants.kFieldSeparatorA + sKey + NWDConstants.kFieldSeparatorA + tAccountReference;
            NWDDebug.Log("I need basis NWDBasisPreferences with reference " + tKey);
            NWDBasisPreferences rPref = NWDBasisHelper.GetRawDataByReference<NWDBasisPreferences>(tKey);
            if (rPref == null)
            {
                NWDDebug.Log("I need create basis preferences with reference " + tKey);
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
                rPref.InternalKey = (sKey + " " + tAccountReference).Replace(NWDConstants.kFieldSeparatorA, " ").Replace("  ", " ");
                rPref.InternalDescription = sEnvironment.Environment;
#if UNITY_EDITOR
                rPref.Tag = NWDBasisTag.TagAdminCreated;
#else
                rPref.Tag = NWDBasisTag.TagUserCreated;
#endif
                rPref.SaveData();
            }
            else
            {
                NWDDebug.Log("basis preferences with reference " + tKey + " exists ... I return it");
            }
            return rPref;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
