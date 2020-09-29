//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDCredentialsRequired : int
    {
        ForSFTPGenerateForAll,
        ForSFTPGenerateOneOrMore,
        ForSFTPGenerateDev,
        ForSFTPGeneratePreprod,
        ForSFTPGenerateProd,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDProjectCredentialsManagerContent : NWDEditorWindowContent
    {
        const string UNKNOW = "unassigned";
        //-------------------------------------------------------------------------------------------------------------
        enum NWDProjectCredentialsTag
        {
            Standard,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDProjectCredentialsManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The scroll position.
        /// </summary>
        static Vector2 ScrollPosition;
        static public string Password = string.Empty;
        static public string Title = string.Empty;
        static public string VectorString = string.Empty;
        static public string Passphrase = string.Empty;
        static public bool ShowPasswordInLog = false;
        static public bool SaveCredentials = false;
        static public bool PassphraseUsed = true;
        static NWDProjectCredentialsTag Tag = NWDProjectCredentialsTag.Standard;
        bool Loaded = false;
        List<string> ListOfCredential = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectCredentialsManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDProjectCredentialsManagerContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void FlushCredentials(NWDCredentialsRequired sCredentialsType)
        {
            NWDBenchmark.Start();
            Passphrase = string.Empty;
            Password = string.Empty;
            VectorString = string.Empty;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Load()
        {
            NWDBenchmark.Start();
            if (Loaded == false)
            {
                ListOfCredential.Clear();
                foreach (NWDProjectCredentialsTag tValue in Enum.GetValues(typeof(NWDProjectCredentialsTag)))
                {
                    ListOfCredential.Insert((int)tValue, NWDProjectPrefs.GetString(tValue.ToString() + NWDConstants.K_CREDENTIALS_TITLE, UNKNOW));
                }
                //Debug.Log("NWDProjectCredentialsManagerContent LOAD in progress");
                // get values
                Loaded = true;
                Tag = (NWDProjectCredentialsTag)NWDProjectPrefs.GetInt(NWDConstants.K_CREDENTIALS_TAG, 0);
                Title = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_TITLE, UNKNOW);
                SaveCredentials = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SAVE, false);
                ShowPasswordInLog = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, false);
                Password = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSWORD, string.Empty);
                VectorString = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_VECTOR, string.Empty);
                Passphrase = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE, string.Empty);
                PassphraseUsed = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE_USED, false);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReLoad()
        {
            NWDBenchmark.Start();
            SaveCredentials = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SAVE, false);
            ShowPasswordInLog = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, false);
            Password = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSWORD, string.Empty);
            Title = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_TITLE, UNKNOW);
            VectorString = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_VECTOR, string.Empty);
            Passphrase = NWDProjectPrefs.GetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE, string.Empty);
            PassphraseUsed = NWDProjectPrefs.GetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE_USED, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            NWDBenchmark.Start();
            // set values
            NWDProjectPrefs.SetInt(NWDConstants.K_CREDENTIALS_TAG, (int)Tag);
            if (string.IsNullOrEmpty(Title))
            {
                ListOfCredential[(int)Tag] = UNKNOW;
            }
            else
            {
                ListOfCredential[(int)Tag] = Title;
            }
            NWDProjectPrefs.SetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SAVE, SaveCredentials);
            NWDProjectPrefs.SetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE_USED, PassphraseUsed);
            if (SaveCredentials == true)
            {
                NWDProjectPrefs.SetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, ShowPasswordInLog);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE, Passphrase);

                if (string.IsNullOrEmpty(Title))
                {
                    NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_TITLE, UNKNOW);
                }
                else
                {
                    NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_TITLE, Title);
                }

                if (PassphraseUsed == true)
                {
                    if (string.IsNullOrEmpty(Passphrase) == false)
                    {
                        string tPhraseShaOne = NWESecurityTools.GenerateSha(Passphrase);
                        Password = tPhraseShaOne.Substring(0, tPhraseShaOne.Length / 2);
                        VectorString = tPhraseShaOne.Substring(tPhraseShaOne.Length / 2, (tPhraseShaOne.Length / 2) - 1);
                    }
                    else
                    {
                        Password = string.Empty;
                        VectorString = string.Empty;
                    }
                }
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSWORD, Password);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_VECTOR, VectorString);
            }
            else
            {
                NWDProjectPrefs.SetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_SHOW_PASSWORDS_IN_LOG, false);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_TITLE, UNKNOW);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSWORD, string.Empty);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_VECTOR, string.Empty);
                NWDProjectPrefs.SetString(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE, string.Empty);
                NWDProjectPrefs.SetBool(Tag.ToString() + NWDConstants.K_CREDENTIALS_PASSPHRASE_USED, false);
            }
            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Flush()
        {
            NWDBenchmark.Start();
            SaveCredentials = false;
            ShowPasswordInLog = false;
            Password = string.Empty;
            VectorString = string.Empty;
            Passphrase = string.Empty;
            Title = string.Empty;
            Save();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FlushAll()
        {
            NWDBenchmark.Start();
            foreach (NWDProjectCredentialsTag tValue in Enum.GetValues(typeof(NWDProjectCredentialsTag)))
            {
                Tag = tValue;
                Flush();
            }
            Tag = NWDProjectCredentialsTag.Standard;
            Save();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            NWDGUILayout.Title("Credentials for project");
            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            NWDGUILayout.Section("Credentials tag");
            NWDProjectCredentialsTag tTag = (NWDProjectCredentialsTag)EditorGUILayout.Popup("Credential list", (int)Tag, ListOfCredential.ToArray());
            if (tTag != Tag)
            {
                Save();
                Tag = tTag;
                ReLoad();
            }
            EditorGUI.BeginChangeCheck();
            Title = EditorGUILayout.TextField("Credential Title", Title);
            //General preferences
            NWDGUILayout.Section("Credentials preferences");
            SaveCredentials = EditorGUILayout.Toggle("Save credentials", SaveCredentials);
            ShowPasswordInLog = EditorGUILayout.Toggle("Show passwords in log", ShowPasswordInLog);
            //Credentials preferences
            NWDGUILayout.Section("Credentials");
            bool tPassphraseUsed = EditorGUILayout.Toggle("Use passphrase", PassphraseUsed);
            if (tPassphraseUsed != PassphraseUsed)
            {
                if (tPassphraseUsed == false)
                {
                    Password = string.Empty;
                    VectorString = string.Empty;
                }
                PassphraseUsed = tPassphraseUsed;
            }
            if (PassphraseUsed == true)
            {
                Passphrase = EditorGUILayout.PasswordField("General passphrase", Passphrase);
                EditorGUI.BeginDisabledGroup(true);
                Password = EditorGUILayout.PasswordField("General password", Password);
                VectorString = EditorGUILayout.PasswordField("General vector", VectorString);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                Password = EditorGUILayout.PasswordField("General password", Password);
                VectorString = EditorGUILayout.PasswordField("General vector", VectorString);
            }
            //check change
            if (EditorGUI.EndChangeCheck())
            {
                Save();
            }
            //Actions in scroll
            NWDGUILayout.Section("Actions");
            if (GUILayout.Button("Flush"))
            {
                Flush();
            }
            GUILayout.EndScrollView();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDProjectCredentialsManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "credentials-manager/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDProjectCredentialsManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectCredentialsManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectCredentialsManager), ShowAsWindow()) as NWDProjectCredentialsManager;
            }
            NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
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
            NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 300;
            NormalizeHeight = 300;
            // set title
            TitleInit(NWDConstants.K_CREDENTIALS_CONFIGURATION_TITLE, typeof(NWDProjectCredentialsManager));
            NWDProjectCredentialsManagerContent.SharedInstance().Load();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On Disable action.
        /// </summary>
        private void OnDisable()
        {
            NWDBenchmark.Start();
            // for all NWDEditorWindow refresh
            NWDProjectConfigurationManager.Refresh();
            NWDAppConfigurationManager.Refresh();
            NWDAppEnvironmentConfigurationManager.Refresh();
            NWDModelManager.Refresh();
            NWDAppEnvironmentSync.Refresh();
            NWDAppEnvironmentChooser.Refresh();
            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool Checked(NWDCredentialsRequired sCredentialsType)
        {
            NWDBenchmark.Start();
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
            NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsValid(NWDCredentialsRequired sCredentialsType)
        {
            NWDBenchmark.Start();
            bool rReturn = false;
            //NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
            switch (sCredentialsType)
            {
                case NWDCredentialsRequired.ForSFTPGenerateForAll:
                    {
                        rReturn = NWDAppConfiguration.SharedInstance().DevServerIsActive() && NWDAppConfiguration.SharedInstance().PreprodServerIsActive() && NWDAppConfiguration.SharedInstance().ProdServerIsActive();
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGenerateOneOrMore:
                    {
                        rReturn = NWDAppConfiguration.SharedInstance().DevServerIsActive() || NWDAppConfiguration.SharedInstance().PreprodServerIsActive() || NWDAppConfiguration.SharedInstance().ProdServerIsActive();
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGenerateDev:
                    {
                        rReturn = NWDAppConfiguration.SharedInstance().DevServerIsActive();
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGeneratePreprod:
                    {
                        rReturn = NWDAppConfiguration.SharedInstance().PreprodServerIsActive();
                    }
                    break;
                case NWDCredentialsRequired.ForSFTPGenerateProd:
                    {
                        rReturn = NWDAppConfiguration.SharedInstance().ProdServerIsActive();
                    }
                    break;
            }
            NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDProjectCredentialsManagerContent.SharedInstance().OnPreventGUI(position);
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            NWDGUI.BeginRedArea();
            //Actions in window
            if (GUILayout.Button("Flush active credentials and close"))
            {
                NWDProjectCredentialsManagerContent.SharedInstance().Flush();
                Close();
            }
            if (GUILayout.Button("Flush all credentials and close"))
            {
                NWDProjectCredentialsManagerContent.SharedInstance().FlushAll();
                Close();
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
