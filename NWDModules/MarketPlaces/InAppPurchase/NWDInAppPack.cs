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
    [NWDClassTrigrammeAttribute("IAP")]
    [NWDClassDescriptionAttribute("In App Purchase descriptions Class")]
    [NWDClassMenuNameAttribute("In App Purchase")]
    public partial class NWDInAppPack : NWDBasis<NWDInAppPack>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Specific Store ID", true, true, true)]
        public string AppleID
        {
            get; set;
        }
        public string GoogleID
        {
            get; set;
        }
        public string UnityID
        {
            get; set;
        }
        public string SteamID
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================