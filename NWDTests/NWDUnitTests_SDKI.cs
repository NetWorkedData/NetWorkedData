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

#if UNITY_INCLUDE_TESTS
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public string GetWithSpecialSDKI()
        {
            return WithSpecialSDKI;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        static bool TestDevice = false;
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
        public static bool FakeDevice()
        {
            return TestDevice;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveDevice()
        {
            TestDevice = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DisableDevice()
        {
            TestDevice = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetDevice()
        {
            LogStep("ResetDevice()");
            DevicePlayerReset();
            DeviceEditorReset();
            ShowDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDevice()
        {
            LogStep("ShowDevice()");
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            Log("WithSpecialSDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetWithSpecialSDKI());
            Log("SecretKeyDevice SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevice());
            Log("SecretKeyDeviceEditor SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Log("SecretKeyDevicePlayer SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void TemporaryAccount()
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
            LogStep("TemporaryAccount()");
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            ShowDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif