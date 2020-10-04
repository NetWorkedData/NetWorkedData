//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
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
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class MDEConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string Path = "NWDMacroPref.MDE";
        public const float RowHeight = 16.0F;
        public const float ManagementWidth = 200.0F;
        public const string NONE = "NONE";
        //-------------------------------------------------------------------------------------------------------------
        public const string Management = "Management";
        public const string Remove = "remove";
        public const string EnableAll = "enable";
        public const string DisableAll = "disable";
        public const string MacroDefineEditor = "Macro Define Editor";
        public const string Menu = "Window/" + MacroDefineEditor;
        public const string EnumArea = "Macro add by enum";
        public const string BoolArea = "Macro add by boolean";
        public const string ErrorInLoading = "... error in loading ...";
        public const string Reload = "Reload";
        public const string Result = "Result in build settings";
        //public const string NewMacroArea = "Define new macro here";
        public const string NewMacro = "Macro";
        public const string NewMacroButton = "Add Macro";
        public const string SaveArea = "Save configuration and compile if necessairy";
        public const string Save = "Save and compile";
        //-------------------------------------------------------------------------------------------------------------
        public const string AlertTitle = "Warning";
        public const string AlertMessage = "You will save and compile another configuration, are you sure?";
        public const string AlertOK = "Yes, save";
        public const string AlertCancel = "No, cancel";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
