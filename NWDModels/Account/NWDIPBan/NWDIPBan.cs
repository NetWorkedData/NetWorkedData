//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("IPB")]
    [NWDClassDescriptionAttribute("IP banned")]
    [NWDClassMenuNameAttribute("IP Ban")]
    public partial class NWDIPBan : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        //PROPERTIES
        [NWDAddIndexed(NWD.K_IP_BAN_INDEX,"AXC")]
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
        public NWDIPType IP {get; set;}
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public NWDDateTimeUtcType Deadline {get; set;}
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public int Counter {get; set;}
        //[NWDNotEditable]
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public int CounterMaximum {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================