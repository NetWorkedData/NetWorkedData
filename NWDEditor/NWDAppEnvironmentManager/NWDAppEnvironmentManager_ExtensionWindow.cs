//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++	
	/// <summary>
	/// NWD app environment manager.
	/// </summary>
	public partial class NWDAppEnvironmentManager
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The scroll position.
		/// </summary>
		static Vector2 ScrollPosition;
		/// <summary>
		/// The tab selected.
		/// </summary>
		static int TabSelected = 0;
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
		public static void DrawInEditor (EditorWindow sEditorWindow, bool sAutoSelect=false)
		{
			// Draw warning if salt for class is false
			if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass () == false) {
				EditorGUILayout.HelpBox (NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
			}
			// Draw helpbox
			//EditorGUILayout.HelpBox (NWDConstants.K_APP_CONFIGURATION_HELPBOX, MessageType.None);
			GUILayout.Label (NWDConstants.K_APP_CONFIGURATION_ENVIRONMENT_AREA, EditorStyles.boldLabel);
			// List environment
			string[] tTabList = new string[3] {
				NWDConstants.K_APP_CONFIGURATION_DEV,
				NWDConstants.K_APP_CONFIGURATION_PREPROD,
				NWDConstants.K_APP_CONFIGURATION_PROD
			};
			// Draw interface for environment chooser
			int tTabSelect = GUILayout.Toolbar (TabSelected, tTabList);
			if (tTabSelect != TabSelected) {
				GUI.FocusControl (null);
				ScrollPosition = Vector2.zero;
				TabSelected = tTabSelect;
			}
			//update the veriosn of Bundle
			//NWDVersion.UpdateVersionBundle ();
			// Draw interface for environment selected inn scrollview
			ScrollPosition = GUILayout.BeginScrollView (ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			switch (tTabSelect) {
			case 0:
				{
					NWDAppConfiguration.SharedInstance().DevEnvironment.DrawInEditor ( sEditorWindow,  sAutoSelect);
				}
				break;
			case 1:
				{
					NWDAppConfiguration.SharedInstance().PreprodEnvironment.DrawInEditor (sEditorWindow,  sAutoSelect);
				}
				break;
			case 2:
				{
					NWDAppConfiguration.SharedInstance().ProdEnvironment.DrawInEditor (sEditorWindow,  sAutoSelect);
				}
				break;
			}
			GUILayout.EndScrollView ();

			GUILayout.Space (8.0f);
			if (GUILayout.Button (NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON)) {
				NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().SelectedEnvironment ());
			}
			GUILayout.Space (8.0f);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif