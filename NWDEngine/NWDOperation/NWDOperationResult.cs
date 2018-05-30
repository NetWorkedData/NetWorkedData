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
        public int timestamp
        {
            get; private set;
        }
        public float perform
        {
            get; private set;
        }
        public float performRequest
        {
            get; private set;
        }
        public bool isError
        {
            get; private set;
        }
        public string errorCode
        {
            get; private set;
        }
        public NWDError errorDesc
        {
            get; private set;
        }
        public string token
        {
            get; private set;
        }
        public NWDAppEnvironmentPlayerStatut sign
        {
            get; private set;
        }
        public bool isSignUpdate
        {
            get; private set;
        }
        public string uuid
        {
            get; private set;
        }
        public bool isSignIn
        {
            get; private set;
        }
        public bool isSignOut
        {
            get; private set;
        }
        public bool isSignUp
        {
            get; private set;
        }
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
        public bool isCreateAnonymous
        {
            get; private set;
        }
        public string signkey
        {
            get; private set;
        }
        public bool isNewUser
        {
            get; private set;
        }
        public bool isUserTransfert
        {
            get; private set;
        }
        public bool isReloadingData
        {
            get; private set;
        }
        public int wsBuild
        {
            get; private set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> param
        {
            get; private set;
        }
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
        public void SetData(Dictionary<string, object> data)
        {
            //Init();

            if (data.ContainsKey("timestamp"))
            {
                timestamp = int.Parse(data["timestamp"].ToString());
            }
            if (data.ContainsKey("perform"))
            {
                perform = float.Parse(data["perform"].ToString());
            }
            if (data.ContainsKey("performRequest"))
            {
                performRequest = float.Parse(data["performRequest"].ToString());
            }
            if (data.ContainsKey("token"))
            {
                token = data["token"] as string;
            }
            if (data.ContainsKey("signin"))
            {
                isSignIn = (bool)data["signin"];
            }
            if (data.ContainsKey("signout"))
            {
                isSignOut = (bool)data["signout"];
            }
            if (data.ContainsKey("signup"))
            {
                isSignUp = (bool)data["signup"];
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
            if (data.ContainsKey("create-anonymous"))
            {
                isCreateAnonymous = (bool)data["create-anonymous"];
            }
            if (data.ContainsKey("sign"))
            {
                isSignUpdate = true;
                try
                {
                    sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), data["sign"].ToString(), true);
                }
                catch (ArgumentException e)
                {
                    Debug.Log(e.StackTrace);
                }
            }
            if (data.ContainsKey("signkey"))
            {
                signkey = data["signkey"] as string;
            }
            if (data.ContainsKey("error"))
            {
                isError = (bool)data["error"];
            }
            if (data.ContainsKey("newuser"))
            {
                isNewUser = (bool)data["newuser"];
            }
            if (data.ContainsKey("usertransfert"))
            {
                isUserTransfert = (bool)data["usertransfert"];
            }
            if (data.ContainsKey("reloaddatas"))
            {
                isReloadingData = (bool)data["reloaddatas"];
            }
            if (data.ContainsKey("error_code"))
            {
                errorCode = data["error_code"] as string;
            }
            if (data.ContainsKey("uuid"))
            {
                uuid = data["uuid"] as string;
            }
            if (data.ContainsKey("wsbuild"))
            {
                wsBuild = int.Parse(data["wsbuild"].ToString());
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
                errorDesc = NWDError.GetErrorWithCode(errorCode) as NWDError;
            }

            param = new Dictionary<string, object>(data);
        }
        //-------------------------------------------------------------------------------------------------------------
        //     public NWDOperationResult(string sCode)
        //     {
        //         Init();
        //errorDesc = NWDError.GetErrorWithCode(sCode) as NWDError;
        //         isError = true;
        //errorCode = sCode;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void SetErrorCode(string sCode)
        {
            errorDesc = NWDError.GetErrorWithCode(sCode) as NWDError;
            isError = true;
            errorCode = sCode;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Init()
        {
            timestamp = 0;
            perform = 0.0f;
            isError = false;
            errorCode = "";
            token = "";
            isSignIn = false;
            isSignOut = false;
            //isGoogleSignIn = false;
            //isGoogleSignUp = false;
            //isFacebookSignIn = false;
            //isFacebookSignUp = false;
            isCreateAnonymous = false;
            signkey = "";
            sign = NWDAppEnvironmentPlayerStatut.Unknow;
            isSignUpdate = false;
            isNewUser = false;
            isUserTransfert = false;
            isReloadingData = false;
            uuid = "";

            param = new Dictionary<string, object>();

            PrepareDateTime = DateTime.Now;
            WebDateTime = DateTime.Now;
            UploadedDateTime = DateTime.Now;
            DownloadedDateTime = DateTime.Now;
            FinishDateTime = DateTime.Now;
            OctetUpload = 0.0F;
            OctetDownload = 0.0F;
            ClassPullCounter = 0;
            ClassPushCounter = 0;
            RowPullCounter = 0;
            RowPushCounter = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================