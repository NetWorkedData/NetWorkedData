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
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    public class NWDOperationResult : NWEOperationResult
    {
        //-------------------------------------------------------------------------------------------------------------
        public string errorCode { get; private set; }
        public string token { get; private set; }
        public string uuid { get; private set; }
        public string preview_user { get; private set; }
        public string next_user { get; private set; }
        public string errorInfos { get; private set; }
        public NWDError errorDesc { get; private set; }
        public bool isNewUser { get; private set; }
        public bool isUserTransfert { get; private set; }
        public bool isError { get; private set; }
        public int wsBuild { get; private set; }
        public int timestamp { get; private set; }
        public int avg { get; private set; }
        public float perform { get; private set; }
        public float performRequest { get; private set; }
        public string URL { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> param { get; private set; }
        //-------------------------------------------------------------------------------------------------------------
        //public DateTime PrepareDateTime;
        //public DateTime WebDateTime;
        //public DateTime UploadedDateTime;
        //public DateTime DownloadedDateTime;
        //public DateTime FinishDateTime;
        public NWDWebBenchmark Benchmark = new NWDWebBenchmark();

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
            //NWDBenchmark.Start();
            foreach (KeyValuePair<string, object> k in sData)
            {
                string tKey = k.Key;
                object tValue = k.Value;

                switch (tKey)
                {
                    case NWD.K_JSON_TIMESTAMP_KEY:
                        timestamp = int.Parse(tValue.ToString());
                        break;
                    case NWD.K_JSON_AVG_KEY:
                        avg = NWDToolbox.IntFromString(tValue.ToString());
                        NWDAccountInfos.LoadBalacing(avg);
                        break;
                    case NWD.K_JSON_PERFORM_KEY:
                        perform = float.Parse(tValue.ToString());
                        break;
                    case NWD.K_JSON_PERFORM_REQUEST_KEY:
                        performRequest = float.Parse(tValue.ToString());
                        break;
                    case NWD.K_WEB_REQUEST_TOKEN_KEY:
                        token = tValue.ToString();
                        break;
                    case NWD.K_JSON_ERROR_INFOS_KEY:
                        errorInfos = tValue.ToString();
                        break;
                    case NWD.K_WEB_ACTION_PREVIEW_USER_KEY:
                        preview_user = tValue.ToString();
                        break;
                    case NWD.K_WEB_ACTION_NEXT_USER_KEY:
                        next_user = tValue.ToString();
                        break;
                    case NWD.K_JSON_ERROR_KEY:
                        isError = bool.Parse(tValue.ToString());
                        break;
                    case NWD.K_WEB_ACTION_NEW_USER_KEY:
                        isNewUser = bool.Parse(tValue.ToString());
                        break;
                    case NWD.K_WEB_ACTION_USER_TRANSFERT_KEY:
                        isUserTransfert = bool.Parse(tValue.ToString());
                        break;
                    case NWD.K_JSON_ERROR_CODE_KEY:
                        errorCode = tValue.ToString();
                        break;
                    case NWD.K_WEB_UUID_KEY:
                        uuid = tValue.ToString();
                        break;
                    case NWD.K_JSON_WEB_SERVICE_KEY:
                        wsBuild = int.Parse(tValue.ToString());
                        int tWSBuildEditor = NWDAppConfiguration.SharedInstance().WebBuild;
                        if (wsBuild != tWSBuildEditor)
                        {
                            Debug.LogWarning("wsBuild(" + wsBuild + ") != wsBuildEditor(" + tWSBuildEditor + ")");
                            // TODO : Error if Ws service is not the good version ? perhaps Error is not necessary ?!
                            // imagine ws are conciliant!?
                            // let's go... no error for this moment
                        }
                        break;
                    case NWD.K_WEB_BENCHMARK_Key:
                        //#if UNITY_EDITOR
                        if (NWDAppConfiguration.SharedInstance().SelectedEnvironment().LogMode == NWDEnvironmentLogMode.LogInConsole)
                        {
                            foreach (string tL in tValue as List<object>)
                            {
                                Debug.Log("WS " + NWD.K_WEB_BENCHMARK_Key + " : " + tL.ToString());
                            }
                        }
                        //#endif
                        break;
                    case NWD.K_WEB_LOG_Key:
                        //#if UNITY_EDITOR
                        if (NWDAppConfiguration.SharedInstance().SelectedEnvironment().LogMode == NWDEnvironmentLogMode.LogInConsole)
                        {
                            foreach (string tL in tValue.ToString().Split(new string[] { "\\r" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                Debug.Log("WS " + NWD.K_WEB_LOG_Key + " : " + tL);
                            }
                        }
                        //#endif
                        break;
                    default:
                        Type t = tValue.GetType();
                        bool isDict = t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
                        if (!isDict)
                        {
                            //Debug.LogWarning("Key: " + tKey + " with value: " + tValue + " not parse!");
                        }
                        break;
                }
            }
            //NWDBenchmark.Step();
            if (isError)
            {
                errorDesc = NWDError.FindDataByCode(errorCode) as NWDError;
            }
            //NWDBenchmark.Step();
            param = new Dictionary<string, object>(sData);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetError(NWDError sError)
        {
            isError = true;
            if (sError != null)
            {
                errorDesc = sError;
                errorCode = sError.Code;
            }
            else
            {
                errorDesc = null; //new NWDError();
                errorCode = "WEB00";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Init()
        {
            timestamp = 0;
            avg = -1;
            perform = 0.0f;
            isError = false;
            errorCode = string.Empty;
            //errorDesc = new NWDError();
            token = string.Empty;
            preview_user = string.Empty;
            next_user = string.Empty;
            isNewUser = false;
            isUserTransfert = false;
            uuid = string.Empty;

            param = new Dictionary<string, object>();

            //PrepareDateTime = DateTime.Now;
            //WebDateTime = DateTime.Now;
            //UploadedDateTime = DateTime.Now;
            //DownloadedDateTime = DateTime.Now;
            //FinishDateTime = DateTime.Now;

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
