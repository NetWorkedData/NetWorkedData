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
        [NWDInspectorGroupStart("Information")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Domain")]
        public NWDPreferencesDomain Domain
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Default value")]
        public NWDMultiType Default
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Notify change for account or user preference")]
        public bool NotifyChange
        {
            get; set; 
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================