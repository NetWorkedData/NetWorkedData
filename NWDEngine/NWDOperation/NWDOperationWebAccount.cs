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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperationWebAccountAction
    {
        signin = 0,
        signout = 1,
        signup = 2,
        rescue = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebAccount : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDOperationWebAccountAction Action;
        public string PasswordToken; // rename SignInHash
        public string SignHash; // rename SignUpHash
        public string RescueHash;
        public string LoginHash;
        public string RescueEmail;
        public string RescueLanguage;
        public NWDAccountSignType SignType;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebAccount AddOperation(string sName,
                                                           NWEOperationBlock sSuccessBlock = null,
                                                           NWEOperationBlock sFailBlock = null,
                                                           NWEOperationBlock sCancelBlock = null,
                                                           NWEOperationBlock sProgressBlock = null,
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
                                                     NWEOperationBlock sSuccessBlock = null,
                                                     NWEOperationBlock sFailBlock = null,
                                                     NWEOperationBlock sCancelBlock = null,
                                                     NWEOperationBlock sProgressBlock = null,
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
                GameObject tGameObjectToSpawn = new GameObject(NWDToolbox.RandomStringUnix(16) + sName);
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
            return NWD.K_AUTHENTICATION_PHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ServerBase()
        {
            //Debug.Log("NWDOperationWebUnity ServerBase()");
            // use exceptionaly the default server
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            return Environment.GetConfigurationServerHTTPS() + "/" + tFolderWebService + "/" + Environment.Environment + "/" + ServerFile();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            //Statut = NWEOperationState.ReStart;
            //return true;
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            //go in secure
            SecureData = true;
            Dictionary<string, object> tDataNotAccount = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas(ResultInfos, Environment, false, NWDDataManager.SharedInstance().ClassSynchronizeList);
            Data = Data.Concat(tDataNotAccount).ToDictionary(x => x.Key, x => x.Value);
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
                DataAddSecretDevicekey();
            }
            else if (Action == NWDOperationWebAccountAction.signup)
            {
                if (Data.ContainsKey(NWD.K_WEB_SIGN_UP_TYPE_Key))
                {
                    Data[NWD.K_WEB_SIGN_UP_TYPE_Key] = SignType.ToLong();
                }
                else
                {
                    Data.Add(NWD.K_WEB_SIGN_UP_TYPE_Key, SignType.ToLong());
                }

                if (Data.ContainsKey(NWD.K_WEB_SIGN_UP_VALUE_Key))
                {
                    Data[NWD.K_WEB_SIGN_UP_VALUE_Key] = SignHash;
                }
                else
                {
                    Data.Add(NWD.K_WEB_SIGN_UP_VALUE_Key, SignHash);
                }
                if (string.IsNullOrEmpty(RescueHash))
                {
                    RescueHash = NWDAccountSign.K_NO_HASH;
                }
                if (Data.ContainsKey(NWD.K_WEB_SIGN_UP_RESCUE_Key))
                {
                    Data[NWD.K_WEB_SIGN_UP_RESCUE_Key] = RescueHash;
                }
                else
                {
                    Data.Add(NWD.K_WEB_SIGN_UP_RESCUE_Key, RescueHash);
                }
                if (string.IsNullOrEmpty(LoginHash))
                {
                    LoginHash = NWDAccountSign.K_NO_HASH;
                }
                if (Data.ContainsKey(NWD.K_WEB_SIGN_UP_LOGIN_Key))
                {
                    Data[NWD.K_WEB_SIGN_UP_LOGIN_Key] = LoginHash;
                }
                else
                {
                    Data.Add(NWD.K_WEB_SIGN_UP_LOGIN_Key, LoginHash);
                }
            }
            else if (Action == NWDOperationWebAccountAction.signin)
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
            else if (Action == NWDOperationWebAccountAction.rescue)
            {
                if (Data.ContainsKey(NWD.K_WEB_RESCUE_EMAIL_Key))
                {
                    Data[NWD.K_WEB_RESCUE_EMAIL_Key] = RescueEmail;
                }
                else
                {
                    Data.Add(NWD.K_WEB_RESCUE_EMAIL_Key, RescueEmail);
                }
                if (Data.ContainsKey(NWD.K_WEB_RESCUE_LANGUAGE_Key))
                {
                    Data[NWD.K_WEB_RESCUE_LANGUAGE_Key] = RescueLanguage;
                }
                else
                {
                    Data.Add(NWD.K_WEB_RESCUE_LANGUAGE_Key, RescueLanguage);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {
            //Debug.Log ("NWDOperationWebAccount DataDownloadedCompute start");
            NWDDataManager.SharedInstance().SynchronizationPullClassesDatas(ResultInfos, Environment, sData, NWDDataManager.SharedInstance().ClassAccountDependentList, NWDOperationSpecial.None);
            //Debug.Log ("NWDOperationWebAccount DataDownloadedCompute finish");
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================