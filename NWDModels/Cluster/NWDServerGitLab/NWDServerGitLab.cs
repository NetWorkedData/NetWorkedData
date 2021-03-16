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
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassMacro("UNITY_EDITOR")]
    [NWDClassTrigrammeAttribute("SSOGLB")]
    [NWDClassDescriptionAttribute("ServerServerGitLab")]
    [NWDClassMenuNameAttribute("Server GitLab")]
    [NWDWindowOwner(typeof(NWDServerWindow))]
    public partial class NWDServerGitLab : NWDServer
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        [NWDInspectorGroupStart("GitLab")]
        public string GitLabDomainNameServer { get; set; }
        public string GitLabEmail { get; set; }
        public string GitLabAWSDirectory { get; set; }
        public NWDSecurePassword GitLabAWSAccessKeyID { get; set; }
        public NWDSecurePassword GitLabAWSSecretAccessKey { get; set; }
        public string GitLabAWSRegion { get; set; }
        public NWDSecurePassword GitLabAWSPassword { get; set; }
        public string GitLabBackupFile { get; set; }
        public int GitLabBackupTimeStamp { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
