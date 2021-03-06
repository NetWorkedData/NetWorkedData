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

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void SavePreferences(NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.SavePreferences();
#else
            sEnvironment.SavePreferences();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences(NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.LoadPreferences();
#else
            sEnvironment.LoadPreferences();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPreferences(NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.ResetPreferences();
#else
            sEnvironment.ResetPreferences();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AccountLanguageSave(string sNewLanguage)
        {
            if (DeviceDatabaseLoaded == true)
            {
                NWDAccountPreference tAccountLanguage = NWDAccountPreference.GetByInternalKeyOrCreate(NWD.K_PlayerLanguageKey, new NWDMultiType(string.Empty));
                tAccountLanguage.Value.SetStringValue(sNewLanguage);
                tAccountLanguage.UpdateData();
            }
            else
            {
                NWDRuntimePrefs.ShareInstance().set(NWD.K_PlayerLanguageKey, sNewLanguage);
            }
            PlayerLanguage = sNewLanguage;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AccountLanguageLoad()
        {
            NWDBenchmarkLauncher.Start();
            if (DeviceDatabaseLoaded == true)
            {
                NWDBenchmarkLauncher.Start("account language");
                NWDAccountPreference tAccountLanguage = NWDAccountPreference.GetByInternalKeyOrCreate(NWD.K_PlayerLanguageKey, new NWDMultiType(string.Empty));
                if (tAccountLanguage != null)
                {
                    tAccountLanguage.PropertiesPrevent(); // Auto fill the properties if necessary
                    if (tAccountLanguage.Value.GetStringValue() == string.Empty)
                    {
                        tAccountLanguage.Value.SetStringValue(NWDDataLocalizationManager.SystemLanguageString());
                        tAccountLanguage.UpdateData();
                    }
                    PlayerLanguage = tAccountLanguage.Value.GetStringValue();
                }
                else
                {
                    Debug.LogWarning("NO NWDAccountPreference tAccountLanguage ... error in access ? or no datas loaded ? or another thing ?!");
                }
                NWDBenchmarkLauncher.Finish("account language", true, "PlayerLanguage = " + PlayerLanguage + "");
            }
            else
            {
                NWDBenchmarkLauncher.Start("device language");
                PlayerLanguage = NWDRuntimePrefs.ShareInstance().getString(NWD.K_PlayerLanguageKey, PlayerLanguage);
                NWDBenchmarkLauncher.Finish("device language", true, "PlayerLanguage bypass by NWEPrefsManager :  " + PlayerLanguage + "");
            }
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
#if UNITY_EDITOR
            NWDBenchmarkLauncher.Step(true, "<b>PlayerLanguageLoad</b> Language is " + PlayerLanguage);
#endif
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
            NWDBenchmarkLauncher.Finish();
            return PlayerLanguage;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
