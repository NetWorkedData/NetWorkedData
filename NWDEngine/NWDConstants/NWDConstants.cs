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

using System.Globalization;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWD
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string InspectorBasisHeader = "Informations";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PlayerLanguageKey = "PlayerLanguageKey";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_DeviceDatabaseName = "PRP_Device.prp";
        public const string K_EditorDatabaseName = "PRP_EditorDB.prp";
        public const string K_BuildDatabaseName = "PRP_EditorDB.pbp";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_StreamingAssets = "StreamingAssets";
        public const string K_Assets = "Assets";
        public const string K_Resources = "Resources";
        //-------------------------------------------------------------------------------------------------------------
        //public const string K_DevEnvironment = "Dev";
        //public const string K_PreprodEnvironment = "Preprod";
        //public const string K_ProdEnvironment = "Prod";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_ReturnLine = "\n";
        public static string K_CommentSeparator = "//--------------------";
        public static string K_CommentAutogenerate = "// NWD Autogenerate File at ";
        public static string K_CommentCopyright = "// Copyright NetWorkedData ideMobi ";
        public static string K_CommentCreator = "// NWD Autogenerate script created by Jean-François CONTART ";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_LOADER = "Loader";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public static CultureInfo FormatCountry = CultureInfo.InvariantCulture;
        //-------------------------------------------------------------------------------------------------------------
        public static string FloatFormat = "F5";
        public static string FloatSQLFormat = "5";
        public static string DoubleFormat = "F5";
        public static string DoubleSQLFormat = "5";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_DEVELOPMENT_NAME = "Dev";
        public static string K_PREPRODUCTION_NAME = "Preprod";
        public static string K_PRODUCTION_NAME = "Prod";
        //-------------------------------------------------------------------------------------------------------------
        public static string kStandardSeparator = "|";
        public static string kStandardSeparatorSubstitute = "@0#";
        //-------------------------------------------------------------------------------------------------------------
        //public static string kFieldNone = "<color=gray>none</color>";
        public static string kFieldNone = "none";
        public static string kFieldEmpty = "empty";
        public static string kFieldNotEmpty = "not empty";
        public static char kFieldSeparatorA_char = '•';
        public static char kFieldSeparatorB_char = ':';
        public static char kFieldSeparatorC_char = '_';
        public static char kFieldSeparatorD_char = '∆';
        public static char kFieldSeparatorE_char = '∂';
        public static char kFieldSeparatorF_char = ';';

        public static string kFieldSeparatorA = string.Empty + kFieldSeparatorA_char;
        public static string kFieldSeparatorB = string.Empty + kFieldSeparatorB_char;
        public static string kFieldSeparatorC = string.Empty + kFieldSeparatorC_char;
        public static string kFieldSeparatorD = string.Empty + kFieldSeparatorD_char;
        public static string kFieldSeparatorE = string.Empty + kFieldSeparatorE_char;
        public static string kFieldSeparatorF = string.Empty + kFieldSeparatorF_char;   

        public static string kFieldSeparatorASubstitute = "@1#";
        public static string kFieldSeparatorBSubstitute = "@2#";
        public static string kFieldSeparatorCSubstitute = "@3#";
        public static string kFieldSeparatorDSubstitute = "@4#";
        public static string kFieldSeparatorESubstitute = "@5#";
        //-------------------------------------------------------------------------------------------------------------
        static public string kPrefSaltValidKey = "SaltValid";
        static public string kPrefSaltAKey = "SaltA";
        static public string kPrefSaltBKey = "SaltB";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
