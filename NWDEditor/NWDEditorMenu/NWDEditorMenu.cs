//=====================================================================================================================
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
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD editor menu.
    /// </summary>
    public abstract class NWDEditorMenu
    {
        //-------------------------------------------------------------------------------------------------------------
#region const
        //-------------------------------------------------------------------------------------------------------------
        const string K_NETWORKEDDATA = "NeWeeDy/";
        const string K_EDITOR = "Project/";
        const string K_DOCUMENTATION = "Documentation/";
        const string K_APPLICATION = "Application/";
        const string K_LOCALIZATION = "Localization/";
        const string K_ENVIRONMENT = "Environment/";
        const string K_CLUSTER = "Cluster/";
        const string K_MODELS = "Models/";
        const string K_TOOLS = "Tools/";
        const string K_ALL_ENVIRONMENT = "All environments/";
        const string K_DEV_ENVIRONMENT = "Dev environment/";
        const string K_PREPROD_ENVIRONMENT = "Preprod environment/";
        const string K_PROD_ENVIRONMENT = "Prod environment/";
        //-------------------------------------------------------------------------------------------------------------
        const string K_WS_REGENERATE = "Regenerate WebServices";
        const string K_WS_NEED_CREDENTIALS = "Need credentials";
        const string K_WS_EDITOR_CREDENTIALS = "Need editor credentials";
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region var
        //-------------------------------------------------------------------------------------------------------------
        static bool kStopRecompile = false;
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Menu
        //-------------------------------------------------------------------------------------------------------------
#region ideMobi
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Developed by idéMobi", false, 1)]
        public static void DevelopedBy()
        {
            if (EditorUtility.DisplayDialog(NWDConstants.K_ALERT_IDEMOBI_TITLE,
                NWDConstants.K_ALERT_IDEMOBI_MESSAGE,
                NWDConstants.K_ALERT_IDEMOBI_OK,
                NWDConstants.K_ALERT_IDEMOBI_SEE_WEBSITE))
            {
            }
            else
            {
                Application.OpenURL(NWDConstants.K_ALERT_IDEMOBI_DOC_HTTP);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Documentation
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION, false, 2)]
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION + "Website", false, 3)]
        public static void Website()
        {
            Application.OpenURL("https://www.net-worked-data.com");
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION + "Tutorial", false, 4)]
        public static void Tutorial()
        {
            Application.OpenURL("https://www.net-worked-data.com");
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Editor
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Project configuration", false, 20)]
        public static void ProjectPreferencesWindow()
        {
            NWDProjectConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Synchronize window", false, 21)]
        public static void SynchronizeWindow()
        {
            NWDAppEnvironmentSync.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Chooser window", false, 22)]
        public static void ChooserWindow()
        {
            NWDAppEnvironmentChooser.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Application
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_APPLICATION + "Application configuration", false, 24)]
        public static void AppConfigurationWindow()
        {
            NWDAppConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Localization
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Localization configuration", false, 25)]
        public static void LocalizationConfigurationWindow()
        {
            NWDLocalizationConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Reorder localized fields", false, 26)]
        public static void LocalizationReorderDatas()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Export localization in CSV", false, 27)]
        public static void LocalizationExportDatas()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Import localization in CSV", false, 28)]
        public static void LocalizationImportDatas()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Environment
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_ENVIRONMENT + "Environment configuration", false, 27)]
        public static void EnvironmentConfigurationWindow()
        {
            NWDAppEnvironmentConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_ENVIRONMENT + "Synchronize window", false, 28)]
        public static void EnvironmentSynchronizeWindow()
        {
            NWDAppEnvironmentSync.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_ENVIRONMENT + "Chooser window", false, 29)]
        public static void EnvironmentChooserWindow()
        {
            NWDAppEnvironmentChooser.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Cluster
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + "Credentials window", false, 40)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + K_WS_NEED_CREDENTIALS, false, 50)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_DEV_ENVIRONMENT + K_WS_NEED_CREDENTIALS, false, 79)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PREPROD_ENVIRONMENT + K_WS_NEED_CREDENTIALS, false, 83)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PROD_ENVIRONMENT + K_WS_NEED_CREDENTIALS, false, 87)]
        public static void ClusterCredentialsWindow()
        {
            NWDProjectCredentialsManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + K_WS_NEED_CREDENTIALS, true, 50)]
        public static bool NeedCredentialsValidMenu()
        {
            return !NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_DEV_ENVIRONMENT + K_WS_NEED_CREDENTIALS, true, 79)]
        public static bool NeedCredentialsValidMenuDev()
        {
            return !NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateDev);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PREPROD_ENVIRONMENT + K_WS_NEED_CREDENTIALS, true, 83)]
        public static bool NeedCredentialsValidMenuPreprod()
        {
            return !NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGeneratePreprod);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PROD_ENVIRONMENT + K_WS_NEED_CREDENTIALS, true, 87)]
        public static bool NeedCredentialsValidMenuProd()
        {
            return !NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateProd);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + "Cluster datas", false, 32)]
        public static void ClusterDatas()
        {
            NWDServerWindow.ShowAndFocusWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + "Increment and generate WebServices", true, 51)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + K_WS_REGENERATE, true, 52)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_DEV_ENVIRONMENT + K_WS_REGENERATE, true, 80)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PREPROD_ENVIRONMENT + K_WS_REGENERATE, true, 84)]
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PROD_ENVIRONMENT + K_WS_REGENERATE, true, 88)]
        public static bool GenerateServersValidMenu()
        {
            return NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + "Increment and generate WebServices", false, 51)]
        public static void GenerateServersGenerateAll()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_ALL_ENVIRONMENT + K_WS_REGENERATE, false, 52)]
        public static void GenerateServersRegenerateAll()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(false, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_DEV_ENVIRONMENT + K_WS_REGENERATE, false, 80)]
        public static void GenerateServersRegenerateDev()
        {
            NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PREPROD_ENVIRONMENT + K_WS_REGENERATE, false, 84)]
        public static void GenerateServersRegeneratePreprod()
        {
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + K_PROD_ENVIRONMENT + K_WS_REGENERATE, false, 88)]
        public static void GenerateServersRegenerateProd()
        {
            NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Models
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new class", false, 800)]
        public static void NewClass()
        {
            NWDEditorNewClass.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new class extension", false, 801)]
        public static void NewClassExtension()
        {
            NWDEditorNewExtension.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new classes window", false, 802)]
        public static void NewWindowExtension()
        {
            NWDEditorNewWindow.SharedInstance();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Models manager", false, 822)]
        public static void ModelsManager()
        {
            NWDModelManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Nodal view", false, 842)]
        public static void NodalView()
        {
            NWDNodeEditor.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "All Datas window", false, 842)]
        public static void AllDatasWindow()
        {
            NWDAllClassesWindow.ShowAndFocusWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
#region Tools
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Password Analyze", false, 9000)]
        public static void PasswordAnalyze()
        {
            NWEPassAnalyseWindow.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "New footer Window", false, 9001)]
        public static void NewFooterWindow()
        {
            NWDEditorFooter.NewFooter();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Cluster Sizer", false, 9031)]
        public static void ClusterClusterSizer()
        {
            NWDClusterSizer.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
#region Special Recompile
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Stop recompile", true)]
        public static bool TestStopRecompile()
        {
            return !kStopRecompile;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Restart recompile", true)]
        public static bool TestUnStopRecompile()
        {
            return kStopRecompile;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Stop recompile", false, 9998)]
        public static void StopRecompile()
        {
            if (kStopRecompile == false)
            {
                kStopRecompile = true;
                EditorApplication.LockReloadAssemblies();
            }
            else
            {
                kStopRecompile = false;
                EditorApplication.UnlockReloadAssemblies();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Restart recompile", false, 9999)]
        public static void UnStopRecompile()
        {
            StopRecompile();
        }
#endregion
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_MENU_IDEMOBI
        // no menu 
#else
        /// <summary>
        /// Idemobis the net worked data info show.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_IDEMOBI, false, 9)]
        public static void IdemobiInfoShow()
        {
            if (EditorUtility.DisplayDialog(NWDConstants.K_ALERT_IDEMOBI_TITLE,
                NWDConstants.K_ALERT_IDEMOBI_MESSAGE,
                NWDConstants.K_ALERT_IDEMOBI_OK,
                NWDConstants.K_ALERT_IDEMOBI_SEE_DOC))
            {
            }
            else
            {
                Application.OpenURL(NWDConstants.K_ALERT_IDEMOBI_DOC_HTTP);
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor NWDPackage preferences window.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_EDITOR_PREFERENCES, false, 20)]
        public static void EditorPreferenceShow()
        {
            NWDProjectConfigurationManager.SharedInstance().ShowUtility();
            NWDProjectConfigurationManager.SharedInstance().Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor by node.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_EDITOR_NODAL, false, 20)]
        public static void EditorNodeShow()
        {
            NWDNodeEditor.SharedInstance();
        }

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor NewClass window.
        /// </summary>
        static NWDEditorNewClass kNWDEditorNewClass;
        [MenuItem(NWDConstants.K_MENU_EDITOR_NEWCLASS, false, 40)]
        public static void EditorNewClassShow()
        {
            if (kNWDEditorNewClass == null)
            {
                kNWDEditorNewClass = EditorWindow.GetWindow(typeof(NWDEditorNewClass)) as NWDEditorNewClass;
            }
            kNWDEditorNewClass.ShowUtility();
            kNWDEditorNewClass.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor New extension window.
        /// </summary>
        static NWDEditorNewExtension kNWDEditorNewExtension;
        [MenuItem(NWDConstants.K_MENU_EDITOR_NEWEXTENSION, false, 80)]
        public static void EditorNewExtensionShow()
        {
            if (kNWDEditorNewExtension == null)
            {
                kNWDEditorNewExtension = EditorWindow.GetWindow(typeof(NWDEditorNewExtension)) as NWDEditorNewExtension;
            }
            kNWDEditorNewExtension.ShowUtility();
            kNWDEditorNewExtension.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor NewClass window.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_EDITOR_FOOTER, false, 10)]
        public static void EditorNewFooter()
        {
            NWDEditorFooter tFooter = EditorWindow.CreateWindow<NWDEditorFooter>();
            tFooter.ShowUtility();
            tFooter.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor NWDPackage preferences window.
        /// </summary>
        static NWDEditorNewWindow kNWDEditorNewWindow;
        [MenuItem(NWDConstants.K_MENU_EDITOR_NEWWINDOW, false, 41)]
        public static void EditorNewWindowShow()
        {
            if (kNWDEditorNewWindow == null)
            {
                kNWDEditorNewWindow = EditorWindow.GetWindow(typeof(NWDEditorNewWindow)) as NWDEditorNewWindow;
            }
            kNWDEditorNewWindow.ShowUtility();
            kNWDEditorNewWindow.Focus();
        }


        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_APP_EDIT, false, 60)]
        public static void AppConfigurationManagerWindowShow()
        {
            NWDAppConfigurationManager.SharedInstanceFocus();
        }

        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_MODEL_MANAGER, false, 63)]
        public static void ModelManagerWindowShow()
        {
            NWDModelManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Menus the method.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT_EDIT, false, 62)]
        public static void AppEnvironmentManagerWindowShow()
        {
            NWDAppEnvironmentConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT, false, 64)]
        public static void EnvironementChooserShow()
        {
            NWDAppEnvironmentChooser.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT_SYNC, false, 65)]
        public static void EnvironementSyncShow()
        {
            NWDAppEnvironmentSync.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        // LOOP FOR NWDCLASSES MENU 1000 ... 9000
        //-------------------------------------------------------------------------------------------------------------
        // CREATE FILE
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_ERRORS_AND_MESSAGES, false, 9001)]
        public static void CreateErrorsAndMessages()
        {
            NWDDataManager.SharedInstance().CreateErrorsAndMessagesAllClasses();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_FILES, false, 9002)]
        public static void CreatePHP()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(true, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_FILES_NO_INCREMENT_WS, false, 9003)]
        public static void CreatePHPWitoutIncrement()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(false, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_EXPORT_WEB_SITE, false, 9020)]
        public static void CreateWebsitesPHP()
        {
            //          NWDFindPackage.SharedInstance ();
            NWDDataManager.SharedInstance().ExportWebSites();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_FILES_SFTP, false, 9002)]
        public static void CreatePHP_SFTP()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_FILES_NO_INCREMENT_WS_SFTP, false, 9003)]
        public static void CreatePHPWitoutIncrement_SFTP()
        {
            NWDDataManager.SharedInstance().CreatePHPAllClass(false, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_MODELS_RESET, false, 9001)]
        public static void ModelsReset()
        {
            NWDDataManager.SharedInstance().ModelResetAllClass();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREDENTIALS, false, 1000)]
        public static void EditorCredentialsManager()
        {
            NWDProjectCredentialsManager.SharedInstance().Show();
            NWDProjectCredentialsManager.SharedInstance().Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        //		[MenuItem (NWDConstants.K_MENU_CREATE_CSHARP_FILES, false, 9022)]
        //		public static void CreateCSharp ()
        //		{
        //			NWDDataManager.SharedInstance().CreateCShapAllClass();
        //		}
        //-------------------------------------------------------------------------------------------------------------
        // LOCALIZATION
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_LOCALIZATION, false, 9052)]
        //public static void LocalizationConfig()
        //{
        //    EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDDataLocalizationManagerWindow));
        //    tWindow.Show();
        //    tWindow.Focus();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCALIZATION_CONFIG, false, 9054)]
        //public static void DataLocalizationManager()
        //{
        //    NWDLocalizationConfigurationManager.SharedInstance().Show();
        //    NWDLocalizationConfigurationManager.SharedInstance().Focus();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCALIZATION_REORDER, false, 9055)]
        //public static void LocalizationReorder()
        //{
        //    NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCALIZATION_EXPORT, false, 9070)]
        //public static void LocalizationExport()
        //{
        //    NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCALIZATION_IMPORT, false, 9071)]
        //public static void LocalizationImport()
        //{
        //    NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
        //}
        //-------------------------------------------------------------------------------------------------------------
        // ZIP DATA BASE 
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_SAVE_DATABASE, false, 9071)]
        public static void SaveDatabase()
        {
            DateTime tDateTime = DateTime.UtcNow;
            string tDateTimeFormated = tDateTime.Year + "-" + tDateTime.Month + "-" + tDateTime.Day + "_" + tDateTime.Hour + "-" + tDateTime.Minute + "-" + tDateTime.Second + "_UTC";
            string tBDDName = NWD.K_EditorDatabaseName.Replace(".prp", "_" + tDateTimeFormated + ".prp-save");

            string tPathOriginal = NWD.K_Assets + "/" + NWD.K_StreamingAssets + "/" + NWD.K_EditorDatabaseName;
            string tPathFinal = "DatabaseSaved/" + tBDDName;

            if (!Directory.Exists("DatabaseSaved"))
            {
                Directory.CreateDirectory("DatabaseSaved");
            }

            FileUtil.ReplaceFile(tPathOriginal, tPathFinal);
            EditorUtility.DisplayDialog("Database", "Database saved in directory: \"DatabaseSaved\"\n\n" + tBDDName + "\n\nDate: " + tDateTimeFormated, "Ok");
        }
        //-------------------------------------------------------------------------------------------------------------
        // DEV 
#region DEV
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_SFTP_WEBSERVICE, false, 9100)]
        public static void DevCreatePHPWitoutIncrement_SFTP()
        {
            NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_CREATE_TABLES, false, 9101)]
        public static void DevCreateTablesServer()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().DevEnvironment, true, true, NWDOperationSpecial.Upgrade);
            //OperationSynchroAllTable(sEnvironment, true, true, sOperation);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_SYNCHRONIZE_DATAS, false, 9102)]
        public static void DevSynchronizeAllData()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().DevEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_FORCE_SYNCHRONIZE, false, 9103)]
        public static void DevForceSynchronizeAllData()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().DevEnvironment, true, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_RESET_CONNEXION, false, 9140)]
        public static void DevResetUuidAndToken()
        {
            NWDAppEnvironmentSync.SharedInstance().Reset(NWDAppConfiguration.SharedInstance().DevEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEV_FLUSH_CONNEXION, false, 9141)]
        public static void DevFlushQueue()
        {
            NWDAppEnvironmentSync.SharedInstance().Flush(NWDAppConfiguration.SharedInstance().DevEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        // 
#endregion
        //PREPROD
#region PREPROD
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_SFTP_WEBSERVICE, false, 9100)]
        public static void PreprodCreatePHPWitoutIncrement_SFTP()
        {
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_CREATE_TABLES, false, 9104)]
        public static void PreprodCreateTablesServer()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().PreprodEnvironment, true, true, NWDOperationSpecial.Upgrade);
            //OperationSynchroAllTable(sEnvironment, true, true, sOperation);

        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_SYNCHRONIZE_DATAS, false, 9105)]
        public static void PreprodSynchronizeAllData()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_FORCE_SYNCHRONIZE, false, 9106)]
        public static void PreprodForceSynchronizeAllData()
        {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().PreprodEnvironment, true, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_RESET_CONNEXION, false, 9142)]
        public static void PreprodFlushQueue()
        {
            NWDAppEnvironmentSync.SharedInstance().Reset(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PREPROD_FLUSH_CONNEXION, false, 9143)]
        public static void PreprodResetUuidAndToken()
        {
            NWDAppEnvironmentSync.SharedInstance().Flush(NWDAppConfiguration.SharedInstance().DevEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //PROD
#region PROD
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_PROD, false, 9103)]
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_SFTP_WEBSERVICE, false, 9100)]
        public static void ProdCreatePHPWitoutIncrement_SFTP()
        {
            NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_CREATE_TABLES, false, 9107)]
        public static void ProdCreateTablesServer()
        {
            //				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
            //					    NWDConstants.K_SYNC_ALERT_MESSAGE,
            //					    NWDConstants.K_SYNC_ALERT_OK,
            //				NWDConstants.K_SYNC_ALERT_CANCEL)) {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().ProdEnvironment, true, true, NWDOperationSpecial.Upgrade);
            //OperationSynchroAllTable(sEnvironment, true, true, sOperation);

            //				}
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_SYNCHRONIZE_DATAS, false, 9108)]
        public static void ProdSynchronizeAllData()
        {
            //				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
            //					    NWDConstants.K_SYNC_ALERT_MESSAGE,
            //					    NWDConstants.K_SYNC_ALERT_OK,
            //				NWDConstants.K_SYNC_ALERT_CANCEL)) {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            //				}
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_FORCE_SYNCHRONIZE, false, 9109)]
        public static void ProdForceSynchronizeAllData()
        {
            //				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
            //					NWDConstants.K_SYNC_ALERT_MESSAGE,
            //					NWDConstants.K_SYNC_ALERT_OK,
            //				NWDConstants.K_SYNC_ALERT_CANCEL)) {
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(NWDAppConfiguration.SharedInstance().ProdEnvironment, true, true);
            //				}
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_RESET_CONNEXION, false, 9144)]
        public static void ProdResetUuidAndToken()
        {
            NWDAppEnvironmentSync.SharedInstance().Reset(NWDAppConfiguration.SharedInstance().ProdEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_PROD_FLUSH_CONNEXION, false, 9145)]
        public static void ProdFlushQueue()
        {
            NWDAppEnvironmentSync.SharedInstance().Flush(NWDAppConfiguration.SharedInstance().ProdEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //LOCALS
#region LOCAL
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_LOCAL, false, 9200)]
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CREATE_TABLES, false, 9201)]
        public static void CreateTables()
        {
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tBasisHelper = NWDBasisHelper.FindTypeInfos(tType);
                tBasisHelper.DropTable();
            }

            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();

            NWDDataManager.SharedInstance().ReloadAllObjects(NWDBundle.ALL);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RECREATE_INDEX_TABLE, false, 9201)]
        public static void RecreateAllIndexForAllTables()
        {
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tBasisHelper = NWDBasisHelper.FindTypeInfos(tType);
                tBasisHelper.RecreateAllIndexForTable();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RELOAD_DATAS, false, 9202)]
        public static void ReloadAllDatas()
        {
            NWDDataManager.SharedInstance().ReloadAllObjects(NWDBundle.ALL);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CLEAN_TRASHED_DATAS, false, 9203)]
        public static void PurgeAllDatas()
        {
            //NWDDataManager.SharedInstance().CleanAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CleanAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects(NWDBundle.ALL);
        }
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_PURGE_DATAS, false, 9203)]
        //public static void CleanAllDatas()
        //{
        //    NWDDataManager.SharedInstance().PurgeAllTablesLocalAccount();
        //    NWDDataManager.SharedInstance().PurgeAllTablesLocalEditor();
        //    NWDDataManager.SharedInstance().ReloadAllObjects(NWDBundle.ALL);
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_UPDATE_DATAS, false, 9204)]
        //public static void UpdateAllDatas()
        //{
        //    NWDDataManager.SharedInstance().UpdateAllTablesLocalAccount();
        //    NWDDataManager.SharedInstance().UpdateAllTablesLocalEditor();
        //    NWDDataManager.SharedInstance().ReloadAllObjects();
        //}
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RESET_DATAS, false, 9205)]
        public static void ResetAllDatas()
        {
            //NWDDataManager.SharedInstance().ResetAllTablesLocalAccount();
            NWDDataManager.SharedInstance().ResetAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects(NWDBundle.ALL);
        }
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_DECONNECT_ACCOUNT_DATAS, false, 9303)]
        //public static void DeconnectAccountDataBase()
        //{
        //    NWDLauncher.DeconnectFromDatabaseAccount();
        //    //NWDLauncher.Launch();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_DELETEACCOUNTE_DATAS, false, 9304)]
        //public static void DeleetAccountDataBase()
        //{
        //    if (EditorUtility.DisplayDialog("DELETE ACCOUNT DATABASE", "YOU WILL DELETE ACCOUNT DATABASE! ARE YOU SURE?", "DELETE!", "CANCEL"))
        //    {
        //        NWDLauncher.DeleteDatabaseAccount();
        //        //NWDLauncher.Launch();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_CREATE_DATAS, false, 9305)]
        //public static void CreateAllDataBases()
        //{
        //    NWDDataManager.SharedInstance().RecreateDatabase();
        //    NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
        //    NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
        //    NWDDataManager.SharedInstance().ReloadAllObjects();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDConstants.K_MENU_LOCAL_CREATE_DATAS_AND_PASS, false, 9305)]
        //public static void CreateAllDataBasesAndPass()
        //{
        //    NWDDataManager.SharedInstance().RecreateDatabase(true, true);
        //    NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
        //    NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
        //    NWDDataManager.SharedInstance().ReloadAllObjects();
        //}

        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_INTEGRITY_TO_TRASHED, false, 9355)]
        public static void InterigrityErrorToTrahs()
        {
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tBasisHelper = NWDBasisHelper.FindTypeInfos(tType);

                // TODO : Change to remove invoke!
                //var tMethodInfo = tType.GetMethod("Datas", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.GetBasisHelper);
                //if (tMethodInfo != null)
                //{
                //NWDBasisHelper tBasisHelper = (NWDBasisHelper)tMethodInfo.Invoke(null, null);
                if (tBasisHelper != null)
                {
                    foreach (NWDTypeClass tData in tBasisHelper.Datas)
                    {
                        if (tData.IntegrityIsValid() == false)
                        {
                            tData.TrashAction();
                        }
                    }
                }
                //}
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_REINTEGRITATE_ALL_DATAS, false, 9355)]
        public static void ReinterigrityAllDatas()
        {
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tBasisHelper = NWDBasisHelper.FindTypeInfos(tType);

                // TODO : Change to remove invoke!
                //var tMethodInfo = tType.GetMethod("Datas", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.GetBasisHelper);
                //if (tMethodInfo != null)
                //{
                //NWDBasisHelper tBasisHelper = (NWDBasisHelper)tMethodInfo.Invoke(null, null);
                if (tBasisHelper != null)
                {
                    foreach (NWDTypeClass tObject in tBasisHelper.Datas)
                    {
                        tObject.UpdateData();
                    }
                }
                //}
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
        }
#endregion
        //-------------------------------------------------------------------------------------------------------------
        static bool kBlock = false;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_BLOCK_RECOMPILE, false, 9998)]
        public static void BlockRecompile()
        {
            if (kBlock == false)
            {
                kBlock = true;
                EditorApplication.LockReloadAssemblies();
            }
            else
            {
                kBlock = false;
                EditorApplication.UnlockReloadAssemblies();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_BLOCK_RECOMPILE, true)]
        public static bool TestBlockRecompile()
        {
            return !kBlock;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_UNBLOCK_RECOMPILE, false, 9999)]
        public static void UnblockRecompile()
        {
            BlockRecompile();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_UNBLOCK_RECOMPILE, true)]
        public static bool TestUnblockRecompile()
        {
            return kBlock;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "/SPECIAL/extract list of file", false, 9999)]
        public static void ExtractListOfFiles()
        {
            string tInitialPath = NWDFindPackage.PathOfPackage();
            List<string> tResult = FolderFiles(tInitialPath);
            StringBuilder tString = new StringBuilder();
            foreach (string tPath in tResult)
            {
                tString.AppendLine(tPath);
            }
            GUIUtility.systemCopyBuffer = tString.ToString();
            Debug.Log(tString.ToString());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<string> FolderFiles(string sFolder, string sSubFolder = "", string sSubTab = "")
        {
            string tFolder = sFolder + "/" + sSubFolder;
            tFolder = tFolder.Replace("//", "/");
            tFolder = tFolder.TrimEnd(new char[] { '/' });
            DirectoryInfo tDirectory = new DirectoryInfo(tFolder);
            FileInfo[] tInfo = tDirectory.GetFiles("*.*");
            List<string> rList = new List<string>();
            foreach (FileInfo tFile in tInfo)
            {
                //if (tFile.Extension != ".meta" && tFile.Name[0] != '.')
                    if (tFile.Extension == ".cs" )
                    {
                    rList.Add(sSubTab + "/" + tFile.Name);
                }
            }
            DirectoryInfo[] tSubFoldersArray = tDirectory.GetDirectories();
            foreach (DirectoryInfo tSubFolder in tSubFoldersArray)
            {
                rList.Add(sSubTab + "/" + tSubFolder.Name);
                rList.AddRange(FolderFiles(sFolder, sSubFolder + "/" + tSubFolder.Name, sSubTab + "\t"));
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "/SPECIAL/List of trigrammes and classes", false, 4000)]
        public static void EditorListOfTrigramme()
        {
            Dictionary<string, string> tListClassesTrigramme = new Dictionary<string, string>(new StringIndexKeyComparer());
            StringBuilder tText = new StringBuilder();
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tData = NWDBasisHelper.FindTypeInfos(tType);
                tListClassesTrigramme.Add(tData.ClassNamePHP, tData.ClassTrigramme);
                tText.AppendLine(tData.ClassNamePHP + "\t" + tData.ClassTrigramme);
            }
            GUIUtility.systemCopyBuffer = tText.ToString();
            Debug.Log(tText.ToString());
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_DEBUG_FOLDER, false, 8998)]
        public static void ShowDebugFolder()
        {
            string tPath = Application.persistentDataPath + "/Unity";
            EditorUtility.RevealInFinder(tPath);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_UNITTEST_CLEAN, false, 8999)]
        public static void CleanUnitTest()
        {
            NWDUnitTests.CleanUnitTests(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_REINDEX_ALL_DATAS, false, 9998)]
        public static void ReindexAllDatas()
        {
            NWDBenchmark.Start();
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.IndexInBaseAllObjects();
                tHelper.IndexInMemoryAllObjects();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_FLUSH_ACCOUNT_ALL_DATAS, false, 9999)]
        public static void FLUSH_ACCOUNT_ALL_DATAS()
        {
            NWDBenchmark.Start();
            NWDDataInspector.InspectNetWorkedData(null, true, false);
            if (EditorUtility.DisplayDialog("WARNING", "YOU WILL DELETE ALL DATAS OF PLAYERS!", "YES", "CANCEL"))
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.ResetPreferences();
                List<string> tTableList = new List<string>();
                // reset database on local...and add for cluster
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    if (tHelper.TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                    {
                        tHelper.SetObjectInEdition(null);
                        tHelper.FlushTable();
                        tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().DevEnvironment));
                    }
                }
                // reset database on cluster...
                foreach (NWDServerDatas tDatabaseServer in NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas)).Datas)
                {
                    if (tDatabaseServer.Dev == true)
                    {
                        List<string> tCommandList = new List<string>();
                        foreach (string tTable in tTableList)
                        {
                            tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                            tCommandList.Add("mysql -u root -p\"" + tDatabaseServer.Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + tDatabaseServer.MySQLBase + "; DELETE FROM " + tDatabaseServer.MySQLBase + "." + tTable + ";\"");
                        }
                        NWDServer tServer = tDatabaseServer.Server.GetRawData();
                        tServer.ExecuteSSH("FlushAccount", tCommandList);
                    }
                }
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif