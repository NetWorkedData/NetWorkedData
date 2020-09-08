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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEScriptOpener : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        static public void OpenScript(Type tType)
        {
            //Debug.Log("NWEScriptOpener OpenScript to edit " + tType.Name);
            string[] assetPaths = AssetDatabase.GetAllAssetPaths();
            string assetPathFinal = null;
            foreach (string assetPath in assetPaths)
            {
                if (assetPath.Contains(tType.Name)) // or .js if you want
                {
                    string tFileName = Path.GetFileName(assetPath);
                    //Debug.Log("filename = " + tFileName);
                    string tExtension = Path.GetExtension(tFileName);
                    //Debug.Log("filename extension = " + tExtension);
                    if (tExtension == ".cs")
                    {
                        if (tFileName == tType.Name + ".cs")
                        {
                            assetPathFinal = assetPath;
                            break;
                        }
                        else if (tFileName == tType.Name + "_Workflow.cs")
                        {
                            assetPathFinal = assetPath;
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(assetPathFinal)==false)
            {
                UnityEngine.Object tFile = AssetDatabase.LoadMainAssetAtPath(assetPathFinal);
                AssetDatabase.OpenAsset(tFile);
                EditorGUIUtility.PingObject(tFile);
                Selection.activeGameObject = tFile as GameObject;
            }
            //UnityEditorInternal.ScriptEditorUtility.
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
//=====================================================================================================================
#endif
