//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

using BasicToolBox;
using BTBMiniJSON;

//=====================================================================================================================
namespace NetWorkedData
{
	[ExecuteInEditMode]
	public partial class NWDOperationWebUnity : BTBOperation
	{
		//-------------------------------------------------------------------------------------------------------------
		public static int kTimeOutOfRequest = 30;
		public GameObject GameObjectToSpawn;

		public bool SecureData = false;

		public UnityWebRequest Request;

		public NWDAppEnvironment Environment;

		//-------------------------------------------------------------------------------------------------------------
		static public NWDOperationWebUnity AddOperation (string sName, NWDAppEnvironment sEnvironment = null, bool sPriority = false)
		{
			NWDOperationWebUnity rReturn = NWDOperationWebUnity.Create (sName, sEnvironment);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		static public NWDOperationWebUnity Create (string sName, NWDAppEnvironment sEnvironment = null)
		{
			NWDOperationWebUnity rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation";
			}
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance.SelectedEnvironment ();
			}
			GameObject tGameObjectToSpawn = new GameObject (sName);
			rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUnity> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;
			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
			#if UNITY_EDITOR
			#else
			DontDestroyOnLoad (tGameObjectToSpawn);
			#endif
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Execute ()
		{
			StartCoroutine (ExecuteAsync ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual void DataUploadPrepare ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual void DataDownloadedCompute (Dictionary<string, object> sData)
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual string ServerFile ()
		{
			return "index.php";
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual string ServerBase ()
		{
			return Environment.ServerHTTPS.TrimEnd ('/') + "/Environment/" + Environment.Environment + "/" + ServerFile ();
		}
		//-------------------------------------------------------------------------------------------------------------
		IEnumerator ExecuteAsync ()
		{
			Statut = BTBOperationState.Start;
			//callback error
			NWDOperationResult tInfos = new NWDOperationResult ();
			ProgressInvoke (0.0f, tInfos);

			Statut = BTBOperationState.InProgress;
			float tStart = Time.time;
//			Debug.LogVerbose ("ExecuteAsync tStart = " + tStart.ToString ());
			// Put Sync in progress
			// ParentQueue.SynchronizeInProgress = true;
			// Send this operation in actual operation for this environment
			Parent.Controller [QueueName].ActualOperation = this;
			// Force all datas to be write in database
			NWDDataManager.SharedInstance.UpdateQueueExecute ();
			#if UNITY_EDITOR
			// Deselect all object
			Selection.activeObject = null;
			#endif
			// I prepare the data
			DataUploadPrepare ();
			// I insert the data
			WWWForm tWWWForm = InsertDataInRequest ();
			using (Request = UnityWebRequest.Post (ServerBase (), tWWWForm))
            {
				Request.timeout = kTimeOutOfRequest;
				Debug.Log ("URL : " + Request.url);
				// I prepare the header 
				// I put the header in my request
				InsertHeaderInRequest ();
				//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("header inserted", this));
				// I send the data
				Request.Send();
				Debug.Log ("Request URL " + Request.url);

				while (!Request.isDone)
                {
					//Debug.Log ("Inside waiting loop, updating progress");
					Statut = BTBOperationState.InProgress;
					NWDOperationResult tInfosProgress = new NWDOperationResult ();
					ProgressInvoke (Request.downloadProgress, tInfosProgress);
					if (Request.uploadProgress < 1.0f) {
						//Debug.LogVerbose ("NWDOperationWebUnity uploadProgress : " + Request.uploadProgress);
						BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_UPLOAD_IN_PROGRESS, this));
					}
					if (Request.downloadProgress < 1.0f) {
						//Debug.LogVerbose ("NWDOperationWebUnity downloadProgress : " + Request.downloadProgress);
						BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_IN_PROGRESS, this));
					}
					yield return null;
				}

				if (Request.isDone == true)
                {
					Debug.Log ("NWDOperationWebUnity Upload / Download Request isDone: " + Request.isDone);
					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_IS_DONE, this));
				}

				if (Request.isNetworkError)
                { 
					Debug.Log ("NWDOperationWebUnity isNetworkError ");
					//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("error", this));

					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

					NWDGameDataManager.UnitySingleton().NetworkStatutChange (NWDNetworkState.OffLine);

					Statut = BTBOperationState.Error;
					NWDOperationResult tInfosError = new NWDOperationResult ("WEB01");
					FailInvoke (Request.downloadProgress, tInfosError);

					if (Application.isPlaying == true) {
						NWDGameDataManager.UnitySingleton ().ErrorManagement (tInfosError.errorDesc);
					}


				} else if (Request.isHttpError) { // Error
					Debug.Log ("NWDOperationWebUnity isHttpError ");
					//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("error", this));

					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

					NWDGameDataManager.UnitySingleton().NetworkStatutChange (NWDNetworkState.OnLine);

					Statut = BTBOperationState.Error;
					NWDOperationResult tInfosError = new NWDOperationResult ("WEB02");
					tInfosError.Octects = 0;
					FailInvoke (Request.downloadProgress, tInfosError);

					if (Application.isPlaying == true) {
						NWDGameDataManager.UnitySingleton ().ErrorManagement (tInfosError.errorDesc);
					}

				} else { 

					// Success ... but put 100% in progress anyway
					NWDOperationResult tInfosProgress = new NWDOperationResult ();
					ProgressInvoke (1.0f, tInfosProgress);

					// Success
					//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("success", this));
					Debug.Log ("NWDOperationWebUnity text : " + Request.downloadHandler.text);

					NWDGameDataManager.UnitySingleton().NetworkStatutChange (NWDNetworkState.OnLine);

					Dictionary<string, object> tData = new Dictionary<string, object> ();
					if (Request.downloadHandler.text.Equals ("")) {

						BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

						Statut = BTBOperationState.Error;
						NWDOperationResult tInfosFail = new NWDOperationResult ("WEB03");
						tInfosFail.Octects = 0;
						FailInvoke (Request.downloadProgress, tInfosFail);

						if (Application.isPlaying == true) {
							NWDGameDataManager.UnitySingleton ().ErrorManagement (tInfosFail.errorDesc);
						}

					} else {
						tData = Json.Deserialize (Request.downloadHandler.text) as Dictionary<string, object>;

						// verif Dico answer
						// TODO : TOKEN IS FAILED : DISCONNECT AND RESET DATA FOR THIS USER... NO SYNC AUTHORIZED... DELETE LOCAL DATA... RESTAURE FROM LOGIN
						if (tData == null) {


							BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_ERROR, this));

							Statut = BTBOperationState.Error;
							NWDOperationResult tInfosFail = new NWDOperationResult ("WEB04");
							tInfosFail.Octects = Request.downloadHandler.text.Length;
							FailInvoke (Request.downloadProgress, tInfosFail);

							if (Application.isPlaying == true) {
								NWDGameDataManager.UnitySingleton ().ErrorManagement (tInfosFail.errorDesc);
							}

						} else {

							//BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_SUCCESSED, this));

							NWDOperationResult tInfosResult = new NWDOperationResult (tData);

							tInfosResult.Octects = Request.downloadHandler.text.Length;
							// memorize the token for next connexion
							if (!tInfosResult.token.Equals ("")) {
								Environment.RequesToken = tInfosResult.token;
							}

							// Check if error
							if (tInfosResult.isError) {
								Statut = BTBOperationState.Failed;

								//TODO if error do something
								if (tInfosResult.errorCode == "RQT90" ||
								    tInfosResult.errorCode == "RQT91" ||
								    tInfosResult.errorCode == "RQT92" ||
								    tInfosResult.errorCode == "RQT93" ||
									tInfosResult.errorCode == "RQT94") {
									// TODO : Alert (Session expire)
									#if UNITY_EDITOR
									//EditorUtility.DisplayDialog ("Alert", "Session expired (error code " + tInfosResult.errorCode + ")", "Ok");
									#endif
									BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_SESSION_EXPIRED, tInfosResult));
									// TODO : Change for anonymous account
									NWDAppConfiguration.SharedInstance.SelectedEnvironment ().RestaureAnonymousSession ();
								} else {

									#if UNITY_EDITOR
//									string tDescription = "unknown error (error code " + tInfosResult.errorCode + ")";
//									if (tInfosResult.errorDesc!=null)
//									{
//										tDescription = "error " +tInfosResult.errorCode + " : " +tInfosResult.errorDesc.LocalizedDescription.GetLocalString ();
//									}
//									EditorUtility.DisplayDialog ("  Alert", tDescription, "Ok");
									#endif
									BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_ERROR, tInfosResult));
								}
								FailInvoke (Request.downloadProgress, tInfosResult);
								if (Application.isPlaying == true) {
									NWDGameDataManager.UnitySingleton ().ErrorManagement (tInfosResult.errorDesc);
								}


							} else {
								Statut = BTBOperationState.Success;

								string tUUID = tInfosResult.uuid;
								if (!tUUID.Equals ("")) {
									Environment.PlayerAccountReference = tUUID;
								}

								if (tInfosResult.isNewUser && tInfosResult.isUserTransfert) {
									if (!tUUID.Equals ("")) {
										NWDDataManager.SharedInstance.ChangeAllDatasForUserToAnotherUser (Environment, tUUID);
									}
								}

								if (tInfosResult.isSignUpdate) {
									Environment.PlayerStatut = tInfosResult.sign;
									if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Unknow) {
										//Nothing to do
									} else if (tInfosResult.sign == NWDAppEnvironmentPlayerStatut.Anonymous) {
										if (!tUUID.Equals ("")) {
											Environment.AnonymousPlayerAccountReference = tUUID;
										}
										if (!tInfosResult.signkey.Equals ("")) {
											Environment.AnonymousResetPassword = tInfosResult.signkey;
										}
									} else if (tInfosResult.sign != NWDAppEnvironmentPlayerStatut.Temporary) {
										if (Environment.PlayerAccountReference == Environment.AnonymousPlayerAccountReference) {
											//Using signed account as anonymous account = reset!
											Environment.ResetAnonymousSession ();
										}
									}
								}
								if (tInfosResult.isReloadingData) {
									//TODO : need reload data
								}

								DataDownloadedCompute (tData);

								BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_DOWNLOAD_SUCCESSED, tInfosResult));
								SuccessInvoke (Request.downloadProgress, tInfosResult);
							}
						}
					}
				}

				//Save preference localy
				Environment.SavePreferences ();

				//float tEnd = Time.time;
				//float tDelta = tEnd - tStart;
//				Debug.LogVerbose ("ExecuteAsync tEnd = " + tEnd.ToString ());
//				Debug.LogVerbose ("ExecuteAsync tDelta = " + tDelta.ToString ());
				Finish ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Cancel ()
		{
//			Debug.LogVerbose ("NWDOperationWebUnity Cancel");
			this.Statut = BTBOperationState.Cancel;
			if (Request != null) {
				Request.Abort ();
				// risk of token lost integrity : operation reconnect ?
			}

			NWDOperationResult tInfosCancel = new NWDOperationResult ();
			CancelInvoke (Request.downloadProgress, tInfosCancel);

			IsFinish = true;
			Parent.NextOperation (this.QueueName);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Finish ()
		{
//			Debug.LogVerbose ("NWDOperationWebUnity Finish");
			this.Statut = BTBOperationState.Finish;
			IsFinish = true;
			Parent.NextOperation (this.QueueName);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DestroyThisOperation ()
		{
			this.Statut = BTBOperationState.Destroy;
			// destroy this object now
#if UNITY_EDITOR
			DestroyImmediate (GameObjectToSpawn);
#else
			Destroy (GameObjectToSpawn);
			Debug.Log("BTBOperation Destroy ");
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		static string OSKey = "os";
		static string LangKey = "lang";
		static string VersionKey = "version";
		static string UUIDKey = "uuid";
		static string RequestTokenKey = "token";
		static string HashKey = "hash";
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		static string AdminHashKey = "adminHash";
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		public string OS;
		public string Lang;
		public string Version;
		public string UUID;
		public string RequestToken;
		//-------------------------------------------------------------------------------------------------------------
		public void InsertHeaderInRequest ()
		{
			//TODO: Insert Header In Request
			Dictionary<string, object> tHeaderParams = new Dictionary<string, object> ();
//			Debug.LogVerbose ("UUID USED IS : '" + Environment.PlayerAccountReference + "' and token is = " + Environment.RequesToken);
			// define values
			UUID = Environment.PlayerAccountReference;
			RequestToken = Environment.RequesToken;
			Version = Application.version;
#if UNITY_EDITOR
			Version = PlayerSettings.bundleVersion;
#endif
			OS = "unity";
#if UNITY_EDITOR
			OS = "editor";
#else
			#if UNITY_ANDROID
			    OS = "android";
			#elif UNITY_IOS
			    OS = "ios";
			#elif UNITY_STANDALONE_OSX
			    OS = "osx";
			#elif UNITY_WP8
			    OS = "wp8";
			#elif UNITY_WINRT
			    OS = "win";
			#else
			    OS = "unity";
			#endif
#endif
			Lang = NWDDataManager.SharedInstance.PlayerLanguage;
			// insert value in header dico
			tHeaderParams.Add (UUIDKey, UUID);
			tHeaderParams.Add (RequestTokenKey, RequestToken);
			tHeaderParams.Add (OSKey, OS);
			tHeaderParams.Add (VersionKey, Version);
			tHeaderParams.Add (LangKey, Lang);
			// create hash security
			string tHashValue = string.Format ("{0}{1}{2}{3}{4}{5}", OS, Version, Lang,
				                    NWDToolbox.GenerateSALT (Environment.SaltFrequency),
				                    UUID, RequestToken);
			tHeaderParams.Add (HashKey, BTBSecurityTools.GenerateSha (tHashValue, BTBSecurityShaTypeEnum.Sha1));
			// add hash for admin
			#if UNITY_EDITOR
			tHeaderParams.Add (AdminHashKey, NWDToolbox.GenerateAdminHash (Environment.AdminKey, Environment.SaltFrequency));
			string tDebug = "";
			#endif

			// insert dico of header in request header
			foreach (KeyValuePair<string, object> tEntry in tHeaderParams) {
				Request.SetRequestHeader (tEntry.Key, tEntry.Value.ToString ());
			#if UNITY_EDITOR
			tDebug += tEntry.Key + " = '" + tEntry.Value.ToString () + "' , ";
			#endif
			}
			#if UNITY_EDITOR
			Debug.Log ("Header : " + tDebug);
			#endif

		}
		//-------------------------------------------------------------------------------------------------------------
		static string UnSecureKey = "prm";
		static string SecureKey = "scr";
		static string UnSecureDigestKey = "prmdgt";
		static string SecureDigestKey = "scrdgt";
		//-------------------------------------------------------------------------------------------------------------
		public Dictionary<string,object> Data = new Dictionary<string,object> ();
		//-------------------------------------------------------------------------------------------------------------
		public WWWForm InsertDataInRequest ()
		{
			//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("data insert start", this));

			WWWForm tBodyData = new WWWForm ();
			string tParamKey = UnSecureKey;
			string tDigestKey = UnSecureDigestKey;
			string tParamValue = "";
			string tDigestValue = "";
			Debug.Log ("Data : " + Json.Serialize (Data));
			if (SecureData) {
				tParamKey = SecureKey;
				tDigestKey = SecureDigestKey;
				tParamValue = BTBSecurityTools.AddAes (Data, Environment.DataSHAPassword, Environment.DataSHAVector, BTBSecurityAesTypeEnum.Aes128);
				tDigestValue = BTBSecurityTools.GenerateSha (Environment.SaltStart + tParamValue + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
			} else {
				tParamValue = BTBSecurityTools.Base64Encode (Json.Serialize (Data));
				tDigestValue = BTBSecurityTools.GenerateSha (Environment.SaltStart + tParamValue + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
			}
			tBodyData.AddField (tParamKey, tParamValue);
			tBodyData.AddField (tDigestKey, tDigestValue);
			//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("data insert finish", this));

			 #if UNITY_EDITOR
			NWDEditorMenu.EnvironementSync ().SendOctects = tParamValue.Length + tDigestValue.Length;
			 #endif
			return tBodyData;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================