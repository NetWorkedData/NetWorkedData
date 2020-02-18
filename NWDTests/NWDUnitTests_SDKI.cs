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
using System.Collections.Generic;

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
        static Dictionary<string, string> FakeDevicePlayerHashDico = new Dictionary<string, string>();
        static Dictionary<string, string> FakeDeviceEditorHashDico = new Dictionary<string, string>();
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
        public static void MemoryFakeDevice(string sKey)
        {
            if (FakeDevicePlayerHashDico.ContainsKey(sKey))
            {
                FakeDevicePlayerHashDico[sKey] = FakeDevicePlayerHash + string.Empty;
            }
            else
            {
                FakeDevicePlayerHashDico.Add(sKey, FakeDevicePlayerHash + string.Empty);
            }
            if (FakeDeviceEditorHashDico.ContainsKey(sKey))
            {
                FakeDeviceEditorHashDico[sKey] = FakeDeviceEditorHash + string.Empty;
            }
            else
            {
                FakeDeviceEditorHashDico.Add(sKey, FakeDeviceEditorHash + string.Empty);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool RestaureFakeDevice(string sKey)
        {
            bool rReturn = true;
            if (FakeDevicePlayerHashDico.ContainsKey(sKey))
            {
                FakeDevicePlayerHash = FakeDevicePlayerHashDico[sKey] + string.Empty;
            }
            else
            {
                rReturn = false;
            }
            if (FakeDeviceEditorHashDico.ContainsKey(sKey))
            {
                FakeDeviceEditorHash = FakeDeviceEditorHashDico[sKey] + string.Empty;
            }
            else
            {
                rReturn = false;
            }
            if (rReturn)
            {
                LogStep("RestaureDevice(" + sKey + ") : Success!");
            }
            else
            {
                LogStep("RestaureDevice(" + sKey + ") : FAIL!");
            }
            return rReturn;
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
        public static string UseTemporaryAccount(bool sNewAccount = false)
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences(true);
            LogStep("TemporaryAccount()");
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            ShowFakeDevice();
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RestaureAccount(string sAccount)
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference = sAccount;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetAccount()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif