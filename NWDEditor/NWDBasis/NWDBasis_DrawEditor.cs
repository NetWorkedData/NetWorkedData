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
#endif

using SQLite4Unity3d;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        static Editor mGameObjectEditor;
        //-------------------------------------------------------------------------------------------------------------
        static Type tLastTypeEdited;
        public static Vector2 m_ObjectEditorScrollPosition = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
        static Color kIdentityColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        static Color kPropertyColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        //-------------------------------------------------------------------------------------------------------------

        public static void SelectedFirstObjectInTable(EditorWindow sEditorWindow)
        {
            if (ObjectsInEditorTableList.Count > 0)
            {
                string tNextReference = ObjectsInEditorTableList.ElementAt(0);
                int tNextObjectIndex = ObjectsByReferenceList.IndexOf(tNextReference);
                SetObjectInEdition(ObjectsList.ElementAt(tNextObjectIndex));
                sEditorWindow.Focus();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO: rename PutObjectInInspector
        public static void SetObjectInEdition(object sObject, bool sResetStack = true, bool sFocus = true)
        {

            GUI.FocusControl(null);
            NWDDataInspector.InspectNetWorkedData(sObject, sResetStack, sFocus);
            if (sObject != null)
            {
                tLastTypeEdited = sObject.GetType();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(tLastTypeEdited);
            }
            else if (tLastTypeEdited != null)
            {
                // repaint all window?
                // or just last type?
                NWDDataManager.SharedInstance().RepaintWindowsInManager(tLastTypeEdited);
                tLastTypeEdited = null;
            }
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
        ////		public static Dictionary<int,bool> DrawableList = new Dictionary<int,bool>();
        //		public static List<bool> GroupLevelList = new List<bool>();
        //		public static List<bool> GroupOpenList = new List<bool>();
        //		//-------------------------------------------------------------------------------------------------------------
        //		public void ResetDrawable ()
        //		{
        ////			DrawableList = new Dictionary<int,bool>();
        //			DrawableList = new List<bool>();
        //		}
        //		//-------------------------------------------------------------------------------------------------------------
        //		public bool GetDrawable ()
        //		{
        ////			bool rReturn = true;
        ////			int tI = EditorGUI.indentLevel;
        ////			while (tI > 0) {
        ////				if (DrawableList.ContainsKey (tI)) {
        ////					if (DrawableList [tI] == false) {
        ////						rReturn = false;
        ////					}
        ////				}
        ////				tI--;
        ////			}
        ////			rReturn = true;
        ////			return rReturn;
        ////			return DrawableList.LastOrDefault<bool>(true);
        //
        //			bool rReturn = true;
        //			foreach (bool tV in DrawableList) {
        //				if (tV == false) {
        //					rReturn = false;
        //				}
        //			}
        //
        //			rReturn = true;
        //
        //			return rReturn;
        //		}
        //		//-------------------------------------------------------------------------------------------------------------
        //		public void AddDrawable (bool sValue)
        //		{
        ////			if (DrawableList.ContainsKey (EditorGUI.indentLevel)) {
        ////				DrawableList [EditorGUI.indentLevel] = sValue;
        ////			} else {
        ////				DrawableList.Add(EditorGUI.indentLevel,sValue);
        ////			}
        //			DrawableList.Add(sValue);
        //		}
        //		
        //-------------------------------------------------------------------------------------------------------------
        protected Texture2D PreviewTexture;
        protected bool PreviewTextureIsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D ReloadPreviewTexture2D()
        {
            PreviewTextureIsLoaded = true;
            PreviewTexture = null;
            UnityEngine.Object tObject = null;
            if (Preview != null && Preview != "")
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

            NWDConstants.LoadImages();
            NWDConstants.LoadStyles();

            float tY = 0;
            //EditorGUI.indentLevel = 0;
            //			ResetDrawable ();
            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), 100);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tColorFieldStyle = new GUIStyle(EditorStyles.colorField);
            tColorFieldStyle.fixedHeight = tColorFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tToggleStyle = new GUIStyle(EditorStyles.toggle);
            tToggleStyle.fixedHeight = tToggleStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tBoldFoldoutStyle.fontStyle = FontStyle.Bold;
            tBoldFoldoutStyle.fixedHeight = tBoldFoldoutStyle.CalcHeight(new GUIContent("A"), 100);

            Type tType = ClassType();

            bool tDraw = true;
            NWDGroupStartAttribute tActualGroupReference = null;

            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.Name != "ID" && tProp.Name != "Reference" && tProp.Name != "DC" && tProp.Name != "DM" &&
                    tProp.Name != "DD" && tProp.Name != "DS" && tProp.Name != "AC" && tProp.Name != "XX"
                    && tProp.Name != "Integrity" && tProp.Name != "InternalKey" && tProp.Name != "InternalDescription" && tProp.Name != "Preview"
                    && tProp.Name != "DevSync" && tProp.Name != "PreprodSync" && tProp.Name != "ProdSync" && tProp.Name != "Tag" && tProp.Name != "ServerHash" && tProp.Name != "ServerLog")
                {

                    foreach (NWDGroupEndAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupEndAttribute), true))
                    {
                        if (tActualGroupReference != null)
                        {
                            tActualGroupReference = tActualGroupReference.Parent;
                        }
                        if (tActualGroupReference != null)
                        {
                            tDraw = tActualGroupReference.IsDrawable(ClassName());
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
                            tActualDraw = tReference.GetDrawable(ClassName());
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
                            tReference.SetDrawable(ClassName(), tActualDraw);
                            tDraw = tReference.IsDrawable(ClassName());
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
                            //						if (tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true).Length > 0) {
                            tY += NWDConstants.kFieldMarge * 3;
                        }
                        foreach (NWDHeaderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDHeaderAttribute), true))
                        {
                            //						GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), tReference.mHeader, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }
                    // finish the foldout group management
                    // So Iif I nee dto draw somethings … 
                    if (tDraw)
                    {
                        //						if (tProp.GetCustomAttributes (typeof(NWDNotEditableAttribute), true).Length > 0) {
                        //							tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        //						} else 
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
                                tY += tTextFieldStyle.fixedHeight* NWDConstants.kLongString + NWDConstants.kFieldMarge;
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
                                else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int16) || tTypeOfThis == typeof(Int32) || tTypeOfThis == typeof(Int64))
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else if (tTypeOfThis == typeof(float) || tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Single) || tTypeOfThis == typeof(Double) || tTypeOfThis == typeof(Decimal))
                                {
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
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

                                    //									float tHeight = 0.0f;
                                    //									var tMethodInfo = tTypeOfThis.GetMethod ("ControlFieldHeight", BindingFlags.Public | BindingFlags.Instance);
                                    //									if (tMethodInfo != null) {
                                    //										string tHeightString = tMethodInfo.Invoke (tValue, null) as string;
                                    //										float.TryParse (tHeightString, out tHeight);
                                    //									}
                                    //									tY += tHeight + NWDConstants.kFieldMarge;
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

            NWDConstants.LoadImages();
            NWDConstants.LoadStyles();

            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tColorFieldStyle = new GUIStyle(EditorStyles.colorField);
            tColorFieldStyle.fixedHeight = tColorFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tToggleStyle = new GUIStyle(EditorStyles.toggle);
            tToggleStyle.fixedHeight = tToggleStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tBoldFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tBoldFoldoutStyle.fontStyle = FontStyle.Bold;
            tBoldFoldoutStyle.fixedHeight = tBoldFoldoutStyle.CalcHeight(new GUIContent("A"), tWidth);


            bool rNeedBeUpdate = false;
            if (sWithScrollview == true)
            {
                float tScrollBarMarge = 0;
                float tHeightContent = DrawObjectInspectorHeight();
                if (tHeightContent >= sInRect.height)
                {
                    tScrollBarMarge = 20.0f;
                }
                NWDBasis<K>.m_ObjectEditorScrollPosition = GUI.BeginScrollView(sInRect, NWDBasis<K>.m_ObjectEditorScrollPosition, new Rect(0, 0, sInRect.width - tScrollBarMarge, tHeightContent));

                tWidth = sInRect.width - tScrollBarMarge - NWDConstants.kFieldMarge * 2;
                tX = NWDConstants.kFieldMarge;
                tY = NWDConstants.kFieldMarge;
            }

            bool tDraw = true;
            NWDGroupStartAttribute tActualGroupReference = null;
            Type tType = ClassType();

            EditorGUI.BeginDisabledGroup(sEditionEnable == false);

            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.Name != "ID" && tProp.Name != "Reference" && tProp.Name != "DC" && tProp.Name != "DM" &&
                    tProp.Name != "DD" && tProp.Name != "DS" && tProp.Name != "AC" && tProp.Name != "XX"
                    && tProp.Name != "Integrity" && tProp.Name != "InternalKey" && tProp.Name != "InternalDescription" && tProp.Name != "Preview"
                    && tProp.Name != "DevSync" && tProp.Name != "PreprodSync" && tProp.Name != "ProdSync" && tProp.Name != "Tag" && tProp.Name != "ServerHash" && tProp.Name != "ServerLog")
                {

                    foreach (NWDGroupEndAttribute tReference in tProp.GetCustomAttributes(typeof(NWDGroupEndAttribute), true))
                    {
                        if (tActualGroupReference != null)
                        {
                            tActualGroupReference = tActualGroupReference.Parent;
                        }
                        if (tActualGroupReference != null)
                        {
                            tDraw = tActualGroupReference.IsDrawable(ClassName());
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
                        //						if (tProp.GetCustomAttributes (typeof(NWDSeparatorAttribute), true).Length > 0) {
                        EditorGUI.DrawRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1), kRowColorLine);
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
                            tActualDraw = tReference.GetDrawable(ClassName());
                            if (tReducible == true)
                            {
                                if (tBold == true)
                                {
                                    tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tBoldFoldoutStyle.fixedHeight), tActualDraw, tReference.mGroupName, tBoldFoldoutStyle);
                                    tY += tBoldFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
                                }
                                else
                                {
                                    tActualDraw = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tFoldoutStyle.fixedHeight), tActualDraw, tReference.mGroupName, tFoldoutStyle);
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
                            tReference.SetDrawable(ClassName(), tActualDraw);
                            tDraw = tReference.IsDrawable(ClassName());
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
                            EditorGUI.DrawRect(tRect, kRowColorLine);
                            tY += NWDConstants.kFieldMarge * 2;
                        }
                        // draw space
                        foreach (NWDSpaceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDSpaceAttribute), true))
                        {
                            //						if (tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true).Length > 0) {
                            //						NWDSpaceAttribute tReference = (NWDSpaceAttribute)tProp.GetCustomAttributes (typeof(NWDSpaceAttribute), true) [0];
                            tY += NWDConstants.kFieldMarge * 3;
                        }
                        foreach (NWDHeaderAttribute tReference in tProp.GetCustomAttributes(typeof(NWDHeaderAttribute), true))
                        {
                            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), tReference.mHeader, tBoldLabelStyle);
                            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        }
                    }

                    // So Iif I nee dto draw somethings … 
                    if (tDraw)
                    {

                        // get entitled and toolstips
                        string tEntitled = NWDToolbox.SplitCamelCase(tProp.Name);
                        string tToolsTips = "";
                        if (tProp.GetCustomAttributes(typeof(NWDEntitledAttribute), true).Length > 0)
                        {
                            NWDEntitledAttribute tReference = (NWDEntitledAttribute)tProp.GetCustomAttributes(typeof(NWDEntitledAttribute), true)[0];
                            tEntitled = tReference.Entitled;
                            tToolsTips = tReference.ToolsTips;
                        }

                        if (tProp.GetCustomAttributes(typeof(NWDTooltipsAttribute), true).Length > 0)
                        {
                            NWDTooltipsAttribute tReference = (NWDTooltipsAttribute)tProp.GetCustomAttributes(typeof(NWDTooltipsAttribute), true)[0];
                            tToolsTips = tReference.ToolsTips;
                        }

                        // if is enable 
                        //						if (tProp.GetCustomAttributes (typeof(NWDNotEditableAttribute), true).Length > 0) {
                        ////							string tValue = tProp.GetValue (this, null) as string;
                        ////							EditorGUI.LabelField (new Rect (tX, tY, tWidth, tLabelStyle.fixedHeight), tEntitled, tValue, tLabelStyle);
                        ////							tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                        //
                        //
                        //						} 
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

                                //tToolsTips = tReference.ToolsTips;
                            }
                        }
                        EditorGUI.BeginDisabledGroup(tDisabled);
                        //else 
                        {
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
                                string tValueNext = EditorGUI.TextArea(new Rect(tX+ EditorGUIUtility.labelWidth, tY, tWidth-EditorGUIUtility.labelWidth, tTextFieldStyle.fixedHeight*NWDConstants.kLongString),tValue, NWDConstants.kTextAreaStyle);
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

                                int tValueIntNext = EditorGUI.Popup(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth -EditorGUIUtility.labelWidth, tPopupdStyle.fixedHeight), "", tValueInt, tV, tPopupdStyle);
                                tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                                string tValueNext = "";
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

                                int tValueIntNext = EditorGUI.Popup(new Rect(tX+EditorGUIUtility.labelWidth, tY, tWidth-EditorGUIUtility.labelWidth, tPopupdStyle.fixedHeight), "", tValueInt, tV, tPopupdStyle);
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
                                    Enum tValueNext = EditorGUI.EnumPopup(new Rect(tX, tY, tWidth, tPopupdStyle.fixedHeight), tContent, tValue, tPopupdStyle);
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
                                else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int16) || tTypeOfThis == typeof(Int32) || tTypeOfThis == typeof(Int64))
                                {
                                    int tValue = (int)tProp.GetValue(this, null);
                                    int tValueNext = EditorGUI.IntField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tTextFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    if (tValueNext != tValue)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(float) || tTypeOfThis == typeof(Single) || tTypeOfThis == typeof(Decimal))
                                {
                                    float tValue = (float)tProp.GetValue(this, null);
                                    float tValueNext = EditorGUI.FloatField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), tContent, tValue, tTextFieldStyle);
                                    tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                                    float EPSILON = 0;
                                    if (Math.Abs(tValueNext - tValue) > EPSILON)
                                    {
                                        tProp.SetValue(this, tValueNext, null);
                                        rNeedBeUpdate = true;
                                    }
                                }
                                else if (tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Double))
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
                        }

                        EditorGUI.EndDisabledGroup();
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

            if (AddonEdited(rNeedBeUpdate) == true)
            {
                rNeedBeUpdate = true;
            }
            if (rNeedBeUpdate == true)
            {
                NWDDataInspector.ActiveRepaint();
                if (sEditionEnable == true)
                {

                    if (IntegrityValue() != this.Integrity)
                    {
                        DM = NWDToolbox.Timestamp();
                        UpdateIntegrity();
                        UpdateMeLater();
                        // TODO Imagine an timer to refresh if modified
                        //NWDNodeEditor.UpdateNodeWindow(this);
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            Rect tFinalRect = new Rect(sInRect.position.x, tY, sInRect.width, tY - sInRect.position.y);
            return tFinalRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool kSyncAndMoreInformations = false;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawObjectEditor(Rect sInRect, bool sWithScrollview)
        {

            //			float tWidth = EditorGUIUtility.currentViewWidth;
            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            //			float tHeight = sInRect.height-tMarge*2;
            float tX = NWDConstants.kFieldMarge;
            float tY = NWDConstants.kFieldMarge;
            //			int tRow = 0;

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
            tMiniLabelStyle.fixedHeight = tMiniLabelStyle.CalcHeight(new GUIContent("A"), tWidth);


            GUIStyle tFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            tFoldoutStyle.fixedHeight = tFoldoutStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tBoldLabelStyle.fixedHeight = tBoldLabelStyle.CalcHeight(new GUIContent("A"), tWidth);


            GUIStyle tTitleLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tTitleLabelStyle.alignment = TextAnchor.MiddleCenter;
            tTitleLabelStyle.fontSize = 14;
            tTitleLabelStyle.fixedHeight = tTitleLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tHelpBoxStyle = new GUIStyle(EditorStyles.helpBox);
            tHelpBoxStyle.fixedHeight = tHelpBoxStyle.CalcHeight(new GUIContent("A\nA\nA"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            bool tCanBeEdit = true;
            bool tTestIntegrity = TestIntegrity();

            //
            //
            //
            //GUI.SetNextControlName(NWDConstants.K_CLASS_FOCUS_ID);
            //			GUI.Label (new Rect (tX, tY, tWidth, tTitleLabelStyle.fixedHeight), ClassNamePHP () + "'s Object", tTitleLabelStyle);
            string tTitle = InternalKey;
            if (tTitle == "")
            {
                tTitle = "Unamed " + ClassNamePHP() + "";
                //				tTitle = ClassNamePHP () + "'s Object";
            }
            GUI.Label(new Rect(tX, tY, tWidth, tTitleLabelStyle.fixedHeight), tTitle, tTitleLabelStyle);

            // add button to navigate next / preview
            if (NWDDataInspector.InspectNetWorkedPreview())
            {
                if (GUI.Button(new Rect(tX, tY + 10, 20, 20), "<"))
                {
                    NWDDataInspector.InspectNetWorkedDataPreview();
                }
            }

            if (NWDDataInspector.InspectNetWorkedNext())
            {
                if (GUI.Button(new Rect(tX + tWidth - 20, tY + 10, 20, 20), ">"))
                {
                    NWDDataInspector.InspectNetWorkedDataNext();
                }
            }
            tY += tTitleLabelStyle.fixedHeight + NWDConstants.kFieldMarge * 2;

            Texture2D tTextureOfClass = TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUI.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
            }
            tY += 32 + NWDConstants.kFieldMarge;


            //			EditorGUI.BeginDisabledGroup (true);
            //			EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), ClassNamePHP () , this,  typeof(NWDBasis<K>), false);
            //			tY += tObjectFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            //			EditorGUI.EndDisabledGroup ();
            //
            //			GUI.Label (new Rect (tX,tY,tWidth,tBoldLabelStyle.fixedHeight),"Test de font BBBB",tBoldLabelStyle);
            //			tY+=tBoldLabelStyle.fixedHeight + tMarge;
            //if (tTestIntegrity == false || XX > 0) {tCanBeEdit = false;}EditorGUI.BeginDisabledGroup (tCanBeEdit == false);EditorGUI.EndDisabledGroup ();

            if (tTestIntegrity == false)
            {

                EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), kRowColorError);
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
                        UpdateMe(true);
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    }
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                tY += NWDConstants.kFieldMarge;

            }
            else if (XX > 0)
            {

                EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), kRowColorError);
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
                        UnTrashMe();
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


            EditorGUI.DrawRect(new Rect(0, tY - NWDConstants.kFieldMarge, sInRect.width, sInRect.height), kIdentityColor);

            tY += NWDConstants.kFieldMarge;

            //GUI.Label (new Rect (NWDConstants.kFieldMarge, tY, tImageWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PREVIEW, tMiniLabelStyle);

            UnityEngine.Object tObject = null;
            if (Preview != null && Preview != "")
            {
                tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(UnityEngine.Object)) as UnityEngine.Object;
            }
            //Texture2D tTexture2D = null;

            //if (tObject != null)
            //{
            //    //				if (mGameObjectEditor == null) {
            //    //					mGameObjectEditor = Editor.CreateEditor (tObject);
            //    //				}
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


            DrawPreviewTexture2D(new Rect(NWDConstants.kFieldMarge, tY +tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge, tImageWidth, tImageWidth));

            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_EDITOR_NODAL))
            {
                NWDNodeEditor.SetObjectInNodeWindow(this);
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            //			GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INFORMATIONS, tBoldLabelStyle);
            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), ClassNamePHP() + "'s Object", tBoldLabelStyle);
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE + Reference, tMiniLabelStyle);
            tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DC + NWDToolbox.TimeStampToDateTime(DC).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
            tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DM + NWDToolbox.TimeStampToDateTime(DM).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
            tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;



            kSyncAndMoreInformations = EditorGUI.Foldout(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), kSyncAndMoreInformations, NWDConstants.K_APP_BASIS_Sync + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), tFoldoutStyle);
            tY += tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (kSyncAndMoreInformations)
            {
                tX = NWDConstants.kFieldMarge;
                tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_Sync + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DevSync + NWDToolbox.TimeStampToDateTime(DevSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PreprodSync + NWDToolbox.TimeStampToDateTime(PreprodSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ProdSync + NWDToolbox.TimeStampToDateTime(ProdSync).ToString("yyyy/MM/dd HH:mm:s")
                            + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_xx + XX.ToString(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ac + AC.ToString(), tMiniLabelStyle);
                tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            }


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
//			tStyle.alignment = TextAnchor.MiddleLeft;
			if (AC == false) {
//				GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVED, tBoldLabelStyle);
//				tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
//				GUI.Label (new Rect (tX, tY, tWidth, tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INACTIVED + NWDToolbox.TimeStampToDateTime (DD).ToString ("G"), tMiniLabelStyle);
//				tY += tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
				if (GUI.Button (new Rect (tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, tMiniButtonStyle)) {
					EnableMe ();
					NWDDataManager.SharedInstance().RepaintWindowsInManager (this.GetType ());
				}
				tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			} else {
//				GUI.Label (new Rect (tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTIVE, tBoldLabelStyle);
//				tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
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

            string tPreFabGameObject = "";
            if (pObj != null)
            {
                //if (PrefabUtility.GetPrefabType(pObj) == PrefabType.Prefab)
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
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
            }


            bool tInternalKeyEditable = true;
            //			NWDInternalKeyNotEditable

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
                    UpdateObjectInListOfEdition(this);
                    NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
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
                UpdateObjectInListOfEdition(this);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }


            NWDBasisTag tInternalTag = (NWDBasisTag)EditorGUI.EnumPopup(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_TAG, (NWDBasisTag)Tag);
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if ((int)tInternalTag != Tag)
            {
                Tag = (int)tInternalTag;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateObjectInListOfEdition(this);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }



            float tBottomHeight = tBoldLabelStyle.fixedHeight * 2 + tMiniButtonStyle.fixedHeight * 3 + tLabelStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 7;

            Rect tRectProperty = new Rect(0, tY, sInRect.width, sInRect.height - tY - tBottomHeight);
            EditorGUI.DrawRect(tRectProperty, kPropertyColor);

            EditorGUI.DrawRect(new Rect(tX, tY, tWidth, 1), kRowColorLine);

            EditorGUI.EndDisabledGroup();
            /*Rect tPropertyRect =*/
            DrawObjectInspector(tRectProperty, sWithScrollview, tCanBeEdit);


            EditorGUI.BeginDisabledGroup(tCanBeEdit == false);
            tY = sInRect.height - tBottomHeight;

            //			EditorGUI.indentLevel = 0;

            EditorGUI.DrawRect(new Rect(tX, tY, tWidth, 1), kRowColorLine);
            EditorGUI.DrawRect(new Rect(tX, tY + 1, tWidth, tBottomHeight), kIdentityColor);


            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTION_ZONE, tBoldLabelStyle);
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            float tButtonWidth = (tWidth - (NWDConstants.kFieldMarge * 3)) / 4.0f;


            Color tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;

            if (GUI.Button(new Rect(tX, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_PUT_IN_TRASH, tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_PUT_IN_TRASH_WARNING,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_MESSAGE,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_OK,
                        NWDConstants.K_APP_BASIS_PUT_IN_TRASH_CANCEL))
                {
                    TrashMe();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }
            GUI.backgroundColor = tOldColor;

            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UPDATE, tMiniButtonStyle))
            {
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateObjectInListOfEdition(this);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                NWDDataManager.SharedInstance().UpdateQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }
            if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 2, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DUPPLICATE, tMiniButtonStyle))
            {
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                K tNexObject = (K)DuplicateMe();
                AddObjectInListOfEdition(tNexObject);
                NWDDataManager.SharedInstance().AddObjectToUpdateQueue(tNexObject);
                SetObjectInEdition(tNexObject);
                m_PageSelected = m_MaxPage * 3;
                NWDDataManager.SharedInstance().UpdateQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }
            if (AC == false)
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, tMiniButtonStyle))
                {
                    EnableMe();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }
            else
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVE, tMiniButtonStyle))
                {
                    DisableMe();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }




            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            GUI.Label(new Rect(tX, tY, tWidth, tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WARNING_ZONE, tBoldLabelStyle);
            tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            tY += tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DELETE, tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_DELETE_WARNING,
                        NWDConstants.K_APP_BASIS_DELETE_MESSAGE,
                        NWDConstants.K_APP_BASIS_DELETE_OK,
                        NWDConstants.K_APP_BASIS_DELETE_CANCEL))
                {
                    RemoveObjectInListOfEdition(this);
                    DeleteMe();
                    SetObjectInEdition(null);
                    //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    RepaintTableEditor();
                    NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_REFERENCE, tMiniButtonStyle))
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


            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_VALUE, Integrity, tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_VALUE + " new", IntegrityValue(), tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

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
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================