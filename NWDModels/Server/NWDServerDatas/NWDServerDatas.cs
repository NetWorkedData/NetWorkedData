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
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("SSD")]
    [NWDClassDescriptionAttribute("Server Datas descriptions Class")]
    [NWDClassMenuNameAttribute("Server Datas")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDServerDatas : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string RangeEditor = "0000";
        const int kRangeMin = 1000;
        const int kRangeMax = 9990;
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        //public NWDServerContinent Continent { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Data Account Range")]
        [NWDNotEditable]
        public int Range { get; set; }
        [NWDIntSlider(kRangeMin, kRangeMax)]
        public int RangeMin { get; set; }
        [NWDIntSlider(kRangeMin, kRangeMax)]
        public int RangeMax { get; set; }
        public int UserMax { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Authentification MySQL")]
        [NWDEntitled("MySQL IP")]
        public NWDIPType MySQLIP { get; set; }
        [NWDEntitled("MySQL Port")]
        public int MySQLPort { get; set; }
        [NWDEntitled("MySQL User")]
        public string MySQLUser { get; set; }
        [NWDEntitled("MySQL Password (AES)")]
        public NWDSecurePassword MySQLSecurePassword { get; set; }
        [NWDEntitled("MySQL Base")]
        public string MySQLBase { get; set; }
        [NWDEntitled("MySQL Root Password (AES)")]
        public NWDSecurePassword Root_MySQLSecurePassword { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Acces MySQL")]
        public bool External { get; set; }
        [NWDEntitled("PHP My Admin")]
        public bool PhpMyAdmin { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Copy base from ...")]
        public NWDReferenceType<NWDServerDatas> ServerEditorOriginal { get; set; }
        [NWDTooltips("Use for special operation")]
        public NWDReferenceType<NWDServerDatas> ServerAccountOriginal { get; set; }
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