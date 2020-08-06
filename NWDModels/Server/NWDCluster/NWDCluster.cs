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

using System;
using UnityEngine;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDClusterHelper : NWDHelper<NWDCluster>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDCluster class. This class is use for (complete description here).
    /// </summary>
    [NWDClassTrigrammeAttribute("CLS")]
    [NWDClassDescriptionAttribute("Cluster")]
    [NWDClassMenuNameAttribute("Cluster")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDCluster : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]

        [NWDInspectorGroupStart("Servers")]
        [NWDNotEditable]
        public NWDReferencesListType<NWDServerDomain> Domains { get; set; }
        public NWDReferencesListType<NWDServerServices> Services { get; set; }
        public NWDReferencesListType<NWDServerDatas> DataBases { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Cluster Environment Actif")]
        public bool Dev { get; set; }
        public bool Preprod { get; set; }
        public bool Prod { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("For Editor")]
        public NWDSecurePassword AdminKey { get; set; }
        public NWDSecurePassword SaltServer { get; set; }
        [NWDNotEditable]
        public bool LimitEditorByIP { get; set; }
        [NWDNotEditable]
        public NWDIPType EditorIP { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Email services")]
        public string MailFrom { get; set; }
        public string RescueEmail { get; set; }
        public bool MailBySMTP { get; set; }
        [NWDIf("MailBySMTP", true)]
        public string MailHost { get; set; }
        [NWDIf("MailBySMTP", true)]
        public bool MailSSL { get; set; }
        [NWDIf("MailBySMTP", true)]
        public int MailPort { get; set; }
        [NWDIf("MailBySMTP", true)]
        public bool MailAuth { get; set; }
        [NWDIf("MailBySMTP", true)]
        [NWDIf("MailAuth", true)]
        public string MailUserName { get; set; }
        [NWDIf("MailBySMTP", true)]
        [NWDIf("MailAuth", true)]
        public NWDSecurePassword MailPassword { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Resume")]
        [NWDNotEditable]
        [NWDVeryLongString]
        public string Information { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        public NWDCluster()
        {
            //Debug.Log("NWDCluster Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCluster(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCluster Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================