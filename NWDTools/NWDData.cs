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
    static public partial class NWDData
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void sessionBlock(bool result, NWDOperationResult infos);
        public static sessionBlock sessionBlockDelegate;
        public delegate void sessionBlockProgress(float progress);
        public static sessionBlockProgress sessionBlockProgressDelegate;
        public delegate void sessionBlockCancel();
        public static sessionBlockCancel sessionBlockCancelDelegate;
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync a list of Datas on server in active environment.
        /// </summary>
        /// <returns>
        /// Return a <c>NWDOperationWebSynchronisation</c>
        /// </returns>
        /// <param name="sTypeList">List of NWD or extended class to sync</param>
        public static NWDOperationWebSynchronisation Sync(List<Type> sTypeList)
        {
            return NWDDataManager.SharedInstance().AddWebRequestSynchronization(sTypeList);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync a list of Datas on server in active environment with block.
        /// </summary>
        /// <returns>
        /// Return a <c>NWDOperationWebSynchronisation</c>
        /// </returns>
        /// <param name="sTypeList">List of NWD or extended class to sync</param>
        public static NWDOperationWebSynchronisation SyncWithBlock(List<Type> sTypeList)
        {
            return NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(sTypeList, SessionSuccessBlock, SessionFailedBlock, SessionCancelBlock, SessionProgressBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync all Datas on server in active environment.
        /// </summary>
        /// <returns>
        /// Return a <c>NWDOperationWebSynchronisation</c>
        /// </returns>
        public static NWDOperationWebSynchronisation SyncAll()
        {
            return NWDDataManager.SharedInstance().AddWebRequestAllSynchronization();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sync all Datas on server in active environment with block.
        /// </summary>
        /// <returns>
        /// Return a <c>NWDOperationWebSynchronisation</c>
        /// </returns>
        public static NWDOperationWebSynchronisation SyncAllWithBlock()
        {
            return NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock(SessionSuccessBlock, SessionFailedBlock, SessionCancelBlock, SessionProgressBlock);
        }
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        static void SessionSuccessBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            NWDOperationResult tInfos = sInfos as NWDOperationResult;
            sessionBlockDelegate?.Invoke(true, tInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionFailedBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            NWDOperationResult tInfos = sInfos as NWDOperationResult;
            sessionBlockDelegate?.Invoke(false, tInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionCancelBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            sessionBlockCancelDelegate?.Invoke();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionProgressBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            sessionBlockProgressDelegate?.Invoke(sProgress);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
