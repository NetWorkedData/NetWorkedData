//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.Reflection;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentSync : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        //-------------------------------------------------------------------------------------------------------------
        Texture2D DevIcon;
        Texture2D PreprodIcon;
        Texture2D ProdIcon;
        string DevProgress = "";
        string PreprodProgress = "";
        string ProdProgress = "";
        bool DevSessionExpired = false;
        bool PreprodSessionExpired = false;
        bool ProdSessionExpired = false;
        //-------------------------------------------------------------------------------------------------------------
        NWDOperationResult LastInfos = new NWDOperationResult();
        //-------------------------------------------------------------------------------------------------------------
        private BTBOperationBlock SuccessBlock = null;
        private BTBOperationBlock FailBlock = null;
        private BTBOperationBlock CancelBlock = null;
        private BTBOperationBlock ProgressBlock = null;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstance()
        {
            NWDEditorMenu.EnvironementSyncShow();
            return NWDEditorMenu.kNWDAppEnvironmentSync;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWDConstants.LoadImages();
            DevIcon = NWDConstants.kImageEmpty;
            PreprodIcon = NWDConstants.kImageEmpty;
            ProdIcon = NWDConstants.kImageEmpty;
            // SUCCESS BLOCK
            SuccessBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                //EndTime = DateTime.Now;
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                //ReceiptOctects = tInfos.OctetDownload;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDConstants.kImageGreen;
                    DevProgress = "Finished";
                    DevSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDConstants.kImageGreen;
                    PreprodProgress = "Finished";
                    PreprodSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDConstants.kImageGreen;
                    ProdProgress = "Finished";
                    ProdSessionExpired = false;
                }
                Repaint();
            };
            // FAIL BLOCK
            FailBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                //EndTime = DateTime.Now;
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                //ReceiptOctects = tInfos.OctetDownload;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDConstants.kImageRed;
                    DevProgress = "Failed";
                    if (tErrorCode.Contains("RQT"))
                    {
                        DevSessionExpired = true;
                    }
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDConstants.kImageRed;
                    PreprodProgress = "Failed";
                    if (tErrorCode.Contains("RQT"))
                    {
                        PreprodSessionExpired = true;
                    }
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDConstants.kImageRed;
                    ProdProgress = "Failed";
                    if (tErrorCode.Contains("RQT"))
                    {
                        ProdSessionExpired = true;
                    }
                }
                Repaint();
                if (LastInfos.isError)
                {
                    if (tErrorCode.Contains("RQT"))
                    {
                        EditorUtility.DisplayDialog("Alert", "Session expired (error code " + LastInfos.errorCode + ")", "Ok");
                    }
                    else
                    {
                        string tTitle = "ERROR";
                        string tDescription = "Unknown error (error code " + LastInfos.errorCode + ")";
                        if (LastInfos.errorDesc != null)
                        {
                            tTitle = LastInfos.errorDesc.Domain;
                            tDescription = LastInfos.errorDesc.Code;
                            if (LastInfos.errorDesc.Description != null)
                            {
                                tDescription += " : " + LastInfos.errorDesc.Description.GetBaseString();
                            }
                        }
                        EditorUtility.DisplayDialog(tTitle, tDescription, "Ok");
                    }
                }
                Repaint();
            };
            //CANCEL BLOCK
            CancelBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                //EndTime = DateTime.Now;
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                //ReceiptOctects = tInfos.OctetDownload;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDConstants.kImageForbidden;
                    DevProgress = "Cancelled";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDConstants.kImageForbidden;
                    PreprodProgress = "Cancelled";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDConstants.kImageForbidden;
                    ProdProgress = "Cancelled";
                }
                Repaint();
            };
            // PROGRESS BLOCK
            ProgressBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                //ReceiptOctects = tInfos.OctetDownload;

                //if (bProgress >= 1.0f)
                //{
                //    MiddleTime = DateTime.Now;
                //}
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDConstants.kImageWaiting;
                    DevProgress = (bProgress*100.0F).ToString("000.00") +"%";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDConstants.kImageWaiting;
                    PreprodProgress = (bProgress * 100.0F).ToString("000.00") + "%";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDConstants.kImageWaiting;
                    ProdProgress = (bProgress * 100.0F).ToString("000.00") + "%";
                }
                Repaint();
            };
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            // use these bools to fix the bug of error on redraw
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment; // default value replace after in script
            bool tSync = false;
            bool tSyncForce = false;
            bool tPull = false;
            bool tPullForce = false;

            bool tOperationClean = false;
            bool tOperationSpecial = false;
            bool tOperationUpgrade = false;
            bool tOperationOptimize = false;

            NWDAppEnvironment tDevEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
            NWDAppEnvironment tPreprodEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
            NWDAppEnvironment tProdEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;

            this.minSize = new Vector2(300, 500);
            this.maxSize = new Vector2(300, 4096);
            // set title of window
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDAppEnvironmentSync t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        //Debug.Log("TextureOfClass GUID " + tGUID);
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        //Debug.Log("tPathFilename = " + tPathFilename);
                        if (tPathFilename.Equals("NWDAppEnvironmentSync"))
                        {
                            //Debug.Log("TextureOfClass " + tPath);
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            // show helpbox
            EditorGUILayout.HelpBox(NWDConstants.K_APP_SYNC_ENVIRONMENT, MessageType.None);



            var tStyleCenter = new GUIStyle(EditorStyles.label);
            tStyleCenter.alignment = TextAnchor.MiddleCenter;


            var tStyleBoldCenter = new GUIStyle(EditorStyles.boldLabel);
            tStyleBoldCenter.alignment = TextAnchor.MiddleCenter;

            int tObjectInQueue = NWDDataManager.SharedInstance().DataQueueCounter();
            if (tObjectInQueue == 0)
            {
                GUILayout.Label("No Object in waiting to update", tStyleBoldCenter);
            }
            else if (tObjectInQueue == 1)
            {
                GUILayout.Label(tObjectInQueue + " Object in waiting to update", tStyleBoldCenter);
            }
            else  if (tObjectInQueue > 1)
            {
                GUILayout.Label(tObjectInQueue + " Objects in waiting to update", tStyleBoldCenter);
            }


            GUILayout.BeginHorizontal();
            GUILayout.Label("Dev Database", tStyleBoldCenter);
            GUILayout.Label("Preprod Database", tStyleBoldCenter);
            GUILayout.Label("Prod Database", tStyleBoldCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Sync all", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Force Sync all ", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Force Sync all ", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Dev Database", tStyleBoldCenter);
            GUILayout.Label("Preprod Database", tStyleBoldCenter);
            GUILayout.Label("Prod Database", tStyleBoldCenter);
            GUILayout.EndHorizontal();

            Color tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tOperationClean = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tOperationClean = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tOperationClean = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Special all", EditorStyles.miniButton))
            {
                tOperationSpecial = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Special all", EditorStyles.miniButton))
            {
                tOperationSpecial = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Special all", EditorStyles.miniButton))
            {
                tOperationSpecial = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Upgrade all", EditorStyles.miniButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Upgrade all", EditorStyles.miniButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Upgrade all", EditorStyles.miniButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Optimize all", EditorStyles.miniButton))
            {
                tOperationOptimize = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Optimize all", EditorStyles.miniButton))
            {
                tOperationOptimize = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Optimize all", EditorStyles.miniButton))
            {
                tOperationOptimize = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = tOldColor;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Dev Database", tStyleBoldCenter);
            GUILayout.Label("Preprod Database", tStyleBoldCenter);
            GUILayout.Label("Prod Database", tStyleBoldCenter);
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.Label(DevProgress, tStyleBoldCenter);
            //GUILayout.Label(PreprodProgress, tStyleBoldCenter);
            //GUILayout.Label(ProdProgress, tStyleBoldCenter);
            //GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(DevIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.Label(PreprodIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.Label(ProdIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.EndHorizontal();

            double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
            double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
            double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());

            // add separator please

            if (LastInfos.RowPushCounter == 0)
            {
                EditorGUILayout.LabelField("Rows pushed", LastInfos.RowPushCounter.ToString() + " no row (no class)");
            }
            else if (LastInfos.RowPushCounter == 1)
            {
                EditorGUILayout.LabelField("Rows pushed", LastInfos.RowPushCounter.ToString() + " row (" + LastInfos.ClassPushCounter.ToString() + " class)");
            }
            else
            {
                if (LastInfos.ClassPushCounter == 1)
                {
                    EditorGUILayout.LabelField("Rows pushed", LastInfos.RowPushCounter.ToString() + " rows (" + LastInfos.ClassPushCounter.ToString() + " class)");
                }
                else
                {
                    EditorGUILayout.LabelField("Rows pushed", LastInfos.RowPushCounter.ToString() + " rows (" + LastInfos.ClassPushCounter.ToString() + " classes)");
                }
            }

            EditorGUILayout.LabelField("Data Prepare", tPrepareNetMilliseconds.ToString("#0.000") + " s");
            EditorGUILayout.LabelField("Network Upload", tUploadNetMilliseconds.ToString("#0.000") + " s");
            float tKoUpload = (float)LastInfos.OctetUpload / 1024.0F;
            float tMoUpload = tKoUpload / 1024.0F;
            EditorGUILayout.LabelField("Octect send", LastInfos.OctetUpload.ToString() + " o = " + tKoUpload.ToString("0.0") + "Ko = " + tMoUpload.ToString("0.0") + "Mo");
            EditorGUILayout.LabelField("Server Perform Request", LastInfos.performRequest.ToString("#0.000") + " s");
            EditorGUILayout.LabelField("Network Download", tDowloadNetMilliseconds.ToString("#0.000") + " s");
            float tKoDownload = (float)LastInfos.OctetDownload / 1024.0F;
            float tMoDownload = tKoDownload / 1024.0F;
            EditorGUILayout.LabelField("Octect receipt", LastInfos.OctetDownload.ToString() + " o = " + tKoDownload.ToString("0.0") + "Ko = " + tMoDownload.ToString("0.0") + "Mo");
           
            //RowPullCounter
            if (LastInfos.RowPullCounter == 0)
            {
                EditorGUILayout.LabelField("Rows pulled", " no row (no class)");
            }
            else if (LastInfos.RowPullCounter == 1)
            {
                EditorGUILayout.LabelField("Rows pulled", LastInfos.RowPullCounter.ToString() + " row (" + LastInfos.ClassPullCounter.ToString() + " class)");
            }
            else
            {
                if (LastInfos.ClassPullCounter == 1)
                {
                    EditorGUILayout.LabelField("Rows pulled", LastInfos.RowPullCounter.ToString() + " rows (" + LastInfos.ClassPullCounter.ToString() + " class)");
                }
                else
                {
                    EditorGUILayout.LabelField("Rows pulled", LastInfos.RowPullCounter.ToString() + " rows (" + LastInfos.ClassPullCounter.ToString() + " classes)");
                }
            }

            //RowUpdatedCounter
            if (LastInfos.RowUpdatedCounter == 0)
            {
                EditorGUILayout.LabelField("Rows updated", " no row");
            }
            else if (LastInfos.RowUpdatedCounter == 1)
            {
                EditorGUILayout.LabelField("Rows updated", LastInfos.RowUpdatedCounter.ToString() + " row ");
            }
            else
            {
                EditorGUILayout.LabelField("Rows updated", LastInfos.RowUpdatedCounter.ToString() + " rows");
            }

            //RowUpdatedCounter
            if (LastInfos.RowAddedCounter == 0)
            {
                EditorGUILayout.LabelField("Rows added", " no row");
            }
            else if (LastInfos.RowAddedCounter == 1)
            {
                EditorGUILayout.LabelField("Rows added", LastInfos.RowAddedCounter.ToString() + " row ");
            }
            else
            {
                EditorGUILayout.LabelField("Rows added", LastInfos.RowAddedCounter.ToString() + " rows");
            }

            EditorGUILayout.LabelField("DataBase compute", tComputeNetMilliseconds.ToString("#0.000") + " s");
            EditorGUILayout.LabelField("Sync duration", tDurationNetMilliseconds.ToString("#0.000") + " s", EditorStyles.boldLabel);

            if (DevSessionExpired == true || PreprodSessionExpired == true || ProdSessionExpired == true)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!DevSessionExpired);
                if (GUILayout.Button("Reset token", EditorStyles.miniButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!PreprodSessionExpired);
                if (GUILayout.Button("Reset token", EditorStyles.miniButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!ProdSessionExpired);
                if (GUILayout.Button("Reset token", EditorStyles.miniButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Flush web queue", EditorStyles.miniButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Flush web queue", EditorStyles.miniButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Flush web queue", EditorStyles.miniButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20.0F);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Dev WS", tStyleBoldCenter);
            GUILayout.Label("Preprod WS", tStyleBoldCenter);
            GUILayout.Label("Prod WS", tStyleBoldCenter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            if (GUILayout.Button("maintenance", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetMaintenance(true);
            }
            if (GUILayout.Button("maintenance", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetMaintenance(true);
            }
            if (GUILayout.Button("maintenance", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetMaintenance(true);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("obsolete", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetObsolete(true);
            }
            if (GUILayout.Button("obsolete", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetObsolete(true);
            }
            if (GUILayout.Button("obsolete", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetObsolete(true);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("activate", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetActivate();
            }
            if (GUILayout.Button("activate", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetActivate();
            }
            if (GUILayout.Button("activate", EditorStyles.miniButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetActivate();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20.0F);

            GUILayout.Label("Local database", tStyleBoldCenter);

            if (GUILayout.Button("Clean all local tables", EditorStyles.miniButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_CLEAN_ALERT_TITLE,
                        NWDConstants.K_CLEAN_ALERT_MESSAGE,
                        NWDConstants.K_CLEAN_ALERT_OK,
                        NWDConstants.K_CLEAN_ALERT_CANCEL))
                {
                    NWDDataManager.SharedInstance().CleanAllTablesLocal();
                }
            }
            if (GUILayout.Button("Purge all local tables", EditorStyles.miniButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
                        NWDConstants.K_PURGE_ALERT_MESSAGE,
                        NWDConstants.K_PURGE_ALERT_OK,
                        NWDConstants.K_PURGE_ALERT_CANCEL))
                {
                    NWDDataManager.SharedInstance().PurgeAllTablesLocal();
                }
            }

            GUILayout.Label("TESTS ANTI-HACK");
            //if (GUILayout.Button("ReInject last request result", EditorStyles.miniButton))
            //{

            //}
            if (GUILayout.Button("Use false token", EditorStyles.miniButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDToolbox.RandomStringUnix(16);
            }
            if (GUILayout.Button("ReUse Last token", EditorStyles.miniButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().PreviewRequesToken;
            }
            if (GUILayout.Button("ReUse Last Last token", EditorStyles.miniButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().LastPreviewRequesToken;
            }
            GUI.backgroundColor = tOldColor;

            // Show version selected
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            NWDAccount tAccount = NWDAccount.CurrentAccount();
            if (tAccount != null)
            {
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
            }
            // run button selected (if GUI prevent)
            if (tSync == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, false);
            }
            if (tSyncForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true);
            }
            if (tPull == true)
            {
                OperationSynchroAllClasses(tEnvironment,false, false, NWDOperationSpecial.Pull);
            }
            if (tPullForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true, NWDOperationSpecial.Pull);
            }
            if (tOperationClean == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Clean);
            }
            if (tOperationUpgrade == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Upgrade);
            }
            if (tOperationOptimize == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Optimize);
            }
            if (tOperationSpecial == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Special);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchroAllClasses(NWDAppEnvironment sEnvironment,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            OperationSynchro( sEnvironment,NWDDataManager.SharedInstance().mTypeSynchronizedList, sForceSync, sPriority, sOperation);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchro(NWDAppEnvironment sEnvironment,
                                           List<Type> sTypeList = null,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            bool tOk = false;
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                        NWDConstants.K_SYNC_ALERT_MESSAGE,
                        NWDConstants.K_SYNC_ALERT_OK,
                        NWDConstants.K_SYNC_ALERT_CANCEL))
                {
                    tOk = true;
                }
            }
            else
            {
                tOk = true;
            }
            if (tOk == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("App Environnement Sync " + sOperation.ToString(), SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, sForceSync, sPriority, sOperation);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Reset(NWDAppEnvironment sEnvironment)
        {
            StartProcess(sEnvironment);
            sEnvironment.ResetPreferences();
            // TODO : add message in window
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                DevIcon = NWDConstants.kImageEmpty;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreprodIcon = NWDConstants.kImageEmpty;
                PreprodSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = NWDConstants.kImageEmpty;
                ProdSessionExpired = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush(NWDAppEnvironment sEnvironment)
        {
            StartProcess(sEnvironment);
            NWDDataManager.SharedInstance().WebOperationQueue.Flush(sEnvironment.Environment);
            // TODO : add message in window
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartProcess(NWDAppEnvironment sEnvironment)
        {
            DevIcon = NWDConstants.kImageEmpty;
            PreprodIcon = NWDConstants.kImageEmpty;
            ProdIcon = NWDConstants.kImageEmpty;

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                DevIcon = NWDConstants.kImageEmpty;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreprodIcon = NWDConstants.kImageEmpty;
                PreprodSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = NWDConstants.kImageEmpty;
                ProdSessionExpired = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
