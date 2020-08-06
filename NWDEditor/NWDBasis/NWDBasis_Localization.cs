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
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reorder all localizations in object for all properties (clean the string value).
        /// </summary>
        /// <param name="sLanguageArray">languages array to use.</param>
        public override void ReOrderLocalizationsValues(string[] sLanguageArray)
        {
            //NWDBenchmark.Start();
            if (IntegrityIsValid() == true)
            {
                bool tUpdate = false;
                //				string tRows = "";
                Type tType = ClassType();
                List<string> tPropertiesList = BasisHelper().PropertiesOrderArray();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports localizations in csv for this object.
        /// </summary>
        /// <returns>The csv.</returns>
        /// <param name="sLanguageArray">S language array.</param>
        public override string ExportCSV(string[] sLanguageArray)
        {
            //NWDBenchmark.Start();
            string tRows = string.Empty;
            Type tType = ClassType();
            List<string> tPropertiesList = BasisHelper().PropertiesOrderArray();
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                Type tTypeOfThis = tProp.PropertyType;
                //TO-DO : (FUTUR ADDS) Insert new NWDxxxxType Localizable 
                if (tTypeOfThis.IsSubclassOf(typeof(NWDLocalizableType)))
                {
                    NWDLocalizableType tValueObject = (NWDLocalizableType)tProp.GetValue(this, null);
                    string tValue = tValueObject.Value;
                    Dictionary<string, string> tResultSplitDico = new Dictionary<string, string>(new StringIndexKeyComparer());

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
            //NWDBenchmark.Finish();
            return tRows;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif