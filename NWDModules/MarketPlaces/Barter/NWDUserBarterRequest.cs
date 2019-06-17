//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:7
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BasicToolBox;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBR")]
    [NWDClassDescriptionAttribute("User Barter Request descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Request")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterRequest : NWDBasis<NWDUserBarterRequest>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Barter Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        //[NWDAlias("BarterPlace")]
        public NWDReferenceType<NWDBarterPlace> BarterPlace { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("For Relationship Only", true, true, true)]

       // [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly { get; set; }
        //[NWDAlias("RelationshipAccountReferences")]
        public NWDReferenceType<NWDUserRelationship> UserRelationship { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Barter References", true, true, true)]
        //[NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }
        //[NWDAlias("ItemsSuggested")]
        public NWDReferencesQuantityType<NWDItem> ItemsSuggested { get; set; }
        //[NWDAlias("ItemsReceived")]
        [NWDNotEditable]
        public NWDReferencesQuantityType<NWDItem> ItemsReceived { get; set; }
        //[NWDAlias("BarterStatus")]
        public NWDTradeStatus BarterStatus { get; set; }
        [NWDNotEditable]
       // [NWDAlias("BarterHash")]
        public string BarterHash { get; set; }
        [NWDNotEditable]
        //[NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime { get; set; }
        [NWDNotEditable]
        [NWDIntSlider(NWDBarterPlace.K_BARTER_PROPOSITIONS_PER_REQUEST_MIN, NWDBarterPlace.K_BARTER_PROPOSITIONS_PER_REQUEST_MAX)]
        //[NWDAlias("MaxPropositions")]
        public int MaxPropositions { get; set; }
        [NWDNotEditable]
        //[NWDAlias("PropositionsCounter")]
        public int PropositionsCounter { get; set; }
        [NWDNotEditable]
        //[NWDAlias("Propositions")]
        public NWDReferencesListType<NWDUserBarterProposition> Propositions { get; set; }
        //[NWDAlias("WinnerProposition")]
        public NWDReferenceType<NWDUserBarterProposition> WinnerProposition { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Tags", true, true, true)]
        public NWDReferencesListType<NWDWorld> TagWorlds { get; set; }
        public NWDReferencesListType<NWDCategory> TagCategories { get; set; }
        public NWDReferencesListType<NWDFamily> TagFamilies { get; set; }
        public NWDReferencesListType<NWDKeyword> TagKeywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================