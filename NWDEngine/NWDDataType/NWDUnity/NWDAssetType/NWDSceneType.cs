//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDSceneType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDSceneType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSceneType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
         public bool IsEmpty()
        {
            return (Value == "");
        }
        //-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsInError()
        {
            List<string> tScenesInBuildList = new List<string>();
            EditorBuildSettingsScene[] tBuildScenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene tSettingScene in tBuildScenes)
            {
                if (tSettingScene.enabled)
                {
                    tScenesInBuildList.Add(tSettingScene.path);
                }
            }
            tScenesInBuildList.Insert(0, "");
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (tScenesInBuildList.Contains(Value) == false)
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight ()
		{
			int tAdd = 0;
            if (IsInError()) {
				tAdd = 1;
			}
            GUIStyle tPopupFieldStyle = new GUIStyle (EditorStyles.popup);
            tPopupFieldStyle.fixedHeight = tPopupFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);
            GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100.0F);

            return tPopupFieldStyle.fixedHeight + tAdd * (NWDConstants.kPrefabSize + NWDConstants.kFieldMarge);
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDSceneType tTemporary = new NWDSceneType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			tTemporary.Value = Value;

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

            GUIStyle tPopupFieldStyle = new GUIStyle (EditorStyles.popup);
            tPopupFieldStyle.fixedHeight = tPopupFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

            List<string> tScenesInBuildList = new List<string>();
            List<GUIContent> tScenesInContentList = new List<GUIContent>();
            EditorBuildSettingsScene[] tBuildScenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene tSettingScene in tBuildScenes)
            {
                if (tSettingScene.enabled)
                {
                    tScenesInBuildList.Add(tSettingScene.path);
                }
            }
            tScenesInBuildList.Sort((tA, tB) => tA.CompareTo(tB));
            tScenesInBuildList.Insert(0, "");
            tScenesInBuildList.Insert(1, " ");
            foreach (string tSettingSceneName in tScenesInBuildList)
            {
                tScenesInContentList.Add(new GUIContent(tSettingSceneName.Replace('/', '…')));
            }
            int tSceneIndex = tScenesInBuildList.IndexOf(Value);
            int tNextSceneIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth, tPopupFieldStyle.fixedHeight), tContent,tSceneIndex,tScenesInContentList.ToArray(), tPopupFieldStyle);
            tTemporary.Value = tScenesInBuildList[tNextSceneIndex];
            if (tTemporary.Value == " ")
            {
                tTemporary.Value = "";
            }
            if (IsInError() == true) {
				tTemporary.Value = Value;
				Color tOldColor = GUI.backgroundColor;
				GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
				if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = "";
				}
				GUI.backgroundColor = tOldColor;
				tY = tY + NWDConstants.kFieldMarge + tMiniButtonStyle.fixedHeight;
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================