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
#endif
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public void WriteFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas)
        {
            //NWEBenchmark.Start();
            foreach (string tFolder in sFolders)
            {
                if (string.IsNullOrEmpty(tFolder) == false)
                {
                    Directory.CreateDirectory(tFolder);
                }
            }
            foreach (KeyValuePair<string, string> tFileAndData in sFilesAndDatas)
            {
                File.WriteAllText(tFileAndData.Key, tFileAndData.Value);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif