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

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//	#if UNITY_EDITOR
	//	public class BTBOperationControllerMenuTest
	//	{
	//		static string KEmail = "";
	//		static string KPassword = "";
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/test", false, 00)]
	//		public static void Test ()
	//		{
	//			NWDOperationWebUnity.AddOperation ("test");
	////			SharedInstance.AddWebRequestAllSynchronization ();
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/reset", false, 20)]
	//		public static void AccountReset ()
	//		{
	//			NWDAppConfiguration.SharedInstance().SelectedEnvironment ().ResetSession ();
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/session test", false, 20)]
	//		public static void SessionReset ()
	//		{
	//			SharedInstance.AddWebRequestSessionWithBlock(delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
	//				Debug.Log("####### Progress");
	//			}, delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
	//				Debug.Log("####### Finish");
	//			}, delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
	//				Debug.Log("####### Error");
	//			}, delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos) {
	//				Debug.Log("####### Cancel");
	//			},false,NWDAppConfiguration.SharedInstance().SelectedEnvironment ()
	//			);
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/sign-up", false, 22)]
	//		public static void AccountSignUp ()
	//		{
	//			KEmail = NWDToolbox.RandomStringUnix (8) + "@idemobi.com";
	//			KPassword = NWDToolbox.RandomString (18);
	//			KPassword = "1234";
	//			Debug.Log ("Refrence : " + NWDAppConfiguration.SharedInstance().SelectedEnvironment ().PlayerAccountReference + " Email : " + KEmail + " Password : " + KPassword);
	//
	//			SharedInstance.AddWebRequestSignUp (KEmail, KPassword, KPassword);
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/sign-out", false, 23)]
	//		public static void AccountSignOut ()
	//		{
	//			SharedInstance.AddWebRequestSignOut ();
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/sign-in", false, 24)]
	//		public static void AccountSignIn ()
	//		{
	//			SharedInstance.AddWebRequestSignIn (KEmail, KPassword);
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/modifiy", false, 25)]
	//		public static void AccountModify ()
	//		{
	//			string tOldPassword = KPassword + "";
	//			KEmail = NWDToolbox.RandomStringUnix (8) + "@idemobi.com";
	//			KPassword = NWDToolbox.RandomString (18);
	//			SharedInstance.AddWebRequestSignModify (KEmail, tOldPassword, KPassword, KPassword);
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/delete", false, 26)]
	//		public static void AccountDelete ()
	//		{
	//			SharedInstance.AddWebRequestSignDelete (KPassword, KPassword);
	//		}
	//
	//
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/FLUSH QUEUE", false, 999)]
	//		public static void FlushQueue ()
	//		{
	//			SharedInstance.WebRequestFlush ();
	//		}
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/INFOS QUEUE", false, 999)]
	//		public static void InfosQueue ()
	//		{
	//			SharedInstance.WebRequestInfos ();
	//		}
	//
	//
	//
	//		//-------------------------------------------------------------------------------------------------------------
	//		[MenuItem ("NWDWEB/AccountSequence A", false, 100)]
	//		public static void AccountSequence ()
	//		{
	//			NWDAppConfiguration.SharedInstance().SelectedEnvironment ().ResetSession ();
	//
	//			KEmail = NWDToolbox.RandomStringUnix (8) + "@idemobi.com";
	//			KPassword = NWDToolbox.RandomString (18);
	//			SharedInstance.AddWebRequestSignUp (KEmail, KPassword, KPassword);
	//
	//			SharedInstance.AddWebRequestSignOut ();
	//
	//			SharedInstance.AddWebRequestSignIn (KEmail, KPassword);
	//
	//			KEmail = NWDToolbox.RandomStringUnix (8) + "@idemobi.com";
	//			string tOldPassword = KPassword + "";
	//			KPassword = NWDToolbox.RandomString (18);
	//			SharedInstance.AddWebRequestSignModify (KEmail, tOldPassword, KPassword, KPassword);
	//
	//			SharedInstance.AddWebRequestSignIn (KEmail, KPassword);
	//
	//			SharedInstance.AddWebRequestSignOut ();
	//
	//			SharedInstance.AddWebRequestSignIn (KEmail, KPassword);
	//
	//			SharedInstance.AddWebRequestSignDelete (KPassword, KPassword);
	//
	//			SharedInstance.AddWebRequestSignIn (KEmail, KPassword);
	//		}
	//	}
	//
	//	#endif

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNetworkState
    {
        Unknow,
        OnLine,
        OffLine,

        Check,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public BTBOperationController WebOperationQueue = new BTBOperationController ();
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationClean (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronization");
			return AddWebRequestAllSynchronizationWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronization (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronization");
			return AddWebRequestAllSynchronizationCleanWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationForce (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronizationForce");
			return AddWebRequestAllSynchronizationForceWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationClean (List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronization");
			return AddWebRequestSynchronizationCleanWithBlock (sTypeList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronization (List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronization");
			return AddWebRequestSynchronizationWithBlock (sTypeList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationForce (List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationForce");
			return AddWebRequestSynchronizationForceWithBlock (sTypeList, null, null, null, null, sPriority, sEnvironment);
		}
		//		public List<Type> mTypeAccountDependantList = new List<Type> ();
		//		public List<Type> mTypeNotAccountDependantList = new List<Type> ();
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestNotAccountDependantSynchronization (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronization");
			return AddWebRequestSynchronizationWithBlock (mTypeNotAccountDependantList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestNotAccountDependantSynchronizationForce (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationForce");
			return AddWebRequestSynchronizationForceWithBlock (mTypeNotAccountDependantList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAccountDependantSynchronization (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronization");
			return AddWebRequestSynchronizationWithBlock (mTypeAccountDependantList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAccountDependantSynchronizationForce (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationForce");
			return AddWebRequestSynchronizationForceWithBlock (mTypeAccountDependantList, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSession (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSession");
			return AddWebRequestSessionWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignUp (string sEmail, string sPassword, string sConfirmPassword, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignUp");
			return AddWebRequestSignUpWithBlock (sEmail, sPassword, sConfirmPassword, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignIn (string sEmail, string sPassword, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("NWDOperationWebAccount");
			return AddWebRequestSignInWithBlock (sEmail, sPassword, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignTest(string sEmailHash, string sPasswordHash, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("NWDOperationWebAccount");
            return AddWebRequestSignTestWithBlock(sEmailHash, sPasswordHash, null, null, null, null, sPriority, sEnvironment);
        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignOut (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignOut");
			return AddWebRequestSignOutWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestAnonymousRestaure (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAnonymousRestaure");
			return AddWebRequestAnonymousRestaureWithBlock (null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignModify (string sEmail, string sOldPassword, string sNewPassword, string sConfirmPassword, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignModify");
			return AddWebRequestSignModifyWithBlock (sEmail, sOldPassword, sNewPassword, sConfirmPassword, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestRescue (string sEmail, string sAppName, string sAppMail, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignModify");
			return AddWebRequestRescueWithBlock (sEmail, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignDelete (string sPassword, string sConfirmPassword, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignDelete");
			return AddWebRequestSignDeleteWithBlock (sPassword, sConfirmPassword, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestLogFacebook (string sSocialToken, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestLogFacebook");
			return AddWebRequestLogFacebookWithBlock (sSocialToken, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestLogGoogle (string sSocialToken, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestLogGoogle");
			return AddWebRequestLogGoogleWithBlock (sSocialToken, null, null, null, null, sPriority, sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationCleanWithBlock (
			BTBOperationBlock sSuccessBlock = null, 
			BTBOperationBlock sErrorBlock = null, 
			BTBOperationBlock sCancelBlock = null,
			BTBOperationBlock sProgressBlock = null, 
			bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronizationWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization clean", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, mTypeSynchronizedList, false, sPriority, true);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationWithBlock (
			BTBOperationBlock sSuccessBlock = null, 
			BTBOperationBlock sErrorBlock = null, 
			BTBOperationBlock sCancelBlock = null,
			BTBOperationBlock sProgressBlock = null, 
			bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronizationWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, mTypeSynchronizedList, false, sPriority);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationForceWithBlock (
			BTBOperationBlock sSuccessBlock = null, 
			BTBOperationBlock sErrorBlock = null, 
			BTBOperationBlock sCancelBlock = null,
			BTBOperationBlock sProgressBlock = null, 
			bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronizationForceWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization force", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, mTypeSynchronizedList, true, sPriority);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationCleanWithBlock (List<Type> sTypeList,
			BTBOperationBlock sSuccessBlock = null, 
			BTBOperationBlock sErrorBlock = null, 
			BTBOperationBlock sCancelBlock = null,
			BTBOperationBlock sProgressBlock = null, 
			bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority, true);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationWithBlock (List<Type> sTypeList,
		                                                                             BTBOperationBlock sSuccessBlock = null, 
		                                                                             BTBOperationBlock sErrorBlock = null, 
		                                                                             BTBOperationBlock sCancelBlock = null,
		                                                                             BTBOperationBlock sProgressBlock = null, 
		                                                                             bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationForceWithBlock (List<Type> sTypeList,
		                                                                                  BTBOperationBlock sSuccessBlock = null, 
		                                                                                  BTBOperationBlock sErrorBlock = null, 
		                                                                                  BTBOperationBlock sCancelBlock = null,
		                                                                                  BTBOperationBlock sProgressBlock = null, 
		                                                                                  bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationForceWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, true, sPriority);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSessionWithBlock (BTBOperationBlock sSuccessBlock = null, 
                                                        			 BTBOperationBlock sErrorBlock = null, 
                                                        			 BTBOperationBlock sCancelBlock = null,
                                                        			 BTBOperationBlock sProgressBlock = null,
                                                        			 bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSessionWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Session with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "session";
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignUpWithBlock (string sEmail, string sPassword, string sConfirmPassword,
		                                                            BTBOperationBlock sSuccessBlock = null, 
		                                                            BTBOperationBlock sErrorBlock = null, 
		                                                            BTBOperationBlock sCancelBlock = null,
		                                                            BTBOperationBlock sProgressBlock = null, 
		                                                            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignUpWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Sign-Up with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "signup";
			sOperation.Email = sEmail;
			sOperation.Password = sPassword;
			sOperation.ConfirmPassword = sConfirmPassword;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignInWithBlock (string sEmail, string sPassword,
		                                                            BTBOperationBlock sSuccessBlock = null, 
		                                                            BTBOperationBlock sErrorBlock = null, 
		                                                            BTBOperationBlock sCancelBlock = null, 
		                                                            BTBOperationBlock sProgressBlock = null, 
		                                                            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignInWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Sign-in with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "signin";
			sOperation.Email = sEmail;
			sOperation.Password = sPassword;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignTestWithBlock(string sEmailHash, string sPasswordHash,
                                                                    BTBOperationBlock sSuccessBlock = null,
                                                                    BTBOperationBlock sErrorBlock = null,
                                                                    BTBOperationBlock sCancelBlock = null,
                                                                    BTBOperationBlock sProgressBlock = null,
                                                                    bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestSignInWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Account Sign-test with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "signin";
            sOperation.EmailHash = sEmailHash;
            sOperation.PasswordHash = sPasswordHash;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignOutWithBlock (BTBOperationBlock sSuccessBlock = null, 
                                                        			 BTBOperationBlock sErrorBlock = null, 
                                                        			 BTBOperationBlock sCancelBlock = null, 
                                                        			 BTBOperationBlock sProgressBlock = null, 
                                                        			 bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignOutWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Sign-out with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "signout";
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestAnonymousRestaureWithBlock (BTBOperationBlock sSuccessBlock = null, 
                                                                			   BTBOperationBlock sErrorBlock = null, 
                                                                			   BTBOperationBlock sCancelBlock = null, 
                                                                			   BTBOperationBlock sProgressBlock = null, 
                                                                			   bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAnonymousRestaureWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Sign-out with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "signanonymous";
			if (sEnvironment != null) {
				sOperation.AnonymousPlayerAccountReference = sEnvironment.AnonymousPlayerAccountReference;
				sOperation.AnonymousResetPassword = sEnvironment.AnonymousResetPassword;
			}
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignModifyWithBlock (string sEmail, string sOldPassword, string sNewPassword, string sConfirmPassword,
		                                                                BTBOperationBlock sSuccessBlock = null, 
		                                                                BTBOperationBlock sErrorBlock = null, 
		                                                                BTBOperationBlock sCancelBlock = null,
		                                                                BTBOperationBlock sProgressBlock = null,
		                                                                bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignModifyWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Modifiy with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "modify";
			sOperation.Email = sEmail;
			sOperation.OldPassword = sOldPassword;
			sOperation.NewPassword = sNewPassword;
			sOperation.ConfirmPassword = sConfirmPassword;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestRescueWithBlock (string sEmail,
		                                                            BTBOperationBlock sSuccessBlock = null, 
		                                                            BTBOperationBlock sErrorBlock = null, 
		                                                            BTBOperationBlock sCancelBlock = null,
		                                                            BTBOperationBlock sProgressBlock = null,
		                                                            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestRescueWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Modifiy with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "modify";
			sOperation.EmailRescue = sEmail;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestSignDeleteWithBlock (string sPassword, string sConfirmPassword,
		                                                                BTBOperationBlock sSuccessBlock = null, 
		                                                                BTBOperationBlock sErrorBlock = null, 
		                                                                BTBOperationBlock sCancelBlock = null,
		                                                                BTBOperationBlock sProgressBlock = null, 
		                                                                bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSignDeleteWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Delete with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "delete";
			sOperation.Password = sPassword;
			sOperation.ConfirmPassword = sConfirmPassword;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestLogFacebookWithBlock (string sSocialToken,
		                                                                 BTBOperationBlock sSuccessBlock = null, 
		                                                                 BTBOperationBlock sErrorBlock = null, 
		                                                                 BTBOperationBlock sCancelBlock = null,
		                                                                 BTBOperationBlock sProgressBlock = null, 
		                                                                 bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestLogFacebookWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Facebook with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "facebook";
			sOperation.SocialToken = sSocialToken;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebAccount AddWebRequestLogGoogleWithBlock (string sSocialToken,
		                                                               BTBOperationBlock sSuccessBlock = null, 
		                                                               BTBOperationBlock sErrorBlock = null, 
		                                                               BTBOperationBlock sCancelBlock = null,
		                                                               BTBOperationBlock sProgressBlock = null, 
		                                                               bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestLogGoogleWithBlock");
			NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Account Google with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
			sOperation.Action = "google";
			sOperation.SocialToken = sSocialToken;
            SharedInstance().WebOperationQueue.AddOperation (sOperation, sPriority);
			return sOperation;
		}






        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestRemoveFacebookWithBlock(string sSocialToken,
                                                                         BTBOperationBlock sSuccessBlock = null,
                                                                         BTBOperationBlock sErrorBlock = null,
                                                                         BTBOperationBlock sCancelBlock = null,
                                                                         BTBOperationBlock sProgressBlock = null,
                                                                         bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestLogFacebookWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Disocciate Facebook with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "facebook_remove";
            sOperation.SocialToken = sSocialToken;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestRemoveGoogleWithBlock(string sSocialToken,
                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestLogGoogleWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Disocciate Google with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "google_remove";
            sOperation.SocialToken = sSocialToken;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
		//-------------------------------------------------------------------------------------------------------------
		public void WebRequestFlush (NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("WebRequestFlush");
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
			}
			WebOperationQueue.Flush (sEnvironment.Environment);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void WebRequestInfos (NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("WebRequestInfos");
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
			}
			WebOperationQueue.Infos (sEnvironment.Environment);
		}

        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebBlank AddWebRequestBlankWithBlock(BTBOperationBlock sSuccessBlock = null,
                                                                BTBOperationBlock sErrorBlock = null,
                                                                BTBOperationBlock sCancelBlock = null,
                                                                BTBOperationBlock sProgressBlock = null,
                                                                bool sPriority = false, 
                                                                NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestBlankWithBlock");
            NWDOperationWebBlank sOperation = NWDOperationWebBlank.Create("Blank with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "arghhh";
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebNoPage AddWebRequestNoPageWithBlock(BTBOperationBlock sSuccessBlock = null,
                                                                BTBOperationBlock sErrorBlock = null,
                                                                BTBOperationBlock sCancelBlock = null,
                                                                BTBOperationBlock sProgressBlock = null,
                                                                bool sPriority = false,
                                                                NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestNoPageWithBlock");
            NWDOperationWebNoPage sOperation = NWDOperationWebNoPage.Create("NoPage with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "arghhh";
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The synchronize in progress.
		/// </summary>
		//		public static bool SynchronizeInProgress = false;
		//		/// <summary>
		//		/// The synchronize is in progress and another task want connect.
		//		/// Of course because token flow integirty, it's not possible... So I could memorize the task an run after. 
		//		/// But it's too long and difficult : I prefer to ask new Synchronise all 
		//		/// Memorize to repeat as soon as possible without lost the token flow integrity
		//		/// </summary>
		//		public static bool SynchronizeRepeat = false;
		//		/// <summary>
		//		/// The synchronize is in progress and another task ask repeat but in force (update all begin timestamp 0.
		//		/// So Memorize force required
		//		/// </summary>
		//		public static bool SynchronizeRepeatInForce = false;
		//-------------------------------------------------------------------------------------------------------------
        public void ChangeAllDatasForUserToAnotherUser (NWDAppEnvironment sEnvironment, string sNewAccountReference, string sAnonymousResetPassword)
		{
			// change account refrence 
			// generate new Reference for this objetc (based on account reference)
            //Debug.Log("NWDDataManager ChangeAllDatasForUserToAnotherUser()");

            if (sEnvironment.AnonymousPlayerAccountReference == sEnvironment.PlayerAccountReference)
            {
                sEnvironment.AnonymousPlayerAccountReference = sNewAccountReference;
                sEnvironment.AnonymousResetPassword = sAnonymousResetPassword;
            }

			foreach (Type tType in mTypeList)
            {
				var tMethodInfo = tType.GetMethod ("TryToChangeUserForAllObjects", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null)
                {
					tMethodInfo.Invoke (null, new object[]{ sEnvironment.PlayerAccountReference, sNewAccountReference });
				}
			}
			sEnvironment.PlayerAccountReference = sNewAccountReference;

			#if UNITY_EDITOR
            // NEVER DO THAT!!!!
            //NWDAccount tAccount = NWDBasis<NWDAccount>.NewObjectWithReference (sEnvironment.PlayerAccountReference);
            //tAccount.SaveModifications();
            #endif

            //SavePreferences (NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            SavePreferences(sEnvironment);
		}
		//-------------------------------------------------------------------------------------------------------------
        public void SynchronizationPullClassesDatas (NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, List<Type> sTypeList)
        {
            //Debug.Log("NWDDataManager SynchronizationPullClassesDatas()");
            //Debug.Log("NWDDataManager SynchronizationPullClassesDatas() THREAD ID" + System.Threading.Thread.CurrentThread.GetHashCode().ToString());

            // I must autoanalyze the Type of data?
            if (sTypeList == null)
            {
                List<Type> tTypeList = NWDDataManager.SharedInstance().mTypeList;
                sTypeList = tTypeList;
            }

            bool sUpdateData = false;
			if (sTypeList != null)
            {
				foreach (Type tType in sTypeList)
                {
                    //Debug.Log("NWDDataManager SynchronizationPullClassesDatas() tType = " + tType.Name);

					var tMethodInfo = tType.GetMethod ("SynchronizationPullData", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        string tResult = tMethodInfo.Invoke(null, new object[] { sInfos, sEnvironment, sData }) as string;
                        if (tResult == "YES")
                        {
                            sUpdateData = true;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SynchronizationPullData not found for "+ tType.Name);
                    }
				}
			}

			if (sUpdateData == true)
            {
                BTBNotificationManager.SharedInstance().PostNotification (new BTBNotification (NWDNotificationConstants.K_DATAS_WEB_UPDATE, null));
			}
		}
		//-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> SynchronizationPushClassesDatas (NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList, bool sClean = false)
		{

//#if UNITY_EDITOR 
            sInfos.ClassPushCounter =0;
            sInfos.ClassPullCounter = 0;
            sInfos.RowPullCounter = 0;
            sInfos.RowPushCounter = 0;
//#endif

            Dictionary<string, object> rSend = new Dictionary<string, object> ();
			if (sTypeList != null) {
				foreach (Type tType in sTypeList) {
					var tMethodInfo = tType.GetMethod ("SynchronizationPushData", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
                        Dictionary<string, object> rSendPartial = tMethodInfo.Invoke (null, new object[]{sInfos, sEnvironment, sForceAll, sClean }) as Dictionary<string, object>;
						foreach (string tKey in rSendPartial.Keys) {
							rSend.Add (tKey, rSendPartial [tKey]);
						}
					}
				}
			}
			return rSend;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void TokenError (NWDAppEnvironment sEnvironment)
		{
			DeleteUser (sEnvironment);
			//TODO : Restaure anonymous account

		}
		//-------------------------------------------------------------------------------------------------------------
		public void DeleteUser (NWDAppEnvironment sEnvironment)
		{
			foreach (Type tType in mTypeList) {
				var tMethodInfo = tType.GetMethod ("DeleteUser", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, new object[]{ sEnvironment });
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		//		public bool SynchronizationClassesDatas (NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList, bool sBackground = true, bool sLoader = false)
		//		{
		//
		//			Debug.Log ("######### SynchronizationClassesDatas start " + Time.time.ToString ());
		//			bool rReturn = false;
		//			if (SynchronizeInProgress == false) {
		//				SynchronizeInProgress = true;
		//				rReturn = true;
		//				// I write all data
		//				UpdateQueueExecute ();
		//
		//				#if UNITY_EDITOR
		//				// Deselect all object
		//				Selection.activeObject = null;
		//				#endif
		//
		//				BTBUnityWebServiceDataRequest request = NWDWebRequestor.ShareInstance ().RequestDataRequestorWebService (sEnvironment, SynchronizationPushClassesDatas (sEnvironment, sForceAll, sTypeList));
		//				request.successBlockDelegate = delegate(Dictionary<string, object> data) {
		//					// test if error
		//					NWDWebRequestorResult tResult = NWDWebRequestor.RespondAnalyzeIsValid (sEnvironment, data);
		//					if (tResult == NWDWebRequestorResult.Success) {
		//						SynchronizationPullClassesDatas (sEnvironment, data, sTypeList);
		//					} else if (tResult == NWDWebRequestorResult.NewUser) {
		//						// I must resend my request
		//						SynchronizationClassesDatas (sEnvironment, sForceAll, sTypeList, sBackground, sLoader);
		//					} else if (tResult == NWDWebRequestorResult.Error) {
		//
		//					}
		//					SynchronizeInProgress = false;
		//					if (SynchronizeRepeat == true) {
		//						bool tForceAll = SynchronizeRepeatInForce;
		//						SynchronizeRepeat = false;
		//						SynchronizeRepeatInForce = false;
		//						SynchronizeAllData (sEnvironment, tForceAll);
		//					}
		//
		//					Debug.Log ("######### SynchronizationClassesDatas finish success " + Time.time.ToString ());
		//				};
		//				request.errorBlockDelegate = delegate(Error error) {
		//					Debug.Log ("Error: " + error.code + " // " + error.localizedDescription);
		//					SynchronizeInProgress = false;
		//					if (SynchronizeRepeat == true) {
		//						bool tForceAll = SynchronizeRepeatInForce;
		//						SynchronizeRepeat = false;
		//						SynchronizeRepeatInForce = false;
		//						SynchronizeAllData (sEnvironment, tForceAll);
		//					}
		//
		//					Debug.Log ("######### SynchronizationClassesDatas finish error " + Time.time.ToString ());
		//				};
		//
		//				Debug.Log ("Synchronization send");
		//				request.send ();
		//			} else {
		//				SynchronizeRepeat = true;
		//				if (sForceAll == true) {
		//					SynchronizeRepeatInForce = sForceAll;
		//				}
		//
		//				Debug.Log ("Synchronization all ready in progress ... prepare to repeat but in force if one ask it ?");
		//			}
		//			return rReturn;
		//		}

		//		public bool SynchronizationAllClassDatas (NWDAppEnvironment sEnvironment, bool sForceAll, bool sBackground = true, bool sLoader = false)
		//		{
		//			return SynchronizationClassesDatas (sEnvironment, sForceAll, mTypeSynchronizedList, sBackground, sLoader);
		//		}
		//
		//		public bool SynchronizeAllData (NWDAppEnvironment sEnvironment, bool sForceAll)
		//		{
		//			//Debug.Log ("SynchronizeAllData");
		//			return SynchronizationAllClassDatas (sEnvironment, sForceAll);
		//		}

		//		public bool FirstSynchronisation (NWDAppEnvironment sEnvironment)
		//		{
		//			bool rReturn = false;
		//			if (SynchronizeInProgress == false) {
		//				rReturn = true;
		//				SynchronizeInProgress = true;
		//				Dictionary<string,object> tDico = new Dictionary<string,object> ();
		//				tDico.Add ("test", "test");
		//				BTBUnityWebServiceDataRequest request = NWDWebRequestor.ShareInstance ().RequestDataRequestorWebService (sEnvironment, tDico);
		//				request.successBlockDelegate = delegate(Dictionary<string, object> data) {
		//					SynchronizeInProgress = false;
		//					NWDWebRequestorResult tResult = NWDWebRequestor.RespondAnalyzeIsValid (sEnvironment, data);
		//					if (tResult == NWDWebRequestorResult.Success) {
		//					} else if (tResult == NWDWebRequestorResult.NewUser) {
		//					} else if (tResult == NWDWebRequestorResult.Error) {
		//					}
		//				};
		//				request.errorBlockDelegate = delegate(Error error) {
		//					SynchronizeInProgress = false;
		//					Debug.Log ("Error: " + error.code + " // " + error.localizedDescription);
		//				};
		//
		//				Debug.Log ("webservice send");
		//				request.send ();
		//			} else {
		//				//SynchronizeRepeat = true;
		//				// Not necessairy to repeat, the first connection is in pogress by another task :-)
		//
		//				Debug.Log ("webservice all ready in progress");
		//			}
		//			return rReturn;
		//		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================