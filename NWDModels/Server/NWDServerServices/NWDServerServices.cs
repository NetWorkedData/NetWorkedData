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
    [NWDClassTrigrammeAttribute("SSS")]
    [NWDClassDescriptionAttribute("Server Services descriptions Class")]
    [NWDClassMenuNameAttribute("Server Services")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDServerServices : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        public NWDServerContinent Continent { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Server Services")]
        public NWDReferenceType<NWDServerDomain> ServerDomain { get; set; }
        [NWDEntitled("Email for SSL certification")]
        public string Email { get; set; }
        public bool UserInstalled { get; set; }
        [NWDIf("UserInstalled", false)]
        public string Folder { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP User")]
        public string User { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP Password (AES)")]
        public NWDSecurePassword Secure_Password { get; set; }
        [NWDInspectorGroupEnd]
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