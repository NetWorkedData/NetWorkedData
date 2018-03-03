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
	//TODO: FINISH THIS CLASS NWDJsonType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDJsonType : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        const int kCONST_NUMBER_OF_LINE = 40;
		//-------------------------------------------------------------------------------------------------------------
		public NWDJsonType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDJsonType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public object Unlinearize ()
		{
			return null;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Linearize(List<object> sList)
		{
			Value = "???";
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Linearize(Dictionary<object,object> sDictionary)
		{
			Value = "???";
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            float rReturn = (NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDConstants.kFieldMarge) + NWDConstants.kTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDJsonType tTemporary = new NWDJsonType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), tContent);
            string tNextValue = EditorGUI.TextArea(new Rect(tX + EditorGUIUtility.labelWidth , tY, tWidth - EditorGUIUtility.labelWidth, NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), Value, NWDConstants.kTextAreaStyle);
            if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDConstants.kTextFieldStyle.fixedHeight*kCONST_NUMBER_OF_LINE + NWDConstants.kFieldMarge, tWidth - EditorGUIUtility.labelWidth, NWDConstants.kTextFieldStyle.fixedHeight), "Test If Valid (must be developped)"))
            {
                // test if valide
            }

            tTemporary.Value = tNextValue; 
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
//		public override bool ChangeAssetPath (string sOldPath, string sNewPath) {
//			return false;
//		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================