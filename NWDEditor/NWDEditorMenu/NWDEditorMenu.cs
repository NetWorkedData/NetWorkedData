// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
    public class NWDEditorMenu
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="NetWorkedData.NWDEditorMenu"/> class.
        /// </summary>
        public NWDEditorMenu()
        {
            //NWDDataManager.SharedInstance().ConnectToDatabase ();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_MENU_IDEMOBI
        // no menu 
#else
		/// <summary>
		/// Idemobis the net worked data info show.
		/// </summary>
		[MenuItem (NWDConstants.K_MENU_IDEMOBI, false, 9)]
		public static void IdemobiInfoShow()
		{
			if (EditorUtility.DisplayDialog (NWDConstants.K_ALERT_IDEMOBI_TITLE,
				NWDConstants.K_ALERT_IDEMOBI_MESSAGE,
				NWDConstants.K_ALERT_IDEMOBI_OK,
				NWDConstants.K_ALERT_IDEMOBI_SEE_DOC)) {
			} else {
				Application.OpenURL (NWDConstants.K_ALERT_IDEMOBI_DOC_HTTP);
			};
		}
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD editor NWDPackage preferences window.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_EDITOR_PREFERENCES, false, 20)]
        public static void EditorPreferenceShow()
        {
            NWDEditorConfigurationManager.SharedInstance().ShowUtility();
            NWDEditorConfigurationManager.SharedInstance().Focus();
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
        /// <summary>
        /// Menus the method.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT_EDIT, false, 60)]
        public static void AppEnvironmentManagerWindowShow()
        {
            NWDAppEnvironmentConfigurationManager.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD app configuration window.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT, false, 61)]
        public static void EnvironementChooserShow()
        {
            NWDAppEnvironmentChooser.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// TheNWD app configuration window.
        /// </summary>
        [MenuItem(NWDConstants.K_MENU_ENVIRONMENT_SYNC, false, 62)]
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
        [MenuItem(NWDConstants.K_MENU_LOCALIZATION_CONFIG, false, 9054)]
        public static void DataLocalizationManager()
        {
            NWDLocalizationConfigurationManager.SharedInstance().Show();
            NWDLocalizationConfigurationManager.SharedInstance().Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCALIZATION_REORDER, false, 9055)]
        public static void LocalizationReorder()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCALIZATION_EXPORT, false, 9070)]
        public static void LocalizationExport()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCALIZATION_IMPORT, false, 9071)]
        public static void LocalizationImport()
        {
            NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
        }
        //-------------------------------------------------------------------------------------------------------------
        // ZIP DATA BASE 
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_SAVE_DATABASE, false, 9071)]
        public static void SaveDatabase()
        {
            DateTime tDateTime = DateTime.UtcNow;
            string tDateTimeFormated = tDateTime.Year + "-" + tDateTime.Month + "-" + tDateTime.Day + "_" + tDateTime.Hour + "-" + tDateTime.Minute + "-" + tDateTime.Second + "_UTC";
            string tBDDName = NWDDataManager.SharedInstance().DatabaseNameEditor.Replace(".prp", "_" + tDateTimeFormated + ".prp-save");

            string tPathOriginal = "Assets/" + NWDDataManager.SharedInstance().DatabasePathEditor + "/" + NWDDataManager.SharedInstance().DatabaseNameEditor;
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
            NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false);
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
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false);
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
            NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false);
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
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RELOAD_DATAS, false, 9202)]
        public static void ReloadAllDatas()
        {
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CLEAN_TRASHED_DATAS, false, 9203)]
        public static void PurgeAllDatas()
        {
            NWDDataManager.SharedInstance().CleanAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CleanAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_PURGE_DATAS, false, 9203)]
        public static void CleanAllDatas()
        {
            NWDDataManager.SharedInstance().PurgeAllTablesLocalAccount();
            NWDDataManager.SharedInstance().PurgeAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_UPDATE_DATAS, false, 9204)]
        public static void UpdateAllDatas()
        {
            NWDDataManager.SharedInstance().UpdateAllTablesLocalAccount();
            NWDDataManager.SharedInstance().UpdateAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RESET_DATAS, false, 9205)]
        public static void ResetAllDatas()
        {
            NWDDataManager.SharedInstance().ResetAllTablesLocalAccount();
            NWDDataManager.SharedInstance().ResetAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_DECONNECT_ACCOUNT_DATAS, false, 9303)]
        public static void DeconnectAccountDataBase()
        {
            NWDLauncher.DeconnectFromDatabaseAccount();
            //NWDLauncher.Launch();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_DELETEACCOUNTE_DATAS, false, 9304)]
        public static void DeleetAccountDataBase()
        {
            if (EditorUtility.DisplayDialog("DELETE ACCOUNT DATABASE", "YOU WILL DELETE ACCOUNT DATABASE! ARE YOU SURE?", "DELETE!", "CANCEL"))
            {
                NWDLauncher.DeleteDatabaseAccount();
                //NWDLauncher.Launch();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CREATE_DATAS, false, 9305)]
        public static void CreateAllDataBases()
        {
            NWDDataManager.SharedInstance().RecreateDatabase();
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CREATE_DATAS_AND_PASS, false, 9305)]
        public static void CreateAllDataBasesAndPass()
        {
            NWDDataManager.SharedInstance().RecreateDatabase(true, true);
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }

        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_INTEGRITY_TO_TRASHED, false, 9355)]
        public static void InterigrityErrorToTrahs()
        {
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
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
                            if (tObject.DataIntegrityState() == false)
                            {
                                tObject.TrashAction();
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
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
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
                //AssetDatabase.StartAssetEditing();
            }
            else
            {
                kBlock = false;
                //AssetDatabase.StopAssetEditing();
                //AssetDatabase.Refresh();
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
                if (tFile.Extension != ".meta" && tFile.Name[0] != '.')
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
            Dictionary<string, string> tListClassesTrigramme = new Dictionary<string, string>();
            StringBuilder tText = new StringBuilder();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                NWDBasisHelper tData = NWDBasisHelper.FindTypeInfos(tType);
                tListClassesTrigramme.Add(tData.ClassNamePHP, tData.ClassTrigramme);
                tText.AppendLine(tData.ClassNamePHP + "\t" + tData.ClassTrigramme);
            }
            GUIUtility.systemCopyBuffer = tText.ToString();
            Debug.Log(tText.ToString());
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif