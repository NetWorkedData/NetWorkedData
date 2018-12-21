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
    //TODO: FINISH THIS CLASS NWDVector2Int
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDVector2Int : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector2Int()
        {
            Value = 0 + NWDConstants.kFieldSeparatorA + 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector2Int(string sValue = BTBConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = string.Empty;
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
        public void SetVectorInt(Vector2Int sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2Int GetVectorInt()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            if (tFloats.Count() == 2)
            {
                int.TryParse(tFloats[0], out tX);
                int.TryParse(tFloats[1], out tY);
            }
            Vector2Int rReturn = new Vector2Int(tX, tY);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0f);
            return tHeight*2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            NWDVector2Int tTemporary = new NWDVector2Int();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Vector2Int tVector = GetVectorInt();
            Vector2Int tNexVector = EditorGUI.Vector2IntField(new Rect(sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight),
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