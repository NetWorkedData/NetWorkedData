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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
//=====================================================================================================================
using NetWorkedData.NWDEditor;
using Renci.SshNet;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWebServiceState : int
    {
        Active,
        Maitenance,
        Obsolete,
        ToDelete,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<int, NWDWebServiceState> WebservicesStateByKey = new Dictionary<int, NWDWebServiceState>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDWebServiceState GetWebservicesStateByKey(int sWebService)
        {
            NWDWebServiceState rReturn = NWDWebServiceState.Active;
            if (WebservicesStateByKey.ContainsKey(sWebService))
            {
                rReturn = WebservicesStateByKey[sWebService];
            }
            else
            {
                WebservicesStateByKey.Add(sWebService, NWDWebServiceState.Active);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(int sWebService, bool sMaintenance)
        {
            NWDBenchmark.Start();
            if (WebservicesStateByKey.ContainsKey(sWebService))
            {
                WebservicesStateByKey[sWebService] = NWDWebServiceState.Maitenance;
            }
            else
            {
                WebservicesStateByKey.Add(sWebService, NWDWebServiceState.Maitenance);
            }
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetMaintenance(this, sWebService, sMaintenance);
            }
            if (sMaintenance == true)
            {
                NWDOperationWebhook.NewMessage(this, ":no_entry: " + sWebService + " Maintenance is on", WebHookType.Ugrade);
            }
            else
            {
                NWDOperationWebhook.NewMessage(this, ":+1: " + sWebService + " Maintenance is off", WebHookType.Ugrade);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(int sWebService, bool sObsolete)
        {
            NWDBenchmark.Start();
            if (WebservicesStateByKey.ContainsKey(sWebService))
            {
                WebservicesStateByKey[sWebService] = NWDWebServiceState.Obsolete;
            }
            else
            {
                WebservicesStateByKey.Add(sWebService, NWDWebServiceState.Obsolete);
            }
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetObsolete(this, sWebService, sObsolete);
            }
            if (sObsolete == true)
            {
                NWDOperationWebhook.NewMessage(this, ":no_entry: " + sWebService + " Obsolete is on", WebHookType.Ugrade);
            }
            else
            {
                NWDOperationWebhook.NewMessage(this, ":+1: " + sWebService + " Obsolete is off", WebHookType.Ugrade);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate(int sWebService)
        {
            NWDBenchmark.Start();
            if (WebservicesStateByKey.ContainsKey(sWebService))
            {
                WebservicesStateByKey[sWebService] = NWDWebServiceState.Active;
            }
            else
            {
                WebservicesStateByKey.Add(sWebService, NWDWebServiceState.Active);
            }
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetActivate(this, sWebService);
            }
            NWDOperationWebhook.NewMessage(this, ":+1: " + sWebService + " are actived", WebHookType.Ugrade);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(string sAlternate, string sFolder, string[] sWSFiles)
        {
            NWDBenchmark.Start();
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, sWSFiles);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(string sAlternate, string sFolder, string sWSFile, bool sAutoDeconnect = true)
        {
            NWDBenchmark.Start();
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, new string[] { sWSFile });
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            NWDBenchmark.Start();
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                Debug.Log("tConn : " + tConn.Host + " to foler root : " + tConn.Folder);
                tConn.SendFolderAndFiles(sFolders, sFilesAndDatas, sFolderRecurssive);
            }
            NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchroAllClasses(this, false, true, NWDOperationSpecial.Upgrade);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif
