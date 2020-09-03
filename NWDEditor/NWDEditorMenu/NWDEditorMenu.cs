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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
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
using NetWorkedData.MacroDefine;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
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
        public const string K_NETWORKEDDATA = "NetWorkedData/";
        public const string K_EDITOR = "Project/";
        public const string K_DOCUMENTATION = "Documentation/";
        public const string K_APPLICATION = "Application/";
        public const string K_LOCALIZATION = "Localization/";
        public const string K_ENVIRONMENT = "Environment/";
        public const string K_CLUSTER = "Cluster/";
        public const string K_MODELS = "Create models/";
        public const string K_TOOLS = "Tools/";
        public const string K_ALL_ENVIRONMENT = "All environments/";
        public const string K_DEV_ENVIRONMENT = "Dev environment/";
        public const string K_PREPROD_ENVIRONMENT = "Preprod environment/";
        public const string K_PROD_ENVIRONMENT = "Prod environment/";
        //-------------------------------------------------------------------------------------------------------------
        const string K_WS_REGENERATE = "Regenerate webservices";
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
        public const int K_ENGINE_MANAGEMENT_INDEX = 800;
        public const int K_PLAYER_MANAGEMENT_INDEX = 900;
        public const int K_MODULES_MANAGEMENT_INDEX = 1000;
        public const int K_CUSTOMS_MANAGEMENT_INDEX = 2000;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Engine management", true, K_ENGINE_MANAGEMENT_INDEX)]
        public static bool EngineModelsFalse()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Engine management", false, K_ENGINE_MANAGEMENT_INDEX)]
        public static void EngineModels()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Players management", true, K_PLAYER_MANAGEMENT_INDEX)]
        public static bool PlayersModelsFalse()
        {
            return false;
        }//-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Players management", false, K_PLAYER_MANAGEMENT_INDEX)]
        public static void PlayersModels()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Modules management", true, K_MODULES_MANAGEMENT_INDEX)]
        public static bool ModulesModelsFalse()
        {
            return false;
        }//-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Modules management", false, K_MODULES_MANAGEMENT_INDEX)]
        public static void ModulesModels()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Customs management", true, K_CUSTOMS_MANAGEMENT_INDEX-1)]
        public static bool CustomsModelsFalse()
        {
            return false;
        }//-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Customs management", false, K_CUSTOMS_MANAGEMENT_INDEX-1)]
        public static void CustomsModels()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #region ideMobi
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + "Developed by idéMobi", false, 0)]
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
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_DOCUMENTATION + "Version " + NWDEngineVersion.Version, true, 10)]
        public static bool VersionShowDisable()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_DOCUMENTATION + "Version " + NWDEngineVersion.Version, false, 10)]
        public static void VersionShow()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION, false, 2)]
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION + "Website", false, 23)]
        public static void Website()
        {
            Application.OpenURL("https://www.net-worked-data.com");
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION + "Tutorial", false, 24)]
        public static void Tutorial()
        {
            Application.OpenURL("https://www.net-worked-data.com");
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_DOCUMENTATION + "Documentation online", false, 44)]
        public static void Doxygne()
        {
            Application.OpenURL("https://net-worked-data.com/Documentation/NetWorkedData/html/index.html");
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
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Macro configuration", false, 20)]
        public static void MacroPreferencesWindow()
        {
            MDEMacroDefineEditor.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Synchronize datas window", false, 41)]
        [MenuItem(K_NETWORKEDDATA + K_ENVIRONMENT + "Synchronize datas window", false, 48)]
        public static void SynchronizeWindow()
        {
            NWDAppEnvironmentSync.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_EDITOR + "Environment chooser window", false, 42)]
        [MenuItem(K_NETWORKEDDATA + K_ENVIRONMENT + "Environment chooser window", false, 49)]
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
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Reorder localized fields", false, 46)]
        public static void LocalizationReorderDatas()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Export localization in CSV", false, 57)]
        public static void LocalizationExportDatas()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_LOCALIZATION + "Import localization in CSV", false, 58)]
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
        #endregion
        #region Cluster
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + "Credentials window", false, 105)]
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
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + "Cluster configuration", false, 32)]
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
        [MenuItem(K_NETWORKEDDATA + K_CLUSTER + "Cluster sizer window", false, 100)]
        public static void ClusterClusterSizer()
        {
            NWDClusterSizer.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Models
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new class", false, 160)]
        public static void NewClass()
        {
            NWDEditorNewClass.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new class extension", false, 161)]
        public static void NewClassExtension()
        {
            NWDEditorNewExtension.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Create new classes window", false, 162)]
        public static void NewWindowExtension()
        {
            NWDEditorNewWindow.SharedInstance();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Models management", false, 185)]
        public static void ModelsManager()
        {
            NWDModelManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "Nodal window", false, 192)]
        public static void NodalView()
        {
            NWDNodeEditor.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_MODELS + "All datas window", false, 192)]
        public static void AllDatasWindow()
        {
            NWDAllClassesWindow.ShowAndFocusWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Tools
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Password analyzer window", false, 200)]
        public static void PasswordAnalyze()
        {
            NWEPassAnalyseWindow.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "New footer window", false, 201)]
        public static void NewFooterWindow()
        {
            NWDEditorFooter.NewFooter();
        }
        //-------------------------------------------------------------------------------------------------------------
        #region Special Recompile
        #if NWD_DEVELOPER
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
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Stop recompile", false, 298)]
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
        [MenuItem(K_NETWORKEDDATA + K_TOOLS + "Restart recompile", false, 299)]
        public static void UnStopRecompile()
        {
            StopRecompile();
        }
        #endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif