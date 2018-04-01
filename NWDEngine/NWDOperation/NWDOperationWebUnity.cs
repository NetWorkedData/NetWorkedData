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
    [ExecuteInEditMode]
    public partial class NWDOperationWebUnity : BTBOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        public static int kTimeOutOfRequest = 30;
        public GameObject GameObjectToSpawn;
        public bool SecureData = false;
        public UnityWebRequest Request;
        public NWDAppEnvironment Environment;
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
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUnity>();
            rReturn.GameObjectToSpawn = tGameObjectToSpawn;
            rReturn.Environment = sEnvironment;
            rReturn.QueueName = sEnvironment.Environment;
#if UNITY_EDITOR
#else
			DontDestroyOnLoad (tGameObjectToSpawn);
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
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
        IEnumerator ExecuteAsync()
        {
            Statut = BTBOperationState.Start;
            bool tUserChange = false;

            //Debug.Log("NWDOperationWebUnity ExecuteAsync() THREAD ID " + System.Threading.Thread.CurrentThread.GetHashCode().ToString());
            //callback error
            NWDOperationResult tInfos = new NWDOperationResult();
            ProgressInvoke(0.0f, tInfos);

            //Operation progress
            Statut = BTBOperationState.InProgress;
            float tStart = Time.time;

            // Put Sync in progress
            // ParentQueue.SynchronizeInProgress = true;
            // Send this operation in actual operation for this environment
            Parent.Controller[QueueName].ActualOperation = this;

            // Force all datas to be write in database
            NWDDataManager.SharedInstance().UpdateQueueExecute();

#if UNITY_EDITOR
            // Deselect all object
            Selection.activeObject = null;
#endif

            // I prepare the data
            DataUploadPrepare();

            // I insert the data
            WWWForm tWWWForm = InsertDataInRequest();
            using (Request = UnityWebRequest.Post(ServerBase(), tWWWForm))
            {
                Request.timeout = kTimeOutOfRequest;
                Debug.Log("URL : " + Request.url);

                // I prepare the header 
                // I put the header in my request
                InsertHeaderInRequest();

                // I send the data
                //TODO : update method
                //Request.Send();

                Request.SendWebRequest();
                Debug.Log("Request URL " + Request.url);

                while (!Request.isDone)
                {
                    Statut = BTBOperationState.InProgress;

                    NWDOperationResult tInfosProgress = new NWDOperationResult();
                    ProgressInvoke(Request.downloadProgress, tInfosProgress);

                    if (Request.uploadProgress < 1.0f)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_UPLOAD_IN_PROGRESS, this));
                    }
                    if (Request.downloadProgress < 1.0f)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_IN_PROGRESS, this));
                    }

                    yield return null;
                }

                if (Request.isDone == true)
                {
                    Debug.Log("NWDOperationWebUnity Upload / Download Request isDone: " + Request.isDone);
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_IS_DONE, this));
                    Debug.Log("NWDOperationWebUnity Request.isDone text DOWNLOADED: " + Request.downloadHandler.text.Replace("\\\\r", "\r\n"));
                }

                if (Request.isNetworkError)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

                    NWDGameDataManager.UnitySingleton().NetworkStatutChange(NWDNetworkState.OffLine);

                    Statut = BTBOperationState.Error;
                    NWDOperationResult tInfosError = new NWDOperationResult("WEB01");
                    FailInvoke(Request.downloadProgress, tInfosError);

                    if (Application.isPlaying == true)
                    {
                        NWDGameDataManager.UnitySingleton().ErrorManagement(tInfosError.errorDesc);
                    }
                }
                else if (Request.isHttpError)
                {
                    // Error
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

                    NWDGameDataManager.UnitySingleton().NetworkStatutChange(NWDNetworkState.OnLine);

                    Statut = BTBOperationState.Error;
                    NWDOperationResult tInfosError = new NWDOperationResult("WEB02");
                    tInfosError.Octects = 0;
                    FailInvoke(Request.downloadProgress, tInfosError);

                    if (Application.isPlaying == true)
                    {
                        NWDGameDataManager.UnitySingleton().ErrorManagement(tInfosError.errorDesc);
                    }
                }
                else
                {
                    // Success
                    NWDOperationResult tInfosProgress = new NWDOperationResult();
                    ProgressInvoke(1.0f, tInfosProgress);

                    NWDGameDataManager.UnitySingleton().NetworkStatutChange(NWDNetworkState.OnLine);

                    Dictionary<string, object> tData = new Dictionary<string, object>();
                    if (Request.downloadHandler.text.Equals(""))
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

                        Statut = BTBOperationState.Error;
                        NWDOperationResult tInfosFail = new NWDOperationResult("WEB03");
                        tInfosFail.Octects = 0;
                        FailInvoke(Request.downloadProgress, tInfosFail);

                        if (Application.isPlaying == true)
                        {
                            NWDGameDataManager.UnitySingleton().ErrorManagement(tInfosFail.errorDesc);
                        }
                    }
                    else
                    {
                        tData = Json.Deserialize(Request.downloadHandler.text) as Dictionary<string, object>;

                        // TODO : TOKEN IS FAILED : DISCONNECT AND RESET DATA FOR THIS USER... NO SYNC AUTHORIZED... DELETE LOCAL DATA... RESTAURE FROM LOGIN
                        if (tData == null)
                        {
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

                            Statut = BTBOperationState.Error;
                            NWDOperationResult tInfosFail = new NWDOperationResult("WEB04");
                            tInfosFail.Octects = Request.downloadHandler.text.Length;
                            FailInvoke(Request.downloadProgress, tInfosFail);

                            if (Application.isPlaying == true)
                            {
                                NWDGameDataManager.UnitySingleton().ErrorManagement(tInfosFail.errorDesc);
                            }
                        }
                        else
                        {
                            NWDOperationResult tInfosResult = new NWDOperationResult(tData);
                            tInfosResult.Octects = Request.downloadHandler.text.Length;

                            // memorize the token for next connection
                            if (!tInfosResult.token.Equals(""))
                            {
                                Environment.RequesToken = tInfosResult.token;
                            }

                            // Check if error
                            if (tInfosResult.isError)
                            {
                                Statut = BTBOperationState.Failed;

                                //TODO if error do something
                                if (tInfosResult.errorCode == "RQT90" ||
                                    tInfosResult.errorCode == "RQT91" ||
                                    tInfosResult.errorCode == "RQT92" ||
                                    tInfosResult.errorCode == "RQT93" ||
                                    tInfosResult.errorCode == "RQT94")
                                {
                                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_SESSION_EXPIRED, tInfosResult));

                                    // TODO : Change for anonymous account
                                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().RestaureAnonymousSession();
                                }
                                else
                                {
                                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_ERROR, tInfosResult));
                                }

                                FailInvoke(Request.downloadProgress, tInfosResult);
                                if (Application.isPlaying == true)
                                {
                                    NWDGameDataManager.UnitySingleton().ErrorManagement(tInfosResult.errorDesc);
                                }
                            }
                            else if (tInfosResult.isNewUser && tInfosResult.isUserTransfert)
                            {
                                string tUUID = tInfosResult.uuid;
                                if (!tUUID.Equals(""))
                                {
                                    //TODO :  notify user change

                                    NWDDataManager.SharedInstance().ChangeAllDatasForUserToAnotherUser(Environment, tUUID, tInfosResult.signkey);
                                    Statut = BTBOperationState.ReStart;
                                    tUserChange = true;
                                }
                            }
                            else
                            {
                                Statut = BTBOperationState.Success;
                                string tUUID = tInfosResult.uuid;

                                if (tInfosResult.isNewUser)
                                {
                                    //TODO :  notify user change
                                    tUserChange = true;
                                }

                                if (!tUUID.Equals(""))
                                {
                                    Environment.PlayerAccountReference = tUUID;
                                }

                                if (tInfosResult.isSignUpdate)
                                {
                                    tUserChange = true;
                                    NWDUserInfos tActiveUser = NWDUserInfos.GetUserInfoByEnvironmentOrCreate(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                                    Environment.PlayerStatut = tInfosResult.sign;
                                    tActiveUser.AccountType = tInfosResult.sign;
                                    if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Unknow)
                                    {
                                    }
                                    else if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.LoginPassword)
                                    {
                                    }
                                    else if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Facebook)
                                    {
                                    }
                                    else if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Google)
                                    {
                                    }
                                    else if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Anonymous)
                                    {
                                        if (!tUUID.Equals(""))
                                        {
                                            Environment.AnonymousPlayerAccountReference = tUUID;
                                        }
                                        if (!tInfosResult.signkey.Equals(""))
                                        {
                                            Environment.AnonymousResetPassword = tInfosResult.signkey;
                                        }
                                    }
                                    else if (tInfosResult.sign != NWDAppEnvironmentPlayerStatut.Temporary)
                                    {
                                        if (Environment.PlayerAccountReference == Environment.AnonymousPlayerAccountReference)
                                        {
                                            //Using signed account as anonymous account = reset!
                                            Environment.ResetAnonymousSession();
                                        }
                                    }
                                }

                                if (tInfosResult.isReloadingData)
                                {
                                    //TODO : need reload data
                                }

                                DataDownloadedCompute(tInfosResult);

                                BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_DOWNLOAD_SUCCESSED, tInfosResult));
                                SuccessInvoke(Request.downloadProgress, tInfosResult);
                            }
                        }
                    }
                }

                //Save preference localy
                Environment.SavePreferences();

                //Perform next operation

                if (tUserChange == true)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_USER_CHANGE, null));
                }

                Finish();

            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            this.Statut = BTBOperationState.Cancel;
            if (Request != null)
            {
                Request.Abort();
                //TODO risk of token lost integrity : operation reconnect ?
            }

            NWDOperationResult tInfosCancel = new NWDOperationResult();
            CancelInvoke(Request.downloadProgress, tInfosCancel);

            IsFinish = true;
            Parent.NextOperation(this.QueueName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Finish()
        {
            if (Statut == BTBOperationState.ReStart)
            {
                Debug.Log("I MUST RESTART THE REQUEST BECAUSE BEFORE I WAS TEMPORARY ACCOUNT");
                Parent.ReplayOperation(this.QueueName);
            }
            else
            {
                this.Statut = BTBOperationState.Finish;
                IsFinish = true;
                Parent.NextOperation(this.QueueName);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DestroyThisOperation()
        {
            this.Statut = BTBOperationState.Destroy;
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

            Debug.Log("Header : " + tDebug);
#else
            // insert dico of header in request header
            string tDebug = "";
            foreach (KeyValuePair<string, object> tEntry in tHeaderParams)
            {
                 Request.SetRequestHeader (tEntry.Key, tEntry.Value.ToString ());
                tDebug += tEntry.Key + " = '" + tEntry.Value.ToString() + "' , ";
            }

            Debug.Log("Header : " + tDebug);
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
        public WWWForm InsertDataInRequest()
        {
            WWWForm tBodyData = new WWWForm();
            string tParamKey = UnSecureKey;
            string tDigestKey = UnSecureDigestKey;
            string tParamValue = "";
            string tDigestValue = "";
            Debug.Log("Data : " + Json.Serialize(Data));

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
                NWDEditorMenu.EnvironementSync().SendOctects = tParamValue.Length + tDigestValue.Length;
            }
#endif
            return tBodyData;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================