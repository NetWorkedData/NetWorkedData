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
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        const string KClassKey = "kClassName";
        const string KModelKey = "kModelSQL";
        const string KDatasKey = "kDatas";
        const string KExportExtension = "nwd";
        //-------------------------------------------------------------------------------------------------------------
        public void ExportCSV(bool sOnlySelection)
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
            // ask for final file path
            string tPath = EditorUtility.SaveFilePanel(
                "Export Datas",
                string.Empty,
                ClassNamePHP + "." + KExportExtension,
                KExportExtension);
            if (string.IsNullOrEmpty(tPath) == false)
            {
                Dictionary<string, object> tFileDico = new Dictionary<string, object>();
                tFileDico.Add(KClassKey, ClassNamePHP);
                tFileDico.Add(KModelKey, WebModelSQLOrder[LastWebBuild]);
                List<string> tFile = new List<string>();
                if (sOnlySelection == false)
                {
                    foreach (NWDTypeClass tObject in Datas)
                    {
                        tFile.Add(tObject.CSVAssembly());
                    }
                }
                else
                {
                    foreach (KeyValuePair<NWDTypeClass, bool> tObjectSelection in EditorTableDatasSelected)
                    {
                        if (tObjectSelection.Value == true)
                        {
                            tFile.Add(tObjectSelection.Key.CSVAssembly());
                        }
                    }
                }
                tFileDico.Add(KDatasKey, tFile);
                string tF = NWEMiniJSON.Json.Serialize(tFileDico);
                File.WriteAllText(tPath, tF);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ImportCSV()
        {
            NWDBenchmark.Start();
            string tPath = EditorUtility.OpenFilePanel("Import Datas", string.Empty, KExportExtension);
            if (string.IsNullOrEmpty(tPath) == false)
            {
                string tFile = File.ReadAllText(tPath);
                if (tFile != null)
                {
                    Dictionary<string, object> tFileDico = (Dictionary<string, object>)NWEMiniJSON.Json.Deserialize(tFile);
                    string tA = tFileDico[KClassKey] as string;
                    if (ClassNamePHP == tA)
                    {
                        string tB = tFileDico[KModelKey] as string;
                        bool tTryImport = false;
                        if (WebModelSQLOrder.ContainsKey(LastWebBuild) == true)
                        {
                            if (WebModelSQLOrder[LastWebBuild] == tB)
                            {
                                tTryImport = true;
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            tTryImport = true;
                        }
                        if (tTryImport)
                        {
                            // ok
                            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                            List<object> tDatas = tFileDico[KDatasKey] as List<object>;
                            //UpdateDataFromWeb(NWDAppEnvironment sEnvironment, string[] sDataArray, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal);
                            if (tDatas != null)
                            {
                                Debug.Log("try import tDatas count :" + tDatas.Count);
                                foreach (object tDataObject in tDatas)
                                {
                                    string tData = tDataObject as string;
                                    string[] tDataArray = tData.Split(NWDConstants.kStandardSeparator.ToCharArray());
                                    for (int tI = 0; tI < tDataArray.Length; tI++)
                                    {
                                        tDataArray[tI] = NWDToolbox.TextCSVUnprotect(tDataArray[tI]);
                                    }
                                    string tReference = GetReferenceValueFromCSV(tDataArray);
                                    Debug.Log("try import data referenced :" + tReference);
                                    NWDTypeClass tObject = GetDataByReference(tReference);
                                    if (tObject == null)
                                    {
                                        tObject = NewDataFromWeb(tEnvironment, tDataArray, tReference);
                                    }
                                    else
                                    {
                                        tObject.UpdateDataFromWeb(tEnvironment, tDataArray);
                                    }
                                    tObject.ErrorCheck();
                                }
                                EditorUtility.DisplayDialog(ClassNamePHP, "Import finished", "Ok");
                            }
                            else
                            {
                                EditorUtility.DisplayDialog(ClassNamePHP, "No valid datas ", "Cancel");
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog(ClassNamePHP, "Not valid model in file!", "Cancel");
                        }
                    }
                    else
                    {
                        EditorUtility.DisplayDialog(ClassNamePHP, "Not valid class in file!", "Cancel");
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