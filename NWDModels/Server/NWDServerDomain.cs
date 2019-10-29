//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
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
    [NWDInternalKeyNotEditable]
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("WWW")]
    [NWDClassDescriptionAttribute("Server descriptions Class")]
    [NWDClassMenuNameAttribute("Server Domain")]
    public partial class NWDServerDomain : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server DNS")]
        [NWDEntitled("Server DNS")]
        public string ServerDNS { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Performance")]
        [NWDIntSlider(0,100)]
        public int BalanceLoad { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Environment Actif")]
        public bool Dev { get; set; }
        public bool Preprod { get; set; }
        public bool Prod { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
