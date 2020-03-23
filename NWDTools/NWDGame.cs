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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDGame
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
        /// Return if NetWorkedData Engine is ready or not!
        /// </summary>
        /// <returns>
        /// Return true if data is ready and indexed
        /// </returns>
        public static bool Ready()
        {
            return NWDDataManager.SharedInstance().DatasAreReady();
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
