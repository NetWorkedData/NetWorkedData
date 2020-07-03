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
#endif
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
using System.IO;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentSync : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        private static NWDAppEnvironmentSync kSharedInstance;
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
        NWDOperationResult LastInfos; // = new NWDOperationResult();
        //-------------------------------------------------------------------------------------------------------------
        private NWEOperationBlock SuccessBlock = null;
        private NWEOperationBlock FailBlock = null;
        private NWEOperationBlock CancelBlock = null;
        private NWEOperationBlock ProgressBlock = null;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentSync)) as NWDAppEnvironmentSync;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppEnvironmentSync));
            foreach (NWDAppEnvironmentSync tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstance()
        {
            //NWEBenchmark.Start();
            bool rReturn = false;
            if (kSharedInstance != null)
            {
                rReturn = true;
            }
            return rReturn;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            TitleInit(NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE, typeof(NWDAppEnvironmentSync));

            if (LastInfos == null)
            {
                LastInfos = new NWDOperationResult();
            }

            DevIcon = NWDGUI.kImageWaiting;
            PreprodIcon = NWDGUI.kImageWaiting;
            ProdIcon = NWDGUI.kImageWaiting;
            // SUCCESS BLOCK
            SuccessBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
            {
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDGUI.kImageGreen;
                    DevProgress = "Finished";
                    DevSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDGUI.kImageGreen;
                    PreprodProgress = "Finished";
                    PreprodSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDGUI.kImageGreen;
                    ProdProgress = "Finished";
                    ProdSessionExpired = false;
                }
                Repaint();
            };
            // FAIL BLOCK
            FailBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
            {
                if (bOperation != null)
                {
                    LastInfos = (NWDOperationResult)bInfos;
                    NWDError tError = LastInfos.errorDesc;
                    string tErrorCode = LastInfos.errorCode;
                    if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                    {
                        DevIcon = NWDGUI.kImageRed;
                        DevProgress = "Failed";
                        if (tErrorCode.Contains("RQT"))
                        {
                            DevSessionExpired = true;
                        }
                    }
                    if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                    {
                        PreprodIcon = NWDGUI.kImageRed;
                        PreprodProgress = "Failed";
                        if (tErrorCode.Contains("RQT"))
                        {
                            PreprodSessionExpired = true;
                        }
                    }
                    if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                    {
                        ProdIcon = NWDGUI.kImageRed;
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
                            Debug.LogWarning("Alert Session expired(error code " + LastInfos.errorCode);
                            //EditorUtility.DisplayDialog("Alert", "Session expired (error code " + LastInfos.errorCode + ")", "Ok");
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
#if UNITY_EDITOR
                                Debug.LogWarning("" + tTitle + " " + tDescription);
                                //LastInfos.errorDesc.ShowAlert(LastInfos.errorInfos);
#endif
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Operation hardly failed!!");
                }
                Repaint();
            };
            //CANCEL BLOCK
            CancelBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
            {
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDGUI.kImageSyncForbidden;
                    DevProgress = "Cancelled";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDGUI.kImageSyncForbidden;
                    PreprodProgress = "Cancelled";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDGUI.kImageSyncForbidden;
                    ProdProgress = "Cancelled";
                }
                Repaint();
            };
            // PROGRESS BLOCK
            ProgressBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
            {
                LastInfos = (NWDOperationResult)bInfos;
                NWDError tError = LastInfos.errorDesc;
                string tErrorCode = LastInfos.errorCode;
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment)
                {
                    DevIcon = NWDGUI.kImageSyncWaiting;
                    DevProgress = (bProgress * 100.0F).ToString("000.00") + "%";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreprodIcon = NWDGUI.kImageSyncWaiting;
                    PreprodProgress = (bProgress * 100.0F).ToString("000.00") + "%";
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = NWDGUI.kImageSyncWaiting;
                    ProdProgress = (bProgress * 100.0F).ToString("000.00") + "%";
                }
                Repaint();
            };
            SyncInfosTab = NWDProjectPrefs.GetInt("SyncInfosTab");
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("WebService Sync");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            WebServicesSync();
            WebServicesLastSync();
            WebServicesTools();
            HackTools();
            WritingDatabaseState();
            //DatasLocal();
            NWDGUILayout.BigSpace();
            EditorGUILayout.EndScrollView();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WritingDatabaseState()
        {
            //NWEBenchmark.Start();
            NWDGUILayout.Section("Database writing");
            // use these bools to fix the bug of error on redraw
            this.minSize = new Vector2(300, 200);
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
            //NWEBenchmark.Finsih();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesSync()
        {
            //NWEBenchmark.Start();
            float tWidthThird = Mathf.Floor((position.width - NWDGUI.KTAB_BAR_HEIGHT) / 3.0F);
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
            bool tOperationIndexes = false;
            bool tOperationBlank = false;
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev Database", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tDevEnvironment;
                }

                if (GUILayout.Button("Force Sync all", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tDevEnvironment;
                }

                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Force Pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tDevEnvironment;
                }
                GUILayout.Label("Dev Database", NWDGUI.KTableSearchTitle);
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Clean all", NWDGUI.KTableSearchButton))
                {
                    tOperationClean = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Special all", NWDGUI.KTableSearchButton))
                {
                    tOperationSpecial = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Upgrade all", NWDGUI.KTableSearchButton))
                {
                    tOperationUpgrade = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Optimize all", NWDGUI.KTableSearchButton))
                {
                    tOperationOptimize = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Index all", NWDGUI.KTableSearchButton))
                {
                    tOperationIndexes = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tDevEnvironment;
                }
                NWDGUI.EndRedArea();
                GUILayout.Label("Dev Database", NWDGUI.KTableSearchTitle);
                GUILayout.Label(DevIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
                GUILayout.Label("Dev Database", NWDGUI.KTableSearchTitle);
                if (GUILayout.Button("Flush web queue", NWDGUI.KTableSearchButton))
                {
                    Flush(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.BeginDisabledGroup(!DevSessionExpired);
                if (GUILayout.Button("Reset token", NWDGUI.KTableSearchButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Preprod Database", NWDGUI.KTableSearchTitle);

            if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Force Sync all ", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Force Pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tPreprodEnvironment;
                }
                GUILayout.Label("Preprod Database", NWDGUI.KTableSearchTitle);
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Clean all", NWDGUI.KTableSearchButton))
                {
                    tOperationClean = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Special all", NWDGUI.KTableSearchButton))
                {
                    tOperationSpecial = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Upgrade all", NWDGUI.KTableSearchButton))
                {
                    tOperationUpgrade = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Optimize all", NWDGUI.KTableSearchButton))
                {
                    tOperationOptimize = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Index all", NWDGUI.KTableSearchButton))
                {
                    tOperationIndexes = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tPreprodEnvironment;
                }
                NWDGUI.EndRedArea();
                GUILayout.Label("Preprod Database", NWDGUI.KTableSearchTitle);
                GUILayout.Label(PreprodIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
                GUILayout.Label("Preprod Database", NWDGUI.KTableSearchTitle);
                if (GUILayout.Button("Flush web queue", NWDGUI.KTableSearchButton))
                {
                    Flush(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                EditorGUI.BeginDisabledGroup(!PreprodSessionExpired);
                if (GUILayout.Button("Reset token", NWDGUI.KTableSearchButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Prod Database", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Force Sync all ", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Force Pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tProdEnvironment;
                }
                GUILayout.Label("Prod Database", NWDGUI.KTableSearchTitle);
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Clean all", NWDGUI.KTableSearchButton))
                {
                    tOperationClean = true;
                    tEnvironment = tProdEnvironment;
                }

                if (GUILayout.Button("Special all", NWDGUI.KTableSearchButton))
                {
                    tOperationSpecial = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Upgrade all", NWDGUI.KTableSearchButton))
                {
                    tOperationUpgrade = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Optimize all", NWDGUI.KTableSearchButton))
                {
                    tOperationOptimize = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Index all", NWDGUI.KTableSearchButton))
                {
                    tOperationIndexes = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tProdEnvironment;
                }
                NWDGUI.EndRedArea();
                GUILayout.Label("Prod Database", NWDGUI.KTableSearchTitle);
                GUILayout.Label(ProdIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
                GUILayout.Label("Prod Database", NWDGUI.KTableSearchTitle);
                if (GUILayout.Button("Flush web queue", NWDGUI.KTableSearchButton))
                {
                    Flush(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                }
                EditorGUI.BeginDisabledGroup(!ProdSessionExpired);
                if (GUILayout.Button("Reset token", NWDGUI.KTableSearchButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDGUI.kFieldMarge);
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
            if (tOperationIndexes == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Indexes);
                GUIUtility.ExitGUI();
            }
            if (tOperationBlank == true)
            {
                OperationSynchroBlank(tEnvironment);
                GUIUtility.ExitGUI();
            }
            if (tOperationSpecial == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Special);
                GUIUtility.ExitGUI();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesLastSync()
        {
            //NWEBenchmark.Start();
            if (LastInfos == null)
            {
                LastInfos = new NWDOperationResult();
            }
            //double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            //double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            //double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
            //double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
            //double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;


            NWDGUILayout.Section("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " last request");
            EditorGUILayout.LabelField("URL",LastInfos.URL);
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            int tSyncInfosTab = GUILayout.Toolbar(SyncInfosTab, new string[] { "all", "seconds", "rows", "weight" });
            if (SyncInfosTab != tSyncInfosTab)
            {
                SyncInfosTab = tSyncInfosTab;
                NWDProjectPrefs.SetInt("SyncInfosTab", SyncInfosTab);
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
                EditorGUILayout.LabelField("Data Prepare", (LastInfos.Benchmark.GetPrepareTime() / 1000.0F).ToString("#0.000") + " s");
                //EditorGUILayout.LabelField("Network Upload", (LastInfos.Benchmark.GetUploadTime() / 1000.0F).ToString("#0.000") + " s");
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
                EditorGUILayout.LabelField("Server Perform", LastInfos.perform.ToString("#0.000") + " s");
                //EditorGUILayout.LabelField("Network Download", (LastInfos.Benchmark.GetDownloadTime() / 1000.0F).ToString("#0.000") + " s");
                //EditorGUILayout.LabelField("Network Download", (tDowloadNetMilliseconds - LastInfos.performRequest).ToString("#0.000") + " s");
                double tNetWork = LastInfos.Benchmark.GetUploadTime() + LastInfos.Benchmark.GetPerformTime() + LastInfos.Benchmark.GetDownloadTime() - LastInfos.perform;
                if (tNetWork<0)
                {
                    tNetWork = 0; // network in progress... dont calculate!
                }
                EditorGUILayout.LabelField("Network Perform", (tNetWork / 1000.0F).ToString("#0.000") + " s");
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
                EditorGUILayout.LabelField("DataBase compute", (LastInfos.Benchmark.GetComputeTime() / 1000.0F).ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Sync duration", (LastInfos.Benchmark.GetTotalTime()/1000.0F).ToString("#0.000") + " s", EditorStyles.boldLabel);
            }
            GUILayout.Space(NWDGUI.kFieldMarge);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesTools()
        {
            //NWEBenchmark.Start();
            float tWidthThird = Mathf.Floor((position.width - NWDGUI.KTAB_BAR_HEIGHT) / 3.0F);
            NWDGUILayout.Section("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " tools");
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev WS", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("maintenance", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().DevEnvironment.SetMaintenance(true);
                }
                if (GUILayout.Button("obsolete", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().DevEnvironment.SetObsolete(true);
                }
                if (GUILayout.Button("activate", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().DevEnvironment.SetActivate();
                }
                NWDGUI.EndRedArea();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Preprod WS", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("maintenance", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetMaintenance(true);
                }
                if (GUILayout.Button("obsolete", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetObsolete(true);
                }
                if (GUILayout.Button("activate", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetActivate();
                }
                NWDGUI.EndRedArea();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Prod WS", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("maintenance", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.SetMaintenance(true);
                }
                if (GUILayout.Button("obsolete", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.SetObsolete(true);
                }
                if (GUILayout.Button("activate", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.SetActivate();
                }
                NWDGUI.EndRedArea();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDGUI.kFieldMarge);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void DatasLocal()
        //{
        //    //NWEBenchmark.Start();
        //    NWDGUILayout.Section("Local datas");
        //    if (GUILayout.Button("Clean all local tables", NWDGUI.KTableSearchButton))
        //    {
        //        if (EditorUtility.DisplayDialog(NWDConstants.K_CLEAN_ALERT_TITLE,
        //                NWDConstants.K_CLEAN_ALERT_MESSAGE,
        //                NWDConstants.K_CLEAN_ALERT_OK,
        //                NWDConstants.K_CLEAN_ALERT_CANCEL))
        //        {
        //            NWDDataManager.SharedInstance().CleanAllTablesLocalAccount();
        //            NWDDataManager.SharedInstance().CleanAllTablesLocalEditor();
        //        }
        //    }
        //    if (GUILayout.Button("Purge all local tables", NWDGUI.KTableSearchButton))
        //    {
        //        if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
        //                NWDConstants.K_PURGE_ALERT_MESSAGE,
        //                NWDConstants.K_PURGE_ALERT_OK,
        //                NWDConstants.K_PURGE_ALERT_CANCEL))
        //        {
        //            NWDDataManager.SharedInstance().PurgeAllTablesLocalAccount();
        //            NWDDataManager.SharedInstance().PurgeAllTablesLocalEditor();
        //        }
        //    }
        //    GUILayout.Space(NWDGUI.kFieldMarge);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void HackTools()
        {
            //NWEBenchmark.Start();
            NWDGUILayout.Section("Test anti-hack");
            if (GUILayout.Button("Use false token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDToolbox.RandomStringUnix(16);
            }
            if (GUILayout.Button("ReUse Last token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().PreviewRequesToken;
            }
            if (GUILayout.Button("ReUse Last Last token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().LastPreviewRequesToken;
            }
            NWDGUI.EndRedArea();
            GUILayout.Space(NWDGUI.kFieldMarge);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchroAllClasses(NWDAppEnvironment sEnvironment,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            Debug.Log("OperationSynchroAllClasses () with Operation " + sOperation.ToString());
            //NWEBenchmark.Start(sOperation.ToString());
            OperationSynchro(sEnvironment, NWDDataManager.SharedInstance().ClassSynchronizeList, sForceSync, sPriority, sOperation);
            //NWEBenchmark.Finish(sOperation.ToString());       
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchro(NWDAppEnvironment sEnvironment,
                                           List<Type> sTypeList = null,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            //NWEBenchmark.Start();
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
                NWDOperationWebSynchronisation.AddOperation("App Environnement Sync " + sOperation.ToString(), SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, null, sForceSync, sPriority, sOperation);
            }
            //NWEBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void OperationPullReferences(NWDAppEnvironment sEnvironment,
                                           Dictionary<Type, List<string>> sTypeAndReferences,
                                           bool sPriority = false)
        {
            //NWEBenchmark.Start();
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
                NWDOperationWebSynchronisation.AddOperation("App Environnement pull reference ", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, null, sTypeAndReferences, false, sPriority, NWDOperationSpecial.PullReference);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationManagement(NWDAppEnvironment sEnvironment, bool sForceSync = true)
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchroBlank(NWDAppEnvironment sEnvironment)
        {
            StartProcess(sEnvironment);
            NWDOperationWebBlank.AddOperation("blank", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, true, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Reset(NWDAppEnvironment sEnvironment)
        {
            //NWEBenchmark.Start();
            StartProcess(sEnvironment);
            sEnvironment.ResetPreferences();
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                DevIcon = NWDGUI.kImageWaiting;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreprodIcon = NWDGUI.kImageWaiting;
                PreprodSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = NWDGUI.kImageWaiting;
                ProdSessionExpired = false;
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush(NWDAppEnvironment sEnvironment)
        {
            //NWEBenchmark.Start();
            StartProcess(sEnvironment);
            NWDDataManager.SharedInstance().WebOperationQueue.Flush(sEnvironment.Environment);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartProcess(NWDAppEnvironment sEnvironment)
        {
            //NWEBenchmark.Start();
            DevIcon = NWDGUI.kImageWaiting;
            PreprodIcon = NWDGUI.kImageWaiting;
            ProdIcon = NWDGUI.kImageWaiting;

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                DevIcon = NWDGUI.kImageWaiting;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreprodIcon = NWDGUI.kImageWaiting;
                PreprodSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = NWDGUI.kImageWaiting;
                ProdSessionExpired = false;
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
