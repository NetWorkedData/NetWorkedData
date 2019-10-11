//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
//using BasicToolBox;
using UnityEditor;

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
        //public string New_ReferenceConnectionHeight(string sValue, bool sShowInspector)
        //{
        //    //NWEBenchmark.Start();
        //    float tWidth = EditorGUIUtility.currentViewWidth;
        //    GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
        //    tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
        //    float rReturn = tPopupdStyle.fixedHeight;
        //    NWDTypeClass tObject = New_GetDataByReference(sValue);
        //    if (tObject != null)
        //    {
        //        if (tObject.InternalDescription != string.Empty && tObject.InternalDescription != null)
        //        {
        //            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
        //            float tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth - 20);
        //            rReturn += tHelpBoxHeight + NWDGUI.kFieldMarge;
        //        }
        //        if (sShowInspector == true)
        //        {
        //            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
        //            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
        //            rReturn += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
        //            rReturn += tObject.DrawInspectorHeight(null, tWidth) + NWDGUI.kFieldMarge * 2;
        //        }
        //    }
        //    //NWEBenchmark.Finish();
        //    return rReturn.ToString();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public virtual string New_ReferenceConnectionField(Rect sPosition, string sEntitled, string sValue, string sToolsTips, bool sShowInspector, bool sEditionEnable, bool sEditButton, bool sNewButton)
        //{
        //    //NWEBenchmark.Start();
        //    NWDGUI.LoadStyles();
        //    float tX = sPosition.x;
        //    float tY = sPosition.y;
        //    float tWidth = sPosition.width;
        //    GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
        //    tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);
        //    GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
        //    tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);
        //    Type tType = ClassType;
        //    List<string> tReferenceList = new List<string>();
        //    List<string> tInternalNameList = new List<string>();
        //    tReferenceList.Add(NWDConstants.kFieldSeparatorA);
        //    tInternalNameList.Add(" ");

        //    string tValue = sValue;
        //    int tIndex = tReferenceList.IndexOf(tValue);
        //    var tPopupRect = new Rect(tX, tY, sPosition.width - NWDGUI.kEditWidth - NWDGUI.kFieldMarge, tPopupdStyle.fixedHeight);
        //    var tButtonRect = new Rect(tX + sPosition.width - NWDGUI.kEditWidth, tY, NWDGUI.kEditWidth, tMiniButtonStyle.fixedHeight);
        //    tY += tPopupdStyle.fixedHeight + NWDGUI.kFieldMarge;
        //    int rIndex = EditorGUI.Popup(tPopupRect, sEntitled, tIndex, tInternalNameList.ToArray(), EditorStyles.popup);
        //    bool tAutoChange = false;
        //    if (rIndex != tIndex)
        //    {
        //        string tNextValue = tReferenceList.ElementAt(rIndex);
        //        tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
        //        tValue = tNextValue;
        //        tAutoChange = true;
        //    }
        //    NWDTypeClass tObject = New_GetDataByReference(tValue);
        //    if (tAutoChange == true)
        //    {
        //        New_SetObjectInEdition(tObject);
        //    }
        //    if (tObject != null)
        //    {
        //        if (sEditButton == true)
        //        {
        //            if (GUI.Button(tButtonRect, NWDGUI.kEditContentIcon, NWDGUI.kEditButtonStyle))
        //            {
        //                New_SetObjectInEdition(tObject);
        //            }
        //        }
        //        float tHelpBoxHeight = 0;
        //        if (tObject.InternalDescription != string.Empty && tObject.InternalDescription != null)
        //        {
        //            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
        //            tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth);
        //            EditorGUI.HelpBox(new Rect(tX + NWDGUI.kConnectionIndent, tY, tWidth - NWDGUI.kConnectionIndent, tHelpBoxHeight), tObject.InternalDescription, MessageType.None);
        //            tY += tHelpBoxHeight + NWDGUI.kFieldMarge;
        //            tHelpBoxHeight += NWDGUI.kFieldMarge;
        //        }
        //        if (sShowInspector == true)
        //        {
        //            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
        //            tBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
        //            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

        //            Rect tRectToDrawProperties = new Rect(
        //                                             tX + NWDGUI.kConnectionIndent + tBorder,
        //                                             tY + tBoldLabelStyle.fixedHeight,
        //                                             sPosition.width - NWDGUI.kConnectionIndent - tBorder * 2,
        //                                             sPosition.height - tPopupdStyle.fixedHeight - tBoldLabelStyle.fixedHeight - NWDGUI.kFieldMarge - tBorder - tHelpBoxHeight);

        //            EditorGUI.DrawRect(tRectToDrawProperties, NWDGUI.kIdentityColor);
        //            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "Net Worked Data : " + ClassNamePHP, tBoldLabelStyle);
        //            tY += tBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
        //            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "<" + tObject.Reference + ">", tBoldLabelStyle);
        //            tY += tBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
        //            // draw properties in this rect
        //            Rect tRectToDawInspector = new Rect(
        //                                           tX + NWDGUI.kConnectionIndent + tBorder,
        //                                           tY,
        //                                           sPosition.width - NWDGUI.kConnectionIndent - tBorder,
        //                                           sPosition.height - tPopupdStyle.fixedHeight - tBorder);
        //           tObject.DrawEditor(tRectToDawInspector, false, null);
        //        }
        //    }
        //    else
        //    {
        //        if (sNewButton == true)
        //        {
        //            if (GUI.Button(tButtonRect, NWDGUI.kNewContentIcon, NWDGUI.kEditButtonStyle))
        //            {
        //                tObject = New_NewData();
        //                tObject.UpdateData(true);

        //                tValue = tObject.Reference;
        //                New_SetObjectInEdition(tObject); // PROVOQUE UN GROS BUG!!!
        //                NWDDataManager.SharedInstance().RepaintWindowsInManager(tObject.GetType());
        //            }
        //        }
        //    }
        //    //NWEBenchmark.Finish();
        //    return tValue;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual float ReferenceConnectionHeightSerialized(SerializedProperty sProperty, bool sShowInspector)
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ReferenceConnectionFieldSerialized(Rect sPosition, string sEntitled, SerializedProperty sProperty, string sToolsTips, bool sShowInspector)
        {
            //NWEBenchmark.Start();
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
                           tObject.DrawEditor(tRectToDawInspector,false, null);
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif