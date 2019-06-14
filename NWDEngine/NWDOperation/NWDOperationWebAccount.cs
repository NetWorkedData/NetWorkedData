// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:38
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SQLite4Unity3d;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationWebAccountAction : int
    {
        signin = 0,
        signout = 1,
        rescue = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebAccount : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccountAction Action;
        public string PasswordToken;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebAccount AddOperation(string sName,
                                                           BTBOperationBlock sSuccessBlock = null,
                                                           BTBOperationBlock sFailBlock = null,
                                                           BTBOperationBlock sCancelBlock = null,
                                                           BTBOperationBlock sProgressBlock = null,
                                                           NWDAppEnvironment sEnvironment = null, bool sPriority = false)
        {
            NWDOperationWebAccount rReturn = NWDOperationWebAccount.Create(sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebAccount Create(string sName,
                                                     BTBOperationBlock sSuccessBlock = null,
                                                     BTBOperationBlock sFailBlock = null,
                                                     BTBOperationBlock sCancelBlock = null,
                                                     BTBOperationBlock sProgressBlock = null,
                                                     NWDAppEnvironment sEnvironment = null)
        {
            NWDOperationWebAccount rReturn = null;
            if (NWDDataManager.SharedInstance().DataLoaded() == true)
            {
                if (sName == null)
                {
                    sName = "Web Operation Account";
                }
                if (sEnvironment == null)
                {
                    sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                }
                GameObject tGameObjectToSpawn = new GameObject(sName);
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebAccount>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
                rReturn.SecureData = true;
                rReturn.InitBlock(sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);
            }
            else
            {
                sFailBlock(null, 1.0F, null);
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ServerFile()
        {
            return NWD.K_AUTHENTIFICATION_PHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            //go in secure
            SecureData = true;
            // insert action
            if (Data.ContainsKey(NWD.K_WEB_ACTION_KEY))
            {
                Data[NWD.K_WEB_ACTION_KEY] = Action.ToString();
            }
            else
            {
                Data.Add(NWD.K_WEB_ACTION_KEY, Action.ToString());
            }
            // wich sign will be inserted?
            if (Action == NWDOperationWebAccountAction.signout)
            {
                // insert device key in data and go in secure
                DataAddSecetDevicekey();
            }
            else
            {
                // insert sign
                if (Data.ContainsKey(NWD.K_WEB_SIGN_Key))
                {
                    Data[NWD.K_WEB_SIGN_Key] = PasswordToken;
                }
                else
                {
                    Data.Add(NWD.K_WEB_SIGN_Key, PasswordToken);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {
            //Debug.Log ("NWDOperationWebAccount DataDownloadedCompute start");
            NWDDataManager.SharedInstance().SynchronizationPullClassesDatas(ResultInfos, Environment, sData, NWDDataManager.SharedInstance().mTypeAccountDependantList, NWDOperationSpecial.None);
            //Debug.Log ("NWDOperationWebAccount DataDownloadedCompute finish");
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================