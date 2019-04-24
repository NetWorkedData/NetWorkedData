// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        private string InformationsString;
        //-------------------------------------------------------------------------------------------------------------
        public void InformationsUpdate()
        {
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            DateTime tDate = BTBDateHelper.ConvertFromTimestamp(tEnvironment.BuildTimestamp);
            string tInformations = "Environment : " + tEnvironment.Environment + "\n" +
                                   "BuildTimestamp : " + tEnvironment.BuildTimestamp + "\n" +
                                   "BuildTimestamp => " + tDate.ToString("yyyy/MM/dd HH:mm:ss") + "\n" +
                                   "BuildDate : " + tEnvironment.BuildDate + "\n" +
                                   "Web Service : " + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "\n" +
                                   "Account : " + tEnvironment.PlayerAccountReference + "\n" +
                                   "_______________\n";
            foreach (Type tType in mTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tInformations += " • " + tHelper.New_Informations();
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_Informations);
                //if (tMethodInfo != null)
                //{
                //    tInformations += " • " + tMethodInfo.Invoke(null, null);
                //}
                //else
                //{
                //    tInformations += tType.Name + " error \n";
                //}
            }
            InformationsString = tInformations;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Informations()
        {
            return InformationsString;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentLoaded()
        {
            return (float)ClassDataLoaded / (float)ClassExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentIndexed()
        {
            return (float)ClassIndexation / (float)ClassExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentEditorLoaded()
        {
            return (float)ClassEditorDataLoaded / (float)ClassEditorExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentAccountLoaded()
        {
            return (float)ClassAccountDataLoaded / (float)ClassAccountExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================