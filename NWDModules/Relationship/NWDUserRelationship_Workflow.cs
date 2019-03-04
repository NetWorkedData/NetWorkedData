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
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserRelationship : NWDBasis<NWDUserRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void askCodePinBlock(bool error, NWDOperationResult result, string pinCode = BTBConstants.K_EMPTY_STRING);
        public askCodePinBlock askCodePinBlockDelegate;
        public delegate void relationshipBlock(bool error, NWDOperationResult result);
        public relationshipBlock relationshipBlockDelegate;
        public delegate void waitingResponseBlock(bool error, NWDOperationResult result, NWDRelationshipStatus stat = NWDRelationshipStatus.None);
        public waitingResponseBlock waitingResponseBlockDelegate;
        public delegate void synchronizeBlock(bool error, NWDOperationResult result);
        public static synchronizeBlock synchronizeBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserRelationship()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserRelationship(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            // Todo : list the all classe shared in App 
            List<Type> rClasses = new List<Type> { typeof(NWDUserRelationship), typeof(NWDRelationshipPlace) };
            foreach (NWDRelationshipPlace tPlace in NWDRelationshipPlace.FindDatas())
            {
                rClasses.AddRange(tPlace.ClassesSharedToStartRelation.GetClassesTypeList());
                rClasses.AddRange(tPlace.ClassesShared.GetClassesTypeList());
            }
            return rClasses;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserRelationship CreateNewRelationshipWith(NWDRelationshipPlace sPlace)
        {
            // Create a new Proposal
            NWDUserRelationship rRelationship = NewData();
            #if UNITY_EDITOR
            rRelationship.InternalKey = NWDAccountNickname.GetNickname() + " - " + sPlace.InternalKey;
            #endif
            rRelationship.Tag = NWDBasisTag.TagUserCreated;
            rRelationship.RelationPlace.SetObject(sPlace);
            rRelationship.SaveData();
            
            return rRelationship;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AskPinCodeFromServer()
        {
            RelationshipStatus = NWDRelationshipStatus.GenerateCode;
            SaveData();
            
            //Ask server to generate a new Code Pin
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (askCodePinBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    askCodePinBlockDelegate(false, tResult, RelationshipHash);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if(askCodePinBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    askCodePinBlockDelegate(true, tResult);
                }
            };

            string tUserNickName = BTBConstants.K_MINUS;
            foreach (NWDAccountNickname user in NWDAccountNickname.FindDatas())
            {
                tUserNickName = user.Nickname;
                if (user.IsSynchronized())
                {
                    break;
                }
            }

            // Sync NWDUserRelationship
            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EnterCodePin(string sPinCode)
        {
            RelationshipStatus = NWDRelationshipStatus.InsertCode;
            RelationshipCode = sPinCode;
            SaveData();

            string tUserNickName = BTBConstants.K_MINUS;
            foreach (NWDAccountNickname user in NWDAccountNickname.FindDatas())
            {
                tUserNickName = user.Nickname;
                if (user.IsSynchronized())
                {
                    break;
                }
            }
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WaitingResponse()
        {
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (waitingResponseBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    waitingResponseBlockDelegate(false, tResult, RelationshipStatus);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (waitingResponseBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    waitingResponseBlockDelegate(true, tResult);
                }
            };
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AcceptRelationship()
        {
            RelationshipStatus = NWDRelationshipStatus.AcceptFriend;
            SaveData();
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveRelationship()
        {
            RelationshipStatus = NWDRelationshipStatus.Delete;
            SaveData();
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RefuseRelationship()
        {
            RelationshipStatus = NWDRelationshipStatus.RefuseFriend;
            SaveData();
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RefreshAndSynchronizeDatas()
        {
            RefreshDatas();
            SynchronizeDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RefreshDatas()
        {
            foreach (NWDUserRelationship k in FindDatas())
            {
                if (k.RelationshipStatus == NWDRelationshipStatus.Valid)
                {
                    k.RelationshipStatus = NWDRelationshipStatus.Sync;
                    k.SaveData();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(true, tResult);
                }
            };
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        void RelationshipFailedBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (relationshipBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                relationshipBlockDelegate(true, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RelationshipSuccessBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (relationshipBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                relationshipBlockDelegate(false, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================