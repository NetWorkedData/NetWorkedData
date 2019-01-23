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
	//TODO: FINISH THIS CLASS NWDFloatRange
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
    public class NWDFloatRange : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDFloatRange ()
		{
            Value = 0.0F + NWDConstants.kFieldSeparatorA + 1.0F;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDFloatRange (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = "0.0F";
        }
		//-------------------------------------------------------------------------------------------------------------
        public void SetFloat (float sStart, float sEnd)
		{
            if (sStart<=sEnd)
            {
                Value = sStart.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA + sEnd.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
            }
            else
            {
                Value = sEnd.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA + sStart.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry); 
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetStart(float sStart)
        {
            SetFloat(sStart, GetFloats()[1]);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetEnd(float sEnd)
        {
            SetFloat(GetFloats()[0], sEnd);
        }
		//-------------------------------------------------------------------------------------------------------------
		public float[] GetFloats ()
		{
            string[] tFloats=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tStart =0.0F;
            float tEnd = 0.0F;
			if (tFloats.Count() == 2) {
                float.TryParse(tFloats [0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tStart);
                float.TryParse(tFloats [1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tEnd);
            }
            float[] rReturn = new float[] { tStart, tEnd };
			return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetStart()
        {
            return GetFloats()[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetEnd()
        {
            return GetFloats()[1];
        }
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
		{
            NWDFloatRange tTemporary = new NWDFloatRange ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tStart = 0.0F;
            float tEnd = 0.0F;
            if (tFloats.Count() == 2)
            {
                float.TryParse(tFloats[0], out tStart);
                float.TryParse(tFloats[1], out tEnd);
            }

			//float tX = sPos.x + EditorGUIUtility.labelWidth;
            EditorGUI.MinMaxSlider(new Rect(sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight),
                                    tContent,
                                   ref tStart,
                                   ref tEnd,
                                   0.0F,
                                   1.0F);

            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;
            tTemporary.SetFloat(tStart,tEnd);
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================