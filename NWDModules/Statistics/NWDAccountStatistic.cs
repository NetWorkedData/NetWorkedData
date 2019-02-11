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
    [NWDClassTrigrammeAttribute("STC")]
    [NWDClassDescriptionAttribute("Account Stat")]
    [NWDClassMenuNameAttribute("Account Stat")]
    public partial class NWDAccountStatistic : NWDBasis<NWDAccountStatistic>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account {get; set;}
        public NWDReferenceType<NWDStatisticKey> StatKey
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Values")]
        public double Total
        {
            get; set;
        }
        public double Counter
        {
            get; set;
        }
        public double Average
        {
            get; set;
        }
        public double AverageWithParent
        {
            get; set;
        }
        public double Last
        {
            get; set;
        }
        public double Max
        {
            get; set;
        }
        public double Min
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================