using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataLocalizationManager
	{
		public void ReOrderAllLocalizations ()
		{
			string tProgressBarTitle = "NetWorkedData Reorder localization";
			float tCountClass = NWDDataManager.SharedInstance.mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Prepare operation", tOperation/tCountClass);
			tOperation++;

			foreach( Type tType in NWDDataManager.SharedInstance.mTypeList)
			{
				EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reorder localization in  "+tType.Name+" objects", tOperation/tCountClass);
				tOperation++;
				var tMethodInfo = tType.GetMethod("ReOrderAllLocalizations", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
			}
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar();
		}

		public void ExportToCSV ()
		{
			Debug.Log ("ExportToCSV");
			// apply the pending modification : prevent lost modification
			NWDDataManager.SharedInstance.UpdateQueueExecute ();
			// ask for final file path
			string tPath = EditorUtility.SaveFilePanel(
				"Export Localization CSV",
				"",
				"NWDDataLocalization.csv",
				"csv");
			if (tPath != null) {
				// prepare header
				string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" +
				                 NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString.Replace (";", "\";\"") + "\"\n";
				// start to create file
				string tFile = tHeaders;
				// populate file by class result
				foreach (Type tType in NWDDataManager.SharedInstance.mTypeList) {
					var tMethodInfo = tType.GetMethod ("ExportAllLocalization", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
						string tResult = tMethodInfo.Invoke (null, null) as string;
						tFile += tResult;
					}
				}
				// write file
				File.WriteAllText (tPath, tFile);
			}
		}

		public void ImportFromCSV ()
		{
			Debug.Log ("ImportFromCSV");
			string tPath = EditorUtility.OpenFilePanel("Import Localization CSV", "", "csv");
			// more complexe 

			if (tPath != null) {
				string tLanguage = NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString;
				string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);

				string tFile = File.ReadAllText (tPath);
				string[] tFileRows = tFile.Split (new string[]{ "\n" }, StringSplitOptions.RemoveEmptyEntries);

				if (tFile != null) {
					foreach (Type tType in NWDDataManager.SharedInstance.mTypeList) {
						var tMethodInfo = tType.GetMethod ("ImportAllLocalizations", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (tMethodInfo != null) {
							string tResult = tMethodInfo.Invoke (null, new object[]{ tLanguageArray, tFileRows }) as string;
							tFile += tResult;
						}
					}
				}
				NWDDataManager.SharedInstance.UpdateQueueExecute ();
			}
		}
	}
}
//=====================================================================================================================
#endif