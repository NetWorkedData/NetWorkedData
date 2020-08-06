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

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDColorType : NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDColorType ()
		{
            Value = NWDToolbox.ColorZero();
		}
		//-------------------------------------------------------------------------------------------------------------
        public NWDColorType (string sValue = null)
		{
			if (sValue == null) {
                Value = NWDToolbox.ColorZero();
            } else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDColorType(Color sColor)
        {
            Value = NWDToolbox.ColorToString(sColor);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.ColorZero();
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
        public Color GetColor ()
		{
            //Color tColor = new Color ();
            //ColorUtility.TryParseHtmlString (NWEConstants.K_HASHTAG + Value, out tColor);
            //return tColor;
            return NWDToolbox.ColorFromString(Value);
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SetColor (Color sColor)
		{
			//Value = ColorUtility.ToHtmlStringRGBA (sColor);
            Value = NWDToolbox.ColorToString(sColor);
        }
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tColorFieldStyle = new GUIStyle (EditorStyles.colorField);
			return tColorFieldStyle.CalcHeight (new GUIContent ("A"), 100);
		}
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
			NWDColorType tTemporary = new NWDColorType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.SetColor (EditorGUI.ColorField (sPosition, tContent, GetColor ()));
			return tTemporary;
		}
        //-------------------------------------------------------------------------------------------------------------
        #endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================