//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++	
	/// <summary>
	/// NWD app environment manager.
	/// </summary>
	public partial class NWDEditorConfigurationManager
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
        [NWDAliasMethod(NWDConstants.M_DrawInEditor)]
        public static void DrawInEditor (EditorWindow sEditorWindow, bool sAutoSelect=false)
        {
            float tMinWidht = 270.0F;
            float tScrollMarge = 20.0f;
            int tColum = 1;
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
			// List environment
			//update the veriosn of Bundle
			//NWDVersion.UpdateVersionBundle ();
			// Draw interface for environment selected inn scrollview
			ScrollPosition = GUILayout.BeginScrollView (ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));

            EditorGUILayout.HelpBox("Tags", MessageType.None);

            EditorGUILayout.LabelField("Tag managment (all environements)", EditorStyles.boldLabel);
            if (tColum > 1)
            {
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));

            NWDAppConfiguration.SharedInstance().TagList[-1] = "No Tag";

            Dictionary<int, string> tTagList = new Dictionary<int, string>(NWDAppConfiguration.SharedInstance().TagList);
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumber; tI++)
            {
                if (NWDAppConfiguration.SharedInstance().TagList.ContainsKey(tI) == false)
                {
                    NWDAppConfiguration.SharedInstance().TagList.Add(tI, "tag " + tI.ToString());
                }
                EditorGUI.BeginDisabledGroup(tI < 0 || tI > NWDAppConfiguration.SharedInstance().TagNumberUser);
                string tV = EditorGUILayout.TextField("tag " + tI.ToString(), NWDAppConfiguration.SharedInstance().TagList[tI]);
                tTagList[tI] = tV.Replace("\"", "`");
                EditorGUI.EndDisabledGroup();
            }
            NWDAppConfiguration.SharedInstance().TagList = tTagList;
            EditorGUILayout.EndVertical();
            if (tColum > 1)
            {
                EditorGUILayout.EndHorizontal();
            }



            GUILayout.EndScrollView ();
			GUILayout.Space (8.0f);
			if (GUILayout.Button (NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON)) 
            {
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