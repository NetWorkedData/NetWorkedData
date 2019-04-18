// =====================================================================================================================
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
// =====================================================================================================================
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
        protected UnityEngine.Object PreviewObject = null;
        protected Texture2D PreviewTexture;
        protected bool PreviewTextureIsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        //public bool RecalulateHeight = true; 
        public float TotalHeight = 0.0F;
        public float TopHeight = 0.0F;
        public float MiddleHeight = 0.0F;
        public float InspectorHeight = 0.0F;
        public float BottomHeight = 0.0F;
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D ReloadPreview()
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            DrawPreviewTexture2D(new Rect(sOrigin.x, sOrigin.y, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize));
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreviewTexture2D(Rect sRect)
        {
            //BTBBenchmark.Start();
            Texture2D tTexture = PreviewTexture2D();
            if (tTexture != null)
            {
                GUI.DrawTexture(sRect, tTexture, ScaleMode.ScaleToFit, true);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public float DrawObjectInspectorHeight(NWDNodeCard sNodalCard)
        //{
        //    //BTBBenchmark.Start();
        //    float tY = 0;
        //    BasisHelper().AnalyzeForInspector();
        //    NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
        //    foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
        //    {
        //        tY += tElement.NewDrawObjectInspectorHeight(this, sNodalCard);
        //    }
        //    tY += AddonEditorHeight();
        //    //BTBBenchmark.Finish();
        //    return tY;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public Rect DrawInspector(Rect sInRect, NWDNodeCard sNodalCard, bool sEditionEnable)
        {
            //BTBBenchmark.Start();
            Rect tRect = new Rect(sInRect.position.x, sInRect.position.y, sInRect.width, 0);
            BasisHelper().AnalyzeForInspector();
            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            float tY = sInRect.position.y + NWDGUI.kFieldMarge;
            BasisHelper().AnalyzeForInspector();
            NWDBasisHelperGroup tInspectorHelper = BasisHelper().InspectorHelper;
            foreach (NWDBasisHelperElement tElement in tInspectorHelper.Elements)
            {
                tY = tElement.NewDrawObjectInspector(this, sNodalCard, tX, tY, tWidth);
                tRect.height += tY;
            }

            tRect.height += AddonEditor(new Rect(sInRect.position.x, sInRect.position.y+ tY, sInRect.width, 0));
            //BTBBenchmark.Finish();
            return tRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override float New_DrawObjectEditorHeight(NWDNodeCard sNodalCard)
        //{
        //    //BTBBenchmark.Start();
        //    //Todo Calculate the real height
        //    float tHeightTop = 130;

        //    if (sNodalCard != null)
        //    {
        //        sNodalCard.TotalHeightDynamique += tHeightTop;
        //    }

        //    float tHeightBottom = 60;
        //    float tY = New_DrawObjectInspectorHeight(sNodalCard) +130;
        //    return tY;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public override float New_DrawObjectInspectorHeight(NWDNodeCard sNodalCard)
        //{
        //    //BTBBenchmark.Start();
        //    float tY = 0;
        //    if (sNodalCard != null)
        //    {

        //    }
        //    NWDGUI.LoadStyles();
        //    Type tType = ClassType();
        //    tY += DrawObjectInspectorHeight(sNodalCard);
        //    tY += NWDGUI.kFieldMarge;
        //    tY += AddonEditorHeight();
        //    //BTBBenchmark.Finish();
        //    return tY;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void DrawEditorTop(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard)
        {
            //BTBBenchmark.Start();
            BasisHelper().RowAnalyze();
            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.x;
            float tXmarge = sInRect.x + NWDGUI.kFieldMarge;
            float tY = sInRect.y;
            float tYzero = sInRect.y;
            bool tCanBeEdit = true;
            bool tTestIntegrity = TestIntegrity();

            if (sNodalCard == null)
            {
                DrawEditorTotalHeight(null, tWidth);
            }
            // FOR Graphix Debug 
            //Rect tTopTestRect = new Rect(tX+2, tY, 3, TopHeight);
            //EditorGUI.DrawRect(tTopTestRect, Color.blue);
            //Rect tMiddleTestRect = new Rect(tX + 4, tY+ TopHeight, 3, MiddleHeight);
            //EditorGUI.DrawRect(tMiddleTestRect, Color.red);
            //Rect tInspectorTestRect = new Rect(tX + 8, tY + TopHeight, 3, InspectorHeight);
            //EditorGUI.DrawRect(tInspectorTestRect, Color.yellow);
            //Rect tBottomTestRect = new Rect(tX + 6, tY + sInRect.height - BottomHeight, 3, BottomHeight);
            //EditorGUI.DrawRect(tBottomTestRect, Color.green);
            if (sNodalCard != null)
            {
                Rect tTopTestRectB = new Rect(tX -4, tY, 3, sNodalCard.TopHeight);
                EditorGUI.DrawRect(tTopTestRectB, Color.blue);
                Rect tMiddleTestRectB = new Rect(tX -4, tY + sNodalCard.TopHeight, 3, sNodalCard.MiddleHeight);
                EditorGUI.DrawRect(tMiddleTestRectB, Color.red);
                Rect tBottomTestRectB = new Rect(tX -4, tY + sNodalCard.TopHeight + sNodalCard.MiddleHeight, 3, sNodalCard.BottomHeight);
                EditorGUI.DrawRect(tBottomTestRectB, Color.green);
            }
            // FINISH DEBUG 


                if (sNodalCard!=null)
            {
                tY += NWDGUI.kFieldMarge;
                tY += sNodalCard.ParentDocument.DrawAnalyzer(new Rect(tXmarge, tY, tWidth, NWDGUI.kPopupStyle.fixedHeight),GetType().Name);
            }

            //Draw Internal Key
            string tTitle = InternalKey;

            if (BasisHelper().WebModelChanged == true)
            {
                tY += NWDGUI.WarningBox(new Rect(tXmarge, tY, tWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL).height + NWDGUI.kFieldMarge;
            }

            if (BasisHelper().WebModelDegraded == true)
            {
                tY += NWDGUI.WarningBox(new Rect(tXmarge, tY, tWidth - NWDGUI.kFieldMarge * 2, 0), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED).height + NWDGUI.kFieldMarge;
            }
            if (string.IsNullOrEmpty(tTitle))
            {
                tTitle = "Unamed " + BasisHelper().ClassNamePHP + string.Empty;
            }
            if (InError == true)
            {
                tTitle = "<b><color=red>" + NWDConstants.K_WARNING + tTitle + "</color></b>";
            }

            if (NWDDataInspector.InspectNetWorkedPreview())
            {
                if (GUI.Button(new Rect(tXmarge, tY + 10, 20, 20), NWDGUI.kLeftContentIcon, NWDGUI.kEditButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedDataPreview();
                }
            }
            if (NWDDataInspector.InspectNetWorkedNext())
            {
                if (GUI.Button(new Rect(tXmarge + tWidth - 20, tY + 10, 20, 20), NWDGUI.kRightContentIcon, NWDGUI.kEditButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedDataNext();
                }
            }

            GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kInspectorInternalTitle.fixedHeight), tTitle, NWDGUI.kInspectorInternalTitle);
            tY += NWDGUI.kInspectorInternalTitle.fixedHeight;

            // Draw reference
            GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kInspectorReferenceCenter.fixedHeight), Reference, NWDGUI.kInspectorReferenceCenter);
            tY += NWDGUI.kInspectorReferenceCenter.fixedHeight + NWDGUI.kFieldMarge;

            //Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
            //if (tTextureOfClass != null)
            //{
            //    GUI.DrawTexture(new Rect(tX + tWidth / 2.0F - tImageWidth/2.0F, tY, tImageWidth, tImageWidth), tTextureOfClass);
            //}
            //tY += tImageWidth + NWDGUI.kFieldMarge;
            if (WebserviceVersionIsValid())
            {
                if (tTestIntegrity == false)
                {

                    EditorGUI.DrawRect(new Rect(tX, tYzero, sInRect.width, sInRect.height), NWDGUI.kRowColorError);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_IS_FALSE, NWDGUI.kBoldLabelStyle);
                    tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tXmarge, tY, tWidth, NWDGUI.kHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_HELPBOX, MessageType.Error);
                    tY += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                    if (GUI.Button(new Rect(tXmarge, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTEGRITY_REEVAL, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_INTEGRITY_WARNING,
                                NWDConstants.K_APP_BASIS_INTEGRITY_WARNING_MESSAGE,
                                NWDConstants.K_APP_BASIS_INTEGRITY_OK,
                                NWDConstants.K_APP_BASIS_INTEGRITY_CANCEL))
                        {
                            UpdateData(true);
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                            //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                        }
                    }
                    tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tY += NWDGUI.kFieldMarge;

                }
                else if (XX > 0)
                {

                    EditorGUI.DrawRect(new Rect(tX, tYzero, sInRect.width, sInRect.height), NWDGUI.kRowColorTrash);
                    tCanBeEdit = false;

                    GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH, NWDGUI.kBoldLabelStyle);
                    tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tXmarge, tY, tWidth, NWDGUI.kHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_IN_TRASH_HELPBOX, MessageType.Warning);
                    tY += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                    if (GUI.Button(new Rect(tXmarge, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UNTRASH, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_UNTRASH_WARNING,
                                NWDConstants.K_APP_BASIS_UNTRASH_WARNING_MESSAGE,
                                NWDConstants.K_APP_BASIS_UNTRASH_OK,
                                NWDConstants.K_APP_BASIS_UNTRASH_CANCEL
                            ))
                        {
                            UnTrashData();
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                            //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                        }
                    }
                    tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tY += NWDGUI.kFieldMarge;
                }

                else if (IsEnable() == false)
                {
                    EditorGUI.DrawRect(new Rect(tX, tYzero, sInRect.width, sInRect.height), NWDGUI.kRowColorDisactive);
                    tCanBeEdit = true;

                    GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISABLED, NWDGUI.kBoldLabelStyle);
                    tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                    EditorGUI.HelpBox(new Rect(tXmarge, tY, tWidth, NWDGUI.kHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISABLED_HELPBOX, MessageType.Warning);
                    tY += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                    if (GUI.Button(new Rect(tXmarge, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE_LONG, NWDGUI.kMiniButtonStyle))
                    {
                        if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_REACTIVE_WARNING,
                                NWDConstants.K_APP_BASIS_REACTIVE_WARNING_MESSAGE,
                                NWDConstants.K_APP_BASIS_REACTIVE_OK,
                                NWDConstants.K_APP_BASIS_REACTIVE_CANCEL
                            ))
                        {
                            EnableData();
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                            //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                        }
                    }
                    tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    tY += NWDGUI.kFieldMarge;
                }
            }
            else
            {
                EditorGUI.DrawRect(new Rect(tX, tYzero, sInRect.width, sInRect.height), NWDGUI.kRowColorWarning);
                tCanBeEdit = false;

                GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR, NWDGUI.kBoldLabelStyle);
                tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                EditorGUI.HelpBox(new Rect(tXmarge, tY, tWidth, NWDGUI.kHelpBoxStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_HELPBOX, MessageType.Warning);
                tY += NWDGUI.kHelpBoxStyle.fixedHeight + NWDGUI.kFieldMarge;

                if (GUI.Button(new Rect(tXmarge, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_WS_ERROR_FIX, NWDGUI.kMiniButtonStyle))
                {
                    if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_WARNING_MESSAGE,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_OK,
                                                    NWDConstants.K_APP_BASIS_WS_ERROR_FIX_CANCEL
                        ))
                    {
                        UpdateData();
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                        //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                    }
                }
                tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                tY += NWDGUI.kFieldMarge;
            }
            float tImageWidth = (NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge) * 3;

            float tXb = tX + tImageWidth + NWDGUI.kFieldMarge * 2;
            tWidth = sInRect.width - NWDGUI.kFieldMarge * 3 - tImageWidth;

            EditorGUI.BeginDisabledGroup(tCanBeEdit == false);

            if (sNodalCard == null)
            {
                EditorGUI.DrawRect(new Rect(tX, tY - NWDGUI.kFieldMarge, sInRect.width, sInRect.height), NWDGUI.kIdentityColor);
            }
            tY += NWDGUI.kFieldMarge;

            // draw preview
            DrawPreviewTexture2D(new Rect(tX + NWDGUI.kFieldMarge, tY, tImageWidth, tImageWidth));
            //draw class icon
            Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUI.DrawTexture(new Rect(tX + sInRect.width - NWDGUI.kFieldMarge - tImageWidth, tY, tImageWidth, tImageWidth), tTextureOfClass);
            }
            // draw button reload preview
            if (GUI.Button(new Rect(tXmarge + NWDGUI.kFieldMarge, tY + tImageWidth + NWDGUI.kFieldMarge, tImageWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Reload", NWDGUI.kMiniButtonStyle))
            {
                Debug.Log("Reload");
                ReloadPreview();
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), BasisHelper().ClassNamePHP + "'s Object", NWDGUI.kBoldLabelStyle);
            tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

            GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DM + NWDToolbox.TimeStampToDateTime(DM).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kMiniLabelStyle);
            tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

            BasisHelper().kSyncAndMoreInformations = EditorGUI.Foldout(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), BasisHelper().kSyncAndMoreInformations, NWDConstants.K_APP_BASIS_INFORMATIONS, NWDGUI.kFoldoutStyle);
            tY += NWDGUI.kFoldoutStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.kFieldMarge;

            if (BasisHelper().kSyncAndMoreInformations)
            {

                EditorGUI.EndDisabledGroup();


                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DC + "(" + DC.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DC).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_Sync + "(" + DS.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DS).ToString("yyyy/MM/dd HH:mm:ss"), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_DevSync + "(" + DevSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(DevSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_PreprodSync + "(" + PreprodSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(PreprodSync).ToString("yyyy/MM/dd HH:mm:ss")
                           + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ProdSync + "(" + ProdSync.ToString() + ")" + NWDToolbox.TimeStampToDateTime(ProdSync).ToString("yyyy/MM/dd HH:mm:s")
                            + " (last sync request " + NWDToolbox.TimeStampToDateTime(BasisHelper().New_SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss") + ")", NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_xx + XX.ToString(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                GUI.Label(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ac + AC.ToString(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;


                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "server log ", ServerLog, NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "server hash ", ServerHash, NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "integrity val", Integrity, NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "dyn integrity seq", IntegrityAssembly(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "head integrity Csv", CSVAssemblyHead(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "dyn integrity Csv", CSVAssembly(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.TextField(new Rect(tXb, tY, tWidth, NWDGUI.kMiniLabelStyle.fixedHeight), "dyn integrity req", IntegrityValue(), NWDGUI.kMiniLabelStyle);
                tY += NWDGUI.kMiniLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

                EditorGUI.BeginDisabledGroup(tCanBeEdit == false);
            }

            //tX = NWDGUI.kFieldMarge;

            tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;

            UnityEngine.Object pObj = EditorGUI.ObjectField(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight),
                                          NWDConstants.K_APP_BASIS_PREVIEW_GAMEOBJECT,
                                                            PreviewObject, typeof(UnityEngine.Object), false);
            tY += NWDGUI.kObjectFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

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
                BasisHelper().New_RepaintTableEditor();
                //NWDNodeEditor.ReAnalyzeIfNecessary(this);
            }

            bool tInternalKeyEditable = true;

            if (GetType().GetCustomAttributes(typeof(NWDInternalKeyNotEditableAttribute), true).Length > 0)
            {
                tInternalKeyEditable = false;
            }

            if (tInternalKeyEditable == true)
            {
                string tInternalNameActual = NWDToolbox.TextUnprotect(InternalKey);
                string tInternalName = EditorGUI.TextField(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, tInternalNameActual, NWDGUI.kTextFieldStyle);
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (tInternalName != InternalKey)
                {
                    tInternalName = NWDToolbox.TextProtect(tInternalName);
                    InternalKey = tInternalName;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    UpdateData(true, NWDWritingMode.ByEditorDefault);
                    RowAnalyze();
                    BasisHelper().New_RepaintTableEditor();
                    //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.LabelField(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_KEY, InternalKey, NWDGUI.kTextFieldStyle);
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                EditorGUI.EndDisabledGroup();
            }

            string tInternalDescriptionActual = NWDToolbox.TextUnprotect(InternalDescription);
            string tInternalDescription = EditorGUI.TextField(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDConstants.K_APP_BASIS_INTERNAL_DESCRIPTION, tInternalDescriptionActual, NWDGUI.kTextFieldStyle);
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (tInternalDescription != InternalDescription)
            {
                tInternalDescription = NWDToolbox.TextProtect(tInternalDescription);
                InternalDescription = tInternalDescription;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                RowAnalyze();
                BasisHelper().New_RepaintTableEditor();
                //NWDNodeEditor.ReAnalyzeIfNecessary(this);
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

                int tWebServiceVersionIndex = EditorGUI.Popup(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight),
                                                                 "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")",
                                                                 tWebServiceVersionOldIndex, tWebServicesString.ToArray());
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
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
                    BasisHelper().New_RepaintTableEditor();
                    //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            else
            {
                NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
                int tWebBuilt = tApp.WebBuild;
                EditorGUI.TextField(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), "WebModel " + WebModel + "(/" + tWebBuilt.ToString() + ")", WebModel.ToString());
                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            }

            // Tag management
            List<int> tTagIntList = new List<int>();
            List<string> tTagStringList = new List<string>();
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                tTagIntList.Add(tTag.Key);
                tTagStringList.Add(tTag.Value);
            }

            NWDBasisTag tInternalTag = (NWDBasisTag)EditorGUI.IntPopup(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight),
                                                                        NWDConstants.K_APP_BASIS_INTERNAL_TAG,
                                                                       (int)Tag,
                                                                       tTagStringList.ToArray(), tTagIntList.ToArray());
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (tInternalTag != Tag)
            {
                Tag = tInternalTag;
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                RowAnalyze();
                BasisHelper().New_RepaintTableEditor();
                //NWDNodeEditor.ReAnalyzeIfNecessary(this);
            }

            if (BasisHelper().kAccountDependent == false)
            {
                if (CheckList == null)
                {
                    CheckList = new NWDBasisCheckList();
                }
                NWDBasisCheckList tCheckList = (NWDBasisCheckList)CheckList.ControlField(new Rect(tXmarge, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDConstants.K_APP_TABLE_SEARCH_CHECKLIST);

                tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                if (tCheckList.Value != CheckList.Value)
                {
                    CheckList = tCheckList;
                    DM = NWDToolbox.Timestamp();
                    UpdateIntegrity();
                    UpdateData(true, NWDWritingMode.MainThread);
                    RowAnalyze();
                    BasisHelper().New_RepaintTableEditor();
                    //NWDNodeEditor.ReAnalyzeIfNecessary(this);
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

            // Toogle Dev Prepprod Prod and operation associated
            float tWidthTiers = tWidth / 3.0f;
            bool tDevLockAnalyze = false;
            if (DevSync >= 0)
            {
                tDevLockAnalyze = true;
            }
            bool tDevLock = EditorGUI.ToggleLeft(new Rect(tXmarge, tY, tWidthTiers, NWDGUI.kTextFieldStyle.fixedHeight), "Dev", tDevLockAnalyze);
            bool tPreprodLockAnalyze = false;
            if (PreprodSync >= 0)
            {
                tPreprodLockAnalyze = true;
            }
            bool tPreprodLock = EditorGUI.ToggleLeft(new Rect(tXmarge + tWidthTiers, tY, tWidthTiers, NWDGUI.kTextFieldStyle.fixedHeight), "Preprod", tPreprodLockAnalyze);
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
            bool tProdLock = EditorGUI.ToggleLeft(new Rect(tXmarge + tWidthTiers + tWidthTiers, tY, tWidthTiers, NWDGUI.kTextFieldStyle.fixedHeight), "Prod", tProdLockAnalyze);
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
                BasisHelper().New_RepaintTableEditor();
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
                BasisHelper().New_RepaintTableEditor();
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
                BasisHelper().New_RepaintTableEditor();
            }
            tY += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            if (sNodalCard != null)
            {
                float tH = AddOnNodeDrawHeight(NWDGUI.kNodeCardWidth);
                EditorGUI.HelpBox(new Rect(tXmarge, tY, tWidth, tH), "Nodal area", MessageType.Info);
                tY += tH;
                tY += NWDGUI.kFieldMarge;
            }

            NWDGUI.Line(new Rect(tX, tY - 1, tWidth + NWDGUI.kFieldMarge * 2, 1));

            float tBottomHeight = 0;
            if (sNodalCard== null)
            {
                tBottomHeight = BottomHeight;
            }


            Rect tRectProperty = new Rect(tX, tY, sInRect.width, sInRect.height - tY - tBottomHeight);
            if (sNodalCard == null)
            {
                EditorGUI.DrawRect(tRectProperty, NWDGUI.kPropertyColor);
            }
            DrawEditorMiddle(tRectProperty, sWithScrollview, sNodalCard, tCanBeEdit);

            if (sNodalCard == null)
            {
                tY = sInRect.height - tBottomHeight;
            }
            else
            {
                tY += sNodalCard.MiddleHeight;
            }

            //            EditorGUI.indentLevel = 0;

            NWDGUI.Line(new Rect(tX, tY , tWidth + NWDGUI.kFieldMarge * 2, 1));

            // Prepare Action and Warning zone
            DrawEditorBottom(new Rect (sInRect.x, tY, sInRect.width, BottomHeight),sWithScrollview, sNodalCard, tCanBeEdit);

            // Shortcut navigation

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
            {
                NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                if (tSelected != null)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tSelected))
                    {
                        int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                        if (tIndexSelected < BasisHelper().EditorTableDatas.Count - 1)
                        {
                            K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected + 1) as K;
                            BasisHelper().New_SetObjectInEdition(tNextSelected);
                            BasisHelper().New_ChangeScroolPositionToSelection();
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
                NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                if (tSelected != null)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tSelected))
                    {
                        int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                        if (tIndexSelected > 0)
                        {
                            K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected - 1) as K;
                            BasisHelper().New_SetObjectInEdition(tNextSelected);
                            BasisHelper().New_ChangeScroolPositionToSelection();
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
                        K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSel) as K;
                        BasisHelper().New_SetObjectInEdition(tNextSelected);
                        BasisHelper().New_ChangeScroolPositionToSelection();
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
                    K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected) as K;
                    BasisHelper().New_SetObjectInEdition(tNextSelected);
                    BasisHelper().New_ChangeScroolPositionToSelection();
                    Event.current.Use();
                }
                else
                {
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DrawEditorMiddle(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard, bool sEditionEnable)
        {
            //BTBBenchmark.Start();
            NWDGUI.LoadStyles();

            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            float tY = sInRect.position.y;

            Rect tFinalRect = new Rect(sInRect.position.x, tY, sInRect.width, tY - sInRect.position.y);
            bool rNeedBeUpdate = false;
            EditorGUI.BeginChangeCheck();

            // Start scrollview
            if (sWithScrollview == true)
            {
                float tScrollBarMarge = 0;
                if (InspectorHeight >= sInRect.height)
                {
                    tScrollBarMarge = NWDGUI.kScrollbar;
                }
                BasisHelper().ObjectEditorScrollPosition = GUI.BeginScrollView(sInRect, BasisHelper().ObjectEditorScrollPosition, new Rect(0, 0, sInRect.width - tScrollBarMarge, InspectorHeight + NWDGUI.kFieldMarge));

                tWidth = sInRect.width - tScrollBarMarge;
                tX = 0;
                tY = 0;
            }
            EditorGUI.BeginDisabledGroup(sEditionEnable == false);
            Rect tRectDrawed = DrawInspector(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), sNodalCard, sEditionEnable);
            tY += tRectDrawed.height;
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DrawEditorBottom(Rect sInRect, bool sWithScrollview, NWDNodeCard sNodalCard, bool sEditionEnable)
        {
            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.x;
            float tXmarge = sInRect.x + NWDGUI.kFieldMarge;
            float tY = sInRect.y;

            float tButtonWidth = (tWidth - (NWDGUI.kFieldMarge * 3)) / 4.0f;


            //EditorGUI.DrawRect(new Rect(tX, tY, tWidth + NWDGUI.kFieldMarge * 2, BottomHeight), NWDGUI.kIdentityColor);
            //EditorGUI.DrawRect(new Rect(tX, tY, tWidth + NWDGUI.kFieldMarge * 2, BottomHeight), Color.red);

            tY += NWDGUI.kFieldMarge;
            // Action Zone
            GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ACTION_ZONE, NWDGUI.kBoldLabelStyle);
            tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (false)
            {
                if (GUI.Button(new Rect(tXmarge, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "UnloadMe", NWDGUI.kMiniButtonStyle))
                {
                    BasisHelper().New_SetObjectInEdition(null);
                    BasisHelper().New_UnloadDataByReference(this.Reference);
                }
                if (GUI.Button(new Rect(tXmarge + tButtonWidth + NWDGUI.kFieldMarge, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "ReloadMe", NWDGUI.kMiniButtonStyle))
                {
                    K tLoaded = LoadDataByReference(this.Reference);
                    BasisHelper().New_SetObjectInEdition(tLoaded);
                }

                tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            }


            if (GUI.Button(new Rect(tXmarge, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_EDITOR_NODAL, NWDGUI.kMiniButtonStyle))
            {
                NWDNodeEditor.SetObjectInNodeWindow(this);
            }

            if (GUI.Button(new Rect(tXmarge + tButtonWidth + NWDGUI.kFieldMarge, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_UPDATE, NWDGUI.kMiniButtonStyle))
            {
                DM = NWDToolbox.Timestamp();
                UpdateIntegrity();
                //UpdateObjectInListOfEdition(this);
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateData(true, NWDWritingMode.ByEditorDefault);
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }

            if (GUI.Button(new Rect(tXmarge + (tButtonWidth + NWDGUI.kFieldMarge) * 2, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DUPPLICATE, NWDGUI.kMiniButtonStyle))
            {

                // todo not update if not modified
                //NWDDataManager.SharedInstance().AddObjectToUpdateQueue(this);
                UpdateDataIfModified(true, NWDWritingMode.ByEditorDefault);
                //K tNexObject = (K)DuplicateMe();
                NWDTypeClass tNexObject = DuplicateData(true, NWDWritingMode.ByEditorDefault);
                //AddObjectInListOfEdition(tNexObject);
                if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                {
                    tNexObject.Tag = BasisHelper().m_SearchTag;
                    tNexObject.UpdateData();
                }
                BasisHelper().New_SetObjectInEdition(tNexObject);
                BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                NWDDataManager.SharedInstance().DataQueueExecute();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            }
            if (AC == false)
            {
                if (GUI.Button(new Rect(tXmarge + (tButtonWidth + NWDGUI.kFieldMarge) * 3, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REACTIVE, NWDGUI.kMiniButtonStyle))
                {
                    EnableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }
            else
            {
                if (GUI.Button(new Rect(tXmarge + (tButtonWidth + NWDGUI.kFieldMarge) * 3, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DISACTIVE, NWDGUI.kMiniButtonStyle))
                {
                    DisableData();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
                }
            }

            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            GUI.Label(new Rect(tXmarge, tY, tWidth, NWDGUI.kBoldLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_WARNING_ZONE, NWDGUI.kBoldLabelStyle);
            tY += NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            NWDGUI.BeginRedArea();

            EditorGUI.BeginDisabledGroup(IsTrashed());
            if (GUI.Button(new Rect(tXmarge, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_PUT_IN_TRASH, NWDGUI.kMiniButtonStyle))
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
            EditorGUI.EndDisabledGroup();

            if (GUI.Button(new Rect(tXmarge + tButtonWidth * 1 + NWDGUI.kFieldMarge * 1, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_DELETE, NWDGUI.kMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_DELETE_WARNING,
                        NWDConstants.K_APP_BASIS_DELETE_MESSAGE,
                        NWDConstants.K_APP_BASIS_DELETE_OK,
                        NWDConstants.K_APP_BASIS_DELETE_CANCEL))
                {

                    DeleteData(NWDWritingMode.ByEditorDefault);
                    BasisHelper().New_SetObjectInEdition(null);
                    BasisHelper().New_RepaintTableEditor();
                    //NWDNodeEditor.ReAnalyzeIfNecessary(this);
                }
            }
            //tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tXmarge + tButtonWidth * 2 + NWDGUI.kFieldMarge * 2, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_SHORT_REFERENCE, NWDGUI.kMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                {
                    RegenerateNewShortReference();
                }
            }
            if (GUI.Button(new Rect(tXmarge + tButtonWidth * 3 + NWDGUI.kFieldMarge * 3, tY, tButtonWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_NEW_REFERENCE, NWDGUI.kMiniButtonStyle))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_BASIS_NEW_REFERENCE_WARNING,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_MESSAGE,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_OK,
                        NWDConstants.K_APP_BASIS_NEW_REFERENCE_CANCEL))
                {
                    RegenerateNewReference();
                }
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            NWDGUI.EndRedArea();

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual float AddonEditor(Rect sInRect)
        {
            return 00.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual float AddonEditorHeight()
        //{
        //    return 00.0f;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool AddonEdited(bool sNeedBeUpdate)
        {
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual float AddOnNodeDrawWidth(float sDocumentWidth)
        //{
        //    return 250.0f;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual float AddOnNodeDrawHeight(float sCardWidth)
        //{
        //    return 130.0f;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        //public virtual void AddOnNodePropertyDraw(string sPpropertyName, Rect sRect)
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public virtual Color AddOnNodeColor()
        //{
        //    return Color.white;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_CheckError)]
        public override void ErrorCheck()
        {
            //BTBBenchmark.Start();
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
                        tBTBDataType.BaseVerif();
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
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataTypeEnum tBTBDataType = tValue as BTBDataTypeEnum;
                        if (tBTBDataType.ErrorAnalyze() == true)
                        {
                            tErrorResult = true;
                        }
                    }
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                {
                    var tValue = tProp.GetValue(this, null);
                    if (tValue != null)
                    {
                        BTBDataTypeMask tBTBDataType = tValue as BTBDataTypeMask;
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif