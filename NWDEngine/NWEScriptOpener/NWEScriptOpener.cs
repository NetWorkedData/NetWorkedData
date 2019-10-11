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
                        AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath(assetPath));
                    }
                }
            }

            //UnityEditorInternal.ScriptEditorUtility.
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
//=====================================================================================================================
#endif