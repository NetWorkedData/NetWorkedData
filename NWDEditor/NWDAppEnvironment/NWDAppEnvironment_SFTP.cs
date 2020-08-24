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
    public partial class NWDAppEnvironment
    {
        //public string SFTPHost = string.Empty;
        //public int SFTPPort = 22;
        //public string SFTPFolder = string.Empty;
        //public string SFTPUser = string.Empty;
        //public string SFTPPassword = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(bool sMaintenance)
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetMaintenance(this, sMaintenance);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(bool sObsolete)
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetObsolete(this, sObsolete);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate()
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetActivate(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(string sAlternate, string sFolder, string[] sWSFiles)
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, sWSFiles);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(string sAlternate, string sFolder, string sWSFile, bool sAutoDeconnect = true)
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, new string[] { sWSFile });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            foreach (NWDServerAuthentication tConn in NWDServerServices.GetAllConfigurationServerSFTP(this))
            {
                Debug.Log("tConn : " + tConn.Host + " to foler root : " + tConn.Folder);
                tConn.SendFolderAndFiles(sFolders, sFilesAndDatas, sFolderRecurssive);
            }
            NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchroAllClasses(this, false, true, NWDOperationSpecial.Upgrade);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif