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
    [NWDClassSpecialAccountOnlyAttribute]
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("SSS")]
    [NWDClassDescriptionAttribute("Server SFTP descriptions Class")]
    [NWDClassMenuNameAttribute("Server SFTP")]
    public partial class NWDServerSFTP : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Account Administrator")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server DNS")]
        public NWDReferenceType<NWDServerDNS> Server { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Authentification SFTP")]
        public int Port { get; set; }
        public string Folder { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
