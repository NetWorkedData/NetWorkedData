//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDServerOtherType : int
    {
        No = 0,
        ActionServer = 1,
        Email = 3,
        PushNotifications = 8,
        Streaming = 9,
        WebServer = 12,
        WebDAV = 45,
        GitLab = 99,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassMacro("UNITY_EDITOR")]
    [NWDClassTrigrammeAttribute("SSO")]
    [NWDClassDescriptionAttribute("Server Other descriptions Class")]
    [NWDClassMenuNameAttribute("Server Other")]
    public partial class NWDServerOther : NWDServer
    {
        //-------------------------------------------------------------------------------------------------------------
        const string ServerTypeProperty = "ServerType";
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]

        //[NWDInspectorGroupStart("Server SSH")]
        //public NWDReferenceType<NWDServer> Server { get; set; }
        //[NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Server used for")]
        public NWDServerOtherType ServerType { get; set; }


        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.ActionServer, false)]
        public NWDStringsArrayType ActionServer { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebDAV, false)]
        public string WebDAV_User { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebDAV, false)]
        public NWDSecurePassword WebDAV_Password { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebDAV, false)]
        public string WebDAV_Access { get; set; }



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
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public string WebDomainNameEmail { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public bool WebDomainNameUserInstalled { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public string WebDomainNameFolder { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public string WebDomainNameUser { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public NWDSecurePassword WebDomainNameSecure_Password { get; set; }

        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        [NWDTooltips("Add module : UpdraftPlus , Complianz – GDPR/CCPA Cookie Consent , Contact Form 7, Really Simple SSL, Jetpack ... ")]
        public string WorpressUser { get; set; }
        [NWDIf(ServerTypeProperty, (int)NWDServerOtherType.WebServer, false)]
        public NWDSecurePassword WorpressSecure_Password { get; set; }

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
#endif
