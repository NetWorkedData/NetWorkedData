//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
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
            NWDAppEnvironmentSync.SharedInstance().OperationSynchroAllClasses(this, false, true, NWDOperationSpecial.Upgrade);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif