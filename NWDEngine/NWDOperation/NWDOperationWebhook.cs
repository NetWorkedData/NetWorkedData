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
    //https://api.slack.com/tools/block-kit-builder?mode=message&blocks=%5B%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22This%20is%20a%20mrkdwn%20section%20block%20%3Aghost%3A%20*this%20is%20bold*%2C%20and%20~this%20is%20crossed%20out~%2C%20and%20%3Chttps%3A%2F%2Fgoogle.com%7Cthis%20is%20a%20link%3E%22%7D%7D%2C%7B%22type%22%3A%22divider%22%7D%2C%7B%22type%22%3A%22context%22%2C%22elements%22%3A%5B%7B%22type%22%3A%22image%22%2C%22image_url%22%3A%22https%3A%2F%2Fapi.slack.com%2Fimg%2Fblocks%2Fbkb_template_images%2FnotificationsWarningIcon.png%22%2C%22alt_text%22%3A%22notifications%20warning%20icon%22%7D%2C%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*Conflicts%20with%20Team%20Huddle%3A%204%3A15-4%3A30pm*%22%7D%5D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*%3CfakeLink.toUserProfiles.com%7CIris%20%2F%20Zelda%201-1%3E*%5CnTuesday%2C%20January%2021%204%3A00-4%3A30pm%5CnBuilding%202%20-%20Havarti%20Cheese%20(3)%5Cn2%20guests%22%7D%2C%22accessory%22%3A%7B%22type%22%3A%22image%22%2C%22image_url%22%3A%22https%3A%2F%2Fapi.slack.com%2Fimg%2Fblocks%2Fbkb_template_images%2Fnotifications.png%22%2C%22alt_text%22%3A%22calendar%20thumbnail%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22emoji%22%3Atrue%2C%22text%22%3A%22Looks%20like%20you%20have%20a%20scheduling%20conflict%20with%20this%20event%3A%22%7D%7D%2C%7B%22type%22%3A%22divider%22%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*Propose%20a%20new%20time%3A*%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*Today%20-%204%3A30-5pm*%5CnEveryone%20is%20available%3A%20%40iris%2C%20%40zelda%22%7D%2C%22accessory%22%3A%7B%22type%22%3A%22button%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22emoji%22%3Atrue%2C%22text%22%3A%22Choose%22%7D%2C%22value%22%3A%22click_me_123%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*Tomorrow%20-%204-4%3A30pm*%5CnEveryone%20is%20available%3A%20%40iris%2C%20%40zelda%22%7D%2C%22accessory%22%3A%7B%22type%22%3A%22button%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22emoji%22%3Atrue%2C%22text%22%3A%22Choose%22%7D%2C%22value%22%3A%22click_me_123%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*Tomorrow%20-%206-6%3A30pm*%5CnSome%20people%20aren%27t%20available%3A%20%40iris%2C%20~%40zelda~%22%7D%2C%22accessory%22%3A%7B%22type%22%3A%22button%22%2C%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22emoji%22%3Atrue%2C%22text%22%3A%22Choose%22%7D%2C%22value%22%3A%22click_me_123%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22*%3Cfakelink.ToMoreTimes.com%7CShow%20more%20times%3E*%22%7D%7D%2C%7B%22type%22%3A%22section%22%2C%22text%22%3A%7B%22type%22%3A%22mrkdwn%22%2C%22text%22%3A%22This%20is%20a%20mrkdwn%20section%20block%20%3Aghost%3A%20*this%20is%20bold*%2C%20and%20~this%20is%20crossed%20out~%2C%20and%20%3Chttps%3A%2F%2Fgoogle.com%7Cthis%20is%20a%20link%3E%22%7D%7D%2C%7B%22type%22%3A%22actions%22%2C%22elements%22%3A%5B%7B%22type%22%3A%22conversations_select%22%2C%22placeholder%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Select%20a%20conversation%22%2C%22emoji%22%3Atrue%7D%7D%2C%7B%22type%22%3A%22channels_select%22%2C%22placeholder%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Select%20a%20channel%22%2C%22emoji%22%3Atrue%7D%7D%2C%7B%22type%22%3A%22users_select%22%2C%22placeholder%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Select%20a%20user%22%2C%22emoji%22%3Atrue%7D%7D%2C%7B%22type%22%3A%22static_select%22%2C%22placeholder%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Select%20an%20item%22%2C%22emoji%22%3Atrue%7D%2C%22options%22%3A%5B%7B%22text%22%3A%7B%22type%22%3A%22plain_text%22%2C%22text%22%3A%22Excellent%20item%201%22%2C%22emoji%22%3Atrue%7D%2C%22value%22%3A%22value-0%22%7D%2C%7B%22text%22%3A%7B%22type%
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [ExecuteInEditMode]
    public partial class NWDOperationWebhook : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_BASE + "Tools/Slack_test", false, 20)]
        //public static void MENU_SlackTest()
        //{
        //    NewWebservie(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void NewWebservie(NWDAppEnvironment sEnvironment)
        {
            NWDOperationWebhook.SlackText("[:robot_face: *NetWorkedData* *" + sEnvironment.AppName +
                "*] New WebServices is available for environment "+ sEnvironment.Environment +
                ". It's <"+ sEnvironment.GetServerHTTPS() + "/" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "/" + sEnvironment.Environment + "/" + "|WS" + NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")+">");
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebhook SlackBlock(string sText)
        {
            Dictionary<string, object> tJson = new Dictionary<string, object>();
            List<Dictionary<string, object>> tBlocks = new List<Dictionary<string, object>>();
            tJson.Add("blocks", tBlocks);

            //Dictionary<string, object> tblockOne = new Dictionary<string, object>();
            //tblockOne.Add("type", "divider");
            //tBlocks.Add(tblockOne);
            Dictionary<string, object> tblockTwo = new Dictionary<string, object>();
            tblockTwo.Add("type", "mrkdwn");
            tblockTwo.Add("text", sText);
            Dictionary<string, object> tblockTwoSection = new Dictionary<string, object>();
            tblockTwoSection.Add("type", "section");
            tblockTwoSection.Add("text", tblockTwo);
            tBlocks.Add(tblockTwoSection);

            string tPayLoad = NWEMiniJSON.Json.Serialize(tJson);
            return NWDOperationWebhook.AddOperation(tPayLoad);
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
            tJson.Add("type", "mrkdwn");
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
            Debug.Log("NWDOperationWebhook Create() with " + sJson);
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
            if (Request.isNetworkError)
            {
                Statut = NWEOperationState.Error;
                Debug.Log("NWDOperationWebhook isNetworkError " + Request.downloadHandler.text);
            }
            else if (Request.isHttpError)
            {
                Statut = NWEOperationState.Error;
                Debug.Log("NWDOperationWebhook isHttpError " + Request.downloadHandler.text);
            }
            else
            {
                Debug.Log("NWDOperationWebhook result " + Request.downloadHandler.text);
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