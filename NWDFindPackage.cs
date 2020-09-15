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
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Find package path class.
    /// Use the ScriptableObject to find the path of this package
    /// </summary>
    public class NWDFindPackage : ScriptableObject
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The script file path.
        /// </summary>
        public string ScriptFilePath;
        /// <summary>
        /// The script folder.
        /// </summary>
        public string ScriptFolder;
        /// <summary>
        /// The script folder from assets.
        /// </summary>
        public string ScriptFolderFromAssets;
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDFindPackage kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ascencor to shared instance.
        /// </summary>
        /// <returns>The shared instance.</returns>
        public static NWDFindPackage SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = ScriptableObject.CreateInstance("NWDFindPackage") as NWDFindPackage;
                kSharedInstance.ReadPaths();
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reads the paths.
        /// </summary>
        public void ReadPaths()
        {
            MonoScript tMonoScript = MonoScript.FromScriptableObject(this);
            ScriptFilePath = AssetDatabase.GetAssetPath(tMonoScript);
            FileInfo tFileInfo = new FileInfo(ScriptFilePath);
            ScriptFolder = tFileInfo.Directory.ToString();
            ScriptFolder = ScriptFolder.Replace("\\", "/");
            ScriptFolderFromAssets = NWD.K_Assets + ScriptFolder.Replace(Application.dataPath, string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Packages the path.
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="sAddPath">S add path.</param>
        public static string PathOfPackage(string sAddPath = NWEConstants.K_EMPTY_STRING)
        {
            return SharedInstance().ScriptFolderFromAssets + sAddPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PathEditorTextures(string sAddPath = NWEConstants.K_EMPTY_STRING)
        {
            return SharedInstance().ScriptFolderFromAssets + "/NWDEditor/Editor/Images/" + sAddPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PathEditor(string sAddPath = NWEConstants.K_EMPTY_STRING)
        {
            return SharedInstance().ScriptFolderFromAssets + "/NWDEditor/Editor/Images/" + sAddPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D PackageEditorTexture(string sAddPath = NWEConstants.K_EMPTY_STRING)
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(PathEditorTextures(sAddPath));
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D PackageEditor(string sAddPath = NWEConstants.K_EMPTY_STRING)
        {
            string tPro = string.Empty;
            if (EditorGUIUtility.isProSkin)
            {
                tPro = NWDConstants._pro;
            }
            string tPath = PathEditor(sAddPath + tPro + ".png");
            Texture2D rTexture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(tPath);
            if (rTexture2D == null)
            {
                tPath = PathEditor(sAddPath + tPro + ".psd");
                rTexture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(tPath);
            }
            //if (rTexture2D == null)
            //{
            //    Debug.LogWarning("erreur to find " + tPath);
            //}
            //else
            //{
            //    Debug.LogWarning("GOOD found " + tPath);
            //}
            return rTexture2D;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Texture EditorTexture(string sName)
        {
            Texture rTexture = PackageEditorTexture(sName + ".png");
            if (rTexture == null)
            {
                rTexture = PackageEditorTexture(sName + ".psd");
            }
            //if (rTexture == null)
            //{
            //    rTexture = PackageEditorTexture(sName + ".jpeg");
            //}
            if (rTexture == null)
            {
                string[] sGUIDs = AssetDatabase.FindAssets(sName + " t:texture");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(sName))
                    {
                        rTexture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    }
                }
            }
            return rTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
