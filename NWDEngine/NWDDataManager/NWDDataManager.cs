//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        private static readonly NWDDataManager kSharedInstance = new NWDDataManager ();
        private bool kConnectedToDatabase = false;
        private bool kConnectedToDatabaseIsProgress = false;
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguage = "en";
        // Memebes properties for editor data
        public SQLiteConnection SQLiteConnectionEditor;
        public string DatabasePathEditor = "StreamingAssets";
        public string DatabaseNameEditor  = "NWDDatabaseEditor.prp";
        //public SQLiteConnection SQLiteConnectionEditorV4;
        // Members properties for account dependant database (data from user)
        public SQLiteConnection SQLiteConnectionAccount;
        //public string DatabasePathAccount = "Editor";
        public string DatabaseNameAccount = "Account.prp";
        //public SQLiteConnection SQLiteConnectionAccountV4;
        //public BTBNotificationManager NotificationCenter;
        //public bool NeedCopy = false;
        public bool IsLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //-------------------------------------------------------------------------------------------------------------
        //public static bool StartNetWorkedData()
        //{
        //    //Debug.Log("NWDDataManager StartNetWorkedData()");
        //    // just to init the class
        //    return NWDTypeLauncher.IsLaunched;
        //}
        //-------------------------------------------------------------------------------------------------------------
        const string PlayerLanguageKey = "PlayerLanguageKey";
        //-------------------------------------------------------------------------------------------------------------
        public void PlayerLanguageSave(string sNewLanguage)
        {
            //Debug.Log("NWDDataManager PlayerLanguageSave()");
            NWDUserPreference tUserLanguage = NWDUserPreference.GetByInternalKeyOrCreate(PlayerLanguageKey, new NWDMultiType(string.Empty));
            tUserLanguage.Value.SetStringValue(sNewLanguage);
            tUserLanguage.UpdateData();
            PlayerLanguage = sNewLanguage;
            // notify the change
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PlayerLanguageLoad()
        {
            //Debug.Log("NWDDataManager PlayerLanguageLoad()");
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
            //Debug.Log("NWDDataManager private instance Constructor");
            PlayerLanguage = NWDDataLocalizationManager.SystemLanguageString();
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);

            //NotificationCenter = BTBNotificationManager.SharedInstance();
            LoadPreferences (NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDDataManager ()
        {
            //Debug.Log ("NWDDataManager Destructor");
            // reccord all modifications because this instance will be destroyed
            SharedInstance().DataQueueExecute();
            // remove notification center
            //if (NotificationCenter != null) {
                BTBNotificationManager.SharedInstance().RemoveAll ();
            //    NotificationCenter = null;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> mTypeLoadedList = new List<Type> ();
        public List<Type> mTypeList = new List<Type> ();
        public List<Type> mTypeSynchronizedList = new List<Type> ();
        public List<Type> mTypeUnSynchronizedList = new List<Type> ();

        public List<Type> mTypeAccountDependantList = new List<Type> ();
        public List<Type> mTypeNotAccountDependantList = new List<Type> ();

        public Dictionary<string, Type> mTrigramTypeDictionary = new Dictionary<string, Type> ();
        //-------------------------------------------------------------------------------------------------------------
        //public void AddClassToManage (Type sType, bool sServerSynchronize, string sClassTrigramme, string sMenuName, string sDescription = "")
        //{
        //    if (mTypeList.Contains (sType) == false) {
        //        mTypeList.Add (sType);
        //    }
        //    if (sServerSynchronize == true) {
        //        if (mTypeSynchronizedList.Contains (sType) == false) {
        //            mTypeSynchronizedList.Add (sType);
        //        }
        //        if (mTypeUnSynchronizedList.Contains (sType) == true) {
        //            mTypeUnSynchronizedList.Remove (sType);
        //        }
        //    } else {
        //        if (mTypeSynchronizedList.Contains (sType) == true) {
        //            mTypeSynchronizedList.Remove (sType);
        //        }
        //        if (mTypeUnSynchronizedList.Contains (sType) == false) {
        //            mTypeUnSynchronizedList.Add (sType);
        //        }
        //    }
        //    if (mTypeLoadedList.Contains (sType) == false) {
        //        var tMethodInfo = sType.GetMethod ("redefineClassToUse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //        if (tMethodInfo != null) {
        //            tMethodInfo.Invoke (null, new object[]{ sType,sServerSynchronize, sClassTrigramme, sMenuName, sDescription });
        //        }

        //        if (mTrigramTypeDictionary.ContainsKey (sClassTrigramme)) {
        //            Debug.Log ("ERROR this trigramme '" + sClassTrigramme + "' is allreday use by another class! (" + mTrigramTypeDictionary [sClassTrigramme] + ")");
        //        } else {
        //            mTrigramTypeDictionary.Add (sClassTrigramme, sType);
        //        }

        //        mTypeLoadedList.Add (sType);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //void ReLoadAllClass ()
        //{
        //    foreach (Type tType in mTypeList) {
        //        if (mTypeLoadedList.Contains (tType) == false) {
        //            var tMethodInfo = tType.GetMethod ("ClassLoad", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //            if (tMethodInfo != null) {
        //                tMethodInfo.Invoke (null, null);
        //            }
        //            mTypeLoadedList.Add (tType);
        //        }
        //    }
  //          //IsLoaded = true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public bool TestSaltMemorizationForAllClass ()
        {
            //Debug.Log("NWDDataManager TestSaltMemorizationForAllClass()");
            bool rReturn = true;
            foreach (Type tType in mTypeList) {

                if (NWDDatas.FindTypeInfos(tType).SaltOk != "ok")
                {
                    Debug.LogWarning(" Erreur in salt for " + NWDDatas.FindTypeInfos(tType).ClassName);
                    NWDDatas.FindTypeInfos(tType).SaltRegenerate();
                    rReturn = false;
                    //break;
                }

                //var tMethodInfo = tType.GetMethod ("PrefSalt", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tMethodInfo != null) {
                //    string tR = tMethodInfo.Invoke (null, null) as string;
                //    if (tR != "ok") {
                //        rReturn = false;
                //    }
                //}
            }
            if (rReturn == false) {
                // do reccord and recompile
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