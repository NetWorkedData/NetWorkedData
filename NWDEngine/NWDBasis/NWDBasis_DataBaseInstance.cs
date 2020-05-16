//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
            this.InitConstructor();
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
                this.InitConstructor();
                //Insert in NetWorkedData;
                NewNetWorkedData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InitConstructor()
        {
            Initialization();
        }
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
            if (tHelper.NWDDataPropertiesArray != null)
            {
                foreach (var tPropertyInfo in tHelper.NWDDataPropertiesArray)
                {
                    Type tTypeOfThis = tPropertyInfo.PropertyType;
                    if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                    {
                        NWEDataType tObject = (NWEDataType)Activator.CreateInstance(tTypeOfThis);
                        //tObject.SetValue(string.Empty);
                        tPropertyInfo.SetValue(this, tObject, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                    {
                        NWEDataTypeInt tObject = (NWEDataTypeInt)Activator.CreateInstance(tTypeOfThis);
                        //tObject.SetLong(0);
                        tPropertyInfo.SetValue(this, tObject, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                    {
                        NWEDataTypeFloat tObject = (NWEDataTypeFloat)Activator.CreateInstance(tTypeOfThis);
                        //tObject.SetDouble(0);
                        tPropertyInfo.SetValue(this, tObject, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                    {
                        NWEDataTypeEnum tObject = (NWEDataTypeEnum)Activator.CreateInstance(tTypeOfThis);
                        //tObject.SetLong(0);
                        tPropertyInfo.SetValue(this, tObject, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        NWEDataTypeMask tObject = (NWEDataTypeMask)Activator.CreateInstance(tTypeOfThis);
                        //tObject.SetLong(0);
                        tPropertyInfo.SetValue(this, tObject, null);
                    }
                }
            }
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