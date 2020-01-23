//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:49
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

//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDCryptDataType : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        const int kCONST_NUMBER_OF_LINE = 40;
		//-------------------------------------------------------------------------------------------------------------
		public NWDCryptDataType()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDCryptDataType(string sValue = NWEConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = CryptValue(string.Empty);
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
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
        private string DecryptValue(string sValue)
        {
            return NWESecurityTools.DecryptAes(sValue, "ee","ee", NWESecurityAesTypeEnum.Aes128);
            //return NWESecurityTools.DecryptAes(sValue, "ee", "ee", NWESecurityAesTypeEnum.Aes256);
        }
        //-------------------------------------------------------------------------------------------------------------
        private string CryptValue(string sValue)
        {
            return NWESecurityTools.CryptAes(sValue, "ee", "ee", NWESecurityAesTypeEnum.Aes128);
            //return NWESecurityTools.CryptAes(sValue, "ee", "ee", NWESecurityAesTypeEnum.Aes256);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the value as string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string GetValue()
        {
            if (Value == null)
            {
                return string.Empty;
            }
            return DecryptValue(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the string as value.
        /// </summary>
        /// <param name="sString">S string.</param>
        public override void SetValue(string sString)
        {
            if (sString == null)
            {
                Value = CryptValue(string.Empty);
            }
            else
            {
                Value = CryptValue(sString);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight ()
		{
            float rReturn = (NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kFieldMarge) + NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
            NWDCryptDataType tTemporary = new NWDCryptDataType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            string tNextValue = EditorGUI.TextArea(new Rect(tX + EditorGUIUtility.labelWidth , tY, tWidth - EditorGUIUtility.labelWidth, NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), Value.Replace("{","\n{").Replace("}", "}\n"), NWDGUI.kTextAreaStyle);
            EditorGUI.indentLevel = tIndentLevel;
            tTemporary.Value = tNextValue.Replace("\n{", "{").Replace("}\n", "}"); 
			return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================