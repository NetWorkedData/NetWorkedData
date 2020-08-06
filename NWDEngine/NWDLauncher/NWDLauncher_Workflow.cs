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
#define NWD_LOG
#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        static private void LaunchEngine()
        {
            if (Launched == false)
            {
                TimeStart = Time.realtimeSinceStartup;
                ActiveBenchmark = NWDAppConfiguration.SharedInstance().LauncherBenchmark;
                StepSum = 0;
                StepIndex = 0;
                NWDBenchmark.Start("Launch");
                Launched = true;
                //NWDToolbox.EditorAndPlaying("NWDLauncher Launch()");
                bool tEditorByPass = false;
                switch (CompileAs())
                {
                    case NWDCompileType.Editor:
                        {
#if UNITY_EDITOR
                            if (ActiveBenchmark)
                            {
                                NWDBenchmark.Log("Launch in editor");
                            }
                            tEditorByPass = true;
#endif
                        }
                        break;
                    case NWDCompileType.PlayMode:
                        {
                            if (ActiveBenchmark)
                            {
                                NWDBenchmark.Log("Launch as playmode");
                            }
                            tEditorByPass = true;
                        }
                        break;
                    case NWDCompileType.Runtime:
                        {
                            tEditorByPass = false;
                        }
                        break;
                }
                if (tEditorByPass == true)
                {
                    Preload = true;
                    LaunchStandard();
                }
                else
                {
                    Preload = NWDAppConfiguration.SharedInstance().PreloadDatas;
                    if (Preload == true)
                    {
                        if (ActiveBenchmark)
                        {
                            NWDBenchmark.Log("Launch in runtime preload (sync)");
                        }
                        LaunchRuntimeSync();
                    }
                    else
                    {
                        if (ActiveBenchmark)
                        {
                            NWDBenchmark.Log("Launch in runtime by NWDGameDataManager.ShareInstance (async)");
                        }
                        //Launch_Runtime_Async(); // waiting order from NWDGameDataManager.ShareInstance()
                    }
                }
                //tSW.Stop();
                //UnityEngine.Debug.Log("STOPWATCH : " + (tSW.ElapsedMilliseconds / 1000.0F).ToString("F3") + " s");
                NWDBenchmark.Finish("Launch");
                if (Preload == true)
                {
                    if (ActiveBenchmark)
                    {
                        LauncherBenchmarkToMarkdown();
                        NWBBenchmarkResult.CurrentData().BenchmarkNow();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================