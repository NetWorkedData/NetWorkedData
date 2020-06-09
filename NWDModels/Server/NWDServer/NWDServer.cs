//=====================================================================================================================
//
//  ideMobi 2020©
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
    [NWDClassTrigrammeAttribute("SSH")]
    [NWDClassDescriptionAttribute("Server descriptions Class")]
    [NWDClassMenuNameAttribute("Server")]
    public partial class NWDServer : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("DNS use to find IP")]
        [NWDTooltips("Optional DNS of this server (not the public DNS, just usable DNS)")]
        public string DomainNameServer { get; set; }
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

        [NWDInspectorGroupStart("Admmin")]
        public bool AdminInstalled { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin User")]
        public string Admin_User { get; set; }
        [NWDIf("AdminInstalled", false)]
        //[NWDEntitled("SSH Admin Password")]
        //public NWDPasswordType Admin_Password { get; set; }
        [NWDIf("AdminInstalled", false)]
        [NWDEntitled("SSH Admin Password (AES)")]
        public NWDSecurePassword Admin_Secure_Password { get; set; }
        [NWDInspectorGroupEnd()]

        [NWDInspectorGroupStart("Root")]
        public bool RootForbidden { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root User")]
        public string Root_User { get; set; }
        [NWDIf("RootForbidden", false)]
        //[NWDEntitled("SSH Root Password")]
        //public NWDPasswordType Root_Password { get; set; }
        [NWDIf("RootForbidden", false)]
        [NWDEntitled("SSH Root Password (AES)")]
        public NWDSecurePassword Root_Secure_Password { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Install Server Options")]
        public NWDServerDistribution Distribution { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================