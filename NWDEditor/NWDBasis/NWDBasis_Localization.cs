//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
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
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reorder all localizations in all object for all properties (clean the string value).
        /// </summary>
        //[NWDAliasMethod(NWDConstants.M_ReOrderAllLocalizations)]
        //public static void ReOrderAllLocalizations()
        //{
        //    string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
        //    string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (NWDBasis<K> tObject in NWDBasis<K>.BasisHelper().Datas)
        //    {
        //        tObject.ReOrderLocalizationsValues(tLanguageArray);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reorder all localizations in object for all properties (clean the string value).
        /// </summary>
        /// <param name="sLanguageArray">languages array to use.</param>
        public override void ReOrderLocalizationsValues(string[] sLanguageArray)
        {
            if (TestIntegrity() == true)
            {
                bool tUpdate = false;
                //				string tRows = "";
                Type tType = ClassType();
                List<string> tPropertiesList = PropertiesOrderArray();
                foreach (string tPropertieName in tPropertiesList)
                {
                    PropertyInfo tProp = tType.GetProperty(tPropertieName);
                    Type tTypeOfThis = tProp.PropertyType;

                    //TO-DO : (FUTUR ADDS) Insert new NWDxxxxType Localizable
                    if (tTypeOfThis.IsSubclassOf(typeof(NWDLocalizableType)))
                    {
                        NWDLocalizableType tValueStruct = (NWDLocalizableType)tProp.GetValue(this, null);
                        if (tValueStruct.ReOrder(sLanguageArray))
                        {
                            tUpdate = true;
                        }
                    }
                }
                if (tUpdate == true)
                {
                    UpdateData();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the localization.
        /// </summary>
        //public static void ExportLocalization()
        //{
        //    //Debug.Log ("ExportThisLocalization");
        //    NWDDataManager.SharedInstance().DataQueueExecute();
        //    // ask for final file path
        //    string tPath = EditorUtility.SaveFilePanel(
        //        "Export Localization CSV",
        //        string.Empty,
        //        BasisHelper().ClassNamePHP + ".csv",
        //        "csv");
        //    if (tPath != null)
        //    {
        //        // prepare header
        //        string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" +
        //                         NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Replace(";", "\";\"") + "\"\n";
        //        // start to create file
        //        string tFile = tHeaders;tFile += ExportLocalizationInCSV();
        //        // write file
        //        File.WriteAllText(tPath, tFile);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports all localization in CVS string.
        /// </summary>
        /// <returns>The all localization.</returns>
        //[NWDAliasMethod(NWDConstants.M_ExportLocalizationInCSV)]
        //public static string ExportLocalizationInCSV()
        //{
        //    string tRows = string.Empty;
        //    string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
        //    string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (NWDBasis<K> tObject in NWDBasis<K>.BasisHelper().Datas)
        //    {
        //        tRows += tObject.ExportCSV(tLanguageArray);
        //    }
        //    return tRows;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports localizations in csv for this object.
        /// </summary>
        /// <returns>The csv.</returns>
        /// <param name="sLanguageArray">S language array.</param>
        public override string ExportCSV(string[] sLanguageArray)
        {
            string tRows = string.Empty;
            Type tType = ClassType();
            List<string> tPropertiesList = PropertiesOrderArray();
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                Type tTypeOfThis = tProp.PropertyType;
                //TO-DO : (FUTUR ADDS) Insert new NWDxxxxType Localizable 
                if (tTypeOfThis.IsSubclassOf(typeof(NWDLocalizableType)))
                {
                    NWDLocalizableType tValueObject = (NWDLocalizableType)tProp.GetValue(this, null);
                    string tValue = tValueObject.Value;
                    Dictionary<string, string> tResultSplitDico = new Dictionary<string, string>();

                    if (tValue != null && tValue != string.Empty && tValue != NWDDataLocalizationManager.kBaseDev + NWDConstants.kFieldSeparatorB)
                    {
                        string[] tValueArray = tValue.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string tValueArrayLine in tValueArray)
                        {
                            string[] tLineValue = tValueArrayLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                            if (tLineValue.Length == 2)
                            {
                                string tLangague = tLineValue[0];
                                string tText = tLineValue[1];
                                if (tResultSplitDico.ContainsKey(tLangague) == false)
                                {
                                    tResultSplitDico.Add(tLangague, tText);
                                }
                            }
                        }
                        tRows += "\"" + BasisHelper().ClassTrigramme + "\";\"" + Reference + "\";\"" + InternalKey + "\";\"" + InternalDescription + "\";\"" + tPropertieName + "\";";
                        foreach (string tLang in sLanguageArray)
                        {
                            if (tResultSplitDico.ContainsKey(tLang) == true)
                            {
                                string tValueToWrite = tResultSplitDico[tLang];
                                tValueToWrite = tValueToWrite.Replace("\"", "&quot;");
                                tValueToWrite = tValueToWrite.Replace(";", "&#59");
                                tValueToWrite = tValueToWrite.Replace("\r\n", "\n");
                                tValueToWrite = tValueToWrite.Replace("\n\r", "\n");
                                tValueToWrite = tValueToWrite.Replace("\r", "\n");
                                tValueToWrite = tValueToWrite.Replace("\n", "&#00");

                                tRows += "\"" + tValueToWrite + "\"";
                            }
                            tRows += ";";
                        }
                        tRows += "\n";
                    }
                }
            }
            return tRows;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Imports all localizations.
        /// </summary>
        /// <param name="sLanguageArray">S language array.</param>
        /// <param name="sCSVFileArray">S CSVF ile array.</param>
        //[NWDAliasMethod(NWDConstants.M_ImportAllLocalizations)]
        //public static void ImportAllLocalizations(string[] sLanguageArray, string[] sCSVFileArray)
        //{
        //    //Debug.Log ("ImportAllLocalizations");
        //    //string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" + 
        //    int tI = 0;
        //    int tCount = sCSVFileArray.Length;
        //    string tKeys = sCSVFileArray[0];
        //    //Debug.Log ("ImportAllLocalizations tKeys = " + tKeys);
        //    string[] tKeysArray = tKeys.Split(new string[] { ";" }, StringSplitOptions.None);

        //    //Debug.Log ("ImportAllLocalizations tCount = " + tCount);
        //    for (tI = 1; tI < tCount; tI++)
        //    {
        //        NWDBasis<K>.ImportLocalization(sLanguageArray, tKeysArray, sCSVFileArray[tI]);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Imports the localization CVS rows.
        /// </summary>
        /// <param name="sLanguageArray">S language array.</param>
        /// <param name="sKeysArray">S keys array.</param>
        /// <param name="sCSVrow">S CSV row.</param>
        //public static void ImportLocalization(string[] sLanguageArray, string[] sKeysArray, string sCSVrow)
        //{
        //    //Debug.Log ("sCSVrow = " + sCSVrow);
        //    //string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" + 
        //    string[] tValuesArray = sCSVrow.Split(new string[] { ";" }, StringSplitOptions.None);
        //    Dictionary<string, string> tDico = new Dictionary<string, string>();
        //    int i = 0;
        //    for (i = 0; i < tValuesArray.Length; i++)
        //    {
        //        string tKey = sKeysArray[i].Trim('"');
        //        string tValue = tValuesArray[i].Trim('"');
        //        //Debug.Log ("dico : " + tKey + " =  " + tValue);
        //        tDico.Add(tKey, tValue);
        //    }
        //    //if (tDico.ContainsKey ("Reference") && tDico.ContainsKey ("PropertyName") && tDico.ContainsKey ("Type")) 
        //    {
        //        if (tDico["Type"] == BasisHelper().ClassTrigramme)
        //        {
        //            //Debug.Log ("tDico [\"Reference\"] = " + tDico ["Reference"]);
        //            NWDBasis<K> tObject = NWDBasis<K>.GetDataByReference(tDico["Reference"]);
        //            if (tObject != null)
        //            {
        //                //Debug.Log ("tObject reference " + tObject.Reference + " found ");
        //                if (tObject.TestIntegrity() == true)
        //                {
        //                    List<string> tValueNextList = new List<string>();
        //                    foreach (string tLang in sLanguageArray)
        //                    {
        //                        if (tDico.ContainsKey(tLang))
        //                        {
        //                            string tLangValue = tDico[tLang];
        //                            tLangValue = tLangValue.Replace("&#59", ";");
        //                            tLangValue = tLangValue.Replace("&#00", "\n");
        //                            tLangValue = tLangValue.Replace("&quot;", "\"");
        //                            tValueNextList.Add(tLang + NWDConstants.kFieldSeparatorB + tLangValue);
        //                        }
        //                    }
        //                    string[] tNextValueArray = tValueNextList.Distinct().ToArray();
        //                    string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        //                    tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
        //                    if (tNextValue == NWDConstants.kFieldSeparatorB)
        //                    {
        //                        tNextValue = string.Empty;
        //                    }
        //                    string tPropertyName = tDico["PropertyName"];

        //                    PropertyInfo tInfo = tObject.GetType().GetProperty(tPropertyName);
        //                    if (tInfo == null)
        //                    {
        //                        Debug.LogError("tPropertyName not exist : " + tPropertyName);
        //                    }
        //                    else
        //                    {
        //                        if (tInfo.PropertyType.IsSubclassOf(typeof(NWDLocalizableType)))
        //                        {
        //                            Debug.Log("import : " + tDico["Type"] + " "+ tPropertyName);
        //                            NWDLocalizableType tPropertyValueOld = (NWDLocalizableType)tInfo.GetValue(tObject, null);
        //                            if (tPropertyValueOld.Value != tNextValue)
        //                            {
        //                                tPropertyValueOld.Value = tNextValue;
        //                                tObject.UpdateData(true, NWDWritingMode.ByEditorDefault);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    NWDDataManager.SharedInstance().DataQueueExecute();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif