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
using System.Collections;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        public static int CodePinTentative = 0;
        public static string CodePinValue;
        public static string CodePinValueConfirm;
        //public static bool CodePinNeeded = false;
        //public static bool CodePinCreationNeeded = false;
        //-------------------------------------------------------------------------------------------------------------
        static private void EngineLaunch()
        {
            BTBBenchmark.Start();
            State = NWDStatut.EngineLaunching;
            NWDTypeLauncher.RunLauncher();
            State = NWDStatut.EngineLaunched;
            BTBBenchmark.Finish();
            // Ok engine is launched
            BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void ConnectToDatabaseEditor()
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataEditorConnecting;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
            {
                State = NWDStatut.DataEditorConnected;
            }
            else
            {
                State = NWDStatut.Error;
            }
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseEditorTable()
        {
            BTBBenchmark.Start();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            State = NWDStatut.DataEditorTableUpdated;
            BTBBenchmark.Finish();
            BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_EDITOR_READY);
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseEditorLoadData()
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
            State = NWDStatut.DataEditorLoaded;
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public IEnumerator DatabaseEditorLoadDataAsync()
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            IEnumerator tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsEditor();
            yield return tWaitTime;
            State = NWDStatut.DataEditorLoaded;
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DeconnectAccount()
        {
            NWDDataManager tDataManager = NWDDataManager.SharedInstance();
            tDataManager.DataAccountConnected = false;
            //Close SLQite
            if (tDataManager.SQLiteConnectionAccount != null)
            {
                //tDataManager.SQLiteConnectionAccount.Commit();
                //tDataManager.SQLiteConnectionAccount.Dispose();
                tDataManager.SQLiteConnectionAccount.Close();
            }
            tDataManager.SQLiteConnectionAccount = null;
            tDataManager.DataAccountLoaded = false;
            tDataManager.DataAccountConnected = false;
            CodePinTentative = 0;
            CodePinValue = string.Empty;
            CodePinValueConfirm = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DeconnectAccountAnalyzeState()
        {
            if (NWDAppConfiguration.SharedInstance().SurProtected == true)
            {
                if (NWDDataManager.SharedInstance().DatabaseAccountExists() == true)
                {
                    State = NWDStatut.DataAccountCodePinRequest;
                }
                else
                {
                    State = NWDStatut.DataAccountCodePinCreate;
                }
            }
            else
            {
                State = NWDStatut.DataEditorLoaded;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DeconnectFromDatabaseAccount()
        {
            BTBBenchmark.Start();
            DeconnectAccount();
            DeconnectAccountAnalyzeState();
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DeleteDatabaseAccount()
        {
            BTBBenchmark.Start();
            DeconnectAccount();
            File.Delete(NWDDataManager.SharedInstance().PathDatabaseAccount());
            DeconnectAccountAnalyzeState();
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void ConnectToDatabaseAccount()
        {
            BTBBenchmark.Start();
            NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
            // prevent old connexion
            tShareInstance.DeconnectFromDatabaseAccount();

            string tSurProtection = string.Empty;
            if (NWDAppConfiguration.SharedInstance().SurProtected == true)
            {
                if (tShareInstance.DatabaseAccountExists() == false)
                {
                    Debug.LogWarning("### Database NOT EXISTS");
                    State = NWDStatut.DataAccountCodePinCreate;
                    BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);
                }
                else
                {
                    Debug.LogWarning("### Database EXISTS NEED PINCODE");
                    State = NWDStatut.DataAccountCodePinRequest;
                    BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
                }
            }
            else
            {
                DatabaseAccountConnection(string.Empty);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DatabaseAccountConnection(string sSurProtection)
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataAccountConnecting;
            Debug.Log("<color=orange>DatabaseAccountConnection(" + sSurProtection + ")</color>");
            BTBBenchmark.Start();
            //if (IsLaunched == true && DataAccountConnected == false && IsLaunching == true)
            if (NWDDataManager.SharedInstance().DataAccountConnected == false)
            {
                CodePinTentative++;
                // Get ShareInstance of datamanager instance
                if (NWDDataManager.SharedInstance().ConnectToDatabaseAccount(sSurProtection) == false)
                {
                    if (CodePinTentative < NWDAppConfiguration.SharedInstance().ProtectionTentativeMax)
                    {
                        State = NWDStatut.DataAccountCodePinFail;
#if UNITY_EDITOR
                        EditorUtility.DisplayDialog("ERROR", "CodePin for account database is invalid!", "OK");
#endif
                        Debug.Log("<color=orange>Database is not openable with this sur protected code! Tentative n°" + CodePinTentative + " : " + sSurProtection + "</color>");
                        BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
                        //DatabaseAccountLauncher();
                    }
                    else
                    {
                        State = NWDStatut.DataAccountCodePinStop;
                        Debug.Log("<color=orange>Database is not openable max tentative over! Tentative n°" + CodePinTentative + "</color>");
                        // Kill App || Destroy Database || Call FBI || Vodoo ?
                        BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
                    }
                }
                else
                {
                    CodePinTentative = 0;
                    if (NWDAppConfiguration.SharedInstance().SurProtected == true)
                    {
                        State = NWDStatut.DataAccountCodePinSuccess;
                        Debug.Log("<color=orange>Database is opened with this sur protected code! Tentative n°" + CodePinTentative + "</color>");
                        BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
                    }
                    State = NWDStatut.DataAccountConnected;
                }
            }
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseAccountTable()
        {
            BTBBenchmark.Start();
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            State = NWDStatut.DataAccountTableUpdated;
            BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_READY);
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseAccountLoadData()
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            State = NWDStatut.DataAccountLoaded;
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public IEnumerator DatabaseAccountLoadDataAsync()
        {
            BTBBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            IEnumerator tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsAccount();
            yield return tWaitTime;
            State = NWDStatut.DataAccountLoaded;
            BTBBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================