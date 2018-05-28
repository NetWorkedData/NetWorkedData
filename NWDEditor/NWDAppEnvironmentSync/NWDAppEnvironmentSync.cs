//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BasicToolBox;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDA pp environment sync.
    /// This class is used to show synchronize withn server.
    /// </summary>
    public class NWDAppEnvironmentSync : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The send octects.
        /// </summary>
        //      public double SendOctects = 0;
        //      /// <summary>
        //      /// The receipt octects.
        //      /// </summary>
        //public double ReceiptOctects = 0;
        ///// <summary>
        ///// The class pull counter.
        ///// </summary>
        //public int ClassPullCounter = 0;
        ///// <summary>
        ///// The class push counter.
        ///// </summary>
        //public int ClassPushCounter = 0;
        ///// <summary>
        ///// The row pull counter.
        ///// </summary>
        //public int RowPullCounter = 0;
        ///// <summary>
        ///// The row push counter.
        ///// </summary>
        //public int RowPushCounter = 0;
        /// <summary>
        /// The start time.
        ///// </summary>
        //DateTime StartTime;
        ///// <summary>
        ///// The middle time.
        ///// </summary>
        //DateTime MiddleTime;
        ///// <summary>
        ///// The middle time.
        ///// </summary>
        //DateTime MiddleTimeB;
        ///// <summary>
        ///// The end time.
        ///// </summary>
        //DateTime EndTime;
        /// <summary>
        /// The dev icon.
        /// </summary>
        Texture2D DevIcon;
        /// <summary>
        /// The pre prod icon.
        /// </summary>
        Texture2D PreProdIcon;
        /// <summary>
        /// The prod icon.
        /// </summary>
        Texture2D ProdIcon;
        /// <summary>
        /// The dev session expired.
        /// </summary>
        bool DevSessionExpired = false;
        /// <summary>
        /// The pre prod session expired.
        /// </summary>
        bool PreProdSessionExpired = false;
        /// <summary>
        /// The prod session expired.
        /// </summary>
        bool ProdSessionExpired = false;

        NWDOperationResult LastInfos = new NWDOperationResult();
        //-------------------------------------------------------------------------------------------------------------
        private BTBOperationBlock SuccessBlock = null;
        private BTBOperationBlock FailBlock = null;
        private BTBOperationBlock CancelBlock = null;
        private BTBOperationBlock ProgressBlock = null;
        //-------------------------------------------------------------------------------------------------------------
        // Icons for Sync
        private Texture2D kImageRed;
        private Texture2D kImageGreen;
        //private Texture2D kImageOrange;
        private Texture2D kImageForbidden;
        private Texture2D kImageEmpty;
        private Texture2D kImageWaiting;

        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstance()
        {
            NWDEditorMenu.EnvironementSyncShow();
            return NWDEditorMenu.kNWDAppEnvironmentSync;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            //    Debug.Log ("NWDAppEnvironmentSync Start");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Awake()
        {
            //    Debug.Log ("NWDAppEnvironmentSync Awake");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //    Debug.Log ("NWDAppEnvironmentSync OnEnable");

            kImageRed = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDRed.psd"));
            kImageGreen = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDGreen.psd"));
            //kImageOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDOrange.psd"));
            kImageForbidden = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDForbidden.psd"));
            kImageEmpty = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDEmpty.psd"));
            kImageWaiting = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDWaiting.psd"));

            DevIcon = kImageEmpty;
            PreProdIcon = kImageEmpty;
            ProdIcon = kImageEmpty;
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
                    DevIcon = kImageGreen;
                    DevSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreProdIcon = kImageGreen;
                    PreProdSessionExpired = false;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = kImageGreen;
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
                    DevIcon = kImageRed;
                    if (tErrorCode.Contains("RQT"))
                    {
                        DevSessionExpired = true;
                    }
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreProdIcon = kImageRed;
                    if (tErrorCode.Contains("RQT"))
                    {
                        PreProdSessionExpired = true;
                    }
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = kImageRed;
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
                    DevIcon = kImageForbidden;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreProdIcon = kImageForbidden;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = kImageForbidden;
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
                    DevIcon = kImageWaiting;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment)
                {
                    PreProdIcon = kImageWaiting;
                }
                if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment)
                {
                    ProdIcon = kImageWaiting;
                }
                Repaint();
            };
        }
        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event.
        /// </summary>
        public void OnGUI()
        {
            // use these bools to fix the bug of error on redraw
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment; // default value replace after in script
            bool tUpdateTable = false;
            bool tSync = false;
            bool tSyncForce = false;
            bool tPull = false;
            bool tPullForce = false;
            bool tSyncClean = false;
            bool tPublish = false;
            //bool tResethToken = false;
            //bool tFlushWebQueue = false;


            this.minSize = new Vector2(300, 500);
            this.maxSize = new Vector2(300, 4096);
            // set title of window
            titleContent = new GUIContent(NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE);
            // show helpbox
            EditorGUILayout.HelpBox(NWDConstants.K_APP_SYNC_ENVIRONMENT, MessageType.None);



            var tStyleCenter = new GUIStyle(EditorStyles.label);
            tStyleCenter.alignment = TextAnchor.MiddleCenter;


            var tStyleBoldCenter = new GUIStyle(EditorStyles.boldLabel);
            tStyleBoldCenter.alignment = TextAnchor.MiddleCenter;


            GUILayout.BeginHorizontal();
            GUILayout.Label("Dev Database", tStyleBoldCenter);
            GUILayout.Label("Preprod Database", tStyleBoldCenter);
            GUILayout.Label("Prod Database", tStyleBoldCenter);
            GUILayout.EndHorizontal();
            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Database", tStyleBoldCenter);
            //GUILayout.Label("Database", tStyleBoldCenter);
            //GUILayout.Label("Database", tStyleBoldCenter);
            //GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Upgrade", EditorStyles.miniButton))
            {
                tUpdateTable = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //CreateTable (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Upgrade", EditorStyles.miniButton))
            {
                tUpdateTable = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //CreateTable (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Upgrade", EditorStyles.miniButton))
            {
                tUpdateTable = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //CreateTable (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Prepare to Publish", EditorStyles.miniButton))
            {
                tPublish = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Prepare to Publish", EditorStyles.miniButton))
            {
                tPublish = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Prepare to Publish", EditorStyles.miniButton))
            {
                tPublish = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Sync all", EditorStyles.miniButton))
            {
                tSync = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Sync all", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Force Sync all ", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Force Sync all ", EditorStyles.miniButton))
            {
                tSyncForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();







            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Pull all", EditorStyles.miniButton))
            {
                tPull = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronization (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Force Pull all", EditorStyles.miniButton))
            {
                tPullForce = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronizationForce (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();



            Color tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tSyncClean = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                //AllSynchronizationClean (NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tSyncClean = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                //AllSynchronizationClean (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            if (GUILayout.Button("Clean all", EditorStyles.miniButton))
            {
                tSyncClean = true;
                tEnvironment = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                //AllSynchronizationClean (NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = tOldColor;
            GUILayout.BeginHorizontal();
            GUILayout.Label(DevIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.Label(PreProdIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.Label(ProdIcon, tStyleCenter, GUILayout.Height(20));
            GUILayout.EndHorizontal();
            double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
            double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
            double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
            double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            if (LastInfos.RowPushCounter == 0)
            {
                EditorGUILayout.LabelField("Rows pused", LastInfos.RowPushCounter.ToString() + " no row (no class)");
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
                 EditorGUILayout.LabelField("Rows pulled", LastInfos.RowUpdatedCounter.ToString() + " rows");
            }
            EditorGUILayout.LabelField("DataBase compute", tComputeNetMilliseconds.ToString("#0.000") + " s");
            EditorGUILayout.LabelField("Sync duration", tDurationNetMilliseconds.ToString("#0.000") + " s", EditorStyles.boldLabel);


            if (DevSessionExpired == true || PreProdSessionExpired == true || ProdSessionExpired == true)
            {
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!DevSessionExpired);
                if (GUILayout.Button("Reset token", EditorStyles.miniButton))
                {
                    Reset(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!PreProdSessionExpired);
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

            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
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
            GUI.backgroundColor = tOldColor;



            // Show version selected
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            NWDAccount tAccount = NWDAccount.ActualAccount();
            if (tAccount != null)
            {
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
            }



            if (tUpdateTable == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                CreateTable(tEnvironment);
            }
            if (tSync == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllSynchronization(tEnvironment);
            }
            if (tSyncForce == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllSynchronizationForce(tEnvironment);
            }
            if (tPull == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllSynchronizationPull(tEnvironment);
            }
            if (tPullForce == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllSynchronizationPullForce(tEnvironment);
            }
            if (tSyncClean == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllSynchronizationClean(tEnvironment);
            }
            if (tPublish == true)
            {
                if (Application.isPlaying == true)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                AllPrepareToPublish(tEnvironment);
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateTable(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebManagement.AddOperation("Create table on server", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllSynchronization(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("All Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, false, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllSynchronizationForce(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("All Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, true, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllSynchronizationPull(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebPull.AddOperation("All Pull", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, false, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllSynchronizationPullForce(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebPull.AddOperation("All Pull Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, true, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllSynchronizationClean(NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("All Synchronization Clean", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, true, true, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AllPrepareToPublish(NWDAppEnvironment sEnvironment)
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
                if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
                    {
                        var tMethodInfo = tType.GetMethod("PrepareToProdPublish", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(null, null);
                        }
                    }
                }
                else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
                    {
                        var tMethodInfo = tType.GetMethod("PrepareToPreprodPublish", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(null, null);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Pull(List<Type> sTypeList, NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebPull.AddOperation("Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PullForce(List<Type> sTypeList, NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebPull.AddOperation("Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, true, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Synchronization(List<Type> sTypeList, NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationClean(List<Type> sTypeList, NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("Synchronization clean", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationForce(List<Type> sTypeList, NWDAppEnvironment sEnvironment)
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
                StartProcess(sEnvironment);
                NWDOperationWebSynchronisation.AddOperation("Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, true, true);
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
                DevIcon = kImageEmpty;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreProdIcon = kImageEmpty;
                PreProdSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = kImageEmpty;
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
            DevIcon = kImageEmpty;
            PreProdIcon = kImageEmpty;
            ProdIcon = kImageEmpty;

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                DevIcon = kImageEmpty;
                DevSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                PreProdIcon = kImageEmpty;
                PreProdSessionExpired = false;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                ProdIcon = kImageEmpty;
                ProdSessionExpired = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
