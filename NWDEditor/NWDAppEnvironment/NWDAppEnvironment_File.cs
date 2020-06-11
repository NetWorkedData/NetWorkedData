//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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