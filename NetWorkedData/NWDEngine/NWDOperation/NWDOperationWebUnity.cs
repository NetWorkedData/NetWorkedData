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
		public GameObject GameObjectToSpawn;

		public bool SecureData = false;

		public UnityWebRequest Request;

		public NWDAppEnvironment Environment;

		static public NWDOperationWebUnity AddOperation (string sName, NWDAppEnvironment sEnvironment = null, bool sPriority = false)
		{
			NWDOperationWebUnity rReturn = NWDOperationWebUnity.Create (sName, sEnvironment);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}

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

		public override void Execute ()
		{
			StartCoroutine (ExecuteAsync ());
		}

		public virtual void DataUploadPrepare ()
		{
		}

		public virtual void DataDownloadedCompute (Dictionary<string, object> sData)
		{
		}

		public virtual string ServerFile ()
		{
			return "index.php";
		}

		public virtual string ServerBase ()
		{
			return Environment.ServerHTTPS.TrimEnd ('/') + "/Environment/" + Environment.Environment + "/" + ServerFile ();
		}

		IEnumerator ExecuteAsync ()
		{
			Statut = BTBOperationState.Start;
			//callback error
			NWDOperationResult tInfos = new NWDOperationResult ();
			ProgressInvoke (0.0f, tInfos);

			Statut = BTBOperationState.InProgress;
			float tStart = Time.time;
			BTBDebug.Log ("ExecuteAsync tStart = " + tStart.ToString ());
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
			using (Request = UnityWebRequest.Post (ServerBase (), tWWWForm)) {
				//BTBDebug.Log ("URL : " + Request.url);
				// I prepare the header 
				// I put the header in my request
				InsertHeaderInRequest ();
				//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("header inserted", this));
				// I send the data
				Request.Send ();
				BTBDebug.Log ("Request URL " + Request.url);
				while (!Request.isDone) {
					//BTBDebug.Log ("Inside waiting loop, updating progress");
					Statut = BTBOperationState.InProgress;
					NWDOperationResult tInfosProgress = new NWDOperationResult ();
					ProgressInvoke (Request.downloadProgress, tInfosProgress);
					if (Request.uploadProgress < 1.0f) {
						//BTBDebug.Log ("NWDOperationWebUnity uploadProgress : " + Request.uploadProgress);
						//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("uploadProgress", this));
					}
					if (Request.downloadProgress < 1.0f) {
						//BTBDebug.Log ("NWDOperationWebUnity downloadProgress : " + Request.downloadProgress);
						//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("downloadProgress", this));
					}
					if (Request.isDone == true) {
						BTBDebug.Log ("NWDOperationWebUnity Upload / Download Request isDone: " + Request.isDone);
					}
					#if UNITY_EDITOR
					yield return null;
					#else
					//yield return new WaitForEndOfFrame ();
					yield return null;
					#endif
				}

				BTBDebug.Log ("NWDOperationWebUnity Request isDone: " + Request.isDone);
				if (Request.isNetworkError) { // Error
					//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("error", this));
					Statut = BTBOperationState.Error;
					NWDOperationResult tInfosError = new NWDOperationResult (Request.error);
					FailInvoke (Request.downloadProgress, tInfosError);
				} else { // Success
					//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("success", this));
					BTBDebug.Log ("NWDOperationWebUnity text : " + Request.downloadHandler.text);
					Dictionary<string, object> tData = new Dictionary<string, object> ();
					if (Request.downloadHandler.text.Equals ("")) {
						Statut = BTBOperationState.Error;
						//BTBError tError = BTBErrorManager.ShareInstance().FindError("INN00");
						NWDOperationResult tInfosFail = new NWDOperationResult ("INN00");
						FailInvoke (Request.downloadProgress, tInfosFail);
					} else {
						tData = Json.Deserialize (Request.downloadHandler.text) as Dictionary<string, object>;

						// verif Dico answer
						if (tData == null) {
							Statut = BTBOperationState.Error;
							NWDOperationResult tInfosFail = new NWDOperationResult ("INN00");
							FailInvoke (Request.downloadProgress, tInfosFail);
						} else {
							NWDOperationResult tInfosResult = new NWDOperationResult (tData);

							// memorize the token for next connexion
							if (!tInfosResult.token.Equals ("")) {
								Environment.RequesToken = tInfosResult.token;
							}

							// Check if error
							if (tInfosResult.isError) {
								Statut = BTBOperationState.Failed;

								//TODO if error do something
                                
								FailInvoke (Request.downloadProgress, tInfosResult);
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

								SuccessInvoke (Request.downloadProgress, tInfosResult);
							}
						}
					}
				}

				//Save preference localy
				Environment.SavePreferences ();

				float tEnd = Time.time;
				float tDelta = tEnd - tStart;
				BTBDebug.Log ("ExecuteAsync tEnd = " + tEnd.ToString ());
				BTBDebug.Log ("ExecuteAsync tDelta = " + tDelta.ToString ());
				Finish ();
			}
		}

		public override void Cancel ()
		{
			BTBDebug.Log ("NWDOperationWebUnity Cancel");
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

		public override void Finish ()
		{
			BTBDebug.Log ("NWDOperationWebUnity Finish");

			this.Statut = BTBOperationState.Finish;
			IsFinish = true;
			Parent.NextOperation (this.QueueName);
		}

		public override void DestroyThisOperation ()
		{
			this.Statut = BTBOperationState.Destroy;
			// destroy this object now
#if UNITY_EDITOR
			DestroyImmediate (GameObjectToSpawn);
#else
			Destroy (GameObjectToSpawn);
			BTBDebug.Log("BTBOperation Destroy ");
#endif
		}

		static string OSKey = "os";
		static string LangKey = "lang";
		static string VersionKey = "version";
		static string UUIDKey = "uuid";
		static string RequestTokenKey = "token";
		static string HashKey = "hash";

		#if UNITY_EDITOR
		static string AdminHashKey = "adminHash";
		#endif

		public string OS;
		public string Lang;
		public string Version;
		public string UUID;
		public string RequestToken;

		public void InsertHeaderInRequest ()
		{
			//TODO: Insert Header In Request
			Dictionary<string, object> tHeaderParams = new Dictionary<string, object> ();
			BTBDebug.Log ("UUID USED IS : '" + Environment.PlayerAccountReference + "' and token is = " + Environment.RequesToken);
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
			#endif
			// insert dico of header in request header
			foreach (KeyValuePair<string, object> tEntry in tHeaderParams) {
				Request.SetRequestHeader (tEntry.Key, tEntry.Value.ToString ());
			}
		}

		static string UnSecureKey = "prm";
		static string SecureKey = "scr";
		static string UnSecureDigestKey = "prmdgt";
		static string SecureDigestKey = "scrdgt";

		public Dictionary<string,object> Data = new Dictionary<string,object> ();

		public WWWForm InsertDataInRequest ()
		{
			//BTBNotificationManager.ShareInstance.PostNotification (new BTBNotification ("data insert start", this));

			WWWForm tBodyData = new WWWForm ();
			string tParamKey = UnSecureKey;
			string tDigestKey = UnSecureDigestKey;
			string tParamValue = "";
			string tDigestValue = "";
			BTBDebug.Log ("Insert data in request : json = " + Json.Serialize (Data), BTBDebugResult.Success);
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
			return tBodyData;
		}
	}
}
//=====================================================================================================================