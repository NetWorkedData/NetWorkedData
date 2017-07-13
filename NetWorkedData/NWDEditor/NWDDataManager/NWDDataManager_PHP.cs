using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void CreatePHPAllClass ()
		{
			CreateAllPHP ();
			foreach (Type tType in mTypeList) {
				var tMethodInfo = tType.GetMethod ("CreateAllPHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, null);
				}
			}

//
//			foreach (Type tType in mTypeList) {
//				NWDBasis<> tTypeBasis = tType as NWDBasis<>;
//				tTypeBasis.CreateAllPHP ();
//			}

		}
		//-------------------------------------------------------------------------------------------------------------
		public void CreateAllPHP ()
		{
			CopyEnginePHP ();
			foreach (NWDAppEnvironment tEnvironement in  NWDAppConfiguration.SharedInstance.AllEnvironements()) {
				tEnvironement.CreatePHP ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ExportWebSites ()
		{
			string tPath = EditorUtility.SaveFolderPanel ("Export WebSite(s)", "", "NetWorkedDataServer");
			if (tPath != null) {
				if (Directory.Exists (tPath + "/NetWorkedDataServer") == false) {
					Directory.CreateDirectory (tPath + "/NetWorkedDataServer");
				}
				if (Directory.Exists (tPath + "/NetWorkedDataServer") == true) {
					NWDToolbox.ExportCopyFolderFiles ("Assets/NetWorkedDataServer", tPath + "/NetWorkedDataServer");
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void CopyEnginePHP ()
		{
			string tFolder = NWDFindPackage.SharedInstance ().ScriptFolderFromAssets + "/NWDServer";
			Debug.Log ("tFolder = " + tFolder);
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer") == false) {
				AssetDatabase.CreateFolder ("Assets", "NetWorkedDataServer");
			}
			NWDToolbox.CopyFolderFiles (tFolder, "Assets/NetWorkedDataServer");
		}
		//-------------------------------------------------------------------------------------------------------------
	
	}
}
//=====================================================================================================================
#endif