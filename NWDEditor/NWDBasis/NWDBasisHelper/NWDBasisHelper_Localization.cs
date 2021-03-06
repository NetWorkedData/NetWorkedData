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
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void ReOrderLocalizations(List<NWDTypeClass> sDatas)
        {
            NWDBenchmark.Start();
            string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
            string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (NWDTypeClass tObject in sDatas)
            {
                tObject.ReOrderLocalizationsValues(tLanguageArray);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReOrderAllLocalizations()
        {
            NWDBenchmark.Start();
            ReOrderLocalizations(Datas);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExportLocalization(bool sOnlySelection = false)
        {
            NWDBenchmark.Start();
            NWDDataManager.SharedInstance().DataQueueExecute();
            // ask for final file path
            string tPath = EditorUtility.SaveFilePanel(
                "Export Localization CSV",
                string.Empty,
                ClassNamePHP + ".csv",
                "csv");
            if (string.IsNullOrEmpty(tPath) == false)
            {
                // prepare header
                string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" +
                                 NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Replace(";", "\";\"") + "\"\n";
                // start to create file
                string tFile = tHeaders;
                tFile += ExportLocalizationInCSV(sOnlySelection);
                // write file
                File.WriteAllText(tPath, tFile);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string ExportLocalizationInCSV(bool sOnlySelection = false)
        {
            NWDBenchmark.Start();
            string tRows = string.Empty;
            string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
            string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (sOnlySelection == false)
            {
                foreach (NWDTypeClass tObject in Datas)
                {
                    tRows += tObject.ExportCSV(tLanguageArray);
                }
            }
            else
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tObjectSelection in EditorTableDatasSelected)
                {
                    if (tObjectSelection.Value == true)
                    {
                        tRows += tObjectSelection.Key.ExportCSV(tLanguageArray);
                    }
                }
            }
            NWDBenchmark.Finish();
            return tRows;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ImportAllLocalizations(string[] sLanguageArray, string[] sCSVFileArray)
        {
            NWDBenchmark.Start();
            int tI = 0;
            int tCount = sCSVFileArray.Length;
            string tKeys = sCSVFileArray[0];
            string[] tKeysArray = tKeys.Split(new string[] { ";" }, StringSplitOptions.None);
            for (tI = 1; tI < tCount; tI++)
            {
                ImportLocalization(sLanguageArray, tKeysArray, sCSVFileArray[tI]);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ImportLocalization(string[] sLanguageArray, string[] sKeysArray, string sCSVrow)
        {
            NWDBenchmark.Start();
            string[] tValuesArray = sCSVrow.Split(new string[] { ";" }, StringSplitOptions.None);
            Dictionary<string, string> tDico = new Dictionary<string, string>();
            int i = 0;
            for (i = 0; i < tValuesArray.Length; i++)
            {
                string tKey = sKeysArray[i].Trim('"');
                string tValue = tValuesArray[i].Trim('"');
                tDico.Add(tKey, tValue);
            }
            //if (tDico.ContainsKey ("Reference") && tDico.ContainsKey ("PropertyName") && tDico.ContainsKey ("Type")) 
            {
                if (tDico["Type"] == ClassTrigramme)
                {
                    //Debug.Log ("tDico [\"Reference\"] = " + tDico ["Reference"]);
                    NWDTypeClass tData = GetDataByReference(tDico["Reference"]);
                    if (tData != null)
                    {
                        //Debug.Log ("tObject reference " + tObject.Reference + " found ");
                        if (tData.IntegrityIsValid() == true)
                        {
                            List<string> tValueNextList = new List<string>();
                            foreach (string tLang in sLanguageArray)
                            {
                                if (tDico.ContainsKey(tLang))
                                {
                                    string tLangValue = tDico[tLang];
                                    tLangValue = tLangValue.Replace("&#59", ";");
                                    tLangValue = tLangValue.Replace("&#00", "\n");
                                    tLangValue = tLangValue.Replace("&quot;", "\"");
                                    tValueNextList.Add(tLang + NWDConstants.kFieldSeparatorB + tLangValue);
                                }
                            }
                            string[] tNextValueArray = tValueNextList.Distinct().ToArray();
                            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
                            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                            if (tNextValue == NWDConstants.kFieldSeparatorB)
                            {
                                tNextValue = string.Empty;
                            }
                            string tPropertyName = tDico["PropertyName"];

                            PropertyInfo tInfo = tData.GetType().GetProperty(tPropertyName);
                            if (tInfo == null)
                            {
                                Debug.LogError("tPropertyName not exist : " + tPropertyName);
                            }
                            else
                            {
                                if (tInfo.PropertyType.IsSubclassOf(typeof(NWDLocalizableType)))
                                {
                                    Debug.Log("import : " + tDico["Type"] + " " + tPropertyName);
                                    NWDLocalizableType tPropertyValueOld = (NWDLocalizableType)tInfo.GetValue(tData, null);
                                    if (tPropertyValueOld.Value != tNextValue)
                                    {
                                        tPropertyValueOld.Value = tNextValue;
                                        tData.UpdateData(true, NWDWritingMode.ByEditorDefault);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
