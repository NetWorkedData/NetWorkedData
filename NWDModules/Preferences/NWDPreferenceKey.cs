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
    public enum NWDPreferencesDomain : int
    {
        AccountPreference = 0,
        UserPreference = 1,
        LocalPreference = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PRK")]
    [NWDClassDescriptionAttribute("Preference Key")]
    [NWDClassMenuNameAttribute("Preference Key")]
    public partial class NWDPreferenceKey : NWDBasis<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Information")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDGroupEnd]
       
        [NWDGroupStart("Domain")]
        public NWDPreferencesDomain Domain
        {
            get; set;
        }
        [NWDGroupEnd]
       
        [NWDGroupStart("Default value")]
        public NWDMultiType Default
        {
            get; set;
        }
        [NWDGroupEnd]
       
        [NWDGroupStart("Notify change for account or user preference")]
        public bool NotifyChange
        {
            get; set; 
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================