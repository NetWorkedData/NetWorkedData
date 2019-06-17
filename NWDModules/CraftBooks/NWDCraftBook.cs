//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:37
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
        public NWDReferenceType<NWDCraftBook> CraftBookAttachment
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
        public NWDReferenceType<NWDItem> ItemDescription
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