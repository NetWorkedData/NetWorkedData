//=====================================================================================================================
//
// ideMobi copyright 2017 
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
	/// NWD app configuration window.
	/// </summary>
	public class NWDAppEnvironmentChooser : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event.
		/// </summary>
		public void OnGUI ()
		{
			this.minSize = new Vector2 (300, 72);
			this.maxSize = new Vector2 (300, 72);
			// set title of window
			titleContent = new GUIContent (NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE);
			// show helpbox
			EditorGUILayout.HelpBox (NWDConstants.K_APP_CHOOSER_ENVIRONMENT, MessageType.None);

			// prepare tab to select
			int tTabSelected = -1;
			if (NWDAppConfiguration.SharedInstance.DevEnvironment.Selected == true) {
				tTabSelected = 0;
			}
			if (NWDAppConfiguration.SharedInstance.PreprodEnvironment.Selected == true) {
				tTabSelected = 1;
			}
			if (NWDAppConfiguration.SharedInstance.ProdEnvironment.Selected == true) {
				tTabSelected = 2;
			}
			string[] tTabList = new string[3] {
				NWDConstants.K_APP_CONFIGURATION_DEV,
				NWDConstants.K_APP_CONFIGURATION_PREPROD,
				NWDConstants.K_APP_CONFIGURATION_PROD
			};
			int tTabSelect = GUILayout.Toolbar (tTabSelected, tTabList);
			// check tab select changed
			if (tTabSelect != tTabSelected) {
				GUI.FocusControl (null);
				EditorPrefs.SetInt (NWDAppConfiguration.kEnvironmentSelectedKey, tTabSelect);
				switch (tTabSelect) {
				case 0:
					{
						NWDAppConfiguration.SharedInstance.DevEnvironment.Selected = true;
						NWDAppConfiguration.SharedInstance.PreprodEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance.ProdEnvironment.Selected = false;
					}
					break;
				case 1:
					{
						NWDAppConfiguration.SharedInstance.DevEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance.PreprodEnvironment.Selected = true;
						NWDAppConfiguration.SharedInstance.ProdEnvironment.Selected = false;
					}
					break;
				case 2:
					{
						NWDAppConfiguration.SharedInstance.DevEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance.PreprodEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance.ProdEnvironment.Selected = true;
					}
					break;
				}
				NWDVersion.UpdateVersionBundle ();
			}
			// Show version selected
            EditorGUILayout.LabelField ("Version bundle", PlayerSettings.bundleVersion, EditorStyles.label);
            string tAccountReference = NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference;
            NWDAccount tAccount = null;
            int tObjectIndex = NWDAccount.ObjectsByReferenceList.IndexOf(tAccountReference);
            if (NWDAccount.ObjectsList.Count > tObjectIndex && tObjectIndex >= 0)
            {
                tAccount = NWDAccount.ObjectsList[tObjectIndex] as NWDAccount;
            }
            if (tAccount != null)
            {
                EditorGUILayout.LabelField("Account Reference", tAccount.Reference);
                EditorGUILayout.LabelField("Account InternalKey", tAccount.InternalKey);
                if (GUILayout.Button("Account Select"))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
            }
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
