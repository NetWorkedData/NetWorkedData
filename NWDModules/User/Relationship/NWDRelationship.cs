//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDRelationshipPinState : byte
    {
        None = 0, // non usable
        CreatePin = 1, // send at 1 for creation of of pincode
        Waiting = 2, // return at 2 for waiting user (180 seconds but show 120 seconds)
        FriendProposal = 3, // friend entered your code you can see his nickname
        Accepted = 4, // validate and protected to delete 
        Refused = 5, // validate and protected to delete 

        TimeOut = 6,
        Error = 9,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDFriendConnexion : NWDConnexion<NWDRelationship>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("FRD")]
    [NWDClassDescriptionAttribute("User Friend descriptions Class")]
    [NWDClassMenuNameAttribute("User Friend")]
    //-------------------------------------------------------------------------------------------------------------
    //	[NWDTypeClassInPackageAttribute]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDRelationship : NWDBasis<NWDRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        //#warning YOU MUST FOLLOW THIS INSTRUCTIONS
        //-------------------------------------------------------------------------------------------------------------
        // YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
        // YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
        // YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        /// <summary>
        /// Get or set the account reference.
        /// </summary>
        /// <value>The account reference.</value>
        [Indexed("RelationshipIndex", 0)]
        public NWDReferenceType<NWDAccount> MasterReference
        {
            get; set;
        } // user A as Master
        [Indexed("RelationshipIndex", 1)]
        public NWDReferenceType<NWDAccount> SlaveReference
        {
            get; set;
        } // user B as Slave
        //public string SlaveUniqueNickname { get; set;  } // user B as Slave ID (unique Nickname shorter than reference)
        public string ClassesSharedByMaster
        {
            get; set;
        }
        public string ClassesAcceptedBySlave
        {
            get; set;
        }
        public bool FirstSync
        {
            get; set;
        }

        [Indexed("PinIndex", 3)]
        public NWDRelationshipPinState RelationState
        {
            get; set;
        }


        [Indexed("PinIndex", 0)]
        public string PinCode
        {
            get; set;
        }
        [Indexed("PinIndex", 2)]
        public DateTime PinLimit
        {
            get; set;
        }
        [Indexed("PinIndex", 4)]
        public DateTime PinValidate
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDRelationship()
        {
            //Init your instance here
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #region override of NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public static NWDRelationship CreateNewRelationshipDefault(string[] sClassNames)
        {
            List<string> tList = new List<string>(sClassNames);
            if (tList.Contains(NWDPlayerInfos.ClassName()) == false)
            {
                tList.Add(NWDPlayerInfos.ClassName());
            }
            NWDRelationship tRelation = CreateNewRelationship(tList.ToArray());
            return tRelation;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static NWDRelationship CreateNewRelationship(string[] sClassNames)
        {
            NWDRelationship tReturn = NewObject();
            tReturn.MasterReference.SetObject(NWDAccount.GetCurrentAccount());
            tReturn.ClassesSharedByMaster = string.Join(",", sClassNames);
            tReturn.ClassesAcceptedBySlave = string.Join(",", sClassNames);
            tReturn.FirstSync = true;
            tReturn.RelationState = NWDRelationshipPinState.None;
            tReturn.SaveModificationsIfModified();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDBasis[] ObjectsFromSlave(Type sClass  )
        //{
            
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void AskPinCodeFromServer(DateTime sDateTimeMax,

                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            RelationState = NWDRelationshipPinState.CreatePin;
            PinLimit = sDateTimeMax;
            SaveModificationsIfModified();
            // Start webrequest
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "CreatePinCode";
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AskWaitingFromServer(float sTimer, float sDateTimeMarge = 10.0F, // sTimer repeat every x seconds ..... sDateTimeMarge is marge about sDateTimeMax to cancel
                                         BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                         NWDAppEnvironment sEnvironment = null)
        {
            bool rReturn = false;
            //NWDDataManager.SharedInstance.AddWebRequestAllSynchronization();
            if (RelationState == NWDRelationshipPinState.Waiting)
            {
                rReturn = false;
            }
            else
            {
                rReturn = true;
                // Next Step in development
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string EnterPinToServer(string sPinCode,

                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            string tAccount = NWDAccount.GetCurrentAccountReference();
            string tReference = "???";
            // TODO connect to server
            return tReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AcceptRelation(bool sBilateral,
            BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            RelationState = NWDRelationshipPinState.Accepted;
            SaveModificationsIfModified();

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship AcceptRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "AcceptFriend";
            sOperation.Relationship = this;
            sOperation.Bilateral = sBilateral;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RefuseRelation(
                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            RelationState = NWDRelationshipPinState.Accepted;
            SaveModificationsIfModified();
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship AcceptRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "RefuseFriend";
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);

        }

        //-------------------------------------------------------------------------------------------------------------
        static public void Sync(
                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship Sync", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "Sync";
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);

        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool AcceptedByMaster(string sReference, float sTimer, float sDateTimeMarge = 10.0F)
        {
            bool rReturn = false;
            // TODO connect to server sync
            if (NWDRelationship.GetObjectByReference(sReference) != null)
            {
                NWDRelationship tObject = NWDRelationship.GetObjectByReference(sReference);
                if (tObject.RelationState == NWDRelationshipPinState.Accepted)
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------

    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
