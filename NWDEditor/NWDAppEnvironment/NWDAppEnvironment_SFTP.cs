//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:23
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
        public string SFTPHost = string.Empty;
        public int SFTPPort = 22;
        public string SFTPFolder = string.Empty;
        public string SFTPUser = string.Empty;
        public string SFTPPassword = string.Empty;
        public int SFTPBalanceLoad = 50;
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(bool sMaintenance)
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetMaintenance(this, sMaintenance);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(bool sObsolete)
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetObsolete(this, sObsolete);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate()
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                tConn.SetActivate(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(string sAlternate, string sFolder, string[] sWSFiles)
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, sWSFiles);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(string sAlternate, string sFolder, string sWSFile, bool sAutoDeconnect = true)
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                tConn.SendFiles(this, sAlternate, sFolder, new string[] { sWSFile });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            foreach (NWDServerAuthentification tConn in NWDServerSFTP.GetAllConfigurationServerSFTP(this))
            {
                Debug.Log("tConn : " + tConn.Host + " " + tConn.Folder);
                tConn.SendFolderAndFiles(sFolders, sFilesAndDatas, sFolderRecurssive);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif