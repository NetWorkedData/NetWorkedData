//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:22
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================


using System;
using System.Collections.Generic;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("APR")]
    [NWDClassDescriptionAttribute("Account Preference")]
    [NWDClassMenuNameAttribute("Account Preference")]
    public partial class NWDAccountPreference : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account {get; set; }
        public NWDReferenceType<NWDPreferenceKey> PreferenceKey
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Values")]
        public NWDMultiType Value
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================