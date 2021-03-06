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
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum WebHookType : int
    {
        Ugrade,
        Sync,
        Notification,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[ExecuteInEditMode]
    //public partial class NWDOperationWebhookSync : NWDOperationWebhook
    //{
    //    //-------------------------------------------------------------------------------------------------------------
    //    public static new string WebHookURL()
    //    {
    //        Debug.Log("NWDOperationWebhookSync WebHookURL()");
    //        return NWDAppConfiguration.SharedInstance().SlackWebhookURLSync;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //}
    ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[ExecuteInEditMode]
    //public partial class NWDOperationWebhookWS : NWDOperationWebhook
    //{
    //    //-------------------------------------------------------------------------------------------------------------
    //    public static new string WebHookURL()
    //    {
    //        Debug.Log("NWDOperationWebhookWS WebHookURL()");
    //        return NWDAppConfiguration.SharedInstance().SlackWebhookURL;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------î
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [ExecuteInEditMode]
    public partial class NWDOperationWebhook : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static int kTimeOutOfRequest = 300;
        public GameObject GameObjectToSpawn;
        public UnityWebRequest Request;
        public NWDAppEnvironment Environment;
        public string URL;
        public string Json;
        //-------------------------------------------------------------------------------------------------------------
        public static void NewMessage(string sMessage, WebHookType sWebHookType)
        {
            NewMessage(NWDAppConfiguration.SharedInstance().SelectedEnvironment(), sMessage, sWebHookType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NewEditorOperation(NWDAppEnvironment sEnvironment, string sMessage, List<Type> sTypeList, WebHookType sWebHookType)
        {
            List<string> tlistName = new List<string>();
            foreach (Type tType in sTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tlistName.Add(tHelper.ClassNamePHP);
            }
            string tMessage = sMessage + " " + string.Join(", ", tlistName);
            NewMessage(sEnvironment, tMessage, sWebHookType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NewMessage(NWDAppEnvironment sEnvironment, string sMessage, WebHookType sWebHookType)
        {
            string tWarning = "";
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tWarning = ":warning: ";
            }
            SlackText("[:robot_face: *NetWorkedData* :package: *" + sEnvironment.AppName + "* :grinning: " + NWDProjectConfigurationManagerContent.SharedInstance().UserName + " (" + SystemInfo.deviceUniqueIdentifier + ")] " +
                "\n" +
                tWarning +
                // "<color=" + NWDToolbox.ColorToString(sEnvironment.CartridgeColor) + "> " +
                "*" + sEnvironment.Environment + "*" +
                // "</color>" +
                " : " +
                //"\n" +
                "" + sMessage, sWebHookType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NewWebService(NWDAppEnvironment sEnvironment, List<Type> sTypeList)
        {
            string tClasses = "";
            if (sTypeList.Count == 1)
            {
                tClasses = tClasses + " Classe updated is :";
            }
            if (sTypeList.Count > 1)
            {
                tClasses = tClasses + " Classes updated are :";
            }
            foreach (Type tType in sTypeList)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                tClasses = tClasses + "\n - " + tDatas.ClassNamePHP;
            }
            if (sTypeList.Count > 0)
            {
                tClasses = tClasses + "\n";
            }
            string tMessage = "New WebServices are available for environment *" + sEnvironment.Environment +
                "*. WebServices are available at <" + sEnvironment.GetServerHTTPS() + "/" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "/" + sEnvironment.Environment + "/" + "|WS" + NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000") + "> !" + tClasses;
            NewMessage(sEnvironment, tMessage, WebHookType.Ugrade);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook SlackBlock(string sText, WebHookType sWebHookType)
        {
            Dictionary<string, object> tJson = new Dictionary<string, object>();
            List<Dictionary<string, object>> tBlocks = new List<Dictionary<string, object>>();
            tJson.Add("blocks", tBlocks);
            Dictionary<string, object> tblockTwo = new Dictionary<string, object>();
            tblockTwo.Add("type", "mrkdwn");
            tblockTwo.Add("text", sText);
            Dictionary<string, object> tblockTwoSection = new Dictionary<string, object>();
            tblockTwoSection.Add("type", "section");
            tblockTwoSection.Add("text", tblockTwo);
            tBlocks.Add(tblockTwoSection);

            string tPayLoad = NWEMiniJSON.Json.Serialize(tJson);
            return AddOperation(tPayLoad, sWebHookType);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook SlackText(string sText, WebHookType sWebHookType)
        {
            Dictionary<string, string> tJson = new Dictionary<string, string>();
            tJson.Add("type", "mrkdwn");
            tJson.Add("text", sText);
            string tPayLoad = NWEMiniJSON.Json.Serialize(tJson);
            return AddOperation(tPayLoad, sWebHookType);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook AddOperation(string sJson, WebHookType sWebHookType)
        {
            NWDOperationWebhook rReturn = Create(sJson, sWebHookType);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, true);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string WebHookURL(WebHookType sWebHookType)
        {
            string tReturn = string.Empty;
            switch (sWebHookType)
            {
                case WebHookType.Ugrade:
                    {
                        tReturn = NWDAppConfiguration.SharedInstance().SlackWebhookURLUpgrade;
                    }
                    break;
                case WebHookType.Sync:
                    {
                        tReturn = NWDAppConfiguration.SharedInstance().SlackWebhookURLSync;
                    }
                    break;
                case WebHookType.Notification:
                    {
                        tReturn = NWDAppConfiguration.SharedInstance().SlackWebhookURLNotification;
                    }
                    break;
            }
            Debug.Log(" tReturn = " + tReturn);
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook Create(string sJson, WebHookType sWebHookType)
        {
            NWDOperationWebhook rReturn = null;
            if (string.IsNullOrEmpty(WebHookURL(sWebHookType)) == false)
            {
                GameObject tGameObjectToSpawn = new GameObject("webHook");
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebhook>();
                rReturn.Environment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.URL = WebHookURL(sWebHookType);
                rReturn.Json = sJson;
                Debug.Log(rReturn.GetType().Name + " Create() with " + sJson);
            }
            else
            {
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
            //Debug.Log("NWDOperationWebhook Execute()");
            StartCoroutine(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator ExecuteAsync()
        {
            //Debug.Log("NWDOperationWebhook ExecuteAsync()");
            Statut = NWEOperationState.Start;
            //Operation progress
            Statut = NWEOperationState.InProgress;
            //float tStart = Time.time;
            // Send this operation in actual operation for this environment
            Parent.Controller[QueueName].ActualOperation = this;
            // Force all datas to be write in database
            NWDAppEnvironment.SetEnvironment(Environment);
            var Request = new UnityWebRequest(URL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(Json);
            Request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.downloadHandler = new DownloadHandlerBuffer();
            Request.SendWebRequest();
            while (!Request.isDone)
            {
                Statut = NWEOperationState.InProgress;
                if (Request.uploadProgress < 1.0f)
                {
                    // Notification of an Upload in progress
                }
                if (Request.downloadProgress < 1.0f)
                {
                    // Notification of an Download in progress
                }
                yield return null;
            }
#if UNITY_2019
            if (Request.isNetworkError)
#elif UNITY_2020
            if (Request.result == UnityWebRequest.Result.ConnectionError) //obsolete Request.isNetworkError
#endif
            {
                Statut = NWEOperationState.Error;
                Debug.Log("NWDOperationWebhook isNetworkError " + Request.downloadHandler.text);
            }
#if UNITY_2019
            else if (Request.isHttpError)
#elif UNITY_2020
            else if (Request.result == UnityWebRequest.Result.ProtocolError) //obsolete Request.isHttpError
#endif
            {
                Statut = NWEOperationState.Error;
                Debug.Log("NWDOperationWebhook isHttpError " + Request.downloadHandler.text);
            }
            else
            {
                //Debug.Log("NWDOperationWebhook result " + Request.downloadHandler.text);
            }
            Finish();
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            //Debug.Log("NWDOperationWebhook Cancel()");
            Statut = NWEOperationState.Cancel;
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
            //Debug.Log("NWDOperationWebhook Finish()");
            if (Statut == NWEOperationState.ReStart)
            {
                // I MUST RESTART THE REQUEST BECAUSE BEFORE I WAS TEMPORARY ACCOUNT
                Parent.ReplayOperation(QueueName);
            }
            else
            {
                Statut = NWEOperationState.Finish;
                IsFinish = true;
                Parent.NextOperation(QueueName);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DestroyThisOperation()
        {
            //Debug.Log("NWDOperationWebhook DestroyThisOperation()");
            Statut = NWEOperationState.Destroy;
#if UNITY_EDITOR
            DestroyImmediate(GameObjectToSpawn);
#else
            Destroy (GameObjectToSpawn);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
