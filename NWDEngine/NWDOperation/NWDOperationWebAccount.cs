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
using BTBMiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDOperationWebAccount : NWDOperationWebUnity
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string ActionKey = "action";
		public static string EmailKey = "email";
		public static string EmailRescueKey = "emailrescue";
		public static string PasswordKey = "password";
		public static string OldPasswordKey = "old_password";
		public static string NewPasswordKey = "new_password";
		public static string ConfirmPasswordKey = "password_confirm";
		public static string SocialTokenKey = "social_token";
		//-------------------------------------------------------------------------------------------------------------
		public string AnonymousPlayerAccountReferenceKey = "auuid";
		public string AnonymousResetPasswordKey = "apassword";
		//-------------------------------------------------------------------------------------------------------------
		public string Action;
		public string Email;
		public string Password;
		public string EmailHash;
		public string EmailRescue;
		public string PasswordHash;
		public string OldPassword;
		public string NewPassword;
		public string ConfirmPassword;
		public string SocialToken;
		//-------------------------------------------------------------------------------------------------------------
		public string AnonymousPlayerAccountReference = "";
		public string AnonymousResetPassword = "";
		//-------------------------------------------------------------------------------------------------------------
		static public NWDOperationWebAccount AddOperation (string sName,
		                                                   BTBOperationBlock sSuccessBlock = null, 
		                                                   BTBOperationBlock sFailBlock = null, 
		                                                   BTBOperationBlock sCancelBlock = null, 
		                                                   BTBOperationBlock sProgressBlock = null,
		                                                   NWDAppEnvironment sEnvironment = null, bool sPriority = false)
		{
			NWDOperationWebAccount rReturn = NWDOperationWebAccount.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		static public NWDOperationWebAccount Create (string sName,
		                                             BTBOperationBlock sSuccessBlock = null, 
		                                             BTBOperationBlock sFailBlock = null, 
		                                             BTBOperationBlock sCancelBlock = null, 
		                                             BTBOperationBlock sProgressBlock = null,
		                                             NWDAppEnvironment sEnvironment = null)
		{
			NWDOperationWebAccount rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation Account";
			}
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance.SelectedEnvironment ();
			}

			// IF BTBOperationUnity
			GameObject tGameObjectToSpawn = new GameObject (sName);
			rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebAccount> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;

			rReturn.Environment = sEnvironment;

			rReturn.QueueName = sEnvironment.Environment;
			rReturn.SecureData = true;

			rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);

			#if UNITY_EDITOR
			#else
			DontDestroyOnLoad (tGameObjectToSpawn);
			#endif
			// ELSE IF BTBOperationWWW
			// rReturn = new BTBOperationSynchronisation();
			// END
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override string ServerFile ()
		{
			return "account.php";
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DataUploadPrepare ()
		{
			if (Action != null) {
				if (Action == "signin" || Action == "facebook" || Action == "google" || Action == "session") {
					// TODO : check if work correctly 
					Data = NWDDataManager.SharedInstance.SynchronizationPushClassesDatas (Environment, false, NWDDataManager.SharedInstance.mTypeAccountDependantList);
					//Data = new Dictionary<string, object> ();
				} else {
					// TODO : check if work correctly 
					Data = NWDDataManager.SharedInstance.SynchronizationPushClassesDatas (Environment, false, NWDDataManager.SharedInstance.mTypeSynchronizedList);
				}
				if (Data.ContainsKey (ActionKey)) {
					Data [ActionKey] = Action;
				} else {
					Data.Add (ActionKey, Action);
				}
			}
			if (Email != null) {
				string tEmail = BTBSecurityTools.GenerateSha(Email + Environment.SaltStart, BTBSecurityShaTypeEnum.Sha1);
				if (Data.ContainsKey (EmailKey)) {
					Data [EmailKey] = tEmail;
				} else {
					Data.Add (EmailKey, tEmail);
				}
			}
			if (Password != null) {
				string tPassword = BTBSecurityTools.GenerateSha(Password + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				if (Data.ContainsKey (PasswordKey)) {
					Data [PasswordKey] = tPassword;
				} else {
					Data.Add (PasswordKey, tPassword);
				}
			}
			if (EmailHash != null) {
				string tEmailHash = EmailHash;
				if (Data.ContainsKey (EmailKey)) {
					Data [EmailKey] = tEmailHash;
				} else {
					Data.Add (EmailKey, tEmailHash);
				}
			}
			if (EmailRescue != null) {
				string tEmailRescue = EmailRescue;
				if (Data.ContainsKey (EmailRescueKey)) {
					Data [EmailRescueKey] = tEmailRescue;
				} else {
					Data.Add (EmailRescueKey, tEmailRescue);
				}
			}
			if (PasswordHash != null) {
				string tPasswordHash = PasswordHash;
				if (Data.ContainsKey (PasswordKey)) {
					Data [PasswordKey] = tPasswordHash;
				} else {
					Data.Add (PasswordKey, tPasswordHash);
				}
			}
			if (OldPassword != null) {
				string tOldPassword = BTBSecurityTools.GenerateSha(OldPassword + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				if (Data.ContainsKey (OldPasswordKey)) {
					Data [OldPasswordKey] = tOldPassword;
				} else {
					Data.Add (OldPasswordKey, tOldPassword);
				}
			}
			if (NewPassword != null) {
				string tNewPassword = BTBSecurityTools.GenerateSha(NewPassword + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				if (Data.ContainsKey (NewPasswordKey)) {
					Data [NewPasswordKey] = tNewPassword;
				} else {
					Data.Add (NewPasswordKey, tNewPassword);
				}
			}
			if (ConfirmPassword != null) {
				string tConfirmPassword = BTBSecurityTools.GenerateSha(ConfirmPassword + Environment.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				if (Data.ContainsKey (ConfirmPasswordKey)) {
					Data [ConfirmPasswordKey] = tConfirmPassword;
				} else {
					Data.Add (ConfirmPasswordKey, tConfirmPassword);
				}
			}
			if (SocialToken != null) {
				if (Data.ContainsKey (SocialTokenKey)) {
					Data [SocialTokenKey] = SocialToken;
				} else {
					Data.Add (SocialTokenKey, SocialToken);
				}
			}
			if (Action == "signout") {


				Environment.AnonymousVerification ();
				// prepare data for relog anonymous 
				if (Environment.AnonymousPlayerAccountReference != null) {
					AnonymousPlayerAccountReference = Environment.AnonymousPlayerAccountReference;
				}
				//if (Environment.AnonymousRequesToken != null) {
				//	AnonymousRequesToken = Environment.AnonymousRequesToken;
				//}
				if (Environment.AnonymousResetPassword != null) {
					AnonymousResetPassword = Environment.AnonymousResetPassword;
				}

				if (AnonymousPlayerAccountReference != null) {
					if (Data.ContainsKey (AnonymousPlayerAccountReferenceKey)) {
						Data [AnonymousPlayerAccountReferenceKey] = AnonymousPlayerAccountReference;
					} else {
						Data.Add (AnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
					}
				}
//			if (AnonymousRequesToken != null) {
//				if (Data.ContainsKey (AnonymousRequesTokenKey)) {
//					Data [AnonymousRequesTokenKey] = AnonymousRequesToken;
//				} else {
//					Data.Add (AnonymousRequesTokenKey, AnonymousRequesToken);
//				}
//			}
				if (AnonymousResetPassword != null) {
					if (Data.ContainsKey (AnonymousResetPasswordKey)) {
						Data [AnonymousResetPasswordKey] = AnonymousResetPassword;
					} else {
						Data.Add (AnonymousResetPasswordKey, AnonymousResetPassword);
					}
				}
			}

		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DataDownloadedCompute (Dictionary<string, object> sData)
		{
			Debug.Log ("NWDOperationWebAccount DataDownloadedCompute start");
			if (sData.ContainsKey ("signin")) {
				foreach (Type tType in NWDDataManager.SharedInstance.mTypeAccountDependantList) {
					var tMethodInfo = tType.GetMethod ("ResetTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
					}
				}
			}
			NWDDataManager.SharedInstance.SynchronizationPullClassesDatas (Environment, sData, NWDDataManager.SharedInstance.mTypeAccountDependantList);

			Debug.Log ("NWDOperationWebAccount DataDownloadedCompute finish");
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================