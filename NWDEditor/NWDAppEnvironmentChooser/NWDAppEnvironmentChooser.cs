//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
    /// NWDAppEnvironmentChooser. This class show window with form to choose the environment using in editor.
	/// </summary>
	public class NWDAppEnvironmentChooser : EditorWindow
	{
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SharedInstance to use from NWDEditorMenu.
        /// </summary>
        /// <returns>The instance.</returns>
        public static NWDAppEnvironmentChooser SharedInstance()
        {
            return NWDEditorMenu.kNWDAppEnvironmentChooser;
        }
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Draw window on GUI event.
		/// </summary>
		public void OnGUI ()
		{
            // set the size (min/max)
			this.minSize = new Vector2 (300, 150);
			this.maxSize = new Vector2 (300, 4096);
			// set title of window
			titleContent = new GUIContent (NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE);
			// show helpbox
			EditorGUILayout.HelpBox (NWDConstants.K_APP_CHOOSER_ENVIRONMENT, MessageType.None);

			// prepare tab to select
			int tTabSelected = -1;
			if (NWDAppConfiguration.SharedInstance().DevEnvironment.Selected == true) {
				tTabSelected = 0;
			}
			if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected == true) {
				tTabSelected = 1;
			}
			if (NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected == true) {
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
						NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = true;
						NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
					}
					break;
				case 1:
					{
						NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = true;
						NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
					}
					break;
				case 2:
					{
						NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
						NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = true;
					}
					break;
				}
				NWDVersion.UpdateVersionBundle ();
                if (NWDEditorMenu.kNWDAppEnvironmentSync != null)
                {
                    NWDEditorMenu.kNWDAppEnvironmentSync.Repaint();
                }
			}
			// Show version selected
            EditorGUILayout.LabelField (NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            NWDAccount tAccount = NWDAccount.CurrentAccount();
            if (tAccount != null)
            {
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                    {
                        NWDDatas.FindTypeInfos(tType).m_SearchAccount = tAccount.Reference;
                    }
                }
                string tGameSaveReference = NWDGameSave.CurrentForAccount(tAccount.Reference).Reference;
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_REFERENCE, tGameSaveReference);

                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_FILTER))
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                    {
                        NWDDatas.FindTypeInfos(tType).m_SearchGameSave = tAccount.Reference;
                    }
                }
            
            }
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
