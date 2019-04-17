﻿// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:50
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    public partial class NWDUserQuestAdvancement : NWDBasis<NWDUserQuestAdvancement>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string NOTIFICATION_USER_QUEST_CHANGE = "NWDQuestUserAdvancement_UserQuestChangeNotification";
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserQuestAdvancement()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserQuestAdvancement(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserQuestAdvancement GetAdvancementForQuest(NWDQuest sQuest)
        {
            //Debug.Log("GetAdvancementForQuest");
            NWDUserQuestAdvancement rAdvancement = null;
            foreach (NWDUserQuestAdvancement tAdvancement in GetReachableDatas())
            {
                if (tAdvancement.QuestReference.GetData() == sQuest)
                {
                    rAdvancement = tAdvancement;
                    //Debug.Log("GetAdvancementForQuest NWDUserQuestAdvancement not null");
                    break;
                }
            }
            //Debug.Log("GetAdvancementForQuest step 2");

            if (sQuest != null)
            {
                //Debug.Log("GetAdvancementForQuest quest not null");
                if (rAdvancement == null)
                {
                    //Debug.Log("GetAdvancementForQuest but NWDUserQuestAdvancement is null");
                    rAdvancement = NewData();
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
                    rAdvancement.InternalDescription = NWDUserNickname.GetNickname() + " - " + NWDUserNickname.GetUniqueNickname();
                    //--------------
#endif
                    //--------------
                    rAdvancement.Tag = NWDBasisTag.TagUserCreated; // Data generated by User-created
                    rAdvancement.QuestReference.SetReference(sQuest.Reference);
                    rAdvancement.QuestActualReference.SetReference(sQuest.Reference);
                    rAdvancement.UpdateData();
                }

                // Check if the quest advancement is the good advancement
                // else return the advancement for the actual quest part
                if (rAdvancement.QuestReference.GetReference() != rAdvancement.QuestActualReference.GetReference())
                {
                    //Debug.Log("GetAdvancementForQuest Whaooooooooo!");
                    rAdvancement = GetAdvancementForQuest(rAdvancement.QuestActualReference.GetData());
                }
            }
            return rAdvancement;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialog ActualDialogForQuest(NWDQuest sQuest)
        {
            //return sQuest.FirstDialogOnShowQuest(this);
            NWDUserQuestAdvancement tQuestUserAdvancement = GetAdvancementForQuest(sQuest);
            return tQuestUserAdvancement.QuestReference.GetData().FirstDialogOnShowQuest(tQuestUserAdvancement);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AvailableQuest()
        {
            bool rReturn = true;
            NWDQuest tQuest = QuestActualReference.GetData();
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


                bool tItemsRequired = NWDUserOwnership.ConditionalItems(tQuest.RequiredItems);
                bool tItemsGroupsRequired = NWDUserOwnership.ConditionalItemGroups(tQuest.RequiredItemGroups);

                bool tItemsWanted = NWDUserOwnership.ConditionalItems(tQuest.DesiredItems);
                bool tItemsGroupsWanted = NWDUserOwnership.ConditionalItemGroups(tQuest.DesiredItemGroups);

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
                            NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tQuest.DesiredDialogsList.GetReachableDatasList());
                            if (tDialog == null)
                            {
                                rReturn = false;
                            }
                        }
                    }
                    else
                    {
                        NWDReferencesListType<NWDDialog> tAvailableDialogsList = tQuest.AvailableDialogsList; 
                        List<NWDDialog> tDialogsList = tAvailableDialogsList.GetReachableDatasList();
                        NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tDialogsList);
                        if (tDialog == null)
                        {
                            rReturn = false;
                        }
                    }
                }
                else
                {
                    NWDDialog tDialog = NWDDialog.GetFirstValidDialogs(tQuest.RequiredDialogsList.GetReachableDatasList());
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
            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ")");
            NWDQuest tQuest = QuestActualReference.GetData();
            // If Dialog == null I need determine wich dialog I need to Use
            if (sDialog == null)
            {
                // I need propose the First Dialog ? WTF?
            }
            else
            {
                DialogResumeList.AddData(sDialog);
                // Update Quest Advancement
                switch (sDialog.QuestStep)
                {
                    case NWDQuestState.Accept:
                        if (QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Accept");
                            // I must remove the required object or Not?
                            NWDUserOwnership.RemoveItemToOwnership(tQuest.RequiredItemsToRemove);
                            // put quest in accept
                            QuestState = NWDQuestState.Accept;
                            // increment counters
                            AcceptCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDUserQuestAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Refuse:
                        if (QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Refuse");
                            // put quest in cancel
                            QuestState = NWDQuestState.Refuse;
                            // increment counters
                            RefuseCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDUserQuestAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Cancel:
                        if (QuestState == NWDQuestState.Accept)
                        {
                            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Cancel");
                            // put quest in cancel
                            QuestState = NWDQuestState.Cancel;
                            // increment counters
                            CancelCounter++;
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDUserQuestAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                    case NWDQuestState.Success:
                        if (QuestState != NWDQuestState.Success)

                        //if (QuestState == NWDQuestState.Accept || QuestState == NWDQuestState.Start || QuestState == NWDQuestState.StartAlternate)
                        {
                            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Success");
                            bool tItemsWanted = NWDUserOwnership.ConditionalItems(tQuest.DesiredItems);
                            bool tItemsGroupsWanted = NWDUserOwnership.ConditionalItemGroups(tQuest.DesiredItemGroups);
                            if (tItemsWanted && tItemsGroupsWanted)
                            {
                                // put quest in success
                                QuestState = NWDQuestState.Rewarding;
                                // I reset the dialog historic
                                DialogResumeList.SetValue(string.Empty);

                                // I must remove the required object or Not?
                                NWDUserOwnership.RemoveItemToOwnership(tQuest.DesiredItemsToRemove);
                                // Add items
                                NWDUserOwnership.AddItemToOwnership(tQuest.RewardsItems);
                                // Add Items by itemPacks
                                //foreach (KeyValuePair<NWDItemPack, int> tKeyValue in tQuest.RewardsItemPack.GetObjectAndQuantity())
                                //{
                                //    for (int tI = 0; tI < tKeyValue.Value; tI++)
                                //    {
                                //        NWDOwnership.AddItemToOwnership(tKeyValue.Key.Items);
                                //    }
                                //}
                                //// Add items by Pack
                                //foreach (KeyValuePair<NWDPack, int> tKeyValue in tQuest.RewardsPacks.GetObjectAndQuantity())
                                //{
                                //    for (int tI = 0; tI < tKeyValue.Value; tI++)
                                //    {
                                //        NWDOwnership.AddItemToOwnership(tKeyValue.Key.GetAllItemReferenceAndQuantity());
                                //    }
                                //}
                                // increment counters
                                SuccessCounter++;
                                NWDQuest tNextQuest = sDialog.NextQuest.GetData();
                                if (tNextQuest != null)
                                {
                                    QuestActualReference.SetReference(tNextQuest.Reference);
                                    NWDUserQuestAdvancement tQuestUserAdvancement = NWDUserQuestAdvancement.GetAdvancementForQuest(tNextQuest);
                                    tQuestUserAdvancement.QuestOriginalReference.SetReference(QuestReference.GetReference());
                                    tQuestUserAdvancement.UpdateData();

                                }
                                else
                                {
                                    FinishQuest();
                                }
                                // send BTBNotification
                                BTBNotificationManager.SharedInstance().PostNotification(this, NWDUserQuestAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                            }
                            else
                            {
                                // Error
                                //Debug.LogWarning("Quest " + QuestActualReference.GetReference() + " must be changed to 'success' but items wanted not presents! Fix the quest dialog storyboard by limit dialog by items required!");
#if UNITY_EDITOR
                                EditorUtility.DisplayDialog("ERROR", "Quest " + QuestActualReference.GetReference() + " must be changed to 'success' but items wanted not presents! Fix the quest dialog storyboard by limit dialog by items required!", "OK");
#endif
                            }
                        }
                        break;
                    case NWDQuestState.Fail:
                        //if (QuestState == NWDQuestState.Accept)
                        {
                            //Debug.Log("NWDQuestUserAdvancement AdvancementDialog (" + sDialog.Reference + ") = > Fail");
                            // put quest in fail
                            QuestState = NWDQuestState.Fail;
                            // increment counters
                            FailCounter++;
                            NWDQuest tNextQuest = sDialog.NextQuest.GetData();
                            if (tNextQuest != null)
                            {
                                QuestActualReference.SetReference(tNextQuest.Reference);
                                NWDUserQuestAdvancement tQuestUserAdvancement = NWDUserQuestAdvancement.GetAdvancementForQuest(tNextQuest);
                                tQuestUserAdvancement.QuestOriginalReference.SetReference(QuestReference.GetReference());
                                tQuestUserAdvancement.UpdateData();
                            }
                            else
                            {
                                FinishQuest();
                            }
                            // send BTBNotification
                            BTBNotificationManager.SharedInstance().PostNotification(this, NWDUserQuestAdvancement.NOTIFICATION_USER_QUEST_CHANGE);
                        }
                        break;
                }
                if (sDialog.AnswerState == NWDDialogState.Step || sDialog.AnswerState == NWDDialogState.Sequent)
                {
                    LastDialogReference.SetData(sDialog);
                }
                if (sDialog.AnswerState == NWDDialogState.Reset)
                {
                    // I remove the last dialog... waiting to decalre a new dialog or restart the quest
                    QuestState = NWDQuestState.None;
                    LastDialogReference.SetData(null);
                    QuestActualReference.SetReference(QuestReference.GetReference());
                }
                UpdateData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FinishQuest(bool sWithCounter = true)
        {
            NWDQuest tOriginalQuest = QuestOriginalReference.GetData();
            NWDQuest tActualQuest = QuestActualReference.GetData();
            if (tOriginalQuest != null)
            {
                NWDUserQuestAdvancement tQuestUserAdvancement = null;
                foreach (NWDUserQuestAdvancement tAdvancement in GetReachableDatas())
                {
                    if (tAdvancement.QuestReference.GetData() == tOriginalQuest)
                    {
                        tQuestUserAdvancement = tAdvancement;
                        tQuestUserAdvancement.FinishQuest(tActualQuest.DependantCounter);
                        break;
                    }
                }
            }
            QuestOriginalReference.SetData(null);
            QuestActualReference.SetReference(QuestReference.GetReference());
            if (sWithCounter == true)
            {
                FinishCounter++;
            }
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================