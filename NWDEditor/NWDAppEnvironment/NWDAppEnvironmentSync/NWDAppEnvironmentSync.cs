//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentSync : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDAppEnvironmentSync kSharedInstance;
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
        int SyncInfosTab = 0;
        Vector2 ScrollPosition;
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
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentSync)) as NWDAppEnvironmentSync;
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstanceFocus()
        {
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstance()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            DevIcon = NWDConstants.kImageEmpty;
            PreprodIcon = NWDConstants.kImageEmpty;
            ProdIcon = NWDConstants.kImageEmpty;
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDAppEnvironmentSync t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDAppEnvironmentSync"))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            // SUCCESS BLOCK
            SuccessBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                //EndTime = DateTime.Now;
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
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
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDConstants.kImageWaiting;
                    DevProgress = (bProgress * 100.0F).ToString("000.00") + "%";
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

            SyncInfosTab = EditorPrefs.GetInt("SyncInfosTab");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDConstants.LoadStyles();
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
            WebServicesSync();
            WebServicesLastSync();
            WebServicesTools();
            HackTools();
            WritingDatabaseState();
            DatasLocal();
            EditorGUILayout.EndScrollView();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WritingDatabaseState()
        {
            GUILayout.Label("Database writing", NWDConstants.kLabelTitleStyle);

            // use these bools to fix the bug of error on redraw
            this.minSize = new Vector2(300, 500);
            this.maxSize = new Vector2(300, 4096);

            int tObjectInQueue = NWDDataManager.SharedInstance().DataQueueCounter();
            if (tObjectInQueue == 0)
            {
                GUILayout.Label("No Object in waiting to update");
            }
            else if (tObjectInQueue == 1)
            {
                GUILayout.Label(tObjectInQueue + " Object in waiting to update");
            }
            else if (tObjectInQueue > 1)
            {
                GUILayout.Label(tObjectInQueue + " Objects in waiting to update");
            }
            GUILayout.Space(NWDConstants.kFieldMarge);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesSync()
        {
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
            NWDAppEnvironment tDevEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
            NWDAppEnvironment tPreprodEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
            NWDAppEnvironment tProdEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            bool tSync = false;
            bool tSyncForce = false;
            bool tPull = false;
            bool tPullForce = false;
            bool tOperationClean = false;
            bool tOperationSpecial = false;
            bool tOperationUpgrade = false;
            bool tOperationOptimize = false;
            GUILayout.Label("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " tools", NWDConstants.kLabelTitleStyle);



            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Dev Database", NWDConstants.KTableSearchTitle);

            if (GUILayout.Button("Sync all", NWDConstants.KTableSearchButton))
            {
                tSync = true;
                tEnvironment = tDevEnvironment;
            }

            if (GUILayout.Button("Force Sync all", NWDConstants.KTableSearchButton))
            {
                tSyncForce = true;
                tEnvironment = tDevEnvironment;
            }

            if (GUILayout.Button("Pull all", NWDConstants.KTableSearchButton))
            {
                tPull = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Force Pull all", NWDConstants.KTableSearchButton))
            {
                tPullForce = true;
                tEnvironment = tDevEnvironment;
            }
            GUILayout.Label("Dev Database", NWDConstants.KTableSearchTitle);

            NWDConstants.GUIRedButtonBegin();
            if (GUILayout.Button("Clean all", NWDConstants.KTableSearchButton))
            {
                tOperationClean = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Special all", NWDConstants.KTableSearchButton))
            {
                tOperationSpecial = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Upgrade all", NWDConstants.KTableSearchButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tDevEnvironment;
            }
            if (GUILayout.Button("Optimize all", NWDConstants.KTableSearchButton))
            {
                tOperationOptimize = true;
                tEnvironment = tDevEnvironment;
            }
            NWDConstants.GUIRedButtonEnd();
            GUILayout.Label("Dev Database", NWDConstants.KTableSearchTitle);
            GUILayout.Label(DevIcon, NWDConstants.KTableSearchIcon, GUILayout.Height(20));
            GUILayout.Label("Dev Database", NWDConstants.KTableSearchTitle);
            if (GUILayout.Button("Flush web queue", NWDConstants.KTableSearchButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            EditorGUI.BeginDisabledGroup(!DevSessionExpired);
            if (GUILayout.Button("Reset token", NWDConstants.KTableSearchButton))
            {
                Reset(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            GUILayout.Label("Preprod Database", NWDConstants.KTableSearchTitle);

            if (GUILayout.Button("Sync all", NWDConstants.KTableSearchButton))
            {
                tSync = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Force Sync all ", NWDConstants.KTableSearchButton))
            {
                tSyncForce = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Pull all", NWDConstants.KTableSearchButton))
            {
                tPull = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Force Pull all", NWDConstants.KTableSearchButton))
            {
                tPullForce = true;
                tEnvironment = tPreprodEnvironment;
            }
            NWDConstants.GUIRedButtonBegin();
            GUILayout.Label("Preprod Database", NWDConstants.KTableSearchTitle);

            if (GUILayout.Button("Clean all", NWDConstants.KTableSearchButton))
            {
                tOperationClean = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Special all", NWDConstants.KTableSearchButton))
            {
                tOperationSpecial = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Upgrade all", NWDConstants.KTableSearchButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tPreprodEnvironment;
            }
            if (GUILayout.Button("Optimize all", NWDConstants.KTableSearchButton))
            {
                tOperationOptimize = true;
                tEnvironment = tPreprodEnvironment;
            }

            NWDConstants.GUIRedButtonEnd();
            GUILayout.Label("Preprod Database", NWDConstants.KTableSearchTitle);
            GUILayout.Label(PreprodIcon, NWDConstants.KTableSearchIcon, GUILayout.Height(20));
            GUILayout.Label("Preprod Database", NWDConstants.KTableSearchTitle);
            if (GUILayout.Button("Flush web queue", NWDConstants.KTableSearchButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(!PreprodSessionExpired);
            if (GUILayout.Button("Reset token", NWDConstants.KTableSearchButton))
            {
                Reset(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Prod Database", NWDConstants.KTableSearchTitle);

            if (GUILayout.Button("Sync all", NWDConstants.KTableSearchButton))
            {
                tSync = true;
                tEnvironment = tProdEnvironment;
            }
            if (GUILayout.Button("Force Sync all ", NWDConstants.KTableSearchButton))
            {
                tSyncForce = true;
                tEnvironment = tProdEnvironment;
            }
            if (GUILayout.Button("Pull all", NWDConstants.KTableSearchButton))
            {
                tPull = true;
                tEnvironment = tProdEnvironment;
            }
            if (GUILayout.Button("Force Pull all", NWDConstants.KTableSearchButton))
            {
                tPullForce = true;
                tEnvironment = tProdEnvironment;
            }
            GUILayout.Label("Prod Database", NWDConstants.KTableSearchTitle);
            NWDConstants.GUIRedButtonBegin();

            if (GUILayout.Button("Clean all", NWDConstants.KTableSearchButton))
            {
                tOperationClean = true;
                tEnvironment = tProdEnvironment;
            }

            if (GUILayout.Button("Special all", NWDConstants.KTableSearchButton))
            {
                tOperationSpecial = true;
                tEnvironment = tProdEnvironment;
            }
            if (GUILayout.Button("Upgrade all", NWDConstants.KTableSearchButton))
            {
                tOperationUpgrade = true;
                tEnvironment = tProdEnvironment;
            }
            if (GUILayout.Button("Optimize all", NWDConstants.KTableSearchButton))
            {
                tOperationOptimize = true;
                tEnvironment = tProdEnvironment;
            }


            NWDConstants.GUIRedButtonEnd();
            GUILayout.Label("Prod Database", NWDConstants.KTableSearchTitle);
            GUILayout.Label(ProdIcon, NWDConstants.KTableSearchIcon, GUILayout.Height(20));
            GUILayout.Label("Prod Database", NWDConstants.KTableSearchTitle);
            if (GUILayout.Button("Flush web queue", NWDConstants.KTableSearchButton))
            {
                Flush(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            EditorGUI.BeginDisabledGroup(!ProdSessionExpired);
            if (GUILayout.Button("Reset token", NWDConstants.KTableSearchButton))
            {
                Reset(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();

            // run button selected (if GUI prevent)
            if (tSync == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, false);
                GUIUtility.ExitGUI();
            }
            if (tSyncForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true);
                GUIUtility.ExitGUI();
            }
            if (tPull == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, false, NWDOperationSpecial.Pull);
                GUIUtility.ExitGUI();
            }
            if (tPullForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true, NWDOperationSpecial.Pull);
                GUIUtility.ExitGUI();
            }
            if (tOperationClean == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Clean);
                GUIUtility.ExitGUI();
            }
            if (tOperationUpgrade == true)
            {
                OperationManagement(tEnvironment, true);
                GUIUtility.ExitGUI();
            }
            if (tOperationOptimize == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Optimize);
                GUIUtility.ExitGUI();
            }
            if (tOperationSpecial == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Special);
                GUIUtility.ExitGUI();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesLastSync()
        {
            double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
            double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
            double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;

            GUILayout.Label("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " last request", NWDConstants.kLabelTitleStyle);

            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);

            int tSyncInfosTab = GUILayout.Toolbar(SyncInfosTab, new string[] { "all", "seconds", "rows", "weight" });
            if (SyncInfosTab != tSyncInfosTab)
            {
                SyncInfosTab = tSyncInfosTab;
                EditorPrefs.SetInt("SyncInfosTab", SyncInfosTab);
            }


            // add separator please
            if (SyncInfosTab == 0 || SyncInfosTab == 2)
            {
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
            }
            if (SyncInfosTab == 0 || SyncInfosTab == 1)
            {
                EditorGUILayout.LabelField("Data Prepare", tPrepareNetMilliseconds.ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Network Upload", tUploadNetMilliseconds.ToString("#0.000") + " s");
            }
            if (SyncInfosTab == 0 || SyncInfosTab == 3)
            {
                float tKoUpload = (float)LastInfos.OctetUpload / 1024.0F;
                float tMoUpload = tKoUpload / 1024.0F;
                EditorGUILayout.LabelField("Octect send", LastInfos.OctetUpload.ToString() + " o = " + tKoUpload.ToString("0.0") + "Ko = " + tMoUpload.ToString("0.0") + "Mo");
            }

            if (SyncInfosTab == 0 || SyncInfosTab == 1)
            {
                EditorGUILayout.LabelField("Server Perform Request", LastInfos.performRequest.ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Network Download", (tDowloadNetMilliseconds - LastInfos.performRequest).ToString("#0.000") + " s");
            }
            if (SyncInfosTab == 0 || SyncInfosTab == 3)
            {
                float tKoDownload = (float)LastInfos.OctetDownload / 1024.0F;
                float tMoDownload = tKoDownload / 1024.0F;
                EditorGUILayout.LabelField("Octect receipt", LastInfos.OctetDownload.ToString() + " o = " + tKoDownload.ToString("0.0") + "Ko = " + tMoDownload.ToString("0.0") + "Mo");
            }

            if (SyncInfosTab == 0 || SyncInfosTab == 2)
            {
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
            }

            if (SyncInfosTab == 0 || SyncInfosTab == 1)
            {
                EditorGUILayout.LabelField("DataBase compute", tComputeNetMilliseconds.ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Sync duration", tDurationNetMilliseconds.ToString("#0.000") + " s", EditorStyles.boldLabel);
            }
            GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesTools()
        {
            GUILayout.Label("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " tools", NWDConstants.kLabelTitleStyle);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUILayout.Label("Dev WS", NWDConstants.KTableSearchTitle);
            NWDConstants.GUIRedButtonBegin();
            if (GUILayout.Button("maintenance", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetMaintenance(true);
            }
            if (GUILayout.Button("obsolete", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetObsolete(true);
            }
            if (GUILayout.Button("activate", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.SetActivate();
            }
            NWDConstants.GUIRedButtonEnd();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Preprod WS", NWDConstants.KTableSearchTitle);
            NWDConstants.GUIRedButtonBegin();
            if (GUILayout.Button("maintenance", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetMaintenance(true);
            }
            if (GUILayout.Button("obsolete", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetObsolete(true);
            }
            if (GUILayout.Button("activate", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetActivate();
            }
            NWDConstants.GUIRedButtonEnd();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Prod WS", NWDConstants.KTableSearchTitle);
            NWDConstants.GUIRedButtonBegin();
            if (GUILayout.Button("maintenance", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetMaintenance(true);
            }
            if (GUILayout.Button("obsolete", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetObsolete(true);
            }
            if (GUILayout.Button("activate", NWDConstants.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SetActivate();
            }
            NWDConstants.GUIRedButtonEnd();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DatasLocal()
        {
            //GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();
            //GUILayout.Space(NWDConstants.kFieldMarge);

            GUILayout.Label("Local datas", NWDConstants.kLabelTitleStyle);

            if (GUILayout.Button("Clean all local tables", NWDConstants.KTableSearchButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_CLEAN_ALERT_TITLE,
                        NWDConstants.K_CLEAN_ALERT_MESSAGE,
                        NWDConstants.K_CLEAN_ALERT_OK,
                        NWDConstants.K_CLEAN_ALERT_CANCEL))
                {
                    NWDDataManager.SharedInstance().CleanAllTablesLocal();
                }
            }
            if (GUILayout.Button("Purge all local tables", NWDConstants.KTableSearchButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
                        NWDConstants.K_PURGE_ALERT_MESSAGE,
                        NWDConstants.K_PURGE_ALERT_OK,
                        NWDConstants.K_PURGE_ALERT_CANCEL))
                {
                    NWDDataManager.SharedInstance().PurgeAllTablesLocal();
                }
            }

            GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void HackTools()
        {
            GUILayout.Label("Test anti-hack", NWDConstants.kLabelTitleStyle);

            if (GUILayout.Button("Use false token", NWDConstants.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDToolbox.RandomStringUnix(16);
            }
            if (GUILayout.Button("ReUse Last token", NWDConstants.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().PreviewRequesToken;
            }
            if (GUILayout.Button("ReUse Last Last token", NWDConstants.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().LastPreviewRequesToken;
            }
            NWDConstants.GUIRedButtonEnd();

            //NWDAccount tAccount = NWDAccount.CurrentAccount();
            //if (tAccount != null)
            //{
            //    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
            //    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
            //    if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
            //    {
            //        NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
            //    }
            //}

            GUILayout.Space(NWDConstants.kFieldMarge);
            NWDConstants.GUILayoutLine();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchroAllClasses(NWDAppEnvironment sEnvironment,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            OperationSynchro(sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, sForceSync, sPriority, sOperation);
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
        public void OperationManagement(NWDAppEnvironment sEnvironment, bool sForceSync = true)
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
                NWDOperationWebManagement.AddOperation("App Table management Sync ", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sForceSync);
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
