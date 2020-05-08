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
    //[NWDInternalDescriptionNotEditable]
    //[NWDClassUnityEditorOnlyAttribute]
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SSH")]
    [NWDClassDescriptionAttribute("Server descriptions Class")]
    [NWDClassMenuNameAttribute("Server")]
    public partial class NWDServer : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("DNS use to find IP")]
        [NWDTooltips("Optional DNS of this server (not the public DNS, just usable DNS)")]
        public string DomainNameServer { get; set; }
        //[NWDTooltips("Email for SSL certification")]
        //public string Email { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Authentification SSH / SFTP")]
        [NWDEntitled("SSH IP")]
        public NWDIPType IP { get; set; }
        [NWDEntitled("SSH Port")]
        public bool PortChanged { get; set; }
        [NWDIf("PortChanged", false)]
        public int Port { get; set; }
        [NWDEntitled("SSH change to Port")]
        public int FuturPort { get; set; }
        [NWDInspectorGroupEnd()]

        [NWDInspectorGroupStart("Server root and admmin")]
        //public bool UserInstalled { get; set; }
        //[NWDIf("UserInstalled",false)]
        //[NWDEntitled("SSH User")]
        //public string User { get; set; }
        //[NWDIf("UserInstalled", false)]
        //[NWDEntitled("SSH Password")]
        //public NWDPasswordType Password { get; set; }
        public bool AdminInstalled { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin User")]
        public string Admin_User { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin Password")]
        public NWDPasswordType Admin_Password { get; set; }
        public bool RootForbidden { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root User")]
        public string Root_User { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root Password")]
        public NWDPasswordType Root_Password { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Install Server Options")]
        public NWDServerDistribution Distribution { get; set; }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================