//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols(Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
#define NWD_LOG
#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Use to define the statut of launcher
    /// </summary>
    public enum NWDStatut
    {
        Error = -9,

        None = 0,

        EngineStart = 1,
        EngineFinish = 9,

        ClassDeclareStart = 11,
        ClassDeclareStep = 12,
        ClassDeclareFinish = 19,

        ClassRestaureStart = 21,
        ClassRestaureFinish = 22,

        IndexMethodStart = 23,
        IndexMethodFinish = 24,

        EngineReady = 30,

        DataEditorConnectionStart = 31,
        DataEditorConnectionError = 32,
        DataEditorConnectionFinish = 33,

        DataEditorTableCreateStart = 34,
        DataEditorTableCreateStep = 35,
        DataEditorTableCreateFinish = 36,

        DataEditorLoadStart = 37,
        DataEditorLoadStep = 38,
        DataEditorLoadFinish = 39,

        DataEditorIndexStart = 40,
        DataEditorIndexStep = 41,
        DataEditorIndexFinish = 42,

        EditorReady = 50,

        DataAccountConnectionStart = 51,

        DataAccountCodePinCreate = 52,
        DataAccountCodePinRequest = 53,
        DataAccountCodePinFail = 54,
        DataAccountCodePinStop = 55,
        DataAccountCodePinSuccess = 56,

        DataAccountConnectionError = 62,
        DataAccountConnectionFinish = 63,

        DataAccountTableCreateStart = 64,
        DataAccountTableCreateStep = 65,
        DataAccountTableCreateFinish = 66,

        DataAccountLoadStart = 67,
        DataAccountLoadStep = 68,
        DataAccountLoadFinish = 69,

        DataAccountIndexStart = 70,
        DataAccountIndexStep = 71,
        DataAccountIndexFinish = 72,

        AccountReady = 80,

        NetWorkedDataReady = 99,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================