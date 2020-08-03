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
#endif
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
        public static string SetNewAccount()
        {
            string tNewAccount = NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + NWEConstants.K_MINUS + "00000" + NWEConstants.K_MINUS + NWDToolbox.RandomStringNumeric(10) + NWEConstants.K_MINUS + NWDToolbox.RandomStringNumeric(7) + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE;
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().SetAccountReference(tNewAccount);
            return tNewAccount;
        }
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
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference());
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
            Log("Account : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference());
            ShowFakeDevice();
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RestaureAccount(string sAccount)
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().SetAccountReference(sAccount);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetAccount()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif