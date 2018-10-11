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
            NWDOperationWebUnity rReturn = NWDOperationWebUnity.Create(sName, sEnvironment);
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
            // Add sync in the unitySingleton
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
            // 
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUnity>();
            rReturn.GameObjectToSpawn = tGameObjectToSpawn;
            rReturn.Environment = sEnvironment;
            rReturn.QueueName = sEnvironment.Environment;
#if UNITY_EDITOR
#else
            //DontDestroyOnLoad (tGameObjectToSpawn);
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
            // start coroutine ();
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
            string rURL = Environment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/Environment/" + Environment.Environment + "/" + ServerFile();
            NWDDebug.Log("URL : " + rURL);
            return rURL;
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
            float tStart = Time.time;

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

                // I prepare the header 
                // I put the header in my request
                InsertHeaderInRequest();

                // I send the data
                ResultInfos.WebDateTime = DateTime.Now;
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

                if (Request.isDone == true)
                {
                    ResultInfos.DownloadedDateTime = DateTime.Now;
                    ResultInfos.FinishDateTime = ResultInfos.DownloadedDateTime;

                    // Notification of an Download is done
                    NWDDebug.Log("NWDOperationWebUnity Upload / Download Request isDone: " + Request.isDone);
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, this));
                    NWDDebug.Log("NWDOperationWebUnity Request.isDone text DOWNLOADED: " + Request.downloadHandler.text.Replace("\\\\r", "\r\n"));
                }

                // Check for error
                if (Request.isNetworkError ||
                    Request.isHttpError ||
                    Request.downloadHandler.text.Equals(""))
                {
                    RequestError();
                }
                else
                {
                    // Parse Json Data to Dictionary
                    Dictionary<string, object> tData = Json.Deserialize(Request.downloadHandler.text) as Dictionary<string, object>;

                    // If no data is parse from the downloadHandler
                    if (tData == null)
                    {
                        // Log DownloadHandler in console
                        Debug.LogWarning(Request.downloadHandler.text);

                        RequestError(true);
                    }
                    else
                    {
                        // Request in Progress, send Invoke
                        ProgressInvoke(1.0f, ResultInfos);

                        ResultInfos.SetData(tData);
                        //OctetDownload = Request.downloadHandler.text.Length;
                        ResultInfos.OctetDownload = Request.downloadHandler.text.Length;

                        // memorize the token for next connection
                        if (!ResultInfos.token.Equals(""))
                        {
                            Environment.RequesToken = ResultInfos.token;
                        }

                        // Check if error
                        if (ResultInfos.isError)
                        {
                            Statut = BTBOperationState.Failed;

                            if (ResultInfos.errorCode == "RQT90" ||
                                ResultInfos.errorCode == "RQT91" ||
                                ResultInfos.errorCode == "RQT92" ||
                                ResultInfos.errorCode == "RQT93" ||
                                ResultInfos.errorCode == "RQT94")
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
                                    if(ResultInfos.errorCode == "ACC98" ||
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
                            if (!tUUID.Equals(""))
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

                            if (!tUUID.Equals(""))
                            {
                                Environment.PlayerAccountReference = tUUID;
                            }

                            if (ResultInfos.isSignUpdate)
                            {
                                tUserChange = true;

                                NWDUserInfos tActiveUser = NWDUserInfos.GetUserInfoByEnvironmentOrCreate(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                                Environment.PlayerStatut = ResultInfos.sign;
                                tActiveUser.AccountType = ResultInfos.sign;

                                if (ResultInfos.isSignUp == true)
                                {
                                    Environment.ResetAnonymousSession();
                                }

                                switch(ResultInfos.sign)
                                {
                                    case NWDAppEnvironmentPlayerStatut.Anonymous:
                                        {
                                            if (!tUUID.Equals(""))
                                            {
                                                Environment.AnonymousPlayerAccountReference = tUUID;
                                            }
                                            if (!ResultInfos.signkey.Equals(""))
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

                            DataDownloadedCompute(ResultInfos);

                            // Notification of a Download success
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, ResultInfos));

                            // Request Success, send Invoke
                            SuccessInvoke(Request.downloadProgress, ResultInfos);
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
            else if (sJsonIsNull == false)
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
        //-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        static string AdminHashKey = "adminHash";
        //-------------------------------------------------------------------------------------------------------------
        #endif
        //-------------------------------------------------------------------------------------------------------------
        public string OS;
        public string Lang;
        public string Version;
        public string UUID;
        public string RequestToken;
        //-------------------------------------------------------------------------------------------------------------
        public void InsertHeaderInRequest()
        {
            //TODO: Insert Header In Request
            Dictionary<string, object> tHeaderParams = new Dictionary<string, object>();

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
            tHeaderParams.Add(UUIDKey, UUID);
            tHeaderParams.Add(RequestTokenKey, RequestToken);
            tHeaderParams.Add(OSKey, OS);
            tHeaderParams.Add(VersionKey, Version);
            tHeaderParams.Add(LangKey, Lang);

            // create hash security
            string tHashValue = string.Format("{0}{1}{2}{3}{4}{5}", OS, Version, Lang, NWDToolbox.GenerateSALT(Environment.SaltFrequency), UUID, RequestToken);
            tHeaderParams.Add(HashKey, BTBSecurityTools.GenerateSha(tHashValue, BTBSecurityShaTypeEnum.Sha1));

            #if UNITY_EDITOR
            // add hash for admin
            if (Application.isPlaying == false && Application.isEditor == true)
            {
                tHeaderParams.Add(AdminHashKey, NWDToolbox.GenerateAdminHash(Environment.AdminKey, Environment.SaltFrequency));
            }
            string tDebug = "";

            // insert dico of header in request header
            foreach (KeyValuePair<string, object> tEntry in tHeaderParams)
            {
                Request.SetRequestHeader(tEntry.Key, tEntry.Value.ToString());
                tDebug += tEntry.Key + " = '" + tEntry.Value.ToString() + "' , ";
            }

            NWDDebug.Log("Header : " + tDebug);
            #else
            // insert dico of header in request header
            string tDebug = "";
            foreach (KeyValuePair<string, object> tEntry in tHeaderParams)
            {
                 Request.SetRequestHeader (tEntry.Key, tEntry.Value.ToString ());
                tDebug += tEntry.Key + " = '" + tEntry.Value.ToString() + "' , ";
            }

            NWDDebug.Log("Header : " + tDebug);
            #endif
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
            string tParamValue = "";
            string tDigestValue = "";
            NWDDebug.Log("Data : " + Json.Serialize(Data).Replace("/r","").Replace("/n", ""));

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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================