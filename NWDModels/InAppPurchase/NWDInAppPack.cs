//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:14
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IAP")]
    [NWDClassDescriptionAttribute("In App Pack descriptions Class")]
    [NWDClassMenuNameAttribute("In App Pack")]
    public partial class NWDInAppPack : NWDBasis
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