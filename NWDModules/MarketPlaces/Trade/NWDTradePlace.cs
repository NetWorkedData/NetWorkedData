//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:40
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
    public enum NWDTradeStatus
    {
        None = 0,
        Submit = 1,
        Waiting = 2,
        Deal = 3,
        Accepted = 4,
        Expired = 6,
        Cancel = 8,
        Cancelled = 9,

        NoDeal = 20,

        Refresh = 40,

        Force = 99, // admin only 
        ForceNone = 999, // admin only 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("TRW")]
    [NWDClassDescriptionAttribute("Trade Place descriptions Class")]
    [NWDClassMenuNameAttribute("Trade Place")]
    public partial class NWDTradePlace : NWDBasis<NWDTradePlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items Authorization", true, true, true)]
        public NWDReferencesListType<NWDWorld> FilterWorlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> FilterCategories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FilterFamilies
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> FilterKeywords
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Trade Detail", true, true, true)]
        //[NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        public NWDReferencesListType<NWDItem> Moneys
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> RequestFixCost
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> RequestPerItemCost
        {
            get; set;
        }
        public int MaxRequestPerUser
        {
            get; set;
        }
        [NWDIntSlider(10, 300)]
        public int RefreshDelay
        {
            get; set;
        }
        [NWDTooltips("Minimum time before cancel")]
        public int CancelDelay
        {
            get; set;
        }
        public int RequestLifeTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================