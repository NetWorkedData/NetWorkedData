//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager  // TODO : put in static?
    {
        //-------------------------------------------------------------------------------------------------------------
        const string PlayerLanguageKey = "PlayerLanguageKey";
        //-------------------------------------------------------------------------------------------------------------
        private static NWDDataManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public int ClassExpected = 0;
        public int ClassEditorExpected = 0;
        public int ClassAccountExpected = 0;
        public int ClassDataLoaded = 0;
        public int ClassEditorDataLoaded = 0;
        public int ClassAccountDataLoaded = 0;
        public int ClassIndexation = 0;
        //-------------------------------------------------------------------------------------------------------------
        public bool DataAccountConnected = false;
        public bool DataAccountConnectionInProgress = false;
        public bool DataAccountLoaded = false;
        public string DatabaseNameAccount = "Account.prp";  // TODO rename DataEditorBasename by  replace by DataEditorPath()
        //public SQLiteConnection SQLiteConnectionAccount;    // TODO rename SQLiteAccountConnection
        public IntPtr SQLiteAccountHandle;
        //-------------------------------------------------------------------------------------------------------------
        public bool DataEditorConnected = false;
        public bool DataEditorConnectionInProgress = false;
        public bool DataEditorLoaded = false;
        public string DatabasePathEditor = "StreamingAssets";       // TODO remove and use const!?
        public string DatabaseNameEditor = "NWDDatabaseEditor.prp"; // TODO rename DataEditorBasename by  replace by DataEditorPath()
        //public SQLiteConnection SQLiteConnectionEditor;             // TODO rename SQLiteEditorConnection
        public IntPtr SQLiteEditorHandle;
        //-------------------------------------------------------------------------------------------------------------
        private bool DatasIndexed = false;
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguage = "en";
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> mTypeLoadedList = new List<Type>();                   // TODO rename ClassLoadedList
        public List<Type> mTypeList = new List<Type>();                         // TODO rename ClassList
        public List<Type> mTypeSynchronizedList = new List<Type>();             // TODO rename ClassSynchronizedList
        public List<Type> mTypeUnSynchronizedList = new List<Type>();           // TODO rename ClassUnsynchronizedList
        public List<Type> mTypeAccountDependantList = new List<Type>();         // TODO rename ClassAccountDependentList
        public List<Type> mTypeNotAccountDependantList = new List<Type>();      // TODO rename ClassEditorDependentList
        public Dictionary<string, Type> mTrigramTypeDictionary = new Dictionary<string, Type>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
        public void PlayerLanguageSave(string sNewLanguage)
        {
            if (DataAccountLoaded == true)
            {
                NWDUserPreference tUserLanguage = NWDUserPreference.GetByInternalKeyOrCreate(PlayerLanguageKey, new NWDMultiType(string.Empty));
                tUserLanguage.Value.SetStringValue(sNewLanguage);
                tUserLanguage.UpdateData();
            }
            else
            {
                NWEPrefsManager.ShareInstance().set(PlayerLanguageKey, sNewLanguage);
            }
            PlayerLanguage = sNewLanguage;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguageLoad()
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            if (DataAccountLoaded == true)
            {
                //NWEBenchmark.Start("account language");
                NWDUserPreference tUserLanguage = NWDUserPreference.GetByInternalKeyOrCreate(PlayerLanguageKey, new NWDMultiType(string.Empty));
                if (tUserLanguage.Value.GetStringValue() == string.Empty)
                {
                    tUserLanguage.Value.SetStringValue(NWDDataLocalizationManager.SystemLanguageString());
                    tUserLanguage.UpdateData();
                }
                PlayerLanguage = tUserLanguage.Value.GetStringValue();
                //NWEBenchmark.Finish("account language");
            }
            else
            {
                //NWEBenchmark.Start("device language");
                PlayerLanguage = NWEPrefsManager.ShareInstance().getString(PlayerLanguageKey, PlayerLanguage);
                //NWEBenchmark.Finish("device language");
            }
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
            return PlayerLanguage;
        }
        //-------------------------------------------------------------------------------------------------------------
        private NWDDataManager()
        {
            PlayerLanguage = NWDDataLocalizationManager.SystemLanguageString();
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
            //LoadPreferences(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDDataManager()
        {
            SharedInstance().DataQueueExecute();
            NWENotificationManager.SharedInstance().RemoveAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TestSaltMemorizationForAllClass()
        {
            bool rReturn = true;
            foreach (Type tType in mTypeList)
            {
                if (NWDBasisHelper.FindTypeInfos(tType).SaltValid == false)
                {
                    Debug.LogWarning(" Erreur in salt for " + NWDBasisHelper.FindTypeInfos(tType).ClassName);
                    rReturn = false;
                    break;
                }
            }
            if (rReturn == false)
            {
#if UNITY_EDITOR
                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().SelectedEnvironment ());
#else
                // no... ALERT USER ERROR IN APP DISTRIBUTION
#endif
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDataManager SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new NWDDataManager();
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DataLoaded() // loaded but not indexed
        {
            bool rReturn = true;
            if (DataEditorLoaded == false || DataAccountLoaded == false)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreIndexed() // loaded and indexed
        {
            return DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreNotReady()
        {
            return !DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreReady()
        {
            return DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================