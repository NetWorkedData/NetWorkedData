//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("BPC")]
    [NWDClassDescriptionAttribute("Barter Place descriptions Class")]
    [NWDClassMenuNameAttribute("Barter Place")]
    public partial class NWDBarterPlace : NWDBasis<NWDBarterPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items Authorization", true, true, true)]
        public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
        public NWDReferencesListType<NWDCategory> FilterCategories { get; set; }
        public NWDReferencesListType<NWDFamily> FilterFamilies { get; set; }
        public NWDReferencesListType<NWDKeyword> FilterKeywords { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Barter Detail", true, true, true)]
        //public NWDReferencesListType<NWDItem> Moneys
        //{
        //    get; set;
        //}
       // [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly { get; set; }
        public NWDReferencesQuantityType<NWDItem> RequestFixCost { get; set; }
        public NWDReferencesQuantityType<NWDItem> RequestPerItemCost { get; set; }

        [NWDIntSlider(K_BARTER_REQUEST_PER_USER_MIN, K_BARTER_REQUEST_PER_USER_MAX)]
       // [NWDAlias("MaxRequestPerUser")]
        public int MaxRequestPerUser { get; set; }

        [NWDIntSlider(K_BARTER_PROPOSITIONS_PER_USER_MIN, K_BARTER_PROPOSITIONS_PER_USER_MAX)]
       // [NWDAlias("MaxPropositionsPerUser")]
        public int MaxPropositionsPerUser { get; set; }

        [NWDIntSlider(K_BARTER_PROPOSITIONS_PER_REQUEST_MIN, K_BARTER_PROPOSITIONS_PER_REQUEST_MAX)]
        //[NWDAlias("MaxPropositionsPerRequest")]
        public int MaxPropositionsPerRequest { get; set; }
        [NWDIntSlider(10, 300)]
        public int RefreshDelay { get; set; }
        [NWDTooltips("Minimum time before cancel")]
        //[NWDAlias("CancelDelay")]
        public int CancelDelay { get; set; }
        [NWDTooltips("Minimum time before choose")]
       // [NWDAlias("WaitingLifeTime")]
        public int WaitingLifeTime { get; set; }
        [NWDTooltips("Maximum time life")]
       // [NWDAlias("RequestLifeTime")]
        public int RequestLifeTime { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================