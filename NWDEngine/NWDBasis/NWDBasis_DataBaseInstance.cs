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

using SQLite4Unity3d;

using BasicToolBox;
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
                // perhaps the data came from database and is allready in NetWorkedData;
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
        public override void ReIndex()
        {
            Desindex();
            Index();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Index()
        {
            foreach (MethodInfo tMethod in BasisHelper().IndexInsertMethodList)
            {
                //Debug.Log("Index override" + Reference + " tMethod = "+ tMethod.Name);
                tMethod.Invoke(this, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Desindex()
        {
            foreach (MethodInfo tMethod in BasisHelper().IndexRemoveMethodList)
            {
                tMethod.Invoke(this, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InstanceInit()
        {
            AC = true;
            DM = NWDToolbox.Timestamp();
            DC = NWDToolbox.Timestamp();
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

            int tWebModelToUse = WebModelToUse();
           // Debug.Log(" set from " + this.WebModel + " To " + tWebModelToUse);
            WebModel = tWebModelToUse;
            //Debug.Log("NWDBasis <K> InstanceInit() inserted = " + NWDInserted.ToString());
            Type tType = ClassType();
            foreach (var tPropertyInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tPropertyInfo.PropertyType;
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                {
                // TODO : Change to remove invoke!
                    var tObject = Activator.CreateInstance(tTypeOfThis);
                    var tMethodInfo = tObject.GetType().GetMethod("SetString", BindingFlags.Public | BindingFlags.Instance);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, new object[] { string.Empty });
                    }
                    tPropertyInfo.SetValue(this, tObject, null);
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                {
                    // int is default 0, not null!
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                {
                    // int is default 0, not null!
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                {
                    // int is default 0, not null!
                }
                if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                {
                    // int is default 0, not null!
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