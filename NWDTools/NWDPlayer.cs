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
    static public partial class NWDPlayer
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
        /// Return the <see cref="NWDAccountInfos"/> CurrentData for the player. <see cref="NWDAccountInfos"/> is not the <see cref="NWDUserInfos"/>
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos GetCurrentAccountInfos()
        {
            return NWDAccountInfos.CurrentData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool CheckSession()
        {
            bool tResult = false;
            NWDAccountSign[] tSigns = NWDBasisHelper.GetReachableDatas<NWDAccountSign>();
            foreach (NWDAccountSign k in tSigns)
            {
                if (k.SignStatus == NWDAccountSignAction.Associated)
                {
                    if (k.SignType != NWDAccountSignType.DeviceID &&
                        k.SignType != NWDAccountSignType.None)
                    {
                        tResult = true;
                        break;
                    }
                }
            }

            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddSocialNetwork(string sSocialID, NWDAccountSignType sSocialType)
        {
            NWDAccountSign.CreateAndRegisterSocialNetwork(sSocialID, sSocialType, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddEmailPassword(string sEmail, string sPassword)
        {
            NWDAccountSign.CreateAndRegisterEmailPassword(sEmail, sPassword, SessionSuccessBlock, SessionFailedBlock);
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
