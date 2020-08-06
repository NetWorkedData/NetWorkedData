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
using System.Reflection;
using UnityEngine;

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
            DateTime tDate = NWEDateHelper.ConvertFromTimestamp(tEnvironment.BuildTimestamp);
            string tInformations = "Environment : " + tEnvironment.Environment + "\n" +
                                   "BuildTimestamp : " + tEnvironment.BuildTimestamp + "\n" +
                                   "BuildTimestamp => " + tDate.ToString("yyyy/MM/dd HH:mm:ss") + "\n" +
                                   "BuildDate : " + tEnvironment.BuildDate + "\n" +
                                   "Web Service : " + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "\n" +
                                   "Account : " + tEnvironment.GetAccountReference() + "\n" +
                                   "_______________\n";
            foreach (Type tType in ClassTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tInformations += " • " + tHelper.Informations();
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
            return (float)ClassNumberLoaded / (float)ClassNumberExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentIndexed()
        {
            return (float)ClassNumberIndexation / (float)ClassNumberExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentEditorLoaded()
        {
            return (float)ClassInEditorDatabaseNumberLoaded / (float)ClassInEditorDatabaseRumberExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float PurcentAccountLoaded()
        {
            return (float)ClassInDeviceDatabaseNumberLoaded / (float)ClassInDeviceDatabaseNumberExpected;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================