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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif

#if DEBUG
//#define NWD_LOG
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using NWEMiniJSON;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationWebAction
    {
        Sync = 0,
        SignIn = 1,
        SignOut = 2,
        Rescue = 3,
#if UNITY_EDITOR
        Management = 9,
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationFinalStatut
    {
        Success = 0,
        Fail = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [ExecuteInEditMode]
    public partial class NWDOperationWebUnity : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        NWDOperationFinalStatut FinalStatut;
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
            //Debug.Log("NWDOperationWebUnity AddOperation()");
            // change selected environement! automaticcally?!
            NWDAppEnvironment.SetEnvironment(sEnvironment);
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
        public void DataAddSecretDevicekey()
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
            Environment.CleanSecretKeyDevice();
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
            return Environment.GetServerHTTPS() + "/" + tFolderWebService + "/" + Environment.Environment + "/" + ServerFile();
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
            //string tBenchmark = NWDBenchmark.GetKeyWihRandom();
            //NWDBenchmark.Start(tBenchmark);
            //Debug.Log("NWDOperationWebUnity ExecuteAsync()");
            ResultInfos = new NWDOperationResult();
            ResultInfos.Benchmark.Start();
            // reinit benchmark stat values
            //ResultInfos.PrepareDateTime = DateTime.Now;
            //ResultInfos.WebDateTime = DateTime.Now;
            //ResultInfos.UploadedDateTime = DateTime.Now;
            //ResultInfos.DownloadedDateTime = DateTime.Now;
            //ResultInfos.FinishDateTime = DateTime.Now;
            ResultInfos.OctetUpload = 0.0F;
            ResultInfos.OctetDownload = 0.0F;
            ResultInfos.ClassPullCounter = 0;
            ResultInfos.ClassPushCounter = 0;
            ResultInfos.RowPullCounter = 0;
            ResultInfos.RowPushCounter = 0;
            Statut = NWEOperationState.Start;
            bool tUserChange = false;
            ProgressInvoke(0.0f, ResultInfos);
            //Operation progress
            Statut = NWEOperationState.InProgress;
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

            // change selected environement! automaticcally?!
            //NWDAccountInfos tAccountInfos = NWDBasisHelper.GetCorporateFirstData<NWDAccountInfos>(Environment.PlayerAccountReference, null);
            NWDAppEnvironment.SetEnvironment(Environment);
            if (Environment.CurrentAccountIsCertified() == false)
            {
                DataAddSecretDevicekey();
            }

            //NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            //if (tAccountInfos == null)
            //{
            //    DataAddSecretDevicekey();
            //}
            //else
            //{
            //    if (tAccountInfos.AccountType() == NWDAppEnvironmentPlayerStatut.Temporary)
            //    {
            //        DataAddSecretDevicekey();
            //    }
            //}
            // I prepare the data
            DataUploadPrepare();
            // I insert the data
            WWWForm tWWWForm = InsertDataInRequest(ResultInfos);
            ResultInfos.OctetUpload = tWWWForm.data.Length;
            ResultInfos.URL = ServerBase();
            //NWDBenchmark.Step(tBenchmark);

            using (Request = UnityWebRequest.Post(ServerBase(), tWWWForm))
            {
                //ResultInfos.URL = Request.url;
                //Request.uploadHandler = new UploadHandler(tWWWForm);
                Request.downloadHandler = new DownloadHandlerBuffer();
                Request.timeout = Environment.WebTimeOut;
#if UNITY_EDITOR
                Request.timeout = Environment.EditorWebTimeOut;
#endif
                // I prepare the header 
                // I put the header in my request
                InsertHeaderInRequest();
                // I send the data
                //ResultInfos.WebDateTime = DateTime.Now;
                // Debug Show Header Uploaded
                DebugShowHeaderUploaded(tWWWForm.data);
                // Notification of an Upload start
                NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, this));
                //yield return Request.SendWebRequest();
                ResultInfos.Benchmark.PrepareIsFinished();
                Request.SendWebRequest();
                while (!Request.isDone)
                {
                    //Debug.Log(" request Request.uploadProgress " + Request.uploadProgress.ToString("#0.000") + " Request.downloadProgress " + Request.downloadProgress.ToString("#0.000")); ;
                    Statut = NWEOperationState.InProgress;
                    ProgressInvoke(Request.downloadProgress, ResultInfos);
                    if (Request.uploadProgress < 1.0f)
                    {
                        // Notification of an Upload in progress
                        NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, this));
                    }
                    if (Request.uploadProgress == 1.0F)
                    {
                        ResultInfos.Benchmark.UploadIsFinished();
                    }
                    if (Request.downloadProgress > 0.1f)
                    {
                        ResultInfos.Benchmark.PerformIsFinished();
                        ResultInfos.Benchmark.UploadIsFinished();
                    }
                    if (Request.downloadProgress < 1.0f)
                    {
                        // Notification of an Download in progress
                        NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, this));
                    }
                    yield return null;
                }
#if UNITY_2019
                if (Request.isNetworkError)
#elif UNITY_2020
                if (Request.result == UnityWebRequest.Result.ConnectionError) //obsolete Request.isNetworkError
#endif
                {
#if UNITY_EDITOR
                    Debug.Log(Request.error + "\n" + Request.downloadHandler.text + "\n" + Request);
#endif
                    Statut = NWEOperationState.Error;
                    ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_WEB01));
                }
#if UNITY_2019
                else if (Request.isHttpError)
#elif UNITY_2020
                else if (Request.result == UnityWebRequest.Result.ProtocolError) //obsolete Request.isHttpError
#endif
                {
#if UNITY_EDITOR
                    Debug.Log(Request.error + "\n" + Request.downloadHandler.text + "\n " + Request.url);
#endif
                    Statut = NWEOperationState.Error;
                    ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_WEB02));
                }
                else
                {
                    ResultInfos.Benchmark.UploadIsFinished();
                    ResultInfos.Benchmark.PerformIsFinished();
                    ResultInfos.Benchmark.DownloadIsFinished();
                    while (!Request.downloadHandler.isDone)
                    {
                        yield return null;
                    }
                    //ResultInfos.Benchmark.DownloadIsFinished();
                    if (Request.isDone == true)
                    {
                        //NWDBenchmark.Step(tBenchmark);
                        string tDataConverted = Request.downloadHandler.text;
                        ResultInfos.OctetDownload = tDataConverted.Length;
                        // Notification of an Download is done
                        NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, this));
                        // Debug Show Header Download
                        DebugShowHeaderDownloaded(tDataConverted);
                        // Debug Show Header Up vs Down
                        DebugShowHeaderTotal(tDataConverted);
                        //NWDBenchmark.Step(tBenchmark);
                        // Check for error
                        if (tDataConverted.Equals(string.Empty))
                        {
                            ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_WEB04));
                            // Application is in running mode
                            // Request Failed, send Invoke
                            //FailInvoke(Request.downloadProgress, ResultInfos);
                            FinalStatut = NWDOperationFinalStatut.Fail;
                            DebugShowHeaderOnError(tDataConverted);
                            //NWDBenchmark.Step(tBenchmark);
                        }
                        else
                        {
                            //NWDBenchmark.Step(tBenchmark);
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
                                        ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_WEB03));
                                        // Notification of a Download success
                                        NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_ERROR, ResultInfos));
                                        // Request Failed, send Invoke
                                        //FailInvoke(Request.downloadProgress, ResultInfos);
                                        FinalStatut = NWDOperationFinalStatut.Fail;
                                    }
                                    else
                                    {
                                        if (SecureData)
                                        {
                                            if (tData.ContainsKey(NWEUnityWebService.SecureKey) && tData.ContainsKey(NWEUnityWebService.SecureDigestKey))
                                            {
                                                string tSCR = (string)tData[NWEUnityWebService.SecureKey];
                                                string tSCRDGT = (string)tData[NWEUnityWebService.SecureDigestKey];
                                                string tDigestValue = NWESecurityTools.GenerateSha(Environment.SaltStart + tSCR + Environment.SaltEnd, NWESecurityShaTypeEnum.Sha1);
                                                if (tDigestValue != tSCRDGT)
                                                {
                                                    ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT98));
                                                }
                                                else
                                                {
                                                    tData = NWESecurityTools.RemoveAes(tSCR, Environment.DataSHAPassword, Environment.DataSHAVector, NWESecurityAesTypeEnum.Aes128);
                                                    if (tData == null)
                                                    {
                                                        ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT99));
                                                    }
                                                    else
                                                    {
                                                        DebugShowHeaderTotalDecoded(tData);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT98));
                                            }
                                        }
                                        //NWDBenchmark.Step(tBenchmark);
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
                                                ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT95));
                                            }
                                            else
                                            {
                                                if (Environment.RequesToken == Request.GetResponseHeader(NWD.K_WEB_REQUEST_TOKEN_KEY))
                                                {
                                                    // What the token is not beetween respond and header? It's not possible!
                                                    ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT97));
                                                }
                                                else
                                                {
                                                    if (TestTemporalRequestHash(Request.GetResponseHeader(NWD.HashKey), Request.GetResponseHeader(NWD.K_WEB_REQUEST_TOKEN_KEY)) == false)
                                                    {
                                                        // What the token is not valid!? It's not possible!
                                                        ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_RQT96));
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
                                        //NWDBenchmark.Step(tBenchmark);
                                        // Check if error
                                        if (ResultInfos.isError)
                                        {

                                            NWDError tNWDError_RQT90 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT90);
                                            NWDError tNWDError_RQT91 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT91);
                                            NWDError tNWDError_RQT92 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT92);
                                            NWDError tNWDError_RQT93 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT93);
                                            NWDError tNWDError_RQT94 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT94);
                                            NWDError tNWDError_RQT95 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT95);
                                            NWDError tNWDError_RQT96 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT96);
                                            NWDError tNWDError_RQT97 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT97);
                                            NWDError tNWDError_RQT98 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT98);
                                            NWDError tNWDError_RQT99 = NWDError.GetErrorDomainCode(NWDError.NWDError_RQT99);

                                            NWDError tNWDError_ACC98 = NWDError.GetErrorDomainCode(NWDError.NWDError_ACC98);
                                            NWDError tNWDError_ACC99 = NWDError.GetErrorDomainCode(NWDError.NWDError_ACC99);

                                            Statut = NWEOperationState.Failed;
                                            if (tNWDError_RQT90 != null &&
                                                tNWDError_RQT91 != null &&
                                                tNWDError_RQT92 != null &&
                                                tNWDError_RQT93 != null &&
                                                tNWDError_RQT94 != null &&
                                                tNWDError_RQT95 != null &&
                                                tNWDError_RQT96 != null &&
                                                tNWDError_RQT97 != null &&
                                                tNWDError_RQT98 != null &&
                                                tNWDError_RQT99 != null)
                                            {
                                                if (ResultInfos.errorCode == tNWDError_RQT90.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT91.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT92.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT93.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT94.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT95.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT96.Code ||
                                                    ResultInfos.errorCode == tNWDError_RQT97.Code)
                                                {
                                                    // Notification of a Session expired
                                                    NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, ResultInfos));
                                                    // Restore for anonymous account
                                                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
                                                }
                                                else
                                                {
                                                    if (ResultInfos.errorDesc != null)
                                                    {
                                                        if (ResultInfos.errorCode == tNWDError_ACC98.Code ||
                                                            ResultInfos.errorCode == tNWDError_ACC99.Code)
                                                        {
                                                            // Notification of an Account Banned
                                                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_BANNED, ResultInfos));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //NWDBenchmark.Step(tBenchmark);
                                            Statut = NWEOperationState.Success;
                                            if (ResultInfos.isNewUser)
                                            {
                                                tUserChange = true;
                                                //NWDBenchmark.Log(" CHANGE USER");
                                                CanRestart();
                                                if (ResultInfos.isUserTransfert)
                                                {
                                                    //NWDBenchmark.Log(" IS TRANSFERT USER");
#if NWD_RGPD
                                                    NWDGDPR.Log("The temporary account will be transfert to certified account...");
                                                    if (!ResultInfos.uuid.Equals(string.Empty))
                                                    {
                                                        NWDDataManager.SharedInstance().ChangeAllDatasForUserToAnotherUser(Environment, ResultInfos.preview_user, ResultInfos.next_user /*, ResultInfos.signkey*/);
                                                    }
                                                    NWDGDPR.Log("The temporary account was transfert to certified account!");
#endif
                                                }
                                                else
                                                {
                                                    if (NWDAppConfiguration.SharedInstance().PurgeOldAccountDatabase == true)
                                                    {
                                                        // the best way is to delete all data in the database ... it's slow but better for security
                                                        // change database will be an option, but not secure, the data stay in the old database anyway it's crypted!
                                                        //NWDBenchmark.Start("PURGE ACCOUNT DATABASE");
                                                        if (Application.isEditor == false)
                                                        {
#if NWD_RGPD
                                                            NWDGDPR.Log("Purge database from all old account informations. The old account will be deleted from this device...");
#endif
                                                            // I drop all table account connected?
                                                            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
                                                            {
                                                                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                                                                tHelper.FlushTable();
                                                                //tHelper.ResetDatas();
                                                            }
#if NWD_RGPD
                                                            NWDGDPR.Log("The old account was deleted from this device!");
#endif
                                                        }
                                                        else
                                                        {
                                                            //NWDBenchmark.Log("!!! bypassed because it's editor");
                                                        }
                                                        //NWDBenchmark.Finish("PURGE ACCOUNT DATABASE");
                                                    }
                                                }
#if NWD_RGPD
                                                NWDGDPR.Log("New certified account valid on this device!");
#endif
                                            }
                                            if (!ResultInfos.uuid.Equals(string.Empty))
                                            {
                                                Environment.SetAccountReference(ResultInfos.uuid);
                                                Environment.SetAccountSalt(ResultInfos.salt);
                                            }

                                            //NWDBenchmark.Step(tBenchmark);
                                            DataDownloadedCompute(ResultInfos);

                                            // Notification of a Download success
                                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, ResultInfos));

                                            // Request Success, send Invoke
                                            //SuccessInvoke(Request.downloadProgress, ResultInfos);
                                            FinalStatut = NWDOperationFinalStatut.Success;
                                        }
                                    }
                                }
                                else
                                {
                                    ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_MAINTENANCE));
                                }
                            }
                            else
                            {
                                ResultInfos.SetError(NWDError.GetErrorDomainCode(NWDError.NWDError_OBSOLETE));
                            }
                        }
                    }
                    // Save preference localy
                    //NWDBenchmark.Step(tBenchmark);
                    Environment.SavePreferences();
                    //NWDBenchmark.Step(tBenchmark);

                    // Notification of current Account have change
                    if (tUserChange == true)
                    {
                        NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
                    }
                }

                //NWDBenchmark.Step(tBenchmark);
                if (ResultInfos.isError)
                {
                    if (ResultInfos.errorDesc != null)
                    {
#if UNITY_EDITOR
                        DebugShowHeaderUploaded(tWWWForm.data);
                        EditorUtility.DisplayDialog("Error: " + ResultInfos.errorDesc.Code, "" + ResultInfos.errorDesc.Title + "\n" + ResultInfos.errorDesc.Description, "OK");
#endif

                        if (ResultInfos.errorInfos != null)
                        {
                            ResultInfos.errorDesc.ShowAlert(ResultInfos.errorInfos);
                        }
                    }

                    // Notification of an web operation error
                    NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_ERROR, ResultInfos));

                    // Notification of a download failed
                    NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, ResultInfos));

                    // Invoke fail callback methode
                    //FailInvoke(Request.downloadProgress, ResultInfos);
                    FinalStatut = NWDOperationFinalStatut.Fail;
                }
                //NWDBenchmark.Step(tBenchmark);
                Finish();
            }
            //NWDBenchmark.Step(tBenchmark);
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
            //NWDBenchmark.Finish(tBenchmark);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            //Debug.Log("NWDOperationWebUnity Cancel()");
            if (IsFinish == false)
            {
                //ResultInfos.FinishDateTime = DateTime.Now;
                ResultInfos.Benchmark.ComputeIsFinished();
                ResultInfos.Benchmark.IsFinished();
                Statut = NWEOperationState.Cancel;
                NWDOperationResult tInfosCancel = new NWDOperationResult();
                IsFinish = true;
                if (Request != null)
                {
                    CancelInvoke(Request.downloadProgress, tInfosCancel);
                    Request.Abort();
                    Request.Dispose();
                }
                Parent.NextOperation(QueueName);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Finish()
        {
            //Debug.Log("NWDOperationWebUnity Finish()");
            if (IsFinish == false)
            {
                //ResultInfos.FinishDateTime = DateTime.Now;
                ResultInfos.Benchmark.ComputeIsFinished();
                ResultInfos.Benchmark.IsFinished();
                if (Statut == NWEOperationState.ReStart)
                {
                    // I MUST RESTART THE REQUEST BECAUSE BEFORE I WAS TEMPORARY ACCOUNT
                    Parent.ReplayOperation(QueueName);
                }
                else
                {
                    Statut = NWEOperationState.Finish;
                    IsFinish = true;

                    switch (FinalStatut)
                    {
                        case NWDOperationFinalStatut.Success:
                            {
                                SuccessInvoke(Request.downloadProgress, ResultInfos);
                            }
                            break;
                        case NWDOperationFinalStatut.Fail:
                            {
                                FailInvoke(Request.downloadProgress, ResultInfos);
                            }
                            break;
                    }
                    Parent.NextOperation(QueueName);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DestroyThisOperation()
        {
            //Debug.Log("NWDOperationWebUnity DestroyThisOperation()");
            Statut = NWEOperationState.Destroy;
#if UNITY_EDITOR
            DestroyImmediate(GameObjectToSpawn);
#else
            Destroy(GameObjectToSpawn);
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
            UUID = Environment.GetAccountReference();
            if (SecureData)
            {
                HeaderParams.Add(NWEUnityWebService.SecureKey, NWEUnityWebService.SecureDigestKey);
            }
            else
            {
                HeaderParams.Add(NWEUnityWebService.UnSecureKey, NWEUnityWebService.UnSecureDigestKey);
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
            HeaderParams.Add(NWD.K_WEB_UUID_KEY, UUID);
            HeaderParams.Add(NWD.K_WEB_REQUEST_TOKEN_KEY, RequestToken);
            HeaderParams.Add(NWD.K_WEB_HEADER_OS_KEY, OS);
            HeaderParams.Add(NWD.K_WEB_HEADER_VERSION_KEY, Version);
            HeaderParams.Add(NWD.K_WEB_HEADER_LANG_KEY, Lang);
            // create hash security
            string tHashValue = string.Format("{0}{1}{2}{3}{4}{5}", OS, Version, Lang, NWDToolbox.GenerateSALT(Environment.SaltFrequency), UUID, RequestToken);
            HeaderParams.Add(NWD.HashKey, NWESecurityTools.GenerateSha(tHashValue, NWESecurityShaTypeEnum.Sha1));

            switch (NWDLauncher.CompileAs())
            {
                case NWDCompileType.Editor:
                    {
#if UNITY_EDITOR
                        NWDCluster tCluster = NWDCluster.SelectClusterforEnvironment(Environment, true);
                        if (tCluster != null)
                        {
                            if (tCluster.AdminKey != null)
                            {
                                HeaderParams.Add(NWD.AdminHashKey, NWDToolbox.GenerateAdminHash(tCluster.AdminKey.Decrypt(), Environment.SaltFrequency));
                            }
                        }
#endif
                    }
                    break;
                case NWDCompileType.PlayMode:
                    {
                    }
                    break;
                case NWDCompileType.Runtime:
                    {
                    }
                    break;
            }
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
            string tParamKey = NWEUnityWebService.UnSecureKey;
            string tDigestKey = NWEUnityWebService.UnSecureDigestKey;
            string tParamValue = string.Empty;
            string tDigestValue = string.Empty;
            if (SecureData)
            {
                tParamKey = NWEUnityWebService.SecureKey;
                tDigestKey = NWEUnityWebService.SecureDigestKey;
                tParamValue = NWESecurityTools.AddAes(Data, Environment.DataSHAPassword, Environment.DataSHAVector, NWESecurityAesTypeEnum.Aes128);
                tDigestValue = NWESecurityTools.GenerateSha(Environment.SaltStart + tParamValue + Environment.SaltEnd, NWESecurityShaTypeEnum.Sha1);
            }
            else
            {
                tParamValue = NWESecurityTools.Base64Encode(Json.Serialize(Data));
                tDigestValue = NWESecurityTools.GenerateSha(Environment.SaltStart + tParamValue + Environment.SaltEnd, NWESecurityShaTypeEnum.Sha1);
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
            if (NWESecurityTools.GenerateSha(tSaltA + sVector + sToken) == sHash ||
                NWESecurityTools.GenerateSha(tSaltB + sVector + sToken) == sHash ||
                NWESecurityTools.GenerateSha(tSaltC + sVector + sToken) == sHash)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderUploaded(byte[] sData)
        {
            if (NWDDebug.IsActivated())
            {
#if UNITY_EDITOR
                string tDebugRequestHeader = string.Empty;
                foreach (KeyValuePair<string, object> tEntry in HeaderParams)
                {
                    tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
                }
                NWDDebug.Log("*******************************************************************\n" +
                            "NWDOperationWebUnity UPLOADED " + name + "\n" +
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
                             "*******************************************************************\n"
                );
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderDownloaded(string sData)
        {
            if (NWDDebug.IsActivated())
            {
#if UNITY_EDITOR
                //string tDebugResponseHeader = string.Empty;
                //foreach (KeyValuePair<string, string> tEntry in Request.GetResponseHeaders())
                //{
                //    tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , \n";
                //}
                //NWDDebug.Log("*******************************************************************\n" +
                //    "NWDOperationWebUnity DOWNLOADED " + name + "\n" +
                //             "-------------------\n" +
                //             "<b>Request URl :</b> " + Request.url + "\n" +
                //             "-------------------\n" +
                //             "<b>Headers :</b> \n" +
                //             "-------------------\n" +
                //             tDebugResponseHeader + "\n" +
                //             "-------------------\n" +
                //             "<b>Datas : (" + ResultInfos.OctetDownload + ")</b> \n" +
                //             "-------------------\n" +
                //             sData.Replace("\\\\r", "\r\n") + "\n" +
                //             "-------------------\n" +
                //             "*******************************************************************\n"
                //);
#endif
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderOnError(string sData)
        {
            string tDebugRequestHeader = string.Empty;
            foreach (KeyValuePair<string, object> tEntry in HeaderParams)
            {
                tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , "; //, \n";
            }
            string tDebugResponseHeader = string.Empty;
            if (Request != null)
            {
                Dictionary<string, string> tResponseHeaders = Request.GetResponseHeaders();
                if (tResponseHeaders != null)
                {
                    foreach (KeyValuePair<string, string> tEntry in tResponseHeaders)
                    {
                        tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , "; //, \n";
                    }
                }
                NWDDebug.Log("*******************************************************************\n" +
                                    "NWDOperationWebUnity UPLOAD VS DOWNLOADED " + name + "\n" +
                                    "-------------------\n" +
                                    "<b>Request URl :</b> " + Request.url + "\n" +
                                    "-------------------\n" +
                                    "<b>Headers UPLOAD :</b> \n" +
                                    "-------------------\n" +
                                    tDebugRequestHeader + "\n" +
                                    "-------------------\n" +
                                    "<b>Datas UPLOAD : </b> \n" +
                                    "-------------------\n" +
                                    Json.Serialize(Data) + "\n" +
                                    "-------------------\n\n\n" +
                                    "-------------------\n" +
                                    "<b>Headers DOWNLOAD :</b> \n" +
                                    "-------------------\n" +
                                    tDebugResponseHeader + "\n" +
                                    "-------------------\n" +
                                    "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                                    "-------------------\n" +
                                    sData.Replace("\\\\r", "\r\n") + "\n" +
                                    "-------------------\n" +
                                    "*******************************************************************\n");
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderTotal(string sData)
        {
            if (NWDDebug.IsActivated())
            {
                string tDebugRequestHeader = string.Empty;
                foreach (KeyValuePair<string, object> tEntry in HeaderParams)
                {
                    tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , "; //, \n";
                }
                string tDebugResponseHeader = string.Empty;
                if (Request != null)
                {
                    Dictionary<string, string> tResponseHeaders = Request.GetResponseHeaders();
                    if (tResponseHeaders != null)
                    {
                        foreach (KeyValuePair<string, string> tEntry in tResponseHeaders)
                        {
                            tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , "; //, \n";
                        }
                    }
                    NWDDebug.Log("*******************************************************************\n" +
                                  "NWDOperationWebUnity UPLOAD VS DOWNLOADED " + name + "\n" +
                                  "-------------------\n" +
                                  "<b>Request URl :</b> " + Request.url + "\n" +
                                  "-------------------\n" +
                                  "<b>Headers UPLOAD :</b> \n" +
                                  "-------------------\n" +
                                  tDebugRequestHeader + "\n" +
                                  "-------------------\n" +
                                  "<b>Datas UPLOAD : </b> \n" +
                                  "-------------------\n" +
                                  Json.Serialize(Data) + "\n" +
                                  "-------------------\n\n\n" +
                                  "-------------------\n" +
                                  "<b>Headers DOWNLOAD :</b> \n" +
                                  "-------------------\n" +
                                  tDebugResponseHeader + "\n" +
                                  "-------------------\n" +
                                  "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                                  "-------------------\n" +
                                  sData.Replace("\\\\r", "\r\n") + "\n" +
                                  "-------------------\n" +
                                  "*******************************************************************\n");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DebugShowHeaderTotalDecoded(Dictionary<string, object> sDico)
        {
            if (NWDDebug.IsActivated())
            {
                string tData = Json.Serialize(sDico).Replace("\\\\r", "\r\n");
                string tDebugRequestHeader = string.Empty;
                foreach (KeyValuePair<string, object> tEntry in HeaderParams)
                {
                    tDebugRequestHeader += tEntry.Key + " = '" + tEntry.Value + "' , ";//, \n";
                }
                string tDebugResponseHeader = string.Empty;
                foreach (KeyValuePair<string, string> tEntry in Request.GetResponseHeaders())
                {
                    tDebugResponseHeader += tEntry.Key + " = '" + tEntry.Value + "' , ";//, \n";
                }
                NWDDebug.Log("*******************************************************************\n" +
                             "NWDOperationWebUnity UPLOAD VS DOWNLOADED DECODED " + name + "\n" +
                             "-------------------\n" +
                             "<b>Request URl :</b> " + Request.url + "\n" +
                             "-------------------\n" +
                             "<b>Headers UPLOAD :</b> \n" +
                             "-------------------\n" +
                             tDebugRequestHeader + "\n" +
                             "-------------------\n" +
                             "<b>Datas UPLOAD :</b> \n" +
                             "-------------------\n" +
                             Json.Serialize(Data) + "\n" +
                             "-------------------\n\n\n" +
                             "-------------------\n" +
                             "<b>Headers DOWNLOAD :</b> \n" +
                             "-------------------\n" +
                             tDebugResponseHeader + "\n" +
                             "-------------------\n" +
                             "<b>Datas DOWNLOAD : (" + ResultInfos.OctetDownload + ")</b> \n" +
                             "-------------------\n" +
                             tData.Replace("\\\\r", "\r\n") + "\n" +
                             "-------------------\n" +
                             "*******************************************************************\n");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
