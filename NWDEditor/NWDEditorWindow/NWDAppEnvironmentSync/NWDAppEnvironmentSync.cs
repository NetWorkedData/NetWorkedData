//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentSyncContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
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
        bool Init = false;
        //-------------------------------------------------------------------------------------------------------------
        NWDOperationResult LastInfos; // = new NWDOperationResult();
        //-------------------------------------------------------------------------------------------------------------
        private NWEOperationBlock SuccessBlock = null;
        private NWEOperationBlock FailBlock = null;
        private NWEOperationBlock CancelBlock = null;
        private NWEOperationBlock ProgressBlock = null;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDAppEnvironmentSyncContent _kSharedInstanceContent;
        List<NWDEditorWindow> EditorWindowList = new List<NWDEditorWindow>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppEnvironmentSyncContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDAppEnvironmentSyncContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }

        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {
            if (EditorWindowList.Contains(sEditorWindow))
            {
                EditorWindowList.Remove(sEditorWindow);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            if (EditorWindowList.Contains(sEditorWindow) == false)
            {
                EditorWindowList.Add(sEditorWindow);
            }
            if (Init == false)
            {
                Init = true;

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
                    foreach (NWDEditorWindow tEditorWindow in EditorWindowList)
                    {
                        tEditorWindow.Repaint();
                    }
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
                        foreach (NWDEditorWindow tEditorWindow in EditorWindowList)
                        {
                            tEditorWindow.Repaint();
                        }
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
                                    Debug.LogWarning("" + tTitle + " " + tDescription +  " infos " + LastInfos.errorInfos);
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
                    foreach (NWDEditorWindow tEditorWindow in EditorWindowList)
                    {
                        tEditorWindow.Repaint();
                    }
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
                    foreach (NWDEditorWindow tEditorWindow in EditorWindowList)
                    {
                        tEditorWindow.Repaint();
                    }
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
                    foreach (NWDEditorWindow tEditorWindow in EditorWindowList)
                    {
                        tEditorWindow.Repaint();
                    }
                };
                SyncInfosTab = NWDProjectPrefs.GetInt("SyncInfosTab");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            NWDGUILayout.Title("Webservice");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            float tRectWidth = sRect.width;
            WebServicesSync(tRectWidth);
            WebServicesLastSync();
            WebServicesTools(tRectWidth);
            HackTools();
            WritingDatabaseState();
            //DatasLocal();
            NWDGUILayout.BigSpace();
            EditorGUILayout.EndScrollView();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WritingDatabaseState()
        {
            //NWDBenchmark.Start();
            NWDGUILayout.Section("Database writing");
            // use these bools to fix the bug of error on redraw

            int tObjectInQueue = NWDDataManager.SharedInstance().DataQueueCounter();
            if (tObjectInQueue == 0)
            {
                GUILayout.Label("No data is waiting to write");
            }
            else if (tObjectInQueue == 1)
            {
                GUILayout.Label(tObjectInQueue + " data is waiting to write");
            }
            else if (tObjectInQueue > 1)
            {
                GUILayout.Label(tObjectInQueue + " datas are waiting to write");
            }
            //NWDBenchmark.Finsih();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesSync(float sRectWidth)
        {
            //NWDBenchmark.Start();




            float tWidthThird = Mathf.Floor((sRectWidth - NWDGUI.KTAB_BAR_HEIGHT) / 3.0F);
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





            NWDGUILayout.Section("Webservice result");

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().DevServerSyncActive())
            {
                GUILayout.Label(DevIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Preprod", NWDGUI.KTableSearchTitle);

            if (NWDAppConfiguration.SharedInstance().PreprodServerSyncActive())
            {
                GUILayout.Label(PreprodIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Prod", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().ProdServerSyncActive())
            {
                GUILayout.Label(ProdIcon, NWDGUI.KTableSearchIcon, GUILayout.Height(20));
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



            NWDGUILayout.Section("Webservice sync");
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().DevServerSyncActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tDevEnvironment;
                }

                if (GUILayout.Button("Force sync all", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tDevEnvironment;
                }

                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tDevEnvironment;
                }
                if (GUILayout.Button("Force pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tDevEnvironment;
                }
                GUILayout.Space(NWDGUI.kFieldMarge);
                //NWDGUILayout.Separator();
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
            GUILayout.Label("Preprod", NWDGUI.KTableSearchTitle);

            if (NWDAppConfiguration.SharedInstance().PreprodServerSyncActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Force sync all ", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tPreprodEnvironment;
                }
                if (GUILayout.Button("Force pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tPreprodEnvironment;
                }
                GUILayout.Space(NWDGUI.kFieldMarge);
                //NWDGUILayout.Separator();
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
            GUILayout.Label("Prod", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().ProdServerSyncActive())
            {
                if (GUILayout.Button("Sync all", NWDGUI.KTableSearchButton))
                {
                    tSync = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Force sync all ", NWDGUI.KTableSearchButton))
                {
                    tSyncForce = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Pull all", NWDGUI.KTableSearchButton))
                {
                    tPull = true;
                    tEnvironment = tProdEnvironment;
                }
                if (GUILayout.Button("Force pull all", NWDGUI.KTableSearchButton))
                {
                    tPullForce = true;
                    tEnvironment = tProdEnvironment;
                }
                GUILayout.Space(NWDGUI.kFieldMarge);
                //NWDGUILayout.Separator();
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






            NWDGUILayout.Section("Webservice actions");

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().DevServerSyncActive())
            {
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
                if (GUILayout.Button("Blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tDevEnvironment;
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
            GUILayout.Label("Preprod", NWDGUI.KTableSearchTitle);

            if (NWDAppConfiguration.SharedInstance().PreprodServerSyncActive())
            {
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
                if (GUILayout.Button("Blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tPreprodEnvironment;
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
            GUILayout.Label("Prod", NWDGUI.KTableSearchTitle);
            if (NWDAppConfiguration.SharedInstance().ProdServerSyncActive())
            {
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
                if (GUILayout.Button("Blank", NWDGUI.KTableSearchButton))
                {
                    tOperationBlank = true;
                    tEnvironment = tProdEnvironment;
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













            // run button selected (if GUI prevent)
            if (tSync == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, false);
                NWDOperationWebhook.NewMessage(tEnvironment, "Sync all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tSyncForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true);
                NWDOperationWebhook.NewMessage(tEnvironment, "Sync all force", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tPull == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, false, NWDOperationSpecial.Pull);
                NWDOperationWebhook.NewMessage(tEnvironment, "Pull all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tPullForce == true)
            {
                OperationSynchroAllClasses(tEnvironment, true, true, NWDOperationSpecial.Pull);
                NWDOperationWebhook.NewMessage(tEnvironment, "Pull all force", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationClean == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Clean);
                NWDOperationWebhook.NewMessage(tEnvironment, "Clean all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationUpgrade == true)
            {
                OperationManagement(tEnvironment, true);
                NWDOperationWebhook.NewMessage(tEnvironment, "Upgrade all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationOptimize == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Optimize);
                NWDOperationWebhook.NewMessage(tEnvironment, "Optimize all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationIndexes == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Indexes);
                NWDOperationWebhook.NewMessage(tEnvironment, "Indexes all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationBlank == true)
            {
                OperationSynchroBlank(tEnvironment);
                NWDOperationWebhook.NewMessage(tEnvironment, "blank test", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            if (tOperationSpecial == true)
            {
                OperationSynchroAllClasses(tEnvironment, false, true, NWDOperationSpecial.Special);
                NWDOperationWebhook.NewMessage(tEnvironment, "Special all", WebHookType.Sync);
                GUIUtility.ExitGUI();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesLastSync()
        {
            //NWDBenchmark.Start();
            if (LastInfos == null)
            {
                LastInfos = new NWDOperationResult();
            }
            //double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            //double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            //double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
            //double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
            //double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;


            //NWDGUILayout.Section("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " last request");
            NWDGUILayout.Section("Last request");
            EditorGUILayout.LabelField("URL", LastInfos.URL);
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
                EditorGUILayout.LabelField("Data prepare", (LastInfos.Benchmark.GetPrepareTime() / 1000.0F).ToString("#0.000") + " s");
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
                EditorGUILayout.LabelField("Server perform request", LastInfos.performRequest.ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Server perform", LastInfos.perform.ToString("#0.000") + " s");
                //EditorGUILayout.LabelField("Network Download", (LastInfos.Benchmark.GetDownloadTime() / 1000.0F).ToString("#0.000") + " s");
                //EditorGUILayout.LabelField("Network Download", (tDowloadNetMilliseconds - LastInfos.performRequest).ToString("#0.000") + " s");
                double tNetWork = LastInfos.Benchmark.GetUploadTime() + LastInfos.Benchmark.GetPerformTime() + LastInfos.Benchmark.GetDownloadTime() - LastInfos.perform;
                if (tNetWork < 0)
                {
                    tNetWork = 0; // network in progress... dont calculate!
                }
                EditorGUILayout.LabelField("Network perform", (tNetWork / 1000.0F).ToString("#0.000") + " s");
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
                EditorGUILayout.LabelField("Database compute", (LastInfos.Benchmark.GetComputeTime() / 1000.0F).ToString("#0.000") + " s");
                EditorGUILayout.LabelField("Sync duration", (LastInfos.Benchmark.GetTotalTime() / 1000.0F).ToString("#0.000") + " s", EditorStyles.boldLabel);
            }
            GUILayout.Space(NWDGUI.kFieldMarge);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebServicesTools(float sRectWidth)
        {
            //NWDBenchmark.Start();
            //NWDGUILayout.Section("Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + " tools");
            //NWDWebServiceManagerContent.DrawForWebservice(NWDAppConfiguration.SharedInstance().WebBuild);
            //GUILayout.Space(NWDGUI.kFieldMarge);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void DatasLocal()
        //{
        //    //NWDBenchmark.Start();
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
        //    //NWDBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void HackTools()
        {
            //NWDBenchmark.Start();
            NWDGUILayout.Section("Test anti-hack");
            if (GUILayout.Button("Use false token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDToolbox.RandomStringUnix(16);
            }
            if (GUILayout.Button("Reuse last token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().PreviewRequesToken;
            }
            if (GUILayout.Button("Reuse before last token", NWDGUI.KTableSearchButton))
            {
                NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDAppEnvironment.SelectedEnvironment().LastPreviewRequesToken;
            }
            NWDGUI.EndRedArea();
            GUILayout.Space(NWDGUI.kFieldMarge);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchroAllClasses(NWDAppEnvironment sEnvironment,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            NWDDebug.Log("OperationSynchroAllClasses () with Operation " + sOperation.ToString());
            //NWDBenchmark.Start(sOperation.ToString());
            OperationSynchro(sEnvironment, NWDDataManager.SharedInstance().ClassSynchronizeList, null, sForceSync, sPriority, sOperation);
            //NWDBenchmark.Finish(sOperation.ToString());       
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationSynchro(NWDAppEnvironment sEnvironment,
                                           List<Type> sTypeList = null,
                                           Dictionary<Type, List<string>> sTypeAndReferences = null,
                                           bool sForceSync = false,
                                           bool sPriority = false,
                                           NWDOperationSpecial sOperation = NWDOperationSpecial.None)
        {
            //NWDBenchmark.Start();
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
                NWDOperationWebSynchronisation.AddOperation("App Environnement Sync " + sOperation.ToString(), SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, sTypeAndReferences, sForceSync, sPriority, sOperation);
            }
            //NWDBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void OperationPullReferences(NWDAppEnvironment sEnvironment,
                                           Dictionary<Type, List<string>> sTypeAndReferences,
                                           bool sPriority = false)
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OperationManagement(NWDAppEnvironment sEnvironment, bool sForceSync = true)
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
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
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            StartProcess(sEnvironment);
            NWDDataManager.SharedInstance().WebOperationQueue.Flush(sEnvironment.Environment);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartProcess(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentSync : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "app-environment-sync/";
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDAppEnvironmentSync kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentSync), ShowAsWindow()) as NWDAppEnvironmentSync;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
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
            //NWDBenchmark.Start();
            bool rReturn = false;
            if (kSharedInstance != null)
            {
                rReturn = true;
            }
            return rReturn;
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 350;
            NormalizeHeight = 1000;
            // set title
            TitleInit(NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE, typeof(NWDAppEnvironmentSync));
            NWDAppEnvironmentSyncContent.SharedInstance().OnEnable(this);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDisable()
        {
            NWDAppEnvironmentSyncContent.SharedInstance().OnDisable(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDAppEnvironmentSyncContent.SharedInstance().OnPreventGUI(position);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
