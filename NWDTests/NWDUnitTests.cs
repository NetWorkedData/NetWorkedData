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
using NetWorkedData;

#if UNITY_INCLUDE_TESTS
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        private static bool kActive;
        //-------------------------------------------------------------------------------------------------------------
        public static void EnableTest()
        {
            Debug.Log("NWDUnitTests EnableTest()");
            kActive = true;
            // active device
            EnableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DisableTest()
        {
            Debug.Log("NWDUnitTests DisableTest()");
            kActive = false;
            // restaure device
            DisableFakeDevice();
            // clean data? YES
            CleanUnitTests(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsTest()
        {
            return kActive;
        }
        //-------------------------------------------------------------------------------------------------------------
        //const int kUnitTestDC = 123456789;
        const string kDescriptionMark = "For UnitTest only";
        const string kDescriptionMarkNew = "For UnitTest only -- new --";
        //-------------------------------------------------------------------------------------------------------------
        public static T PermanentData<T>(string sAddInternalKey, string sReference) where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.GetRawDataByReference<T>(NWDToolbox.AplhaNumericCleaner(sReference), true);
            if (rObject == null)
            {
                Debug.Log("PermanentData create data with reference : " + NWDToolbox.AplhaNumericCleaner(sReference));
                rObject = NWDBasisHelper.NewDataWithReference<T>(NWDToolbox.AplhaNumericCleaner(sReference));
                rObject.InternalKey = sAddInternalKey + " (UnitTest " + NWDToolbox.RandomStringCypher(8) + ")";
                rObject.InternalDescription = kDescriptionMarkNew;
                rObject.DevSync = -1;
                rObject.PreprodSync = -1;
                rObject.ProdSync = -1;
                rObject.Tag = NWDBasisTag.UnitTestNotDelete;
                rObject.UpdateData();
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T NewLocalData<T>(string sAddInternalKey = "") where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.NewData<T>();
            rObject.InternalKey = sAddInternalKey + " (UnitTest " + NWDToolbox.RandomStringCypher(24) + ")";
            SetUnitTestData(rObject);
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void UnitTestData(NWDTypeClass sData)
        {
            sData.InternalDescription = kDescriptionMark;
            sData.DevSync = -1;
            sData.PreprodSync = -1;
            sData.ProdSync = -1;
            sData.Tag = NWDBasisTag.UnitTestToDelete;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetUnitTestData(NWDTypeClass sData)
        {
            UnitTestData(sData);
            sData.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        static int Step;
        //-------------------------------------------------------------------------------------------------------------
        public static void LogNewTest()
        {
            Step = 0;
            Debug.Log("==================== START NEW TEST ====================");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LogStep(string sString = null)
        {
            Step++;
            if (string.IsNullOrEmpty(sString))
            {
                sString = string.Empty;
            }
            Debug.Log("==================== STEP N°" + Step + " (" + sString + ") ====================");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Log(string sString)
        {
            Debug.Log("=== " + sString);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
