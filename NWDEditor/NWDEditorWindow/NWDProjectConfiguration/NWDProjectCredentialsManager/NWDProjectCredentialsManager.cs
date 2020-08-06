﻿//=====================================================================================================================
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

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDCredentialsRequired : int
    {
        ForSFTPGenerate,
        ForSFTPGenerateDev,
        ForSFTPGeneratePreprod,
        ForSFTPGenerateProd,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDProjectCredentialsManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDProjectCredentialsManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The scroll position.
        /// </summary>
        static Vector2 ScrollPosition;
        static public string Password = string.Empty;
        static public string VectorString = string.Empty;
        static public bool ShowPasswordInLog = false;
        static public bool SaveCredentials = false;
        //-------------------------------------------------------------------------------------------------------------
        public static bool Checked(NWDCredentialsRequired sCredentialsType)
        {
            bool rReturn = IsValid(sCredentialsType);
            if (rReturn == false)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_ALERT_IDEMOBI_TITLE,
                   "This operation need credentials to decrypt some passwords.",
                   "Credentials window",
                   "Cancel"))
                {
                    SharedInstanceFocus();
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsValid(NWDCredentialsRequired sCredentialsType)
        {
            bool rReturn = false;
            switch (sCredentialsType)
            {
                case NWDCredentialsRequired.ForSFTPGenerate:
                    {
                        rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGenerateDev:
                    {
                        rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGeneratePreprod:
                    {
                        rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGenerateProd:
                    {
                        rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectCredentialsManager SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectCredentialsManager)) as NWDProjectCredentialsManager;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all Editor Configuration Manager Windows.
        /// </summary>
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDProjectCredentialsManager));
            foreach (NWDProjectCredentialsManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstanced()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            TitleInit(NWDConstants.K_CREDENTIALS_CONFIGURATION_TITLE, typeof(NWDProjectCredentialsManager));
            Load();
            // get values
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On Disable action.
        /// </summary>
        private void OnDisable()
        {
            // for all NWDEditorWindow refresh

            NWDProjectConfigurationManager.Refresh();
            NWDAppConfigurationManager.Refresh();
            NWDAppEnvironmentConfigurationManager.Refresh();
            NWDModelManager.Refresh();
            NWDAppEnvironmentSync.Refresh();
            NWDAppEnvironmentChooser.Refresh();
            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void FlushCredentials(NWDCredentialsRequired sCredentialsType)
        {
            Password = string.Empty;
            VectorString = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Load()
        {
            //NWDBenchmark.Start();
            // get values
            SaveCredentials = NWDProjectPrefs.GetBool(NWDConstants.K_CREDENTIALS_SAVE, false);
            ShowPasswordInLog = NWDProjectPrefs.GetBool(NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, false);
            Password = NWDProjectPrefs.GetString(NWDConstants.K_CREDENTIALS_PASSWORD, string.Empty);
            VectorString = NWDProjectPrefs.GetString(NWDConstants.K_CREDENTIALS_VECTOR, string.Empty);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            //NWDBenchmark.Start();
            // set values
            NWDProjectPrefs.SetBool(NWDConstants.K_CREDENTIALS_SAVE, SaveCredentials);
            if (SaveCredentials == true)
            {
                NWDProjectPrefs.SetBool(NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, ShowPasswordInLog);
                NWDProjectPrefs.SetString(NWDConstants.K_CREDENTIALS_PASSWORD, Password);
                NWDProjectPrefs.SetString(NWDConstants.K_CREDENTIALS_VECTOR, VectorString);
            }
            else
            {
                NWDProjectPrefs.SetBool(NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, false);
                NWDProjectPrefs.SetString(NWDConstants.K_CREDENTIALS_PASSWORD, string.Empty);
                NWDProjectPrefs.SetString(NWDConstants.K_CREDENTIALS_VECTOR, string.Empty);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void flush()
        {
            SaveCredentials = false;
            ShowPasswordInLog = false;
            Password = string.Empty;
            VectorString = string.Empty;
            Save();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Credentials for project");
            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUI.BeginChangeCheck();

            //General preferences
            NWDGUILayout.Section("Credentials preferences");

            SaveCredentials = EditorGUILayout.Toggle("Save Credentials", SaveCredentials);
            ShowPasswordInLog = EditorGUILayout.Toggle("Show passwords in Log", ShowPasswordInLog);

            NWDGUILayout.Section("Credentials");

            Password = EditorGUILayout.PasswordField("General password", Password);
            VectorString = EditorGUILayout.PasswordField("General vector", VectorString);

            if (EditorGUI.EndChangeCheck())
            {
                //Debug.Log("Change");
                Save();
            }

            NWDGUILayout.Section("Actions");

            if (GUILayout.Button("Flush"))
            {
                flush();
            }

            if (GUILayout.Button("Flush and close"))
            {
                flush();
                Close();
            }
            // end scroll
            GUILayout.EndScrollView();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif