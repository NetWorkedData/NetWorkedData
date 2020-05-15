//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void SavePreferences (NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SavePreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.SavePreferences();
#else
            sEnvironment.SavePreferences ();
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		public void LoadPreferences (NWDAppEnvironment sEnvironment)
		{
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.LoadPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.LoadPreferences();
#else
            sEnvironment.LoadPreferences ();
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ResetPreferences (NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            NWDAppConfiguration.SharedInstance().DevEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.ResetPreferences();
            NWDAppConfiguration.SharedInstance().ProdEnvironment.ResetPreferences();
#else
            sEnvironment.ResetPreferences ();
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
                NWEPrefsManager.ShareInstance().set(NWD.K_PlayerLanguageKey, sNewLanguage);
            }
            PlayerLanguage = sNewLanguage;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AccountLanguageLoad()
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            if (DeviceDatabaseLoaded == true)
            {
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Start("account language");
                }
                NWDAccountPreference tAccountLanguage = NWDAccountPreference.GetByInternalKeyOrCreate(NWD.K_PlayerLanguageKey, new NWDMultiType(string.Empty));
                if (tAccountLanguage.Value.GetStringValue() == string.Empty)
                {
                    tAccountLanguage.Value.SetStringValue(NWDDataLocalizationManager.SystemLanguageString());
                    tAccountLanguage.UpdateData();
                }
                PlayerLanguage = tAccountLanguage.Value.GetStringValue();
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Finish("account language");
                }
            }
            else
            {
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Start("device language");
                }
                PlayerLanguage = NWEPrefsManager.ShareInstance().getString(NWD.K_PlayerLanguageKey, PlayerLanguage);
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Finish("device language");
                }
            }
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
#if UNITY_EDITOR
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Step(true, "<color=red>PlayerLanguageLoad</color> Language is " + PlayerLanguage);
            }
#endif
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
            return PlayerLanguage;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================