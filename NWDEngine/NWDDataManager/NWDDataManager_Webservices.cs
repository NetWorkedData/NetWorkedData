//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

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
        public NWEOperationController WebOperationQueue = new NWEOperationController();
        public NWEOperationController AssetOperationQueue = new NWEOperationController();
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAllSynchronization(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestAllSynchronization");
            return AddWebRequestAllSynchronizationWithBlock(null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationForce(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestAllSynchronizationForce");
            return AddWebRequestAllSynchronizationForceWithBlock(null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationClean(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationCleanWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationSpecial(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationSpecialWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronization(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPull(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestPullWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullReferences(Dictionary<Type, List<string>> sTypeAndReferences, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestPullReferencesWithBlock(sTypeAndReferences, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPushReferences(Dictionary<Type, List<string>> sTypeAndReferences, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestPushReferences");
            return AddWebRequestPushReferencesWithBlock(sTypeAndReferences, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullForce(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestPullForceWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationForce(List<Type> sTypeList, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationForce");
            return AddWebRequestSynchronizationForceWithBlock(sTypeList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestNotAccountDependantSynchronization(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationWithBlock(ClassNotAccountDependentList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestNotAccountDependantSynchronizationForce(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationForce");
            return AddWebRequestSynchronizationForceWithBlock(ClassNotAccountDependentList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAccountDependantSynchronization(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronization");
            return AddWebRequestSynchronizationWithBlock(ClassAccountDependentList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAccountDependantSynchronizationForce(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationForce");
            return AddWebRequestSynchronizationForceWithBlock(ClassAccountDependentList, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignUp(NWDAccountSignType sSignType, string sSignHash, string sRescueHash, string sLoginHash, bool sPriority = true, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("NWDOperationWebAccount");
            return AddWebRequestSignUpWithBlock(sSignType, sSignHash, sRescueHash, sLoginHash, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignIn(string sPasswordToken, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("NWDOperationWebAccount");
            return AddWebRequestSignInWithBlock(sPasswordToken, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignOut(bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSignOut");
            return AddWebRequestSignOutWithBlock(null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestRescue(string sEmail, bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSignModify");
            return AddWebRequestRescueWithBlock(sEmail, null, null, null, null, sPriority, sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationCleanWithBlock(
            NWEOperationBlock sSuccessBlock = null,
            NWEOperationBlock sErrorBlock = null,
            NWEOperationBlock sCancelBlock = null,
            NWEOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestAllSynchronizationWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization clean", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, ClassSynchronizeList, null, false, sPriority, NWDOperationSpecial.Clean);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationWithBlock(
            NWEOperationBlock sSuccessBlock = null,
            NWEOperationBlock sErrorBlock = null,
            NWEOperationBlock sCancelBlock = null,
            NWEOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("###### DEBUG ####### AddWebRequestAllSynchronizationWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, ClassSynchronizeList, null, false, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestAllSynchronizationForceWithBlock(
            NWEOperationBlock sSuccessBlock = null,
            NWEOperationBlock sErrorBlock = null,
            NWEOperationBlock sCancelBlock = null,
            NWEOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestAllSynchronizationForceWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization force", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, ClassSynchronizeList, null, true, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationCleanWithBlock(List<Type> sTypeList,
            NWEOperationBlock sSuccessBlock = null,
            NWEOperationBlock sErrorBlock = null,
            NWEOperationBlock sCancelBlock = null,
            NWEOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationCleanWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, false, sPriority, NWDOperationSpecial.Clean);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationSpecialWithBlock(List<Type> sTypeList,
            NWEOperationBlock sSuccessBlock = null,
            NWEOperationBlock sErrorBlock = null,
            NWEOperationBlock sCancelBlock = null,
            NWEOperationBlock sProgressBlock = null,
            bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationSpecialWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, false, sPriority, NWDOperationSpecial.Special);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationWithBlock(List<Type> sTypeList,
                                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                                     NWEOperationBlock sErrorBlock = null,
                                                                                     NWEOperationBlock sCancelBlock = null,
                                                                                     NWEOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, false, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullWithBlock(List<Type> sTypeList,
                                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                                     NWEOperationBlock sErrorBlock = null,
                                                                                     NWEOperationBlock sCancelBlock = null,
                                                                                     NWEOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            /*NWEOperationSynchronisation sOperation = */
            //return NWDOperationWebCheckout.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, false, sPriority);
            return NWDOperationWebSynchronisation.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, false, sPriority, NWDOperationSpecial.Pull);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullReferencesWithBlock(Dictionary<Type, List<string>> sTypeAndReferences,
                                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                                     NWEOperationBlock sErrorBlock = null,
                                                                                     NWEOperationBlock sCancelBlock = null,
                                                                                     NWEOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            return NWDOperationWebSynchronisation.AddOperation("Pull Reeference", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, null, sTypeAndReferences, false, sPriority, NWDOperationSpecial.PullReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPushReferencesWithBlock(Dictionary<Type, List<string>> sTypeAndReferences,
                                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                                     NWEOperationBlock sErrorBlock = null,
                                                                                     NWEOperationBlock sCancelBlock = null,
                                                                                     NWEOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestPushReferencesWithBlock");
            return NWDOperationWebSynchronisation.AddOperation("Push Reeference", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, null, sTypeAndReferences, false, sPriority, NWDOperationSpecial.PushReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestPullForceWithBlock(List<Type> sTypeList,
                                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                                     NWEOperationBlock sErrorBlock = null,
                                                                                     NWEOperationBlock sCancelBlock = null,
                                                                                     NWEOperationBlock sProgressBlock = null,
                                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationWithBlock");
            return NWDOperationWebSynchronisation.AddOperation("Pull", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, true, sPriority, NWDOperationSpecial.Pull);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebSynchronisation AddWebRequestSynchronizationForceWithBlock(List<Type> sTypeList,
                                                                                          NWEOperationBlock sSuccessBlock = null,
                                                                                          NWEOperationBlock sErrorBlock = null,
                                                                                          NWEOperationBlock sCancelBlock = null,
                                                                                          NWEOperationBlock sProgressBlock = null,
                                                                                          bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSynchronizationForceWithBlock");
            return NWDOperationWebSynchronisation.AddOperation("Synchronization", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, null, true, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignInWithBlock(string sSign,
                                                                    NWEOperationBlock sSuccessBlock = null,
                                                                    NWEOperationBlock sErrorBlock = null,
                                                                    NWEOperationBlock sCancelBlock = null,
                                                                    NWEOperationBlock sProgressBlock = null,
                                                                    bool sPriority = false, NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("AddWebRequestSignInWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Account Sign-in with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = NWDOperationWebAccountAction.signin;
            sOperation.PasswordToken = sSign;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignUpWithBlock(
                                                                    NWDAccountSignType sSignType,
                                                                    string sSignHash,
                                                                    string sRescueHash,
                                                                    string sLoginHash,
                                                                    NWEOperationBlock sSuccessBlock = null,
                                                                    NWEOperationBlock sErrorBlock = null,
                                                                    NWEOperationBlock sCancelBlock = null,
                                                                    NWEOperationBlock sProgressBlock = null,
                                                                    bool sPriority = false, NWDAppEnvironment sEnvironment = null
                                                                    )
        {
            //Debug.Log ("AddWebRequestSignUpWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Account Sign-up with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = NWDOperationWebAccountAction.signup;
            sOperation.SignType = sSignType;
            sOperation.SignHash = sSignHash;
            sOperation.RescueHash = sRescueHash;
            sOperation.LoginHash = sLoginHash;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestSignOutWithBlock(
                                                                     NWEOperationBlock sSuccessBlock = null,
                                                                     NWEOperationBlock sErrorBlock = null,
                                                                     NWEOperationBlock sCancelBlock = null,
                                                                     NWEOperationBlock sProgressBlock = null,
                                                                     bool sPriority = false, NWDAppEnvironment sEnvironment = null
                                                                     )
        {
            //Debug.Log ("AddWebRequestSignOutWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Account Sign-out with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = NWDOperationWebAccountAction.signout;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccount AddWebRequestRescueWithBlock(
                                                                    string sEmail,
                                                                    NWEOperationBlock sSuccessBlock = null,
                                                                    NWEOperationBlock sErrorBlock = null,
                                                                    NWEOperationBlock sCancelBlock = null,
                                                                    NWEOperationBlock sProgressBlock = null,
                                                                    bool sPriority = false, NWDAppEnvironment sEnvironment = null
                                                                    )
        {
            //Debug.Log ("AddWebRequestRescueWithBlock");
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Account Modifiy with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = NWDOperationWebAccountAction.rescue;
            sOperation.RescueEmail = sEmail;
            sOperation.RescueLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            SharedInstance().WebOperationQueue.AddOperation(sOperation, sPriority);
            return sOperation;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebRequestFlush(NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("WebRequestFlush");
            if (sEnvironment == null)
            {
                sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            }
            WebOperationQueue.Flush(sEnvironment.Environment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebRequestInfos(NWDAppEnvironment sEnvironment = null)
        {
            //Debug.Log ("WebRequestInfos");
            if (sEnvironment == null)
            {
                sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            }
            WebOperationQueue.Infos(sEnvironment.Environment);
        }

        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebBlank AddWebRequestBlankWithBlock(NWEOperationBlock sSuccessBlock = null,
                                                                NWEOperationBlock sErrorBlock = null,
                                                                NWEOperationBlock sCancelBlock = null,
                                                                NWEOperationBlock sProgressBlock = null,
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
        public NWDOperationWebNoPage AddWebRequestNoPageWithBlock(NWEOperationBlock sSuccessBlock = null,
                                                                NWEOperationBlock sErrorBlock = null,
                                                                NWEOperationBlock sCancelBlock = null,
                                                                NWEOperationBlock sProgressBlock = null,
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
        public NWDOperationWebMaintenance AddWebRequestMaintenanceWithBlock(NWEOperationBlock sSuccessBlock = null,
                                                                NWEOperationBlock sErrorBlock = null,
                                                                NWEOperationBlock sCancelBlock = null,
                                                                NWEOperationBlock sProgressBlock = null,
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
        public void ChangeAllDatasForUserToAnotherUser(NWDAppEnvironment sEnvironment, string sOldAccountReference, string sNewAccountReference /*, string sAnonymousResetPassword*/)
        {
            Debug.Log("##### ChangeAllDatasForUserToAnotherUser " + sOldAccountReference + " to " + sNewAccountReference);
            NWDDataManager.SharedInstance().DataQueueExecute();
            //NWDAccountInfos.ChangeCurrentData(sOldAccountReference, sNewAccountReference);
            foreach (Type tType in ClassTypeList)
            {
                NWDBasisHelper.FindTypeInfos(tType).TryToChangeUserForAllObjects(sOldAccountReference, sNewAccountReference);
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
            sEnvironment.SetAccountReference(sNewAccountReference);
            SavePreferences(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationPullClassesDatas(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, List<Type> sTypeList, NWDOperationSpecial sSpecial)
        {
            NWDBenchmark.Start();
            //Debug.Log("SynchronizationPullClassesDatas()");
            // I must autoanalyze the Type of data?
            if (sTypeList == null)
            {
                List<Type> tTypeList = SharedInstance().ClassTypeList;
                sTypeList = tTypeList;
            }

            bool tUpdateData = false;
            if (sTypeList != null)
            {
                //List<string> tClassNameSync = new List<string>();
                //List<string> tClassNameNotSync = new List<string>();
                //List<string> tClassNameNotFound = new List<string>();

                foreach (Type tType in sTypeList)
                {
                    bool tResult = NWDBasisHelper.FindTypeInfos(tType).SynchronizationPullData(sInfos, sEnvironment, sData, sSpecial);
                    if (tResult == true)
                    {
                        tUpdateData = true;
                        //tClassNameSync.Add(tType.Name);
                    }
                    //else
                    //{
                    //    tClassNameNotSync.Add(tType.Name);
                    //}
                }
            }
            SharedInstance().DataQueueExecute();
            if (tUpdateData)
            {
                //SharedInstance().DataQueueExecute();
#if NWD_CRUD_NOTIFICATION
                NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_DATAS_WEB_UPDATE, null));
#endif
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> SynchronizationPushClassesDatas(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList,
                                                                   Dictionary<Type, List<string>> sTypeAndReferences = null,
                                                                   NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
        {
            sInfos.ClassPushCounter = 0;
            sInfos.ClassPullCounter = 0;
            sInfos.RowPullCounter = 0;
            sInfos.RowPushCounter = 0;
            Dictionary<string, object> rSend = new Dictionary<string, object>(new StringIndexKeyComparer());
            if (sTypeList != null)
            {
                foreach (Type tType in sTypeList)
                {
                    Dictionary<string, object> rSendPartial = NWDBasisHelper.FindTypeInfos(tType).SynchronizationPushData(sInfos, sEnvironment, sTypeAndReferences, sForceAll, sSpecial);
                    foreach (string tKey in rSendPartial.Keys)
                    {
                        rSend.Add(tKey, rSendPartial[tKey]);
                    }
                }
            }
            return rSend;
        }

        //-------------------------------------------------------------------------------------------------------------
        //public Dictionary<string, object> SynchronizationGetClassesDatas(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, List<Type> sTypeList)
        //{
        //    sInfos.ClassPushCounter = 0;
        //    sInfos.ClassPullCounter = 0;
        //    sInfos.RowPullCounter = 0;
        //    sInfos.RowPushCounter = 0;

        //    Dictionary<string, object> rSend = new Dictionary<string, object>();
        //    if (sTypeList != null)
        //    {
        //        foreach (Type tType in sTypeList)
        //        {
        //            Dictionary<string, object> rSendPartial = NWDBasisHelper.FindTypeInfos(tType).SynchronizationGetNewData(sInfos, sEnvironment, sForceAll);
        //            foreach (string tKey in rSendPartial.Keys)
        //            {
        //                rSend.Add(tKey, rSendPartial[tKey]);
        //            }
        //        }
        //    }
        //    return rSend;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void TokenError(NWDAppEnvironment sEnvironment)
        {
            DeleteUser(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteUser(NWDAppEnvironment sEnvironment)
        {
            foreach (Type tType in ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.DeleteUser(sEnvironment);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
