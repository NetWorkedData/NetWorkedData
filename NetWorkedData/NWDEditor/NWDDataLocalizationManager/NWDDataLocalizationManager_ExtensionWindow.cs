using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataLocalizationManager
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The scroll position.
		/// </summary>
		static Vector2 ScrollPosition;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Menu name.
		/// </summary>
		/// <returns>The name.</returns>
		public static string MenuName ()
		{
			return NWDConstants.K_APP_CONFIGURATION_MENU_NAME;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Draws in editor.
		/// </summary>
		public static void DrawInEditor ()
		{
			// Draw interface for language chooser
			Dictionary<string,string> tLanguageDico = NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguageDico;

			GUILayout.Label (NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA, EditorStyles.boldLabel);

			ScrollPosition = GUILayout.BeginScrollView (ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			GUILayout.BeginHorizontal ();
			List<string> tResult = new List<string> ();
			float tToggleWidth = 140.0f;
			int tColunm = 0;
			float tWidthUsed = EditorGUIUtility.currentViewWidth;
			int tColunmMax = Mathf.CeilToInt (tWidthUsed / tToggleWidth) - 1;
			if (tColunmMax < 1) {
				tColunmMax = 1;
			}
			foreach (KeyValuePair<string,string> tKeyValue in tLanguageDico) {
				if (tColunmMax <= tColunm) {
					tColunm = 0;
					GUILayout.EndHorizontal ();
					GUILayout.BeginHorizontal ();
				}
				tColunm++;
				bool tContains = NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString.Contains (tKeyValue.Value);
				tContains = EditorGUILayout.ToggleLeft (tKeyValue.Key, tContains, GUILayout.Width (tToggleWidth));
				if (tContains == true) {
					tResult.Add (tKeyValue.Value);
				}
			}
			GUILayout.EndHorizontal ();
			tResult.Sort ();
			string tNewLanguages = NWDDataLocalizationManager.kBaseDev + ";" + string.Join (";", tResult.ToArray ());
			if (NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString != tNewLanguages) {
				NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString = tNewLanguages;
				NWDDataInspector.ActiveRepaint ();
			}
			GUILayout.EndScrollView ();
			GUILayout.Space (8.0f);
			if (GUILayout.Button (NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON)) {
				NWDAppConfiguration.SharedInstance.GenerateCSharpFile (NWDAppConfiguration.SharedInstance.SelectedEnvironment ());
			}
			GUILayout.Space (8.0f);
			// show for debug
			//GUILayout.Label (NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString, EditorStyles.label);
		}
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
		#endif