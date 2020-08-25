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

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Text.RegularExpressions;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasis()
        {
            //Debug.Log("NWDBasis <K> NWDBasis Constructor inserted = " + NWDInserted.ToString());
            //Init your instance here
            //this.InitConstructor();
            //Insert in NetWorkedData;
            NewNetWorkedData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasis(bool sInsertInNetWorkedData = false)
        {
            if (sInsertInNetWorkedData == false)
            {
                // do nothing 
                // perhaps the data came from database and is already in NetWorkedData;
            }
            else
            {
                //Init your instance here
                //this.InitConstructor();
                //Insert in NetWorkedData;
                NewNetWorkedData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void InitConstructor()
        //{
        //    Initialization();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void NewNetWorkedData()
        {
            this.InstanceInit();
            this.Reference = this.NewReference();
            this.PropertiesAutofill();
            this.Initialization();
            this.InsertData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexInBase()
        {
            BasisHelper().IndexInBaseData(this);
            //foreach (MethodInfo tMethod in BasisHelper().IndexInBaseMethodList)
            //{
            //    tMethod.Invoke(this, null);
            //    NWDDataManager.SharedInstance().IndexationCounterOp++;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexInMemory()
        {
            BasisHelper().IndexInMemoryData(this);
            //foreach (MethodInfo tMethod in BasisHelper().IndexInMemoryMethodList)
            //{
            //    tMethod.Invoke(this, null);
            //    NWDDataManager.SharedInstance().IndexationCounterOp++;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexInBase()
        {
            BasisHelper().DeindexInBaseData(this);
            //foreach (MethodInfo tMethod in BasisHelper().DeindexInBaseMethodList)
            //{
            //    tMethod.Invoke(this, null);
            //    NWDDataManager.SharedInstance().IndexationCounterOp++;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexInMemory()
        {
            BasisHelper().DeindexInMemoryData(this);
            //foreach (MethodInfo tMethod in BasisHelper().DeindexInMemoryMethodList)
            //{
            //    tMethod.Invoke(this, null);
            //    NWDDataManager.SharedInstance().IndexationCounterOp++;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PropertiesPrevent()
        {
            //if (BasisHelper().NWDDataPropertiesArray != null)
            //{
            //    foreach (var tPropertyInfo in BasisHelper().NWDDataPropertiesArray)
            //    {
            //        Type tTypeOfThis = tPropertyInfo.PropertyType;
            //        if (tPropertyInfo.GetValue(this) == null)
            //        {
            //            NWEDataType tObject = (NWEDataType)Activator.CreateInstance(tTypeOfThis);
            //            tPropertyInfo.SetValue(this, tObject, null);
            //        }
            //    }
            //}
            if (BasisHelper().NWDDataPropertiesArray != null)
            {
                foreach (var tPropertyInfo in BasisHelper().NWDDataPropertiesArray)
                {
                    Type tTypeOfThis = tPropertyInfo.PropertyType;
                    if (tTypeOfThis != null)
                    {
                        if (tPropertyInfo.GetValue(this) == null)
                        {
                            if (tTypeOfThis.GetType() == typeof(Type))
                            {
                                NWEDataType tObject = (NWEDataType)Activator.CreateInstance(tTypeOfThis);
                                tPropertyInfo.SetValue(this, tObject, null);
                            }
                            else
                            {
                                Debug.LogWarning(tTypeOfThis.GetType().Name + " is not a Type for " + tPropertyInfo.Name + " in " + ClassType().Name + "!");
                            }
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void PropertiesAutofill()
        {
#if NWD_NEVER_NULL_DATATYPE
            PropertiesPrevent();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InstanceInit()
        {
            AC = true;
            DM = NWDToolbox.Timestamp();
            DC = DM;
            DS = 0;
            DD = 0;
            DevSync = 0;
            PreprodSync = 0;
            ProdSync = 0;
            ServerHash = string.Empty;
            ServerLog = string.Empty;
            InternalKey = string.Empty;
            InternalDescription = string.Empty;
            Preview = string.Empty;
            NWDBasisHelper tHelper = BasisHelper();
            WebModel = tHelper.LastWebBuild;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void WebserviceVersionCheckMe()
        {
            //Debug.Log("NWDBasis WebserviceVersionCheckMe()");
            // Find the good webservice version
            int tWebBuildUsed = WebModelToUse();
            // test the web service version
            if (WebModel < tWebBuildUsed && WebModel != 0)
            {
                //Debug.Log("NWDBasis WebserviceVersionCheckMe() Update version");
                this.AddonWebversionUpgradeMe(WebModel, tWebBuildUsed);
                // use to update NWDBasis when push to server.
                //this.AddonVersionMe(); // Modify the special webservice override ( for example version)
                this.UpdateData(false, NWDWritingMode.ByDefaultLocal, true);
                //NWDDataManager.SharedInstance().UpdateObject(this, AccountDependent());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool WebserviceVersionIsValid()
        {
            bool rReturn = true;
            if (WebModel > BasisHelper().LastWebBuild || WebModel > NWDAppConfiguration.SharedInstance().WebBuild)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================