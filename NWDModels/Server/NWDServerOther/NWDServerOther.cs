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
    public enum NWDServerOtherType : int
    {
        No = 0,
        Email = 3,
        PushNotifications = 8,
        Streaming = 9,
        GitLab = 99,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassUnityEditorOnlyAttribute]
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SSO")]
    [NWDClassDescriptionAttribute("Server Other descriptions Class")]
    [NWDClassMenuNameAttribute("Server Other")]
    public partial class NWDServerOther : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server used for")]
        public NWDServerOtherType ServerType { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================