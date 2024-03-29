//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
    public class NWDLanguageType : NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
        public NWDLanguageType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
        public NWDLanguageType (string sValue = NWEConstants.K_EMPTY_STRING)
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
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight ()
		{
            float rReturn = NWDGUI.kFieldMarge+NWDGUI.kPopupStyle.fixedHeight;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDLanguageType tTemporary = new NWDLanguageType ();
            tTemporary.Value = Value;
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

            float tLangWidth = EditorGUIUtility.labelWidth + NWDGUI.kLangWidth;

			List<string> tLocalizationList = new List<string> ();
			tLocalizationList.Add (NWEConstants.K_MINUS);

			string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
			string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tLanguageList = new List<string>(tLanguageArray);
            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tLanguageList)
                {
                    tContentFuturList.Add(new GUIContent(tS));
                }
            int tIndex = tLanguageList.IndexOf(tTemporary.Value);
            tIndex = EditorGUI.Popup(new Rect(tX, tY, tLangWidth, NWDGUI.kPopupStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDGUI.kPopupStyle);
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
