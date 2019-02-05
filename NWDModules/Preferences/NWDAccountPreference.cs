//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("APR")]
    [NWDClassDescriptionAttribute("Account Preference")]
    [NWDClassMenuNameAttribute("Account Preference")]
    public partial class NWDAccountPreference : NWDBasis<NWDAccountPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account {get; set; }
        public NWDReferenceType<NWDPreferenceKey> PreferenceKey
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Values")]
        public NWDMultiType Value
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================