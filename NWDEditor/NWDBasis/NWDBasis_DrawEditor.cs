//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        protected Texture2D PreviewTexture;
        protected UnityEngine.Object PreviewObject = null;
        protected bool PreviewTextureIsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectedFirstObjectInTable(EditorWindow sEditorWindow)
        {
            if (BasisHelper().EditorTableDatas.Count > 0)
            {
                K sObject = BasisHelper().EditorTableDatas.ElementAt(0) as K;
                //int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                SetObjectInEdition(sObject);
                sEditorWindow.Focus();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_LAST_TYPE_KEY = "K_EDITOR_LAST_TYPE_KEY_5fdshjktr";
        public const string K_EDITOR_LAST_REFERENCE_KEY = "K_EDITOR_LAST_REFERENCE_KEY_ed5f5dtr";
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_RestaureObjectInEdition)]
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
                if (sClassPHP == BasisHelper().ClassNamePHP)
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
            NWDBasis<K> tObject = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
            if (tObject == null)
            {
                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, string.Empty);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, string.Empty);
            }
            else
            {

                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, NWDBasisHelper.FindTypeInfos(tObject.GetType()).ClassNamePHP);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, tObject.Reference);
            }
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
                NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                NWDBasisEditor.ObjectEditorLastType = null;
            }
            SaveObjectInEdition();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsObjectInEdition(object sObject)
        {
            bool rReturn = false;
            if (NWDDataInspector.ObjectInEdition() == sObject)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D ReloadPreviewTexture2D()
        {

            PreviewTextureIsLoaded = true;
            PreviewTexture = null;
            PreviewObject = null;
            if (string.IsNullOrEmpty(Preview) == false)
            {
                if (PreviewObject is GameObject)
                {
                    Debug.Log("ReloadPreviewTexture2D use gameobject");
                    ((GameObject)PreviewObject).transform.localRotation = Quaternion.Euler(-180,-180,-180);
                }
                    PreviewObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(UnityEngine.Object)) as UnityEngine.Object;
                if (PreviewObject != null)
                {
                    PreviewTexture = AssetPreview.GetAssetPreview(PreviewObject);
                }
            }
            return AssetPreview.GetAssetPreview(PreviewObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D GetPreviewTexture2D()
        {
            if (PreviewTextureIsLoaded == false)
            {
                ReloadPreviewTexture2D();
            }
            return AssetPreview.GetAssetPreview(PreviewObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Vector2 sOrigin)
        {
            DrawPreviewTexture2D(new Rect(sOrigin.x, sOrigin.y, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Rect sRect)
        {
            Texture2D tTexture = GetPreviewTexture2D();
            if (tTexture != null)
            {
                EditorGUI.DrawPreviewTexture(sRect, tTexture);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public float DrawObjectInspectorHeight()
        {
            NWDConstants.LoadStyles();
            float tY = 0;
            Type tType = ClassType();
            tY += NewDrawObjectInspectorHeight();
            tY += NWDConstants.kFieldMarge;
            tY += AddonEditorHeight();
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect DrawObjectInspector(Rect sInRect, bool sWithScrollview, bool sEditionEnable)
        {
            NWDConstants.LoadStyles();

            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;
            bool rNeedBeUpdate = false;
            EditorGUI.BeginChangeCheck();

            // Start scrollview
            if (sWithScrollview == true)
            {
                float tScrollBarMarge = 0;
                float tHeightContent = DrawObjectInspectorHeight();
                if (tHeightContent >= sInRect.height)
                {
                    tScrollBarMarge = 20.0f;
                }
                BasisHelper().ObjectEditorScrollPosition = GUI.BeginScrollView(sInRect, BasisHelper().ObjectEditorScrollPosition, new Rect(0, 0, sInRect.width - tScrollBarMarge, tHeightContent));

                tWidth = sInRect.width - tScrollBarMarge - NWDConstants.kFieldMarge * 2;
                tX = NWDConstants.kFieldMarge;
                tY = NWDConstants.kFieldMarge;
            }
            EditorGUI.BeginDisabledGroup(sEditionEnable == false);
            NewDrawObjectInspector(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), sEditionEnable);
            EditorGUI.EndDisabledGroup();
            // finish scrollview 
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
                if (sEditionEnable == true)
                {
                    ErrorCheck();
                    WebserviceVersionCheckMe();
                    if (IntegrityValue() != this.Integrity)
                    {
                        UpdateData(true, NWDWritingMode.ByEditorDefault);
                    }
                }
            }
            Rect tFinalRect = new Rect(sInRect.position.x, tY, sInRect.width, tY - sInRect.position.y);
            return tFinalRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        Editor gameObjectEditor;
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_DrawObjectEditor)]
        public void DrawObjectEditor(Rect sInRect, bool sWithScrollview)
        {
            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = NWDConstants.kFieldMarge;
            float tY = NWDConstants.kFieldMarge;

            bool tCanBeEdit = true;
            bool tTestIntegrity = TestIntegrity();
            //Draw Internal Key
            string tTitle = InternalKey;

            if (BasisHelper().WebModelChanged == true)
            {
                string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL + "</color></b>";
                GUIContent tCC = new GUIContent(tTEXTWARNING);
                NWDConstants.tWarningBoxStyle.fixedHeight = NWDConstants.tWarningBoxStyle.CalcHeight(tCC, tWidth);
                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tWarningBoxStyle.fixedHeight), tCC, NWDConstants.tWarningBoxStyle);
                tY += NWDConstants.tWarningBoxStyle.fixedHeight + NWDConstants.kFieldMarge;
            }

            if (BasisHelper().WebModelDegraded == true)
            {
                string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED + "</color></b>";
                GUIContent tCC = new GUIContent(tTEXTWARNING);
                NWDConstants.tWarningBoxStyle.fixedHeight = NWDConstants.tWarningBoxStyle.CalcHeight(tCC, tWidth);
                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tWarningBoxStyle.fixedHeight), tCC, NWDConstants.tWarningBoxStyle);
                tY += NWDConstants.tWarningBoxStyle.fixedHeight + NWDConstants.kFieldMarge;
            }
            if (string.IsNullOrEmpty(tTitle))
            {
                tTitle = "Unamed " + BasisHelper().ClassNamePHP + string.Empty;
            }
            if (InError == true)
            {
                tTitle = "<b><color=red>" + NWDConstants.K_WARNING + tTitle + "</color></b>";
            }
            GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tTitleLabelStyle.fixedHeight), tTitle, NWDConstants.tTitleLabelStyle);

            // Draw reference
            GUI.Label(new Rect(tX, tY + NWDConstants.tTitleLabelStyle.fixedHeight - NWDConstants.kFieldMarge, tWidth, NWDConstants.tTitleLabelStyle.fixedHeight), Reference, NWDConstants.tMiniLabelStyleCenter);

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
            tY += NWDConstants.tTitleLabelStyle.fixedHeight + NWDConstants.kFieldMarge * 2;

            Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUI.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
            }
            tY += 32 + NWDConstants.kFieldMarge;
            if (WebserviceVersionIsValid())
            {
                if (tTestIntegrity == false)
                {

                    EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorError);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_IS_FALSE, NWDConstants.tBoldLabelStyle);
                    tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tX, tY, tWidth, NWDConstants.tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_HELPBOX, MessageType.Error);
                    tY += NWDConstants.tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                    if (GUI.Button(new Rect(tX, tY, tWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_REEVAL, NWDConstants.tMiniButtonStyle))
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
                    tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                    tY += NWDConstants.kFieldMarge;

                }
                else if (XX > 0)
                {

                    EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorTrash);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH, NWDConstants.tBoldLabelStyle);
                    tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tX, tY, tWidth, NWDConstants.tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH_HELPBOX, MessageType.Warning);
                    tY += NWDConstants.tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                    if (GUI.Button(new Rect(tX, tY, tWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UNTRASH, NWDConstants.tMiniButtonStyle))
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
                    tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                    tY += NWDConstants.kFieldMarge;
                }
            }
            else
            {
                EditorGUI.DrawRect(new Rect(0, 0, sInRect.width, sInRect.height), NWDConstants.kRowColorWarning);
                tCanBeEdit = false;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR, NWDConstants.tBoldLabelStyle);
                tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                EditorGUI.HelpBox(new Rect(tX, tY, tWidth, NWDConstants.tHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_HELPBOX, MessageType.Warning);
                tY += NWDConstants.tHelpBoxStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (GUI.Button(new Rect(tX, tY, tWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_FIX, NWDConstants.tMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING_MESSAGE,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_OK,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_CANCEL
                        ))
                    {
                        UpdateData();
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                    }
                }
                tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                tY += NWDConstants.kFieldMarge;
            }
            float tImageWidth = (NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge) * 3;

            tX = tImageWidth + NWDConstants.kFieldMarge * 2;
            tWidth = sInRect.width - NWDConstants.kFieldMarge * 3 - tImageWidth;

            EditorGUI.BeginDisabledGroup(tCanBeEdit == false);


            EditorGUI.DrawRect(new Rect(0, tY - NWDConstants.kFieldMarge, sInRect.width, sInRect.height), NWDConstants.kIdentityColor);

            tY += NWDConstants.kFieldMarge;
            Texture2D tTexture2D = GetPreviewTexture2D();
            if (tTexture2D != null)
            {
               EditorGUI.DrawPreviewTexture(new Rect(NWDConstants.kFieldMarge, tY, tImageWidth, tImageWidth), tTexture2D);
                //if (PreviewObject != null)
                //{
                //    //if (gameObjectEditor == null)
                //    //{
                //        gameObjectEditor = Editor.CreateEditor(PreviewObject);
                //    //}
                //    //else
                //    //{
                //    //    //gameObjectEditor.
                //    //}
                //    gameObjectEditor.OnInteractivePreviewGUI(new Rect(NWDConstants.kFieldMarge, tY, tImageWidth, tImageWidth), EditorStyles.whiteLabel);
                //}
            }

            GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), BasisHelper().ClassNamePHP + "'s Object", NWDConstants.tBoldLabelStyle);
            tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DM + NWDToolbox.TimeStampToDateTime(DM).ToString("yyyy/MM/dd HH:mm:ss"), NWDConstants.tMiniLabelStyle);
            tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            BasisHelper().kSyncAndMoreInformations = EditorGUI.Foldout(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), BasisHelper().kSyncAndMoreInformations, NWDConstants.K_APP_BASIS_INFORMATIONS, NWDConstants.tFoldoutStyle);
            tY += NWDConstants.tFoldoutStyle.fixedHeight + NWDConstants.kFieldMarge;

            tY += NWDConstants.kFieldMarge;

            if (BasisHelper().kSyncAndMoreInformations)
            {

                EditorGUI.EndDisabledGroup();


                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DC + "(" + DC.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DC).ToString("yyyy/MM/dd HH:mm:ss"), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_Sync + "(" + DS.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DevSync + "(" + DevSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DevSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PreprodSync + "(" + PreprodSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(PreprodSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ProdSync + "(" + ProdSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(ProdSync).ToString("yyyy/MM/dd HH:mm:s")
                            + " (last sync request " + NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_xx + XX.ToString(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ac + AC.ToString(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;


                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "server log ", ServerLog, NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "server hash ", ServerHash, NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "integrity val", Integrity, NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "dyn integrity seq", IntegrityAssembly(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "head integrity Csv", CSVAssemblyHead(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "dyn integrity Csv", CSVAssembly(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tMiniLabelStyle.fixedHeight), "dyn integrity req", IntegrityValue(), NWDConstants.tMiniLabelStyle);
                tY += NWDConstants.tMiniLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                EditorGUI.BeginDisabledGroup(tCanBeEdit == false);
            }

            tX = NWDConstants.kFieldMarge;

            tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;

            UnityEngine.Object pObj = EditorGUI.ObjectField(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight),
                                          NWDConstants.K_APP_BASIS_PREVIEW_GAMEOBJECT,
                                                            PreviewObject, typeof(UnityEngine.Object), false);
            tY += NWDConstants.tObjectFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            string tPreFabGameObject = string.Empty;
            if (pObj != null)
            {
                tPreFabGameObject = AssetDatabase.GetAssetPath(pObj);
            }

            if (Preview != tPreFabGameObject)
            {
                Preview = tPreFabGameObject;
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                ReloadPreviewTexture2D();
                RowAnalyze();
                RepaintTableEditor();
                NWDNodeEditor.ReAnalyzeIfNecessary(this);
            }

            bool tInternalKeyEditable = true;

            if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
            {
                tInternalKeyEditable = false;
            }

            if (tInternalKeyEditable == true)
            {
                string tInternalNameActual = NWDToolbox.TextUnprotect(InternalKey);
                string tInternalName = EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, tInternalNameActual, NWDConstants.tTextFieldStyle);
                tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (tInternalName != InternalKey)
                {
                    tInternalName = NWDToolbox.TextProtect(tInternalName);
                    InternalKey = tInternalName;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    UpdateData(true, NWDWritingMode.ByEditorDefault);
                    RowAnalyze();
                    RepaintTableEditor();
                    NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, InternalKey, NWDConstants.tTextFieldStyle);
                tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                EditorGUI.EndDisabledGroup();
            }

            string tInternalDescriptionActual = NWDToolbox.TextUnprotect(InternalDescription);
            string tInternalDescription = EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_DESCRIPTION, tInternalDescriptionActual, NWDConstants.tTextFieldStyle);
            tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (tInternalDescription != InternalDescription)
            {
                tInternalDescription = NWDToolbox.TextProtect(tInternalDescription);
                InternalDescription = tInternalDescription;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                RowAnalyze();
                RepaintTableEditor();
                NWDNodeEditor.ReAnalyzeIfNecessary(this);
            }

            if (tCanBeEdit == true)
            {
                // Web Service Version management
                NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                int tWebBuilt = tApp.WebBuild;
                List<int> tWebServicesInt = new List<int>();
                List<string> tWebServicesString = new List<string>();
                tWebServicesInt.Add(0);

                foreach (KeyValuePair<int, bool> tKeyValue in tApp.WSList)
                //for (int tW = 0; tW <= tWebBuilt; tW++)
                {
                    int tWebModel = 0;
                    if (tKeyValue.Value == true)
                    {
                        if (BasisHelper().WebServiceWebModel.ContainsKey(tKeyValue.Key))
                        {
                            tWebModel = BasisHelper().WebServiceWebModel[tKeyValue.Key];
                            if (tWebServicesInt.Contains(tWebModel) == false)
                            {
                                tWebServicesInt.Add(tWebModel);
                            }
                        }
                    }
                }
                tWebServicesInt.Sort();

                foreach (int tWebMOdel in tWebServicesInt)
                {
                    tWebServicesString.Add("WebModel " + tWebMOdel.ToString("0000"));
                }

                int tWebServiceVersionOldIndex = tWebServicesInt.IndexOf(WebModel);
                if (tWebServiceVersionOldIndex < 0)
                {
                    if (tWebServicesInt.Count > 0)
                    {
                        tWebServiceVersionOldIndex = 0;
                    }
                }

                int tWebServiceVersionIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight),
                                                                 "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")",
                                                                 tWebServiceVersionOldIndex, tWebServicesString.ToArray());
                tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                int tWebServiceVersionNew = 0;
                if (tWebServicesInt.Count > 0)
                {
                    tWebServiceVersionNew = tWebServicesInt[tWebServiceVersionIndex];
                }
                if (WebModel != tWebServiceVersionNew)
                {

                    // Debug.Log(" set from " + WebModel + " To " + tWebServiceVersionNew);
                    WebModel = tWebServiceVersionNew;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    //UpdateObjectInListOfEdition(this);
                    UpdateData(true, NWDWritingMode.ByEditorDefault, false);
                    RowAnalyze();
                    RepaintTableEditor();
                    NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            else
            {
                NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                int tWebBuilt = tApp.WebBuild;
                EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight), "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")", WebModel.ToString());
                tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            }

            // Tag management
            List<int> tTagIntList = new List<int>();
            List<string> tTagStringList = new List<string>();
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                tTagIntList.Add(tTag.Key);
                tTagStringList.Add(tTag.Value);
            }

            NWDBasisTag tInternalTag = (NWDBasisTag)EditorGUI.IntPopup(new Rect(tX, tY, tWidth, NWDConstants.tTextFieldStyle.fixedHeight),
                                                                        NWDConstants.K_APP_BASIS_INTERNAL_TAG,
                                                                       (int)Tag,
                                                                       tTagStringList.ToArray(), tTagIntList.ToArray());
            tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (tInternalTag != Tag)
            {
                Tag = tInternalTag;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                RowAnalyze();
                RepaintTableEditor();
                NWDNodeEditor.ReAnalyzeIfNecessary(this);
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
            bool tDevLock = EditorGUI.ToggleLeft(new Rect(tX, tY, tWidthTiers, NWDConstants.tTextFieldStyle.fixedHeight), "Dev", tDevLockAnalyze);
            bool tPreprodLockAnalyze = false;
            if (PreprodSync >= 0)
            {
                tPreprodLockAnalyze = true;
            }
            bool tPreprodLock = EditorGUI.ToggleLeft(new Rect(tX + tWidthTiers, tY, tWidthTiers, NWDConstants.tTextFieldStyle.fixedHeight), "Preprod", tPreprodLockAnalyze);
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
            bool tProdLock = EditorGUI.ToggleLeft(new Rect(tX + tWidthTiers + tWidthTiers, tY, tWidthTiers, NWDConstants.tTextFieldStyle.fixedHeight), "Prod", tProdLockAnalyze);
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
            tY += NWDConstants.tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;



            //  Action Zone + Warning Zone Height
            float tBottomHeight = NWDConstants.tBoldLabelStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 3
                                  + NWDConstants.tMiniButtonStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 3
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
            GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTION_ZONE, NWDConstants.tBoldLabelStyle);
            tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;


            if (GUI.Button(new Rect(tX, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_EDITOR_NODAL, NWDConstants.tMiniButtonStyle))
            {
                NWDNodeEditor.SetObjectInNodeWindow(this);
            }

            if (GUI.Button(new Rect(tX + tButtonWidth + NWDConstants.kFieldMarge, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UPDATE, NWDConstants.tMiniButtonStyle))
            {
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //UpdateObjectInListOfEdition(this);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }

            if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 2, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DUPPLICATE, NWDConstants.tMiniButtonStyle))
            {

                // todo not update if not modified
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                //K tNexObject = (K)DuplicateMe();
                NWDBasis<K> tNexObject = DuplicateData(true, NWDWritingMode.ByEditorDefault);
                //AddObjectInListOfEdition(tNexObject);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(tNexObject);
                if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                {
                    tNexObject.Tag = BasisHelper().m_SearchTag;
                    tNexObject.UpdateData();
                }
                SetObjectInEdition(tNexObject);
                BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }
            if (AC == false)
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, NWDConstants.tMiniButtonStyle))
                {
                    EnableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }
            else
            {
                if (GUI.Button(new Rect(tX + (tButtonWidth + NWDConstants.kFieldMarge) * 3, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVE, NWDConstants.tMiniButtonStyle))
                {
                    DisableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }




            tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            GUI.Label(new Rect(tX, tY, tWidth, NWDConstants.tBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WARNING_ZONE, NWDConstants.tBoldLabelStyle);
            tY += NWDConstants.tBoldLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            NWDConstants.GUIRedButtonBegin();

            if (GUI.Button(new Rect(tX, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_PUT_IN_TRASH, NWDConstants.tMiniButtonStyle))
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


            if (GUI.Button(new Rect(tX + tButtonWidth * 1 + NWDConstants.kFieldMarge * 1, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DELETE, NWDConstants.tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_DELETE_WARNING,
                        NWDConstants.K_APP_BASIS_DELETE_MESSAGE,
                        NWDConstants.K_APP_BASIS_DELETE_OK,
                        NWDConstants.K_APP_BASIS_DELETE_CANCEL))
                {

                    DeleteData(NWDWritingMode.ByEditorDefault);
                    SetObjectInEdition(null);
                    RepaintTableEditor();
                    NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            //tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX + tButtonWidth * 2 + NWDConstants.kFieldMarge * 2, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_SHORT_REFERENCE, NWDConstants.tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                {
                    RegenerateNewShortReference();
                }
            }
            if (GUI.Button(new Rect(tX + tButtonWidth * 3 + NWDConstants.kFieldMarge * 3, tY, tButtonWidth, NWDConstants.tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_REFERENCE, NWDConstants.tMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                {
                    RegenerateNewReference();
                }
            }
            tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            NWDConstants.GUIRedButtonEnd();
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
        [NWDAliasMethod(NWDConstants.M_CheckError)]
        public void ErrorCheck()
        {
            //  Debug.Log("NWDBasis ErrorCheck()");
            bool tErrorResult = false;
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
                        if (tBTBDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataTypeInt tBTBDataType = tValue as BTBDataTypeInt;
                        if (tBTBDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataTypeFloat tBTBDataType = tValue as BTBDataTypeFloat;
                        if (tBTBDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                        }
                    }
                }
            }

            if (AddonErrorFound() == true)
            {
                tErrorResult = true;
            }

            if (InError != tErrorResult)
            {
                InError = tErrorResult;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif