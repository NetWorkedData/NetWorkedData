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
    public class NWDEditorCredentialsManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDEditorCredentialsManager kSharedInstance;
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
        public static NWDEditorCredentialsManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorCredentialsManager)) as NWDEditorCredentialsManager;
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
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDEditorCredentialsManager));
            foreach (NWDEditorCredentialsManager tWindow in tWindows)
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
            TitleInit(NWDConstants.K_EDITOR_CONFIGURATION_TITLE, typeof(NWDEditorCredentialsManager));
            SaveCredentials = EditorPrefs.GetBool(SaveCredentialsKey, false);
            ShowPasswordInLog = EditorPrefs.GetBool(ShowPasswordInLogKey, false);
            Password = EditorPrefs.GetString(PasswordKey, string.Empty);
            VectorString = EditorPrefs.GetString(VectorStringKey, string.Empty);

            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On Disable action.
        /// </summary>
        private void OnDisable()
        {
            // for all NWDEditorWindow refresh

            NWDEditorConfigurationManager.Refresh();
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
            NWDGUILayout.Title("Password of cluster");
            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            //General preferences
            NWDGUILayout.Section("Enter the password use to decrypt passwords of cluster");

            //EditorGUILayout.LabelField("Debug password", Password);
            //EditorGUILayout.LabelField("Debug vector", VectorString);

            EditorGUI.BeginChangeCheck();
            SaveCredentials = EditorGUILayout.Toggle("Save Credentials", SaveCredentials);
            Password = EditorGUILayout.PasswordField("General password", Password);
            VectorString = EditorGUILayout.PasswordField("General vector", VectorString);
            ShowPasswordInLog = EditorGUILayout.Toggle("Show passwords in Log", ShowPasswordInLog);
            if (EditorGUI.EndChangeCheck())
            {
                //Debug.Log("Change");
                EditorPrefs.SetBool(SaveCredentialsKey, SaveCredentials);
                if (SaveCredentials == true)
                {
                    EditorPrefs.SetBool(ShowPasswordInLogKey, ShowPasswordInLog);
                    EditorPrefs.SetString(PasswordKey, Password);
                    EditorPrefs.SetString(VectorStringKey, VectorString);
                }
                else
                {
                    EditorPrefs.SetBool(ShowPasswordInLogKey, false);
                    EditorPrefs.SetString(PasswordKey, string.Empty);
                    EditorPrefs.SetString(VectorStringKey, string.Empty);
                }
            }

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
            EditorPrefs.SetBool(SaveCredentialsKey, false);
            EditorPrefs.SetBool(ShowPasswordInLogKey, false);
            EditorPrefs.SetString(PasswordKey, string.Empty);
            EditorPrefs.SetString(VectorStringKey, string.Empty);
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
