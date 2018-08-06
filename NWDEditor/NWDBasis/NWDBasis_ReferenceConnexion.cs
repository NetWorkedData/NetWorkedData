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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using System.IO;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string ReferenceConnectionHeight(string sValue, bool sShowInspector)
        {
            float tWidth = EditorGUIUtility.currentViewWidth;
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), tWidth);
            float rReturn = tPopupdStyle.fixedHeight;
            NWDBasis<K> tObject = NWDBasis<K>.GetDataByReference(sValue);
            //int tObjectIndex = Datas().ObjectsByReferenceList.IndexOf(sValue);
            //if (tObjectIndex >= 0 && tObjectIndex < Datas().ObjectsByReferenceList.Count)
            //{
            //    tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tObjectIndex);
            //}
            if (tObject != null)
            {
                if (tObject.InternalDescription != "" && tObject.InternalDescription != null)
                {
                    GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    float tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth - 20);
                    rReturn += tHelpBoxHeight + NWDConstants.kFieldMarge;
                }
                if (sShowInspector == true)
                {
                    GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
                    tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);
                    rReturn += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    rReturn += tObject.DrawObjectInspectorHeight() + NWDConstants.kFieldMarge * 2;
                }
            }
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        const float tButtonWidth = 40.0f;
        //-------------------------------------------------------------------------------------------------------------
        const float tButtonMarge = 3.0f;
        //-------------------------------------------------------------------------------------------------------------
        const float tMargeInspector = 20.0f;
        //-------------------------------------------------------------------------------------------------------------
        const float tBorder = 1.0f;
        //-------------------------------------------------------------------------------------------------------------
        public static string ReferenceConnectionField(Rect sPosition, string sEntitled, string sValue, string sToolsTips, bool sShowInspector, bool sEditionEnable, bool sEditButton, bool sNewButton)
        {
            float tX = sPosition.x;
            float tY = sPosition.y;
            float tWidth = sPosition.width;
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), 100);
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);
            Type tType = ClassType();
            List<string> tReferenceList = new List<string>();
            List<string> tInternalNameList = new List<string>();
            tReferenceList.Add(NWDConstants.kFieldSeparatorA);
            tInternalNameList.Add(" ");
            var tReferenceListInfo = tType.GetField("ObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tReferenceListInfo != null)
            {
                tReferenceList.AddRange(tReferenceListInfo.GetValue(null) as List<string>);
            }
            var tInternalNameListInfo = tType.GetField("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tInternalNameListInfo != null)
            {
                tInternalNameList.AddRange(tInternalNameListInfo.GetValue(null) as List<string>);
            }
            string tValue = sValue;
            int tIndex = tReferenceList.IndexOf(tValue);
            var tPopupRect = new Rect(tX, tY, sPosition.width - tButtonWidth - tButtonMarge, tPopupdStyle.fixedHeight);
            var tButtonRect = new Rect(tX + sPosition.width - tButtonWidth, tY, tButtonWidth, tMiniButtonStyle.fixedHeight);
            tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
            int rIndex = EditorGUI.Popup(tPopupRect, sEntitled, tIndex, tInternalNameList.ToArray(), EditorStyles.popup);
            bool tAutoChange = false;
            if (rIndex != tIndex)
            {
                string tNextValue = tReferenceList.ElementAt(rIndex);
                tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                tValue = tNextValue;
                tAutoChange = true;
            }
            NWDBasis<K> tObject = NWDBasis<K>.GetDataByReference(tValue);;
            //int tObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tValue);
            //if (tObjectIndex >= 0 && tObjectIndex < Datas().ObjectsByReferenceList.Count)
            //{
            //    tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tObjectIndex);
            //}
            if (tAutoChange == true)
            {
                SetObjectInEdition(tObject);
            }
            if (tObject != null)
            {
                if (sEditButton == true)
                {
                    if (GUI.Button(tButtonRect, NWDConstants.K_APP_CONNEXION_EDIT, EditorStyles.miniButton))
                    {
                        //if (Datas().ObjectsList.Count > tObjectIndex && tObjectIndex >= 0)
                        //{
                            SetObjectInEdition(tObject);
                        //}
                    }
                }
                float tHelpBoxHeight = 0;
                if (tObject.InternalDescription != "" && tObject.InternalDescription != null)
                {
                    GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth);
                    EditorGUI.HelpBox(new Rect(tX + tMargeInspector, tY, tWidth - tMargeInspector, tHelpBoxHeight), tObject.InternalDescription, MessageType.None);
                    tY += tHelpBoxHeight + NWDConstants.kFieldMarge;
                    tHelpBoxHeight += NWDConstants.kFieldMarge;
                }
                if (sShowInspector == true)
                {
                    GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                    tBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
                    tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent("A"), 100);
                    Rect tRectToDrawHeader = new Rect(
                                                 tX + tMargeInspector,
                                                 tY,
                                                 sPosition.width - tMargeInspector,
                                                 sPosition.height - tPopupdStyle.fixedHeight - NWDConstants.kFieldMarge - tHelpBoxHeight);

                    EditorGUI.DrawRect(tRectToDrawHeader, NWDConstants.kHeaderColorBackground);

                    Rect tRectToDrawProperties = new Rect(
                                                     tX + tMargeInspector + tBorder,
                                                     tY + tBoldLabelStyle.fixedHeight,
                                                     sPosition.width - tMargeInspector - tBorder * 2,
                                                     sPosition.height - tPopupdStyle.fixedHeight - tBoldLabelStyle.fixedHeight - NWDConstants.kFieldMarge - tBorder - tHelpBoxHeight);

                    EditorGUI.DrawRect(tRectToDrawProperties, NWDConstants.kIdentityColor);
                    GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "Net Worked Data : " + Datas().ClassNamePHP, tBoldLabelStyle);
                    tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "<" + tObject.Reference + ">", tBoldLabelStyle);
                    tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                    // draw properties in this rect
                    Rect tRectToDawInspector = new Rect(
                                                   tX + tMargeInspector + tBorder,
                                                   tY,
                                                   sPosition.width - tMargeInspector - tBorder,
                                                   sPosition.height - tPopupdStyle.fixedHeight - tBorder);
                    tObject.DrawObjectInspector(tRectToDawInspector, false, sEditionEnable);
                }
            }
            else
            {
                if (sNewButton == true)
                {
                    if (GUI.Button(tButtonRect, NWDConstants.K_APP_CONNEXION_NEW, EditorStyles.miniButton))
                    {
                        //						tObject = NewInstance ();
                        //						tObject.UpdateMe (true);
                        //						AddObjectInListOfEdition (tObject);
                        //
                        tObject = NWDBasis<K>.NewData();
                        tObject.UpdateData(true);

                        tValue = tObject.Reference;
                        SetObjectInEdition(tObject);
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tObject.GetType());
                    }
                }
            }
            return tValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float ReferenceConnectionHeightSerialized(SerializedProperty sProperty, bool sShowInspector)
        {
            float tWidth = EditorGUIUtility.currentViewWidth;
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), tWidth);
            float rReturn = tPopupdStyle.fixedHeight;
            NWDBasis<K> tObject = NWDBasis<K>.GetDataByReference(sProperty.FindPropertyRelative("Reference").stringValue);
            //int tObjectIndex = Datas().ObjectsByReferenceList.IndexOf(sProperty.FindPropertyRelative("Reference").stringValue);
            //if (tObjectIndex >= 0 && tObjectIndex < Datas().ObjectsByReferenceList.Count)
            //{
            //    tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tObjectIndex);
            //}
            if (tObject != null)
            {
                if (tObject.InternalDescription != "" && tObject.InternalDescription != null)
                {
                    GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                    float tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth - 20);
                    rReturn += tHelpBoxHeight + NWDConstants.kFieldMarge;
                }
                if (sShowInspector == true)
                {
                    // add foldout
                    rReturn += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    if (kInspectorFoldout == true)
                    {
                        GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
                        tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);
                        rReturn += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        rReturn += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        rReturn += tObject.DrawObjectInspectorHeight() + NWDConstants.kFieldMarge * 2;
                    }
                }
            }
            // check if value must be clean or not 
            string tValue = sProperty.FindPropertyRelative("Reference").stringValue;
            if (tValue != null && tValue != "")
            {
                if (NWDBasis<K>.GetDataByReference(tValue) == null)
                {
                    GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                    tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);
                    rReturn = rReturn + tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string ReferenceConnectionHeightSerializedString(SerializedProperty sProperty, bool sShowInspector)
        {
            return ReferenceConnectionHeightSerialized(sProperty, sShowInspector).ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool kInspectorFoldout = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// References the connection field serialized.
        /// </summary>
        /// <param name="sPosition">position.</param>
        /// <param name="sEntitled">entitled.</param>
        /// <param name="sProperty">property.</param>
        /// <param name="sToolsTips">tools tips.</param>
        /// <param name="sShowInspector">If set to <c>true</c> show inspector.</param>
        /// <param name="sEditionEnable">If set to <c>true</c> edition enable.</param>
        /// <param name="sEditButton">If set to <c>true</c> edit button.</param>
        /// <param name="sNewButton">If set to <c>true</c> new button.</param>
        public static void ReferenceConnectionFieldSerialized(Rect sPosition, string sEntitled, SerializedProperty sProperty, string sToolsTips, bool sShowInspector, bool sEditionEnable, bool sEditButton, bool sNewButton)
        {

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

                GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
                tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), 100);

                GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

                GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
                tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent("A"), 100);

                GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                tBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
                tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent("A"), 100);

                // test if connection is ok or ko
                bool tConnection = true;
                if (tValue != null && tValue != "")
                {
                    if (NWDBasis<K>.GetDataByReference(tValue) == null)
                    {
                        tConnection = false;
                    }
                }

                // editable? 
                EditorGUI.BeginDisabledGroup(!tConnection);

                // find class and invoke methods
                Type tType = ClassType();
                List<string> tReferenceList = new List<string>();
                List<string> tInternalNameList = new List<string>();

                tReferenceList.Add(NWDConstants.kFieldSeparatorA);
                tInternalNameList.Add(NWDConstants.kFieldNone);

                foreach (KeyValuePair<string, string> tKeyValue in NWDDatas.FindTypeInfos(typeof(K)).EditorDatasMenu.OrderBy(i => i.Value))
                {
                    tReferenceList.Add(tKeyValue.Key);
                    tInternalNameList.Add(tKeyValue.Value);
                }

                //tReferenceList.Add(NWDConstants.kFieldSeparatorA);
                //tInternalNameList.Add(" ");
                //var tReferenceListInfo = tType.GetField("ObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tReferenceListInfo != null)
                //{
                //    tReferenceList.AddRange(tReferenceListInfo.GetValue(null) as List<string>);
                //}
                //var tInternalNameListInfo = tType.GetField("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tInternalNameListInfo != null)
                //{
                //    tInternalNameList.AddRange(tInternalNameListInfo.GetValue(null) as List<string>);
                //}
                int tIndex = tReferenceList.IndexOf(tValue);

                var tPopupRect = new Rect(tX, tY, sPosition.width - tButtonWidth - tButtonMarge, tPopupdStyle.fixedHeight);
                var tButtonRect = new Rect(tX + sPosition.width - tButtonWidth, tY, tButtonWidth, tMiniButtonStyle.fixedHeight);
                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                int rIndex = EditorGUI.Popup(tPopupRect, sEntitled, tIndex, tInternalNameList.ToArray(), EditorStyles.popup);
                bool tAutoChange = false;
                if (rIndex != tIndex)
                {
                    string tNextValue = tReferenceList.ElementAt(rIndex);
                    tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                    tFuturValue = tNextValue;
                    tAutoChange = true;
                }
                NWDBasis<K> tObject = NWDBasis<K>.GetDataByReference(tFuturValue);
                //int tObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tFuturValue);
                //if (tObjectIndex >= 0 && tObjectIndex < Datas().ObjectsByReferenceList.Count)
                //{
                //    tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tObjectIndex);
                //}
                if (tAutoChange == true)
                {
                    SetObjectInEdition(tObject, true, false);
                }
                if (tObject != null)
                {
                    if (sEditButton == true)
                    {
                        if (GUI.Button(tButtonRect, NWDConstants.K_APP_CONNEXION_EDIT, EditorStyles.miniButton))
                        {
                            //if (Datas().ObjectsList.Count > tObjectIndex && tObjectIndex >= 0)
                            //{
                                SetObjectInEdition(tObject, true, true);
                            //}
                        }
                    }
                    float tHelpBoxHeight = 0;
                    if (tObject.InternalDescription != "" && tObject.InternalDescription != null)
                    {
                        GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
                        tHelpBoxHeight = tHelpBoxStyle.CalcHeight(new GUIContent(tObject.InternalDescription), tWidth);
                        EditorGUI.HelpBox(new Rect(tX + tMargeInspector, tY, tWidth - tMargeInspector, tHelpBoxHeight), tObject.InternalDescription, MessageType.None);
                        tY += tHelpBoxHeight + NWDConstants.kFieldMarge;
                        tHelpBoxHeight += NWDConstants.kFieldMarge;
                    }
                    if (sShowInspector == true)
                    {

                        // kInspectorFoldout = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tPopupdStyle.fixedHeight),kInspectorFoldout,NWDConstants.K_APP_BASIS_INSPECTOR_FOLDOUT);
                        kInspectorFoldout = EditorGUI.ToggleLeft(new Rect(tX + tMargeInspector, tY, tWidth - tMargeInspector, tPopupdStyle.fixedHeight), NWDConstants.K_APP_BASIS_INSPECTOR_FOLDOUT, kInspectorFoldout);
                        tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                        if (kInspectorFoldout == true)
                        {
                            Rect tRectToHelpBox = new Rect(
                                tX + NWDConstants.kConnectionIndent,
                                                         tY,
                                sPosition.width - NWDConstants.kConnectionIndent,
                                sPosition.height - tPopupdStyle.fixedHeight - NWDConstants.kFieldMarge - tPopupdStyle.fixedHeight - NWDConstants.kFieldMarge - tHelpBoxHeight);


                            EditorGUI.HelpBox(tRectToHelpBox, "", MessageType.None);
                            //EditorGUI.DrawRect (tRectToDrawProperties, kIdentityColor);
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "Net Worked Data : " + Datas().ClassNamePHP, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), "<" + tObject.Reference + ">", tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                            // draw properties in this rect
                            Rect tRectToDawInspector = new Rect(
                                                           tX + tMargeInspector + tBorder,
                                                           tY,
                                                           sPosition.width - tMargeInspector - tBorder,
                                                           sPosition.height - tPopupdStyle.fixedHeight - tBorder);
                            tObject.DrawObjectInspector(tRectToDawInspector, false, sEditionEnable);
                        }
                    }
                }
                else
                {
                    if (sNewButton == true)
                    {
                        if (GUI.Button(tButtonRect, NWDConstants.K_APP_CONNEXION_NEW, EditorStyles.miniButton))
                        {
                            //							tObject = NewInstance ();
                            //							tObject.UpdateMe (true);
                            //							AddObjectInListOfEdition (tObject);

                            tObject = NWDBasis<K>.NewData();
                            tObject.UpdateData(true);

                            tFuturValue = tObject.Reference;
                            SetObjectInEdition(tObject, true, true);
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(tObject.GetType());
                        }
                    }
                }
                if (EditorGUI.EndChangeCheck())
                {
                    sProperty.FindPropertyRelative("Reference").stringValue = tFuturValue;
                }









                EditorGUI.EndDisabledGroup();

                if (tConnection == false)
                {
                    //GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_ERROR, tLabelStyle);
                    //tY = tY + NWDConstants.kFieldMarge + tLabelStyle.fixedHeight;
                    ////              GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
                    ////              tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                    Color tOldColor = GUI.backgroundColor;
                    GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
                    if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle))
                    {
                        sProperty.FindPropertyRelative("Reference").stringValue = "";
                    }
                    GUI.backgroundColor = tOldColor;
                    tY = tY + NWDConstants.kFieldMarge + tMiniButtonStyle.fixedHeight;
                }



            }
            EditorGUI.EndProperty();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
 #endif