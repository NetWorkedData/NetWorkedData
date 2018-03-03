//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	// Use
	// #if UNITY_EDITOR
	// NWDEditorMenu.EnvironementSync ().SendOctects = …
	// NWDEditorMenu.EnvironementSync ().ReceiptOctects = …
	// #endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD app configuration window.
	/// </summary>
	public class NWDAppEnvironmentSync : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		
		public double SendOctects = 0;
		public double ReceiptOctects = 0;
		public double ActionCounter = 0;
		public double ClassActionCounter = 0;
		public double RowCounter = 0;
		DateTime StartTime;
		DateTime MiddleTime;
		DateTime EndTime;

		Texture2D DevIcon;
		Texture2D PreProdIcon;
		Texture2D ProdIcon;

		bool DevSessionExpired = false;
		bool PreProdSessionExpired = false;
		bool ProdSessionExpired = false;
		//-------------------------------------------------------------------------------------------------------------
		private BTBOperationBlock SuccessBlock = null;
		private BTBOperationBlock FailBlock = null;
		private BTBOperationBlock CancelBlock = null;
		private BTBOperationBlock ProgressBlock = null;
		//-------------------------------------------------------------------------------------------------------------
		// Icons for Sync
		private Texture2D kImageRed;
		private Texture2D kImageGreen;
		//private Texture2D kImageOrange;
		private Texture2D kImageForbidden;
		private Texture2D kImageEmpty;
		private Texture2D kImageWaiting;

        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentSync SharedInstance()
        {
            return NWDEditorMenu.kNWDAppEnvironmentSync;
        }
		//-------------------------------------------------------------------------------------------------------------
		public void Start ()
		{
		//	Debug.Log ("NWDAppEnvironmentSync Start");
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Awake ()
		{
		//	Debug.Log ("NWDAppEnvironmentSync Awake");
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{
			//	Debug.Log ("NWDAppEnvironmentSync OnEnable");

			kImageRed = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDRed.psd"));
			kImageGreen = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDGreen.psd"));
			//kImageOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDOrange.psd"));
			kImageForbidden = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDForbidden.psd"));
			kImageEmpty = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDEmpty.psd"));
			kImageWaiting = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDWaiting.psd"));

			DevIcon = kImageEmpty;
			PreProdIcon = kImageEmpty;
			ProdIcon = kImageEmpty;
			// SUCCESS BLOCK
			SuccessBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				EndTime = DateTime.Now;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				ReceiptOctects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment) {
					DevIcon = kImageGreen;
					DevSessionExpired = false;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment) {
					PreProdIcon = kImageGreen;
					PreProdSessionExpired = false;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment) {
					ProdIcon = kImageGreen;
					ProdSessionExpired = false;
				}
				Repaint ();
			};

			// FAIL BLOCK
			FailBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				EndTime = DateTime.Now;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				ReceiptOctects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment) {
					DevIcon = kImageRed;
					if (tErrorCode.Contains ("RQT")) {
						DevSessionExpired = true;
					}
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment) {
					PreProdIcon = kImageRed;
					if (tErrorCode.Contains ("RQT")) {
						PreProdSessionExpired = true;
					}
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment) {
					ProdIcon = kImageRed;
					if (tErrorCode.Contains ("RQT")) {
						ProdSessionExpired = true;
					}
				}
				Repaint ();
				if (tInfos.isError) {
					if (tErrorCode.Contains ("RQT"))
                    {
						EditorUtility.DisplayDialog ("Alert", "Session expired (error code " + tInfos.errorCode + ")", "Ok");
				    }
                    else
                    {
					    string tDescription = "Unknown error (error code " + tInfos.errorCode + ")";
					    if (tInfos.errorDesc != null)
                        {
						    tDescription = "Error " + tInfos.errorCode + " : " + tInfos.errorDesc.LocalizedDescription.GetLocalString ();
					    }
					    EditorUtility.DisplayDialog ("Alert", tDescription, "Ok");
					}
				}
				Repaint ();
			};

			//CANCEL BLOCK
			CancelBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				EndTime = DateTime.Now;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				ReceiptOctects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment) {
					DevIcon = kImageForbidden;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment) {
					PreProdIcon = kImageForbidden;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment) {
					ProdIcon = kImageForbidden;
				}
				Repaint ();
			};


			// PROGRESS BLOCK
			ProgressBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				if (bProgress >= 1.0f) {
					MiddleTime = DateTime.Now;
				}
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				ReceiptOctects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().DevEnvironment.Environment) {
					DevIcon = kImageWaiting;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment) {
					PreProdIcon = kImageWaiting;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment) {
					ProdIcon = kImageWaiting;
				}
				Repaint ();
			};
		}
		//-------------------------------------------------------------------------------------------------------------


		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event.
		/// </summary>
		public void OnGUI ()
		{
			this.minSize = new Vector2 (300, 330);
			this.maxSize = new Vector2 (300, 380);
			// set title of window
			titleContent = new GUIContent (NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE);
			// show helpbox
			EditorGUILayout.HelpBox (NWDConstants.K_APP_SYNC_ENVIRONMENT, MessageType.None);



			var tStyleCenter = new GUIStyle (EditorStyles.label);
			tStyleCenter.alignment = TextAnchor.MiddleCenter;


			var tStyleBoldCenter = new GUIStyle (EditorStyles.boldLabel);
			tStyleBoldCenter.alignment = TextAnchor.MiddleCenter;


			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Dev", tStyleBoldCenter);
			GUILayout.Label ("Preprod", tStyleBoldCenter);
			GUILayout.Label ("Prod", tStyleBoldCenter);
			GUILayout.EndHorizontal ();



			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Update Table", EditorStyles.miniButton)) {
				CreateTable (NWDAppConfiguration.SharedInstance().DevEnvironment);
			}
			if (GUILayout.Button ("Update Table", EditorStyles.miniButton)) {
				CreateTable (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
			}
			if (GUILayout.Button ("Update Table", EditorStyles.miniButton)) {
				CreateTable (NWDAppConfiguration.SharedInstance().ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance().DevEnvironment);
			}
			if (GUILayout.Button ("Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
			}
			if (GUILayout.Button ("Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance().ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().DevEnvironment);
			}
			if (GUILayout.Button ("Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
			}
			if (GUILayout.Button ("Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().ProdEnvironment);
			}
			GUILayout.EndHorizontal ();


			Color tOldColor = GUI.backgroundColor;
			GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Sync Clean", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().DevEnvironment);
			}
			if (GUILayout.Button ("Sync Clean", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
			}
			if (GUILayout.Button ("Sync Clean", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance().ProdEnvironment);
			}
			GUILayout.EndHorizontal ();
			GUI.backgroundColor = tOldColor;


			GUILayout.BeginHorizontal ();
			GUILayout.Label (DevIcon, tStyleCenter, GUILayout.Height (20));
			GUILayout.Label (PreProdIcon, tStyleCenter, GUILayout.Height (20));
			GUILayout.Label (ProdIcon, tStyleCenter, GUILayout.Height (20));
			GUILayout.EndHorizontal ();

//			EditorGUILayout.LabelField ("- Start", NWDToolbox.Timestamp (StartTime).ToString ());
//			EditorGUILayout.LabelField ("- Middle", NWDToolbox.Timestamp (MiddleTime).ToString ());
//			EditorGUILayout.LabelField ("- End", NWDToolbox.Timestamp (EndTime).ToString ());

//			int tDurationNet = NWDToolbox.Timestamp (MiddleTime) - NWDToolbox.Timestamp (StartTime); 
//			EditorGUILayout.LabelField ("Network Duration", tDurationNet.ToString () +"s");
			double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds (MiddleTime) - NWDToolbox.TimestampMilliseconds (StartTime)) / 1000.0F; 
			EditorGUILayout.LabelField ("Network Duration", tDurationNetMilliseconds.ToString ("#0.000") + " s");
			EditorGUILayout.LabelField ("Octect Send", SendOctects.ToString ());
			EditorGUILayout.LabelField ("Octect receipt", ReceiptOctects.ToString ());

//			int tDuration = NWDToolbox.Timestamp (EndTime) - NWDToolbox.Timestamp (StartTime); 
			//			EditorGUILayout.LabelField ("Total Duration", tDuration.ToString () +"s");

			double tDurationDataMilliseconds = (NWDToolbox.TimestampMilliseconds (EndTime) - NWDToolbox.TimestampMilliseconds (MiddleTime)) / 1000.0F;

			EditorGUILayout.LabelField ("DataBase Duration", tDurationDataMilliseconds.ToString ("#0.000") + " s");

			double tDurationMilliseconds = (NWDToolbox.TimestampMilliseconds (EndTime) - NWDToolbox.TimestampMilliseconds (StartTime)) / 1000.0F;

			EditorGUILayout.LabelField ("Total Duration", tDurationMilliseconds.ToString ("#0.000") + " s");


			if (DevSessionExpired == true || PreProdSessionExpired == true || ProdSessionExpired == true) {
				GUILayout.BeginHorizontal ();
				EditorGUI.BeginDisabledGroup (!DevSessionExpired);
				if (GUILayout.Button ("reset token", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance().DevEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				EditorGUI.BeginDisabledGroup (!PreProdSessionExpired);
				if (GUILayout.Button ("reset token", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				EditorGUI.BeginDisabledGroup (!ProdSessionExpired);
				if (GUILayout.Button ("reset token", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance().ProdEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				GUILayout.EndHorizontal ();
			}


			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("flush web queue", EditorStyles.miniButton)) {
				Flush (NWDAppConfiguration.SharedInstance().DevEnvironment);
			}
			if (GUILayout.Button ("flush web queue", EditorStyles.miniButton)) {
				Flush (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
			}
			if (GUILayout.Button ("flush web queue", EditorStyles.miniButton)) {
				Flush (NWDAppConfiguration.SharedInstance().ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			GUILayout.Space (20.0F);

			GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
			GUILayout.Label ("Local database", tStyleBoldCenter);

			if (GUILayout.Button ("Clean all local tables", EditorStyles.miniButton)) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_CLEAN_ALERT_TITLE,
					    NWDConstants.K_CLEAN_ALERT_MESSAGE,
					    NWDConstants.K_CLEAN_ALERT_OK,
					    NWDConstants.K_CLEAN_ALERT_CANCEL)) {
					NWDDataManager.SharedInstance().CleanAllTablesLocal ();
				}
			}
			GUI.backgroundColor = tOldColor;



            // Show version selected
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            NWDAccount tAccount = NWDAccount.ActualAccount();
            if (tAccount != null)
            {
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
            }
		}
		//-------------------------------------------------------------------------------------------------------------
		public void CreateTable (NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebManagement.AddOperation ("Create table on server", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, true);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AllSynchronization (NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("All Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, false, false);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AllSynchronizationForce (NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("All Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, true, true);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AllSynchronizationClean (NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					NWDConstants.K_SYNC_ALERT_MESSAGE,
					NWDConstants.K_SYNC_ALERT_OK,
					NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("All Synchronization Clean", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance().mTypeSynchronizedList, true, true, true);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Synchronization (List<Type> sTypeList, NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SynchronizationClean (List<Type> sTypeList, NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					NWDConstants.K_SYNC_ALERT_MESSAGE,
					NWDConstants.K_SYNC_ALERT_OK,
					NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("Synchronization clean", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false, true);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SynchronizationForce (List<Type> sTypeList, NWDAppEnvironment sEnvironment)
		{
			bool tOk = false;
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					tOk = true;
				}
			} else {
				tOk = true;
			}
			if (tOk == true) {
				StartProcess (sEnvironment);
				NWDOperationWebSynchronisation.AddOperation ("Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, true, true);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Reset (NWDAppEnvironment sEnvironment)
		{
			StartProcess (sEnvironment);
			sEnvironment.ResetPreferences ();
			// TODO : add message in window
			if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment) {
				DevIcon = kImageEmpty;
				DevSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment) {
				PreProdIcon = kImageEmpty;
				PreProdSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				ProdIcon = kImageEmpty;
				ProdSessionExpired = false;
			}
			EndTime = DateTime.Now;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Flush (NWDAppEnvironment sEnvironment)
		{
			StartProcess (sEnvironment);
			NWDDataManager.SharedInstance().WebOperationQueue.Flush (sEnvironment.Environment);
			// TODO : add message in window
			EndTime = DateTime.Now;
		}
		//-------------------------------------------------------------------------------------------------------------
		void StartProcess (NWDAppEnvironment sEnvironment)
		{
			StartTime = DateTime.Now;
			MiddleTime = StartTime;
			EndTime = StartTime;
			SendOctects = 0;
			ReceiptOctects = 0;
			ActionCounter = 0;
			ClassActionCounter = 0;
			RowCounter = 0;

			DevIcon = kImageEmpty;
			PreProdIcon = kImageEmpty;
			ProdIcon = kImageEmpty;

			if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment) {
				DevIcon = kImageEmpty;
				DevSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment) {
				PreProdIcon = kImageEmpty;
				PreProdSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment) {
				ProdIcon = kImageEmpty;
				ProdSessionExpired = false;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
