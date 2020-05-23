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
    /// <summary>
    /// NWDIPBan class. This class is use for (complete description here).
    /// </summary>
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IPB")]
    [NWDClassDescriptionAttribute("IP banned")]
    [NWDClassMenuNameAttribute("IP Ban")]
    [NWDClassClusterAttribute(1, 2048)]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDIPBan : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
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