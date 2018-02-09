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
    public enum NWDRelationshipPinState
    {
        None = 0, // non usable
        CreatePin = 1, // send at 1 for creation of of pincode
        Waiting = 2, // return at 2 for waiting user (180 seconds but show 120 seconds)
        FriendProposal = 3, // friend entered your code you can see his nickname
        Accepted = 4, // validate and protected to delete 
        Refused = 5, // validate and protected to delete  /// put in trash...


        TimeOut = 6, /// put in trash...
        AllReadyFriends = 7, /// put in trash...
        Error = 9, /// put in trash...

        Banned = 99, // banned this user to my friends  
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDFriendConnexion : NWDConnexion<NWDRelationship>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("RLS")]
    [NWDClassDescriptionAttribute("Relationship  descriptions Class")]
    [NWDClassMenuNameAttribute("Relationship")]
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
        //[Indexed("RelationshipIndex", 1)]
        public NWDReferenceType<NWDAccount> SlaveReference
        {
            get; set;
        }
        public string SlaveNickname
        {
            get; set;
        } 
        public string MasterNickname
        {
            get; set;
        } 
        public NWDReferenceType<NWDRelationship> Reciprocity
        {
            get; set;
        } 
        // user B as Slave
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
        [Indexed("PinIndex", 0)]
        [Indexed("RelationshipIndex", 1)]
        public NWDRelationshipPinState RelationState
        {
            get; set;
        }
        [Indexed("PinIndex", 1)]
        public string PinCode
        {
            get; set;
        }
        [Indexed("PinIndex", 2)]
        public int PinLimit
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
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tYadd = sInRect.y;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            EditorGUI.DrawRect(new Rect(tX, tYadd + NWDConstants.kFieldMarge, tWidth, 1), kRowColorLine);
            tYadd += NWDConstants.kFieldMarge * 2;

            EditorGUI.LabelField(new Rect(tX, tYadd, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            tYadd += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            float tWidthTiers = (tWidth - NWDConstants.kFieldMarge * 1) / 2.0f;

//            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Add object", tMiniButtonStyle))
//            {
//                BTBConsole.Clean();
//                new NWDRelationship();
//#if UNITY_EDITOR
//                NWDDataManager.SharedInstance.RepaintWindowsInManager(typeof(NWDRelationship));
//#endif
            //}
            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "reset", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                    
             List<Type> tListClasses = new List<Type>();
                tListClasses.Add(typeof(NWDPlayerInfos));
                tListClasses.Add(typeof(NWDOwnership));
                List<string> tList = new List<string>();
                foreach (Type tClass in tListClasses.ToArray())
                {
                    if (tClass.IsSubclassOf(typeof(NWDTypeClass)))
                    {
                        var tMethodInfo = tClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            string tClassName = tMethodInfo.Invoke(null, null) as string;
                            tList.Add(tClassName);
                        }
                    }
                }
                this.MasterReference.SetReference(NWDAccount.GetCurrentAccountReference());
               // this.MasterReference.SetReference(NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference);
                this.SlaveReference.SetObject(null);
                this.ClassesSharedByMaster = string.Join(",", tList.ToArray());
                this.ClassesAcceptedBySlave = string.Join(",", tList.ToArray());
                this.PinCode = "------";
                this.PinLimit = 0;
                this.FirstSync = true;
                this.RelationState = NWDRelationshipPinState.None;
                this.UpdateMe();

                this.AskWaitingFromServer();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;


            EditorGUI.BeginDisabledGroup(RelationState!= NWDRelationshipPinState.None);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Ask pincode", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                DateTime tDateTime = DateTime.Now;
                tDateTime.AddMinutes(1.5F);
                this.AskPinCodeFromServer();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();


            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.Waiting);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "refresh", tMiniButtonStyle))
            {
                this.AskWaitingFromServer();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();


            
            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.Waiting);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "auto send the pincode to me ", tMiniButtonStyle))
            {
                BTBConsole.Clean();
                EnterPinToServer(PinCode);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();




            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.FriendProposal);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Accept Friends", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                this.AcceptRelation(false);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Accept Friends bilateral", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                this.AcceptRelation(true);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Refuse Friends", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                this.RefuseRelation();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();


            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.Accepted);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Sync", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                Sync();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Sync Force", tMiniButtonStyle))
            {

                BTBConsole.Clean();
                SyncForce();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), 100);

            tYadd = NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge * 2;
            tYadd += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge;

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public NWDRelationship CreateBilateralRelationship()
        {
            NWDRelationship tReturn = NewObject();
            tReturn.InsertMe();
            tReturn.MasterReference.SetReference( this.SlaveReference.GetReference());
            tReturn.SlaveReference.SetReference(this.MasterReference.GetReference());
            tReturn.ClassesAcceptedBySlave = this.ClassesSharedByMaster;
            tReturn.ClassesSharedByMaster = this.ClassesAcceptedBySlave;
            tReturn.FirstSync = this.FirstSync;
            tReturn.RelationState = this.RelationState;
            tReturn.PinCode = "reciproque";
            tReturn.UpdateMe();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDRelationship CreateNewRelationshipDefault(Type[] sClasses)
        {
            List<Type> tList = new List<Type>(sClasses);
            if (tList.Contains(typeof(NWDPlayerInfos)) == false)
            {
                tList.Add(typeof(NWDPlayerInfos));
            }
            NWDRelationship tRelation = CreateNewRelationship(tList.ToArray());
            return tRelation;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static NWDRelationship CreateNewRelationship(Type[] sClasses)
        {

            List<string> tList = new List<string>();
            foreach (Type tClass in sClasses)
            {
                if (tClass.IsSubclassOf(typeof(NWDTypeClass)))
                {
                    var tMethodInfo = tClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        string tClassName = tMethodInfo.Invoke(null, null) as string;
                        tList.Add(tClassName);
                    }
                }
            }
            NWDRelationship tReturn = NewObject();
            //tReturn.MasterReference.SetObject(NWDAccount.GetCurrentAccount());
            tReturn.SlaveReference.SetObject(null);
            tReturn.ClassesSharedByMaster = string.Join(",", tList.ToArray());
            tReturn.ClassesAcceptedBySlave = string.Join(",", tList.ToArray());
            tReturn.FirstSync = true;
            tReturn.RelationState = NWDRelationshipPinState.None;
            tReturn.InsertMe();

#if UNITY_EDITOR
            NWDDataManager.SharedInstance.RepaintWindowsInManager(typeof(NWDRelationship));
#endif
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDBasis[] ObjectsFromSlave(Type sClass  )
        //{

        //}
        //-------------------------------------------------------------------------------------------------------------
        public void AddClassesToMaster(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ClassesSharedByMaster = "," + ClassesSharedByMaster + ",";
                    ClassesSharedByMaster = ClassesSharedByMaster.Replace("," + tClassName + ",", ",");
                    ClassesSharedByMaster = ClassesSharedByMaster.Trim(new char[] { ',' });
                    // add
                    ClassesSharedByMaster = ClassesSharedByMaster + "," + tClassName;
                    ClassesSharedByMaster = ClassesSharedByMaster.Trim(new char[] { ',' });
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveClassesToMaster(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ClassesSharedByMaster = "," +ClassesSharedByMaster +",";
                    ClassesSharedByMaster = ClassesSharedByMaster.Replace(","+tClassName+",", ",");
                    ClassesSharedByMaster = ClassesSharedByMaster.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddClassesToSlave(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ClassesAcceptedBySlave = "," + ClassesAcceptedBySlave + ",";
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave.Replace("," + tClassName + ",", ",");
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave.Trim(new char[] { ',' });
                    // add
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave + "," + tClassName;
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveClassesToSlave(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ClassesAcceptedBySlave = "," + ClassesAcceptedBySlave + ",";
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave.Replace("," + tClassName + ",", ",");
                    ClassesAcceptedBySlave = ClassesAcceptedBySlave.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AskPinCodeFromServer(int sSeconds = 60,
                                         int sPinSize = 6,

                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            RelationState = NWDRelationshipPinState.CreatePin;
             SaveModificationsIfModified();
            // Start webrequest
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship with Block", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "CreatePinCode";
            sOperation.PinSize = sPinSize;
            sOperation.PinDelay = sSeconds;
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AskWaitingFromServer(
            //float sTimer, float sDateTimeMarge = 10.0F, // sTimer repeat every x seconds ..... sDateTimeMarge is marge about sDateTimeMax to cancel
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

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship Waiting", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "Waiting";
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO register the reference of pincode relationship found  object
        //-------------------------------------------------------------------------------------------------------------
        static public void EnterPinToServer(string sPinCode,

                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                       NWDAppEnvironment sEnvironment = null)
        {
            string tAccount = NWDAccount.GetCurrentAccountReference();
            // TODO connect to server
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship EnterPinCode", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "EnterPinCode";
            sOperation.PinCode = sPinCode;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
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
            if (sBilateral == true)
            {
                // create a new Relationship to send to server
                CreateBilateralRelationship();
            }

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship AcceptRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "AcceptFriend";
            sOperation.Relationship = this;
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
            RelationState = NWDRelationshipPinState.Refused;
            TrashMe();
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
        static public void SyncForce(
                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship Sync", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "SyncForce";
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
        static public List<NWDRelationship> GetMasters()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.MasterReference.GetReference() == NWDAccount.GetCurrentAccountReference())
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDRelationship> GetSlaves()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.SlaveReference.GetReference() == NWDAccount.GetCurrentAccountReference())
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool YouAreMaster()
        {
            bool rReturn = false;
            if (this.MasterReference.GetReference() == NWDAccount.GetCurrentAccountReference())
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool YouAreSlave()
        {
            bool rReturn = false;
            if (this.SlaveReference.GetReference() == NWDAccount.GetCurrentAccountReference())
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> SlaveReciprocity()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.SlaveReference.GetReference() == this.MasterReference.GetReference() 
                    && tObject.MasterReference.GetReference() == this.SlaveReference.GetReference() 
                    && tObject!=this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> MasterReciprocity()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
                    && tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
                    && tObject != this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> SlaveReciprocityByState(NWDRelationshipPinState sNWDRelationState =NWDRelationshipPinState.Accepted)
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.RelationState == sNWDRelationState &&
                    tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
                    && tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
                    && tObject != this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> MasterReciprocityByState(NWDRelationshipPinState sNWDRelationState = NWDRelationshipPinState.Accepted)
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.RelationState == sNWDRelationState &&
                    tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
                    && tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
                    && tObject != this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AllReadyFriendOrBanned()
        {
            bool rReturn = false;
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                // TODO : to prevent dupplicate 
                //if (tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
                //    && tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
                //    && tObject != this)
                //{
                //    rList.Add(tObject);
                //}
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
