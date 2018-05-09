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
    public class NWDQuestUserAdvancementConnection : NWDConnection<NWDQuestUserAdvancement>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("QUA")]
    [NWDClassDescriptionAttribute("Quest User Advancement descriptions Class")]
    [NWDClassMenuNameAttribute("Quest User Advancement")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDQuestUserAdvancement : NWDBasis<NWDQuestUserAdvancement>
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
        [NWDGroupStartAttribute("Advancement information", true, true, true)]
        [NWDTooltips("Account, reference, last dialogue see and state of the quest")]
        public NWDReferenceType<NWDAccount> AccountReference
        {
            get; set;
        }
        [NWDTooltips("Original quest")]
        public NWDReferenceType<NWDQuest> QuestReference
        {
            get; set;
        }
        [NWDTooltips("Original quests")]
        public NWDReferenceType<NWDQuest> QuestOriginalReference
        {
            get; set;
        }
        [NWDTooltips("Actual quest")]
        public NWDReferenceType<NWDQuest> QuestActualReference
        {
            get; set;
        }
        [NWDTooltips("the last dialog to use for resume quest")]
        public NWDReferenceType<NWDDialog> LastDialogReference
        {
            get; set;
        }
        [NWDTooltips("All dialog to use for the resume quest's book")]
        public NWDReferencesListType<NWDDialog> DialogResumeList
        {
            get; set;
        }
        [NWDTooltips("Actual quest State")]
        public NWDQuestState QuestState
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Counters", "Quest Counter", true, true, true)]
        public int AcceptCounter
        {
            get; set;
        }
        public int RefuseCounter
        {
            get; set;
        }
        public int CancelCounter
        {
            get; set;
        }
        public int SuccessCounter
        {
            get; set;
        }
        public int FailCounter
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Counter of gloab quest sequence", "", true, true, true)]
        public int FinishCounter
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string NOTIFICATION_USER_QUEST_CHANGE = "NWDQuestUserAdvancement_UserQuestChangeNotification";
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuestUserAdvancement()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuestUserAdvancement(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDQuestUserAdvancement GetAdvancementForQuest(NWDQuest sQuest)
        {
            NWDQuestUserAdvancement rAdvancement = null;
            foreach (NWDQuestUserAdvancement tAdvancement in GetAllObjects())
            {
                if (tAdvancement.QuestReference.GetObject() == sQuest)
                {
                    rAdvancement = tAdvancement;
                    break;
                }
            }
            if (sQuest != null)
            {
                if (rAdvancement == null)
                {
                    rAdvancement = NewObject();
                    //--------------
#if UNITY_EDITOR
                    //--------------
                    if (sQuest.Title != null)
                    {
                        string tQuestBaseName = sQuest.Title.GetBaseString();
                        if (tQuestBaseName != null)
                        {
                            rAdvancement.InternalKey = tQuestBaseName;
                        }
                    }
                    rAdvancement.InternalDescription = NWDUserNickname.GetNickName() + " - " + NWDUserNickname.GetUniqueNickName();
                    //--------------
#endif
                    //--------------
                    rAdvancement.Tag = 20; // Data generated by User-created
                    rAdvancement.QuestReference.SetReference(sQuest.Reference);
                    rAdvancement.QuestActualReference.SetReference(sQuest.Reference);
                    rAdvancement.SaveModifications();
                }

                // Check if the quest advancement is the good advancement
                // else return the advancement for the actual quest part
                if (rAdvancement.QuestReference.GetReference() != rAdvancement.QuestActualReference.GetReference())
                {
                    rAdvancement = GetAdvancementForQuest(rAdvancement.QuestActualReference.GetObject());
                }
            }
            return rAdvancement;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialog ActualDialogForQuest(NWDQuest sQuest)
        {
            //return sQuest.FirstDialogOnShowQuest(this);
            NWDQuestUserAdvancement tQuestUserAdvancement = GetAdvancementForQuest(sQuest);
            return tQuestUserAdvancement.QuestReference.GetObject().FirstDialogOnShowQuest(tQuestUserAdvancement);
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public bool AvailableQuest()
        {
            bool rReturn = true;
            NWDQuest tQuest = QuestActualReference.GetObject();
            if (tQuest == null)
            {
                rReturn = false;
            }
            else
            {
                switch (tQuest.Type)
                {
                    case NWDQuestType.Unique:
                        {
                            if (FinishCounter >= 1)
                            {
                                rReturn = false;
                            }
                        }
                        break;
                    case NWDQuestType.Multiple:
                        {
                            if (FinishCounter >= tQuest.Number)
                            {
                                rReturn = false;
                            }
                        }
                        break;
                    case NWDQuestType.Infiny:
                        {
                            rReturn = true;
                        }
                        break;
                }
                if (rReturn == true)
                {
                    rReturn = tQuest.AvailabilitySchedule.AvailableNowInGameTime();
                }


                bool tItemsRequired = NWDOwnership.ConditionalItems(tQuest.RequiredItems);
                bool tItemsGroupsRequired = NWDOwnership.ConditionalItemGroups(tQuest.RequiredItemGroups);

                bool tItemsWanted = NWDOwnership.ConditionalItems(tQuest.DesiredItems);
                bool tItemsGroupsWanted = NWDOwnership.ConditionalItemGroups(tQuest.DesiredItemGroups);

                if (tQuest.DesiredItems == null)
                {
                    tQuest.DesiredItems = new NWDReferencesConditionalType<NWDItem>();
                }
                if (tQuest.DesiredItemGroups == null)
                {
                    tQuest.DesiredItemGroups = new NWDReferencesConditionalType<NWDItemGroup>();
                }

                if (tQuest.DesiredItems.IsEmpty() && tQuest.DesiredItemGroups.IsEmpty())
                {
                    tItemsWanted = false;
                    tItemsGroupsWanted = false;
                }
                // I need propose the First Dialaog
                if (tItemsRequired && tItemsGroupsRequired)
                {
                    if (tItemsWanted && tItemsGroupsWanted)
                    {
                        if (tQuest.DesiredDialogsList != null)
                        {
                            NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tQuest.DesiredDialogsList.GetObjectsList());
                            if (tDialog == null)
                            {
                                rReturn = false;
                            }
                        }
                    }
                    else
                    {
                        NWDReferencesListType<NWDDialog> tAvailableDialogsList = tQuest.AvailableDialogsList; 
                        List<NWDDialog> tDialogsList = tAvailableDialogsList.GetObjectsList();
                        NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tDialogsList);
                        if (tDialog == null)
                        {
                            rReturn = false;
                        }
                    }
                }
                else
                {
                    NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tQuest.RequiredDialogsList.GetObjectsList());
                    if (tDialog == null)
                    {
                        rReturn = false;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AvailableQuest(NWDQuest sQuest)
        {
            return GetAdvancementForQuest(sQuest).AvailableQuest();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDDialog FirstDialogOnShowQuest()
        //{
        //    // I return the first dialog
        //    NWDDialog rDialog = LastDialogReference.GetObject();
        //    NWDQuest tQuest = QuestActualReference.GetObject();
        //    // If first dialog is not an step dialog I found the Dialog to use
        //    if (rDialog == null)
        //    {
        //        bool tItemsRequired = NWDOwnership.ConditionalItems(tQuest.ItemsRequired);
        //        bool tItemsGroupsRequired = NWDOwnership.ConditionalItemGroups(tQuest.ItemGroupsRequired);

        //        bool tItemsWanted = NWDOwnership.ConditionalItems(tQuest.ItemsWanted);
        //        bool tItemsGroupsWanted = NWDOwnership.ConditionalItemGroups(tQuest.ItemGroupsWanted);

        //        if (tQuest.ItemsWanted.IsEmpty() && tQuest.ItemGroupsWanted.IsEmpty())
        //        {
        //            tItemsWanted = false;
        //            tItemsGroupsWanted = false;
        //        }
        //        // I need propose the First Dialaog
        //        if (tItemsRequired && tItemsGroupsRequired)
        //        {
        //            if (tItemsWanted && tItemsGroupsWanted)
        //            {
        //                rDialog = tQuest.AlternateDialogReference.GetObject();
        //                QuestState = NWDQuestState.StartAlternate;
        //            }
        //            else
        //            {
        //                rDialog = tQuest.DialogReference.GetObject();
        //                QuestState = NWDQuestState.Start;
        //            }
        //        }
        //        else
        //        {
        //            rDialog = tQuest.NoRequiredDialogReference.GetObject();
        //            QuestState = NWDQuestState.None;
        //        }
        //    }
        //    else
        //    {
        //        if (rDialog.AnswerState == NWDDialogState.Sequent)
        //        {
        //           NWDDialog[] tDialogs = rDialog.GetNextDialogs();
        //            if (tDialogs.Length > 0)
        //            {
        //                rDialog = tDialogs[0];
        //            }
        //        }
        //    }

        //    // Save the modification on user quest advancement
        //    SaveModifications();

        //    return rDialog;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void AdvancementDialog(NWDQuest sQuest, NWDDialog sDialog)
        {
            GetAdvancementForQuest(sQuest).AdvancementDialog(sDialog);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AdvancementDialog(NWDDialog sDialog)
        {
            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ")");
            NWDQuest tQuest = QuestActualReference.GetObject();
            // If Dialog == null I need determine wich dialog I need to Use
            if (sDialog == null)
            {
                // I need propose the First Dialog ? WTF?
            }
            else
            {
                DialogResumeList.AddObject(sDialog);
                // Update Quest Advancement
                switch (sDialog.QuestStep)
                {
                    case NWDQuestState.Accept:
                        if (QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Accept");
                            // I must remove the required object or Not?
                            NWDOwnership.RemoveItemToOwnership(tQuest.RequiredItemsToRemove);
                            // put quest in accept
                            QuestState = NWDQuestState.Accept;
                            // increment counters
                            AcceptCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDQuestUserAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Refuse:
                        if (QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Refuse");
                            // put quest in cancel
                            QuestState = NWDQuestState.Refuse;
                            // increment counters
                            RefuseCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDQuestUserAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Cancel:
                        if (QuestState == NWDQuestState.Accept)
                        {
                            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Cancel");
                            // put quest in cancel
                            QuestState = NWDQuestState.Cancel;
                            // increment counters
                            CancelCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDQuestUserAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Success:
                        if (QuestState == NWDQuestState.Accept || QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Success");
                            bool tItemsWanted = NWDOwnership.ConditionalItems(tQuest.DesiredItems);
                            bool tItemsGroupsWanted = NWDOwnership.ConditionalItemGroups(tQuest.DesiredItemGroups);
                            if (tItemsWanted && tItemsGroupsWanted)
                            {
                                // put quest in success
                                QuestState = NWDQuestState.Success;
                                // I must remove the required object or Not?
                                NWDOwnership.RemoveItemToOwnership(tQuest.DesiredItemsToRemove);
                                // Add items
                                NWDOwnership.AddItemToOwnership(tQuest.RewardsItems);
                                // Add Items by itemPacks
                                foreach (KeyValuePair<NWDItemPack, int> tKeyValue in tQuest.RewardsItemPack.GetObjectAndQuantity())
                                {
                                    for (int tI = 0; tI < tKeyValue.Value; tI++)
                                    {
                                        NWDOwnership.AddItemToOwnership(tKeyValue.Key.Items);
                                    }
                                }
                                // Add items by Pack
                                foreach (KeyValuePair<NWDPack, int> tKeyValue in tQuest.RewardsPacks.GetObjectAndQuantity())
                                {
                                    for (int tI = 0; tI < tKeyValue.Value; tI++)
                                    {
                                        NWDOwnership.AddItemToOwnership(tKeyValue.Key.GetAllItemReferenceAndQuantity());
                                    }
                                }
                                // increment counters
                                SuccessCounter++;
                                NWDQuest tNextQuest = sDialog.NextQuest.GetObject();
                                if (tNextQuest != null)
                                {
                                    QuestActualReference.SetReference(tNextQuest.Reference);
                                    NWDQuestUserAdvancement tQuestUserAdvancement = NWDQuestUserAdvancement.GetAdvancementForQuest(tNextQuest);
                                    tQuestUserAdvancement.QuestOriginalReference.SetReference(QuestReference.GetReference());
                                    tQuestUserAdvancement.SaveModifications();

                                }
                                else
                                {
                                    FinishQuest();
                                }
                                // send BTBNotification
                                BTBNotificationManager.SharedInstance().PostNotification(this, NWDQuestUserAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                            }
                            else
                            {
                                // Error
                                Debug.LogWarning("Quest " + QuestActualReference.GetReference() + " must be changed to 'success' but items wanted not presents! Fix the quest dialog storyboard by limit dialog by items required!");
#if UNITY_EDITOR
                                EditorUtility.DisplayDialog("ERROR", "Quest " + QuestActualReference.GetReference() + " must be changed to 'success' but items wanted not presents! Fix the quest dialog storyboard by limit dialog by items required!", "OK");
#endif
                            }
                        }
                        break;
                    case NWDQuestState.Fail:
                        //if (QuestState == NWDQuestState.Accept)
                        {
                            Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Fail");
                            // put quest in fail
                            QuestState = NWDQuestState.Fail;
                            // increment counters
                            FailCounter++;
                            NWDQuest tNextQuest = sDialog.NextQuest.GetObject();
                            if (tNextQuest != null)
                            {
                                QuestActualReference.SetReference(tNextQuest.Reference);
                                NWDQuestUserAdvancement tQuestUserAdvancement = NWDQuestUserAdvancement.GetAdvancementForQuest(tNextQuest);
                                tQuestUserAdvancement.QuestOriginalReference.SetReference(QuestReference.GetReference());
                                tQuestUserAdvancement.SaveModifications();
                            }
                            else
                            {
                                FinishQuest();
                            }
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDQuestUserAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                }
                if (sDialog.AnswerState == NWDDialogState.Step || sDialog.AnswerState == NWDDialogState.Sequent)
                {
                    LastDialogReference.SetObject(sDialog);
                }
                if (sDialog.AnswerState == NWDDialogState.Reset)
                {
                    // I remove the last dialog... waiting to decalre a new dialog or restart the quest
                    QuestState = NWDQuestState.None;
                    LastDialogReference.SetObject(null);
                    QuestActualReference.SetReference(QuestReference.GetReference());
                }
                SaveModifications();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishQuest(bool sWithCounter = true)
        {
            NWDQuest tOriginalQuest = QuestOriginalReference.GetObject();
            NWDQuest tActualQuest = QuestActualReference.GetObject();
            if (tOriginalQuest != null)
            {
                NWDQuestUserAdvancement tQuestUserAdvancement = null;
                foreach (NWDQuestUserAdvancement tAdvancement in GetAllObjects())
                {
                    if (tAdvancement.QuestReference.GetObject() == tOriginalQuest)
                    {
                        tQuestUserAdvancement = tAdvancement;
                        tQuestUserAdvancement.FinishQuest(tActualQuest.DependantCounter);
                        break;
                    }
                }
            }
            QuestOriginalReference.SetObject(null);
            QuestActualReference.SetReference(QuestReference.GetReference());
            if (sWithCounter == true)
            {
                FinishCounter++;
            }
            SaveModifications();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
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
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================