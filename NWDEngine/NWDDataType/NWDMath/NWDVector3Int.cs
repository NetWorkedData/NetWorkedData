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
    //TODO: FINISH THIS CLASS NWDVector3Int
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDVector3Int : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector3Int()
        {
            Value = NWDToolbox.Vector3IntZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDVector3Int(string sValue = NWEConstants.K_EMPTY_STRING)
        //{
        //    if (sValue == null)
        //    {
        //        Value = string.Empty;
        //    }
        //    else
        //    {
        //        Value = sValue;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector3Int(Vector3Int sVector)
        {
            Value = NWDToolbox.Vector3IntToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.Vector3IntZero();
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
        public void SetVectorInt(Vector3Int sVector)
        {
            //Value = sVector.x + NWDConstants.kFieldSeparatorA +
            //sVector.y + NWDConstants.kFieldSeparatorA +
            //sVector.z;
            Value = NWDToolbox.Vector3IntToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3Int GetVectorInt()
        {
            //string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //int tX = 0;
            //int tY = 0;
            //int tZ = 0;
            //if (tFloats.Count() == 3)
            //{
            //    int.TryParse(tFloats[0], out tX);
            //    int.TryParse(tFloats[1], out tY);
            //    int.TryParse(tFloats[2], out tZ);
            //}
            //Vector3Int rReturn = new Vector3Int(tX, tY, tZ);
            //return rReturn;
            return NWDToolbox.Vector3IntFromString(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            return tHeight*2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDVector3Int tTemporary = new NWDVector3Int();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Vector3Int tVector = GetVectorInt();
            Vector3Int tNexVector = EditorGUI.Vector3IntField(new Rect(sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight),
                                   tContent,tVector);

            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.SetVectorInt(tNexVector);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
