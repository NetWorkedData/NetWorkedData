//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:21:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using SQLite4Unity3d;
using System.IO;
//using BasicToolBox;
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
                ClassNamePHP + "."+ KExportExtension,
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
            NWEBenchmark.Start();
            string tPath = EditorUtility.OpenFilePanel("Import Datas", string.Empty, KExportExtension);
            if (string.IsNullOrEmpty(tPath) == false)
            {
                string tFile = File.ReadAllText(tPath);
                if (tFile != null)
                {
                    Dictionary<string, object> tFileDico = (Dictionary<string, object>) NWEMiniJSON.Json.Deserialize(tFile);
                    string tA = tFileDico[KClassKey] as string;
                    if (ClassNamePHP == tA)
                    {
                        string tB = tFileDico[KModelKey] as string;
                        if (WebModelSQLOrder[LastWebBuild] == tB)
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
                        EditorUtility.DisplayDialog(ClassNamePHP, "Not valid class in file!","Cancel");
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