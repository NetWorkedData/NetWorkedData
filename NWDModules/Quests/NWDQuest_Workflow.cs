//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDQuestType : int
    {
        Unique = 0,
        Multiple = 1,
        Infiny = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDQuestImportance : int
    {
        Normal = 0,     // normal quest ... 
        Slave = 1,      // quest is allways slave of another quest
        Master = 2,     // quest is allways master
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDQuest : NWDBasis<NWDQuest>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuest()
        {
            //Debug.Log("NWDQuest Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDQuest Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //Title = new NWDLocalizableStringType();
            ////Title.AddBaseString("");
            //SubTitle = new NWDLocalizableStringType();
            ////SubTitle.AddBaseString("");
            //Description = new NWDLocalizableTextType();
            ////Description.AddBaseString("");
            ////CanBecameActualQuest = false;
            //DesiredItemGroups = new NWDReferencesConditionalType<NWDItemGroup>();
            //DesiredItems = new NWDReferencesConditionalType<NWDItem>();
            //DesiredItemsToRemove = new NWDReferencesQuantityType<NWDItem>();
            //AvailableDialogsList = new NWDReferencesListType<NWDDialog>;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog FirstDialogOnShowQuest(NWDUserQuestAdvancement tQuestUserAdvancement=null)
        {
            NWDDialog rDialog = null;
            if (tQuestUserAdvancement != null)
            {
                // I try to get the last dialog reccord in advancement 
                rDialog = tQuestUserAdvancement.LastDialogReference.GetObject();
            }
            if (rDialog == null)
            {
                // I return the first dialog of this quest
                // If first dialog is not an step dialog I found the Dialog to use
                bool tItemsRequired = NWDUserOwnership.ConditionalItems(RequiredItems);
                bool tItemsGroupsRequired = NWDUserOwnership.ConditionalItemGroups(RequiredItemGroups);

                bool tItemsWanted = NWDUserOwnership.ConditionalItems(DesiredItems);
                bool tItemsGroupsWanted = NWDUserOwnership.ConditionalItemGroups(DesiredItemGroups);

                if (DesiredItems == null)
                {
                    DesiredItems = new NWDReferencesConditionalType<NWDItem>();
                }
                if (DesiredItemGroups == null)
                {
                    DesiredItemGroups = new NWDReferencesConditionalType<NWDItemGroup>();
                }
                if (DesiredItems.IsEmpty() && DesiredItemGroups.IsEmpty())
                {
                    tItemsWanted = false;
                    tItemsGroupsWanted = false;
                }
                // I need propose the First Dialaog
                if (tItemsRequired && tItemsGroupsRequired)
                {
                    // I have the objects required
                    if (tItemsWanted && tItemsGroupsWanted)
                    {
                        // And I have the object wanted too ... 
                        if (DesiredDialogsList != null)
                        {
                            rDialog = NWDDialog.GetFirstValidDialogs(DesiredDialogsList.GetObjectsList());
                            if (tQuestUserAdvancement != null)
                            {
                                tQuestUserAdvancement.QuestState = NWDQuestState.StartAlternate;
                                tQuestUserAdvancement.UpdateData();
                            }
                        }
                        // if I Have no valid dialog ... use the available dialog
                        if (rDialog == null)
                        {
                            if (AvailableDialogsList != null)
                                {
                                    rDialog = NWDDialog.GetFirstValidDialogs(AvailableDialogsList.GetObjectsList());
                                    if (tQuestUserAdvancement != null)
                                        {
                                            tQuestUserAdvancement.QuestState = NWDQuestState.Start;
                                    tQuestUserAdvancement.UpdateData();
                                        }
                                }
                        }
                    }
                    else
                    {
                        if (AvailableDialogsList != null)
                        {
                            //rDialog = DialogReference.GetObject();
                            rDialog = NWDDialog.GetFirstValidDialogs(AvailableDialogsList.GetObjectsList());
                            if (tQuestUserAdvancement != null)
                            {
                                tQuestUserAdvancement.QuestState = NWDQuestState.Start;
                                tQuestUserAdvancement.UpdateData();
                            }
                        }
                    }
                }
                else
                {
                    // Idon't have the objects required
                    if (RequiredDialogsList != null)
                    {
                        rDialog = NWDDialog.GetFirstValidDialogs(RequiredDialogsList.GetObjectsList());
                        //rDialog = NoRequiredDialogReference.GetObject();
                        if (tQuestUserAdvancement != null)
                        {
                            tQuestUserAdvancement.QuestState = NWDQuestState.None;
                            tQuestUserAdvancement.UpdateData();
                        }
                    }
                }
            }
            if (rDialog != null)
            {
                // analyze this dialog 
                // is it the real dialog ?
                rDialog = rDialog.ReturnRealDialog(tQuestUserAdvancement);
            }
            return rDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================