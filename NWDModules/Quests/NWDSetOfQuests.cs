//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SOQ")]
    [NWDClassDescriptionAttribute("Set of Quests Class")]
    [NWDClassMenuNameAttribute("Set of Quests")]
    public partial class NWDSetOfQuests : NWDBasis<NWDSetOfQuests>
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
        public NWDReferencesListType<NWDFamily> Families
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> Keywords
        {
            get; set;
        }
        [NWDGroupEndAttribute]
       
        [NWDGroupStartAttribute("Character and all quests", true, true, true)]
        public NWDReferenceType<NWDCharacter> CharacterReference
        {
            get; set;
        }
        public NWDReferencesListType<NWDQuest> QuestsList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================