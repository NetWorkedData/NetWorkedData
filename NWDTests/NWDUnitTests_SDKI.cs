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
        static bool UseFakeDevice = false;
        //-------------------------------------------------------------------------------------------------------------
        static string FakeDevicePlayerHash;
        static string FakeDeviceEditorHash;
        //-------------------------------------------------------------------------------------------------------------
        public static void FakeDevicePlayerReset()
        {
            FakeDevicePlayerHash = "PlaY3r-" + NWDToolbox.RandomStringUnix(NWDAppEnvironment.kSecretKeyDevicePlayerLength);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetFakeDevicePlayer()
        {
            if (string.IsNullOrEmpty(FakeDevicePlayerHash))
            {
                FakeDevicePlayerReset();
            }
            return FakeDevicePlayerHash;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void FakeDeviceEditorReset()
        {
            FakeDeviceEditorHash = "ediT0R-" + NWDToolbox.RandomStringUnix(NWDAppEnvironment.kSecretKeyDevicePlayerLength);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetFakeDeviceEditor()
        {
            if (string.IsNullOrEmpty(FakeDeviceEditorHash))
            {
                FakeDeviceEditorReset();
            }
            return FakeDeviceEditorHash;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsFakeDevice()
        {
            return UseFakeDevice;
        }
        //-------------------------------------------------------------------------------------------------------------

        public delegate void NWEDeviceBlock();
        //-------------------------------------------------------------------------------------------------------------
        public static void TryWithFakeDevice(NWEDeviceBlock sDeviceBlock)
        {
            UseFakeDevice = true;
            sDeviceBlock();
            UseFakeDevice = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void EnableFakeDevice()
        {
            UseFakeDevice = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DisableFakeDevice()
        {
            UseFakeDevice = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetFakeDevice()
        {
            LogStep("ResetDevice()");
            FakeDevicePlayerReset();
            FakeDeviceEditorReset();
            ShowFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowFakeDevice()
        {
            LogStep("ShowDevice()");
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            Log("WithSpecialSDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetWithSpecialSDKI());
            Log("SecretKeyDevice SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevice());
            Log("SecretKeyDeviceEditor SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Log("SecretKeyDevicePlayer SDKI : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UseTemporaryAccount()
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
            LogStep("TemporaryAccount()");
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            ShowFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif