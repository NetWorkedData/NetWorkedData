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
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNetworkCheckType : int
    {
        UnityHLAPI,
        UnityLLAPI,
        UnityLLAPI_Head,
        Ping,
        Net,
        UnityReacha,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDNetworkFinishDelegate(NWDNetworkState sNetworkState);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNetworkCheck : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public int TestEverySeconds = 300;
        public NWDNetworkState NetworkState = NWDNetworkState.Unknow;
        public bool DebugLog = false;
        public NWDNetworkCheckType RequestType = NWDNetworkCheckType.Ping;
        string URL = string.Empty;
        string AddressPing = string.Empty;
        private bool IsPaused = false;
        UnityWebRequest Request;
        public NWDNetworkFinishDelegate TestFinishedBlock;
        //-------------------------------------------------------------------------------------------------------------
        private IEnumerator CheckCoroutine;
        //-------------------------------------------------------------------------------------------------------------
        private void OnApplicationPause(bool pause)
        {
            //Debug.Log("NWDNetworkCheck OnApplicationPause()");
            IsPaused = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnApplicationFocus(bool focus)
        {
            //Debug.Log("NWDNetworkCheck OnApplicationFocus()");
            IsPaused = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            if (NWDDataManager.SharedInstance().EditorDatabaseLoaded == false)
            {
                NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY, delegate (NWENotification sNotification)
                {
                    NWENotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
                    PrepareUpdate();
                });
            }
            else
            {
                PrepareUpdate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void PrepareUpdate()
        {
            //Debug.Log("NWDNetworkCheck Start()");
            CheckCoroutine = CheckUpdate();
            NetworkStatutChange(NWDNetworkState.Unknow);
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            //URL = tEnvironment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/Environment/" + tEnvironment.Environment + "/index.php";
            URL = tEnvironment.GetServerHTTPS() + "/" + tFolderWebService + "/index.php";
            AddressPing = tEnvironment.AddressPing;
            if (string.IsNullOrEmpty(URL))
            {
                RequestType = NWDNetworkCheckType.Ping;
            }
            if (string.IsNullOrEmpty(AddressPing))
            {
                AddressPing = "8.8.8.8";
            }
            StartCoroutine(CheckCoroutine);
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator CheckUpdate()
        {
            while (true)
            {
                //Debug.Log("NWDNetWorkCheck CheckUpdate()");
                if (IsPaused == false)
                {
                    NetworkTest();
                }
                yield return new WaitForSeconds(TestEverySeconds);
            }
            //yield break;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TestNetwork(NWDNetworkFinishDelegate sNetworkTestFinishedBlock = null)
        {
            //Debug.Log("NWDNetworkCheck TestNetwork()");
            TestFinishedBlock = sNetworkTestFinishedBlock;
            NetworkTest();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void NetworkTest()
        {
            //Debug.Log("NWDNetworkCheck PingTest()");
            NetworkStatutChange(NWDNetworkState.Check);
            switch (RequestType)
            {
                case NWDNetworkCheckType.UnityLLAPI:
                    {
                        StartCoroutine(UnityRequestAsync());
                    }
                    break;
                case NWDNetworkCheckType.UnityHLAPI:
                    {
                        StartCoroutine(UnityRequestAsync());
                    }
                    break;
                case NWDNetworkCheckType.UnityLLAPI_Head:
                    {
                        StartCoroutine(UnityRequestAsync());
                    }
                    break;
                case NWDNetworkCheckType.Ping:
                    {
                        StartCoroutine(PingAsync());
                    }
                    break;
                case NWDNetworkCheckType.Net:
                    {
                        HttpRequestAsync();
                    }
                    break;
                case NWDNetworkCheckType.UnityReacha:
                    {
                        Reach();
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator PingAsync()
        {
            //Debug.Log("NWDNetworkCheck PingAsync()");
            double tStartTimestamp = 0;
            double tFinishTimestamp = 0;
            double tDelta = -1;
            if (DebugLog == true)
            {
                tStartTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            const float timeout = 0.50f;
            float startTime = Time.timeSinceLevelLoad;
            var ping = new Ping(AddressPing);
            while (true)
            {
                if (ping.isDone)
                {
                    if (ping.time < 0)
                    {
                        NetworkStatutChange(NWDNetworkState.OffLine);
                    }
                    else
                    {
                        NetworkStatutChange(NWDNetworkState.OnLine);
                    }
                    if (DebugLog == true)
                    {
                        tFinishTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
                        tDelta = tFinishTimestamp - tStartTimestamp;
                        Debug.Log("NWD => NWDNetworkCheck test " + RequestType.ToString() + " (" + AddressPing + ") " + ping.time + ": " + tDelta.ToString("F3") + " seconds");
                    }
                    if (TestFinishedBlock != null)
                    {
                        TestFinishedBlock(NetworkState);
                        TestFinishedBlock = null;
                    }
                    yield break;
                }
                if (Time.timeSinceLevelLoad - startTime > timeout)
                {
                    NetworkStatutChange(NWDNetworkState.OffLine);

                    if (DebugLog == true)
                    {
                        tFinishTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
                        tDelta = tFinishTimestamp - tStartTimestamp;
                        Debug.Log("NWD => NWDNetworkCheck test " + RequestType.ToString() + " (" + AddressPing + ") : " + tDelta.ToString("F3") + " seconds");
                    }
                    if (TestFinishedBlock != null)
                    {
                        TestFinishedBlock(NetworkState);
                        TestFinishedBlock = null;
                    }
                    yield break;
                }
                //yield return new WaitForEndOfFrame();
                yield return null;
            }
            //yield break;
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator UnityRequestAsync()
        {
            //Debug.Log("NWDNetworkCheck UnityRequestAsync()");
            double tStartTimestamp = 0;
            if (DebugLog == true)
            {
                tStartTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            switch (RequestType)
            {
                case NWDNetworkCheckType.UnityLLAPI:
                    {
                        Request = new UnityWebRequest();
                        Request.url = URL;
                        Request.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided
                        Request.useHttpContinue = false;
                        //Request.chunkedTransfer = false;
                        Request.redirectLimit = 0;  // disable redirects
                        Request.timeout = 10;
                        using (Request)
                        {
                            //Request.timeout = tEnvironment.WebTimeOut;
                            Request.SendWebRequest();
                            while (!Request.isDone)
                            {
                                //yield return new WaitForEndOfFrame();
                                yield return null;
                            }
                            if (Request.isDone == true)
                            {
                                NetworkStatutChange(NWDNetworkState.OnLine);
                                //yield break;
                            }
                            else
                            {
                                NetworkStatutChange(NWDNetworkState.OffLine);
                                //yield break;
                            }
                        }
                    }
                    break;
                case NWDNetworkCheckType.UnityLLAPI_Head:
                    {
                        Request = new UnityWebRequest();
                        Request.url = URL;
                        Request.method = UnityWebRequest.kHttpVerbHEAD;   // can be set to any custom method, common constants privided
                        //Request.chunkedTransfer = false;
                        Request.useHttpContinue = false;
                        Request.redirectLimit = 0;  // disable redirects
                        Request.timeout = 10;
                        using (Request)
                        {
                            //Request.timeout = tEnvironment.WebTimeOut;
                            Request.SendWebRequest();
                            while (!Request.isDone)
                            {
                                //yield return new WaitForEndOfFrame();
                                yield return null;
                            }
                            if (Request.isDone == true)
                            {
                                NetworkStatutChange(NWDNetworkState.OnLine);
                                //yield break;
                            }
                            else
                            {
                                NetworkStatutChange(NWDNetworkState.OffLine);
                                //yield break;
                            }
                        }
                    }
                    break;
                case NWDNetworkCheckType.UnityHLAPI:
                    {

                        using (Request = UnityWebRequest.Get(URL))
                        {
                            //Request.timeout = tEnvironment.WebTimeOut;
                            Request.SendWebRequest();
                            while (!Request.isDone)
                            {
                                //yield return new WaitForEndOfFrame();
                                yield return null;
                            }
                            if (Request.isDone == true)
                            {
                                NetworkStatutChange(NWDNetworkState.OnLine);
                                //yield break;
                            }
                            else
                            {
                                NetworkStatutChange(NWDNetworkState.OffLine);
                                //yield break;
                            }
                        }
                    }
                    break;
                default:
                    //yield return null;
                    break;
            }
            if (DebugLog == true)
            {
                double tFinishTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
                double tDelta = tFinishTimestamp - tStartTimestamp;
                Debug.Log("NWD => NWDNetworkCheck test " + RequestType.ToString() + " (" + URL + ") : " + tDelta.ToString("F3") + " seconds");
            }
            if (TestFinishedBlock != null)
            {
                TestFinishedBlock(NetworkState);
                TestFinishedBlock = null;
            }
            yield break;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Reach()
        {
            //Debug.Log("NWDNetworkCheck Reach()");
            //ThreadPool.QueueUserWorkItem(new WaitCallback(MakeRequest));
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                NetworkStatutChange(NWDNetworkState.OffLine);
            }
            //Check if the device can reach the internet via a carrier data network
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                NetworkStatutChange(NWDNetworkState.OnLine);
            }
            //Check if the device can reach the internet via a LAN
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                NetworkStatutChange(NWDNetworkState.OnLine);
            }
            if (TestFinishedBlock != null)
            {
                TestFinishedBlock(NetworkState);
                TestFinishedBlock = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void HttpRequestAsync()
        {
            //Debug.Log("NWDNetworkCheck HttpRequestAsync()");
            //ThreadPool.QueueUserWorkItem(new WaitCallback(MakeRequest));
            MakeRequest(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void MakeRequest(object a)
        {
            double tStartTimestamp = 0;
            double tFinishTimestamp = 0;
            double tDelta = -1;
            if (DebugLog == true)
            {
                tStartTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            var tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "HEAD";
            HttpWebResponse tResponse = null;
            NWDNetworkState tNetworkState = NWDNetworkState.Unknow;
            try
            {
                tResponse = (HttpWebResponse)tRequest.GetResponse();
            }
            catch (WebException e)
            {
                Debug.LogException(e);
                //Debug.Log(URL + " doesn't exist: " + e.Message);
                tNetworkState = NWDNetworkState.OffLine;
            }
            finally
            {
                if (tResponse != null)
                {
                    if (tResponse.StatusCode == HttpStatusCode.OK)
                    {
                        tNetworkState = NWDNetworkState.OnLine;
                    }
                    else
                    {
                        tNetworkState = NWDNetworkState.OffLine;
                    }
                    tResponse.Close();
                }
            }
            NetworkStatutChange(tNetworkState);
            if (DebugLog == true)
            {
                tFinishTimestamp = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
                tDelta = tFinishTimestamp - tStartTimestamp;
                Debug.Log("NWD => NWDNetworkCheck test " + RequestType.ToString() + " (" + URL + ") : " + tDelta.ToString("F3") + " seconds");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Test network connection
        public void NetworkStatutChange(NWDNetworkState sNewNetWorkStatut)
        {
            //Debug.Log("NWDNetworkCheck NetworkStatutChange()");
            if (sNewNetWorkStatut != NetworkState)
            {
                NetworkState = sNewNetWorkStatut;
                switch (NetworkState)
                {
                    case NWDNetworkState.Check:
                        {
                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_CHECK, null));
                        }
                        break;
                    case NWDNetworkState.OnLine:
                        {
                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_ONLINE, null));
                        }
                        break;
                    case NWDNetworkState.OffLine:
                        {
                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_OFFLINE, null));
                        }
                        break;
                    case NWDNetworkState.Unknow:
                    default:
                        {
                            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_UNKNOW, null));
                        }
                        break;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
