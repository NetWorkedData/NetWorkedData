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
            return new List<Type> {typeof(NWDUserRelationship), typeof(NWDRelationshipPlace)};
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserRelationship CreateNewRelationshipWith(NWDRelationshipPlace sPlace)
        {
            // Create a new Proposal
            NWDUserRelationship tRelationship = NewData();
#if UNITY_EDITOR
            tRelationship.InternalKey = NWDAccountNickname.GetNickname() + " - " + sPlace.InternalKey;
#endif
            tRelationship.Tag = NWDBasisTag.TagUserCreated;
            tRelationship.RelationPlace.SetObject(sPlace);
            tRelationship.SaveData();
            
            return tRelationship;
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
            
            /*BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(true, tResult);
                }
            };*/
            
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
            
            /*BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(true, tResult);
                }
            };*/
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RefuseRelationship()
        {
            RelationshipStatus = NWDRelationshipStatus.RefuseFriend;
            SaveData();
            
            /*BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (relationshipBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    relationshipBlockDelegate(true, tResult);
                }
            };*/
            
            // Sync NWDUserRelationship
            SynchronizationFromWebService(RelationshipSuccessBlock, RelationshipFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            NWDUserRelationship[] tOwnerList = FindDatas();
            foreach (NWDUserRelationship tOwner in tOwnerList)
            {
                if (tOwner.RelationshipStatus == NWDRelationshipStatus.Valid)
                {
                    tOwner.RelationshipStatus = NWDRelationshipStatus.Sync;
                    tOwner.SaveData();
                }
            }
            
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