//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using UnityEngine;

using BasicToolBox;
using System.Text;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string GetReferenceValueFromCSV(string[] sDataArray)
        {
            return sDataArray[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetDMValueFromCSV(string[] sDataArray)
        {
            int rReturn = 0;
            int.TryParse(sDataArray[1], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetIntegrityValueFromCSV(string[] sDataArray)
        {
            return sDataArray[sDataArray.Count() - 1];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int GetWebModelValueFromCSV(string[] sDataArray)
        {
            int rReturn = -1;
            int.TryParse(sDataArray[sDataArray.Count() - 2], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool TestIntegrityValueFromCSV(string[] sDataArray)
        {
            bool rReturn = true;
            string tActualIntegrity = GetIntegrityValueFromCSV(sDataArray);
            StringBuilder tAssembly = new StringBuilder();
            tAssembly.Append(sDataArray[0] + sDataArray[1]);
            int tMax = sDataArray.Count() - 1;
            for (int i = 6; i < tMax; i++)
            {
                tAssembly.Append(sDataArray[i]);
            }
            string tCalculateIntegrity = HashSum(BasisHelper().SaltStart + tAssembly.ToString() + BasisHelper().SaltEnd);
            if (tActualIntegrity != tCalculateIntegrity)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<string> PropertiesOrderArray(int sWebBuilt = -1)
        {
            bool tRecalculate = true;
            List<string> rReturnList = null;
            int tWebBuilt = sWebBuilt;
            int tWebModel = sWebBuilt;

            if (tWebBuilt == -1)
            {
                tWebBuilt = NWDAppConfiguration.SharedInstance().WebBuild;
            }

            if (BasisHelper().WebServiceWebModel.ContainsKey(tWebBuilt))
            {
                tWebModel = BasisHelper().WebServiceWebModel[tWebBuilt];
            }
            else
            {
                // tWebBuilt is unknow ... no webmodel !?
            }
            if (BasisHelper().WebModelPropertiesOrder.ContainsKey(tWebModel))
            {
                tRecalculate = false;
                rReturnList = BasisHelper().WebModelPropertiesOrder[tWebModel];
            }
#if UNITY_EDITOR
            if (sWebBuilt == -1)
            {
                tRecalculate = true;
            }
#endif
            if (tRecalculate == true)
            {
                rReturnList = new List<string>();
                Type tType = ClassType();
                foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    rReturnList.Add(tProp.Name);
                }
                //rReturnList.Sort();
                rReturnList.Sort((tA, tB) => string.Compare(tA, tB, StringComparison.OrdinalIgnoreCase));

                // Reorder to prevent remove correctly
                rReturnList.Remove("Integrity");
                rReturnList.Remove("Reference");
                rReturnList.Remove("ID");
                rReturnList.Remove("DM");
                rReturnList.Remove("DS");
                rReturnList.Remove("ServerHash");
                rReturnList.Remove("ServerLog");
                rReturnList.Remove("DevSync");
                rReturnList.Remove("PreprodSync");
                rReturnList.Remove("ProdSync");
                rReturnList.Remove("InError");
                rReturnList.Remove("WebModel");

                // not include in integrity
                //rReturn.Remove("WebServiceVersion");
                // add the good order for this element
                rReturnList.Insert(0, "Reference");
                rReturnList.Insert(1, "DM");
                rReturnList.Insert(2, "DS");
                rReturnList.Insert(3, "DevSync");
                rReturnList.Insert(4, "PreprodSync");
                rReturnList.Insert(5, "ProdSync");
                //rReturnList.Insert(6, "ServerHash");
                //rReturnList.Insert(7, "ServerLog");
                //rReturnList.Insert(8, "InError");
                //rReturnList.Insert(9, "ID");

                //rReturnList.Add("ID");
                rReturnList.Add("WebModel");
                rReturnList.Add("Integrity");
            }
#if UNITY_EDITOR
            // reinit this table of value if not init  
            if (BasisHelper().WebModelPropertiesOrder.Count == 0)
            {
                BasisHelper().WebModelPropertiesOrder.Add(0, rReturnList);
                BasisHelper().WebServiceWebModel.Add(0, 0);
                BasisHelper().WebModelSQLOrder.Add(0, SLQSelect(0));
            }
#endif
            return rReturnList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CSV_IndexOf(string sPropertyName, int sWebBuilt = -1)
        {
            //return Array.IndexOf(CSVAssemblyOrderArray(sWebBuilt), sPropertyName);
            return PropertiesOrderArray(sWebBuilt).IndexOf(sPropertyName);
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public static string[] CSVAssemblyOrderArray(int sWebBuilt = -1)
        //        {
        //            bool tRecalculate = true;
        //            string[] rReturn = null;
        //            int tWebModel = sWebBuilt;
        //            if (tWebModel == -1)
        //            {
        //                tWebModel = Datas().WebServiceWebModel[NWDAppConfiguration.SharedInstance().WebBuild];
        //            }
        //            else
        //            {
        //                if (Datas().WebServiceWebModel.ContainsKey(sWebBuilt))
        //                {
        //                    tWebModel = Datas().WebServiceWebModel[sWebBuilt];
        //                }
        //            }
        //            if (Datas().CSV_OrderArray.ContainsKey(tWebModel))
        //            {
        //                tRecalculate = false;
        //                rReturn = Datas().CSV_OrderArray[tWebModel];
        //            }
        //#if UNITY_EDITOR
        //            if (sWebBuilt == -1)
        //            {
        //                tRecalculate = true;
        //            }
        //            //#endif
        //            if (tRecalculate == true)
        //            {
        //                List<string> rReturnList = new List<string>();
        //                rReturnList.AddRange(PropertiesOrderArray(sWebBuilt));
        //                rReturnList.Remove("ID");
        //                rReturnList.Remove("ServerHash");
        //                rReturnList.Remove("ServerLog");
        //                rReturnList.Remove("InError");
        //                rReturn = rReturnList.ToArray();
        //            }
        //            //#if UNITY_EDITOR
        //            // reinit this table of value if not init  
        //            if (Datas().CSV_OrderArray.Count == 0)
        //            {
        //                Datas().CSV_OrderArray.Add(0, rReturn);
        //            }
        //#endif
        //return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static string[] SLQAssemblyOrderArray(int sWebBuilt = -1) //  for insert of $sCsvList
        {
            string[] rReturn = null;
            List<string> rReturnList = new List<string>();
            rReturnList.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturnList.Remove("Reference");
            rReturn = rReturnList.ToArray();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SLQSelect(int sWebBuilt = -1)
        {
            string rReturnString = string.Empty;
            List<string> tList = new List<string>();
            tList.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturnString = "***";
            foreach (string tPropertyName in tList)
            {
                PropertyInfo tPropertyInfo = ClassType().GetProperty(tPropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (tPropertyInfo != null)
                {
                    Type tTypeOfThis = tPropertyInfo.PropertyType;
                    if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(long))
                    {
                        rReturnString += ", REPLACE(`" + tPropertyName + "`,\",\",\"\") as `" + tPropertyName + "`";
                    }
                    else if (tTypeOfThis == typeof(float))
                    {
                        rReturnString += ", REPLACE(FORMAT(`" + tPropertyName + "`," + NWDConstants.FloatSQLFormat + "),\",\",\"\") as `" + tPropertyName + "`";
                    }
                    else if (tTypeOfThis == typeof(double))
                    {
                        rReturnString += ", REPLACE(FORMAT(`" + tPropertyName + "`," + NWDConstants.DoubleSQLFormat + "),\",\",\"\") as `" + tPropertyName + "`";
                    }
                    else
                    {
                        rReturnString += ", `" + tPropertyName + "`";
                    }
                }
            }
            rReturnString = rReturnString.Replace("***, ", "");
            return rReturnString;
        }

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> SLQIntegrityOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturn.Remove("Integrity");
            rReturn.Remove("DS");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            return rReturn;
        }

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> SLQIntegrityServerOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturn.Remove("Integrity");
            rReturn.Remove("DS");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            rReturn.Remove("DM");
            rReturn.Remove("DD");
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        //public static List<string> DataAssemblyPropertiesList(int sWebBuilt = -1)
        //{
        //        List<string> rReturn = new List<string>();
        //        rReturn.AddRange(PropertiesOrderArray(sWebBuilt));
        //        rReturn.Remove("Integrity"); // not include in integrity
        //        rReturn.Remove("Reference");
        //        rReturn.Remove("ID");
        //        rReturn.Remove("DM");
        //        rReturn.Remove("DS");// not include in integrity
        //        rReturn.Remove("ServerHash");// not include in integrity
        //        rReturn.Remove("ServerLog");// not include in integrity
        //        rReturn.Remove("DevSync");// not include in integrity
        //        rReturn.Remove("PreprodSync");// not include in integrity
        //        rReturn.Remove("ProdSync");// not include in integrity
        //        rReturn.Remove("ProdSync");// not include in integrity

        //        // to prevent integrity error in check InError
        //        rReturn.Remove("InError");// not include in integrity
        //        //rReturn.Remove("WebModel");
        //        //rReturn.Add("WebModel");
        //    return rReturn;
        //}

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New instance from CSV.
        /// </summary>
        /// <returns>The instance from CS.</returns>
        /// <param name="sEnvironment">S environment.</param>
        /// <param name="sDataArray">S data array.</param>
        private static NWDBasis<K> NewDataFromWeb(NWDAppEnvironment sEnvironment,
                                                  string[] sDataArray,
                                                  string sReference,
                                                  NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            // 
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            //Debug.Log("NewDataFromWeb ()");
            NWDBasis<K> rReturnObject = null;
            //rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType());
            rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType(), new object[] { false });
            rReturnObject.InstanceInit();
            rReturnObject.Reference = sReference;

            rReturnObject.FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            BasisHelper().AddData(rReturnObject);

            // Verif if Systeme can use the thread (option in Environment)
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
            // We can only insert free object
            if (rReturnObject.WritingState == NWDWritingState.Free)
            {
                switch (rReturnObject.WritingState)
                {
                    case NWDWritingState.Free:
                        // ok you can change the mode
                        break;
                    case NWDWritingState.MainThread:
                        // strange ... not possible.
                        break;
                    case NWDWritingState.MainThreadInQueue:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.QueuedMainThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                    case NWDWritingState.PoolThread:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.PoolThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                    case NWDWritingState.PoolThreadInQueue:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.QueuedPoolThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                }
                bool tDoInsert = true;
                switch (sWritingMode)
                {
                    case NWDWritingMode.MainThread:
                        break;
                    case NWDWritingMode.QueuedMainThread:
                        if (NWDDataManager.SharedInstance().kInsertDataQueueMain.Contains(rReturnObject))
                        {
                            tDoInsert = false;
                        }
                        break;
                    case NWDWritingMode.PoolThread:
                        break;
                    case NWDWritingMode.QueuedPoolThread:
                        if (NWDDataManager.SharedInstance().kInsertDataQueuePool.Contains(rReturnObject))
                        {
                            tDoInsert = false;
                        }
                        break;
                }
                if (tDoInsert == true)
                {
                    rReturnObject.WritingLockAdd();
                    rReturnObject.WritingPending = NWDWritingPending.InsertInMemory;
                    NWDDataManager.SharedInstance().InsertData(rReturnObject, sWritingMode);
                }
            }
            //BTBBenchmark.Finish();
            //Data waiting for queue to finish the process
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataFromWeb(NWDAppEnvironment sEnvironment,
                                      string[] sDataArray,
                                      NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            // Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            // Force update with CVS value

            FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            BasisHelper().UpdateData(this);
            //Data waiting for queue to finish the process
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
            switch (WritingState)
            {
                case NWDWritingState.Free:
                    // ok you can change the mode
                    break;
                case NWDWritingState.MainThread:
                    // strange ... not possible.
                    break;
                case NWDWritingState.MainThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThread:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.PoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedPoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
            }
            bool tDoUpdate = true;
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    break;
                case NWDWritingMode.QueuedMainThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueueMain.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueuePool.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
            }
            if (tDoUpdate == true)
            {
                WritingLockAdd();
                WritingPending = NWDWritingPending.UpdateInMemory;
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Updates the with CSV.
        /// </summary>
        /// <param name="sDataArray">data array.</param>
        public void FillDataFromWeb(NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            // TODO Determine WebService Model
            // TODO USe the good model to Update
            int tModel = GetWebModelValueFromCSV(sDataArray);


            // FORCE TO ENGLISH FORMAT!
            //Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;

            //Debug.Log("UpdateWithCSV ref " + Reference);
            // get key order assembly of cvs
            string[] tKey = PropertiesOrderArray(tModel).ToArray();
            // get values 
            string[] tValue = sDataArray;
            // Short circuit the sync date
            // not replace the date from the other environment
            //if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            //{
            //    tValue[4] = PreprodSync.ToString();
            //    tValue[5] = ProdSync.ToString();
            //}
            //else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            //{
            //    tValue[3] = DevSync.ToString();
            //    tValue[5] = ProdSync.ToString();
            //}
            //else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            //{
            //    tValue[3] = DevSync.ToString();
            //    tValue[4] = PreprodSync.ToString();
            //}
            // process to insertion
            Type tType = ClassType();
            for (int tI = 0; tI < tKey.Count(); tI++)
            {
                if (tValue.Count() > tI)
                {
                    PropertyInfo tPropertyInfo = tType.GetProperty(tKey[tI], BindingFlags.Public | BindingFlags.Instance);
                    if (tPropertyInfo != null)
                    {
                        Type tTypeOfThis = tPropertyInfo.PropertyType;
                        string tValueString = tValue[tI] as string;

                        if (tTypeOfThis.IsEnum)
                        {
                            // sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), data["sign"].ToString(), true);
                            int tValueInsert = 0;
                            int.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {
                            BTBDataType tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataType;
                            tObject.SetValue(tValueString);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            BTBDataTypeInt tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeInt;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            BTBDataTypeFloat tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeFloat;
                            double tTemp = 0;
                            double.TryParse(tValueString, out tTemp);
                            tObject.SetDouble(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        // Do for Standard type
                        else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {
                            tPropertyInfo.SetValue(this, tValueString, null);
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            bool tValueInsert = false;
                            int tTemp = 0;
                            int.TryParse(tValueString, out tTemp);
                            if (tTemp > 0)
                            {
                                tValueInsert = true;
                            }
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            int tValueInsert = 0;
                            int.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            long tValueInsert = 0;
                            long.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            // TODO: bug with dot, comma
                            //Debug.Log("float tValueString = " + tValueString);
                            float tValueInsert = 0;
                            float.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            // TODO: bug with dot, comma
                            //Debug.Log("float tValueString = " + tValueString);
                            double tValueInsert = 0;
                            double.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else
                        {

                        }
                    }
                }
            }
            AddonUpdatedMeFromWeb();
            AddonIndexMe();
        }

        //public string DynamiqueDataAssembly(bool sAsssemblyAsCSV = false)
        //{
        //    string rReturn = string.Empty;
        //    Type tType = ClassType();
        //    List<string> tPropertiesList = DataAssemblyPropertiesList();

        //    // todo get the good version of assembly 
        //    NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
        //    int tLastWebService = -1;
        //    foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
        //    {
        //        if (tKeyValue.Key <= WebServiceVersion && tKeyValue.Key > tLastWebService)
        //        {
        //            if (tKeyValue.Value.ContainsKey(ClassID()))
        //            {
        //                tPropertiesList = tKeyValue.Value[ClassID()];
        //            }
        //        }
        //    }

        //    //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
        //    foreach (string tPropertieName in tPropertiesList)
        //    {
        //        PropertyInfo tProp = tType.GetProperty(tPropertieName);
        //        if (tProp != null)
        //        {
        //            Type tTypeOfThis = tProp.PropertyType;

        //            // Actif to debug the integrity
        //            //rReturn += "|-" + tPropertieName + ":";
        //            // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

        //            string tValueString = string.Empty;

        //            object tValue = tProp.GetValue(this, null);
        //            if (tValue == null)
        //            {
        //                tValue = string.Empty;
        //            }
        //            tValueString = tValue.ToString();
        //            if (tTypeOfThis.IsEnum)
        //            {
        //                //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
        //                int tInt = (int)tValue;
        //                tValueString = tInt.ToString();
        //            }
        //            if (tTypeOfThis == typeof(bool))
        //            {
        //                //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
        //                if (tValueString == "False")
        //                {
        //                    tValueString = "0";
        //                }
        //                else
        //                {
        //                    tValueString = "1";
        //                }
        //            }
        //            if (sAsssemblyAsCSV == true)
        //            {
        //                rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
        //            }
        //            else
        //            {
        //                rReturn += NWDToolbox.TextCSVProtect(tValueString);
        //            }
        //        }
        //    }
        //    if (sAsssemblyAsCSV == true)
        //    {
        //        rReturn = Reference + NWDConstants.kStandardSeparator +
        //        DM + NWDConstants.kStandardSeparator +
        //        DS + NWDConstants.kStandardSeparator +
        //        DevSync + NWDConstants.kStandardSeparator +
        //        PreprodSync + NWDConstants.kStandardSeparator +
        //        ProdSync + NWDConstants.kStandardSeparator +
        //        // Todo Add WebServiceVersion ?
        //        //WebServiceVersion + NWDConstants.kStandardSeparator +
        //        rReturn +
        //        Integrity;
        //        //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
        //    }
        //    else
        //    {
        //        rReturn = Reference +
        //        DM +
        //        rReturn;
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Datas assembly for integrity calculate or cvs export.
        /// </summary>
        /// <returns>The assembly.</returns>
        /// <param name="sAsssemblyAsCVS">If set to <c>true</c> asssembly as CSV.</param>


        public string CSVAssemblyHead()
        {
            return string.Join(NWDConstants.kStandardSeparator, PropertiesOrderArray(-1).ToArray());
        }

        public string CSVAssembly()
        {
            string[] tValues = Assembly();
            return string.Join(NWDConstants.kStandardSeparator, tValues);
        }

        public string IntegrityAssembly()
        {
            string[] tValues = Assembly();
            tValues[2] = "";
            tValues[3] = "";
            tValues[4] = "";
            tValues[5] = "";
            tValues[tValues.Length - 1] = "";
            return string.Join("", tValues);
        }

        public string[] Assembly(/*int sWebBuilt = -1*/)
        {
            List<string> rReturnList = new List<string>();
            Type tType = ClassType();
            List<string> tPropertiesList = PropertiesOrderArray(-1);
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    Type tTypeOfThis = tProp.PropertyType;
                    string tValueString = string.Empty;
                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = string.Empty;
                    }
                    if (tTypeOfThis.IsEnum)
                    {
                        int tInt = (int)tValue;
                        tValueString = tInt.ToString();
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                    {
                        tValueString = tValue.ToString();
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                    {
                        tValueString = NWDToolbox.LongToString(((BTBDataTypeInt)tValue).Value);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                    {
                        tValueString = NWDToolbox.DoubleToString(((BTBDataTypeFloat)tValue).Value);
                    }
                    else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                    {
                        tValueString = tValue.ToString();
                    }
                    else if (tTypeOfThis == typeof(bool))
                    {
                        tValueString = NWDToolbox.BoolToIntString((bool)tValue);
                    }
                    else if (tTypeOfThis == typeof(int))
                    {
                        tValueString = NWDToolbox.IntToString((int)tValue);
                    }
                    else if (tTypeOfThis == typeof(long))
                    {
                        tValueString = NWDToolbox.LongToString((long)tValue);
                    }
                    else if (tTypeOfThis == typeof(float))
                    {
                        tValueString = NWDToolbox.FloatToString((float)tValue);
                    }
                    else if (tTypeOfThis == typeof(double))
                    {
                        tValueString = NWDToolbox.DoubleToString((double)tValue);
                    }
                    else
                    {
                        tValueString = tValue.ToString();
                    }
                    tValueString = NWDToolbox.TextCSVProtect(tValueString);
                    rReturnList.Add(tValueString);
                }
            }
            return rReturnList.ToArray();
        }




        //public virtual string DataAssembly(bool sAsssemblyAsCSV = false)
        //{
        //    // FORCE TO ENGLISH FORMAT!
        //    //Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;

        //    // TODO: use the StringBuilder 
        //    StringBuilder rReturngBuilder = new StringBuilder();
        //    if (sAsssemblyAsCSV == true)
        //    {
        //        rReturngBuilder.Append(Reference);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //        rReturngBuilder.Append(DM);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //        rReturngBuilder.Append(DS);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //        rReturngBuilder.Append(DevSync);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //        rReturngBuilder.Append(PreprodSync);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //        rReturngBuilder.Append(ProdSync);
        //        rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //    }
        //    else
        //    {
        //        rReturngBuilder.Append(Reference);
        //        rReturngBuilder.Append(DM);
        //    }
        //    //string rReturn = string.Empty;
        //    Type tType = ClassType();
        //    List<string> tPropertiesList = DataAssemblyPropertiesList();

        //    // todo get the good version of assembly 
        //    NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
        //    int tLastWebService = -1;
        //    foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
        //    {
        //        if (tKeyValue.Key <= WebServiceVersion && tKeyValue.Key > tLastWebService)
        //        {
        //            if (tKeyValue.Value.ContainsKey(ClassID()))
        //            {
        //                tPropertiesList = tKeyValue.Value[ClassID()];
        //            }
        //        }
        //    }

        //    //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
        //    foreach (string tPropertieName in tPropertiesList)
        //    {
        //        PropertyInfo tProp = tType.GetProperty(tPropertieName);
        //        if (tProp != null)
        //        {
        //            Type tTypeOfThis = tProp.PropertyType;

        //            // Actif to debug the integrity
        //            //rReturn += "|-" + tPropertieName + ":";
        //            // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

        //            string tValueString = string.Empty;

        //            object tValue = tProp.GetValue(this, null);
        //            if (tValue == null)
        //            {
        //                tValue = string.Empty;
        //            }

        //            //tValueString = tValue.ToString();

        //            if (tTypeOfThis.IsEnum)
        //            {
        //                //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
        //                int tInt = (int)tValue;
        //                tValueString = tInt.ToString();
        //            }
        //            else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
        //            {
        //                tValueString = tValue.ToString();
        //            }
        //            else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
        //            {
        //                //tValueString = tValue.ToString();
        //                tValueString = NWDToolbox.LongToString(((BTBDataTypeInt)tValue).Value);
        //            }
        //            else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
        //            {
        //                //tValueString = tValue.ToString();
        //                tValueString = NWDToolbox.DoubleToString(((BTBDataTypeFloat)tValue).Value);
        //            }
        //            // Do for Standard type
        //            else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
        //            {
        //                tValueString = tValue.ToString();
        //            }
        //            else if (tTypeOfThis == typeof(bool))
        //            {
        //                //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
        //                //tValueString = tValue.ToString();
        //                //if (tValueString.ToLower() == "false")
        //                //{
        //                //    tValueString = "0";
        //                //}
        //                //else
        //                //{
        //                //    tValueString = "1";
        //                //}
        //                tValueString = NWDToolbox.BoolToIntString((bool)tValue);
        //            }
        //            else if (tTypeOfThis == typeof(int))
        //            {
        //                tValueString = NWDToolbox.IntToString((int)tValue);
        //            }
        //            else if (tTypeOfThis == typeof(long))
        //            {
        //                //long tFloat = (long)tValue;
        //                //tValueString = tFloat.ToString(NWDConstants.FormatCountry);
        //                //Debug.Log("tValueString long" + tFloat + "=> " + tValueString);
        //                tValueString = NWDToolbox.LongToString((long)tValue);
        //            }
        //            else if (tTypeOfThis == typeof(float))
        //            {
        //                //float tFloat = (float)tValue;
        //                //tValueString = tFloat.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        //                ////Debug.Log("tValueString float" + tFloat+ "=> " + tValueString);
        //                tValueString = NWDToolbox.FloatToString((float)tValue);
        //            }
        //            else if (tTypeOfThis == typeof(double))
        //            {
        //                //double tDouble = (double)tValue;
        //                //tValueString = tDouble.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        //                //Debug.Log("tValueString double" + tDouble + "=> " + tValueString);
        //                tValueString = NWDToolbox.DoubleToString((double)tValue);
        //            }
        //            else
        //            {
        //                tValueString = tValue.ToString();
        //            }
        //            // type of assembly
        //            if (sAsssemblyAsCSV == true)
        //            {
        //                //rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
        //                rReturngBuilder.Append(NWDToolbox.TextCSVProtect(tValueString));
        //                rReturngBuilder.Append(NWDConstants.kStandardSeparator);
        //            }
        //            else
        //            {
        //                //rReturn += NWDToolbox.TextCSVProtect(tValueString);
        //                rReturngBuilder.Append(NWDToolbox.TextCSVProtect(tValueString));
        //            }
        //        }
        //    }
        //    if (sAsssemblyAsCSV == true)
        //    {
        //        //rReturn = Reference + NWDConstants.kStandardSeparator +
        //        //DM + NWDConstants.kStandardSeparator +
        //        //DS + NWDConstants.kStandardSeparator +
        //        //DevSync + NWDConstants.kStandardSeparator +
        //        //PreprodSync + NWDConstants.kStandardSeparator +
        //        //ProdSync + NWDConstants.kStandardSeparator +
        //        //// Todo Add WebServiceVersion ?
        //        ////WebServiceVersion + NWDConstants.kStandardSeparator +
        //        //rReturn + Integrity;
        //        //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
        //        rReturngBuilder.Append(Integrity);
        //    }
        //    else
        //    {
        //        //rReturn = Reference +
        //        //DM +
        //        //rReturn;
        //    }
        //    //return rReturn;
        //    return rReturngBuilder.ToString();
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        // TODO : DEPLACER VERS NWDDATAS
        public static bool ModelDegraded()
        {
            bool rReturn = false;
            int tLasBuild = BasisHelper().LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            int tActualWebBuildMax = NWDAppConfiguration.SharedInstance().WebBuildMax + 1;
            BasisHelper().WebModelDegradationList.Clear();
            Dictionary<int, List<string>> tModel_Properties = new Dictionary<int, List<string>>(BasisHelper().WebModelPropertiesOrder);
            tModel_Properties.Add(tActualWebBuildMax, PropertiesOrderArray(-1));

            if (tModel_Properties.Count > 0)
            {
                List<int> tKeyList = new List<int>();
                foreach (KeyValuePair<int, List<string>> tModel_Prop in tModel_Properties)
                {
                    tKeyList.Add(tModel_Prop.Key);
                }
                tKeyList.Sort();
                List<string> tProp = tModel_Properties[tKeyList[0]];
                //int tCounter = tProp.Count;
                foreach (int tKey in tKeyList)
                {
                    List<string> tPropList = tModel_Properties[tKey];
                    //if (tCounter < tPropList.Count)
                    //{
                    //    // the number of properties is reduce beetween two version !!!!!
                    //    rReturn = true;
                    //}
                    List<string> tResultRemove = new List<string>(tProp);
                    foreach (string tR in tPropList)
                    {
                        tResultRemove.Remove(tR);
                    }
                    if (tResultRemove.Count > 0)
                    {
                        foreach (string tR in tResultRemove)
                        {
                            BasisHelper().WebModelDegradationList.Add(tR);
                            //Debug.Log("... il reste " + tR + " en trop dans le modele " + tKey + " de " + BasisHelper().ClassNamePHP);
                        }
                        // the properties is not increment beetween two versions !!!!!
                        rReturn = true;
                        break;
                    }
                    tProp = tPropList;
                    //tCounter = tProp.Count;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : DEPLACER VERS NWDDATAS
        public static bool ModelChanged()
        {
            bool rReturn = false;
            int tLasBuild = BasisHelper().LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            if (BasisHelper().WebModelSQLOrder.ContainsKey(tLasBuild))
            {
                if (SLQSelect() != BasisHelper().WebModelSQLOrder[tLasBuild])
                {
                    Debug.LogWarning("THE MODEL " + BasisHelper().ClassNamePHP + " CHANGED FROM THE PREVIEW DATAS WEBSERVICE (" + tLasBuild + " / " + tActualWebBuild + ")!");
                    Debug.LogWarning("new : " + SLQSelect());
                    Debug.LogWarning("old : " + BasisHelper().WebModelSQLOrder[tLasBuild]);
                    rReturn = true;
                }
            }
            else
            {
                Debug.LogWarning("THE MODEL" + BasisHelper().ClassNamePHP + " CHANGED FOR UNKNOW WEBSERVICE(" + tLasBuild + "?/ " + tActualWebBuild + ")!");
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : DEPLACER VERS NWDDATAS
        public static void ForceOrders(int sWebBuild)
        {
            if (BasisHelper().LastWebBuild < sWebBuild)
            {
                BasisHelper().LastWebBuild = sWebBuild;
            }
            if (BasisHelper().WebServiceWebModel.ContainsKey(sWebBuild))
            {
                BasisHelper().WebServiceWebModel.Remove(sWebBuild);
            }
            BasisHelper().WebServiceWebModel.Add(sWebBuild, sWebBuild);
            if (BasisHelper().WebModelPropertiesOrder.ContainsKey(sWebBuild))
            {
                BasisHelper().WebModelPropertiesOrder.Remove(sWebBuild);
            }
            BasisHelper().WebModelPropertiesOrder.Add(sWebBuild, PropertiesOrderArray(sWebBuild));
            if (BasisHelper().WebModelSQLOrder.ContainsKey(sWebBuild))
            {
                BasisHelper().WebModelSQLOrder.Remove(sWebBuild);
            }
            BasisHelper().WebModelSQLOrder.Add(sWebBuild, SLQSelect(sWebBuild));
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeleteOldsModels()
        {
            BasisHelper().WebServiceWebModel.Clear();
            BasisHelper().WebModelPropertiesOrder.Clear();
            BasisHelper().WebModelSQLOrder.Clear();
            ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : DEPLACER VERS NWDDATAS
        public static void PrepareOrders()
        {
            ReplaceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : DEPLACER VERS NWDDATAS
        public static void ReplaceOrders(int sWebBuild)
        {
            if (BasisHelper().WebModelChanged)
            {
                BasisHelper().LastWebBuild = sWebBuild;
                if (BasisHelper().WebModelPropertiesOrder.ContainsKey(sWebBuild) == false)
                {
                    BasisHelper().WebModelPropertiesOrder.Add(sWebBuild, PropertiesOrderArray(sWebBuild));
                }
                if (BasisHelper().WebModelSQLOrder.ContainsKey(sWebBuild) == false)
                {
                    BasisHelper().WebModelSQLOrder.Add(sWebBuild, SLQSelect(sWebBuild));
                }
            }
            else
            {
                //Debug.Log(ClassID() + " doesn't be updated for webservice " + tWebBuild.ToString() + " ... Keep cool");
            }
            if (BasisHelper().WebServiceWebModel.ContainsKey(sWebBuild))
            {
                BasisHelper().WebServiceWebModel.Remove(sWebBuild);
            }

            BasisHelper().WebServiceWebModel.Add(sWebBuild, BasisHelper().LastWebBuild);
            Dictionary<int, int> tNextWebServiceWebModel = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> tWS_WebModel in BasisHelper().WebServiceWebModel)
            {
                if (NWDAppConfiguration.SharedInstance().WSList.ContainsKey(tWS_WebModel.Key))
                {
                    tNextWebServiceWebModel.Add(tWS_WebModel.Key, tWS_WebModel.Value);
                }
            }
            BasisHelper().WebServiceWebModel = tNextWebServiceWebModel;


            Dictionary<int, List<string>> tNewPropertiesOrder = new Dictionary<int, List<string>>();
            foreach (KeyValuePair<int, int> tWS_WebModel in BasisHelper().WebServiceWebModel)
            {
                if (BasisHelper().WebModelPropertiesOrder.ContainsKey(tWS_WebModel.Value))
                {
                    if (tNewPropertiesOrder.ContainsKey(tWS_WebModel.Value) == false)
                    {
                        tNewPropertiesOrder.Add(tWS_WebModel.Value, BasisHelper().WebModelPropertiesOrder[tWS_WebModel.Value]);
                    }
                }
            }
            BasisHelper().WebModelPropertiesOrder = tNewPropertiesOrder;

            Dictionary<int, string> tNewWebModelSQLOrder = new Dictionary<int, string>();
            foreach (KeyValuePair<int, int> tWS_WebModel in BasisHelper().WebServiceWebModel)
            {
                if (BasisHelper().WebModelSQLOrder.ContainsKey(tWS_WebModel.Value))
                {
                    if (tNewWebModelSQLOrder.ContainsKey(tWS_WebModel.Value) == false)
                    {
                        tNewWebModelSQLOrder.Add(tWS_WebModel.Value, BasisHelper().WebModelSQLOrder[tWS_WebModel.Value]);
                    }
                }
            }
            BasisHelper().WebModelSQLOrder = tNewWebModelSQLOrder;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================