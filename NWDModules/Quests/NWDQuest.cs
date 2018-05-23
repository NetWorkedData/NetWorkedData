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
    public enum NWDQuestType : int
    {
        Unique = 0,
        Multiple = 1,
        Infiny = 9,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDQuestImportance : int
    {
        Normal = 0,     // normal quest ... 
        Slave = 1,      // quest is allways slave of another quest
        Master = 2,     // quest is allways master
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDQuestConnection : NWDConnection<NWDQuest>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("QST")]
    [NWDClassDescriptionAttribute("Quest descriptions Class")]
    [NWDClassMenuNameAttribute("Quest")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDQuest : NWDBasis<NWDQuest>
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
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> Categories
        {
            get; set;
        }
        //public NWDReferencesListType<NWDFamily> Families
        //{
        //    get; set;
        //}
        //public NWDReferencesListType<NWDKeyword> Keywords
        //{
        //    get; set;
        //}
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Quest's Description (in Quest's Book)", true, true, true)]
        [NWDEntitled("Title", "Title of the quest in the description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableStringType SubTitle
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> ItemToDescribe
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Type of quest", true, true, true)]
        [NWDTooltips("Determine if quest is replayable or not")]
        public NWDQuestType Type
        {
            get; set;
        }
        [NWDTooltips("Determine if quest can be the actual quest , master or slave when user active this quest")]
        public NWDQuestImportance Importance
        {
            get; set;
        }
        [NWDTooltips("Determine if quest is replayable how much time (if Type = Multiple)")]
        [NWDIf("Type", 1)]
        public int Number
        {
            get; set;
        }
        [NWDTooltips("Determine if original quest must increment finish counter if this quest is finish")]
        public bool DependantCounter
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Availability schedule", true, true, true)]
        [NWDTooltips("Determine the availability schedule of this quest")]
        public NWDDateTimeScheduleType AvailabilitySchedule
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Items required to start quest", true, true, true)]
        [NWDTooltips("Required itemGroup")]
        public NWDReferencesConditionalType<NWDItemGroup> RequiredItemGroups
        {
            get; set;
        }
        [NWDTooltips("Required items")]
        public NWDReferencesConditionalType<NWDItem> RequiredItems
        {
            get; set;
        }
        [NWDTooltips("Remove Required items (removable when quest accepted)")]
        public NWDReferencesQuantityType<NWDItem> RequiredItemsToRemove
        {
            get; set;
        }
        [NWDTooltips("If you have not the requiered item")]
        public NWDReferencesListType<NWDDialog> RequiredDialogsList { get; set; }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Quest First Dialog", true, true, true)]
        [NWDEntitled("Normal Dialog")]
        public NWDReferencesListType<NWDDialog> AvailableDialogsList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Items wanted to finish quest", true, true, true)]
        [NWDTooltips("Wanted itemGroup (not removable when quest success)")]
        public NWDReferencesConditionalType<NWDItemGroup> DesiredItemGroups
        {
            get; set;
        }
        [NWDTooltips("Wanted item (removable when quest success)")]
        public NWDReferencesConditionalType<NWDItem> DesiredItems
        {
            get; set;
        }
        [NWDTooltips("Wanted item (removable when quest success)")]
        public NWDReferencesQuantityType<NWDItem> DesiredItemsToRemove
        {
            get; set;
        }
        //[NWDTooltips("Items wanted remove when quest is success?")]
        //public bool RemoveItemsWanted
        //{
        //    get; set;
        //}
        [NWDEntitled("Alternate Dialog")]
        [NWDTooltips("If you have allready the wanted item the quest start with this dialog")]
        public NWDReferencesListType<NWDDialog> DesiredDialogsList
        {
            get; set;
        } // to start with ListOfItemsAsked
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Quest reward", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> RewardsItems
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItemPack> RewardsItemPack
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDPack> RewardsPacks
        {
            get; set;
        }
        //public int NumberOfRewards
        //{
        //    get; set;
        //}
         //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
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
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog FirstDialogOnShowQuest(NWDQuestUserAdvancement tQuestUserAdvancement=null)
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
                bool tItemsRequired = NWDOwnership.ConditionalItems(RequiredItems);
                bool tItemsGroupsRequired = NWDOwnership.ConditionalItemGroups(RequiredItemGroups);

                bool tItemsWanted = NWDOwnership.ConditionalItems(DesiredItems);
                bool tItemsGroupsWanted = NWDOwnership.ConditionalItemGroups(DesiredItemGroups);

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
                                SaveModifications();
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
                                            SaveModifications();
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
                                SaveModifications();
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
                            SaveModifications();
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
        public void MyInstanceMethod()
        {
            // do something with this object
        }
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
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 240.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            tStyle.richText = true;

            if (RequiredItems == null)
            {
                RequiredItems = new NWDReferencesConditionalType<NWDItem>();
            }

            if (RequiredItemGroups == null)
            {
                RequiredItemGroups = new NWDReferencesConditionalType<NWDItemGroup>();
            }

            if (RequiredItemsToRemove == null)
            {
                RequiredItemsToRemove = new NWDReferencesQuantityType<NWDItem>();
            }
            string tQuestTitle = Title.GetBaseString();
            string tQuestDescription = Description.GetBaseString();
            string tRequiredItemsDescription = RequiredItems.Description();
            string tRequiredItemGroupsDescription = RequiredItemGroups.Description();
            //string tRequiredItemToRemoveDescription = RequiredItemsToRemove.Description();
            string tText = "" + InternalDescription + "\n\n<b>Title : </b>\n" + tQuestTitle + "\n\n<b>Description : </b>\n"+ tQuestDescription+
                "\n <b>Required Items : </b>\n" + tRequiredItemsDescription + 
                "\n <b>Required Items Groups: </b>\n"+ tRequiredItemGroupsDescription+
                "\n ";
            GUI.Label(sRect, tText, tStyle);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override Color AddOnNodeColor()
        {
            return Color.white;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================