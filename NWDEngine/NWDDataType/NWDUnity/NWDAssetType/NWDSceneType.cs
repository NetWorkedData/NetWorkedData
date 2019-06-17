//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:52
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDSceneType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDSceneType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSceneType (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
         public bool IsEmpty()
        {
            return (Value == string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool ErrorAnalyze()
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
            tScenesInBuildList.Insert(0, string.Empty);
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (tScenesInBuildList.Contains(Value) == false)
                {
                    rReturn = true;
                }
            }
            InError = rReturn;
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
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0F);

            return tPopupFieldStyle.fixedHeight + tAdd * (NWDGUI.kPrefabSize + NWDGUI.kFieldMarge);
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
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
            tScenesInBuildList.Insert(0, string.Empty);
            tScenesInBuildList.Insert(1, " ");
            foreach (string tSettingSceneName in tScenesInBuildList)
            {
                tScenesInContentList.Add(new GUIContent(tSettingSceneName.Replace('/', '…')));
            }
            int tSceneIndex = tScenesInBuildList.IndexOf(Value);
            int tNextSceneIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth, tPopupFieldStyle.fixedHeight), tContent,tSceneIndex,tScenesInContentList.ToArray(), tPopupFieldStyle);
            {
                tNextSceneIndex = 0;
            }
            if (tNextSceneIndex<0 || tNextSceneIndex>=tScenesInBuildList.Count())
            tTemporary.Value = tScenesInBuildList[tNextSceneIndex];
            if (tTemporary.Value == " ")
            {
                tTemporary.Value = string.Empty;
            }
            tY = tY + NWDGUI.kFieldMarge + tMiniButtonStyle.fixedHeight;
            if (IsInError() == true) {
				tTemporary.Value = Value;
                NWDGUI.BeginRedArea();
                if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = string.Empty;
                }
                NWDGUI.EndRedArea();
                tY = tY + NWDGUI.kFieldMarge + tMiniButtonStyle.fixedHeight;
			}
			return tTemporary;
		}
        //-------------------------------------------------------------------------------------------------------------
        #endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================