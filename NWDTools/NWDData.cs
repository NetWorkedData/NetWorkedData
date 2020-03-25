//=====================================================================================================================
//
//  ideMobi copyright 2020
//
//  Date        2020-4-23 10:30:00
//  Author      Dolwen (Jérôme DEMYTTENAERE) 
//  Email       jerome.demyttenaere@gmail.com
//  Project     NetWorkedData for Unity3D
//
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
