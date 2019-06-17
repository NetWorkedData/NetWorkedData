//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
        [NWDInspectorGroupStart("Classification", true, true, true)]
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
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Character and all quests", true, true, true)]
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