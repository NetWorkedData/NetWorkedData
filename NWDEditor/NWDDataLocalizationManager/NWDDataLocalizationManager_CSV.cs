//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataLocalizationManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void ReOrderAllLocalizations()
        {
            NWDBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData Reorder localization";
            float tCountClass = NWDDataManager.SharedInstance().ClassTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Prepare operation", tOperation / tCountClass);
            tOperation++;

            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reorder localization in  " + tType.Name + " objects", tOperation / tCountClass);
                tOperation++;

                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.ReOrderAllLocalizations();
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExportToCSV()
        {
            NWDBenchmark.Start();
            // apply the pending modifications : prevent lost modification
            NWDDataManager.SharedInstance().DataQueueExecute();
            // ask for final file path
            string tPath = EditorUtility.SaveFilePanel(
                "Export Localization CSV",
                string.Empty,
                "NWDDataLocalization.csv",
                "csv");
            if (string.IsNullOrEmpty(tPath) == false)
            {
                // prepare header
                string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" +
                                 NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Replace(";", "\";\"") + "\"\n";
                // start to create file
                string tFile = tHeaders;
                // populate file by class result
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tFile +=  tHelper.ExportLocalizationInCSV();
                }
                // write file
                File.WriteAllText(tPath, tFile);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ImportFromCSV()
        {
            NWDBenchmark.Start();
            string tPath = EditorUtility.OpenFilePanel("Import Localization CSV", string.Empty, "csv");
            if (string.IsNullOrEmpty(tPath) == false)
            {
                string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
                string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                string tFile = File.ReadAllText(tPath);
                string[] tFileRows = tFile.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (tFile != null)
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                    {
                        NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                        tHelper.ImportAllLocalizations(tLanguageArray, tFileRows);
                    }
                }
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif