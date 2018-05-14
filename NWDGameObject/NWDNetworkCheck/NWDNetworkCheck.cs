//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using BasicToolBox;
using UnityEngine;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    ///
    /// </summary>
    public class NWDNetworkCheck : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public int TestEverySeconds = 60;
        public NWDNetworkState NetworkState = NWDNetworkState.Unknow;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            //Debug.Log("NWDNetworkCheck Start()");
            StartCoroutine(CheckUpdate());
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator CheckUpdate()
        {
            while (true)
            {
                //Debug.Log("NWDNetWorkCheck CheckUpdate()");
                PingTest();
                yield return new WaitForSeconds(TestEverySeconds);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PingTest()
        {
            //Debug.Log("NWDNetworkCheck PingTest()");
            // NetworkStatutChange(NWDNetworkState.OnLine);
            Ping tPing = new Ping("8.8.8.8");
            if (tPing.isDone)
            {
                NetworkStatutChange(NWDNetworkState.OnLine);
            }
            else
            {
                NetworkStatutChange(NWDNetworkState.OffLine);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TestNetwork()
        {
            //Debug.Log("NWDNetworkCheck TestNetwork()");
            PingTest();
        }







        ////-------------------------------------------------------------------------------------------------------------
        //public int TestEverySeconds = 60;
        //public NWDNetworkState NetworkState = NWDNetworkState.Unknow;
        //private ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
        //public bool doneTesting = false;
        //public bool probingPublicIP = false;
        //public int serverPort = 443;
        //public bool useNat = false;
        //float timer = 0.0f;
        ////-------------------------------------------------------------------------------------------------------------
        //string testStatus = "Testing network connection capabilities.";
        //string testMessage = "Test in progress";
        //string shouldEnableNatMessage = "";
        ////-------------------------------------------------------------------------------------------------------------
        //void Start()
        //{
        //    Debug.Log("NWDNetworkCheck Start()");
        //    StartCoroutine(CheckUpdate());
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //IEnumerator CheckUpdate()
        //{
        //    while (true)
        //    {
        //        Debug.Log("NWDNetWorkCheck UserNetworkinUpdate()");
        //        TestNetwork();
        //        yield return new WaitForSeconds(TestEverySeconds);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void TestNetwork()
        //{
        //    Debug.Log("NWDNetWorkCheck TestNetwork()");
        //    doneTesting = false;
        //    connectionTestResult = ConnectionTesterStatus.Undetermined;
        //    Network.();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //void OnGUI()
        //{
        //    Debug.Log("NWDNetWorkCheck OnGUI()");

        //    GUI.color = Color.blue;
        //    GUILayout.Label("Current Status: " + testStatus);
        //    GUILayout.Label("Test result : " + testMessage);
        //    GUILayout.Label(shouldEnableNatMessage);

        //    if (!doneTesting)
        //    {
        //        TestConnection();
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        // Test network connection
        public void NetworkStatutChange(NWDNetworkState sNewNetWorkStatut)
        {
            //Debug.Log("NWDNetworkCheck NetworkStatutChange()");
            if (sNewNetWorkStatut != NetworkState)
            {
                NetworkState = sNewNetWorkStatut;
                if (NetworkState == NWDNetworkState.OffLine)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_OFFLINE, null));
                }
                else if (NetworkState == NWDNetworkState.OnLine)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_ONLINE, null));
                }
                else
                {
                    BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_UNKNOW, null));
                }
            }
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public void TestConnection()
        //{
        //    Debug.Log("NWDNetWorkCheck TestConnection()");
        //    // Start/Poll the connection test, report the results in a label and
        //    // react to the results accordingly
        //    connectionTestResult = Network.TestConnection();
        //    switch (connectionTestResult)
        //    {
        //        case ConnectionTesterStatus.Error:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.Error");
        //            testMessage = "Problem determining NAT capabilities";
        //            NetworkStatutChange(NWDNetworkState.OffLine);
        //            doneTesting = true;
        //            break;

        //        case ConnectionTesterStatus.Undetermined:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.Undetermined");
        //            testMessage = "Undetermined NAT capabilities";
        //            NetworkStatutChange(NWDNetworkState.Unknow);
        //            doneTesting = false;
        //            break;

        //        case ConnectionTesterStatus.PublicIPIsConnectable:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.PublicIPIsConnectable");
        //            testMessage = "Directly connectable public IP address.";
        //            NetworkStatutChange(NWDNetworkState.OnLine);
        //            useNat = false;
        //            doneTesting = true;
        //            break;

        //        // This case is a bit special as we now need to check if we can
        //        // circumvent the blocking by using NAT punchthrough
        //        case ConnectionTesterStatus.PublicIPPortBlocked:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.PublicIPPortBlocked");
        //            testMessage = "Non-connectable public IP address (port " + serverPort + " blocked), running a server is impossible.";
        //            NetworkStatutChange(NWDNetworkState.OffLine);
        //            useNat = false;
        //            // If no NAT punchthrough test has been performed on this public
        //            // IP, force a test
        //            if (!probingPublicIP)
        //            {
        //                connectionTestResult = Network.TestConnectionNAT();
        //                probingPublicIP = true;
        //                testStatus = "Testing if blocked public IP can be circumvented";
        //                timer = Time.time + 10;
        //            }
        //            // NAT punchthrough test was performed but we still get blocked
        //            else if (Time.time > timer)
        //            {
        //                probingPublicIP = false;        // reset
        //                useNat = true;
        //                doneTesting = true;
        //            }
        //            break;

        //        case ConnectionTesterStatus.PublicIPNoServerStarted:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.PublicIPNoServerStarted");
        //            testMessage = "Public IP address but server not initialized, " +
        //                "it must be started to check server accessibility. Restart " +
        //                "connection test when ready.";
        //            NetworkStatutChange(NWDNetworkState.OffLine);
        //            break;

        //        case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted");
        //            testMessage = "Limited NAT punchthrough capabilities. Cannot " +
        //                "connect to all types of NAT servers. Running a server " +
        //                "is ill advised as not everyone can connect.";
        //            NetworkStatutChange(NWDNetworkState.OnLine);
        //            useNat = true;
        //            doneTesting = true;
        //            break;

        //        case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.LimitedNATPunchthroughSymmetric");
        //            testMessage = "Limited NAT punchthrough capabilities. Cannot " +
        //                "connect to all types of NAT servers. Running a server " +
        //                "is ill advised as not everyone can connect.";
        //            NetworkStatutChange(NWDNetworkState.OnLine);
        //            useNat = true;
        //            doneTesting = true;
        //            break;

        //        case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
        //        case ConnectionTesterStatus.NATpunchthroughFullCone:
        //            Debug.Log("NWDNetWorkCheck TestConnection() ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone");
        //            testMessage = "NAT punchthrough capable. Can connect to all " +
        //                "servers and receive connections from all clients. Enabling " +
        //                "NAT punchthrough functionality.";
        //            NetworkStatutChange(NWDNetworkState.OnLine);
        //            useNat = true;
        //            doneTesting = true;
        //            break;

        //        default:
        //            testMessage = "Error in test routine, got " + connectionTestResult;
        //            Debug.Log("NWDNetWorkCheck TestConnection() default");
        //            NetworkStatutChange(NWDNetworkState.Unknow);
        //            break;
        //    }

        //    if (doneTesting)
        //    {
        //        if (useNat)
        //        {
        //            shouldEnableNatMessage = "When starting a server the NAT " +
        //                "punchthrough feature should be enabled (useNat parameter)";
        //        }
        //        else
        //        {
        //            shouldEnableNatMessage = "NAT punchthrough not needed";
        //        }
        //        testStatus = "Done testing";
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================