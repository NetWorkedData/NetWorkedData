//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:53
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

using SQLite4Unity3d;
//using BasicToolBox;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        const int kUnitTestDC = 123456789;
        const string kDescriptionMark = "For UnitTest only";
        const string kDescriptionMarkNew  = "For UnitTest only -- new --";
        //-------------------------------------------------------------------------------------------------------------
        public static T PermanentData<T>(string sAddInternalKey, string sReference) where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.GetRawDataByReference<T>(NWDToolbox.AplhaNumericCleaner(sReference), true);
            if (rObject == null)
            {
                rObject = NWDBasisHelper.NewDataWithReference<T>(NWDToolbox.AplhaNumericCleaner(sReference));
                rObject.InternalKey = sAddInternalKey + " (UnitTest " + NWDToolbox.RandomStringCypher(8) + ")";
                rObject.InternalDescription = kDescriptionMarkNew;
                rObject.DevSync = -1;
                rObject.PreprodSync = -1;
                rObject.ProdSync = -1;
                //rObject.DC = kUnitTestDC; // because is permanent
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsNewPermanentData<T>(T sData) where T : NWDTypeClass, new()
        {
            bool rReturn = false;
            if (sData != null)
            {
                if (sData.InternalDescription == kDescriptionMarkNew)
                {
                    sData.InternalDescription = kDescriptionMark;
                    sData.UpdateData();
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T NewData<T>(string sAddInternalKey = "") where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.NewData<T>();
            rObject.InternalKey = sAddInternalKey + " (UnitTest " + NWDToolbox.RandomStringCypher(8) + ")";
            UnitTestData(rObject);
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T DuplicateData<T>(T sData) where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.DuplicateData<T>(sData);
            UnitTestData(rObject);
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void UnitTestData(NWDTypeClass sData)
        {
            sData.InternalDescription = kDescriptionMark;
            sData.DevSync = -1;
            sData.PreprodSync = -1;
            sData.ProdSync = -1;
            sData.DC = kUnitTestDC;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetUnitTestData(NWDTypeClass sData)
        {
            UnitTestData(sData);
            sData.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CleanUnitTests()
        {
            List<NWDTypeClass> tToDelete = new List<NWDTypeClass>();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                foreach (NWDTypeClass tObject in tHelper.Datas)
                {
                    if (tObject.DC == kUnitTestDC
                        && tObject.DevSync == -1
                        && tObject.PreprodSync == -1
                        && tObject.ProdSync == -1
                        && tObject.InternalDescription == kDescriptionMark)
                    {
                        tToDelete.Add(tObject);
                    }
                }
            }
            foreach (NWDTypeClass tObject in tToDelete)
            {
                tObject.DeleteData(NWDWritingMode.MainThread);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================