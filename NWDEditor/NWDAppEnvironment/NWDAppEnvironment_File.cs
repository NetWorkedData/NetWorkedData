// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:20
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif