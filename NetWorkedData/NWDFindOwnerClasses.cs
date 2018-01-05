using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// Find package path class.
	/// Use the ScriptableObject to find the path of this package
	/// </summary>
	public class NWDFindOwnerClasses : ScriptableObject
	{
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
		private static NWDFindOwnerClasses kSharedInstance;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Ascencor to shared instance.
		/// </summary>
		/// <returns>The shared instance.</returns>
		public static NWDFindOwnerClasses SharedInstance ()
		{
			if (kSharedInstance == null) {
				kSharedInstance = ScriptableObject.CreateInstance ("NWDFindOwnerClasses") as NWDFindOwnerClasses;
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
	}
}
//=====================================================================================================================
#endif