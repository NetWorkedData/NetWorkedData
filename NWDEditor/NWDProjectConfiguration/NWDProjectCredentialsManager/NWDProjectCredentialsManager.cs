//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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
        //ForDevSync,
        //ForPreprodSync,
        //ForProdSync,
        //Both,
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
        private const string PasswordKey = "PasswordKey_31564873413687653";
        private const string VectorStringKey = "VectorStringKey_79877414532159874";
        private const string ShowPasswordInLogKey = "ShowPasswordInLogKey_79585254215";
        private const string SaveCredentialsKey = "SaveCredentialsKey_7895452114789523654";
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
                        //rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    }
                    break;
                    //case NWDCredentialsRequired.Both:
                    //    {
                    //        rReturn = string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false;
                    //    }
                    //    break;
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
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectCredentialsManager)) as NWDProjectCredentialsManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
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
            //NWEBenchmark.Start();
            TitleInit(NWDConstants.K_CREDENTIALS_CONFIGURATION_TITLE, typeof(NWDProjectCredentialsManager));
            SaveCredentials = NWDProjectPrefs.GetBool(SaveCredentialsKey, false);
            ShowPasswordInLog = NWDProjectPrefs.GetBool(ShowPasswordInLogKey, false);
            Password = NWDProjectPrefs.GetString(PasswordKey, string.Empty);
            VectorString = NWDProjectPrefs.GetString(VectorStringKey, string.Empty);

            //NWEBenchmark.Finish();
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
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
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
                NWDProjectPrefs.SetBool(SaveCredentialsKey, SaveCredentials);
                if (SaveCredentials == true)
                {
                    NWDProjectPrefs.SetBool(ShowPasswordInLogKey, ShowPasswordInLog);
                    NWDProjectPrefs.SetString(PasswordKey, Password);
                    NWDProjectPrefs.SetString(VectorStringKey, VectorString);
                }
                else
                {
                    NWDProjectPrefs.SetBool(ShowPasswordInLogKey, false);
                    NWDProjectPrefs.SetString(PasswordKey, string.Empty);
                    NWDProjectPrefs.SetString(VectorStringKey, string.Empty);
                }
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void flush()
        {
            NWDProjectPrefs.SetBool(SaveCredentialsKey, false);
            NWDProjectPrefs.SetBool(ShowPasswordInLogKey, false);
            NWDProjectPrefs.SetString(PasswordKey, string.Empty);
            NWDProjectPrefs.SetString(VectorStringKey, string.Empty);
            Password = string.Empty;
            VectorString = string.Empty;
            ShowPasswordInLog = false;
            SaveCredentials = false;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
