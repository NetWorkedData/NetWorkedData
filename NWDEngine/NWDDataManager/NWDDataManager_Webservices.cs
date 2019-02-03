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
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronization (bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestAllSynchronization");
			return AddWebRequestAllSynchronizationWithBlock (null, null, null, null, sPriority, sEnvironment);
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
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationSpecial(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationSpecialWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronization (List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronization");
			return AddWebRequestSynchronizationWithBlock (sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPull(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestPullWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullForce(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestPullForceWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestSynchronizationForce (List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("AddWebRequestSynchronizationForce");
			return AddWebRequestSynchronizationForceWithBlock (sTypeList, null, null, null, null, sPriority, sEnvironment);
		}
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
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization clean", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, mTypeSynchronizedList, false, sPriority, NWDOperationSpecial.Clean);
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationWithBlock (
			BTBOperationBlock sSuccessBlock = null, 
			BTBOperationBlock sErrorBlock = null, 
			BTBOperationBlock sCancelBlock = null,
			BTBOperationBlock sProgressBlock = null, 
			bool sPriority = false, NWDAppEnvironment sEnvironment = null)
		{
			//Debug.Log ("###### DEBUG ####### AddWebRequestAllSynchronizationWithBlock");
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
			//Debug.Log ("AddWebRequestSynchronizationCleanWithBlock");
			/*BTBOperationSynchronisation sOperation = */
			return NWDOperationWebSynchronisation.AddOperation ("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority, NWDOperationSpecial.Clean);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationSpecialWithBlock(List<Type> sTypeList,
            BTBOperationBlock sSuccessBlock = null,
            BTBOperationBlock sErrorBlock = null,
            BTBOperationBlock sCancelBlock = null,
            BTBOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationSpecialWithBlock");
            /*BTBOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority, NWDOperationSpecial.Special);
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
        public NWDOperationWebSynchronisation AddWebRequestPullWithBlock(List<Type> sTypeList,
                                                                                     BTBOperationBlock sSuccessBlock = null,
                                                                                     BTBOperationBlock sErrorBlock = null,
                                                                                     BTBOperationBlock sCancelBlock = null,
                                                                                     BTBOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            /*BTBOperationSynchronisation sOperation = */
            //return NWDOperationWebCheckout.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority);
            return NWDOperationWebSynchronisation.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority, NWDOperationSpecial.Pull);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullForceWithBlock(List<Type> sTypeList,
                                                                                     BTBOperationBlock sSuccessBlock = null,
                                                                                     BTBOperationBlock sErrorBlock = null,
                                                                                     BTBOperationBlock sCancelBlock = null,
                                                                                     BTBOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            /*BTBOperationSynchronisation sOperation = */
            //return NWDOperationWebCheckout.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, true, sPriority);
            return NWDOperationWebSynchronisation.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, true, sPriority, NWDOperationSpecial.Pull);
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
        public NWDOperationWebMaintenance AddWebRequestMaintenanceWithBlock(BTBOperationBlock sSuccessBlock = null,
                                                                BTBOperationBlock sErrorBlock = null,
                                                                BTBOperationBlock sCancelBlock = null,
                                                                BTBOperationBlock sProgressBlock = null,
                                                                bool sPriority = false,
                                                                NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log("AddWebRequestNoPageWithBlock");
            NWDOperationWebMaintenance sOperation = NWDOperationWebMaintenance.Create("Maintenance with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
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
                // TODO : Change to remove invoke!
                //var tMethodInfo = tType.GetMethod ("TryToChangeUserForAllObjects", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_TryToChangeUserForAllObjects);
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
        public void SynchronizationPullClassesDatas (NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, List<Type> sTypeList, NWDOperationSpecial sSpecial)
        {
            //BTBBenchmark.Start();
            //BTBBenchmark.Increment(sTypeList.Count());
            //Debug.Log("NWDDataManager SynchronizationPullClassesDatas()");
            //Debug.Log("NWDDataManager SynchronizationPullClassesDatas() THREAD ID" + System.Threading.Thread.CurrentThread.GetHashCode().ToString());
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            
            // I must autoanalyze the Type of data?
            if (sTypeList == null)
            {
                List<Type> tTypeList = SharedInstance().mTypeList;
                sTypeList = tTypeList;
            }
            
            bool tUpdateData = false;
			if (sTypeList != null)
			{
				List<string> tClassNameSync = new List<string>();
				List<string> tClassNameNotSync = new List<string>();
				List<string> tClassNameNotFound = new List<string>();
				
				foreach (Type tType in sTypeList)
                {
                    // TODO : Change to remove invoke!
                    //var tMethodInfo = tType.GetMethod ("SynchronizationPullData", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_SynchronizationPullData);

                    if (tMethodInfo != null)
                    {
                        string tResult = tMethodInfo.Invoke(null, new object[] { sInfos, sEnvironment, sData, sSpecial }) as string;
                        if (tResult == "YES")
                        {
                            tUpdateData = true;
                            tClassNameSync.Add(tType.Name);
                        }
                        else
                        {
	                        tClassNameNotSync.Add(tType.Name);
                        }
                    }
                    else
                    {
	                    tClassNameNotFound.Add(tType.Name);
                    }
				}
				
				////////////////////////////////////////////////////////////////////////////////////////////////////////
				/*Debug.LogWarning("---------------------------------------------------");
				Debug.LogWarning("class Updated : " + tClassNameSync.Count);
				Debug.LogWarning("class not Updated : " + tClassNameNotSync.Count);
				Debug.LogWarning("class not Found : " + tClassNameNotFound.Count);
				Debug.LogWarning("---------------------------------------------------");
				Debug.LogWarning("class not Updated List :");
				tClassNameNotSync.Sort((x, y) => String.Compare(x, y, StringComparison.Ordinal));
				foreach (string k in tClassNameNotSync)
				{
					Debug.LogWarning("" + k);
				}
				Debug.LogWarning("---------------------------------------------------");*/
				////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
			
            //BTBBenchmark.Start("DataQueueExecute");
            SharedInstance().DataQueueExecute();
            //BTBBenchmark.Finish("DataQueueExecute");
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            
			if (tUpdateData)
            {
                BTBNotificationManager.SharedInstance().PostNotification (new BTBNotification (NWDNotificationConstants.K_DATAS_WEB_UPDATE, null));
            }
			
            //BTBBenchmark.Finish();
		}
		//-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> SynchronizationPushClassesDatas (NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList, NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
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
                    // TODO : Change to remove invoke!
                    //var tMethodInfo = tType.GetMethod ("SynchronizationPushData", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_SynchronizationPushData);

                    if (tMethodInfo != null) {
                        Dictionary<string, object> rSendPartial = tMethodInfo.Invoke (null, new object[]{sInfos, sEnvironment, sForceAll, sSpecial }) as Dictionary<string, object>;
						foreach (string tKey in rSendPartial.Keys) {
							rSend.Add (tKey, rSendPartial [tKey]);
						}
					}
				}
			}
			return rSend;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> CheckoutPushClassesDatas(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList, NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
        {

            //#if UNITY_EDITOR 
            sInfos.ClassPushCounter = 0;
            sInfos.ClassPullCounter = 0;
            sInfos.RowPullCounter = 0;
            sInfos.RowPushCounter = 0;
            //#endif

            Dictionary<string, object> rSend = new Dictionary<string, object>();
            if (sTypeList != null)
            {
                foreach (Type tType in sTypeList)
                {
                    // TODO : Change to remove invoke!
                    //var tMethodInfo = tType.GetMethod("CheckoutPushData", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_CheckoutPushData);
                    if (tMethodInfo != null)
                    {
                        Dictionary<string, object> rSendPartial = tMethodInfo.Invoke(null, new object[] { sInfos, sEnvironment, sForceAll, sSpecial }) as Dictionary<string, object>;
                        foreach (string tKey in rSendPartial.Keys)
                        {
                            rSend.Add(tKey, rSendPartial[tKey]);
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
                // TODO : Change to remove invoke!
                //var tMethodInfo = tType.GetMethod ("DeleteUser", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_DeleteUser);

                if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, new object[]{ sEnvironment });
				}
			}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================