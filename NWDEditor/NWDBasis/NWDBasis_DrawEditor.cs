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
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        protected Texture2D PreviewTexture;
        protected bool PreviewTextureIsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectedFirstObjectInTable(EditorWindow sEditorWindow)
        {
            if (Datas().EditorTableDatas.Count > 0)
            {
                K sObject = Datas().EditorTableDatas.ElementAt(0) as K;
                //int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                SetObjectInEdition(sObject);
                sEditorWindow.Focus();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_LAST_TYPE_KEY = "K_EDITOR_LAST_TYPE_KEY_5fdshjktr";
        public const string K_EDITOR_LAST_REFERENCE_KEY = "K_EDITOR_LAST_REFERENCE_KEY_ed5f5dtr";
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasis<K> RestaureObjectInEdition()
        {
            string tTypeEdited = EditorPrefs.GetString(K_EDITOR_LAST_TYPE_KEY);
            string tLastReferenceEdited = EditorPrefs.GetString(K_EDITOR_LAST_REFERENCE_KEY);
            NWDBasis<K> rObject = ObjectInEditionReccord(tTypeEdited, tLastReferenceEdited);
            if (rObject != null)
            {
                SetObjectInEdition(rObject);
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasis<K> ObjectInEditionReccord(string sClassPHP, string sReference)
        {
            NWDBasis<K> rObject = null;
            if (!string.IsNullOrEmpty(sClassPHP) && !string.IsNullOrEmpty(sReference))
            {
                if (sClassPHP == Datas().ClassNamePHP)
                {
                    K tObj = NWDBasis<K>.GetDataByReference(sReference);
                    rObject = tObj;
                }
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SaveObjectInEdition()
        {

            //Debug.Log("NWDBasis<K> SaveObjectInEdition()");
            NWDBasis<K> tObject = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
            if (tObject == null)
            {
                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, string.Empty);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, string.Empty);
            }
            else
            {

                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, NWDDatas.FindTypeInfos(tObject.GetType()).ClassNamePHP);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, tObject.Reference);
            }
            //Debug.Log("NWDBasis<K> SaveObjectInEdition() NWD_LastTypeEdited : " + EditorPrefs.GetString(K_EDITOR_LAST_TYPE_KEY));
            //Debug.Log("NWDBasis<K> SaveObjectInEdition() NWD_LastReferenceEdited : " + EditorPrefs.GetString(K_EDITOR_LAST_REFERENCE_KEY));
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetObjectInEdition(object sObject, bool sResetStack = true, bool sFocus = true)
        {

            GUI.FocusControl(null);
            NWDDataInspector.InspectNetWorkedData(sObject, sResetStack, sFocus);
            if (sObject != null)
            {
                NWDBasisEditor.ObjectEditorLastType = sObject.GetType();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
            }
            else if (NWDBasisEditor.ObjectEditorLastType != null)
            {
                // repaint all window?
                // or just last type?
                NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                NWDBasisEditor.ObjectEditorLastType = null;
            }
            SaveObjectInEdition();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsObjectInEdition(object sObject)
        {
            bool rReturn = false;
            //if (NWDDataManager.UseUnityInpector == false) {
            if (NWDDataInspector.ObjectInEdition() == sObject)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        ////        public static Dictionary<int,bool> DrawableList = new Dictionary<int,bool>();
        //        public static List<bool> GroupLevelList = new List<bool>();
        //        public static List<bool> GroupOpenList = new List<bool>();
        //        //-------------------------------------------------------------------------------------------------------------
        //        public void ResetDrawable ()
        //        {
        ////            DrawableList = new Dictionary<int,bool>();
        //            DrawableList = new List<bool>();
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public bool GetDrawable ()
        //        {
        ////            bool rReturn = true;
        ////            int tI = EditorGUI.indentLevel;
        ////            while (tI > 0) {
        ////                if (DrawableList.ContainsKey (tI)) {
        ////                    if (DrawableList [tI] == false) {
        ////                        rReturn = false;
        ////                    }
        ////                }
        ////                tI--;
        ////            }
        ////            rReturn = true;
        ////            return rReturn;
        ////            return DrawableList.LastOrDefault<bool>(true);
        //
        //            bool rReturn = true;
        //            foreach (bool tV in DrawableList) {
        //                if (tV == false) {
        //                    rReturn = false;
        //                }
        //            }
        //
        //            rReturn = true;
        //
        //            return rReturn;
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public void AddDrawable (bool sValue)
        //        {
        ////            if (DrawableList.ContainsKey (EditorGUI.indentLevel)) {
        ////                DrawableList [EditorGUI.indentLevel] = sValue;
        ////            } else {
        ////                DrawableList.Add(EditorGUI.indentLevel,sValue);
        ////            }
        //            DrawableList.Add(sValue);
        //        }
        //        
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D ReloadPreviewTexture2D()
        {
            PreviewTextureIsLoaded = true;
            PreviewTexture = null;
            UnityEngine.Object tObject = null;
            if (Preview != null && Preview != string.Empty)
            {
                //tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(GameObject)) as GameObject;
                tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(UnityEngine.Object)) as UnityEngine.Object;
                if (tObject != null)
                {
                    PreviewTexture = AssetPreview.GetAssetPreview(tObject);
                    while (AssetPreview.IsLoadingAssetPreview(tObject.GetInstanceID()))
                    {
                        // Loading
                    }
                }
            }
            return PreviewTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D GetPreviewTexture2D()
        {
            if (PreviewTextureIsLoaded == false)
            {
                ReloadPreviewTexture2D();
            }
            return PreviewTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Vector2 sOrigin)
        {
            DrawPreviewTexture2D(new Rect(sOrigin.x, sOrigin.y, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Rect sRect)
        {
            if (PreviewTextureIsLoaded == false)
            {
                ReloadPreviewTexture2D();
            }
            if (PreviewTexture != null)
            {
                EditorGUI.DrawPreviewTexture(sRect, PreviewTexture);
            }
        }
        //-------------------------------------------------------------------------------------------------------------






        public float DrawObjectInspectorHeight()
        {

            //NWDConstants.LoadImages();
            NWDConstants.LoadStyles();

            float tY = 0;
            //EditorGUI.indentLevel = 0;
            //            ResetDrawable ();
            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), 100);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tFloatFieldStyle = new GUIStyle(EditorStyles.numberField);
            tFloatFieldStyle.fixedHeight = tFloatFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tColorFieldStyle = new GUIStyle(EditorStyles.colorField);
            tColorFieldStyle.fixedHeight = tColorFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tToggleStyle = new GUIStyle(EditorStyles.toggle);
            tToggleStyle.fixedHeight = tToggleStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            GUIStyle tBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tBoldFoldoutStyle.fontStyle = FontStyle.Bold;
            tBoldFoldoutStyle.fixedHeight = tBoldFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            Type tType = ClassType();

            bool tDraw = true;
            NWDGroupStartAttribute tActualGroupReference = null;

            PropertyInfo[] tPropertiesArray = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> tPropertiesList = new List<PropertyInfo>();
            List<PropertyInfo> tPropertieListAddon = new List<PropertyInfo>();
            foreach (PropertyInfo tProp in tPropertiesArray)
            {
                if (tProp.GetCustomAttributes(typeof(NWDAddonAttribute), true).Length > 0)
                {
                    tPropertieListAddon.Add(tProp);
                }
                else
                {
                    tPropertiesList.Add(tProp);
                }
            }
            tPropertiesList.AddRange(tPropertieListAddon);

            foreach (var tProp in tPropertiesList)
            {
                if (tProp.Name != "ID"
                    && tProp.Name != "Reference"
                    && tProp.Name != "DC"
                    && tProp.Name != "DM"
                    && tProp.Name != "DD"
                    && tProp.Name != "DS"
                    && tProp.Name != "AC"
                    && tProp.Name != "XX"
                    && tProp.Name != "Integrity"
                    && tProp.Name != "InternalKey"
                    && tProp.Name != "InternalDescription"
                    && tProp.Name != "Preview"
                    && tProp.Name != "DevSync"
                    && tProp.Name != "PreprodSync"
                    && tProp.Name != "ProdSync"
                    && tProp.Name != "Tag"
                    && tProp.Name != "ServerHash"
                    && tProp.Name != "InError"
                    && tProp.Name != "ServerLog"
                    && tProp.Name != "WebServiceVersion"
                    && tProp.Name != "ReferenceVersioned"
                    && tProp.Name != "MinVersion"
                    && tProp.Name != "MaxVersion"

                   )
                {

                    foreach (NWDGroupEndAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupEndAttribute), true))
                    {
                        if (tActualGroupReference != null)
                        {
                            tActualGroupReference = tActualGroupReference.Parent;
                        }
                        if (tActualGroupReference != null)
                        {
                            tDraw = tActualGroupReference.IsDrawable(Datas().ClassName);
                        }
                        else
                        {
                            tDraw = true;
                        }
                    }

                    // draw separator before
                    foreach (NWDGroupSeparatorAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupSeparatorAttribute), true))
                    {
                        tY += NWDConstants.kFieldMarge * 2;
                    }

                    // create a foldout group
                    foreach (NWDGroupStartAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupStartAttribute), true).Reverse())
                    {
                        if (tDraw == true)
                        {
                            bool tBold = tReference.mBoldHeader;
                            bool tReducible = tReference.mReducible;
                            bool tActualDraw = tReference.mOpen;
                            tActualDraw = tReference.GetDrawable(Datas().ClassName);
                            if (tReducible == true)
                            {
                                if (tBold == true)
                                {
                                    tY += tBoldFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else
                                {
                                    tY += tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                            }
                            else
                            {
                                if (tActualDraw == true)
                                {
                                    if (tBold == true)
                                    {
                                        tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    }
                                    else
                                    {
                                        tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    }
                                }
                            }
                            tReference.SetDrawable(Datas().ClassName, tActualDraw);
                            tDraw = tReference.IsDrawable(Datas().ClassName);
                        }
                        tReference.Parent = tActualGroupReference;
                        tActualGroupReference = tReference;
                    }

                    if (tDraw)
                    {

                        foreach (NWDSeparatorAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDSeparatorAttribute), true))
                        {
                            tY += NWDConstants.kFieldMarge * 2;
                        }
                        // draw space
                        foreach (NWDSpaceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDSpaceAttribute), true))
                        {
                            //                        if (tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true).Length > 0) {
                            tY += NWDConstants.kFieldMarge * 3;
                        }
                        foreach (NWDHeaderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDHeaderAttribute), true))
                        {
                            //                        GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), tReference.mHeader, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }

                    //if (tProp.GetCustomAttributes(typeof(NWDIfAttribute), true).Length > 0)
                    //{
                    //    NWDIfAttribute tReference = (NWDIfAttribute)tProp.GetCustomAttributes(typeof(NWDIfAttribute), true)[0];
                    //    if (!tReference.IsDrawable(this) && tReference.mVisible == false)
                    //    {
                    //        tDraw = false;
                    //    }
                    //}

                    // finish the foldout group management
                    // So Iif I nee dto draw somethings … 
                    if (tDraw)
                    {
                        //                        if (tProp.GetCustomAttributes (typeof(NWDNotEditableAttribute), true).Length > 0) {
                        //                            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        //                        } else 
                        {
                            //TO-DO : (FUTUR ADDS) Insert new NWDxxxxType

                            if (tProp.GetCustomAttributes(typeof(NWDIntSliderAttribute), true).Length > 0)
                            {
                                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDFloatSliderAttribute), true).Length > 0)
                            {
                                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDLongStringAttribute), true).Length > 0)
                            {
                                tY += tTextFieldStyle.fixedHeight * NWDConstants.kLongString + NWDConstants.kFieldMarge;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDVeryLongStringAttribute), true).Length > 0)
                            {
                                tY += tTextFieldStyle.fixedHeight * NWDConstants.kVeryLongString + NWDConstants.kFieldMarge;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDEnumStringAttribute), true).Length > 0)
                            {
                                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDEnumAttribute), true).Length > 0)
                            {
                                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                            }
                            else
                            {
                                Type tTypeOfThis = tProp.PropertyType;
                                //Debug.Log (tTypeOfThis.Name);
                                if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis.IsEnum)
                                {
                                    tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(bool))
                                {
                                    tY += tToggleStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(int))
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(long))
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(float))
                                {
                                    tY += tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(double))
                                {
                                    tY += tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }

                                //TO-DO : (FUTUR ADDS) Insert new NWDDataType

                                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                                {
                                    var tValue = tProp.GetValue(this, null);
                                    if (tValue == null)
                                    {
                                        tValue = Activator.CreateInstance(tTypeOfThis);
                                    }
                                    BTBDataType tBTBDataType = (BTBDataType)tValue;
                                    float tHeight = tBTBDataType.ControlFieldHeight();
                                    tY += tHeight + NWDConstants.kFieldMarge;

                                    //                                    float tHeight = 0.0f;
                                    //                                    var tMethodInfo = tTypeOfThis.GetMethod ("ControlFieldHeight", BindingFlags.Public | BindingFlags.Instance);
                                    //                                    if (tMethodInfo != null) {
                                    //                                        string tHeightString = tMethodInfo.Invoke (tValue, null) as string;
                                    //                                        float.TryParse (tHeightString, out tHeight);
                                    //                                    }
                                    //                                    tY += tHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                                {
                                    var tValue = tProp.GetValue(this, null);
                                    if (tValue == null)
                                    {
                                        tValue = Activator.CreateInstance(tTypeOfThis);
                                    }
                                    BTBDataTypeInt tBTBDataType = (BTBDataTypeInt)tValue;
                                    float tHeight = tBTBDataType.ControlFieldHeight();
                                    tY += tHeight + NWDConstants.kFieldMarge;

                                    //                                    float tHeight = 0.0f;
                                    //                                    var tMethodInfo = tTypeOfThis.GetMethod ("ControlFieldHeight", BindingFlags.Public | BindingFlags.Instance);
                                    //                                    if (tMethodInfo != null) {
                                    //                                        string tHeightString = tMethodInfo.Invoke (tValue, null) as string;
                                    //                                        float.TryParse (tHeightString, out tHeight);
                                    //                                    }
                                    //                                    tY += tHeight + NWDConstants.kFieldMarge;
                                }
                                else
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                            }
                        }
                    }
                }
            }
            // add final marge
            tY += NWDConstants.kFieldMarge;
            tY += AddonEditorHeight();
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the object inspector by default.
        /// </summary>
        /// <returns><c>true</c>, if object inspector was updated, <c>false</c> otherwise.</returns>
        public Rect DrawObjectInspector(Rect sInRect, bool sWithScrollview, bool sEditionEnable)
        {

            //NWDConstants.LoadImages();
            NWDConstants.LoadStyles();

            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tFloatFieldStyle = new GUIStyle(EditorStyles.numberField);
            tFloatFieldStyle.fixedHeight = tFloatFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tColorFieldStyle = new GUIStyle(EditorStyles.colorField);
            tColorFieldStyle.fixedHeight = tColorFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tToggleStyle = new GUIStyle(EditorStyles.toggle);
            tToggleStyle.fixedHeight = tToggleStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tBoldFoldoutStyle.fontStyle = FontStyle.Bold;
            tBoldFoldoutStyle.fixedHeight = tBoldFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);


            bool rNeedBeUpdate = false;
            EditorGUI.BeginChangeCheck();

            if (sWithScrollview == true)
            {
                float tScrollBarMarge = 0;
                float tHeightContent = DrawObjectInspectorHeight();
                if (tHeightContent >= sInRect.height)
                {
                    tScrollBarMarge = 20.0f;
                }
                Datas().ObjectEditorScrollPosition = GUI.BeginScrollView(sInRect, Datas().ObjectEditorScrollPosition, new Rect(0, 0, sInRect.width - tScrollBarMarge, tHeightContent));

                tWidth = sInRect.width - tScrollBarMarge - NWDConstants.kFieldMarge * 2;
                tX = NWDConstants.kFieldMarge;
                tY = NWDConstants.kFieldMarge;
            }

            bool tDraw = true;
            NWDGroupStartAttribute tActualGroupReference = null;
            Type tType = ClassType();

            EditorGUI.BeginDisabledGroup(sEditionEnable == false);


            PropertyInfo[] tPropertiesArray = tType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> tPropertiesList = new List<PropertyInfo>();
            List<PropertyInfo> tPropertieListAddon = new List<PropertyInfo>();
            foreach (PropertyInfo tProp in tPropertiesArray)
            {
                if (tProp.GetCustomAttributes(typeof(NWDAddonAttribute), true).Length > 0)
                {
                    tPropertieListAddon.Add(tProp);
                }
                else
                {
                    tPropertiesList.Add(tProp);
                }
            }
            tPropertiesList.AddRange(tPropertieListAddon);

            foreach (var tProp in tPropertiesList)
            {
                if (tProp.Name != "ID"
                    && tProp.Name != "Reference"
                    && tProp.Name != "DC"
                    && tProp.Name != "DM"
                    && tProp.Name != "DD"
                    && tProp.Name != "DS"
                    && tProp.Name != "AC"
                    && tProp.Name != "XX"
                    && tProp.Name != "Integrity"
                    && tProp.Name != "InternalKey"
                    && tProp.Name != "InternalDescription"
                    && tProp.Name != "Preview"
                    && tProp.Name != "DevSync"
                    && tProp.Name != "PreprodSync"
                    && tProp.Name != "ProdSync"
                    && tProp.Name != "Tag"
                    && tProp.Name != "ServerHash"
                    && tProp.Name != "InError"
                    && tProp.Name != "ServerLog"
                    && tProp.Name != "WebServiceVersion"
                    && tProp.Name != "ReferenceVersioned"
                    && tProp.Name != "MinVersion"
                    && tProp.Name != "MaxVersion"

                   )
                {

                    foreach (NWDGroupEndAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupEndAttribute), true))
                    {
                        if (tActualGroupReference != null)
                        {
                            tActualGroupReference = tActualGroupReference.Parent;
                        }
                        if (tActualGroupReference != null)
                        {
                            tDraw = tActualGroupReference.IsDrawable(Datas().ClassName);
                        }
                        else
                        {
                            tDraw = true;
                        }
                        if (tDraw == true)
                        {
                            EditorGUI.indentLevel--;
                        }
                    }
                    // draw separator before
                    foreach (NWDGroupSeparatorAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupSeparatorAttribute), true))
                    {
                        //                        if (tProp.GetCustomAttributes (typeof(NWDSeparatorAttribute), true).Length > 0) {
                        EditorGUI.DrawRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1), NWDConstants.kRowColorLine);
                        tY += NWDConstants.kFieldMarge * 2;
                    }

                    // create a foldout group
                    foreach (NWDGroupStartAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupStartAttribute), true).Reverse())
                    {
                        if (tDraw == true)
                        {

                            bool tBold = tReference.mBoldHeader;
                            bool tReducible = tReference.mReducible;
                            bool tActualDraw = tReference.mOpen;
                            tActualDraw = tReference.GetDrawable(Datas().ClassName);
                            if (tReducible == true)
                            {
                                if (tBold == true)
                                {
                                    tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tBoldFoldoutStyle.fixedHeight), tActualDraw, tReference.Content(), tBoldFoldoutStyle);
                                    tY += tBoldFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else
                                {
                                    tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tFoldoutStyle.fixedHeight), tActualDraw, tReference.Content(), tFoldoutStyle);
                                    tY += tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                            }
                            else
                            {
                                if (tActualDraw == true)
                                {
                                    if (tBold == true)
                                    {
                                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), tReference.mGroupName, tBoldLabelStyle);
                                        tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    }
                                    else
                                    {
                                        EditorGUI.LabelField(new Rect(tX, tY, tWidth, tLabelStyle.fixedHeight), tReference.mGroupName, tLabelStyle);
                                        tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    }
                                }
                            }
                            tReference.SetDrawable(Datas().ClassName, tActualDraw);
                            tDraw = tReference.IsDrawable(Datas().ClassName);
                            EditorGUI.indentLevel++;
                        }
                        tReference.Parent = tActualGroupReference;
                        tActualGroupReference = tReference;
                    }
                    // finish the foldout group management


                    if (tDraw)
                    {

                        foreach (NWDSeparatorAttribute tInsideReference in tProp.GetCustomAttributes(typeof(NWDSeparatorAttribute), true))
                        {
                            Rect tRect = EditorGUI.IndentedRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1));
                            //float tIndentWidth = tRect.x;
                            //                      if (tProp.GetCustomAttributes (typeof(NWDSeparatorAttribute), true).Length > 0) {
                            EditorGUI.DrawRect(tRect, NWDConstants.kRowColorLine);
                            tY += NWDConstants.kFieldMarge * 2;
                        }
                        // draw space
                        foreach (NWDSpaceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDSpaceAttribute), true))
                        {
                            //                        if (tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true).Length > 0) {
                            //                        NWDSpaceAttribute tReference = (NWDSpaceAttribute)tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true) [0];
                            tY += NWDConstants.kFieldMarge * 3;
                        }
                        foreach (NWDHeaderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDHeaderAttribute), true))
                        {
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), tReference.mHeader, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }
                    // So Iif I nee dto draw somethings … 
                    if (tDraw ==true)
                    {
                        bool tHidden = false;
                        // get entitled and toolstips
                        string tEntitled = NWDToolbox.SplitCamelCase(tProp.Name);
                        string tToolsTips = string.Empty;
                        if (tProp.GetCustomAttributes(typeof(NWDEntitledAttribute), true).Length > 0)
                        {
                            NWDEntitledAttribute tReference = (NWDEntitledAttribute)tProp.GetCustomAttributes(typeof(NWDEntitledAttribute), true)[0];
                            tEntitled = tReference.Entitled;
                            tToolsTips = tReference.ToolsTips;
                        }

                        if (tProp.GetCustomAttributes(typeof(NWDNotWorking), true).Length > 0)
                        {
                            NWDNotWorking tReference = (NWDNotWorking)tProp.GetCustomAttributes(typeof(NWDNotWorking), true)[0];

                            tEntitled = "[NOT WORKING] "+tEntitled;
                            tToolsTips = tReference.ToolsTips + " " + tToolsTips;
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDInDevelopment), true).Length > 0)
                        {
                            NWDInDevelopment tReference = (NWDInDevelopment)tProp.GetCustomAttributes(typeof(NWDInDevelopment), true)[0];
                            tEntitled = "[IN DEV] " + tEntitled;
                            tToolsTips = tReference.ToolsTips + " " + tToolsTips;
                        }

                        if (tProp.GetCustomAttributes(typeof(NWDTooltipsAttribute), true).Length > 0)
                        {
                            NWDTooltipsAttribute tReference = (NWDTooltipsAttribute)tProp.GetCustomAttributes(typeof(NWDTooltipsAttribute), true)[0];
                            tToolsTips = tReference.ToolsTips;
                        }

                        // if is enable 
                        //                        if (tProp.GetCustomAttributes (typeof(NWDNotEditableAttribute), true).Length > 0) {
                        ////                            string tValue = tProp.GetValue (this, null) as string;
                        ////                            EditorGUI.LabelField (new Rect (tX, tY, tWidth, tLabelStyle.fixedHeight), tEntitled, tValue, tLabelStyle);
                        ////                            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        //
                        //
                        //                        } 
                        bool tDisabled = false;
                        if (tProp.GetCustomAttributes(typeof(NWDNotEditableAttribute), true).Length > 0)
                        {
                            tDisabled = true;
                        }
                        else
                        {
                            if (tProp.GetCustomAttributes(typeof(NWDIfAttribute), true).Length > 0)
                            {
                                NWDIfAttribute tReference = (NWDIfAttribute)tProp.GetCustomAttributes(typeof(NWDIfAttribute), true)[0];
                                tDisabled = !tReference.IsDrawable(this);
                                if (tDisabled && tReference.mVisible == false)
                                {
                                    tHidden = true;
                                }
                            }
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDNotVisible), true).Length > 0)
                        {
                               tHidden = true;
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDHidden), true).Length > 0)
                        {
                            tHidden = true;
                        }
                        if (tHidden==false)
                        {
                            EditorGUI.BeginDisabledGroup(tDisabled);
                            GUIContent tContent = new GUIContent(tEntitled, tToolsTips);
                            if (tProp.GetCustomAttributes(typeof(NWDIntSliderAttribute), true).Length > 0)
                            {
                                NWDIntSliderAttribute tSlider = tProp.GetCustomAttributes(typeof(NWDIntSliderAttribute), true)[0] as NWDIntSliderAttribute;
                                int tValue = (int)tProp.GetValue(this, null);
                                int tValueNext = EditorGUI.IntSlider(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tSlider.mMin, tSlider.mMax);
                                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDLongStringAttribute), true).Length > 0)
                            {
                                string tValue = (string)tProp.GetValue(this, null);
                                EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), tContent);
                                //remove EditorGUI.indentLevel to draw next controller without indent 
                                int tIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = 0;
                                string tValueNext = EditorGUI.TextArea(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, tTextFieldStyle.fixedHeight * NWDConstants.kLongString), tValue, NWDConstants.kTextAreaStyle);
                                tY += tTextFieldStyle.fixedHeight * NWDConstants.kLongString + NWDConstants.kFieldMarge;
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                                EditorGUI.indentLevel = tIndentLevel;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDVeryLongStringAttribute), true).Length > 0)
                            {
                                string tValue = (string)tProp.GetValue(this, null);
                                EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), tContent);

                                //remove EditorGUI.indentLevel to draw next controller without indent 
                                int tIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = 0;
                                string tValueNext = EditorGUI.TextArea(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, tTextFieldStyle.fixedHeight * NWDConstants.kVeryLongString), tValue, NWDConstants.kTextAreaStyle);
                                tY += tTextFieldStyle.fixedHeight * NWDConstants.kVeryLongString + NWDConstants.kFieldMarge;
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                                EditorGUI.indentLevel = tIndentLevel;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDFloatSliderAttribute), true).Length > 0)
                            {
                                NWDFloatSliderAttribute tSlider = tProp.GetCustomAttributes(typeof(NWDFloatSliderAttribute), true)[0] as NWDFloatSliderAttribute;
                                float tValue = (float)tProp.GetValue(this, null);
                                float tValueNext = EditorGUI.Slider(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tSlider.mMin, tSlider.mMax);
                                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDEnumStringAttribute), true).Length > 0)
                            {
                                NWDEnumStringAttribute tInfo = tProp.GetCustomAttributes(typeof(NWDEnumStringAttribute), true)[0] as NWDEnumStringAttribute;
                                string[] tV = tInfo.mEnumString;
                                string tValue = (string)tProp.GetValue(this, null);
                                int tValueInt = Array.IndexOf<string>(tV, tValue);
                                EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), tContent);

                                //remove EditorGUI.indentLevel to draw next controller without indent 
                                int tIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = 0;

                                int tValueIntNext = EditorGUI.Popup(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, tPopupdStyle.fixedHeight), string.Empty, tValueInt, tV, tPopupdStyle);
                                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                                string tValueNext = string.Empty;
                                if (tValueIntNext < tV.Length && tValueIntNext >= 0)
                                {
                                    tValueNext = tV[tValueIntNext];
                                }
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                                EditorGUI.indentLevel = tIndentLevel;
                            }
                            else if (tProp.GetCustomAttributes(typeof(NWDEnumAttribute), true).Length > 0)
                            {
                                NWDEnumAttribute tInfo = tProp.GetCustomAttributes(typeof(NWDEnumAttribute), true)[0] as NWDEnumAttribute;
                                string[] tV = tInfo.mEnumString;
                                int[] tI = tInfo.mEnumInt;
                                int tValue = (int)tProp.GetValue(this, null);
                                int tValueInt = Array.IndexOf<int>(tI, tValue);
                                EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.kTextFieldStyle.fixedHeight), tContent);

                                //remove EditorGUI.indentLevel to draw next controller without indent 
                                int tIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = 0;

                                int tValueIntNext = EditorGUI.Popup(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth - EditorGUIUtility.labelWidth, tPopupdStyle.fixedHeight), string.Empty, tValueInt, tV, tPopupdStyle);
                                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                                int tValueNext = 0;
                                if (tValueIntNext < tI.Length && tValueIntNext >= 0)
                                {
                                    tValueNext = tI[tValueIntNext];
                                }
                                if (tValueNext != tValue)
                                {
                                    tProp.SetValue(this, tValueNext, null);
                                    rNeedBeUpdate = true;
                                }
                                EditorGUI.indentLevel = tIndentLevel;
                            }
                            else
                            {
                                Type tTypeOfThis = tProp.PropertyType;
                                //Debug.Log (tTypeOfThis.Name);

                                if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                                {
                                    string tValue = tProp.GetValue(this, null) as string;
                                    string tValueNext = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tTextFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis.IsEnum)
                                {
                                    Enum tValue = tProp.GetValue(this, null) as Enum;
                                    Enum tValueNext = tValue;
                                    if (tProp.GetCustomAttributes(typeof(NWDFlagsEnumAttribute), true).Length > 0)
                                    {
                                        tValueNext = EditorGUI.EnumFlagsField(new Rect(tX, tY, tWidth, tPopupdStyle.fixedHeight), tContent, tValue, tPopupdStyle);
                                    }
                                    else
                                    {
                                        tValueNext = EditorGUI.EnumPopup(new Rect(tX, tY, tWidth, tPopupdStyle.fixedHeight), tContent, tValue, tPopupdStyle);
                                    }
                                    tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(bool))
                                {
                                    bool tValue = (bool)tProp.GetValue(this, null);
                                    bool tValueNext = EditorGUI.Toggle(new Rect(tX, tY, tWidth, tToggleStyle.fixedHeight), tContent, tValue, tToggleStyle);
                                    tY += tToggleStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(int))
                                {
                                    int tValue = (int)tProp.GetValue(this, null);
                                    int tValueNext = EditorGUI.IntField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tFloatFieldStyle);
                                    tY += tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(long))
                                {
                                    long tValue = (long)tProp.GetValue(this, null);
                                    long tValueNext = EditorGUI.LongField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tFloatFieldStyle);
                                    tY += tFloatFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(float))
                                {
                                    float tValue = (float)tProp.GetValue(this, null);
                                    float tValueNext = (float)EditorGUI.FloatField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tFloatFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    float EPSILON = 0;
                                    if (Math.Abs(tValueNext - tValue) > EPSILON)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(double))
                                {
                                    double tValue = (double)tProp.GetValue(this, null);
                                    double tValueNext = EditorGUI.DoubleField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tTextFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    float EPSILON = 0;
                                    if (Math.Abs(tValueNext - tValue) > EPSILON)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                                {
                                    var tValue = tProp.GetValue(this, null);
                                    if (tValue == null)
                                    {
                                        tValue = Activator.CreateInstance(tTypeOfThis);
                                    }
                                    BTBDataType tBTBDataType = tValue as BTBDataType;
                                    BTBDataType tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight),
                                                                                             tEntitled, tToolsTips) as BTBDataType;

                                    if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                                    {
                                        //Debug.Log("change in "+tTypeOfThis.Name);
                                        tProp.SetValue(this, tBTBDataTypeNext, null);
                                        rNeedBeUpdate = true;

                                        NWDNodeEditor.ReAnalyzeIfNecessary(this);
                                    }
                                    float tHeight = tBTBDataType.ControlFieldHeight();
                                    tY += tHeight + NWDConstants.kFieldMarge;

                                }
                                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                                {
                                    var tValue = tProp.GetValue(this, null);
                                    if (tValue == null)
                                    {
                                        tValue = Activator.CreateInstance(tTypeOfThis);
                                    }
                                    BTBDataTypeInt tBTBDataType = tValue as BTBDataTypeInt;
                                    BTBDataTypeInt tBTBDataTypeNext = tBTBDataType.ControlField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight),
                                                                                             tEntitled, tToolsTips) as BTBDataTypeInt;

                                    if (tBTBDataTypeNext.Value != tBTBDataType.Value)
                                    {
                                        //Debug.Log("change in "+tTypeOfThis.Name);
                                        tProp.SetValue(this, tBTBDataTypeNext, null);
                                        rNeedBeUpdate = true;

                                        NWDNodeEditor.ReAnalyzeIfNecessary(this);
                                    }
                                    float tHeight = tBTBDataType.ControlFieldHeight();
                                    tY += tHeight + NWDConstants.kFieldMarge;

                                }
                                else
                                {
                                    string tValue = tProp.GetValue(this, null) as string;
                                    EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tEntitled, tValue, tTextFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                            }

                            EditorGUI.EndDisabledGroup();
                        }
                    }
                    //Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(foo, null));
                }
            }
            // add special editor information
            tY += AddonEditor(new Rect(tX, tY, tWidth, 0.0f));

            if (sWithScrollview == true)
            {
                GUI.EndScrollView();
            }

            if (EditorGUI.EndChangeCheck())
            {
                rNeedBeUpdate = true;
            }

            if (AddonEdited(rNeedBeUpdate) == true)
            {
                rNeedBeUpdate = true;
            }
            if (rNeedBeUpdate == true)
            {
                //NWDDataInspector.ActiveRepaint();
                if (sEditionEnable == true)
                {
                    ErrorCheck();
                    WebserviceVersionCheckMe();
                    if (IntegrityValue() != this.Integrity)
                    {
                        //Debug.Log("change need UpdateMeLater() call ");
                        UpdateData(true, NWDWritingMode.ByEditorDefault);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            Rect tFinalRect = new Rect(sInRect.position.x, tY, sInRect.width, tY - sInRect.position.y);
            return tFinalRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawObjectEditor(Rect sInRect, bool sWithScrollview)
        {

            //            float tWidth = EditorGUIUtility.currentViewWidth;
            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            //            float tHeight = sInRect.height-tMarge*2;
            float tX = NWDConstants.kFieldMarge;
            float tY = NWDConstants.kFieldMarge;
            //            int tRow = 0;

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniLabelStyleCenter = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyleCenter.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);
            tMiniLabelStyleCenter.alignment = TextAnchor.MiddleCenter;

            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);


            GUIStyle tTitleLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tTitleLabelStyle.alignment = TextAnchor.MiddleCenter;
            tTitleLabelStyle.fontSize = 14;
            tTitleLabelStyle.fixedHeight = tTitleLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);
            tTitleLabelStyle.richText = true;

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            bool tCanBeEdit = true;
            bool tTestIntegrity = TestIntegrity();

            //Draw Internal Key
            //GUI.SetNextControlName(NWDConstants.K_CLASS_FOCUS_ID);
            //            GUI.Label (new Rect (tX, tY, tWidth, tTitleLabelStyle.fixedHeight), ClassNamePHP () + "'s Object", tTitleLabelStyle);
            string tTitle = InternalKey;

            if (string.IsNullOrEmpty(tTitle))
            {
                tTitle = "Unamed " + Datas().ClassNamePHP + string.Empty;
                //                tTitle = ClassNamePHP () + "'s Object";
            }
            if (InError == true)
            {
                tTitle = "<b><color=red>" + NWDConstants.K_WARNING + tTitle + "</color></b>";
            }
            GUI.Label(new Rect(tX, tY, tWidth, tTitleLabelStyle.fixedHeight), tTitle, tTitleLabelStyle);

            // Draw reference
            GUI.Label(new Rect(tX, tY + tTitleLabelStyle.fixedHeight - NWDConstants.kFieldMarge, tWidth, tTitleLabelStyle.fixedHeight), Reference, tMiniLabelStyleCenter);


            // add button to navigate next / preview
            // TODO : change < to image
            if (NWDDataInspector.InspectNetWorkedPreview())
            {
                if (GUI.Button(new Rect(tX, tY + 10, 20, 20), "<"))
                {
                    NWDDataInspector.InspectNetWorkedDataPreview();
                }
            }

            // TODO : change > to image
            if (NWDDataInspector.InspectNetWorkedNext())
            {
                if (GUI.Button(new Rect(tX + tWidth - 20, tY + 10, 20, 20), ">"))
                {
                    NWDDataInspector.InspectNetWorkedDataNext();
                }
            }
            tY += tTitleLabelStyle.fixedHeight + NWDConstants.kFieldMarge * 2;

            Texture2D tTextureOfClass = Datas().TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUI.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
            }
            tY += 32 + NWDConstants.kFieldMarge;


            //            EditorGUI.BeginDisabledGroup (true);
            //            EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), ClassNamePHP () , this,  typeof(NWDBasis<K>), false);
            //            tY += tObjectFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            //            EditorGUI.EndDisabledGroup ();
            //
            //            GUI.Label (new Rect (tX,tY,tWidth,tBoldLabelStyle.fixedHeight),"Test de font BBBB",tBoldLabelStyle);
            //            tY+=tBoldLabelStyle.fixedHeight + tMarge;
            //if (tTestIntegrity == false || XX > 0) {tCanBeEdit = false;}EditorGUI.BeginDisabledGroup (tCanBeEdit == false);EditorGUI.EndDisabledGroup ();



            if (WebserviceVersionIsValid())
            {


                if (tTestIntegrity == false)
                {

                    EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorError);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_IS_FALSE, tBoldLabelStyle);
                    tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tX, tY, tWidth, tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_HELPBOX, MessageType.Error);
                    tY += tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                    if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_REEVAL, tMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_INTEGRITY_WARNING,
                                NWDConstants.K_APP_BASIS_INTEGRITY_WARNING_MESSAGE,
                                NWDConstants.K_APP_BASIS_INTEGRITY_OK,
                                NWDConstants.K_APP_BASIS_INTEGRITY_CANCEL))
                        {
                            UpdateData(true);
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                        }
                    }
                    tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                    tY += NWDConstants.kFieldMarge;

                }
                else if (XX > 0)
                {

                    EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorTrash);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH, tBoldLabelStyle);
                    tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tX, tY, tWidth, tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH_HELPBOX, MessageType.Warning);
                    tY += tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                    if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UNTRASH, tMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_UNTRASH_WARNING,
                                NWDConstants.K_APP_BASIS_UNTRASH_WARNING_MESSAGE,
                                NWDConstants.K_APP_BASIS_UNTRASH_OK,
                                NWDConstants.K_APP_BASIS_UNTRASH_CANCEL
                            ))
                        {
                            UnTrashData();
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                        }
                    }
                    tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                    tY += NWDConstants.kFieldMarge;
                }
            }
            else
            {
                EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorWarning);
                tCanBeEdit = false;

                GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR, tBoldLabelStyle);
                tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                EditorGUI.HelpBox(new Rect(tX, tY, tWidth, tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_HELPBOX, MessageType.Warning);
                tY += tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_FIX, tMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING_MESSAGE,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_OK,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_CANCEL
                        ))
                    {
                        // TODO UPDATE
                        UpdateData();
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    }
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                tY += NWDConstants.kFieldMarge;
            }
            float tImageWidth = (tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge) * 3;

            tX = tImageWidth + NWDConstants.kFieldMarge * 2;
            tWidth = sInRect.width - NWDConstants.kFieldMarge * 3 - tImageWidth;

            EditorGUI.BeginDisabledGroup(tCanBeEdit == false);


            EditorGUI.DrawRect(new Rect(0, tY - NWDConstants.kFieldMarge, sInRect.width, sInRect.height), NWDConstants.kIdentityColor);

            tY += NWDConstants.kFieldMarge;

            //GUI.Label (new Rect (NWDConstants.kFieldMarge, tY, tImageWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PREVIEW, tMiniLabelStyle);

            UnityEngine.Object tObject = null;
            if (Preview != null && Preview != string.Empty)
            {
                tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(UnityEngine.Object)) as UnityEngine.Object;
            }
            //Texture2D tTexture2D = null;

            //if (tObject != null)
            //{
            //    //                if (mGameObjectEditor == null) {
            //    //                    mGameObjectEditor = Editor.CreateEditor (tObject);
            //    //                }
            //    // draw prefab if it's possible
            //    tTexture2D = AssetPreview.GetAssetPreview(tObject);
            //    while (AssetPreview.IsLoadingAssetPreview(tObject.GetInstanceID()))
            //    {
            //        // Loading
            //    }
            //}
            //if (tTexture2D != null)
            //{
            //    EditorGUI.DrawPreviewTexture(new Rect(NWDConstants.kFieldMarge, tY + tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge, tImageWidth, tImageWidth), tTexture2D);
            //}
            Texture2D tTexture2D = AssetPreview.GetAssetPreview(tObject);
            if (tTexture2D != null)
            {
                EditorGUI.DrawPreviewTexture(new Rect(NWDConstants.kFieldMarge, tY, tImageWidth, tImageWidth), tTexture2D);
            }
            //DrawPreviewTexture2D(new Rect(NWDConstants.kFieldMarge, tY +tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge, tImageWidth, tImageWidth));

            //if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_EDITOR_NODAL))
            //{
            //    NWDNodeEditor.SetObjectInNodeWindow(this);
            //}
            //tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            //            GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INFORMATIONS, tBoldLabelStyle);


            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), Datas().ClassNamePHP + "'s Object", tBoldLabelStyle);
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            //GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE + Reference, tMiniLabelStyle);
            //tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DM + NWDToolbox.TimeStampToDateTime(DM).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
            tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            Datas().kSyncAndMoreInformations = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), Datas().kSyncAndMoreInformations, NWDConstants.K_APP_BASIS_INFORMATIONS, tFoldoutStyle);
            tY += tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;

            tY += NWDConstants.kFieldMarge;

            //tX = NWDConstants.kFieldMarge;

            if (Datas().kSyncAndMoreInformations)
            {

                EditorGUI.EndDisabledGroup();


                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DC + "(" + DC.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DC).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_Sync + "(" + DS.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DevSync + "(" + DevSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DevSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PreprodSync + "(" + PreprodSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(PreprodSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ProdSync + "(" + ProdSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(ProdSync).ToString("yyyy/MM/dd HH:mm:s")
                            + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_xx + XX.ToString(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ac + AC.ToString(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;


                EditorGUI.TextField(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), "server log ", ServerLog, tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), "server hash ", ServerHash, tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), "integrity val", Integrity, tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), "dyn integrity seq", DataAssembly(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), "dyn integrity req", IntegrityValue(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                EditorGUI.BeginDisabledGroup(tCanBeEdit == false);
            }

            tX = NWDConstants.kFieldMarge;

            /*
            if (GUI.Button (new Rect (tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UPDATE, tMiniButtonStyle)) {
                DM = NWDToolbox.Timestamp ();
                UpdateIntegrity ();
                UpdateObjectInListOfEdition (this);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue (this);
                NWDDataManager.SharedInstance().UpdateQueueExecute ();
                NWDDataManager.SharedInstance().RepaintWindowsInManager (this.GetType ());
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            if (GUI.Button (new Rect (tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DUPPLICATE, tMiniButtonStyle)) {
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue (this);
                K tNexObject = (K)DuplicateMe ();
                AddObjectInListOfEdition (tNexObject);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue (tNexObject);
                SetObjectInEdition (tNexObject);
                m_PageSelected = m_MaxPage * 3;
                NWDDataManager.SharedInstance().UpdateQueueExecute ();
                NWDDataManager.SharedInstance().RepaintWindowsInManager (this.GetType ());
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
//            tStyle.alignment = TextAnchor.MiddleLeft;
            if (AC == false) {
//                GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVED, tBoldLabelStyle);
//                tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
//                GUI.Label (new Rect (tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INACTIVED + NWDToolbox.TimeStampToDateTime (DD).ToString ("G"), tMiniLabelStyle);
//                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (GUI.Button (new Rect (tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, tMiniButtonStyle)) {
                    EnableMe ();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager (this.GetType ());
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            } else {
//                GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTIVE, tBoldLabelStyle);
//                tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (GUI.Button (new Rect (tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVE, tMiniButtonStyle)) {
                    DisableMe ();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager (this.GetType ());
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            }
                */
            tX = NWDConstants.kFieldMarge;
            tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;

            UnityEngine.Object pObj = EditorGUI.ObjectField(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight),
                                          NWDConstants.K_APP_BASIS_PREVIEW_GAMEOBJECT,
                                                            (UnityEngine.Object)tObject, typeof(UnityEngine.Object), false);
            tY += tObjectFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            string tPreFabGameObject = string.Empty;
            if (pObj != null)
            {
                if (PrefabUtility.GetPrefabType(pObj) == PrefabType.Prefab)
                {
                    tPreFabGameObject = AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabObject(pObj));
                }
            }

            if (Preview != tPreFabGameObject)
            {
                ReloadPreviewTexture2D();
                RepaintTableEditor();
                NWDNodeEditor.ReDraw();

                Preview = tPreFabGameObject;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //NWDDataManager.SharedInstance().UpdateData(this, NWDWritingMode.ByEditorDefault);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
            }


            bool tInternalKeyEditable = true;
            //            NWDInternalKeyNotEditable

            if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
            {
                tInternalKeyEditable = false;
            }

            if (tInternalKeyEditable == true)
            {
                string tInternalName = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, InternalKey, tTextFieldStyle);
                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (tInternalName != InternalKey)
                {
                    InternalKey = tInternalName;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    //UpdateObjectInListOfEdition(this);
                    //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                    UpdateData(true, NWDWritingMode.ByEditorDefault);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    NWDNodeEditor.UpdateNodeWindow(this);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, InternalKey, tTextFieldStyle);
                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.EndDisabledGroup();
            }

            string tInternalDescription = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_DESCRIPTION, InternalDescription, tTextFieldStyle);
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (tInternalDescription != InternalDescription)
            {
                InternalDescription = tInternalDescription;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //UpdateObjectInListOfEdition(this);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                NWDNodeEditor.UpdateNodeWindow(this);
            }

            if (tCanBeEdit == true)
            {
                // Web Service Version management
                NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                int tWebBuilt = tApp.WebBuild;
                List<int> tWebServicesInt = new List<int>();
                List<string> tWebServicesString = new List<string>();


                int tWebServiceVersionOldValue = 0;
                foreach (KeyValuePair<int, bool> tKeyValue in tApp.WSList)
                //for (int tW = 0; tW <= tWebBuilt; tW++)
                {
                    if (tKeyValue.Value == true)
                    {
                        if (tKeyValue.Key <= WebServiceVersion)
                        {
                            if (tKeyValue.Key >= tWebServiceVersionOldValue)
                            {
                                tWebServiceVersionOldValue = tKeyValue.Key;
                            }
                        }
                        tWebServicesInt.Add(tKeyValue.Key);
                        tWebServicesString.Add("WebService " + tKeyValue.Key.ToString());
                    }
                }
                int tWebServiceVersionOldIndex = tWebServicesInt.IndexOf(tWebServiceVersionOldValue);
                //kjbjgh g hj jgh jgh jghjg hjgh 
                int tWebServiceVersionIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight),
                                                                 "Web service " + WebServiceVersion + "(/" + tWebBuilt.ToString() + ")",
                                                                 tWebServiceVersionOldIndex, tWebServicesString.ToArray());
                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                int tWebServiceVersionNew = 0;
                if (tWebServicesInt.Count > 0)
                {
                    tWebServiceVersionNew = tWebServicesInt[tWebServiceVersionIndex];
                }
                if (WebServiceVersion != tWebServiceVersionNew)
                {
                    WebServiceVersion = tWebServiceVersionNew;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    //UpdateObjectInListOfEdition(this);
                    UpdateData(true, NWDWritingMode.ByEditorDefault, false);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    NWDNodeEditor.UpdateNodeWindow(this);
                }
            }
            else
            {
                NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                int tWebBuilt = tApp.WebBuild;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Web service " + WebServiceVersion + "(/" + tWebBuilt.ToString() + ")", WebServiceVersion.ToString());
                tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            }

            // Tag management
            List<int> tTagIntList = new List<int>();
            List<string> tTagStringList = new List<string>();
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                tTagIntList.Add(tTag.Key);
                tTagStringList.Add(tTag.Value);
            }

            NWDBasisTag tInternalTag = (NWDBasisTag)EditorGUI.IntPopup(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight),
                                                                        NWDConstants.K_APP_BASIS_INTERNAL_TAG,
                                                                       (int)Tag,
                                                                       tTagStringList.ToArray(), tTagIntList.ToArray());
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (tInternalTag != Tag)
            {
                Tag = tInternalTag;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //UpdateObjectInListOfEdition(this);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                NWDNodeEditor.UpdateNodeWindow(this);
            }

            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }

            // Toogle Dev Prepprod Prod and operation associated
            float tWidthTiers = tWidth / 3.0f;
            bool tDevLockAnalyze = false;
            if (DevSync >= 0)
            {
                tDevLockAnalyze = true;
            }
            bool tDevLock = EditorGUI.ToggleLeft(new Rect(tX, tY, tWidthTiers, tTextFieldStyle.fixedHeight), "Dev", tDevLockAnalyze);
            bool tPreprodLockAnalyze = false;
            if (PreprodSync >= 0)
            {
                tPreprodLockAnalyze = true;
            }
            bool tPreprodLock = EditorGUI.ToggleLeft(new Rect(tX + tWidthTiers, tY, tWidthTiers, tTextFieldStyle.fixedHeight), "Preprod", tPreprodLockAnalyze);
            bool tProdLockAnalyze = false;
            if (ProdSync >= 0)
            {
                tProdLockAnalyze = true;
            }
            if (tDisableProd == true)
            {
                tProdLockAnalyze = false;
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);
            bool tProdLock = EditorGUI.ToggleLeft(new Rect(tX + tWidthTiers + tWidthTiers, tY, tWidthTiers, tTextFieldStyle.fixedHeight), "Prod", tProdLockAnalyze);
            EditorGUI.EndDisabledGroup();
            if (tDevLockAnalyze != tDevLock)
            {
                if (tDevLock == false)
                {
                    if (DevSync == 0)
                    {
                        DevSync = -1;
                    }
                    else if (DevSync == 1)
                    {
                        DevSync = -2;
                    }
                    else
                    {
                        DevSync = -DevSync;
                    }
                }
                else
                {
                    if (DevSync == -1)
                    {
                        DevSync = 0;
                    }
                    else
                    {
                        DevSync = 1;
                    }
                }
                UpdateData();
                RepaintTableEditor();
            }
            if (tPreprodLockAnalyze != tPreprodLock)
            {
                if (tPreprodLock == false)
                {
                    if (PreprodSync == 0)
                    {
                        PreprodSync = -1;
                    }
                    else if (PreprodSync == 1)
                    {
                        PreprodSync = -2;
                    }
                    else
                    {
                        PreprodSync = -PreprodSync;
                    }
                }
                else
                {
                    if (PreprodSync == -1)
                    {
                        PreprodSync = 0;
                    }
                    else
                    {
                        PreprodSync = 1;
                    }
                }
                UpdateData();
                RepaintTableEditor();
            }
            if (tProdLockAnalyze != tProdLock)
            {
                if (tProdLock == false)
                {
                    if (ProdSync == 0)
                    {
                        ProdSync = -1;
                    }
                    else if (ProdSync == 1)
                    {
                        ProdSync = -2;
                    }
                    else
                    {
                        ProdSync = -ProdSync;
                    }
                }
                else
                {
                    if (ProdSync == -1)
                    {
                        ProdSync = 0;
                    }
                    else
                    {
                        ProdSync = 1;
                    }
                }
                UpdateData();
                RepaintTableEditor();
            }
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;



            //  Action Zone + Warning Zone Height
            float tBottomHeight = tBoldLabelStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 3
                                  + tMiniButtonStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 3
                                                 //+ tLabelStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 2
                                                 ;

            Rect tRectProperty = new Rect(0, tY, sInRect.width, sInRect.height - tY - tBottomHeight);
            EditorGUI.DrawRect(tRectProperty, NWDConstants.kPropertyColor);

            EditorGUI.DrawRect(new Rect(tX, tY, tWidth, 1), NWDConstants.kRowColorLine);

            EditorGUI.EndDisabledGroup();
            /*Rect tPropertyRect =*/
            DrawObjectInspector(tRectProperty, sWithScrollview, tCanBeEdit);


            EditorGUI.BeginDisabledGroup(tCanBeEdit == false);
            tY = sInRect.height - tBottomHeight;

            //            EditorGUI.indentLevel = 0;

            EditorGUI.DrawRect(new Rect(tX, tY, tWidth, 1), NWDConstants.kRowColorLine);
            EditorGUI.DrawRect(new Rect(tX, tY + 1, tWidth, tBottomHeight), NWDConstants.kIdentityColor);



            // Prepare Action and Warning zone 

            tY += NWDConstants.kFieldMarge; // Add marge 

            float tButtonWidth = (tWidth - (NWDConstants.kFieldMarge * 3)) / 4.0f;



            // Action Zone
            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTION_ZONE, tBoldLabelStyle);
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;


            if (GUI.Button(new Rect(tX, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_EDITOR_NODAL, tMiniButtonStyle))
            {
                NWDNodeEditor.SetObjectInNodeWindow(this);
            }

            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UPDATE, tMiniButtonStyle))
            {
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //UpdateObjectInListOfEdition(this);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }

            /*
            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY +tMiniButtonStyle.fixedHeight, tButtonWidth, tMiniButtonStyle.fixedHeight),
                           "UpdateData", tMiniButtonStyle))
            {
                this.UpdateData(true, NWDWritingMode.MainThread);
            }
            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY + (tMiniButtonStyle.fixedHeight)* 2, tButtonWidth, tMiniButtonStyle.fixedHeight),
                           "UpdateData pool", tMiniButtonStyle))
            {
                this.UpdateData(true,NWDWritingMode.PoolThread);
            }
            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY + (tMiniButtonStyle.fixedHeight)* 3, tButtonWidth, tMiniButtonStyle.fixedHeight),
                           "UpdateData Queue", tMiniButtonStyle))
            {
                this.UpdateData(true, NWDWritingMode.QueuedMainThread);
            }
            */


            if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 2, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DUPPLICATE, tMiniButtonStyle))
            {

                // todo not update if not modified
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                //K tNexObject = (K)DuplicateMe();
                NWDBasis<K> tNexObject = DuplicateData(true, NWDWritingMode.ByEditorDefault);
                //AddObjectInListOfEdition(tNexObject);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(tNexObject);
                if (Datas().m_SearchTag != NWDBasisTag.NoTag)
                {
                    tNexObject.Tag = Datas().m_SearchTag;
                    tNexObject.UpdateData();
                }
                SetObjectInEdition(tNexObject);
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }
            if (AC == false)
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, tMiniButtonStyle))
                {
                    EnableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }
            else
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVE, tMiniButtonStyle))
                {
                    DisableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }




            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.EndDisabledGroup();



            // Warning Zone




            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WARNING_ZONE, tBoldLabelStyle);
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            Color tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;

            if (GUI.Button(new Rect(tX, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_PUT_IN_TRASH, tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_PUT_IN_TRASH_WARNING,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_MESSAGE,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_OK,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_CANCEL))
                {
                    TrashData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }


            if (GUI.Button(new Rect(tX + tButtonWidth * 2 + NWDConstants.kFieldMarge * 2, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DELETE, tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_DELETE_WARNING,
                        NWDConstants.K_APP_BASIS_DELETE_MESSAGE,
                        NWDConstants.K_APP_BASIS_DELETE_OK,
                        NWDConstants.K_APP_BASIS_DELETE_CANCEL))
                {
                    //RemoveObjectInListOfEdition(this);
                    //DeleteMe();
                    DeleteData(NWDWritingMode.ByEditorDefault);
                    SetObjectInEdition(null);
                    //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    RepaintTableEditor();
                    NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            //tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX + tButtonWidth * 3 + NWDConstants.kFieldMarge * 3, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_REFERENCE, tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                {
                    RegenerateNewReference();
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            GUI.backgroundColor = tOldColor;

            //TODO Comment this line to hide the intergrity value 
            //EditorGUI.LabelField(new Rect(tX, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_VALUE, Integrity, tLabelStyle);
            //tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            //EditorGUI.LabelField(new Rect(tX, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_VALUE + " new", IntegrityValue(), tLabelStyle);
            //tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditor(Rect sInRect)
        {
            return 00.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditorHeight()
        {
            return 00.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool AddonEdited(bool sNeedBeUpdate)
        {
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ErrorCheck()
        {
            //  Debug.Log("NWDBasis ErrorCheck()");
            bool tNewValue = false;
            Type tType = ClassType();
            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataType tBTBDataType = tValue as BTBDataType;
                        if (tBTBDataType.IsInError() == true)
                        {
                            tNewValue = true;
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataTypeInt tBTBDataType = tValue as BTBDataTypeInt;
                        if (tBTBDataType.IsInError() == true)
                        {
                            tNewValue = true;
                        }
                    }
                }
            }

            if (AddonErrorFound() == true)
            {
                tNewValue = true;
            }

            if (InError != tNewValue)
            {
                InError = tNewValue;
                //UpdateIntegrity();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool AddonErrorFound()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif