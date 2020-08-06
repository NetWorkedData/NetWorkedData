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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDExample kExample;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDExample Fictive()
        {
            if (kExample == null)
            {
                kExample = NWDBasisHelper.FictiveData<NWDExample>();
            }
            return kExample;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public double SizerCalculate(float sIndexationPercent = 0.15F)
        {
            Sizer = 0;
            foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tPropertyInfo != null)
                {
                    Type tTypeOfThis = tPropertyInfo.PropertyType;
                    if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                    {
                        Sizer += 256;
                    }
                    else if (tTypeOfThis.IsEnum)
                    {
                        Sizer += 1;
                    }
                    else if (tTypeOfThis == typeof(bool))
                    {
                        Sizer += 1;
                    }
                    else if (tTypeOfThis == typeof(int))
                    {
                        Sizer += 4;
                    }
                    else if (tTypeOfThis == typeof(long))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis == typeof(float))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis == typeof(double))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                    {
                        if (tTypeOfThis.IsGenericType)
                        {
                            if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                            {
                                Sizer += 48;
                            }
                        }
                        else
                        {
                            Sizer += 256;
                        }
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                    {
                        Sizer += 8;
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        Sizer += 8;
                    }
                    else
                    {
                    }
                }
            }
            // use index percent
            Sizer += Mathf.FloorToInt((float)Sizer * sIndexationPercent); 
            return Sizer;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string WebServiceOrder(int sWebBuild)
        {
            return string.Join(NWDConstants.kStandardSeparator, PropertiesOrderArray(sWebBuild).ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public string WebServiceSign(int sWebBuild)
        {
            //return string.Join(NWDConstants.kStandardSeparator, PropertiesOrderArray(sWebBuild).ToArray());
            return NWESecurityTools.GenerateSha(SaltEnd+string.Join(NWDConstants.kFieldSeparatorD, PropertiesOrderArray(sWebBuild).ToArray()));
        }
        //-------------------------------------------------------------------------------------------------------------
        private NWDTypeClass NewDataFromWeb(NWDAppEnvironment sEnvironment,
                                                  string[] sDataArray,
                                                  string sReference,
                                                  NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            NWDTypeClass rReturnObject = null;
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
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetReferenceValueFromCSV(string[] sDataArray)
        {
            return sDataArray[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetDMValueFromCSV(string[] sDataArray)
        {
            int rReturn = 0;
            int.TryParse(sDataArray[1], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetIntegrityValueFromCSV(string[] sDataArray)
        {
            return sDataArray[sDataArray.Count() - 1];
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetWebModelValueFromCSV(string[] sDataArray)
        {
            int rReturn = -1;
            int.TryParse(sDataArray[sDataArray.Count() - 2], out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TestIntegrityValueFromCSV(string[] sDataArray)
        {
            bool rReturn = true;
            string tActualIntegrity = GetIntegrityValueFromCSV(sDataArray);
            StringBuilder tAssembly = new StringBuilder();
            tAssembly.Append(sDataArray[0] + sDataArray[1]);
            int tMax = sDataArray.Count() - 1;
            for (int i = 7; i < tMax; i++)
            {
                tAssembly.Append(sDataArray[i]);
            }
            string tCalculateIntegrity = HashSum(SaltStart + tAssembly.ToString() + SaltEnd);
            if (tActualIntegrity != tCalculateIntegrity)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CSV_IndexOf(string sPropertyName, int sWebBuilt = -1)
        {
            return PropertiesOrderArray(sWebBuilt).IndexOf(sPropertyName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> PropertiesOrderArray(int sWebBuilt = -1)
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
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().Integrity));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().Reference));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ID));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DM));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DS));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ServerHash));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ServerLog));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DevSync));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().PreprodSync));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ProdSync));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().InError));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().WebModel));
                rReturnList.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().RangeAccess));

                // not include in integrity
                //rReturn.Remove("WebServiceVersion");
                // add the good order for this element
                rReturnList.Insert(0, NWDToolbox.PropertyName(() => NWDExample.Fictive().Reference));
                rReturnList.Insert(1, NWDToolbox.PropertyName(() => NWDExample.Fictive().DM));
                rReturnList.Insert(2, NWDToolbox.PropertyName(() => NWDExample.Fictive().DS));
                rReturnList.Insert(3, NWDToolbox.PropertyName(() => NWDExample.Fictive().DevSync));
                rReturnList.Insert(4, NWDToolbox.PropertyName(() => NWDExample.Fictive().PreprodSync));
                rReturnList.Insert(5, NWDToolbox.PropertyName(() => NWDExample.Fictive().ProdSync));
                rReturnList.Insert(6, NWDToolbox.PropertyName(() => NWDExample.Fictive().RangeAccess));
                //rReturnList.Insert(7, "ServerHash");
                //rReturnList.Insert(8, "ServerLog");
                //rReturnList.Insert(9, "InError");
                //rReturnList.Insert(10, "ID");

                //rReturnList.Add("ID");
                rReturnList.Add(NWDToolbox.PropertyName(() => NWDExample.Fictive().WebModel));
                rReturnList.Add(NWDToolbox.PropertyName(() => NWDExample.Fictive().Integrity));
            }
            return rReturnList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ModelAnalyze()
        {
            if (WebModelPropertiesOrder.ContainsKey(0) == false)
            {
                WebModelPropertiesOrder.Add(0, PropertiesOrderArray(0));
            }
            if (WebServiceWebModel.ContainsKey(0) == false)
            {
                WebServiceWebModel.Add(0, 0);
            }
            if (WebModelSQLOrder.ContainsKey(0) == false)
            {
                WebModelSQLOrder.Add(0, SLQSelect(0));
            }
#if UNITY_EDITOR
            WebModelDegraded = ModelDegraded();
            WebModelChanged = ModelChanged();
            RefreshAllWindows();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] SLQAssemblyOrderArray(int sWebBuilt = -1) //  for insert of $sCsvList
        {
            string[] rReturn = null;
            List<string> rReturnList = new List<string>();
            rReturnList.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturnList.Remove("Reference");
            rReturn = rReturnList.ToArray();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PHP_SQL_SELECT()
        {
            return SLQSelect(-1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string SLQSelect(int sWebBuilt = -1)
        {
            string rReturnString = string.Empty;
            List<string> tList = new List<string>();
            tList.AddRange(PropertiesOrderArray(sWebBuilt));
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
        public List<string> SLQIntegrityOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().Integrity));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DS));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DevSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().PreprodSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ProdSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().RangeAccess));
            return rReturn;
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public List<string> SLQIntegrityServerOrder(int sWebBuilt = -1)
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray(sWebBuilt));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().Integrity));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DS));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DevSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().PreprodSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().ProdSync));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DM));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().DD));
            rReturn.Remove(NWDToolbox.PropertyName(() => NWDExample.Fictive().RangeAccess));
            return rReturn;
        }//-------------------------------------------------------------------------------------------------------------
        public bool ModelDegraded()
        {
            bool rReturn = false;
            int tLasBuild = LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            int tActualWebBuildMax = NWDAppConfiguration.SharedInstance().WebBuildMax + 1;
            WebModelDegradationList.Clear();
            Dictionary<int, List<string>> tModel_Properties = new Dictionary<int, List<string>>(WebModelPropertiesOrder);
            if (tModel_Properties.ContainsKey( tActualWebBuildMax) == false )
            {
            tModel_Properties.Add(tActualWebBuildMax, PropertiesOrderArray(-1));
            }

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
        public bool ModelChanged()
        {
            bool rReturn = false;
            int tLasBuild = LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            if (WebModelSQLOrder.ContainsKey(tLasBuild))
            {
                if (SLQSelect() != WebModelSQLOrder[tLasBuild])
                {
                    Debug.LogWarning("THE MODEL " + ClassNamePHP + " CHANGED FROM THE PREVIEW DATAS WEBSERVICE (" + tLasBuild + " / " + tActualWebBuild + ")!");
                    Debug.LogWarning("new : " + SLQSelect());
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
        public string ModelChangedGetChange()
        {
            string rReturn = string.Empty;
            int tLasBuild = LastWebBuild;
            int tActualWebBuild = NWDAppConfiguration.SharedInstance().WebBuild;
            if (WebModelSQLOrder.ContainsKey(tLasBuild))
            {
                if (SLQSelect() != WebModelSQLOrder[tLasBuild])
                {
                    List<string> tListA  = new  List<string>(SLQSelect().Split(new char[]{','}));
                    List<string> tListB= new  List<string>(WebModelSQLOrder[tLasBuild].Split(new char[]{','}));
                    foreach (string t in  tListA)
                    {
                        if (tListB.Contains(t)==false)
                        {
                            rReturn += "\n+ Add : " +t;
                        }
                    }
                    foreach (string t in tListB)
                    {
                        if (tListA.Contains(t) == false)
                        {
                            rReturn += "\n- Remove : " + t;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ForceOrders(int sWebBuild)
        {
            //if (LastWebBuild < sWebBuild)
            //{
            //    LastWebBuild = sWebBuild;
            //}
            if (WebServiceWebModel.ContainsKey(sWebBuild))
            {
                WebServiceWebModel.Remove(sWebBuild);
            }
            WebServiceWebModel.Add(sWebBuild, sWebBuild);
            if (WebModelPropertiesOrder.ContainsKey(sWebBuild))
            {
                WebModelPropertiesOrder.Remove(sWebBuild);
            }
            WebModelPropertiesOrder.Add(sWebBuild, PropertiesOrderArray(sWebBuild));
            if (WebModelSQLOrder.ContainsKey(sWebBuild))
            {
                WebModelSQLOrder.Remove(sWebBuild);
            }
            WebModelSQLOrder.Add(sWebBuild, SLQSelect(sWebBuild));
            RefreshAllWindows();
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_ModelReset)]
        public void DeleteOldsModels()
        {
            WebServiceWebModel.Clear();
            WebModelPropertiesOrder.Clear();
            WebModelSQLOrder.Clear();
            ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
            RefreshAllWindows();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PrepareOrders()
        {
            ReplaceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
            RefreshAllWindows();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DetermineLast()
        {
            // generate new last build for this model
            LastWebBuild = 0;
            foreach (KeyValuePair<int, List<string>> tWS_WebModel in WebModelPropertiesOrder)
            {
                if (LastWebBuild < tWS_WebModel.Key)
                {
                    LastWebBuild = tWS_WebModel.Key;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReplaceOrders(int sWebBuild)
        {
            //Debug.Log("ReplaceOrders");
            if (WebModelChanged)
            {
                if (WebModelPropertiesOrder.ContainsKey(sWebBuild) == false)
                {
                    WebModelPropertiesOrder.Add(sWebBuild, PropertiesOrderArray(sWebBuild));
                }
                if (WebModelSQLOrder.ContainsKey(sWebBuild) == false)
                {
                    WebModelSQLOrder.Add(sWebBuild, SLQSelect(sWebBuild));
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

            DetermineLast();

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
            RefreshAllWindows();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================