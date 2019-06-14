// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using BasicToolBox;
using BTBMiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationWebAction : int
    {
        Sync = 1,
        SignIn = 1,
        SignOut = 2,
        Rescue = 3,
#if UNITY_EDITOR
        Management = 9,
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [ExecuteInEditMode]
    public partial class NWDOperationWebUnity : BTBOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static int kTimeOutOfRequest = 300;
        public GameObject GameObjectToSpawn;
        public bool SecureData = false;
        public UnityWebRequest Request;
        public NWDAppEnvironment Environment;
        public NWDOperationResult ResultInfos = new NWDOperationResult();
        public NWDOperationWebAction ActionEnum = NWDOperationWebAction.SignIn;
        public string Sign;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUnity AddOperation(string sName, NWDAppEnvironment sEnvironment = null, bool sPriority = false)
        {
            Debug.Log("NWDOperationWebUnity AddOperation()");
            NWDOperationWebUnity rReturn = NWDOperationWebUnity.Create(sName, sEnvironment);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUnity Create(string sName, NWDAppEnvironment sEnvironment = null)
        {
            Debug.Log("NWDOperationWebUnity Create()");
            NWDOperationWebUnity rReturn = null;
            if (NWDDataManager.SharedInstance().DataLoaded() == true)
            {
                if (sName == null)
                {
                    sName = "UnNamed Web Operation";
                }
                if (sEnvironment == null)
                {
                    sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                }
                GameObject tGameObjectToSpawn = new GameObject(sName);
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUnity>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
            }
            else
            {
                Debug.LogWarning("SYNC NEED TO OPEN ALL ACCOUNT TABLES AND LOADED ALL DATAS!");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
            //Debug.Log("NWDOperationWebUnity Execute()");
            ResultInfos = new NWDOperationResult();
            StartCoroutine(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataUploadPrepare()
        {
            //Debug.Log("NWDOperationWebUnity DataUploadPrepare()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DataAddSecetDevicekey()
        {
            //Debug.Log("NWDOperationWebUnity DataAddSecetDevicekey()");
            // insert device key in data
            if (Data.ContainsKey(NWD.K_WEB_SIGN_Key))
            {
                Data[NWD.K_WEB_SIGN_Key] = Environment.SecretKeyDevice();
            }
            else
            {
                Data.Add(NWD.K_WEB_SIGN_Key, Environment.SecretKeyDevice());
            }
            // force temporary account to be secure to transit the secretkey of device!
            SecureData = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataDownloadedCompute(NWDOperationResult sData)
        {
            //Debug.Log("NWDOperationWebUnity DataDownloadedCompute()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ServerFile()
        {
            //Debug.Log("NWDOperationWebUnity ServerFile()");
            return NWD.K_INDEX_PHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ServerBase()
        {
            //Debug.Log("NWDOperationWebUnity ServerBase()");
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            return Environment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/" + Environment.Environment + "/" + ServerFile();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool CanRestart()
        {
            //Debug.Log("NWDOperationWebUnity CanRestart()");
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator ExecuteAsync()
        {
            //Debug.Log("NWDOperationWebUnity ExecuteAsync()");
            ResultInfos = new NWDOperationResult();
            // reinit benchmark stat values
            ResultInfos.PrepareDateTime = DateTime.Now;
            ResultInfos.WebDateTime = DateTime.Now;
            ResultInfos.UploadedDateTime = DateTime.Now;
            ResultInfos.DownloadedDateTime = DateTime.Now;
            ResultInfos.FinishDateTime = DateTime.Now;
            ResultInfos.OctetUpload = 0.0F;
            ResultInfos.OctetDownload = 0.0F;
            ResultInfos.ClassPullCounter = 0;
            ResultInfos.ClassPushCounter = 0;
            ResultInfos.RowPullCounter = 0;
            ResultInfos.RowPushCounter = 0;
            Statut = BTBOperationState.Start;
            bool tUserChange = false;
            ProgressInvoke(0.0f, ResultInfos);
            //Operation progress
            Statut = BTBOperationState.InProgress;
            float tStart = Time.time;
            // Send this operation in actual operation for this environment
            Parent.Controller[QueueName].ActualOperation = this;
            // Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
#if UNITY_EDITOR
            // Deselect all object
            Selection.activeObject = null;
#endif
            // I insert the device key if necessary 
            // can be override by the DataUploadPrepare if necessary
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetCorporateFirstData(Environment.PlayerAccountReference, null);
            if (tAccountInfos == null)
            {
                DataAddSecetDevicekey();
            }
            else
            {
                if (tAccountInfos.AccountType() == NWDAppEnvironmentPlayerStatut.Temporary)
                {
                    DataAddSecetDevicekey();
                }
            }
            // I prepare the data
            DataUploadPrepare();
            // I insert the data
            WWWForm tWWWForm = InsertDataInRequest(ResultInfos);
            ResultInfos.OctetUpload = tWWWForm.data.Length;
            using (Request = UnityWebRequest.Post(ServerBase(), tWWWForm))
            {
                Request.downloadHandler = new DownloadHandlerBuffer();
                Request.timeout = Environment.WebTimeOut;
#if UNITY_EDITOR
                Request.timeout = Environment.EditorWebTimeOut;
#endif
                // I prepare the header 
                // I put the header in my request
                InsertHeaderInRequest();
                // I send the data
                ResultInfos.WebDateTime = DateTime.Now;
                // Debug Show Header Uploaded
                if (Environment.LogMode == true)
                {
                    DebugShowHeaderUploaded(tWWWForm.data);
                }
                // Notification of an Upload start
                BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, this));
                Request.SendWebRequest();
                while (!Request.isDone)
                {
                    Statut = BTBOperationState.InProgress;
                    ProgressInvoke(Request.downloadProgress, ResultInfos);
                    if (Request.uploadProgress < 1.0f)
                    {
                        // Notification of an Upload in progress
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, this));
                        ResultInfos.UploadedDateTime = DateTime.Now;
                        ResultInfos.DownloadedDateTime = ResultInfos.UploadedDateTime;
                        ResultInfos.FinishDateTime = ResultInfos.UploadedDateTime;
                    }
                    if (Request.downloadProgress < 1.0f)
                    {
                        // Notification of an Download in progress
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, this));
                    }
                    yield return null;
                }
                if (Request.isNetworkError)
                {
#if UNITY_EDITOR
                    Debug.Log(Request.error + "\n" + Request.downloadHandler.text + "\n");
#endif
                    Statut = BTBOperationState.Error;
                    ResultInfos.SetError(NWDError.NWDError_WEB01);
                }
                else if (Request.isHttpError)
                {
#if UNITY_EDITOR
                    Debug.Log(Request.error + "\n" + Request.downloadHandler.text + "\n");
#endif
                    Statut = BTBOperationState.Error;
                    ResultInfos.SetError(NWDError.NWDError_WEB02);
                }
                else
                {
                    while (!Request.downloadHandler.isDone)
                    {
                        yield return null;
                    }
                    if (Request.isDone == true)
                    {
                        string tDataConverted = Request.downloadHandler.text;
                        ResultInfos.DownloadedDateTime = DateTime.Now;
                        ResultInfos.FinishDateTime = ResultInfos.DownloadedDateTime;
                        ResultInfos.OctetDownload = tDataConverted.Length;
                        // Notification of an Download is done
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, this));
                        // Debug Show Header Download
                        if (Environment.LogMode == true)
                        {
                            DebugShowHeaderDownloaded(tDataConverted);
                        }
                        // Debug Show Header Up vs Down
                        if (Environment.LogMode == true)
                        {
                            DebugShowHeaderTotal(tDataConverted);
                        }
                        // Check for error
                        if (tDataConverted.Equals(string.Empty))
                        {
                            ResultInfos.SetError(NWDError.NWDError_WEB04);
                            // Application is in running mode
                            if (Application.isPlaying == true)
                            {
                                NWDGameDataManager.UnitySingleton().ErrorManagement(ResultInfos.errorDesc);
                            }
                            // Request Failed, send Invoke
                            FailInvoke(Request.downloadProgress, ResultInfos);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Request.GetResponseHeader(NWD.K_OBSOLETE_HEADER_KEY)) == true)
                            {
                                if (string.IsNullOrEmpty(Request.GetResponseHeader(NWD.K_MAINTENANCE_HEADER_KEY)) == true)
                                {
                                    // Parse Json Data to Dictionary
                                    Dictionary<string, object> tData = null;
                                    if (Json.IsValidJson(tDataConverted))
                                    {
                                        tData = Json.Deserialize(tDataConverted) as Dictionary<string, object>;
                                    }
                                    // If no data is parse from the downloadHandler
                                    if (tData == null)
                                    {
                                        // Log DownloadHandler in console
                                        ResultInfos.SetError(NWDError.NWDError_WEB03);
                                        // Application is in running mode
                                        if (Application.isPlaying == true)
                                        {
                                            NWDGameDataManager.UnitySingleton().ErrorManagement(ResultInfos.errorDesc);
                                        }
                                        // Notification of a Download success
                                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_ERROR, ResultInfos));
                                        // Request Failed, send Invoke
                                        FailInvoke(Request.downloadProgress, ResultInfos);
                                    }
                                    else
                                    {
                                        if (SecureData)
                                        {
                                            if (tData.ContainsKey(BTBUnityWebService.SecureKey) && tData.ContainsKey(BTBUnityWebService.SecureDigestKey))
                                            {
                                                string tSCR = (string)tData[BTBUnityWebService.SecureKey];
                                                string tSCRDGT = (string)tData[BTBUnityWebService.SecureDigestKey];
                                                string tDigestValue = BTBSecurityTools.GenerateSha(Environment.SaltStart + tSCR + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
                                                if (tDigestValue != tSCRDGT)
                                                {
                                                    ResultInfos.SetError(NWDError.NWDError_RQT98);
                                                }
                                                else
                                                {
                                                    tData = BTBSecurityTools.RemoveAes(tSCR, Environment.DataSHAPassword, Environment.DataSHAVector, BTBSecurityAesTypeEnum.Aes128);
                                                    if (tData == null)
                                                    {
                                                        ResultInfos.SetError(NWDError.NWDError_RQT99);
                                                    }
                                                    else
                                                    {
                                                        if (Environment.LogMode == true)
                                                        {
                                                            DebugShowHeaderTotalDecoded(Json.Serialize(tData).Replace("\\\\r", "\r\n"));
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ResultInfos.SetError(NWDError.NWDError_RQT98);
                                            }
                                        }
                                        // Request in Progress, send Invoke
                                        ProgressInvoke(1.0f, ResultInfos);
                                        ResultInfos.SetData(tData);
                                        ResultInfos.OctetDownload = tDataConverted.Length;
                                        // memorize the token for next connection
                                        if (!ResultInfos.token.Equals(string.Empty))
                                        {
                                            if (Environment.RequesToken == ResultInfos.token)
                                            {
                                                // What the token is the same? It's not possible!
                                                ResultInfos.SetError(NWDError.NWDError_RQT95);
                                            }
                                            else
                                            {
                                                if (Environment.RequesToken == Request.GetResponseHeader(NWD.RequestTokenKey))
                                                {
                                                    // What the token is not beetween respond and header? It's not possible!
                                                    ResultInfos.SetError(NWDError.NWDError_RQT97);
                                                }
                                                else
                                                {
                                                    if (TestTemporalRequestHash(Request.GetResponseHeader(NWD.HashKey), Request.GetResponseHeader(NWD.RequestTokenKey)) == false)
                                                    {
                                                        // What the token is not valid!? It's not possible!
                                                        ResultInfos.SetError(NWDError.NWDError_RQT96);
                                                    }
                                                    else
                                                    {
#if UNITY_EDITOR
                                                        Environment.LastPreviewRequesToken = Environment.PreviewRequesToken;
                                                        Environment.PreviewRequesToken = Environment.RequesToken;
#endif
                                                        Environment.RequesToken = ResultInfos.token;
                                                    }
                                                }
                                            }
                                        }
                                        // Check if error
                                        if (ResultInfos.isError)
                                        {
                                            Statut = BTBOperationState.Failed;

                                            if (
                                                ResultInfos.errorCode == NWDError.NWDError_RQT90.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT91.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT92.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT93.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT94.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT95.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT96.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT97.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT98.Code ||
                                                ResultInfos.errorCode == NWDError.NWDError_RQT99.Code
                                                )
                                            {
                                                // Notification of a Session expired
                                                BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, ResultInfos));
                                                // Restore for anonymous account
                                                NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetSession();
                                            }
                                            else
                                            {
                                                if (ResultInfos.errorDesc != null)
                                                {
                                                    if (
                                                        ResultInfos.errorCode == NWDError.NWDError_ACC98.Code ||
                                                        ResultInfos.errorCode == NWDError.NWDError_ACC99.Code
                                                        )
                                                    {
                                                        // Notification of an Account Banned
                                                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_BANNED, ResultInfos));
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Statut = BTBOperationState.Success;
                                            if (ResultInfos.isNewUser)
                                            {
                                                tUserChange = true;
                                                CanRestart();
                                                if (ResultInfos.isUserTransfert)
                                                {
                                                    if (!ResultInfos.uuid.Equals(string.Empty))
                                                    {
                                                        // creer Device sign and send to serever with new package
                                                        NWDAccountSign tSign = NWDAccountSign.NewData();
                                                        tSign.Account.SetReference(ResultInfos.uuid);
                                                        tSign.RegisterDevice();
                                                        NWDDataManager.SharedInstance().ChangeAllDatasForUserToAnotherUser(Environment, ResultInfos.uuid /*, ResultInfos.signkey*/);
                                                    }
                                                }
                                            }
                                            if (!ResultInfos.uuid.Equals(string.Empty))
                                            {
                                                Environment.PlayerAccountReference = ResultInfos.uuid;
                                            }
                                            DataDownloadedCompute(ResultInfos);
                                            // Notification of a Download success
                                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, ResultInfos));
                                            // Request Success, send Invoke
                                            SuccessInvoke(Request.downloadProgress, ResultInfos);
                                        }
                                    }
                                }
                                else
                                {
                                    ResultInfos.SetError(NWDError.NWDError_MAINTENANCE);
                                }
                            }
                            else
                            {
                                ResultInfos.SetError(NWDError.NWDError_OBSOLETE);
                            }
                        }
                    }
                    // Save preference localy
                    Environment.SavePreferences();
                    // Notification of current Account have change
                    if (tUserChange == true)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
                    }
                }
                //if (ResultInfos.errorDesc != null)
                if (ResultInfos.isError)
                {
                    // Notification of a Download success
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_ERROR, ResultInfos));
                    ResultInfos.errorDesc.ShowAlert(ResultInfos.errorInfos);
                    if (Application.isPlaying == true)
                    {
                        NWDGameDataManager.UnitySingleton().ErrorManagement(ResultInfos.errorDesc);
                    }
                    // Notification of a Download success
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, ResultInfos));
                    FailInvoke(Request.downloadProgress, ResultInfos);
                }
                Finish();
            }
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            //Debug.Log("NWDOperationWebUnity Cancel()");
            ResultInfos.FinishDateTime = DateTime.Now;
            Statut = BTBOperationState.Cancel;
            if (Request != null)
            {
                Request.Abort();
            }
            NWDOperationResult tInfosCancel = new NWDOperationResult();
            CancelInvoke(Request.downloadProgress, tInfosCancel);
            IsFinish = true;
            Parent.NextOperation(QueueName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Finish()
        {
            //Debug.Log("NWDOperationWebUnity Finish()");
            ResultInfos.FinishDateTime = DateTime.Now;
            if (Statut == BTBOperationState.ReStart)
            {
                // I MUST RESTART THE REQUEST BECAUSE BEFORE I WAS TEMPORARY ACCOUNT
                Parent.ReplayOperation(QueueName);
            }
            else
            {
                Statut = BTBOperationState.Finish;
                IsFinish = true;
                Parent.NextOperation(QueueName);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DestroyThisOperation()
        {
            //Debug.Log("NWDOperationWebUnity DestroyThisOperation()");
            Statut = BTBOperationState.Destroy;
#if UNITY_EDITOR
            DestroyImmediate(GameObjectToSpawn);
#else
            Destroy (GameObjectToSpawn);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public string OS;
        public string Lang;
        public string Version;
        public string UUID;
        public string RequestToken;
        Dictionary<string, object> HeaderParams = new Dictionary<string, object>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertHeaderInRequest()
        {
            //Debug.Log("NWDOperationWebUnity InsertHeaderInRequest()");
            HeaderParams.Clear();
            UUID = Environment.PlayerAccountReference;
            if (SecureData)
            {
                HeaderParams.Add(BTBUnityWebService.SecureKey, BTBUnityWebService.SecureDigestKey);
            }
            else
            {
                HeaderParams.Add(BTBUnityWebService.UnSecureKey, BTBUnityWebService.UnSecureDigestKey);
            }
            RequestToken = Environment.RequesToken;
#if UNITY_EDITOR
            Version = PlayerSettings.bundleVersion;
            OS = "editor";
#else
            Version = Application.version;
#if UNITY_ANDROID
            OS = "android";
#elif UNITY_IOS
            OS = "ios";
#elif UNITY_STANDALONE_OSX
            OS = "osx";
#elif UNITY_WP8
            OS = "wp8";
#elif UNITY_WINRT
            OS = "win";
#else
            OS = "unity";
#endif
#endif
            Lang = NWDDataManager.SharedInstance().PlayerLanguage;
            // insert value in header dico
            HeaderParams.Add(NWD.UUIDKey, UUID);
            HeaderParams.Add(NWD.RequestTokenKey, RequestToken);
            HeaderParams.Add(NWD.K_WEB_HEADER_OS_KEY, OS);
            HeaderParams.Add(NWD.K_WEB_HEADER_VERSION_KEY, Version);
            HeaderParams.Add(NWD.K_WEB_HEADER_LANG_KEY, Lang);
            // create hash security
            string tHashValue = string.Format("{0}{1}{2}{3}{4}{5}", OS, Version, Lang, NWDToolbox.GenerateSALT(Environment.SaltFrequency), UUID, RequestToken);
            HeaderParams.Add(NWD.HashKey, BTBSecurityTools.GenerateSha(tHashValue, BTBSecurityShaTypeEnum.Sha1));
#if UNITY_EDITOR
            if (Application.isPlaying == false && Application.isEditor == true)
            {
                HeaderParams.Add(NWD.AdminHashKey, NWDToolbox.GenerateAdminHash(Environment.AdminKey, Environment.SaltFrequency));

            }
#else
            if (NWDAppConfiguration.SharedInstance().AdminInPLayer())
            {
                if (string.IsNullOrEmpty(Environment.AdminKey) == false)
                {
                    HeaderParams.Add(AdminHashKey, NWDToolbox.GenerateAdminHash(Environment.AdminKey, Environment.SaltFrequency));
                }
            }
#endif
            // insert dico of header in request header
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                Request.SetRequestHeader(tEntry.Key, tEntry.Value.ToString());
            }
        }
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        //-------------------------------------------------------------------------------------------------------------
        public WWWForm InsertDataInRequest(NWDOperationResult sInfos)
        {
            //Debug.Log("NWDOperationWebUnity InsertDataInRequest()");
            WWWForm tBodyData = new WWWForm();
            string tParamKey = BTBUnityWebService.UnSecureKey;
            string tDigestKey = BTBUnityWebService.UnSecureDigestKey;
            string tParamValue = string.Empty;
            string tDigestValue = string.Empty;
            if (SecureData)
            {
                tParamKey = BTBUnityWebService.SecureKey;
                tDigestKey = BTBUnityWebService.SecureDigestKey;
                tParamValue = BTBSecurityTools.AddAes(Data, Environment.DataSHAPassword, Environment.DataSHAVector, BTBSecurityAesTypeEnum.Aes128);
                tDigestValue = BTBSecurityTools.GenerateSha(Environment.SaltStart + tParamValue + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
            }
            else
            {
                tParamValue = BTBSecurityTools.Base64Encode(Json.Serialize(Data));
                tDigestValue = BTBSecurityTools.GenerateSha(Environment.SaltStart + tParamValue + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
            }
            tBodyData.AddField(tParamKey, tParamValue);
            tBodyData.AddField(tDigestKey, tDigestValue);

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                sInfos.OctetUpload = tParamValue.Length + tDigestValue.Length;
            }
#endif

            return tBodyData;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool TestTemporalRequestHash(string sHash, string sToken)
        {
            //Debug.Log("NWDOperationWebUnity TestTemporalRequestHash()");
            bool rReturn = false;
            string tSaltA = NWDToolbox.GenerateSALTOutlined(Environment.SaltFrequency, 1);
            string tSaltB = NWDToolbox.GenerateSALTOutlined(Environment.SaltFrequency, 0);
            string tSaltC = NWDToolbox.GenerateSALTOutlined(Environment.SaltFrequency, -1);
            string sVector = Environment.DataSHAVector;
            if (BTBSecurityTools.GenerateSha(tSaltA + sVector + sToken) == sHash ||
                BTBSecurityTools.GenerateSha(tSaltB + sVector + sToken) == sHash ||
                BTBSecurityTools.GenerateSha(tSaltC + sVector + sToken) == sHash)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderUploaded(byte[] sData)
        {
#if UNITY_EDITOR
            string tDebugRequestHeader = string.Empty;
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            NWDDebug.Log("NWDOperationWebUnity UPLOADED \n" +
                         "-------------------\n" +
                         "<b>Request URl :</b> " + Request.url + "\n" +
                         "-------------------\n" +
                         "<b>Headers :</b> \n" +
                         "-------------------\n" +
                         tDebugRequestHeader + "\n" +
                         "-------------------\n" +
                         "<b>Datas :</b> \n" +
                         "-------------------\n" +
                         Json.Serialize(Data).Replace("/r", string.Empty).Replace("/n", string.Empty) + "\n" +
                         "-------------------\n" +
                         Encoding.UTF8.GetString(sData) + "\n" +
                         "-------------------\n" +
                         ""
            );
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderDownloaded(string sData)
        {
#if UNITY_EDITOR
            string tDebugResponseHeader = string.Empty;
            foreach (KeyValuePair<string, string> tEntry in Request.GetResponseHeaders())
            {
                tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            NWDDebug.Log("NWDOperationWebUnity DOWNLOADED \n" +
                         "-------------------\n" +
                         "<b>Request URl :</b> " + Request.url + "\n" +
                         "-------------------\n" +
                         "<b>Headers :</b> \n" +
                         "-------------------\n" +
                         tDebugResponseHeader + "\n" +
                         "-------------------\n" +
                         "<b>Datas : (" + ResultInfos.OctetDownload + ")</b> \n" +
                         "-------------------\n" +
                         sData.Replace("\\\\r", "\r\n") + "\n" +
                         "-------------------\n"
            );
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderTotal(string sData)
        {
#if UNITY_EDITOR
            string tDebugRequestHeader = string.Empty;
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            string tDebugResponseHeader = string.Empty;
            foreach (KeyValuePair<string, string> tEntry in Request.GetResponseHeaders())
            {
                tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            NWDDebug.Log("NWDOperationWebUnity UPLOAD  VS DOWNLOADED \n" +
                         "-------------------\n" +
                         "<b>Request URl :</b> " + Request.url + "\n" +
                         "-------------------\n" +
                         "<b>Headers UPLOAD :</b> \n" +
                         "-------------------\n" +
                         tDebugRequestHeader + "\n" +
                         "-------------------\n" +
                         "<b>Headers DOWNLOAD :</b> \n" +
                         "-------------------\n" +
                         tDebugResponseHeader + "\n" +
                         "-------------------\n" +
                         "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                         "-------------------\n" +
                         sData.Replace("\\\\r", "\r\n") + "\n" +
                         "-------------------\n"
            );
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderTotalDecoded(string sData)
        {
#if UNITY_EDITOR
            string tDebugRequestHeader = string.Empty;
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            string tDebugResponseHeader = string.Empty;
            foreach (KeyValuePair<string, string> tEntry in Request.GetResponseHeaders())
            {
                tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
            }
            NWDDebug.Log("NWDOperationWebUnity UPLOAD  VS DOWNLOADED DECODED \n" +
                         "-------------------\n" +
                         "<b>Request URl : </b> " + Request.url + "\n" +
                         "-------------------\n" +
                         "<b>Headers UPLOAD : </b> \n" +
                         "-------------------\n" +
                         tDebugRequestHeader + "\n" +
                         "-------------------\n" +
                         "<b>Datas UPLOAD : </b> \n" +
                         "-------------------\n" +
                         Json.Serialize(Data).Replace("/r", string.Empty).Replace("/n", string.Empty) + "\n" +
                         "-------------------\n\n\n" +
                         "-------------------\n" +
                         "<b>Headers DOWNLOAD : </b> \n" +
                         "-------------------\n" +
                         tDebugResponseHeader + "\n" +
                         "-------------------\n" +
                         "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                         "-------------------\n" +
                         sData.Replace("\\\\r", "\r\n") + "\n" +
                         "-------------------\n"
            );
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================