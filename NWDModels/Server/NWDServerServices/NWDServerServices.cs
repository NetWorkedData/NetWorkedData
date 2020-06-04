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
    [NWDInternalKeyNotEditable]
    //[NWDClassUnityEditorOnlyAttribute]
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SSS")]
    [NWDClassDescriptionAttribute("Server Services descriptions Class")]
    [NWDClassMenuNameAttribute("Server Services")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDServerServices : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Server Services")]
        public NWDReferenceType<NWDServerDomain> ServerDomain { get; set; }
        [NWDEntitled("Email for SSL certification")]
        public string Email { get; set; }

        public bool UserInstalled { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP User")]
        public string User { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP Password")]
        public NWDPasswordType Password { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP Password (AES)")]
        public NWDSecurePassword Secure_Password { get; set; }
        [NWDIf("UserInstalled", false)]
        public string Folder { get; set; }
        [NWDInspectorGroupEnd]
        //[NWDInspectorGroupStart("Authentification SSH / SFTP")]
        //[NWDEntitled("SSH IP")]
        //public NWDIPType IP { get; set; }
        //[NWDEntitled("SSH Port")]
        //public int Port { get; set; }
        //[NWDEntitled("SSH User")]
        //public string User { get; set; }
        //[NWDEntitled("SSH Password")]
        //public NWDPasswordType Password { get; set; }
        //[NWDEntitled("SSH Admin User")]
        //public string Admin_User { get; set; }
        //[NWDEntitled("SSH Admin Password")]
        //public NWDPasswordType Admin_Password { get; set; }
        //[NWDEntitled("SSH Root User")]
        //public string Root_User { get; set; }
        //[NWDEntitled("SSH Root Password")]
        //public NWDPasswordType Root_Password { get; set; }
        //[NWDInspectorGroupEnd]
        //[NWDInspectorGroupStart("Install Server Options")]
        //public NWDServerDistribution Distribution { get; set; }
        //[NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Environment Actif")]
        [NWDNotEditable]
        public bool Dev { get; set; }
        [NWDNotEditable]
        public bool Preprod { get; set; }
        [NWDNotEditable]
        public bool Prod { get; set; }
        [NWDNotEditable]
        public string Information { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================