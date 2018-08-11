using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

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
		public static NWDFindPackage SharedInstance ()
		{
			if (kSharedInstance == null) {
				kSharedInstance = ScriptableObject.CreateInstance ("NWDFindPackage") as NWDFindPackage;
				kSharedInstance.ReadPaths ();
			}
			return kSharedInstance; 
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Reads the paths.
		/// </summary>
		public void ReadPaths ()
		{
			MonoScript tMonoScript = MonoScript.FromScriptableObject (this);
			ScriptFilePath = AssetDatabase.GetAssetPath (tMonoScript);
			FileInfo tFileInfo = new FileInfo (ScriptFilePath);
			ScriptFolder = tFileInfo.Directory.ToString ();
			ScriptFolder = ScriptFolder.Replace ("\\", "/");
			ScriptFolderFromAssets = "Assets"+ScriptFolder.Replace (Application.dataPath, "");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Packages the path.
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="sAddPath">S add path.</param>
		public static string PathOfPackage (string sAddPath="")
		{
			return SharedInstance ().ScriptFolderFromAssets + sAddPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PathEditorTextures(string sAddPath = "")
        {
            return SharedInstance().ScriptFolderFromAssets +"/NWDEditor/Editor/Resources/Textures/"+ sAddPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D PackageEditorTexture(string sAddPath = "")
        {
            return AssetDatabase.LoadAssetAtPath<Texture2D>(PathEditorTextures(sAddPath));

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
                string[] sGUIDs = AssetDatabase.FindAssets("" + sName + " t:texture");
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