//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:45
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