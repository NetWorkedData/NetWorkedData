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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDJsonType : NWDReferenceMultiple
    {
        //-------------------------------------------------------------------------------------------------------------
        const int kCONST_NUMBER_OF_LINE = 40;
		//-------------------------------------------------------------------------------------------------------------
		public NWDJsonType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDJsonType (string sValue = NWEConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<object> UnlinearizeList()
        {
            return (List<object>)NWEMiniJSON.Json.Deserialize(NWDToolbox.JsonFromString(Value));
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> UnlinearizeDictionary()
        {
            return (Dictionary<string, object>)NWEMiniJSON.Json.Deserialize(NWDToolbox.JsonFromString(Value));
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
        public void LinearizeList(List<object> sList)
		{
			Value = NWDToolbox.JsonToString(NWEMiniJSON.Json.Serialize(sList));
		}
		//-------------------------------------------------------------------------------------------------------------
		public void LinearizeDictionary(Dictionary<string,object> sDictionary)
		{
            Value = NWDToolbox.JsonToString(NWEMiniJSON.Json.Serialize(sDictionary));
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
            NWDJsonType tTemporary = new NWDJsonType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), tContent);

            //remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            string tNextValue = EditorGUI.TextArea(new Rect(tX + EditorGUIUtility.labelWidth , tY, tWidth - EditorGUIUtility.labelWidth, NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), Value.Replace("{","\n{").Replace("}", "}\n"), NWDGUI.kTextAreaStyle);
            //if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.tTextFieldStyle.fixedHeight*kCONST_NUMBER_OF_LINE + NWDGUI.kFieldMarge, tWidth - EditorGUIUtility.labelWidth, NWDGUI.tTextFieldStyle.fixedHeight), "Test If Valid (must be developped)"))
            //{
            //    // test if valide
            //}
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.Value = tNextValue.Replace("\n{", "{").Replace("}\n", "}"); 
			return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnother)]
        //public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        //{
        //    string rReturn = "NO";
        //    if (Value != null)
        //    {
        //        if (Value.Contains(sOldReference))
        //        {
        //            Value = Value.Replace(sOldReference, sNewReference);
        //            rReturn = "YES";
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================