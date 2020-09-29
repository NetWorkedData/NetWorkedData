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
                List<object> tFileDicoArray = new List<object>();
                tFileDicoArray.Add(tFileDico);
                //string tF = NWEMiniJSON.Json.Serialize(tFileDico);
                string tF = NWEMiniJSON.Json.Serialize(tFileDicoArray);
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
                    Dictionary<string, object> tFileDico = null;
                    List<object> tFileDicoArray = null;
                    try
                    {
                        tFileDicoArray = (List<object>)NWEMiniJSON.Json.Deserialize(tFile);
                    }
                    catch (Exception sException)
                    {
                        Debug.LogWarning(sException.ToString());
                    }
                    if (tFileDicoArray != null)
                    {
                        foreach (Dictionary<string, object> tFileDicoTemp in tFileDicoArray)
                        {
                            string tClassKey = tFileDicoTemp[KClassKey] as string;
                            if (tClassKey == ClassNamePHP)
                            {
                                Debug.Log("new import");
                                tFileDico = tFileDicoTemp;
                            }
                        }
                    }
                    else
                    {
                        tFileDico = (Dictionary<string, object>)NWEMiniJSON.Json.Deserialize(tFile);
                    }
                    if (tFileDico == null)
                    {
                        Debug.Log("old import");
                        tFileDico = (Dictionary<string, object>)NWEMiniJSON.Json.Deserialize(tFile);
                    }
                    //Dictionary<string, object> tFileDico = (Dictionary<string, object>)NWEMiniJSON.Json.Deserialize(tFile);
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
        public static void ExportMultiCSV(List<NWDBasis> sDatasList, string sFilename = "MultiDatas")
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
            Dictionary<NWDBasisHelper, List<NWDBasis>> tHelperList = new Dictionary<NWDBasisHelper, List<NWDBasis>>();
            foreach (NWDBasis tdata in sDatasList)
            {
                if (tHelperList.ContainsKey(tdata.BasisHelper()) == false)
                {
                    List<NWDBasis> tList = new List<NWDBasis>();
                    tHelperList.Add(tdata.BasisHelper(), tList);
                }
                if (tHelperList[tdata.BasisHelper()].Contains(tdata) == false)
                {
                    tHelperList[tdata.BasisHelper()].Add(tdata);
                }
            }
            // ask for final file path
            string tPath = EditorUtility.SaveFilePanel(
                "Export Datas",
                string.Empty,
                sFilename + "." + KExportExtension,
                KExportExtension);
            if (string.IsNullOrEmpty(tPath) == false)
            {
                List<object> tFileDicoArray = new List<object>();
                foreach (KeyValuePair<NWDBasisHelper, List<NWDBasis>> tKeyValue in tHelperList)
                {
                    Dictionary<string, object> tFileDico = new Dictionary<string, object>();
                    tFileDico.Add(KClassKey, tKeyValue.Key.ClassNamePHP);
                    tFileDico.Add(KModelKey, tKeyValue.Key.WebModelSQLOrder[tKeyValue.Key.LastWebBuild]);
                    List<string> tFile = new List<string>();
                    foreach (NWDBasis tObjectSelection in tKeyValue.Value)
                    {
                        tFile.Add(tObjectSelection.CSVAssembly());
                    }
                    tFileDico.Add(KDatasKey, tFile);
                    tFileDicoArray.Add(tFileDico);
                }
                //string tF = NWEMiniJSON.Json.Serialize(tFileDico);
                string tF = NWEMiniJSON.Json.Serialize(tFileDicoArray);
                File.WriteAllText(tPath, tF);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ImportMultiCSV()
        {
            NWDBenchmark.Start();
            string tPath = EditorUtility.OpenFilePanel("Import Datas", string.Empty, KExportExtension);
            if (string.IsNullOrEmpty(tPath) == false)
            {
                string tFile = File.ReadAllText(tPath);
                if (tFile != null)
                {
                    List<object> tFileDicoArray = null;
                    try
                    {
                        tFileDicoArray = (List<object>)NWEMiniJSON.Json.Deserialize(tFile);
                    }
                    catch (Exception sException)
                    {
                        Debug.LogWarning(sException.ToString());
                    }
                    if (tFileDicoArray != null)
                    {
                        foreach (Dictionary<string, object> tFileDicoTemp in tFileDicoArray)
                        {
                            string tClassKey = tFileDicoTemp[KClassKey] as string;
                            NWDBasisHelper tBasiHelper = NWDBasisHelper.FindTypeInfos(tClassKey);
                            if (tBasiHelper != null)
                            {
                                Dictionary<string, object> tFileDico = null;
                                tFileDico = tFileDicoTemp;
                                string tB = tFileDico[KModelKey] as string;
                                bool tTryImport = false;
                                if (tBasiHelper.WebModelSQLOrder.ContainsKey(tBasiHelper.LastWebBuild) == true)
                                {
                                    if (tBasiHelper.WebModelSQLOrder[tBasiHelper.LastWebBuild] == tB)
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
                                            string tReference = tBasiHelper.GetReferenceValueFromCSV(tDataArray);
                                            Debug.Log("try import data "+ tBasiHelper.ClassNamePHP + " referenced :" + tReference);
                                            NWDTypeClass tObject = tBasiHelper.GetDataByReference(tReference);
                                            if (tObject == null)
                                            {
                                                tObject = tBasiHelper.NewDataFromWeb(tEnvironment, tDataArray, tReference);
                                            }
                                            else
                                            {
                                                tObject.UpdateDataFromWeb(tEnvironment, tDataArray);
                                            }
                                            tObject.ErrorCheck();
                                        }
                                        EditorUtility.DisplayDialog(tBasiHelper.ClassNamePHP, "Import finished", "Ok");
                                    }
                                    else
                                    {
                                        EditorUtility.DisplayDialog(tBasiHelper.ClassNamePHP, "No valid datas ", "Cancel");
                                    }
                                }
                                else
                                {
                                    EditorUtility.DisplayDialog(tBasiHelper.ClassNamePHP, "Not valid model in file!", "Cancel");
                                }
                            }
                        }
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
