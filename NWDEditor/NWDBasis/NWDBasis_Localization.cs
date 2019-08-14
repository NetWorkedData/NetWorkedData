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
using BasicToolBox;
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
            //BTBBenchmark.Start();
            if (TestIntegrity() == true)
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports localizations in csv for this object.
        /// </summary>
        /// <returns>The csv.</returns>
        /// <param name="sLanguageArray">S language array.</param>
        public override string ExportCSV(string[] sLanguageArray)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return tRows;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif