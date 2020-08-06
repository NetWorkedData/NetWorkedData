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
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager  // TODO : put in static?
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool TestSaltMemorizationForAllClass()
        {
            bool rReturn = true;
            foreach (Type tType in ClassTypeList)
            {
                if (NWDBasisHelper.FindTypeInfos(tType).SaltValid == false)
                {
                    Debug.LogWarning(" Erreur in salt for " + NWDBasisHelper.FindTypeInfos(tType).ClassName);
                    rReturn = false;
                    break;
                }
            }
//            if (rReturn == false)
//            {
//#if UNITY_EDITOR
//                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().SelectedEnvironment ());
//#else
//                // no... ALERT USER ERROR IN APP DISTRIBUTION
//#endif
//            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DataLoaded() // loaded but not indexed// TODO rename DataAreLoaded
        {
            bool rReturn = true;
            if (EditorDatabaseLoaded == false || DeviceDatabaseLoaded == false)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreIndexed() // loaded and indexed
        {
            return DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreNotReady() 
        {
            return !DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasAreReady()
        {
            return DatasIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================