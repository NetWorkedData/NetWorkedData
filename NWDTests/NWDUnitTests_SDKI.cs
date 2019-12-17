//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;
using NetWorkedData;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_INCLUDE_TESTS
        //-------------------------------------------------------------------------------------------------------------
        public string GetWithSpecialSDKI()
        {
            return WithSpecialSDKI;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_INCLUDE_TESTS
        //-------------------------------------------------------------------------------------------------------------
        static string DevicePlayerHash;
        static string DeviceEditorHash;
        //-------------------------------------------------------------------------------------------------------------
        public static void DevicePlayerReset()
        {
            DevicePlayerHash = "PlaY3r-" + NWDToolbox.RandomStringUnix(NWDAppEnvironment.kSecretKeyDevicePlayerLength);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetDevicePlayer()
        {
            if (string.IsNullOrEmpty(DevicePlayerHash))
            {
                DevicePlayerReset();
            }
            return DevicePlayerHash;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeviceEditorReset()
        {
            DeviceEditorHash = "ediT0R-" + NWDToolbox.RandomStringUnix(NWDAppEnvironment.kSecretKeyDevicePlayerLength);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetDeviceEditor()
        {
            if (string.IsNullOrEmpty(DeviceEditorHash))
            {
                DeviceEditorReset();
            }
            return DeviceEditorHash;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetDevice()
        {
            Debug.Log("@@@@@@@@@@@@@@@@@ ResetDevice() @@@@@@@@@@@");
            DevicePlayerReset();
            DeviceEditorReset();
            ShowDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDevice()
        {
            Debug.Log("@@@@@@@@@@@@@@@@@ ShowDevice() @@@@@@@@@@@@");
            Debug.Log("@@@ Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            Debug.Log("@@@ WithSpecialSDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetWithSpecialSDKI());
            Debug.Log("@@@ SecretKeyDevice SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevice());
            Debug.Log("@@@ SecretKeyDeviceEditor SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Debug.Log("@@@ SecretKeyDevicePlayer SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void TemporaryAccount()
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
            Debug.Log("@@@@@@@@@@@@@@@@@ TemporaryAccount() @@@@@@");
            Debug.Log("@@@ Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            ShowDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================