//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UQA")]
    [NWDClassDescriptionAttribute("Quest User Advancement descriptions Class")]
    [NWDClassMenuNameAttribute("Quest User Advancement")]
    public partial class NWDUserQuestAdvancement : NWDBasis<NWDUserQuestAdvancement>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Advancement information", true, true, true)]
        [NWDTooltips("Account, reference, last dialogue see and state of the quest")]
        public NWDReferenceType<NWDAccount> AccountReference
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
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
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Counters", "Quest Counter", true, true, true)]
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
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Counter of gloab quest sequence", "", true, true, true)]
        public int FinishCounter
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================