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
	/// <summary>
	/// NWD app configuration window.
	/// </summary>
	public class NWDAppEnvironmentSync : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		string ErrorCode="";
		string ErrorDescription="";
		double Octects = 0;
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
		private Texture2D kImageOrange;
		private Texture2D kImageForbidden;
		private Texture2D kImageEmpty;
		//-------------------------------------------------------------------------------------------------------------
		public void Start ()
		{
			Debug.Log ("NWDAppEnvironmentSync Start");
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Awake ()
		{
			Debug.Log ("NWDAppEnvironmentSync Awake");
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{

			kImageRed = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDRed.psd"));
			kImageGreen = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDGreen.psd"));
			kImageOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDOrange.psd"));
			kImageForbidden = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDForbidden.psd"));
			kImageEmpty = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDEmpty.psd"));

			Debug.Log ("NWDAppEnvironmentSync OnEnable");
			DevIcon = kImageEmpty;
			PreProdIcon = kImageEmpty;
			ProdIcon = kImageEmpty;
			// SUCCESS BLOCK
			SuccessBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				
				EndTime = new DateTime();
				DevIcon = kImageEmpty;
				PreProdIcon = kImageEmpty;
				ProdIcon = kImageEmpty;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				Octects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.DevEnvironment.Environment)
				{
					DevIcon = kImageGreen;
					DevSessionExpired = false;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.PreprodEnvironment.Environment)
				{
					PreProdIcon = kImageGreen;
					PreProdSessionExpired = false;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.ProdEnvironment.Environment)
				{
					ProdIcon = kImageGreen;
					ProdSessionExpired = false;
				}
				Repaint ();
			};



			// FAIL BLOCK
			FailBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				EndTime = new DateTime();
				DevIcon = kImageEmpty;
				PreProdIcon = kImageEmpty;
				ProdIcon = kImageEmpty;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				Octects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.DevEnvironment.Environment)
				{
					DevIcon = kImageRed;
					if (tErrorCode.Contains ("RQT"))
					{
						DevSessionExpired = true;
					}
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.PreprodEnvironment.Environment)
				{
					PreProdIcon = kImageRed;
					if (tErrorCode.Contains ("RQT"))
					{
						PreProdSessionExpired = true;
					}
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.ProdEnvironment.Environment)
				{
					ProdIcon = kImageRed;
					if (tErrorCode.Contains ("RQT"))
					{
						ProdSessionExpired = true;
					}
				}
				Repaint ();
			};



			//CANCEL BLOCK
			CancelBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				EndTime = new DateTime();
				DevIcon = kImageEmpty;
				PreProdIcon = kImageEmpty;
				ProdIcon = kImageEmpty;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				Octects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.DevEnvironment.Environment)
				{
					DevIcon = kImageForbidden;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.PreprodEnvironment.Environment)
				{
					PreProdIcon = kImageForbidden;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.ProdEnvironment.Environment)
				{
					ProdIcon = kImageForbidden;
				}
				Repaint ();
			};



			// PROGRESS BLOCK
			ProgressBlock = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
				if (bProgress>=1.0f)
				{
				MiddleTime = new DateTime();
				}
				DevIcon = kImageEmpty;
				PreProdIcon = kImageEmpty;
				ProdIcon = kImageEmpty;
				NWDOperationResult tInfos = (NWDOperationResult)bInfos;
				NWDError tError = tInfos.errorDesc;
				string tErrorCode = tInfos.errorCode;
				Octects = tInfos.Octects;
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.DevEnvironment.Environment)
				{
					DevIcon = kImageOrange;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.PreprodEnvironment.Environment)
				{
					PreProdIcon = kImageOrange;
				}
				if (bOperation.QueueName == NWDAppConfiguration.SharedInstance.ProdEnvironment.Environment)
				{
					ProdIcon = kImageOrange;
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
			this.minSize = new Vector2 (300, 92);
			this.maxSize = new Vector2 (300, 300);
			// set title of window
			titleContent = new GUIContent (NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE);
			// show helpbox
			EditorGUILayout.HelpBox (NWDConstants.K_APP_SYNC_ENVIRONMENT, MessageType.None);

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Dev", EditorStyles.boldLabel);
			GUILayout.Label ("Preprod", EditorStyles.boldLabel);
			GUILayout.Label ("Prod", EditorStyles.boldLabel);
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Dev Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance.DevEnvironment);
			}
			if (GUILayout.Button ("Preprod Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			if (GUILayout.Button ("Prod Sync", EditorStyles.miniButton)) {
				AllSynchronization (NWDAppConfiguration.SharedInstance.ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Dev Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance.DevEnvironment);
			}
			if (GUILayout.Button ("Preprod Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			if (GUILayout.Button ("Prod Sync Force", EditorStyles.miniButton)) {
				AllSynchronizationForce (NWDAppConfiguration.SharedInstance.ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			var tStyleCenter = new GUIStyle (EditorStyles.label);
			tStyleCenter.alignment = TextAnchor.MiddleCenter;

			GUILayout.BeginHorizontal ();
			GUILayout.Label(DevIcon,tStyleCenter, GUILayout.Height (20));
			GUILayout.Label(PreProdIcon,tStyleCenter, GUILayout.Height (20));
			GUILayout.Label(ProdIcon,tStyleCenter, GUILayout.Height (20));
			GUILayout.EndHorizontal ();


			EditorGUILayout.LabelField ("Octect receipt", Octects.ToString ());

			int tDurationNet = NWDToolbox.Timestamp (MiddleTime) - NWDToolbox.Timestamp (StartTime); 
			EditorGUILayout.LabelField ("Network Duration", tDurationNet.ToString () +"s");

			double tDurationNetMilliseconds = NWDToolbox.TimestampMilliseconds (MiddleTime) - NWDToolbox.TimestampMilliseconds (StartTime); 
			EditorGUILayout.LabelField ("Network Duration", tDurationNetMilliseconds.ToString () +"s");

			int tDuration = NWDToolbox.Timestamp (EndTime) - NWDToolbox.Timestamp (StartTime); 
			EditorGUILayout.LabelField ("Duration", tDuration.ToString () +"s");

			double tDurationMilliseconds = NWDToolbox.TimestampMilliseconds (EndTime) - NWDToolbox.TimestampMilliseconds (StartTime); 
			EditorGUILayout.LabelField ("Duration", tDurationMilliseconds.ToString () +"s");


			if (DevSessionExpired == true || PreProdSessionExpired == true || ProdSessionExpired == true) {
				GUILayout.BeginHorizontal ();
					EditorGUI.BeginDisabledGroup (!DevSessionExpired);
				if (GUILayout.Button ("Dev reset", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance.DevEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				EditorGUI.BeginDisabledGroup (!PreProdSessionExpired);
				if (GUILayout.Button ("Preprod reset", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				EditorGUI.BeginDisabledGroup (!ProdSessionExpired);
				if (GUILayout.Button ("Prod reset", EditorStyles.miniButton)) {
					Reset (NWDAppConfiguration.SharedInstance.ProdEnvironment);
				}
				EditorGUI.EndDisabledGroup ();
				GUILayout.EndHorizontal ();
			}

		}
		//-------------------------------------------------------------------------------------------------------------
		public void CreateAllTablesServer (NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDOperationWebManagement.AddOperation ("Create table on server",SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, true);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AllSynchronization (NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDOperationWebSynchronisation.AddOperation ("All Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance.mTypeSynchronizedList, false, false);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AllSynchronizationForce (NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDOperationWebSynchronisation.AddOperation ("All Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, NWDDataManager.SharedInstance.mTypeSynchronizedList, true, true);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Synchronization (List<Type> sTypeList, NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDOperationWebSynchronisation.AddOperation ("Synchronization", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, false, false);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SynchronizationForce (List<Type> sTypeList, NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDOperationWebSynchronisation.AddOperation ("Synchronization Force", SuccessBlock, FailBlock, CancelBlock, ProgressBlock, sEnvironment, sTypeList, true, true);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Reset (NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			sEnvironment.ResetPreferences ();
			// TODO : add message in window
			if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment)
			{
				DevIcon = kImageEmpty;
				DevSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment)
			{
				PreProdIcon = kImageEmpty;
				PreProdSessionExpired = false;
			}
			if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment)
			{
				ProdIcon = kImageEmpty;
				ProdSessionExpired = false;
			}
			EndTime = new DateTime ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Flush (NWDAppEnvironment sEnvironment)
		{
			StartTime = new DateTime ();
			MiddleTime = new DateTime ();
			EndTime = new DateTime ();
			NWDDataManager.SharedInstance.WebOperationQueue.Flush (sEnvironment.Environment);
			// TODO : add message in window
			EndTime = new DateTime ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
