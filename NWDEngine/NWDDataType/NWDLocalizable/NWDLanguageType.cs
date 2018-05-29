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
    public class NWDLanguageType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
        public NWDLanguageType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
        public NWDLanguageType (string sValue = "")
		{
			if (sValue == null) {
				Value = "en";
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = "en";
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            float rReturn = NWDConstants.kFieldMarge+NWDConstants.kPopupdStyle.fixedHeight;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
        {
            NWDLanguageType tTemporary = new NWDLanguageType ();
            tTemporary.Value = Value;
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

            float tLangWidth = EditorGUIUtility.labelWidth + NWDConstants.kLangWidth;

			List<string> tLocalizationList = new List<string> ();
			tLocalizationList.Add ("-");

			string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
			string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tLanguageList = new List<string>(tLanguageArray);
            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tLanguageList)
                {
                    tContentFuturList.Add(new GUIContent(tS));
                }
            int tIndex = tLanguageList.IndexOf(tTemporary.Value);
            tIndex = EditorGUI.Popup(new Rect(tX, tY, tLangWidth, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);
            if (tIndex < 0 || tIndex >= tLanguageList.Count) {
					tIndex = 0;
				}
            tTemporary.Value = tLanguageList [tIndex];
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================