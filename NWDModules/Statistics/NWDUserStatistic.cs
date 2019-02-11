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
    [NWDClassTrigrammeAttribute("UST")]
    [NWDClassDescriptionAttribute("UserStat")]
    [NWDClassMenuNameAttribute("UserStat")]
    public partial class NWDUserStatistic : NWDBasis<NWDUserStatistic>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Connections")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
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