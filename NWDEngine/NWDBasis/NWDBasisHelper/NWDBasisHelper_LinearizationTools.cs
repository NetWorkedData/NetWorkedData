//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        private NWDTypeClass New_NewDataFromWeb(NWDAppEnvironment sEnvironment,
                                                  string[] sDataArray,
                                                  string sReference,
                                                  NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            // 
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            //Debug.Log("NewDataFromWeb ()");
            NWDTypeClass rReturnObject = null;
            //rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType());
            rReturnObject = (NWDTypeClass)Activator.CreateInstance(ClassType, new object[] { false });
            rReturnObject.InstanceInit();
            rReturnObject.Reference = sReference;

            rReturnObject.FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            AddData(rReturnObject);

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
        public string New_GetReferenceValueFromCSV(string[] sDataArray)
        {
            return sDataArray[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public int New_GetDMValueFromCSV(string[] sDataArray)
        {
            int rReturn = 0;
            int.TryParse(sDataArray[1], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string New_GetIntegrityValueFromCSV(string[] sDataArray)
        {
            return sDataArray[sDataArray.Count() - 1];
        }
        //-------------------------------------------------------------------------------------------------------------
        public int New_GetWebModelValueFromCSV(string[] sDataArray)
        {
            int rReturn = -1;
            int.TryParse(sDataArray[sDataArray.Count() - 2], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool New_TestIntegrityValueFromCSV(string[] sDataArray)
        {
            bool rReturn = true;
            string tActualIntegrity = New_GetIntegrityValueFromCSV(sDataArray);
            StringBuilder tAssembly = new StringBuilder();
            tAssembly.Append(sDataArray[0] + sDataArray[1]);
            int tMax = sDataArray.Count() - 1;
            for (int i = 6; i < tMax; i++)
            {
                tAssembly.Append(sDataArray[i]);
            }
            string tCalculateIntegrity = New_HashSum(SaltStart + tAssembly.ToString() + SaltEnd);
            if (tActualIntegrity != tCalculateIntegrity)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int New_CSV_IndexOf(string sPropertyName, int sWebBuilt = -1)
        {
            return New_PropertiesOrderArray(sWebBuilt).IndexOf(sPropertyName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> New_PropertiesOrderArray(int sWebBuilt = -1)
        {
            bool tRecalculate = true;
            List<string> rReturnList = null;
            int tWebBuilt = sWebBuilt;
            int tWebModel = sWebBuilt;

            if (tWebBuilt == -1)
            {
                tWebBuilt = NWDAppConfiguration.SharedInstance().WebBuild;
            }

            if (WebServiceWebModel.ContainsKey(tWebBuilt))
            {
                tWebModel = WebServiceWebModel[tWebBuilt];
            }
            else
            {
                // tWebBuilt is unknow ... no webmodel !?
            }
            if (WebModelPropertiesOrder.ContainsKey(tWebModel))
            {
                tRecalculate = false;
                rReturnList = WebModelPropertiesOrder[tWebModel];
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
                Type tType = ClassType;
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
            return rReturnList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ModelAnalyze()
        {
            if (WebModelPropertiesOrder.ContainsKey(0) == false)
            {
                WebModelPropertiesOrder.Add(0, New_PropertiesOrderArray(0));
            }
            if (WebServiceWebModel.ContainsKey(0) == false)
            {
                WebServiceWebModel.Add(0, 0);
            }
            if (WebModelSQLOrder.ContainsKey(0) == false)
            {
                WebModelSQLOrder.Add(0, New_SLQSelect(0));
            }
            WebModelDegraded = New_ModelDegraded();
            WebModelChanged = New_ModelChanged();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] New_SLQAssemblyOrderArray(int sWebBuilt = -1) //  for insert of $sCsvList
        {
            string[] rReturn = null;
            List<string> rReturnList = new List<string>();
            rReturnList.AddRange(New_PropertiesOrderArray(sWebBuilt));
            rReturnList.Remove("Reference");
            rReturn = rReturnList.ToArray();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public  string New_SLQSelect(int sWebBuilt = -1)
        {
            string rReturnString = string.Empty;
            List<string> tList = new List<string>();
            tList.AddRange(New_PropertiesOrderArray(sWebBuilt));
            rReturnString = "***";
            foreach (string tPropertyName in tList)
            {
                PropertyInfo tPropertyInfo = ClassType.GetProperty(tPropertyName, BindingFlags.Public | BindingFlags.Instance);
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
        public List<string> New_SLQIntegrityOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(New_PropertiesOrderArray(sWebBuilt));
            rReturn.Remove("Integrity");
            rReturn.Remove("DS");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            return rReturn;
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public List<string> New_SLQIntegrityServerOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(New_PropertiesOrderArray(sWebBuilt));
            rReturn.Remove("Integrity");
            rReturn.Remove("DS");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            rReturn.Remove("DM");
            rReturn.Remove("DD");
            return rReturn;
        }//-------------------------------------------------------------------------------------------------------------
        public bool New_ModelDegraded()
        {
            bool rReturn = false;
            int tLasBuild = LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            int tActualWebBuildMax = NWDAppConfiguration.SharedInstance().WebBuildMax + 1;
            WebModelDegradationList.Clear();
            Dictionary<int, List<string>> tModel_Properties = new Dictionary<int, List<string>>(WebModelPropertiesOrder);
            tModel_Properties.Add(tActualWebBuildMax, New_PropertiesOrderArray(-1));

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
                            WebModelDegradationList.Add(tR);
                            //Debug.Log("... il reste " + tR + " en trop dans le modele " + tKey + " de " + ClassNamePHP);
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
        public  bool New_ModelChanged()
        {
            bool rReturn = false;
            int tLasBuild = LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            if (WebModelSQLOrder.ContainsKey(tLasBuild))
            {
                if (New_SLQSelect() != WebModelSQLOrder[tLasBuild])
                {
                    Debug.LogWarning("THE MODEL " + ClassNamePHP + " CHANGED FROM THE PREVIEW DATAS WEBSERVICE (" + tLasBuild + " / " + tActualWebBuild + ")!");
                    Debug.LogWarning("new : " + New_SLQSelect());
                    Debug.LogWarning("old : " + WebModelSQLOrder[tLasBuild]);
                    rReturn = true;
                }
            }
            else
            {
                Debug.LogWarning("THE MODEL" + ClassNamePHP + " CHANGED FOR UNKNOW WEBSERVICE(" + tLasBuild + "?/ " + tActualWebBuild + ")!");
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_ForceOrders(int sWebBuild)
        {
            if (LastWebBuild < sWebBuild)
            {
                LastWebBuild = sWebBuild;
            }
            if (WebServiceWebModel.ContainsKey(sWebBuild))
            {
                WebServiceWebModel.Remove(sWebBuild);
            }
            WebServiceWebModel.Add(sWebBuild, sWebBuild);
            if (WebModelPropertiesOrder.ContainsKey(sWebBuild))
            {
                WebModelPropertiesOrder.Remove(sWebBuild);
            }
            WebModelPropertiesOrder.Add(sWebBuild, New_PropertiesOrderArray(sWebBuild));
            if (WebModelSQLOrder.ContainsKey(sWebBuild))
            {
                WebModelSQLOrder.Remove(sWebBuild);
            }
            WebModelSQLOrder.Add(sWebBuild, New_SLQSelect(sWebBuild));
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_ModelReset)]
        public void New_DeleteOldsModels()
        {
            WebServiceWebModel.Clear();
            WebModelPropertiesOrder.Clear();
            WebModelSQLOrder.Clear();
            New_ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_PrepareOrders()
        {
            New_ReplaceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_ReplaceOrders(int sWebBuild)
        {
            if (WebModelChanged)
            {
                LastWebBuild = sWebBuild;
                if (WebModelPropertiesOrder.ContainsKey(sWebBuild) == false)
                {
                    WebModelPropertiesOrder.Add(sWebBuild, New_PropertiesOrderArray(sWebBuild));
                }
                if (WebModelSQLOrder.ContainsKey(sWebBuild) == false)
                {
                    WebModelSQLOrder.Add(sWebBuild, New_SLQSelect(sWebBuild));
                }
            }
            else
            {
                //Debug.Log(ClassID() + " doesn't be updated for webservice " + tWebBuild.ToString() + " ... Keep cool");
            }
            if (WebServiceWebModel.ContainsKey(sWebBuild))
            {
                WebServiceWebModel.Remove(sWebBuild);
            }

            WebServiceWebModel.Add(sWebBuild, LastWebBuild);
            Dictionary<int, int> tNextWebServiceWebModel = new Dictionary<int, int>();
            foreach (KeyValuePair<int, int> tWS_WebModel in WebServiceWebModel)
            {
                if (NWDAppConfiguration.SharedInstance().WSList.ContainsKey(tWS_WebModel.Key))
                {
                    tNextWebServiceWebModel.Add(tWS_WebModel.Key, tWS_WebModel.Value);
                }
            }
            WebServiceWebModel = tNextWebServiceWebModel;


            Dictionary<int, List<string>> tNewPropertiesOrder = new Dictionary<int, List<string>>();
            foreach (KeyValuePair<int, int> tWS_WebModel in WebServiceWebModel)
            {
                if (WebModelPropertiesOrder.ContainsKey(tWS_WebModel.Value))
                {
                    if (tNewPropertiesOrder.ContainsKey(tWS_WebModel.Value) == false)
                    {
                        tNewPropertiesOrder.Add(tWS_WebModel.Value, WebModelPropertiesOrder[tWS_WebModel.Value]);
                    }
                }
            }
            WebModelPropertiesOrder = tNewPropertiesOrder;

            Dictionary<int, string> tNewWebModelSQLOrder = new Dictionary<int, string>();
            foreach (KeyValuePair<int, int> tWS_WebModel in WebServiceWebModel)
            {
                if (WebModelSQLOrder.ContainsKey(tWS_WebModel.Value))
                {
                    if (tNewWebModelSQLOrder.ContainsKey(tWS_WebModel.Value) == false)
                    {
                        tNewWebModelSQLOrder.Add(tWS_WebModel.Value, WebModelSQLOrder[tWS_WebModel.Value]);
                    }
                }
            }
            WebModelSQLOrder = tNewWebModelSQLOrder;
        }
        //-------------------------------------------------------------------------------------------------------------

#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================