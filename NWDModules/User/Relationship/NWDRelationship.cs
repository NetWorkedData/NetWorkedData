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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDRelationshipPinState
    {
        None = 0, // non usable
        CreatePin = 1, // send at 1 for creation of of pincode
        Waiting = 2, // return at 2 for waiting user (180 seconds but show 120 seconds)
        FriendProposal = 3, // friend entered your code you can see his nickname
        Accepted = 4, // validate and protected to delete 
        Refused = 5, // validate and protected to delete  /// put in trash...


        TimeOut = 6, // put in trash...
        AllReadyFriends = 7, // put in trash...
        Error = 9, // put in trash...  TODO

        Banned = 98, // banned this user to my friends TODO
        HashInvalid = 99, // ALERT ! HACKER!  TODO
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        [NWDGroupStart("Publisher of datas")]
        [Indexed("RelationshipIndex", 0)]
        public NWDReferenceType<NWDAccount> PublisherReference
        {
            get; set;
        }
        public string PublisherNickname
        {
            get; set;
        }
        public string PublisherClassesShared
        {
            get; set;
        }
        [NWDGroupEnd()]

        [NWDSeparator]

        [NWDGroupStart("Relation")]
        public NWDReferenceType<NWDRelationship> Reciprocity { get; set; }
        [Indexed("PinIndex", 0)]
        [Indexed("RelationshipIndex", 1)]
        public NWDRelationshipPinState RelationState { get; set; }
        [NWDGroupEnd()]

        [NWDSeparator]

        [NWDGroupStart("Reader of datas")]
        //[Indexed("RelationshipIndex", 1)]
        public NWDReferenceType<NWDAccount> ReaderReference
        {
            get; set;
        }
        public string ReaderNickname
        {
            get; set;
        }
        public string ReaderClassesAccepted { get; set; }
        [NWDGroupEnd()]

        [NWDSeparator]

        [NWDGroupStart("Sync datas")]
        public bool FirstSync { get; set; }
        [NWDGroupEnd()]

        [NWDSeparator]

        [NWDGroupStart("PinCode informations")]
        [Indexed("PinIndex", 1)]
        public string PinCode { get; set; }
        [Indexed("PinIndex", 2)]
        public int PinLimit { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public delegate void SyncRemoveBlock(bool error, NWDOperationResult result = null);
        public SyncRemoveBlock SyncRemoveBlockDelegate;
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
        static NWDRelationship()
        {
            NWDMessage.CreateGenericMessage("TEST DOMAIN", "CODE", "TITLE", "DESCRIPTION");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDRelationship CreateNewRelationshipDefault(Type[] sClasses)
        {
            List<Type> tList = new List<Type>(sClasses);
            if (tList.Contains(typeof(NWDUserInfos)) == false)
            {
                tList.Add(typeof(NWDUserInfos));
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
            tReturn.ReaderReference.SetObject(null);
            tReturn.PublisherClassesShared = string.Join(",", tList.ToArray());
            tReturn.ReaderClassesAccepted = string.Join(",", tList.ToArray());
            tReturn.FirstSync = true;
            tReturn.RelationState = NWDRelationshipPinState.None;
            tReturn.InsertMe();

#if UNITY_EDITOR
            NWDDataManager.SharedInstance.RepaintWindowsInManager(typeof(NWDRelationship));
#endif
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void EnterPinToServer(string sNickname, string sPinCode,
                                            BTBOperationBlock sSuccessBlock = null,
                                            BTBOperationBlock sErrorBlock = null,
                                            BTBOperationBlock sCancelBlock = null,
                                            BTBOperationBlock sProgressBlock = null,
                                            bool sPriority = true,
                                            NWDAppEnvironment sEnvironment = null)
        {
            string tAccount = NWDAccount.GetCurrentAccountReference();

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship EnterPinCode", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "EnterPinCode";
            sOperation.Nickname = sNickname;
            sOperation.PinCode = sPinCode;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SynchronizeSlaveDatas(BTBOperationBlock sSuccessBlock = null,
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
        static public void SynchronizeForceSlaveDatas(BTBOperationBlock sSuccessBlock = null,
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
        static public List<NWDRelationship> GetMasters()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.PublisherReference.GetReference() == NWDAccount.GetCurrentAccountReference())
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
                if (tObject.ReaderReference.GetReference() == NWDAccount.GetCurrentAccountReference())
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        //static public bool AcceptedByMaster(string sReference, float sTimer, float sDateTimeMarge = 10.0F)
        //{
        //    bool rReturn = false;
        //    // TODO connect to server sync
        //    if (NWDRelationship.GetObjectByReference(sReference) != null)
        //    {
        //        NWDRelationship tObject = NWDRelationship.GetObjectByReference(sReference);
        //        if (tObject.RelationState == NWDRelationshipPinState.Accepted)
        //        {
        //            rReturn = true;
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// remove a  Relationship and synchronize to server
        /// </summary>
        public void RemoveRelationship()
        {
            // Trash relationship
            TrashMe();

            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDRelationship)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (SyncRemoveBlockDelegate != null)
                {
                    SyncRemoveBlockDelegate(false);
                }
            };

            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;
                if (SyncRemoveBlockDelegate != null)
                {
                    SyncRemoveBlockDelegate(true, tInfos);
                }
            };
            NWDDataManager.SharedInstance.AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        #region override of NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
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

                BTBConsole.Clear();

                List<Type> tListClasses = new List<Type>();
                tListClasses.Add(typeof(NWDUserInfos));
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
                this.PublisherReference.SetReference(NWDAccount.GetCurrentAccountReference());
                // this.MasterReference.SetReference(NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference);
                this.ReaderReference.SetObject(null);
                this.PublisherClassesShared = string.Join(",", tList.ToArray());
                this.ReaderClassesAccepted = string.Join(",", tList.ToArray());
                this.PublisherNickname = "";
                this.ReaderNickname = "";
                this.Reciprocity.SetReference(null);
                this.PinCode = "";
                this.PinLimit = 0;
                this.FirstSync = true;
                this.RelationState = NWDRelationshipPinState.None;
                this.UpdateMe();
                this.SaveModifications();

                this.AskWaitingFromServer();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;


            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.None);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Ask pincode", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                DateTime tDateTime = DateTime.Now;
                tDateTime.AddMinutes(1.5F);
                this.AskPinCodeFromServer("Dev user test");
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
                BTBConsole.Clear();
                EnterPinToServer("DevUserForTest", PinCode);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.FriendProposal);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Accept Friends", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                this.AcceptRelation(false);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Accept Friends bilateral", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                this.AcceptRelation(true);
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Refuse Friends", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                this.RefuseRelation();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();


            //EditorGUI.BeginDisabledGroup(RelationState != NWDRelationshipPinState.Accepted);
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Sync", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                SynchronizeSlaveDatas();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tYadd, tWidthTiers, tMiniButtonStyle.fixedHeight), "Sync Force", tMiniButtonStyle))
            {

                BTBConsole.Clear();
                SynchronizeForceSlaveDatas();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            //EditorGUI.EndDisabledGroup();

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
        //public NWDRelationship CreateBilateralRelationship()
        //{
        //    NWDRelationship tReturn = NewObject();
        //    tReturn.InsertMe();
        //    tReturn.MasterReference.SetReference( this.SlaveReference.GetReference());
        //    tReturn.SlaveReference.SetReference(this.MasterReference.GetReference());
        //    tReturn.ClassesAcceptedBySlave = this.ClassesSharedByMaster;
        //    tReturn.ClassesSharedByMaster = this.ClassesAcceptedBySlave;
        //    tReturn.FirstSync = this.FirstSync;
        //    tReturn.RelationState = this.RelationState;
        //    tReturn.PinCode = "reciproque";
        //    tReturn.UpdateMe();
        //    return tReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public K[] ObjectsFromPublisher<K>() where K : NWDBasis<K>, new()
        {
            return NWDBasis<K>.GetAllObjects(PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddClassesToPublisher(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    PublisherClassesShared = "," + PublisherClassesShared + ",";
                    PublisherClassesShared = PublisherClassesShared.Replace("," + tClassName + ",", ",");
                    PublisherClassesShared = PublisherClassesShared.Trim(new char[] { ',' });
                    // add
                    PublisherClassesShared = PublisherClassesShared + "," + tClassName;
                    PublisherClassesShared = PublisherClassesShared.Trim(new char[] { ',' });
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveClassesToPublisher(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    PublisherClassesShared = "," + PublisherClassesShared + ",";
                    PublisherClassesShared = PublisherClassesShared.Replace("," + tClassName + ",", ",");
                    PublisherClassesShared = PublisherClassesShared.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddClassesToReader(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ReaderClassesAccepted = "," + ReaderClassesAccepted + ",";
                    ReaderClassesAccepted = ReaderClassesAccepted.Replace("," + tClassName + ",", ",");
                    ReaderClassesAccepted = ReaderClassesAccepted.Trim(new char[] { ',' });
                    // add
                    ReaderClassesAccepted = ReaderClassesAccepted + "," + tClassName;
                    ReaderClassesAccepted = ReaderClassesAccepted.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveClassesToReader(Type sClass)
        {
            if (sClass.IsSubclassOf(typeof(NWDTypeClass)))
            {
                var tMethodInfo = sClass.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    // remove if exists
                    ReaderClassesAccepted = "," + ReaderClassesAccepted + ",";
                    ReaderClassesAccepted = ReaderClassesAccepted.Replace("," + tClassName + ",", ",");
                    ReaderClassesAccepted = ReaderClassesAccepted.Trim(new char[] { ',' });
                    // save modifications
                    SaveModificationsIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #region WebServices
        //-------------------------------------------------------------------------------------------------------------
        public void AskPinCodeFromServer(string sNickname = "no nickname", int sSeconds = 60,
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
            sOperation.Nickname = sNickname;
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
        public void AcceptRelation(bool sBilateral,
            BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            //RelationState = NWDRelationshipPinState.Accepted;
            //SaveModificationsIfModified();

            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship AcceptRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "AcceptFriend";
            sOperation.Relationship = this;
            sOperation.Bilateral = sBilateral;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EnterNicknameIDToServer(string sNickname, string sNicknameID,

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
            sOperation.Action = "EnterNicknameID";
            sOperation.Nickname = sNickname;
            sOperation.NicknameID = sNicknameID;
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
            //RelationState = NWDRelationshipPinState.Refused;
            //TrashMe();
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship AcceptRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "RefuseFriend";
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BannedRelation(
                                                                       BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            //RelationState = NWDRelationshipPinState.Refused;
            //TrashMe();
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship BannedRelation", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "BannedFriend";
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeClassByPublisher(BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            // TODO
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship ChangeClassByPublisher", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "ChangeClassByPublisher";
            sOperation.Classes = PublisherClassesShared;
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeClassByReader(BTBOperationBlock sSuccessBlock = null,
                                                                       BTBOperationBlock sErrorBlock = null,
                                                                       BTBOperationBlock sCancelBlock = null,
                                                                       BTBOperationBlock sProgressBlock = null,
                                                                       bool sPriority = true,
                                                                         NWDAppEnvironment sEnvironment = null)
        {
            // TODO
            NWDOperationWebRelationship sOperation = NWDOperationWebRelationship.Create("Relationship ChangeClassByReader", sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sEnvironment);
            sOperation.Action = "ChangeClassByReader";
            sOperation.Classes = ReaderClassesAccepted;
            sOperation.Relationship = this;
            NWDDataManager.SharedInstance.WebOperationQueue.AddOperation(sOperation, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public bool YouArePublisher()
        {
            bool rReturn = false;
            if (this.PublisherReference.GetReference() == NWDAccount.GetCurrentAccountReference())
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool YouAreReader()
        {
            bool rReturn = false;
            if (this.ReaderReference.GetReference() == NWDAccount.GetCurrentAccountReference())
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> ReaderReciprocity()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.ReaderReference.GetReference() == this.PublisherReference.GetReference()
                    && tObject.PublisherReference.GetReference() == this.ReaderReference.GetReference()
                    && tObject != this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDRelationship> PublisherReciprocity()
        {
            List<NWDRelationship> rList = new List<NWDRelationship>();
            foreach (NWDRelationship tObject in GetAllObjects())
            {
                if (tObject.PublisherReference.GetReference() == this.ReaderReference.GetReference()
                    && tObject.ReaderReference.GetReference() == this.PublisherReference.GetReference()
                    && tObject != this)
                {
                    rList.Add(tObject);
                }
            }
            return rList;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public List<NWDRelationship> SlaveReciprocityByState(NWDRelationshipPinState sNWDRelationState =NWDRelationshipPinState.Accepted)
        //{
        //    List<NWDRelationship> rList = new List<NWDRelationship>();
        //    foreach (NWDRelationship tObject in GetAllObjects())
        //    {
        //        if (tObject.RelationState == sNWDRelationState &&
        //            tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
        //            && tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
        //            && tObject != this)
        //        {
        //            rList.Add(tObject);
        //        }
        //    }
        //    return rList;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public List<NWDRelationship> MasterReciprocityByState(NWDRelationshipPinState sNWDRelationState = NWDRelationshipPinState.Accepted)
        //{
        //    List<NWDRelationship> rList = new List<NWDRelationship>();
        //    foreach (NWDRelationship tObject in GetAllObjects())
        //    {
        //        if (tObject.RelationState == sNWDRelationState &&
        //            tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
        //            && tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
        //            && tObject != this)
        //        {
        //            rList.Add(tObject);
        //        }
        //    }
        //    return rList;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool AllReadyFriendOrBanned()
        //{
        //    bool rReturn = false;
        //    foreach (NWDRelationship tObject in GetAllObjects())
        //    {
        //        // TODO : to prevent dupplicate 
        //        //if (tObject.MasterReference.GetReference() == this.SlaveReference.GetReference()
        //        //    && tObject.SlaveReference.GetReference() == this.MasterReference.GetReference()
        //        //    && tObject != this)
        //        //{
        //        //    rList.Add(tObject);
        //        //}
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetAllObjectsForRelationship(NWDRelationship sRelationship)
        {
            return GetAllObjects(sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetObjectByReferenceForRelationship(string sReference, NWDRelationship sRelationship)
        {
            return GetObjectByReference(sReference, sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetObjectsByReferencesForRelationship(string[] sReferences, NWDRelationship sRelationship)
        {
            return GetObjectsByReferences(sReferences, sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetObjectByInternalKeyForRelationship(string sInternalKey, NWDRelationship sRelationship)
        {
            return GetObjectByInternalKey(sInternalKey, sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetAllObjectsByInternalKeyForRelationship(string sInternalKey, NWDRelationship sRelationship)
        {
            return GetAllObjectsByInternalKey(sInternalKey, sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetObjectsByInternalKeysForRelationship(string[] sInternalKeys, NWDRelationship sRelationship)
        {
            return GetObjectsByInternalKeys(sInternalKeys, sRelationship.PublisherReference.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
