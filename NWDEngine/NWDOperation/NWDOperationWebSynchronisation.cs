//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using SQLite4Unity3d;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationSpecial
    {
        None = 0,
        Special = 1,
        Upgrade = 2,
        Optimize = 3,
        Clean = 4,

        Pull = 5,

        Indexes = 8,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebSynchronisation : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList;
        public bool ForceSync = false;
        //public bool FlushTrash = false;
        public NWDOperationSpecial Special = NWDOperationSpecial.None;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebSynchronisation AddOperation(string sName,
                                                                   NWEOperationBlock sSuccessBlock = null,
                                                                   NWEOperationBlock sFailBlock = null,
                                                                   NWEOperationBlock sCancelBlock = null,
                                                                   NWEOperationBlock sProgressBlock = null,
                                                                   NWDAppEnvironment sEnvironment = null,
                                                                   List<Type> sTypeList = null,
                                                                   bool sForceSync = false,
                                                                   bool sPriority = false,
                                                                   NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
        {
            NWDOperationWebSynchronisation rReturn = Create(sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, sForceSync, sSpecial);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebSynchronisation Create(string sName,
                                                             NWEOperationBlock sSuccessBlock = null,
                                                             NWEOperationBlock sFailBlock = null,
                                                             NWEOperationBlock sCancelBlock = null,
                                                             NWEOperationBlock sProgressBlock = null,
                                                             NWDAppEnvironment sEnvironment = null,
                                                             List<Type> sTypeList = null,
                                                             bool sForceSync = false,
                                                             NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
        {
            NWDOperationWebSynchronisation rReturn = null;

            if (NWDDataManager.SharedInstance().DataLoaded() == true)
            {

                if (sName == null)
                {
                    sName = "UnNamed Web Operation Synchronisation";
                }

                if (sEnvironment == null)
                {
                    sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                }

                GameObject tGameObjectToSpawn = new GameObject(NWDToolbox.RandomStringUnix(16)+sName);
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
                tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebSynchronisation>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
                List<Type> tReturn = new List<Type>();
                //if (sTypeList == null)
                //{
                //    sTypeList = NWDDataManager.SharedInstance().mTypeSynchronizedList;
                //}
                    if (sTypeList != null)
                {
                    foreach (Type tType in sTypeList)
                    {
                        NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                        if (tHelper != null)
                        {
                            foreach (Type tR in tHelper.ClasseInThisSync())
                            {
                                if (tReturn.Contains(tR) == false)
                                {
                                    tReturn.Add(tR);
                                }
                            }
                        }
                        //MethodInfo tMethodInfo = NWDAliasMethod.GetMethod(tType, NWDConstants.M_ClasseInThisSync, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        //if (tMethodInfo != null)
                        //{
                        //    foreach (Type tR in tMethodInfo.Invoke(null, null) as List<Type>)
                        //    {
                        //        if (tReturn.Contains(tR) == false)
                        //        {
                        //            tReturn.Add(tR);
                        //        }
                        //    }
                        //}
                    }
                }
                //Debug.Log("New_ClasseInThisSync return : " + string.Join(" ", tReturn));
                rReturn.TypeList = tReturn;
                rReturn.ForceSync = sForceSync;
                rReturn.Special = sSpecial;
                rReturn.SecureData = sEnvironment.AllwaysSecureData;
                // TODO : Mettre dans le helper!!!!
                if (sTypeList != null)
                {
                    foreach (Type tType in sTypeList)
                    {
                        if (tType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length > 0)
                        {
                            rReturn.SecureData = true;
                            break;
                        }
                    }
                }
                rReturn.InitBlock(sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);
            }
            else
            {
                //NWEOperation tOperation = new NWEOperation();
                //NWDOperationResult tResult = new NWDOperationResult();
                //tOperation.QueueName = NWDAppEnvironment.SelectedEnvironment().Environment;
                sFailBlock(null, 1.0F, null);
                //Debug.LogWarning("SYNC NEED TO OPEN ALL ACCOUNT TABLES AND LOADED ALL DATAS!");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ServerFile()
        {
            return NWD.K_WS_FILE;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            Statut = NWEOperationState.ReStart;
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            // Not synchronize with temporray account
            bool tSync = true;
            //NWDAccountInfos tAccountInfos = NWDBasisHelper.GetCorporateFirstData<NWDAccountInfos>(Environment.PlayerAccountReference, null);
            NWDAppEnvironment.SetEnvironment(Environment);
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos == null)
            {
                tSync = false;
            }
            else
            {
                if (tAccountInfos.AccountType() == NWDAppEnvironmentPlayerStatut.Temporary)
                {
                    tSync = false;
                }
            }
            if (tSync == true)
            {
                Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas(ResultInfos, Environment, ForceSync, TypeList, Special);
                tData.Add(NWD.K_WEB_ACTION_KEY, NWD.K_WEB_ACTION_SYNC_KEY);
                Data = tData;
            }
            else
            {
                DataAddSecretDevicekey();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {
            NWDDataManager.SharedInstance().SynchronizationPullClassesDatas(ResultInfos, Environment, sData, TypeList, Special);
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================