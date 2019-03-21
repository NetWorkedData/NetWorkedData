//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        const string PlayerLanguageKey = "PlayerLanguageKey";
        //-------------------------------------------------------------------------------------------------------------
        private static readonly NWDDataManager kSharedInstance = new NWDDataManager ();
        private bool kConnectedToDatabase = false;
        private bool kConnectedToDatabaseIsProgress = false;
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguage = "en";
        public SQLiteConnection SQLiteConnectionEditor;
        public string DatabasePathEditor = "StreamingAssets";
        public string DatabaseNameEditor  = "NWDDatabaseEditor.prp";
        public SQLiteConnection SQLiteConnectionAccount;
        public string DatabaseNameAccount = "Account.prp";
        public bool IsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> mTypeLoadedList = new List<Type>();
        public List<Type> mTypeList = new List<Type>();
        public List<Type> mTypeSynchronizedList = new List<Type>();
        public List<Type> mTypeUnSynchronizedList = new List<Type>();
        public List<Type> mTypeAccountDependantList = new List<Type>();
        public List<Type> mTypeNotAccountDependantList = new List<Type>();
        public Dictionary<string, Type> mTrigramTypeDictionary = new Dictionary<string, Type>();
        //-------------------------------------------------------------------------------------------------------------
        public void PlayerLanguageSave(string sNewLanguage)
        {
            NWDUserPreference tUserLanguage = NWDUserPreference.GetByInternalKeyOrCreate(PlayerLanguageKey, new NWDMultiType(string.Empty));
            tUserLanguage.Value.SetStringValue(sNewLanguage);
            tUserLanguage.UpdateData();
            PlayerLanguage = sNewLanguage;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguageLoad()
        {
            NWDUserPreference tUserLanguage = NWDUserPreference.GetByInternalKeyOrCreate(PlayerLanguageKey, new NWDMultiType(string.Empty));
            if (tUserLanguage.Value.GetStringValue() == string.Empty)
            {
                tUserLanguage.Value.SetStringValue(NWDDataLocalizationManager.SystemLanguageString());
                tUserLanguage.UpdateData();
            }
            PlayerLanguage = tUserLanguage.Value.GetStringValue();
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
            return PlayerLanguage;
        }
        //-------------------------------------------------------------------------------------------------------------
        private NWDDataManager ()
        {
            PlayerLanguage = NWDDataLocalizationManager.SystemLanguageString();
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
            //LoadPreferences(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDDataManager ()
        {
            SharedInstance().DataQueueExecute();
            BTBNotificationManager.SharedInstance().RemoveAll ();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TestSaltMemorizationForAllClass ()
        {
            bool rReturn = true;
            foreach (Type tType in mTypeList) {
                if (NWDBasisHelper.FindTypeInfos(tType).SaltValid == false)
                {
                    Debug.LogWarning(" Erreur in salt for " + NWDBasisHelper.FindTypeInfos(tType).ClassName);
                    rReturn = false;
                    break;
                }
            }
            if (rReturn == false) {
                #if UNITY_EDITOR
                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().SelectedEnvironment ());
                #else
                // no... ALERT USER ERROR IN APP DISTRIBUTION
                #endif
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDataManager SharedInstance() {
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================