//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:21:1
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
//using BasicToolBox;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        protected UnityEngine.Object PreviewObject = null;
        protected Texture2D PreviewTexture;
        protected bool PreviewTextureIsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        bool CanBeEdit = true;
        string ErrorLog;
        bool WithScrollview = true;
        public Rect TotalRect;
        public Rect HeaderRect;
        public Rect InformationsRect;
        public Rect NodalRect;
        public Rect PropertiesRect;
        public Rect AddOnRect;
        public Rect ActionRect;
        public Rect ContentRect;
        public Rect ScrollRect;
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D ReloadPreview()
        {
            //NWEBenchmark.Start();
            PreviewTexture = null;
            PreviewObject = null;
            if (string.IsNullOrEmpty(Preview) == false)
            {
                PreviewObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(UnityEngine.Object)) as UnityEngine.Object;
                if (PreviewObject is GameObject)
                {
                    ((GameObject)PreviewObject).transform.rotation = Quaternion.Euler(-180, -180, -180);
                }
                if (PreviewObject != null)
                {
                    PreviewTexture = AssetPreview.GetAssetPreview(PreviewObject);
                    PreviewTextureIsLoaded = true;
                }
            }
            //NWEBenchmark.Finish();
            return PreviewTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override Texture2D PreviewTexture2D()
        {
            ReloadPreview();
            return PreviewTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Vector2 sOrigin)
        {
            //NWEBenchmark.Start();
            DrawPreviewTexture2D(new Rect(sOrigin.x, sOrigin.y, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize));
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Rect sRect)
        {
            //NWEBenchmark.Start();
            Texture2D tTexture = PreviewTexture2D();
            if (tTexture != null)
            {
                GUI.DrawTexture(sRect, tTexture, ScaleMode.ScaleToFit, true);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DrawEditor(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            //NodalCard = sNodalCard;
            WithScrollview = sWithScrollview;
            if (sNodalCard != null)
            {
                WithScrollview = false;
            }
            if (sNodalCard == null)
            {
                TotalSize(NWDGUI.MargeAll(sInRect), WithScrollview, sNodalCard);
            }

            Rect tRTotal = TotalRect;
            if (sNodalCard != null)
            {
                tRTotal = sNodalCard.TotalRect;
            }
            tRTotal = NWDGUI.UnMargeAll(tRTotal);
            if (WebserviceVersionIsValid())
            {
                if (TestIntegrityResult == false)
                {
                    //Debug.Log("TestIntegrityResult false");
                    EditorGUI.DrawRect(tRTotal, NWDGUI.kRowColorError);
                    CanBeEdit = false;
                }
                else if (XX > 0)
                {
                    //Debug.Log("XX > 0");
                    EditorGUI.DrawRect(tRTotal, NWDGUI.kRowColorTrash);
                    CanBeEdit = false;
                }

                else if (IsEnable() == false)
                {
                    //Debug.Log("IsEnable == false");
                    EditorGUI.DrawRect(tRTotal, NWDGUI.kRowColorDisactive);
                    CanBeEdit = true;
                }
            }
            else
            {
                //Debug.Log("WebserviceVersionIsValid() == false");
                EditorGUI.DrawRect(tRTotal, NWDGUI.kRowColorWarning);
                CanBeEdit = false;
            }
            DrawHeader(sNodalCard);
            DrawInformations(sNodalCard);
            DrawNodal(sNodalCard);
            DrawScrollView(sNodalCard);
            DrawAction(sNodalCard);
            // MANAGEMENT EVENT
            // Shortcut navigation
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
            {
                    NWDBasis tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis;
                if (tSelected != null)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tSelected))
                    {
                        int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                        if (tIndexSelected < BasisHelper().EditorTableDatas.Count - 1)
                        {
                            NWDTypeClass tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected + 1);
                            BasisHelper().SetObjectInEdition(tNextSelected);
                            BasisHelper().ChangeScroolPositionToSelection();
                            Event.current.Use();
                        }
                    }
                    else
                    {
                    }
                }
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
            {
                NWDBasis tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis;
                if (tSelected != null)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tSelected))
                    {
                        int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                        if (tIndexSelected > 0)
                        {
                            NWDTypeClass tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected - 1);
                            BasisHelper().SetObjectInEdition(tNextSelected);
                            BasisHelper().ChangeScroolPositionToSelection();
                            Event.current.Use();
                        }
                    }
                    else
                    {
                    }
                }
            }
            float tNumberOfPage = BasisHelper().EditorTableDatas.Count / BasisHelper().m_ItemPerPage;
            int tPagesExpected = (int)Math.Floor(tNumberOfPage);
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
            {
                if (BasisHelper().m_PageSelected < tPagesExpected)
                {
                    BasisHelper().m_PageSelected++;
                    int tIndexSel = BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected;
                    if (tIndexSel < BasisHelper().EditorTableDatas.Count)
                    {
                        NWDTypeClass tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSel);
                        BasisHelper().SetObjectInEdition(tNextSelected);
                        BasisHelper().ChangeScroolPositionToSelection();
                        Event.current.Use();
                    }
                }
                else
                {
                }
            }
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
            {
                if (BasisHelper().m_PageSelected > 0)
                {
                    BasisHelper().m_PageSelected--;
                    NWDTypeClass tNextSelected = BasisHelper().EditorTableDatas.ElementAt(BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected);
                    BasisHelper().SetObjectInEdition(tNextSelected);
                    BasisHelper().ChangeScroolPositionToSelection();
                    Event.current.Use();
                }
                else
                {
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawHeader(NWDNodeCard sNodalCard)
        {
            Rect tR = new Rect(HeaderRect.x, HeaderRect.y, HeaderRect.width, HeaderRect.height);
            if (sNodalCard != null)
            {
                tR = new Rect(sNodalCard.HeaderRect.x, sNodalCard.HeaderRect.y, sNodalCard.HeaderRect.width, sNodalCard.HeaderRect.height);
            }
            tR.height = 0;

            if (sNodalCard != null)
            {
                tR.y += NWDGUI.kFieldMarge;
                tR.y += sNodalCard.ParentDocument.DrawAnalyzer(tR, sNodalCard, GetType().Name);
                NWDGUI.Line(NWDGUI.UnMargeLeftRight(tR)); //, Color.blue);
            }
            // DARW MODEL ALERT DEBUG 
            if (BasisHelper().TablePrefix != BasisHelper().TablePrefixOld)
            {
                tR.y += NWDGUI.kFieldMarge;
                tR.y += NWDGUI.WarningBox(tR, NWDConstants.K_APP_BASIS_WARNING_PREFIXE).height + NWDGUI.kFieldMarge;
            }
            if (BasisHelper().WebModelChanged == true)
            {
                tR.y += NWDGUI.kFieldMarge;
                tR.y += NWDGUI.WarningBox(tR, NWDConstants.K_APP_BASIS_WARNING_MODEL).height + NWDGUI.kFieldMarge;
            }
            if (BasisHelper().WebModelDegraded == true)
            {
                tR.y += NWDGUI.kFieldMarge;
                tR.y += NWDGUI.WarningBox(tR, NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED).height + NWDGUI.kFieldMarge;
            }
            if (sNodalCard == null)
            {
                if (NWDDataInspector.InspectNetWorkedPreview())
                {
                    if (GUI.Button(new Rect(tR.x, tR.y + 10, 20, 20), NWDGUI.kLeftContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        NWDDataInspector.InspectNetWorkedDataPreview();
                    }
                }
                if (NWDDataInspector.InspectNetWorkedNext())
                {
                    if (GUI.Button(new Rect(tR.x + tR.width - 20, tR.y + 10, 20, 20), NWDGUI.kRightContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        NWDDataInspector.InspectNetWorkedDataNext();
                    }
                }
            }
            //Draw Internal Key
            string tTitle = InternalKey;
            // Draw Informations
            if (string.IsNullOrEmpty(tTitle))
            {
                tTitle = "Unamed " + BasisHelper().ClassNamePHP + string.Empty;
            }
            if (InError == true)
            {
                tTitle = "<b><color=red>" + NWDConstants.K_WARNING + tTitle + "</color></b>";
            }
            tR.height = NWDGUI.kInspectorInternalTitle.fixedHeight;
            GUI.Label(tR, new GUIContent (tTitle,ErrorLog), NWDGUI.kInspectorInternalTitle);
            tR.y += NWDGUI.kInspectorInternalTitle.fixedHeight;
            Rect tRcenter = new Rect(tR.x + NWDGUI.kIconClassWidth + NWDGUI.kFieldMarge, tR.y, HeaderRect.width - NWDGUI.kFieldMarge * 2 - NWDGUI.kIconClassWidth * 2, NWDGUI.kInspectorReferenceCenter.fixedHeight);
            // Draw reference
            tRcenter.height = NWDGUI.kInspectorReferenceCenter.fixedHeight;
            GUI.Label(tR, Reference, NWDGUI.kInspectorReferenceCenter);
            tRcenter.y += NWDGUI.kInspectorReferenceCenter.fixedHeight;
            Rect tRright = new Rect(tR.x, tRcenter.y, NWDGUI.kIconClassWidth, NWDGUI.kIconClassWidth);
            Rect tRleft = new Rect(tR.x + tR.width - NWDGUI.kIconClassWidth, tRcenter.y, NWDGUI.kIconClassWidth, NWDGUI.kIconClassWidth);
            tRcenter.height = NWDGUI.kInspectorReferenceCenter.fixedHeight;
            GUI.Label(tRcenter, BasisHelper().ClassNamePHP, NWDGUI.kInspectorReferenceCenter);
            tRcenter.y += NWDGUI.kInspectorReferenceCenter.fixedHeight;
            tRcenter.height = NWDGUI.kInspectorReferenceCenter.fixedHeight;
            GUI.Label(tRcenter, NWDConstants.K_APP_BASIS_DM + NWDToolbox.TimeStampToDateTime(DM).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kInspectorReferenceCenter);
            tRcenter.y += NWDGUI.kInspectorReferenceCenter.fixedHeight;

            if (GUI.Button(tRcenter, "Select in table", NWDGUI.kMiniButtonStyle))
            {
                foreach (NWDTypeWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(ClassType()))
                {
                    tWindow.Focus();
                    tWindow.SelectTab(ClassType());
                }
                BasisHelper().SetObjectInEdition(this, false, true);
                BasisHelper().ChangeScroolPositionToSelection();
            }
            // draw preview
            DrawPreviewTexture2D(tRright);
            //draw class icon
            Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUI.DrawTexture(tRleft, tTextureOfClass);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawInformations(NWDNodeCard sNodalCard)
        {
            // DRAW TOP : properties area
            bool tDrawTop = true;
            if (sNodalCard != null)
            {
                tDrawTop = sNodalCard.ParentDocument.DrawInformationsArea;
            }
            if (tDrawTop == true)
            {

                Rect tR = new Rect(InformationsRect.x, InformationsRect.y, InformationsRect.width, InformationsRect.height);
                if (sNodalCard != null)
                {
                    tR = new Rect(sNodalCard.InformationsRect.x, sNodalCard.InformationsRect.y, sNodalCard.InformationsRect.width, sNodalCard.InformationsRect.height);
                }
                tR.height = 0;

                if (WebserviceVersionIsValid())
                {
                    if (TestIntegrityResult == false)
                    {
                        tR.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                        GUI.Label(tR, NWDConstants.K_APP_BASIS_INTEGRITY_IS_FALSE, NWDGUI.kBoldLabelStyle);
                        tR.y += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kHelpBoxStyle.fixedHeight;
                        EditorGUI.HelpBox(tR, NWDConstants.K_APP_BASIS_INTEGRITY_HELPBOX, MessageType.Error);
                        tR.y += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                        if (GUI.Button(tR, NWDConstants.K_APP_BASIS_INTEGRITY_REEVAL, NWDGUI.kMiniButtonStyle))
                        {
                            if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_INTEGRITY_WARNING,
                                    NWDConstants.K_APP_BASIS_INTEGRITY_WARNING_MESSAGE,
                                    NWDConstants.K_APP_BASIS_INTEGRITY_OK,
                                    NWDConstants.K_APP_BASIS_INTEGRITY_CANCEL))
                            {
                                UpdateData(true);
                            }
                        }
                        tR.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                        tR.y += NWDGUI.kFieldMarge;

                    }
                    else if (XX > 0)
                    {
                        tR.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                        GUI.Label(tR, NWDConstants.K_APP_BASIS_IN_TRASH, NWDGUI.kBoldLabelStyle);
                        tR.y += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kHelpBoxStyle.fixedHeight;
                        EditorGUI.HelpBox(tR, NWDConstants.K_APP_BASIS_IN_TRASH_HELPBOX, MessageType.Warning);
                        tR.y += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                        if (GUI.Button(tR, NWDConstants.K_APP_BASIS_UNTRASH, NWDGUI.kMiniButtonStyle))
                        {
                            if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_UNTRASH_WARNING,
                                    NWDConstants.K_APP_BASIS_UNTRASH_MESSAGE,
                                    NWDConstants.K_APP_BASIS_UNTRASH_OK,
                                    NWDConstants.K_APP_BASIS_UNTRASH_CANCEL
                                ))
                            {
                                UnTrashData();
                            }
                        }
                        tR.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                        tR.y += NWDGUI.kFieldMarge;
                    }

                    else if (IsEnable() == false)
                    {
                        tR.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                        GUI.Label(tR, NWDConstants.K_APP_BASIS_DISABLED, NWDGUI.kBoldLabelStyle);
                        tR.y += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kHelpBoxStyle.fixedHeight;
                        EditorGUI.HelpBox(tR, NWDConstants.K_APP_BASIS_DISABLED_HELPBOX, MessageType.Warning);
                        tR.y += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                        tR.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                        if (GUI.Button(tR, NWDConstants.K_APP_BASIS_REACTIVE_LONG, NWDGUI.kMiniButtonStyle))
                        {
                            if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_REACTIVE_WARNING,
                                    NWDConstants.K_APP_BASIS_REACTIVE_WARNING_MESSAGE,
                                    NWDConstants.K_APP_BASIS_REACTIVE_OK,
                                    NWDConstants.K_APP_BASIS_REACTIVE_CANCEL
                                ))
                            {
                                EnableData();
                            }
                        }
                        tR.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                        tR.y += NWDGUI.kFieldMarge;
                    }
                }
                else
                {
                    tR.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_WS_ERROR, NWDGUI.kBoldLabelStyle);
                    tR.y += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                    tR.height = NWDGUI.kHelpBoxStyle.fixedHeight;
                    EditorGUI.HelpBox(tR, NWDConstants.K_APP_BASIS_WS_ERROR_HELPBOX, MessageType.Warning);
                    tR.y += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                    tR.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                    if (GUI.Button(tR, NWDConstants.K_APP_BASIS_WS_ERROR_FIX, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING,
                                                        NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING_MESSAGE,
                                                        NWDConstants.K_APP_BASIS_WS_ERROR_FIX_OK,
                                                        NWDConstants.K_APP_BASIS_WS_ERROR_FIX_CANCEL
                            ))
                        {
                            UpdateData();
                        }
                    }
                    tR.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tR.y += NWDGUI.kFieldMarge;
                }
                EditorGUI.BeginDisabledGroup(CanBeEdit == false);
                // DRAW TOP CARD AREA
                tR.height = NWDGUI.kMiniLabelStyle.fixedHeight;
                BasisHelper().kSyncAndMoreInformations = EditorGUI.Foldout(tR, BasisHelper().kSyncAndMoreInformations, NWDConstants.K_APP_BASIS_INFORMATIONS, NWDGUI.kFoldoutStyle);
                tR.y += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;
                //tR.y += NWDGUI.kFieldMarge;
                if (BasisHelper().kSyncAndMoreInformations)
                {
                    EditorGUI.EndDisabledGroup();
                    tR.height = NWDGUI.kMiniLabelStyle.fixedHeight;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_DC + "(" + DC.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DC).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_Sync + "(" + DS.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_DevSync + "(" + DevSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DevSync).ToString("yyyy/MM/dd HH:mm:ss")
                               + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_PreprodSync + "(" + PreprodSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(PreprodSync).ToString("yyyy/MM/dd HH:mm:ss")
                               + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_ProdSync + "(" + ProdSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(ProdSync).ToString("yyyy/MM/dd HH:mm:s")
                                + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_xx + XX.ToString(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    GUI.Label(tR, NWDConstants.K_APP_BASIS_ac + AC.ToString(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "server log ", ServerLog, NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "server hash ", ServerHash, NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "integrity val", Integrity, NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "dyn integrity seq", IntegrityAssembly(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "head integrity Csv", CSVAssemblyHead(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "dyn integrity Csv", CSVAssembly(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.TextField(tR, "dyn integrity req", IntegrityValue(), NWDGUI.kMiniLabelStyle);
                    tR.y += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                    EditorGUI.BeginDisabledGroup(CanBeEdit == false);
                }
                tR.height = NWDGUI.kObjectFieldStyle.fixedHeight;
                UnityEngine.Object pObj = EditorGUI.ObjectField(tR, NWDConstants.K_APP_BASIS_PREVIEW_GAMEOBJECT, PreviewObject, typeof(UnityEngine.Object), false);
                tR.y += NWDGUI.kObjectFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                string tPreFabGameObject = string.Empty;
                if (pObj != null)
                {
                    tPreFabGameObject = AssetDatabase.GetAssetPath(pObj);
                }

                if (Preview != tPreFabGameObject)
                {
                    Preview = tPreFabGameObject;
                    UpdateData(true, NWDWritingMode.ByEditorDefault);
                    PreviewTexture2D();
                    RowAnalyze();
                    BasisHelper().RepaintTableEditor();
                }

                bool tInternalKeyEditable = true;

                if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
                {
                    tInternalKeyEditable = false;
                }

                if (tInternalKeyEditable == true)
                {
                    tR.height = NWDGUI.kTextFieldStyle.fixedHeight;
                    string tInternalNameActual = NWDToolbox.TextUnprotect(InternalKey);
                    string tInternalName = EditorGUI.TextField(tR, NWDConstants.K_APP_BASIS_INTERNAL_KEY, tInternalNameActual, NWDGUI.kTextFieldStyle);
                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    if (tInternalName != InternalKey)
                    {
                        tInternalName = NWDToolbox.TextProtect(tInternalName);
                        InternalKey = tInternalName;
                        UpdateDataEditor();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.LabelField(tR, NWDConstants.K_APP_BASIS_INTERNAL_KEY, NWDToolbox.TextUnprotect(InternalKey), NWDGUI.kTextFieldStyle);
                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.EndDisabledGroup();
                }


                bool tInternalDescriptionEditable = true;

                if (GetType().GetCustomAttributes(typeof(NWDInternalDescriptionNotEditableAttribute), true).Length > 0)
                {
                    tInternalDescriptionEditable = false;
                }
                if (tInternalDescriptionEditable == true)
                {
                    string tInternalDescriptionActual = NWDToolbox.TextUnprotect(InternalDescription);
                string tInternalDescription = EditorGUI.TextField(tR, NWDConstants.K_APP_BASIS_INTERNAL_DESCRIPTION, tInternalDescriptionActual, NWDGUI.kTextFieldStyle);
                tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (tInternalDescription != InternalDescription)
                {
                    tInternalDescription = NWDToolbox.TextProtect(tInternalDescription);
                    InternalDescription = tInternalDescription;
                        UpdateDataEditor();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.LabelField(tR, NWDConstants.K_APP_BASIS_INTERNAL_DESCRIPTION, NWDToolbox.TextUnprotect(InternalDescription), NWDGUI.kTextFieldStyle);
                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.EndDisabledGroup();
                }

                if (CanBeEdit == true)
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
                    tR.height = NWDGUI.kTextFieldStyle.fixedHeight;
                    int tWebServiceVersionIndex = EditorGUI.Popup(tR,
                                                                     "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")",
                                                                     tWebServiceVersionOldIndex, tWebServicesString.ToArray());
                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    int tWebServiceVersionNew = 0;
                    if (tWebServicesInt.Count > 0)
                    {
                        tWebServiceVersionNew = tWebServicesInt[tWebServiceVersionIndex];
                    }
                    if (WebModel != tWebServiceVersionNew)
                    {
                        // Debug.Log(" set from " + WebModel + " To " + tWebServiceVersionNew);
                        WebModel = tWebServiceVersionNew;
                        UpdateDataEditor();
                    }
                }
                else
                {
                    NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                    int tWebBuilt = tApp.WebBuild;
                    EditorGUI.TextField(tR, "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")", WebModel.ToString());
                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
                // Tag management
                List<int> tTagIntList = new List<int>();
                List<string> tTagStringList = new List<string>();
                foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
                {
                    tTagIntList.Add(tTag.Key);
                    tTagStringList.Add(tTag.Value);
                }
                NWDBasisTag tInternalTag = (NWDBasisTag)EditorGUI.IntPopup(tR,
                                                                            NWDConstants.K_APP_BASIS_INTERNAL_TAG,
                                                                           (int)Tag,
                                                                           tTagStringList.ToArray(), tTagIntList.ToArray());
                tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (tInternalTag != Tag)
                {
                    Tag = tInternalTag;
                        UpdateDataEditor();
                }

                if (BasisHelper().kAccountDependent == false)
                {
                    if (CheckList == null)
                    {
                        CheckList = new NWDBasisCheckList();
                    }
                    NWDBasisCheckList tCheckList = (NWDBasisCheckList)CheckList.ControlField(tR, NWDConstants.K_APP_TABLE_SEARCH_CHECKLIST, !CanBeEdit );

                    tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                    if (tCheckList.Value != CheckList.Value)
                    {
                        CheckList = tCheckList;
                        UpdateDataEditor();
                    }
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

                Rect[] tSyncRect = NWDGUI.DiviseArea(tR, 3, false);
                // Toogle Dev Prepprod Prod and operation associated
                float tWidthTiers = tR.width / 3.0f;
                bool tDevLockAnalyze = false;
                if (DevSync >= 0)
                {
                    tDevLockAnalyze = true;
                }
                bool tDevLock = EditorGUI.ToggleLeft(tSyncRect[0], "Dev", tDevLockAnalyze);
                bool tPreprodLockAnalyze = false;
                if (PreprodSync >= 0)
                {
                    tPreprodLockAnalyze = true;
                }
                bool tPreprodLock = EditorGUI.ToggleLeft(tSyncRect[1], "Preprod", tPreprodLockAnalyze);
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
                bool tProdLock = EditorGUI.ToggleLeft(tSyncRect[2], "Prod", tProdLockAnalyze);
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
                    BasisHelper().RepaintTableEditor();
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
                    BasisHelper().RepaintTableEditor();
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
                    BasisHelper().RepaintTableEditor();
                }
                tR.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                EditorGUI.EndDisabledGroup();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawNodal(NWDNodeCard sNodalCard)
        {
            if (sNodalCard != null)
            {
                EditorGUI.HelpBox(sNodalCard.NodalRect, string.Empty, MessageType.None);
                AddOnNodeDraw(NWDGUI.MargeAll(sNodalCard.NodalRect));

                NWDGUI.Line(NWDGUI.UnMargeLeftRight(new Rect(sNodalCard.NodalRect.x, sNodalCard.NodalRect.y + sNodalCard.NodalRect.height + NWDGUI.kFieldMarge, sNodalCard.NodalRect.width, 1))
                    );//, Color.red);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawScrollView(NWDNodeCard sNodalCard)
        {
            bool rNeedBeUpdate = false;
            EditorGUI.BeginChangeCheck();
            // Start scrollview
            if (WithScrollview == true)
            {
                NWDGUI.Line(NWDGUI.UnMargeLeftRight(new Rect(ScrollRect.x, ScrollRect.y - 1, ScrollRect.width, 1)));//, Color.green);
                BasisHelper().ObjectEditorScrollPosition = GUI.BeginScrollView(ScrollRect, BasisHelper().ObjectEditorScrollPosition, ContentRect);
            }
            EditorGUI.BeginDisabledGroup(CanBeEdit == false);
            // DRAW MIDDLE : properties area
            bool tDrawMiddle = true;
            if (sNodalCard != null)
            {
                tDrawMiddle = sNodalCard.ParentDocument.DrawPropertiesArea;
            }
            if (tDrawMiddle == true)
            {
                DrawProperties(sNodalCard);
            }
            // DRAW EDITOR ADDON : Addon area
            bool tDrawEditor = true;
            if (sNodalCard != null)
            {
                tDrawEditor = sNodalCard.ParentDocument.DrawAddOnArea;
            }
            if (tDrawEditor == true)
            {
                DrawAddOn(sNodalCard);
            }
            EditorGUI.EndDisabledGroup();
            // finish scrollview 
            if (WithScrollview == true)
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
                if (CanBeEdit == true)
                {
                    ErrorCheck();
                    WebserviceVersionCheckMe();
                    if (IntegrityValue() != this.Integrity)
                    {
                        UpdateData(true, NWDWritingMode.ByEditorDefault);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawProperties(NWDNodeCard sNodalCard)
        {
            Rect tR = new Rect(PropertiesRect.x, PropertiesRect.y, PropertiesRect.width, PropertiesRect.height);
            if (sNodalCard != null)
            {
                tR = new Rect(sNodalCard.PropertiesRect.x, sNodalCard.PropertiesRect.y, sNodalCard.PropertiesRect.width, sNodalCard.PropertiesRect.height);
            }
            float tY = tR.y;
            if (sNodalCard == null)
            {
                tY += NWDGUI.kFieldMarge;
            }
            NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
            foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
            {
                tY = tElement.NewDrawObjectInspector(this, sNodalCard, tR.x, tY, tR.width);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawAddOn(NWDNodeCard sNodalCard)
        {
            Rect tR = AddOnRect;
            if (sNodalCard != null)
            {
                tR = sNodalCard.AddOnRect;
            }
            AddonEditor(tR);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawAction(NWDNodeCard sNodalCard)
        {
            bool tDrawBottom = true;
            if (sNodalCard != null)
            {
                tDrawBottom = sNodalCard.ParentDocument.DrawActionArea;
            }
            if (tDrawBottom == true)
            {
                Rect tActionRectO = ActionRect;
                if (sNodalCard != null)
                {
                    tActionRectO = sNodalCard.ActionRect;
                }
                NWDGUI.Line(NWDGUI.UnMargeLeftRight(tActionRectO));//, Color.yellow);
                Rect tActionRect = new Rect(tActionRectO.x, tActionRectO.y + NWDGUI.kFieldMarge, tActionRectO.width, tActionRectO.height - NWDGUI.kFieldMarge);
                Rect[,] tMatrixRect = NWDGUI.DiviseArea(tActionRect, 4, 4, true);
                GUI.Label(NWDGUI.AssemblyArea(tMatrixRect[0, 0], tMatrixRect[3, 0]), NWDConstants.K_APP_BASIS_ACTION_ZONE, NWDGUI.kBoldLabelStyle);
                if (GUI.Button(tMatrixRect[0, 1], NWDConstants.K_BUTTON_EDITOR_NODAL, NWDGUI.kMiniButtonStyle))
                {
                    NWDNodeEditor.SetObjectInNodeWindow(this);
                }
                if (GUI.Button(tMatrixRect[1, 1], NWDConstants.K_APP_BASIS_UPDATE, NWDGUI.kMiniButtonStyle))
                {
                    //DM = NWDToolbox.Timestamp();
                    //UpdateIntegrity();
                    UpdateData(true, NWDWritingMode.ByEditorDefault);
                    NWDDataManager.SharedInstance().DataQueueExecute();
                }
                if (GUI.Button(tMatrixRect[2, 1], NWDConstants.K_APP_BASIS_DUPPLICATE, NWDGUI.kMiniButtonStyle))
                {
                    UpdateDataIfModified(true, NWDWritingMode.ByEditorDefault);
                    NWDTypeClass tNexObject = NWDBasisHelper.DuplicateData(this, true, NWDWritingMode.ByEditorDefault);
                    if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                    {
                        tNexObject.Tag = BasisHelper().m_SearchTag;
                        tNexObject.UpdateData();
                    }
                    BasisHelper().SetObjectInEdition(tNexObject);
                    BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                    NWDDataManager.SharedInstance().DataQueueExecute();
                }
                EditorGUI.BeginDisabledGroup(IsTrashed());
                if (AC == false)
                {
                    if (GUI.Button(tMatrixRect[3, 1], NWDConstants.K_APP_BASIS_REACTIVE, NWDGUI.kMiniButtonStyle))
                    {

                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_REACTIVE_WARNING,
                                    NWDConstants.K_APP_BASIS_REACTIVE_WARNING_MESSAGE,
                                    NWDConstants.K_APP_BASIS_REACTIVE_OK,
                                    NWDConstants.K_APP_BASIS_REACTIVE_CANCEL
                                ))
                        {
                            EnableData();
                        }
                    }
                }
                else
                {
                    if (GUI.Button(tMatrixRect[3, 1], NWDConstants.K_APP_BASIS_DISACTIVE, NWDGUI.kMiniButtonStyle))
                    {
                        DisableData();
                    }
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.EndDisabledGroup();
                GUI.Label(NWDGUI.AssemblyArea(tMatrixRect[0, 2], tMatrixRect[3, 2]), NWDConstants.K_APP_BASIS_WARNING_ZONE, NWDGUI.kBoldLabelStyle);
                NWDGUI.BeginRedArea();
                if (IsTrashed() == false)
                {
                    if (GUI.Button(tMatrixRect[0, 3], NWDConstants.K_APP_BASIS_PUT_IN_TRASH, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_PUT_IN_TRASH_WARNING,
                                NWDConstants.K_APP_BASIS_PUT_IN_TRASH_MESSAGE,
                                NWDConstants.K_APP_BASIS_PUT_IN_TRASH_OK,
                                NWDConstants.K_APP_BASIS_PUT_IN_TRASH_CANCEL))
                        {
                            TrashData();
                        }
                    }
                }
                else
                {
                    if (GUI.Button(tMatrixRect[0, 3], NWDConstants.K_APP_BASIS_UNTRASH, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_UNTRASH_WARNING,
                                NWDConstants.K_APP_BASIS_UNTRASH_MESSAGE,
                                NWDConstants.K_APP_BASIS_UNTRASH_OK,
                                NWDConstants.K_APP_BASIS_UNTRASH_CANCEL))
                        {
                            UnTrashData();
                        }
                    }
                }
                if (GUI.Button(tMatrixRect[1, 3], NWDConstants.K_APP_BASIS_DELETE, NWDGUI.kMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_DELETE_WARNING,
                            NWDConstants.K_APP_BASIS_DELETE_MESSAGE,
                            NWDConstants.K_APP_BASIS_DELETE_OK,
                            NWDConstants.K_APP_BASIS_DELETE_CANCEL))
                    {

                        DeleteData(NWDWritingMode.ByEditorDefault);
                        BasisHelper().SetObjectInEdition(null);
                        BasisHelper().RepaintTableEditor();
                    }
                }
                if (GUI.Button(tMatrixRect[2, 3], NWDConstants.K_APP_BASIS_NEW_SHORT_REFERENCE, NWDGUI.kMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                    {
                        RegenerateNewShortReference();
                    }
                }
                if (GUI.Button(tMatrixRect[3, 3], NWDConstants.K_APP_BASIS_NEW_REFERENCE, NWDGUI.kMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                            NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                    {
                        RegenerateNewReference();
                    }
                }
                NWDGUI.EndRedArea();
                //if (GUI.Button(tMatrixRect[0, 4], "Select in table", NWDGUI.kMiniButtonStyle))
                //{
                //    foreach (NWDTypeWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(ClassType()))
                //    {
                //        tWindow.Focus();
                //        tWindow.SelectTab(ClassType());
                //    }
                //    BasisHelper().New_SetObjectInEdition(this,false,true);
                //    BasisHelper().New_ChangeScroolPositionToSelection();
                //}
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddonEditor(Rect sRect)
        {
            //EditorGUI.DrawRect(sRect, Color.blue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool AddonEdited(bool sNeedBeUpdate)
        {
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddOnNodeDraw(Rect sRect)
        {
            Rect tRect = new Rect(sRect);

            tRect.height = NWDGUI.kBoldLabelStyle.fixedHeight;

            //GUI.Label(tRect, InternalKey, NWDGUI.kBoldLabelStyle);
            //tRect.y+= NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            //GUI.Label(tRect, InternalDescription);
            //tRect.y += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ErrorCheck()
        {
            //NWEBenchmark.Start();
            ErrorLog = string.Empty;
            bool tErrorResult = false;
            Type tType = ClassType();
            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        NWEDataType tNWEDataType = tValue as NWEDataType;
                        tNWEDataType.BaseVerif();
                        if (tNWEDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                            ErrorLog += "error in " + tProp.Name + " with value " + tValue.ToString()+"\n";
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        NWEDataTypeInt tNWEDataType = tValue as NWEDataTypeInt;
                        if (tNWEDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                            ErrorLog += "error in " + tProp.Name + " with value " + tValue.ToString()+"\n";
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        NWEDataTypeFloat tNWEDataType = tValue as NWEDataTypeFloat;
                        if (tNWEDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                            ErrorLog += "error in " + tProp.Name + " with value " + tValue.ToString()+"\n";
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        NWEDataTypeEnum tNWEDataType = tValue as NWEDataTypeEnum;
                        if (tNWEDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                            ErrorLog += "error in " + tProp.Name + " with value " + tValue.ToString()+"\n";
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        NWEDataTypeMask tNWEDataType = tValue as NWEDataTypeMask;
                        if (tNWEDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                            ErrorLog += "error in " + tProp.Name + " with value " + tValue.ToString()+"\n";
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
            if (InError == false)
            {
                ErrorLog = "No error detected";
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif