//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("BRW")]
    [NWDClassDescriptionAttribute("Barter Place descriptions Class")]
    [NWDClassMenuNameAttribute("Barter Place")]
    public partial class NWDBarterPlace : NWDBasis<NWDBarterPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Items Authorization", true, true, true)]
        public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
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
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade Detail", true, true, true)]
        //public NWDReferencesListType<NWDItem> Moneys
        //{
        //    get; set;
        //}
        [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
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
        [NWDIntSlider(K_BARTER_REQUEST_MIN, K_BARTER_REQUEST_MAX)]
        public int MaxRequestPerUser
        {
            get; set;
        }
        [NWDIntSlider(K_BARTER_PROPOSITIONS_PER_USER_MIN, K_BARTER_PROPOSITIONS_PER_USER_MAX)]
        public int MaxPropositionsPerUser
        {
            get; set;
        }
        [NWDIntSlider(K_BARTER_PROPOSITIONS_PER_REQUEST_MIN, K_BARTER_PROPOSITIONS_PER_REQUEST_MAX)]
        public int MaxPropositionsPerRequest
        {
            get; set;
        }
        public int RefreshDelay
        {
            get; set;
        }
        [NWDTooltips("Minimum time before cancel")]
        public int CancelDelay
        {
            get; set;
        }
        [NWDTooltips("Minimum time before choose")]
        public int WaitingLifeTime
        {
            get; set;
        }
        [NWDTooltips("Maximum time life")]
        public int RequestLifeTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================