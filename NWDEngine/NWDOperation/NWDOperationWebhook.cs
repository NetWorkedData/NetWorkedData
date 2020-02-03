//=====================================================================================================================
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
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
//using BasicToolBox;
using NWEMiniJSON;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [ExecuteInEditMode]
    public partial class NWDOperationWebhook : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Tools/Slack_test", false, 20)]
        public static void MENU_SlackTest()
        {
            NWDOperationWebhook.SlackText("Yahoooooooooo");
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static int kTimeOutOfRequest = 300;
        public GameObject GameObjectToSpawn;
        public UnityWebRequest Request;
        public NWDAppEnvironment Environment;
        public string URL;
        public string Json;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook SlackText(string sText)
        {
            Dictionary<string, string> tJson = new Dictionary<string, string>();
            tJson.Add("text", sText);
            string tPayLoad = NWEMiniJSON.Json.Serialize(tJson);
            return NWDOperationWebhook.AddOperation(tPayLoad);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook AddOperation(string sJson)
        {
            NWDOperationWebhook rReturn = NWDOperationWebhook.Create(sJson);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, true);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook Create(string sJson)
        {
            Debug.Log("NWDOperationWebhook Create()");
            NWDOperationWebhook rReturn = null;
            if (string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().SlackWebhookURL) == false)
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
                rReturn.URL = NWDAppConfiguration.SharedInstance().SlackWebhookURL;
                rReturn.Json = sJson;
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
//            NWDDataManager.SharedInstance().DataQueueExecute();
//#if UNITY_EDITOR
//            // Deselect all object
//            Selection.activeObject = null;
//#endif
            NWDAppEnvironment.SetEnvironment(Environment);
            var Request = new UnityWebRequest(URL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(Json);
            Request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            Request.SetRequestHeader("Content-Type", "application/json");
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
                if (Request.isNetworkError)
                {
                    Statut = NWEOperationState.Error;
                }
                else if (Request.isHttpError)
                {
                    Statut = NWEOperationState.Error;
                }
                else
                {
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