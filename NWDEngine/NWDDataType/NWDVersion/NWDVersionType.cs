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
	public class NWDVersionType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDVersionType ()
		{
			Value = "0.00.00";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDVersionType (string sValue = "0.00.00")
		{
			if (sValue == null) {
				Value = "0.00.00";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            return NWDConstants.kPopupdStyle.fixedHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDVersionType tTemporary = new NWDVersionType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			float tWidth = sPosition.width - EditorGUIUtility.labelWidth;
			int tNumberOfSubDivision = 3;
            float tWidthSubPos = Mathf.Ceil (tWidth / tNumberOfSubDivision);
            float tWidthSub = Mathf.Ceil((tWidth -NWDConstants.kFieldMarge-NWDConstants.kFieldMarge) / tNumberOfSubDivision);
			int tMajorIndex = 0;
			int tMinorIndex = 0;
			int tBuildIndex = 0;
			if (Value != null) {
				string[] tValues = Value.Split (new char[]{ '.' }, StringSplitOptions.RemoveEmptyEntries);
				if (tValues.Length > 0) {
					tMajorIndex = Array.IndexOf (NWDConstants.K_VERSION_MAJOR_ARRAY, tValues [0]);
				}
				if (tValues.Length > 1) {
					tMinorIndex = Array.IndexOf (NWDConstants.K_VERSION_MINOR_ARRAY, tValues [1]);
				}
				if (tValues.Length > 2) {
					tBuildIndex = Array.IndexOf (NWDConstants.K_VERSION_BUILD_ARRAY, tValues [2]);
				}
			}
            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in NWDConstants.K_VERSION_MAJOR_ARRAY)
            {
                tContentFuturList.Add(new GUIContent(tS));
            }
            tMajorIndex = EditorGUI.Popup (new Rect (sPosition.x, sPosition.y, sPosition.width - tWidthSub * 2 - NWDConstants.kFieldMarge*2, sPosition.height),
                                           tContent, tMajorIndex, tContentFuturList.ToArray());
            
            tMinorIndex = EditorGUI.Popup (new Rect (sPosition.x+sPosition.width - tWidthSub *2 - NWDConstants.kFieldMarge, sPosition.y, tWidthSub, sPosition.height),
				tMinorIndex, NWDConstants.K_VERSION_MINOR_ARRAY);
            
            tBuildIndex = EditorGUI.Popup (new Rect (sPosition.x + sPosition.width - tWidthSub, sPosition.y, tWidthSub, sPosition.height),
				tBuildIndex, NWDConstants.K_VERSION_BUILD_ARRAY);
            
			tTemporary.Value = NWDConstants.K_VERSION_MAJOR_ARRAY [tMajorIndex] + "." + NWDConstants.K_VERSION_MINOR_ARRAY [tMinorIndex] + "." + NWDConstants.K_VERSION_BUILD_ARRAY [tBuildIndex];
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================