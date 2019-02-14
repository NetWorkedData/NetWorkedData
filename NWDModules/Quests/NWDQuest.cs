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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("QST")]
    [NWDClassDescriptionAttribute("Quest descriptions Class")]
    [NWDClassMenuNameAttribute("Quest")]
    public partial class NWDQuest : NWDBasis<NWDQuest>
    {
        //-------------------------------------------------------------------------------------------------------------
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
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]

       

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

       

        [NWDGroupStartAttribute("Availability schedule", true, true, true)]
        [NWDTooltips("Determine the availability schedule of this quest")]
        public NWDDateTimeScheduleType AvailabilitySchedule
        {
            get; set;
        }
        [NWDGroupEndAttribute]

       

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

       

        [NWDGroupStartAttribute("Quest First Dialog", true, true, true)]
        [NWDEntitled("Normal Dialog")]
        public NWDReferencesListType<NWDDialog> AvailableDialogsList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

       

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

       

        [NWDGroupStartAttribute("Quest auto reward", true, true, true)]
        [NWDTooltips("The list of auto increment rewards at success of quest")]
        public NWDReferencesQuantityType<NWDItem> RewardsItems
        {
            get; set;
        }
        [NWDGroupEndAttribute]

       

        [NWDGroupStartAttribute("Quest reward by choose ", true, true, true)]
        //public NWDReferencesQuantityType<NWDItemPack> RewardsItemPack
        //{
        //    get; set;
        //}
        [NWDTooltips("The list of possible rewards")]
        public NWDReferencesQuantityType<NWDPack> RewardsPacks
        {
            get; set;
        }
        [NWDTooltips("The number of reward to choose (traditionnal 1 of them)")]
        public int RewardsPacksNumber
        {
            get; set;
        }
        //public int NumberOfRewards
        //{
        //    get; set;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================