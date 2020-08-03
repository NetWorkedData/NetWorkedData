//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-09-9 18:24:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	MacroDefineEditor for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace MacroDefineEditor
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// Find package path class.
	/// Use the ScriptableObject to find the path of this package
	/// </summary>
	public class MDEFindPackage : ScriptableObject
	{
		//-------------------------------------------------------------------------------------------------------------
		// Insert static properties here
		/// <summary>
		/// The shared instance.
		/// </summary>
		private static MDEFindPackage kSharedInstance;
		//-------------------------------------------------------------------------------------------------------------
		// Insert instance properties here
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
		//-------------------------------------------------------------------------------------------------------------
		// Insert static methods here
		/// <summary>
		/// Ascensor to shared instance.
		/// </summary>
		/// <returns>The shared instance.</returns>
		public static MDEFindPackage SharedInstance ()
		{
			if (kSharedInstance == null) {
				kSharedInstance = ScriptableObject.CreateInstance ("MDEFindPackage") as MDEFindPackage;
				kSharedInstance.ReadPaths ();
			}
			return kSharedInstance; 
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Packages the path.
		/// </summary>
		/// <returns>The path.</returns>
		/// <param name="sAddPath">add path.</param>
		public static string PathOfPackage (string sAddPath = "")
		{
			return SharedInstance ().ScriptFolderFromAssets + sAddPath;
		}
		//-------------------------------------------------------------------------------------------------------------
		// Insert instance methods here
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
			ScriptFolderFromAssets = "Assets" + ScriptFolder.Replace (Application.dataPath, "");
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================