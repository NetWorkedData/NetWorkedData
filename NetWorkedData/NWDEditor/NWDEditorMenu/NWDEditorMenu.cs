﻿/// <summary>
/// NWD editor menu.
/// </summary>
/// 
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
			NWDDataManager.SharedInstance.ConnectToDatabase ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Idemobis the net worked data info show.
		/// </summary>
		[MenuItem (NWDConstants.K_MENU_IDEMOBI, false, 00)]
		public static void IdemobiNetWorkedDataInfoShow()
		{
			EditorUtility.DisplayDialog(NWDConstants.K_ALERT_IDEMOBI_TITLE, NWDConstants.K_ALERT_IDEMOBI_MESSAGE, NWDConstants.K_ALERT_IDEMOBI_OK);
		}
		//-------------------------------------------------------------------------------------------------------------
		// menu 11 see NWD project window.
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TheNWD app configuration window.
		/// </summary>
		static NWDAppEnvironmentChooser kNWDAppEnvironmentChooser;
		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT, false, 12)]
		public static void EnvironementChooserShow()
		{
			if (kNWDAppEnvironmentChooser == null) {
				kNWDAppEnvironmentChooser = (NWDAppEnvironmentChooser)ScriptableObject.CreateInstance <NWDAppEnvironmentChooser> ();
			}
			kNWDAppEnvironmentChooser.ShowUtility ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// TheNWD app configuration window.
		/// </summary>
		static NWDAppEnvironmentSync kNWDAppEnvironmentSync;
		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT_SYNC, false, 13)]
		public static void EnvironementSyncShow()
		{
			if (kNWDAppEnvironmentSync == null) {
				kNWDAppEnvironmentSync = (NWDAppEnvironmentSync)ScriptableObject.CreateInstance <NWDAppEnvironmentSync> ();
			}
			kNWDAppEnvironmentSync.ShowUtility ();
		}
		//-------------------------------------------------------------------------------------------------------------
		// Loop for classes menu
		//-------------------------------------------------------------------------------------------------------------
		// CREATE FILE
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_CREATE_PHP_FILES, false, 1761)]
		public static void CreatePHP ()
		{
//			NWDFindPackage.SharedInstance ();
			NWDDataManager.SharedInstance.CreatePHPAllClass();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_CREATE_PHP_EXPORT_WEB_SITE, false, 1762)]
		public static void CreateWebsitesPHP ()
		{
//			NWDFindPackage.SharedInstance ();
			NWDDataManager.SharedInstance.ExportWebSites();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_CREATE_CSHARP_FILES, false, 1862)]
		public static void CreateCSharp ()
		{
			NWDDataManager.SharedInstance.CreateCShapAllClass();
		}
		//-------------------------------------------------------------------------------------------------------------
		// LOCALIZATION
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_REORDER, false, 2020)]
		public static void LocalizationReorder ()
		{
			NWDAppConfiguration.SharedInstance.DataLocalizationManager.ReOrderAllLocalizations ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_EXPORT, false, 2040)]
		public static void LocalizationExport ()
		{
			NWDAppConfiguration.SharedInstance.DataLocalizationManager.ExportToCSV ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_IMPORT, false, 2041)]
		public static void LocalizationImport ()
		{
			NWDAppConfiguration.SharedInstance.DataLocalizationManager.ImportFromCSV ();
		}
		//-------------------------------------------------------------------------------------------------------------
		// DEV 
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_CREATE_TABLES, false, 3000)]
		public static void DevCreateTablesServer ()
		{
			NWDDataManager.SharedInstance.CreateAllTablesServer(NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_SYNCHRONIZE_DATAS, false, 2101)]
		public static void DevSynchronizeAllData ()
		{
			NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_FORCE_SYNCHRONIZE, false, 2101)]
		public static void DevForceSynchronizeAllData ()
		{
			NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (true, NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_RESET_CONNEXION, false, 4000)]
		public static void DevResetUuidAndToken ()
		{
			NWDDataManager.SharedInstance.ResetPreferences (NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_DEV_FLUSH_CONNEXION, false, 4030)]
		public static void DevFlushQueue ()
		{
			NWDDataManager.SharedInstance.WebRequestFlush (NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		// PREPROD
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_CREATE_TABLES, false, 3000)]
		public static void PreprodCreateTablesServer ()
		{
			NWDDataManager.SharedInstance.CreateAllTablesServer(NWDAppConfiguration.SharedInstance.PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_SYNCHRONIZE_DATAS, false, 2101)]
		public static void PreprodSynchronizeAllData ()
		{
			NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_FORCE_SYNCHRONIZE, false, 2101)]
		public static void PreprodForceSynchronizeAllData ()
		{
			NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (true, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_RESET_CONNEXION, false, 4000)]
		public static void PreprodFlushQueue ()
		{
			NWDDataManager.SharedInstance.ResetPreferences (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PREPROD_FLUSH_CONNEXION, false, 4030)]
		public static void PreprodResetUuidAndToken ()
		{
			NWDDataManager.SharedInstance.WebRequestFlush (NWDAppConfiguration.SharedInstance.DevEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		//PROD
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_CREATE_TABLES, false, 3000)]
		public static void ProdCreateTablesServer ()
		{
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					NWDDataManager.SharedInstance.CreateAllTablesServer (NWDAppConfiguration.SharedInstance.ProdEnvironment);
				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_SYNCHRONIZE_DATAS, false, 2101)]
		public static void ProdSynchronizeAllData ()
		{
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_FORCE_SYNCHRONIZE, false, 2101)]
		public static void ProdForceSynchronizeAllData ()
		{
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					NWDConstants.K_SYNC_ALERT_MESSAGE,
					NWDConstants.K_SYNC_ALERT_OK,
					NWDConstants.K_SYNC_ALERT_CANCEL)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (true, NWDAppConfiguration.SharedInstance.ProdEnvironment);
				}
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_RESET_CONNEXION, false, 4000)]
		public static void ProdResetUuidAndToken ()
		{
			NWDDataManager.SharedInstance.ResetPreferences (NWDAppConfiguration.SharedInstance.ProdEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_PROD_FLUSH_CONNEXION, false, 4030)]
		public static void ProdFlushQueue ()
		{
			NWDDataManager.SharedInstance.WebRequestFlush (NWDAppConfiguration.SharedInstance.ProdEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		//LOCALS
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCAL_CREATE_TABLES, false, 2101)]
		public static void CreateTables ()
		{
			NWDDataManager.SharedInstance.CreateAllTablesLocal();
		}
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_LOCAL_RELOAD_DATAS, false, 2101)]
		public static void ReloadAllDatas ()
		{
			NWDDataManager.SharedInstance.ReloadAllObjects();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif