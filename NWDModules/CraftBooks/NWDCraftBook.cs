//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis<NWDItem>
    {
        [NWDNotEditable]
        public NWDReferenceType<NWDCraftBook> CraftRecipeAttachment
        {
            get; set;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CFB")]
    [NWDClassDescriptionAttribute("CraftBook Recipes descriptions Class")]
    [NWDClassMenuNameAttribute("CraftBook")]
    public partial class NWDCraftBook : NWDBasis<NWDCraftBook>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)] // ok
        //[NWDNotEditable]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Recipe attribut", true, true, true)] // ok
        public bool OrderIsImportant
        {
            get; set;
        }
        public NWDReferenceType<NWDCraftRecipient> RecipientGroup
        {
            get; set;
        }
        public NWDReferencesArrayType<NWDItemGroup> ItemGroupIngredient
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemResult
        {
            get; set;
        }
        public NWDReferencesListType<NWDCraftReward> AdditionalReward
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("FX (Special Effects)", true, true, true)]
        public NWDPrefabType SuccessParticles
        {
            get; set;
        }
        public NWDPrefabType SuccessSound
        {
            get; set;
        }
        public NWDPrefabType FailParticles
        {
            get; set;
        }
        public NWDPrefabType FailSound
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Development addons", true, true, true)]
        //[NWDNotEditableAttribute]
        //public string RecipeHash
        //{
        //    get; set;
        //}
        public NWDStringsArrayType RecipeHashesArray
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================