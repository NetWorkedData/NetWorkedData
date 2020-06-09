//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:35
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataLocalizationManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void ReOrderAllLocalizations()
        {
            NWEBenchmark.Start();
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
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ReOrderAllLocalizations);
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExportToCSV()
        {
            NWEBenchmark.Start();
            // apply the pending modification : prevent lost modification
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
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ImportFromCSV()
        {
            NWEBenchmark.Start();
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
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif