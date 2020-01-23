//=====================================================================================================================
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
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    public class NWDOperationResult : NWEOperationResult
    {
        //-------------------------------------------------------------------------------------------------------------
        public int timestamp { get; private set; }
        public int avg { get; private set; }
        public float perform { get; private set; }
        public float performRequest { get; private set; }
        public bool isError { get; private set; }
        public string errorCode { get; private set; }
        public string errorInfos { get; private set; }
        public NWDError errorDesc { get; private set; }
        public string token { get; private set; }
        public string uuid { get; private set; }
        public string preview_user { get; private set; }
        public string next_user { get; private set; }
        //public bool isSignIn { get; private set; }
        //public bool isSignOut { get; private set; }
        //public bool isRescue { get; private set; }
        public bool isNewUser { get; private set; }
        public bool isUserTransfert { get; private set; }
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
            if (sData.ContainsKey(NWD.K_JSON_TIMESTAMP_KEY))
            {
                timestamp = int.Parse(sData[NWD.K_JSON_TIMESTAMP_KEY].ToString());
            }
            if (sData.ContainsKey(NWD.K_JSON_AVG_KEY))
            {
                avg = NWDToolbox.IntFromString(sData[NWD.K_JSON_AVG_KEY].ToString());
                NWDAccountInfos.LoadBalacing(avg);
            }
            if (sData.ContainsKey(NWD.K_JSON_PERFORM_KEY))
            {
                perform = float.Parse(sData[NWD.K_JSON_PERFORM_KEY].ToString());
            }
            if (sData.ContainsKey(NWD.K_JSON_PERFORM_REQUEST_KEY))
            {
                performRequest = float.Parse(sData[NWD.K_JSON_PERFORM_REQUEST_KEY].ToString());
            }
            if (sData.ContainsKey(NWD.RequestTokenKey))
            {
                token = sData[NWD.RequestTokenKey] as string;
            }
            //if (sData.ContainsKey(NWD.K_WEB_ACTION_SIGNIN_KEY))
            //{
            //    isSignIn = (bool)sData[NWD.K_WEB_ACTION_SIGNIN_KEY];
            //}
            //if (sData.ContainsKey(NWD.K_WEB_ACTION_SIGNOUT_KEY))
            //{
            //    isSignOut = (bool)sData[NWD.K_WEB_ACTION_SIGNOUT_KEY];
            //}
            //if (sData.ContainsKey(NWD.K_WEB_ACTION_RESCUE_KEY))
            //{
            //    isRescue = (bool)sData[NWD.K_WEB_ACTION_RESCUE_KEY];
            //}
            if (sData.ContainsKey(NWD.K_JSON_ERROR_INFOS_KEY))
            {
                errorInfos = sData[NWD.K_JSON_ERROR_INFOS_KEY] as string;
            }
            if (sData.ContainsKey(NWD.K_WEB_ACTION_PREVIEW_USER_KEY))
            {
                preview_user = sData[NWD.K_WEB_ACTION_PREVIEW_USER_KEY] as string;
            }
            if (sData.ContainsKey(NWD.K_WEB_ACTION_NEXT_USER_KEY))
            {
                next_user = sData[NWD.K_WEB_ACTION_NEXT_USER_KEY] as string;
            }
            if (sData.ContainsKey(NWD.K_JSON_ERROR_KEY))
            {
                isError = (bool)sData[NWD.K_JSON_ERROR_KEY];
            }
            if (sData.ContainsKey(NWD.K_WEB_ACTION_NEW_USER_KEY))
            {
                isNewUser = (bool)sData[NWD.K_WEB_ACTION_NEW_USER_KEY];
            }
            if (sData.ContainsKey(NWD.K_WEB_ACTION_USER_TRANSFERT_KEY))
            {
                isUserTransfert = (bool)sData[NWD.K_WEB_ACTION_USER_TRANSFERT_KEY];
            }
            if (sData.ContainsKey(NWD.K_JSON_ERROR_CODE_KEY))
            {
                errorCode = sData[NWD.K_JSON_ERROR_CODE_KEY] as string;
            }
            if (sData.ContainsKey(NWD.UUIDKey))
            {
                uuid = sData[NWD.UUIDKey] as string;
            }
            if (sData.ContainsKey(NWD.K_JSON_WEB_SERVICE_KEY))
            {
                wsBuild = int.Parse(sData[NWD.K_JSON_WEB_SERVICE_KEY].ToString());
                //int tWSBuildEditor = NWEConfigManager.SharedInstance().GetInt(NWDConstants.K_NWD_WS_BUILD);
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
            }

            param = new Dictionary<string, object>(sData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetError(NWDError sError)
        {
            errorDesc = sError;
            isError = true;
            errorCode = sError.Code;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Init()
        {
            timestamp = 0;
            avg = -1;
            perform = 0.0f;
            isError = false;
            errorCode = string.Empty;
            token = string.Empty;
            preview_user = string.Empty;
            next_user = string.Empty;
            //isSignIn = false;
            //isSignOut = false;
            isNewUser = false;
            isUserTransfert = false;
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