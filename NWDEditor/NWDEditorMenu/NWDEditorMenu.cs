//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
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
		public NWDEditorMenu ()
		{
			NWDDataManager.SharedInstance().ConnectToDatabase ();
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
		static NWDEditorPreferences kNWDEditorPreferences;
		[MenuItem (NWDConstants.K_MENU_EDITOR_PREFERENCES, false, 20)]
		public static void EditorPreferenceShow()
		{
			if (kNWDEditorPreferences == null) {
				kNWDEditorPreferences = (NWDEditorPreferences)ScriptableObject.CreateInstance <NWDEditorPreferences> ();
			}
			kNWDEditorPreferences.ShowUtility ();
			kNWDEditorPreferences.Focus ();
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
		[MenuItem (NWDConstants.K_MENU_EDITOR_NEWCLASS, false, 40)]
		public static void EditorNewClassShow()
		{
			if (kNWDEditorNewClass == null) {
				kNWDEditorNewClass = (NWDEditorNewClass)ScriptableObject.CreateInstance <NWDEditorNewClass> ();
			}
			kNWDEditorNewClass.ShowUtility ();
			kNWDEditorNewClass.Focus ();
		}


		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TheNWD editor NWDPackage preferences window.
		/// </summary>
		static NWDEditorNewWindow kNWDEditorNewWindow;
		[MenuItem (NWDConstants.K_MENU_EDITOR_NEWWINDOW, false, 41)]
		public static void EditorNewWindowShow()
		{
			if (kNWDEditorNewWindow == null) {
				kNWDEditorNewWindow = (NWDEditorNewWindow)ScriptableObject.CreateInstance <NWDEditorNewWindow> ();
			}
			kNWDEditorNewWindow.ShowUtility ();
			kNWDEditorNewWindow.Focus ();
		}


		//-------------------------------------------------------------------------------------------------------------
		static NWDAppEnvironmentManagerWindow kNWDAppEnvironmentManagerWindow;
		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT_EDIT, false, 60)]
		/// <summary>
		/// Menus the method.
		/// </summary>
		public static void AppEnvironmentManagerWindowShow ()
		{
			if (kNWDAppEnvironmentManagerWindow == null) {
				kNWDAppEnvironmentManagerWindow = (NWDAppEnvironmentManagerWindow)ScriptableObject.CreateInstance <NWDAppEnvironmentManagerWindow> ();
			}
			kNWDAppEnvironmentManagerWindow.ShowUtility ();
			kNWDAppEnvironmentManagerWindow.Focus ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TheNWD app configuration window.
		/// </summary>
		public static NWDAppEnvironmentChooser kNWDAppEnvironmentChooser;
		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT, false, 61)]
		public static void EnvironementChooserShow()
		{
			if (kNWDAppEnvironmentChooser == null) {
//				kNWDAppEnvironmentChooser = (NWDAppEnvironmentChooser)ScriptableObject.CreateInstance <NWDAppEnvironmentChooser> ();
				kNWDAppEnvironmentChooser = EditorWindow.GetWindow (typeof(NWDAppEnvironmentChooser)) as NWDAppEnvironmentChooser;
			}
			kNWDAppEnvironmentChooser.ShowUtility ();
			kNWDAppEnvironmentChooser.Focus ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TheNWD app configuration window.
		/// </summary>
		public static NWDAppEnvironmentSync kNWDAppEnvironmentSync;
		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT_SYNC, false, 62)]
		public static void EnvironementSyncShow()
		{
			if (kNWDAppEnvironmentSync == null) {
				//kNWDAppEnvironmentSync = (NWDAppEnvironmentSync)ScriptableObject.CreateInstance <NWDAppEnvironmentSync> ();
				kNWDAppEnvironmentSync = EditorWindow.GetWindow (typeof(NWDAppEnvironmentSync)) as NWDAppEnvironmentSync;
			}
			kNWDAppEnvironmentSync.ShowUtility ();
			kNWDAppEnvironmentSync.Focus ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDAppEnvironmentSync EnvironementSync()
		{
			if (kNWDAppEnvironmentSync == null) {
				//kNWDAppEnvironmentSync = (NWDAppEnvironmentSync)ScriptableObject.CreateInstance <NWDAppEnvironmentSync> ();
				kNWDAppEnvironmentSync = EditorWindow.GetWindow (typeof(NWDAppEnvironmentSync)) as NWDAppEnvironmentSync;
			}
			kNWDAppEnvironmentSync.ShowUtility ();
			kNWDAppEnvironmentSync.Focus ();
			return kNWDAppEnvironmentSync;
		}

		//-------------------------------------------------------------------------------------------------------------
		// Loop for classes menu
		//-------------------------------------------------------------------------------------------------------------
		// CREATE FILE
		//-------------------------------------------------------------------------------------------------------------
//		[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_CREATE, false, 9000)]
//		public static void CreateFiles ()
//		{
        //		}
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_CREATE_PHP_ERRORS, false, 9001)]
        public static void CreateErrors()
        {
            NWDDataManager.SharedInstance().CreateErrorAllClass();
        }
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_CREATE_PHP_FILES, false, 9002)]
		public static void CreatePHP ()
		{
			NWDDataManager.SharedInstance().CreatePHPAllClass();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_CREATE_PHP_EXPORT_WEB_SITE, false, 9020)]
		public static void CreateWebsitesPHP ()
		{
//			NWDFindPackage.SharedInstance ();
			NWDDataManager.SharedInstance().ExportWebSites();
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
		[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_LOCALIZATION, false, 9052)]
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_CONFIG, false, 9052)]
		//-------------------------------------------------------------------------------------------------------------
		public static void LocalizationConfig ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDDataLocalizationManagerWindow));
			tWindow.Show ();
			tWindow.Focus ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_REORDER, false, 9053)]
		public static void LocalizationReorder ()
		{
			NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_EXPORT, false, 9070)]
		public static void LocalizationExport ()
		{
			NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_IMPORT, false, 9071)]
		public static void LocalizationImport ()
		{
			NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV ();
		}
		//-------------------------------------------------------------------------------------------------------------
		// DEV 
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_DEV, false, 9101)]
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_CREATE_TABLES, false, 9101)]
		public static void DevCreateTablesServer ()
		{
			EnvironementSync().CreateTable(NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_SYNCHRONIZE_DATAS, false, 9121)]
		public static void DevSynchronizeAllData ()
		{
			EnvironementSync().AllSynchronization (NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_FORCE_SYNCHRONIZE, false, 9122)]
		public static void DevForceSynchronizeAllData ()
		{
			EnvironementSync().AllSynchronizationForce (NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_RESET_CONNEXION, false, 9140)]
		public static void DevResetUuidAndToken ()
		{
			EnvironementSync().Reset (NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_FLUSH_CONNEXION, false, 9141)]
		public static void DevFlushQueue ()
		{
			EnvironementSync().Flush (NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		// PREPROD
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_PREPROD, false, 9102)]
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_CREATE_TABLES, false, 9101)]
		public static void PreprodCreateTablesServer ()
		{
			EnvironementSync().CreateTable(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_SYNCHRONIZE_DATAS, false, 9121)]
		public static void PreprodSynchronizeAllData ()
		{
			EnvironementSync().AllSynchronization (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_FORCE_SYNCHRONIZE, false, 9122)]
		public static void PreprodForceSynchronizeAllData ()
		{
			EnvironementSync().AllSynchronizationForce (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_RESET_CONNEXION, false, 9140)]
		public static void PreprodFlushQueue ()
		{
			EnvironementSync().Reset (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_FLUSH_CONNEXION, false, 9141)]
		public static void PreprodResetUuidAndToken ()
		{
			EnvironementSync().Flush (NWDAppConfiguration.SharedInstance().DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		//PROD
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_PROD, false, 9103)]
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_CREATE_TABLES, false, 9101)]
		public static void ProdCreateTablesServer ()
		{
//				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//					    NWDConstants.K_SYNC_ALERT_MESSAGE,
//					    NWDConstants.K_SYNC_ALERT_OK,
//				NWDConstants.K_SYNC_ALERT_CANCEL)) {
				EnvironementSync().CreateTable (NWDAppConfiguration.SharedInstance().ProdEnvironment);
//				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_SYNCHRONIZE_DATAS, false, 9121)]
		public static void ProdSynchronizeAllData ()
		{
//				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//					    NWDConstants.K_SYNC_ALERT_MESSAGE,
//					    NWDConstants.K_SYNC_ALERT_OK,
//				NWDConstants.K_SYNC_ALERT_CANCEL)) {
                EnvironementSync().AllSynchronization (NWDAppConfiguration.SharedInstance().ProdEnvironment);
//				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_FORCE_SYNCHRONIZE, false, 9122)]
		public static void ProdForceSynchronizeAllData ()
		{
//				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//					NWDConstants.K_SYNC_ALERT_MESSAGE,
//					NWDConstants.K_SYNC_ALERT_OK,
//				NWDConstants.K_SYNC_ALERT_CANCEL)) {
				EnvironementSync().AllSynchronizationForce (NWDAppConfiguration.SharedInstance().ProdEnvironment);
//				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_RESET_CONNEXION, false, 9140)]
		public static void ProdResetUuidAndToken ()
		{
			EnvironementSync().Reset (NWDAppConfiguration.SharedInstance().ProdEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_FLUSH_CONNEXION, false, 9141)]
		public static void ProdFlushQueue ()
		{
			EnvironementSync().Flush (NWDAppConfiguration.SharedInstance().ProdEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		//LOCALS
		//-------------------------------------------------------------------------------------------------------------
		//[MenuItem (NWDConstants.K_MENU_BASE+NWDConstants.K_MENU_LOCAL, false, 9200)]
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCAL_CREATE_TABLES, false, 9201)]
		public static void CreateTables ()
		{
            NWDDataManager.SharedInstance().CreateAllTablesLocal();
            NWDDataManager.SharedInstance().ReloadAllObjects();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCAL_RELOAD_DATAS, false, 9202)]
		public static void ReloadAllDatas ()
		{
			NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_CLEAN_TRASHED_DATAS, false, 9203)]
        public static void CleanAllDatas()
        {
            NWDDataManager.SharedInstance().CleanAllTablesLocal();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_UPDATE_DATAS, false, 9203)]
        public static void UpdateAllDatas()
        {
            NWDDataManager.SharedInstance().UpdateAllTablesLocal();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_LOCAL_RESET_DATAS, false, 9204)]
        public static void ResetAllDatas()
        {
            NWDDataManager.SharedInstance().ResetAllTablesLocal();
            NWDDataManager.SharedInstance().ReloadAllObjects();
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif