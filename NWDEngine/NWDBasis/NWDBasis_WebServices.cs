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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public void DevSyncActive(bool sLock)
        {
            if (sLock == false)
            {
                if (DevSync == 0)
                {
                    DevSync = -1;
                }
                else if (DevSync == 1)
                {
                    DevSync = -2;
                }
                else
                {
                    DevSync = -Math.Abs(DevSync);
                }
            }
            else
            {
                if (DevSync == -1)
                {
                    DevSync = 0;
                }
                else if (DevSync == -2)
                {
                    DevSync = 1;
                }
                else
                {
                    DevSync = Math.Abs(DevSync);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PreprodSyncActive(bool sLock)
        {
            if (sLock == false)
            {
                if (PreprodSync == 0)
                {
                    PreprodSync = -1;
                }
                else if (PreprodSync == 1)
                {
                    PreprodSync = -2;
                }
                else
                {
                    PreprodSync = -Math.Abs(PreprodSync);
                }
            }
            else
            {
                if (PreprodSync == -1)
                {
                    PreprodSync = 0;
                }
                else if (PreprodSync == -2)
                {
                    PreprodSync = 1;
                }
                else
                {
                    PreprodSync = Math.Abs(PreprodSync);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ProdSyncActive(bool sLock)
        {
            if (sLock == false)
            {
                if (ProdSync == 0)
                {
                    ProdSync = -1;
                }
                else if (ProdSync == 1)
                {
                    ProdSync = -2;
                }
                else
                {
                    ProdSync = -Math.Abs(ProdSync);
                }
            }
            else
            {
                if (ProdSync == -1)
                {
                    ProdSync = 0;
                }
                else if (ProdSync == -2)
                {
                    ProdSync = 1;
                }
                else
                {
                    ProdSync = Math.Abs(ProdSync);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsSynchronized()
        {
            if (SynchronizeStamp() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int SynchronizeStamp()
        {
            int tD = 0;
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement())
            {
                tD = DevSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement())
            {
                tD = PreprodSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsProdEnvironement())
            {
                tD = ProdSync;
            }
                return tD;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override int WebModelToUse()
        {
            int tWebBuildUsed = BasisHelper().LastWebBuild;
            return tWebBuildUsed;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis SynchronizationInsertInBase(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string[] sDataArray)
        //{
        //    return BasisHelper().New_SynchronizationInsertInBase( sInfos,  sEnvironment, sDataArray) as NWDBasis;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis SynchronizationTryToUse(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
        //{
        //    return BasisHelper().SynchronizationTryToUse( sInfos,  sEnvironment,  sData,  sForceToUse) as NWDBasis;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void SynchronizationFromWebService(NWEOperationBlock sSuccessBlock = null,
        // NWEOperationBlock sErrorBlock = null,
        // NWEOperationBlock sCancelBlock = null,
        // NWEOperationBlock sProgressBlock = null,
        // bool sForce = false,
        // bool sPriority = false)
        //{
        //    BasisHelper().SynchronizationFromWebService(sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sForce, sPriority);
        //}
        //-------------------------------------------------------------------------------------------------------------
//        public static bool PullFromWebService(NWDAppEnvironment sEnvironment)
//        {
//            return BasisHelper().PullFromWebService(sEnvironment);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static bool PullFromWebServiceForce(NWDAppEnvironment sEnvironment)
//        {
//            return BasisHelper().PullFromWebServiceForce(sEnvironment);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
//        {
//            return BasisHelper().SynchronizationFromWebServiceForce(sEnvironment);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static bool SynchronizationFromWebService(NWDAppEnvironment sEnvironment)
//        {
//            return BasisHelper().SynchronizationFromWebService(sEnvironment);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
//        {
//            return BasisHelper().SynchronizationFromWebServiceClean(sEnvironment);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//#if UNITY_EDITOR
//        public static bool SynchronizationFromWebServiceSpecial(NWDAppEnvironment sEnvironment, NWDOperationSpecial sSpecial)
//        {
//            return BasisHelper().SynchronizationFromWebServiceSpecial( sEnvironment,  sSpecial);
//        }
//#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================