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
    [NWDClassDescriptionAttribute("Server Services descriptions Class")]
    [NWDClassMenuNameAttribute("Server Services")]
    public partial class NWDServerServices : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server Services")]
        public NWDReferenceType<NWDServerDomain> Server { get; set; }
        public string Email { get; set; }
        public string Folder { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Authentification SSH / SFTP")]
        [NWDEntitled("SSH IP")]
        public NWDIPType IP { get; set; }
        [NWDEntitled("SSH Port")]
        public int Port { get; set; }
        [NWDEntitled("SSH User")]
        public string User { get; set; }
        [NWDEntitled("SSH Password")]
        public NWDPasswordType Password { get; set; }
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
