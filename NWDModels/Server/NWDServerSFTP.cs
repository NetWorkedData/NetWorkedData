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
    public enum NWDServerDistribution
    {
        debian9,
        debian10,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDInternalKeyNotEditable]
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
        public string Email { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Authentification SSH / SFTP")]
        public NWDIPType IP { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public NWDPasswordType Password { get; set; }
        public string Folder { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Install Server Options")]
        public NWDServerDistribution Distribution { get; set; }
        public string ServerName { get; set; }
        public NWDPasswordType RootPassword { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
