//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using BasicToolBox;
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
            //Debug.Log("NWDNetworkCheck Start()");
            CheckCoroutine = CheckUpdate();
            NetworkStatutChange(NWDNetworkState.Unknow);
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            //URL = tEnvironment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/Environment/" + tEnvironment.Environment + "/index.php";
            URL = tEnvironment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/index.php";
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
                tStartTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            const float timeout = 0.50f;
            float startTime = Time.timeSinceLevelLoad;
            var ping = new Ping(AddressPing);
            while (true)
            {
                if (ping.isDone)
                {
                    if (ping.time<0)
                    {
                        NetworkStatutChange(NWDNetworkState.OffLine);
                    }
                    else
                    {
                        NetworkStatutChange(NWDNetworkState.OnLine);
                    }
                    if (DebugLog == true)
                    {
                        tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
                        tDelta = tFinishTimestamp - tStartTimestamp;
                        Debug.Log("NWD => NWDNetworkCheck test " + RequestType.ToString() + " ("+AddressPing+") "+ ping.time + ": " + tDelta.ToString("F3") + " seconds");
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
                        tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
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
                tStartTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            switch (RequestType)
            {
                case NWDNetworkCheckType.UnityLLAPI:
                    {
                        Request = new UnityWebRequest();
                        Request.url = URL;
                        Request.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided
                        Request.useHttpContinue = false;
                        Request.chunkedTransfer = false;
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
                        Request.useHttpContinue = false;
                        Request.chunkedTransfer = false;
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
                double tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
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
                tStartTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            }
            var tRequest = (HttpWebRequest)WebRequest.Create(URL);
            tRequest.Method = "HEAD";
            HttpWebResponse tResponse = null;
            NWDNetworkState tNetworkState = NWDNetworkState.Unknow;
            try
            {
                tResponse = (HttpWebResponse)tRequest.GetResponse();
            }
            catch (WebException tWebException)
            {
                Debug.Log(URL + " doesn't exist: " + tWebException.Message);
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
                tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
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
                    case NWDNetworkState.Check :
                        {
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_CHECK, null));
                        }
                        break;
                    case NWDNetworkState.OnLine:
                        {
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_ONLINE, null));
                        }
                        break;
                    case NWDNetworkState.OffLine:
                        {
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_OFFLINE, null));
                        }
                        break;
                    case NWDNetworkState.Unknow:
                    default :
                        {
                            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_UNKNOW, null));
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