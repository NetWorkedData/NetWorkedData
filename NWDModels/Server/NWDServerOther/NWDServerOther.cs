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
        WebServer = 12,
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
        const string ServerTypeProperty = "ServerType";
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server used for")]
        public NWDServerOtherType ServerType { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabDomainNameServer { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabEmail { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabAWSAccessKeyID { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabAWSSecretAccessKey { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabAWSRegion { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabAWSPassword { get; set; }


        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public string EmailDomainNameServer { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public string Emaillogin { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public string EmailPassword { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public string WebDomainNameServer { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Streaming, false)]
        public string StreambDomainNameServer { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.PushNotifications, false)]
        public string AppleCertificat { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.PushNotifications, false)]
        public string GoogleCertificat { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================