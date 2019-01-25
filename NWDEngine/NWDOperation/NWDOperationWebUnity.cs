//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

using BasicToolBox;
using BTBMiniJSON;

//=====================================================================================================================
namespace NetWorkedData
{
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
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUnity AddOperation(string sName, NWDAppEnvironment sEnvironment = null, bool sPriority = false)
        {
            NWDOperationWebUnity rReturn = Create(sName, sEnvironment);
            NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUnity Create(string sName, NWDAppEnvironment sEnvironment = null)
        {
            NWDOperationWebUnity rReturn = null;
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
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
            // Send Async Request
            ResultInfos = new NWDOperationResult();
            StartCoroutine(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataUploadPrepare()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataDownloadedCompute(NWDOperationResult sData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ServerFile()
        {
            return "index.php";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ServerBase()
        {
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            return Environment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/Environment/" + Environment.Environment + "/" + ServerFile();
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator StartAsync()
        {
            using (Request = new UnityWebRequest(ServerBase(), UnityWebRequest.kHttpVerbPOST))
            {
                InsertHeaderInRequest();
                Request.downloadHandler = new DownloadHandlerBuffer();
                yield return Request.SendWebRequest();
                Debug.Log(Request.downloadHandler.text);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator ExecuteAsync()
        {
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

            //Debug.Log("NWDOperationWebUnity ExecuteAsync() THREAD ID " + System.Threading.Thread.CurrentThread.GetHashCode().ToString());
            //callback error
            ProgressInvoke(0.0f, ResultInfos);

            //Operation progress
            Statut = BTBOperationState.InProgress;
            //float tStart = Time.time;

            // Put Sync in progress
            // ParentQueue.SynchronizeInProgress = true;
            // Send this operation in actual operation for this environment
            Parent.Controller[QueueName].ActualOperation = this;

            // Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();

            #if UNITY_EDITOR
            // Deselect all object
            Selection.activeObject = null;
            #endif

            // I prepare the data
            DataUploadPrepare();

            // I insert the data
            WWWForm tWWWForm = InsertDataInRequest(ResultInfos);

            ResultInfos.OctetUpload = tWWWForm.data.Length;
            using (Request = UnityWebRequest.Post(ServerBase(), tWWWForm))
            {
                //Request.timeout = kTimeOutOfRequest;
                Request.timeout = Environment.WebTimeOut;

                #if UNITY_EDITOR
                Request.timeout = Environment.EditorWebTimeOut;
                #endif

                // I prepare the header 
                // I put the header in my request
                InsertHeaderInRequest();

                // I send the data
                ResultInfos.WebDateTime = DateTime.Now;

                // Show Header Infos Up
                DebugShowHeaderUploaded(tWWWForm.data);

                // Send Request
                Request.SendWebRequest();

                // Notification of an Upload start
                BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, this));

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
                
                if (Request.isDone && Request.downloadHandler.isDone)
                {
                    ResultInfos.DownloadedDateTime = DateTime.Now;
                    ResultInfos.FinishDateTime = ResultInfos.DownloadedDateTime;
                    ResultInfos.OctetDownload = Request.downloadHandler.text.Length;

                    // Notification of an Download is done
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, this));

                    // Show Header Infos Down
                    DebugShowHeaderDownloaded(Request.downloadHandler.text);

                    // Show Header Infos Up vs Down
                    DebugShowHeaderTotal(Request.downloadHandler.text);
                }
                
                if (string.IsNullOrEmpty(Request.GetResponseHeader("obsolete")))
                {
                    if (string.IsNullOrEmpty(Request.GetResponseHeader("maintenance")))
                    {
                        // Check for error
                        if (Request.isNetworkError ||
                            Request.isHttpError ||
                            Request.downloadHandler.text.Equals(string.Empty))
                        {
                            RequestError();
                        }
                        else
                        {
                            Byte[] tByte = Request.downloadHandler.data;
                            string tByteToData = Encoding.ASCII.GetString(tByte);
                            Dictionary<string, object> tData = null;
                            if (Json.IsValidJson(tByteToData))
                            {
                                // Parse Json Data to Dictionary
                                tData = Json.Deserialize(tByteToData) as Dictionary<string, object>;                                
                            }
    
                            // If no data is parse from the downloadHandler
                            if (tData == null)
                            {
                                // Log DownloadHandler in console
                                RequestError(true);
                            }
                            else
                            {
                                if (SecureData == true)
                                {
                                    if (tData.ContainsKey("scr") && tData.ContainsKey("scrdgt"))
                                    {
                                        string tSCR = (string)tData["scr"];
                                        string tSCRDGT = (string)tData["scrdgt"];
    
                                        string tDigestValue = BTBSecurityTools.GenerateSha(Environment.SaltStart + tSCR + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
                                        if (tDigestValue != tSCRDGT)
                                        {
                                            ResultInfos.SetErrorCode("RQT98");
                                        }
                                        else
                                        {
                                            tData = BTBSecurityTools.RemoveAes(tSCR, Environment.DataSHAPassword, Environment.DataSHAVector, BTBSecurityAesTypeEnum.Aes128);
                                            if (tData == null)
                                            {
                                                ResultInfos.SetErrorCode("RQT99");
                                            }
                                            else
                                            {
                                                NWDDebug.Log("NWDOperationWebUnity DOWNLOADED DECODED = " + Json.Serialize(tData).Replace("\\\\r", "\r\n"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ResultInfos.SetErrorCode("RQT98");
                                    }
                                }
                                
                                // Request in Progress, send Invoke
                                ProgressInvoke(1.0f, ResultInfos);
    
                                ResultInfos.SetData(tData);
                                ResultInfos.OctetDownload = Request.downloadHandler.text.Length;
    
                                // memorize the token for next connection
                                if (!ResultInfos.token.Equals(string.Empty))
                                {
                                    if (Environment.RequesToken == ResultInfos.token)
                                    {
                                        // What the token is the same? It's not possible!
                                        ResultInfos.SetErrorCode("RQT95");
                                    }
                                    else
                                    {
                                        if (Environment.RequesToken == Request.GetResponseHeader("token"))
                                        {
                                            // What the token is not beetween respond and header? It's not possible!
                                            ResultInfos.SetErrorCode("RQT97");
                                        }
                                        else
                                        {
                                            //TODO : FIX THIS ERROR IN PREPROD!!!
                                            if (TestTemporalRequestHash(Request.GetResponseHeader("hash"), Request.GetResponseHeader("token")) == false)
                                            {
                                                // What the token is not valid!? It's not possible!
                                                ResultInfos.SetErrorCode("RQT96");
                                            }
                                            else
                                            {
                                                Environment.LastPreviewRequesToken = Environment.PreviewRequesToken;
                                                Environment.PreviewRequesToken = Environment.RequesToken;
                                                Environment.RequesToken = ResultInfos.token;
                                            }
                                        }
                                    }
                                }
    
                                // Check if error
                                if (ResultInfos.isError)
                                {
                                    Statut = BTBOperationState.Failed;
    
                                    if (ResultInfos.errorCode == "RQT90" ||
                                        ResultInfos.errorCode == "RQT91" ||
                                        ResultInfos.errorCode == "RQT92" ||
                                        ResultInfos.errorCode == "RQT93" ||
                                        ResultInfos.errorCode == "RQT94" ||
                                        ResultInfos.errorCode == "RQT95" ||
                                        ResultInfos.errorCode == "RQT96" ||
                                        ResultInfos.errorCode == "RQT97" ||
                                        ResultInfos.errorCode == "RQT98" ||
                                        ResultInfos.errorCode == "RQT99"
                                        )
                                    {
                                        // Notification of a Session expired
                                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, ResultInfos));
    
                                        // Restore for anonymous account
                                        NWDAppConfiguration.SharedInstance().SelectedEnvironment().RestaureAnonymousSession();
                                    }
                                    else
                                    {
                                        // Notification of a Web Error
                                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_ERROR, ResultInfos));
    
                                        if (ResultInfos.errorDesc != null)
                                        {
                                            if (ResultInfos.errorCode == "ACC98" ||
                                                ResultInfos.errorCode == "ACC99")
                                            {
                                                // Notification of an Account Banned
                                                BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_BANNED, ResultInfos));
                                            }
                                        }
                                    }
    
                                    // Application is in running mode
                                    if (Application.isPlaying == true)
                                    {
                                        NWDGameDataManager.UnitySingleton().ErrorManagement(ResultInfos.errorDesc);
                                    }
    
                                    // Notification of an Error
                                    if (ResultInfos.errorDesc != null)
                                    {
                                        #if UNITY_EDITOR
                                        ResultInfos.errorDesc.ShowNativeAlert();
                                        #else
                                        ResultInfos.errorDesc.PostNotificationError();
                                        #endif
                                    }
                                    
                                    // Request Failed, send Invoke
                                    FailInvoke(Request.downloadProgress, ResultInfos);
                                }
                                else if (ResultInfos.isNewUser && ResultInfos.isUserTransfert)
                                {
                                    string tUUID = ResultInfos.uuid;
                                    if (!tUUID.Equals(string.Empty))
                                    {
                                        tUserChange = true;
    
                                        NWDDataManager.SharedInstance().ChangeAllDatasForUserToAnotherUser(Environment, tUUID, ResultInfos.signkey);
                                        Statut = BTBOperationState.ReStart;
                                    }
                                }
                                else
                                {
                                    Statut = BTBOperationState.Success;
                                    string tUUID = ResultInfos.uuid;
    
                                    if (ResultInfos.isNewUser)
                                    {
                                        tUserChange = true;
                                    }
    
                                    if (!tUUID.Equals(string.Empty))
                                    {
                                        Environment.PlayerAccountReference = tUUID;
                                    }
    
                                    if (ResultInfos.isSignUpdate)
                                    {
                                        tUserChange = true;
    
                                        if (ResultInfos.isSignUp == true)
                                        {
                                            Environment.ResetAnonymousSession();
                                        }
    
                                        switch (ResultInfos.sign)
                                        {
                                            case NWDAppEnvironmentPlayerStatut.Anonymous:
                                                {
                                                    if (!tUUID.Equals(string.Empty))
                                                    {
                                                        Environment.AnonymousPlayerAccountReference = tUUID;
                                                    }
                                                    if (!ResultInfos.signkey.Equals(string.Empty))
                                                    {
                                                        Environment.AnonymousResetPassword = ResultInfos.signkey;
                                                    }
                                                }
                                                break;
                                            case NWDAppEnvironmentPlayerStatut.Temporary:
                                                {
                                                    if (Environment.PlayerAccountReference == Environment.AnonymousPlayerAccountReference)
                                                    {
                                                        //Using signed account as anonymous account = reset!
                                                        Environment.ResetAnonymousSession();
                                                    }
                                                }
                                                break;
                                            case NWDAppEnvironmentPlayerStatut.Facebook:
                                            case NWDAppEnvironmentPlayerStatut.Google:
                                            case NWDAppEnvironmentPlayerStatut.LoginPassword:
                                            case NWDAppEnvironmentPlayerStatut.Unknow:
                                                break;
                                        }
                                    }
    
                                    if (ResultInfos.isReloadingData)
                                    {
                                        //TODO : need reload data ?
                                    }
    
                                    // Update Data
                                    DataDownloadedCompute(ResultInfos);
    
                                    // Create or load User Account infos
                                    if (ResultInfos.isSignUpdate)
                                    {
                                        NWDAccountInfos.SetAccountType(ResultInfos.sign);
                                        Environment.PlayerStatut = ResultInfos.sign;
                                    }
    
                                    // Notification of a Download success
                                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, ResultInfos));
    
                                    // Request Success, send Invoke
                                    SuccessInvoke(Request.downloadProgress, ResultInfos);
                                }
                            }
                        }
                    }
                    else
                    {
                        ResultInfos.SetErrorCode("MAINTENANCE");
                        if (ResultInfos.errorDesc != null)
                        {
                            #if UNITY_EDITOR        
                            ResultInfos.errorDesc.ShowNativeAlert();
                            #else
                            ResultInfos.errorDesc.PostNotificationError();
                            #endif
                        }
                    }
                }
                else
                {
                    ResultInfos.SetErrorCode("OBSOLETE");
                    
                    if (ResultInfos.errorDesc != null)
                    {
                        #if UNITY_EDITOR
                        ResultInfos.errorDesc.ShowNativeAlert();
                        #else
                        ResultInfos.errorDesc.PostNotificationError();
                        #endif
                    }
                }
                
                // Save preference localy
                Environment.SavePreferences();

                // Notification of current Account have change
                if (tUserChange == true)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
                }

                Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RequestError(bool sJsonIsNull = false)
        {
            // Notification of a Donwload Error
            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, this));

            Statut = BTBOperationState.Error;
            if (Request.isNetworkError)
            {
                ResultInfos.SetErrorCode("WEB01");
            }
            else if (Request.isHttpError)
            {
                ResultInfos.SetErrorCode("WEB02");
            }
            else if (sJsonIsNull == true)
            {
                ResultInfos.SetErrorCode("WEB03");
            }
            else
            {
                ResultInfos.SetErrorCode("WEB04");
            }

            // Application is in running mode
            if (Application.isPlaying == true)
            {
                NWDGameDataManager.UnitySingleton().ErrorManagement(ResultInfos.errorDesc);
            }

            // Request Failed, send Invoke
            FailInvoke(Request.downloadProgress, ResultInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            ResultInfos.FinishDateTime = DateTime.Now;
            Statut = BTBOperationState.Cancel;
            if (Request != null)
            {
                Request.Abort();
                //TODO risk of token lost integrity : operation reconnect ?
            }

            NWDOperationResult tInfosCancel = new NWDOperationResult();
            CancelInvoke(Request.downloadProgress, tInfosCancel);
            IsFinish = true;
            Parent.NextOperation(QueueName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Finish()
        {
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
            Statut = BTBOperationState.Destroy;
            #if UNITY_EDITOR
            DestroyImmediate(GameObjectToSpawn);
            #else
            Destroy (GameObjectToSpawn);
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
        static string OSKey = "os";
        static string LangKey = "lang";
        static string VersionKey = "version";
        static string UUIDKey = "uuid";
        static string RequestTokenKey = "token";
        static string HashKey = "hash";
        #if UNITY_EDITOR
        static string AdminHashKey = "adminHash";
        #endif
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
            //TODO: Insert Header In Request
            HeaderParams.Clear();
            if (SecureData)
            {
                HeaderParams.Add(SecureKey, SecureDigestKey);
            }
            else
            {
                HeaderParams.Add(UnSecureKey, UnSecureDigestKey);
            }
            UUID = Environment.PlayerAccountReference;
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
            HeaderParams.Add(UUIDKey, UUID);
            HeaderParams.Add(RequestTokenKey, RequestToken);
            HeaderParams.Add(OSKey, OS);
            HeaderParams.Add(VersionKey, Version);
            HeaderParams.Add(LangKey, Lang);

            // create hash security
            string tHashValue = string.Format("{0}{1}{2}{3}{4}{5}", OS, Version, Lang, NWDToolbox.GenerateSALT(Environment.SaltFrequency), UUID, RequestToken);
            HeaderParams.Add(HashKey, BTBSecurityTools.GenerateSha(tHashValue, BTBSecurityShaTypeEnum.Sha1));

            #if UNITY_EDITOR
            // add hash for admin
            if (Application.isPlaying == false && Application.isEditor == true)
            {
                HeaderParams.Add(AdminHashKey, NWDToolbox.GenerateAdminHash(Environment.AdminKey, Environment.SaltFrequency));
            }
            #endif

            // insert dico of header in request header
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                Request.SetRequestHeader(tEntry.Key, tEntry.Value.ToString());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static string UnSecureKey = "prm";
        static string SecureKey = "scr";
        static string UnSecureDigestKey = "prmdgt";
        static string SecureDigestKey = "scrdgt";
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        //-------------------------------------------------------------------------------------------------------------
        public WWWForm InsertDataInRequest(NWDOperationResult sInfos)
        {
            WWWForm tBodyData = new WWWForm();
            string tParamKey = UnSecureKey;
            string tDigestKey = UnSecureDigestKey;
            string tParamValue = string.Empty;
            string tDigestValue = string.Empty;

            //Debug.Log("NWDOperationWebUnity UPLOADED Datas : " + Json.Serialize(Data).Replace("/r", string.Empty).Replace("/n", string.Empty));

            if (SecureData)
            {
                tParamKey = SecureKey;
                tDigestKey = SecureDigestKey;
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
                         tDebugRequestHeader +
                         "-------------------\n" +
                         "<b>Datas :</b> \n" +
                         "-------------------\n" +
                         Json.Serialize(Data).Replace("/r", string.Empty).Replace("/n", string.Empty) +
                         "-------------------\n" +
                         Encoding.UTF8.GetString(sData) +
                         "-------------------\n" +
                         ""
            );
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderDownloaded(string sData)
        {
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
                         tDebugResponseHeader +
                         "-------------------\n" +
                         "<b>Datas : (" + ResultInfos.OctetDownload + ")</b> \n" +
                         "-------------------\n" +
                         sData.Replace("\\\\r", "\r\n") + "\n" +
                         "-------------------\n" +
                         ""
            );
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderTotal(string sData)
        {
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
                         tDebugRequestHeader +
                         "-------------------\n" +
                         "<b>Headers DOWNLOAD :</b> \n" +
                         "-------------------\n" +
                         tDebugResponseHeader +
                         "-------------------\n" +
                         "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                         "-------------------\n" +
                         sData.Replace("\\\\r", "\r\n") + "\n" +
                         "-------------------\n" +
                         ""
            );
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================