//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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