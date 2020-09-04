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
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        const float tBorder = 1.0f;
        //-------------------------------------------------------------------------------------------------------------
        public bool kInspectorFoldout = false;
        //-------------------------------------------------------------------------------------------------------------
        public virtual float ReferenceConnectionHeightSerialized(SerializedProperty sProperty, bool sShowInspector)
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            float tWidth = EditorGUIUtility.currentViewWidth;
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
            float rReturn = NWDGUI.kDataSelectorFieldStyle.fixedHeight;
            NWDTypeClass tObject = GetDataByReference(sProperty.FindPropertyRelative("Reference").stringValue);
            if (tObject != null)
            {
                if (tObject.InternalDescription != string.Empty && tObject.InternalDescription != null)
                {
                    GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    float tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth - 20);
                    rReturn += tHelpBoxHeight + NWDGUI.kFieldMarge;
                }
                if (sShowInspector == true)
                {
                    // add foldout
                    rReturn += tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
                    if (kInspectorFoldout == true)
                    {
                        GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
                        tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
                        rReturn += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                        rReturn += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                        rReturn += tObject.DrawEditorTotalHeight(null, tWidth) + NWDGUI.kFieldMarge * 2;
                    }
                }
            }
            // check if value must be clean or not 
            string tValue = sProperty.FindPropertyRelative("Reference").stringValue;
            if (tValue != null && tValue != string.Empty)
            {
                if (GetDataByReference(tValue) == null)
                {
                    GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                    tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
                    rReturn = rReturn + tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ReferenceConnectionFieldSerialized(Rect sPosition, string sEntitled, SerializedProperty sProperty, string sToolsTips, bool sShowInspector)
        {
            //NWDBenchmark.Start();
            GUIContent tLabelContent = new GUIContent(sEntitled);
            // begin draw property with tLabel
            EditorGUI.BeginProperty(sPosition, tLabelContent, sProperty);
            {
                EditorGUI.BeginChangeCheck();
                string tValue = sProperty.FindPropertyRelative("Reference").stringValue;
                string tFuturValue = sProperty.FindPropertyRelative("Reference").stringValue;
                //Rect tPosition = EditorGUI.PrefixLabel(sPosition, GUIUtility.GetControlID(FocusType.Passive), tLabelContent);
                float tX = sPosition.x;
                float tY = sPosition.y;
                float tWidth = sPosition.width;

                bool tAutoChange = false;

                GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
                tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

                GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

                GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

                GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                tBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
                tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

                tFuturValue = NWDDatasSelector.Field(this, new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tLabelContent, tValue, false);
                tY += NWDGUI.kDataSelectorFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                NWDTypeClass tObject = GetDataByReference(tFuturValue);
                if (tValue != tFuturValue)
                {
                    tAutoChange = true;
                }
                if (tAutoChange == true)
                {
                    SetObjectInEdition(tObject, true, false);
                }
                if (tObject != null)
                {

                    float tHelpBoxHeight = 0;
                    if (tObject.InternalDescription != string.Empty && tObject.InternalDescription != null)
                    {
                        GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                        tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth);
                        EditorGUI.HelpBox(new Rect(tX + NWDGUI.kConnectionIndent, tY, tWidth - NWDGUI.kConnectionIndent, tHelpBoxHeight), tObject.InternalDescription, MessageType.None);
                        tY += tHelpBoxHeight + NWDGUI.kFieldMarge;
                        tHelpBoxHeight += NWDGUI.kFieldMarge;
                    }
                    if (sShowInspector == true)
                    {
                        kInspectorFoldout = EditorGUI.ToggleLeft(new Rect(tX + NWDGUI.kConnectionIndent, tY, tWidth - NWDGUI.kConnectionIndent, tPopupdStyle.fixedHeight), NWDConstants.K_APP_BASIS_INSPECTOR_FOLDOUT, kInspectorFoldout);
                        tY += tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
                        if (kInspectorFoldout == true)
                        {
                            Rect tRectToHelpBox = new Rect(
                                tX ,
                                tY,
                                sPosition.width,
                                sPosition.height - tPopupdStyle.fixedHeight - NWDGUI.kFieldMarge - tPopupdStyle.fixedHeight - NWDGUI.kFieldMarge - tHelpBoxHeight);


                            EditorGUI.HelpBox(tRectToHelpBox, string.Empty, MessageType.None);
                            //EditorGUI.DrawRect (tRectToDrawProperties, kIdentityColor);
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "Net Worked Data : " + ClassNamePHP, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "<" + tObject.Reference + ">", tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                            // draw properties in this rect
                            Rect tRectToDawInspector = new Rect(
                                                           tX + NWDGUI.kConnectionIndent + tBorder,
                                                           tY,
                                                           sPosition.width - NWDGUI.kConnectionIndent - tBorder,
                                                           sPosition.height - tPopupdStyle.fixedHeight - tBorder);
                           tObject.DrawEditor(tRectToDawInspector,false, null, CurrentWindow);
                        }
                    }
                }
                else
                {
                }
                if (EditorGUI.EndChangeCheck())
                {
                    sProperty.FindPropertyRelative("Reference").stringValue = tFuturValue;
                }
            }
            EditorGUI.EndProperty();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif