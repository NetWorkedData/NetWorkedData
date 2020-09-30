//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_CRAFTBOOK
using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        [NWDNotEditable]
        public NWDReferenceType<NWDCraftBook> CraftBookAttachment
        {
            get; set;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassMacro("NWD_CRAFTBOOK")]
    [NWDClassTrigrammeAttribute("CFB")]
    [NWDClassDescriptionAttribute("CraftBook Recipes descriptions Class")]
    [NWDClassMenuNameAttribute("CraftBook")]
    public partial class NWDCraftBook : NWDBasis
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
#endif
