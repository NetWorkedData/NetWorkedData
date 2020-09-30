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
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        static private void LaunchEngine()
        {
            //Debug.Log("LaunchEngine()");
            if (Launched == false)
            {
                Launched = true;
                TimeStart = Time.realtimeSinceStartup;
                StepSum = 0;
                StepIndex = 0;
                NWDBenchmark.Start("Launch");
                //NWDToolbox.EditorAndPlaying("NWDLauncher Launch()");
                bool tEditorByPass = false;
                switch (CompileAs())
                {
                    case NWDCompileType.Editor:
                        {
#if UNITY_EDITOR
                            NWDBenchmarkLauncher.Log("Launch in editor");
                            tEditorByPass = true;
#endif
                        }
                        break;
                    case NWDCompileType.PlayMode:
                        {
                            NWDBenchmarkLauncher.Log("Launch as playmode");
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
                        NWDBenchmarkLauncher.Log("Launch in runtime preload (sync)");
                        LaunchRuntimeSync();
                    }
                    else
                    {
                        NWDBenchmarkLauncher.Log("Launch in runtime by NWDGameDataManager.ShareInstance (async)");
                    }
                }
                //tSW.Stop();
                //UnityEngine.Debug.Log("STOPWATCH : " + (tSW.ElapsedMilliseconds / 1000.0F).ToString("F3") + " s");
                NWDBenchmark.Finish("Launch");
                if (Preload == true)
                {
#if NWD_LAUNCHER_BENCHMARK
                        LauncherBenchmarkToMarkdown();
                        if (NWBBenchmarkResult.CurrentData() != null)
                        {
                            NWBBenchmarkResult.CurrentData().BenchmarkNow();
                        }
#endif
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
