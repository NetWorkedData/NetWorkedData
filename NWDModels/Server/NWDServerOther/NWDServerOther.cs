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
    [NWDClassTrigrammeAttribute("SSO")]
    [NWDClassDescriptionAttribute("Server Other descriptions Class")]
    [NWDClassMenuNameAttribute("Server Other")]
    public partial class NWDServerOther : NWDBasisUnsynchronize
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
        public NWDSecurePassword GitLabAWSAccessKeyID { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public NWDSecurePassword GitLabAWSSecretAccessKey { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public string GitLabAWSRegion { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.GitLab, false)]
        public NWDSecurePassword GitLabAWSPassword { get; set; }


        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public string EmailDomainNameServer { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public string Emaillogin { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Email, false)]
        public NWDSecurePassword EmailPassword { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public string WebDomainNameServer { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.Streaming, false)]
        public string StreamDomainNameServer { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.PushNotifications, false)]
        public NWDSecurePassword AppleCertificat { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.PushNotifications, false)]
        public NWDSecurePassword GoogleCertificat { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================