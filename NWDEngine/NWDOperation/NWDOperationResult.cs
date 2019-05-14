// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:35
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using BasicToolBox;
using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    public class NWDOperationResult : BTBOperationResult
    {
        //-------------------------------------------------------------------------------------------------------------
        public int timestamp { get; private set; }
        public float perform { get; private set; }
        public float performRequest { get; private set; }
        public bool isError { get; private set; }
        public string errorCode { get; private set; }
        public NWDError errorDesc { get; private set; }
        public string token { get; private set; }
        //public NWDAppEnvironmentPlayerStatut sign { get; private set; }
        public bool isSignUpdate { get; private set; }
        public string uuid { get; private set; }
        public bool isSignIn { get; private set; }
        public bool isSignOut { get; private set; }
        public bool isSignUp { get; private set; }
        //public bool isGoogleSignIn
        //{
        //    get; private set;
        //}
        //public bool isGoogleSignUp
        //{
        //    get; private set;
        //}
        //public bool isFacebookSignIn
        //{
        //    get; private set;
        //}
        //public bool isFacebookSignUp
        //{
        //    get; private set;
        //}
        public bool isCreateAnonymous { get; private set; }
        //public string signkey { get; private set; }
        public bool isNewUser { get; private set; }
        public bool isUserTransfert { get; private set; }
        public bool isReloadingData { get; private set; }
        public int wsBuild { get; private set; }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> param { get; private set; }
        //-------------------------------------------------------------------------------------------------------------
        public DateTime PrepareDateTime = DateTime.Now;
        public DateTime WebDateTime = DateTime.Now;
        public DateTime UploadedDateTime = DateTime.Now;
        public DateTime DownloadedDateTime = DateTime.Now;
        public DateTime FinishDateTime = DateTime.Now;
        public double OctetUpload = 0.0F;
        public double OctetDownload = 0.0F;
        public int ClassPullCounter = 0;
        public int ClassPushCounter = 0;
        public int RowPullCounter = 0;
        public int RowUpdatedCounter = 0;
        public int RowAddedCounter = 0;
        public int RowPushCounter = 0;
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationResult()
        {
            Init();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetData(Dictionary<string, object> sData)
        {
            if (sData.ContainsKey("timestamp"))
            {
                timestamp = int.Parse(sData["timestamp"].ToString());
            }
            if (sData.ContainsKey("perform"))
            {
                perform = float.Parse(sData["perform"].ToString());
            }
            if (sData.ContainsKey("performRequest"))
            {
                performRequest = float.Parse(sData["performRequest"].ToString());
            }
            if (sData.ContainsKey(NWD.RequestTokenKey))
            {
                token = sData[NWD.RequestTokenKey] as string;
            }
            if (sData.ContainsKey("signin"))
            {
                isSignIn = (bool)sData["signin"];
            }
            if (sData.ContainsKey("signout"))
            {
                isSignOut = (bool)sData["signout"];
            }
            if (sData.ContainsKey("signup"))
            {
                isSignUp = (bool)sData["signup"];
            }
            //if (data.ContainsKey("google_signin"))
            //{
            //    isGoogleSignIn = (bool)data["google_signin"];
            //}
            //if (data.ContainsKey("google_signup"))
            //{
            //    isGoogleSignUp = (bool)data["google_signup"];
            //}
            //if (data.ContainsKey("facebook_signin"))
            //{
            //    isFacebookSignIn = (bool)data["facebook_signin"];
            //}
            //if (data.ContainsKey("facebook_signup"))
            //{
            //    isFacebookSignUp = (bool)data["facebook_signup"];
            //}
            if (sData.ContainsKey("create-anonymous"))
            {
                isCreateAnonymous = (bool)sData["create-anonymous"];
            }
            if (sData.ContainsKey("sign"))
            {
                isSignUpdate = true;
                try
                {
                    //sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), sData["sign"].ToString(), true);
                }
                catch (ArgumentException e)
                {
                    Debug.Log(e.StackTrace);
                }
            }
            //if (sData.ContainsKey("signkey"))
            //{
            //    signkey = sData["signkey"] as string;
            //}
            if (sData.ContainsKey("error"))
            {
                isError = (bool)sData["error"];
            }
            if (sData.ContainsKey("newuser"))
            {
                isNewUser = (bool)sData["newuser"];
            }
            if (sData.ContainsKey("usertransfert"))
            {
                isUserTransfert = (bool)sData["usertransfert"];
            }
            if (sData.ContainsKey("reloaddatas"))
            {
                isReloadingData = (bool)sData["reloaddatas"];
            }
            if (sData.ContainsKey("error_code"))
            {
                errorCode = sData["error_code"] as string;
            }
            if (sData.ContainsKey(NWD.UUIDKey))
            {
                uuid = sData[NWD.UUIDKey] as string;
            }
            if (sData.ContainsKey("wsbuild"))
            {
                wsBuild = int.Parse(sData["wsbuild"].ToString());
                //int tWSBuildEditor = BTBConfigManager.SharedInstance().GetInt(NWDConstants.K_NWD_WS_BUILD);
                int tWSBuildEditor = NWDAppConfiguration.SharedInstance().WebBuild;
                if (wsBuild != tWSBuildEditor)
                {
                    // TODO : Error if Ws service is not the good version ? perhaps Error is not necessary ?!
                    // imagine ws are conciliant!?
                    // let's go... no error for this moment
                }
            }

            if (isError)
            {
                errorDesc = NWDError.FindDataByCode(errorCode) as NWDError;
                // Move to the good place
                //if (errorDesc != null)
                //{
                //    errorDesc.ShowAlert();
                //}
            }

            param = new Dictionary<string, object>(sData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetErrorCode(string sCode)
        {
            errorDesc = NWDError.FindDataByCode(sCode) as NWDError;
            isError = true;
            errorCode = sCode;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Init()
        {
            timestamp = 0;
            perform = 0.0f;
            isError = false;
            errorCode = string.Empty;
            token = string.Empty;
            isSignIn = false;
            isSignOut = false;
            //isGoogleSignIn = false;
            //isGoogleSignUp = false;
            //isFacebookSignIn = false;
            //isFacebookSignUp = false;
            isCreateAnonymous = false;
            //signkey = string.Empty;
            //sign = NWDAppEnvironmentPlayerStatut.Unknow;
            isSignUpdate = false;
            isNewUser = false;
            isUserTransfert = false;
            isReloadingData = false;
            uuid = string.Empty;

            param = new Dictionary<string, object>();

            PrepareDateTime = DateTime.Now;
            WebDateTime = DateTime.Now;
            UploadedDateTime = DateTime.Now;
            DownloadedDateTime = DateTime.Now;
            FinishDateTime = DateTime.Now;
            OctetUpload = 0.0f;
            OctetDownload = 0.0f;
            ClassPullCounter = 0;
            ClassPushCounter = 0;
            RowPullCounter = 0;
            RowPushCounter = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================