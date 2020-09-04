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
//=====================================================================================================================
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDInternalDescriptionNotEditable]
    [NWDClassTrigrammeAttribute(NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM)]
    [NWDClassDescriptionAttribute("Account descriptions Class")]
    [NWDClassMenuNameAttribute("Account")]
#if UNITY_EDITOR
    [NWDClassClusterAttribute(1, 2)]
#endif
    public partial class NWDAccount : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to tag account reference as temporary reference, cluster need return a real account reference.
        /// </summary>
        public const string K_ACCOUNT_TEMPORARY_SUFFIXE = "T";
        /// <summary>
        /// Use to tag account reference as certified reference, cluster returned this reference as unique.
        /// </summary>
        public const string K_ACCOUNT_CERTIFIED_SUFFIXE = "C";
        /// <summary>
        /// Use to tag account reference as temporary reference to create a new user, cluster need return a real account reference.
        /// </summary>
        public const string K_ACCOUNT_NEW_SUFFIXE = "Z";
        /// <summary>
        /// Use to tag account reference as fake account reference, cluster use this reference as real reference. It's reserved by editor mode.
        /// </summary>
        public const string K_ACCOUNT_FROM_EDITOR = "E";
        /// <summary>
        /// The trigramme of this class is always fixeddat 'ACC'.
        /// </summary>
        public const string K_ACCOUNT_PREFIX_TRIGRAM = "ACC";
        //-------------------------------------------------------------------------------------------------------------
        const string K_LOGIN_INDEX = "LoginIndex";
        const string K_SECRET_INDEX = "SecretIndex";
        const string K_SOCIAL_INDEX = "SecretIndex";
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(K_LOGIN_INDEX, "AC")]
        [NWDAddIndexed(K_SECRET_INDEX, "AC")]
        [NWDAddIndexed(K_SOCIAL_INDEX, "AC")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(NWD.K_BASIS_INDEX, "AC")]
        [NWDAddIndexed(NWD.K_BASIS_INDEX, "Reference")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Account statut")]
        [NWDTooltips("The statut of this account in process of test (normal and default are 'InGame')")]
        [NWDCertified]
        public NWDAccountEnvironment UseInEnvironment { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Account ban")]
        /// <summary>
        /// Gets or sets a value indicating whether this account <see cref="NWDEditor.NWDAccount"/> is banned.
        /// </summary>
        /// <value><c>true</c> if ban; otherwise, <c>false</c>.</value>
        [NWDTooltips("If account is banned set the unix timestamp of ban's date")]
        [NWDCertified]
        public int Ban { get; set; }
        //[NWDTooltips("If account is froozen in iceberg")]
        //public bool Archived { get; set; }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
