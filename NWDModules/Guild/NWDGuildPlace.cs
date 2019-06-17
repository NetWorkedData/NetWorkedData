//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:55
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
    public enum NWDGuildStatus
    {
        None = 0,
        Active = 1,
        Waiting = 2,
        Deal = 3,
        Accepted = 4,
        Expired = 6,
        Cancel = 8,
        Cancelled = 9,
        Refresh = 10,

        Sync = 40,

        Force = 99, // admin only 
        ForceNone = 999, // admin only 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("GPC")]
    [NWDClassDescriptionAttribute("Guild Place descriptions Class")]
    [NWDClassMenuNameAttribute("Guild Place")]
    public partial class NWDGuildPlace : NWDBasis<NWDGuildPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Guild Detail", true, true, true)]
        [NWDIntSlider(K_Guild_REQUEST_MIN, K_Guild_REQUEST_MAX)]
        //[NWDAlias("MaxMember")]
        public int MaxMember
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================