//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:6
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
using SQLite4Unity3d;
//using BasicToolBox;
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
        private static int CodePinTentative = 0;
        public static string CodePinValue;
        public static string CodePinValueConfirm;
        //-------------------------------------------------------------------------------------------------------------
        static private void EngineLaunch()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.EngineLaunching;
            NWDTypeLauncher.RunLauncher();
            State = NWDStatut.EngineLaunched;
            //NWEBenchmark.Finish();
            // Ok engine is launched
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
            if (NWDAppConfiguration.SharedInstance().PreloadDatas == true || EditorByPass == true)
            {
                LaunchNext();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void ConnectToDatabaseEditor()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataEditorConnecting;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
            {
                State = NWDStatut.DataEditorConnected;
            }
            else
            {
                State = NWDStatut.Error;
            }
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseEditorTable()
        {
            //NWEBenchmark.Start();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            State = NWDStatut.DataEditorTableUpdated;
            //NWEBenchmark.Finish();
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_EDITOR_READY);
            if (Preload == true)
            {
                LaunchNext();
            }
            else
            {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_EDITOR_START_ASYNC_LOADING);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DatabaseEditorLoadData()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
            State = NWDStatut.DataEditorLoaded;
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public IEnumerator DatabaseEditorLoadDataAsync()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            IEnumerator tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsEditor();
            yield return tWaitTime;
            State = NWDStatut.DataEditorLoaded;
            //NWEBenchmark.Finish();
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
#if UNITY_EDITOR
            if (EditorPrefs.HasKey(K_PINCODE_KEY))
            {
                EditorPrefs.DeleteKey(K_PINCODE_KEY);
            }
#endif
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
            //NWEBenchmark.Start();
            DeconnectAccount();
            DeconnectAccountAnalyzeState();
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DeleteDatabaseAccount()
        {
            //NWEBenchmark.Start();
            DeconnectAccount();
            File.Delete(NWDDataManager.SharedInstance().PathDatabaseAccount());
            DeconnectAccountAnalyzeState();
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void ConnectToDatabaseAccount()
        {
            //NWEBenchmark.Start();
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
                    NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);
                }
                else
                {
                    Debug.LogWarning("### Database EXISTS NEED PINCODE");
                    State = NWDStatut.DataAccountCodePinRequest;
                    NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
#if UNITY_EDITOR
                    if (EditorByPass == true)
                    {
                        if (EditorPrefs.HasKey(K_PINCODE_KEY))
                        {
                            DatabaseAccountConnection(EditorPrefs.GetString(K_PINCODE_KEY));
                        }
                    }
#endif
                }
            }
            else
            {
                string tPincode = string.Empty;
#if UNITY_EDITOR
                if (EditorByPass == true)
                {
                    if (EditorPrefs.HasKey(K_PINCODE_KEY))
                    {
                        tPincode = EditorPrefs.GetString(K_PINCODE_KEY);
                    }
                }
#endif
                DatabaseAccountConnection(tPincode);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DatabaseAccountConnection(string sSurProtection)
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataAccountConnecting;
            //Debug.Log("<color=orange>DatabaseAccountConnection(" + sSurProtection + ")</color>");
            NWEBenchmark.Start();
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
                        //#if UNITY_EDITOR
                        //                        EditorUtility.DisplayDialog("ERROR", "CodePin for account database is invalid!", "OK");
                        //#endif
                        //Debug.Log("<color=orange>Database is not openable with this sur protected code! Tentative n°" + CodePinTentative + " : " + sSurProtection + "</color>");
                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
                        //DatabaseAccountLauncher();
                    }
                    else
                    {
                        State = NWDStatut.DataAccountCodePinStop;
                        //Debug.Log("<color=orange>Database is not openable max tentative over! Tentative n°" + CodePinTentative + "</color>");
                        // Kill App || Destroy Database || Call FBI || Vodoo ?
                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
                    }
                }
                else
                {
                    CodePinTentative = 0;
                    if (NWDAppConfiguration.SharedInstance().SurProtected == true)
                    {
#if UNITY_EDITOR
                        if (EditorByPass == true)
                        {
                            EditorPrefs.SetString(K_PINCODE_KEY, sSurProtection);
                        }
#endif

                        State = NWDStatut.DataAccountCodePinSuccess;
                        //Debug.Log("<color=orange>Database is opened with this sur protected code! Tentative n°" + CodePinTentative + "</color>");
                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
                    }
                    State = NWDStatut.DataAccountConnected;
                }
            }
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseAccountTable()
        {
            //NWEBenchmark.Start();
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            State = NWDStatut.DataAccountTableUpdated;
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_READY);
            //NWEBenchmark.Finish();
            if (Preload == true)
            {
                LaunchNext();
            }
            else
            {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_START_ASYNC_LOADING);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseAccountLoadData()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            State = NWDStatut.DataAccountLoaded;
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public IEnumerator DatabaseAccountLoadDataAsync()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            IEnumerator tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsAccount();
            yield return tWaitTime;
            State = NWDStatut.DataAccountLoaded;
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        
        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseIndexationStart()
        {
            State = NWDStatut.DataIndexationStart;
            if (Preload == true)
            {
                DatabaseIndexation();
            }
            else
            {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_INDEXATION_START_ASYNC);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        static private void DatabaseIndexation()
        {
            //NWEBenchmark.Start();
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataIndexationFinish;
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static public IEnumerator DatabaseIndexationAsync()
        {
            //NWEBenchmark.Start();
            IEnumerator tWaitTime = NWDDataManager.SharedInstance().AsyncIndexAllObjects();
            yield return tWaitTime;
            State = NWDStatut.DataIndexationFinish;
            //NWEBenchmark.Finish();
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void Ready()
        {
            //NWEBenchmark.Start();
            State = NWDStatut.NetWorkedDataReady;
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_READY);
            //NWEBenchmark.Finish();
            NWEBenchmark.Finish("NetWorkedData");
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
       static public void OnApplicationPause(bool sPauseStatus)
        {
            if (sPauseStatus == false)
            {
                //Debug.Log("OnApplicationPause Pause is OFF");
            }
            else
            {
                //Debug.Log("OnApplicationPause Pause is ON");
                if (NWDAppConfiguration.SharedInstance().SurProtected == true)
                {
                    NWDDataManager.SharedInstance().DeconnectFromDatabaseAccount();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================