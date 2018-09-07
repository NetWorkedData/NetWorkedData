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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------

        #region Class Methods

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the reference value from CSV.
        /// </summary>
        /// <returns>The reference value from CSV.</returns>
        /// <param name="sDataArray">data array.</param>
        public static string GetReferenceValueFromCSV(string[] sDataArray)
        {
            return sDataArray[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the DM value from CSV. DM for Date Modification.
        /// </summary>
        /// <returns>The DM value from CSV.</returns>
        /// <param name="sDataArray">data array.</param>
        public static int GetDMValueFromCSV(string[] sDataArray)
        {
            int rReturn = 0;
            int.TryParse(sDataArray[1], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the integrity value from CSV.
        /// </summary>
        /// <returns>The integrity value from CSV.</returns>
        /// <param name="sDataArray">data array.</param>
        public static string GetIntegrityValueFromCSV(string[] sDataArray)
        {
            return sDataArray[sDataArray.Count() - 1];
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Tests the integrity value from CSV.
        /// </summary>
        /// <returns><c>true</c>, if integrity value from CSV was tested, <c>false</c> otherwise.</returns>
        /// <param name="sDataArray">data array.</param>
        public static bool TestIntegrityValueFromCSV(string[] sDataArray)
        {
            bool rReturn = true;
            string tActualIntegrity = GetIntegrityValueFromCSV(sDataArray);
            string tAssembly = sDataArray[0] + sDataArray[1];
            int tMax = sDataArray.Count() - 1;
            for (int i = 6; i < tMax; i++)
            {
                tAssembly += sDataArray[i];
            }
            string tCalculateIntegrity = HashSum(Datas().SaltA + tAssembly +Datas().SaltB);
            if (tActualIntegrity != tCalculateIntegrity)
            {
                rReturn = false;
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, List<string>> kPropertiesOrderArray = new Dictionary<string, List<string>>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Propertieses the order array.
        /// </summary>
        /// <returns>The order array.</returns>
        public static List<string> PropertiesOrderArray()
        {
            if (kPropertiesOrderArray.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                Type tType = ClassType();
                foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    rReturn.Add(tProp.Name);
                }
                rReturn.Sort();
                kPropertiesOrderArray[ClassID()] = rReturn;
            }
            return kPropertiesOrderArray[ClassID()];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, string[]> kCSVAssemblyOrderArray = new Dictionary<string, string[]>();

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// CSV assembly order array.
        /// </summary>
        /// <returns>The assembly order array.</returns>
        public static string[] CSVAssemblyOrderArray()
        {
            if (kCSVAssemblyOrderArray.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DM");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("ServerLog");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");
                // add the good order for this element
                rReturn.Insert(0, "Reference");
                rReturn.Insert(1, "DM");
                rReturn.Insert(2, "DS");
                rReturn.Insert(3, "DevSync");
                rReturn.Insert(4, "PreprodSync");
                rReturn.Insert(5, "ProdSync");
                //rReturn.Add("WebServiceVersion");
                rReturn.Add("Integrity");
                kCSVAssemblyOrderArray[ClassID()] = rReturn.ToArray<string>();
            }
            return kCSVAssemblyOrderArray[ClassID()];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, string[]> kSLQAssemblyOrderArray = new Dictionary<string, string[]>();


        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order array.
        /// </summary>
        /// <returns>The assembly order array.</returns>
        public static string[] SLQAssemblyOrderArray() //  for insert of $sCsvList
        {
            if (kSLQAssemblyOrderArray.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DM");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("ServerLog");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");
                // add the good order for this element
                rReturn.Insert(0, "DM");
                rReturn.Insert(1, "DS");
                rReturn.Insert(2, "DevSync");
                rReturn.Insert(3, "PreprodSync");
                rReturn.Insert(4, "ProdSync");
                //rReturn.Add("WebServiceVersion");
                rReturn.Add("Integrity");
                kSLQAssemblyOrderArray[ClassID()] = rReturn.ToArray<string>();
            }
            return kSLQAssemblyOrderArray[ClassID()];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, string> kSLQAssemblyOrder = new Dictionary<string, string>();


        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static string SLQAssemblyOrder()
        {
            if (kSLQAssemblyOrder.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DM");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("ServerLog");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");
                // add the good order for this element
                rReturn.Insert(0, "Reference");
                rReturn.Insert(1, "DM");
                rReturn.Insert(2, "DS");
                rReturn.Insert(3, "DevSync");
                rReturn.Insert(4, "PreprodSync");
                rReturn.Insert(5, "ProdSync");
                //rReturn.Add("WebServiceVersion");
                rReturn.Add("Integrity");
                kSLQAssemblyOrder[ClassID()] = "`" + string.Join("`, `", rReturn.ToArray()) + "`";
            }
            return kSLQAssemblyOrder[ClassID()];
        }

        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, List<string>> kSLQIntegrityOrder = new Dictionary<string, List<string>>();

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> SLQIntegrityOrder()
        {
            if (kSLQIntegrityOrder.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DM");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("ServerLog");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");
                rReturn.Sort((tA, tB) => string.Compare(tA, tB, StringComparison.Ordinal));
                // add the good order for this element
                rReturn.Insert(0, "Reference");
                rReturn.Insert(1, "DM");
                //rReturn.Add("WebServiceVersion");
                kSLQIntegrityOrder[ClassID()] = rReturn;
            }
            return kSLQIntegrityOrder[ClassID()];
        }

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, List<string>> kSLQIntegrityServerOrder = new Dictionary<string, List<string>>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> SLQIntegrityServerOrder()
        {
            if (kSLQIntegrityServerOrder.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("ServerLog");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");

                // I remove this to be able to trash and untrash object without break server integrity (perhaps bad solution ?)
                rReturn.Remove("DM");
                rReturn.Remove("AC");
                rReturn.Remove("DC");
                rReturn.Remove("DD");
                // add the good order for this element
                rReturn.Sort((tA, tB) => string.Compare(tB, tA, StringComparison.Ordinal));
                // add the good order for this element
                rReturn.Insert(2, "Reference");
                // add another order for these element (perhaps bad solution ?)
                rReturn.Add("AC");
                //rReturn.Add("DD");
                rReturn.Add("DC");
                //rReturn.Add("DM");
                kSLQIntegrityServerOrder[ClassID()] = rReturn;
            }
            return kSLQIntegrityServerOrder[ClassID()];
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, List<string>> kDataAssemblyPropertiesList = new Dictionary<string, List<string>>();

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> DataAssemblyPropertiesList()
        {
            if (kDataAssemblyPropertiesList.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity"); // not include in integrity
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DM");
                rReturn.Remove("DS");// not include in integrity
                rReturn.Remove("ServerHash");// not include in integrity
                rReturn.Remove("ServerLog");// not include in integrity
                rReturn.Remove("DevSync");// not include in integrity
                rReturn.Remove("PreprodSync");// not include in integrity
                rReturn.Remove("ProdSync");// not include in integrity
                rReturn.Remove("ProdSync");// not include in integrity

                // to prevent integrity error in check InError
                rReturn.Remove("InError");// not include in integrity
                //rReturn.Remove("WebServiceVersion");
                //rReturn.Add("WebServiceVersion");
                kDataAssemblyPropertiesList[ClassID()] = rReturn;
            }
            return kDataAssemblyPropertiesList[ClassID()];
        }



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
                                                  NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            //Debug.Log("NewDataFromWeb ()");
            NWDBasis<K> rReturnObject = null;
            //rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType());
            rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType(), new object[] { false });
            rReturnObject.InstanceInit();
            rReturnObject.Reference = sReference;

            rReturnObject.FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            Datas().AddData(rReturnObject);

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

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Instance Methods

        public void UpdateDataFromWeb(NWDAppEnvironment sEnvironment,
                                      string[] sDataArray,
                                      NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            // Force update with CVS value

            FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            Datas().UpdateData(this);
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
            //Debug.Log("UpdateWithCSV ref " + Reference);
            // get key order assembly of cvs
            string[] tKey = CSVAssemblyOrderArray();
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
                        //						var tObject = Activator.CreateInstance (tTypeOfThis);
                        //						var tMethodInfo = tObject.GetType ().GetMethod ("SetString", BindingFlags.Public | BindingFlags.Instance);
                        //						if (tMethodInfo != null) {
                        //							tMethodInfo.Invoke (tObject, new object[]{ tValueString });
                        //						}
                        //
                        BTBDataType tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataType;
                        tObject.SetString(tValueString);

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
                    else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int16) || tTypeOfThis == typeof(Int32) || tTypeOfThis == typeof(Int64))
                    {
                        int tValueInsert = 0;
                        int.TryParse(tValueString, out tValueInsert);
                        tPropertyInfo.SetValue(this, tValueInsert, null);
                    }
                    else if (tTypeOfThis == typeof(float) || tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Single) || tTypeOfThis == typeof(Double) || tTypeOfThis == typeof(Decimal))
                    {
                        float tValueInsert = 0;
                        float.TryParse(tValueString, out tValueInsert);
                        tPropertyInfo.SetValue(this, tValueInsert, null);
                    }
                    else
                    {

                    }
                }
            }
            //NWDDataManager.SharedInstance().UpdateObjectDirect(this, AccountDependent());
            //NWDDataManager.SharedInstance().UpdateData(this, NWDWritingMode.QueuedMainThread);
            AddonUpdatedMeFromWeb();
            AddonIndexMe();
        }

        public string DynamiqueDataAssembly(bool sAsssemblyAsCSV = false)
        {
            string rReturn = "";
            Type tType = ClassType();
            List<string> tPropertiesList = DataAssemblyPropertiesList();

            // todo get the good version of assembly 
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tLastWebService = -1;
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
            {
                if (tKeyValue.Key <= WebServiceVersion && tKeyValue.Key > tLastWebService)
                {
                    if (tKeyValue.Value.ContainsKey(ClassID()))
                    {
                        tPropertiesList = tKeyValue.Value[ClassID()];
                    }
                }
            }

            //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    Type tTypeOfThis = tProp.PropertyType;

                    // Actif to debug the integrity
                    //rReturn += "|-" + tPropertieName + ":";
                    // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

                    string tValueString = "";

                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = "";
                    }
                    tValueString = tValue.ToString();
                    if (tTypeOfThis.IsEnum)
                    {
                        //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
                        int tInt = (int)tValue;
                        tValueString = tInt.ToString();
                    }
                    if (tTypeOfThis == typeof(bool))
                    {
                        //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
                        if (tValueString == "False")
                        {
                            tValueString = "0";
                        }
                        else
                        {
                            tValueString = "1";
                        }
                    }
                    if (sAsssemblyAsCSV == true)
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
                    }
                    else
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString);
                    }
                }
            }
            if (sAsssemblyAsCSV == true)
            {
                rReturn = Reference + NWDConstants.kStandardSeparator +
                DM + NWDConstants.kStandardSeparator +
                DS + NWDConstants.kStandardSeparator +
                DevSync + NWDConstants.kStandardSeparator +
                PreprodSync + NWDConstants.kStandardSeparator +
                ProdSync + NWDConstants.kStandardSeparator +
                // Todo Add WebServiceVersion ?
                //WebServiceVersion + NWDConstants.kStandardSeparator +
                rReturn +
                Integrity;
                //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
            }
            else
            {
                rReturn = Reference +
                DM +
                rReturn;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Datas assembly for integrity calculate or cvs export.
        /// </summary>
        /// <returns>The assembly.</returns>
        /// <param name="sAsssemblyAsCVS">If set to <c>true</c> asssembly as CSV.</param>
        public string DataAssembly(bool sAsssemblyAsCSV = false)
        {
            string rReturn = "";
            Type tType = ClassType();
            List<string> tPropertiesList = DataAssemblyPropertiesList();

            // todo get the good version of assembly 
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tLastWebService = -1;
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in tApp.kWebBuildkDataAssemblyPropertiesList)
            {
                if (tKeyValue.Key <= WebServiceVersion && tKeyValue.Key > tLastWebService)
                {
                    if (tKeyValue.Value.ContainsKey(ClassID()))
                    {
                        tPropertiesList = tKeyValue.Value[ClassID()];
                    }
                }
            }

            //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    Type tTypeOfThis = tProp.PropertyType;

                    // Actif to debug the integrity
                    //rReturn += "|-" + tPropertieName + ":";
                    // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

                    string tValueString = "";

                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = "";
                    }
                    tValueString = tValue.ToString();
                    if (tTypeOfThis.IsEnum)
                    {
                        //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
                        int tInt = (int)tValue;
                        tValueString = tInt.ToString();
                    }
                    if (tTypeOfThis == typeof(bool))
                    {
                        //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
                        if (tValueString == "False")
                        {
                            tValueString = "0";
                        }
                        else
                        {
                            tValueString = "1";
                        }
                    }
                    if (sAsssemblyAsCSV == true)
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
                    }
                    else
                    {
                        rReturn += NWDToolbox.TextCSVProtect(tValueString);
                    }
                }
            }
            if (sAsssemblyAsCSV == true)
            {
                rReturn = Reference + NWDConstants.kStandardSeparator +
                DM + NWDConstants.kStandardSeparator +
                DS + NWDConstants.kStandardSeparator +
                DevSync + NWDConstants.kStandardSeparator +
                PreprodSync + NWDConstants.kStandardSeparator +
                ProdSync + NWDConstants.kStandardSeparator +
                // Todo Add WebServiceVersion ?
                //WebServiceVersion + NWDConstants.kStandardSeparator +
                rReturn +
                Integrity;
                //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
            }
            else
            {
                rReturn = Reference +
                DM +
                rReturn;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------

        #endregion


        // TODO : Create WebService memorize

        //public static Dictionary<int, Dictionary<string, string[]>> kWebBuildkCSVAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();

        //public static Dictionary<int, Dictionary<string, string[]>> kWebBuildkSLQAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();

        //public static Dictionary<int, Dictionary<string, string>> kWebBuildkSLQAssemblyOrder = new Dictionary<int, Dictionary<string, string>>();

        //public static Dictionary<int, Dictionary<string, List<string>>> kWebBuildkSLQIntegrityOrder = new Dictionary<int, Dictionary<string, List<string>>>();

        //public static Dictionary<int, Dictionary<string, List<string>>> kWebBuildkSLQIntegrityServerOrder = new Dictionary<int, Dictionary<string, List<string>>>();

        //public static Dictionary<int, Dictionary<string, List<string>>> kWebBuildkDataAssemblyPropertiesList = new Dictionary<int, Dictionary<string, List<string>>>();

#if UNITY_EDITOR
        public static void PrepareOrders()
        {
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            int tWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            //Debug.Log("PrepareOrders for webservice : "+tWebBuild.ToString());

            // TODO test old version is diffeerent from new version of data
            string tLastRegister = "";
            int tLast = 0;
            foreach (KeyValuePair<int, Dictionary<string, string>> tPair in tApp.kWebBuildkSLQAssemblyOrder)
            {
                if (tLast < tPair.Key)
                {
                    tLast = tPair.Key;
                    if (tPair.Value.ContainsKey(ClassID()))
                    {
                        tLastRegister = tPair.Value[ClassID()];
                    }
                }
            }
            string tActualRegister = SLQAssemblyOrder();

            //Debug.Log(ClassID() + " tActualRegister = "+ tActualRegister);
            //Debug.Log(ClassID() + " tLastRegister = " + tLastRegister);
            if (tActualRegister != tLastRegister)
            {
                //Debug.Log(ClassID() + " must be updated for webservice " + tWebBuild.ToString());
                if (tApp.kWebBuildkCSVAssemblyOrderArray.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkCSVAssemblyOrderArray.Add(tWebBuild, new Dictionary<string, string[]>());
                }
                if (tApp.kWebBuildkCSVAssemblyOrderArray[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkCSVAssemblyOrderArray[tWebBuild].Add(ClassID(), CSVAssemblyOrderArray());
                }

                if (tApp.kWebBuildkSLQAssemblyOrderArray.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkSLQAssemblyOrderArray.Add(tWebBuild, new Dictionary<string, string[]>());
                }
                if (tApp.kWebBuildkSLQAssemblyOrderArray[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkSLQAssemblyOrderArray[tWebBuild].Add(ClassID(), SLQAssemblyOrderArray());
                }


                if (tApp.kWebBuildkSLQAssemblyOrder.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkSLQAssemblyOrder.Add(tWebBuild, new Dictionary<string, string>());
                }
                if (tApp.kWebBuildkSLQAssemblyOrder[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkSLQAssemblyOrder[tWebBuild].Add(ClassID(), SLQAssemblyOrder());
                }


                if (tApp.kWebBuildkSLQIntegrityOrder.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkSLQIntegrityOrder.Add(tWebBuild, new Dictionary<string, List<string>>());
                }
                if (tApp.kWebBuildkSLQIntegrityOrder[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkSLQIntegrityOrder[tWebBuild].Add(ClassID(), SLQIntegrityOrder());
                }



                if (tApp.kWebBuildkSLQIntegrityServerOrder.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkSLQIntegrityServerOrder.Add(tWebBuild, new Dictionary<string, List<string>>());
                }
                if (tApp.kWebBuildkSLQIntegrityServerOrder[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkSLQIntegrityServerOrder[tWebBuild].Add(ClassID(), SLQIntegrityServerOrder());
                }


                if (tApp.kWebBuildkDataAssemblyPropertiesList.ContainsKey(tWebBuild) == false)
                {
                    tApp.kWebBuildkDataAssemblyPropertiesList.Add(tWebBuild, new Dictionary<string, List<string>>());
                }
                if (tApp.kWebBuildkDataAssemblyPropertiesList[tWebBuild].ContainsKey(ClassID()) == false)
                {
                    tApp.kWebBuildkDataAssemblyPropertiesList[tWebBuild].Add(ClassID(), DataAssemblyPropertiesList());
                }
            }
            else
            {
                //Debug.Log(ClassID() + " doesn't be updated for webservice " + tWebBuild.ToString() + " ... Keep cool");
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================