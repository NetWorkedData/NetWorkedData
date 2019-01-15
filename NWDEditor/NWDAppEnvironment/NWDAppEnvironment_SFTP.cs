//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
	{
        // SFTP
        //-------------------------------------------------------------------------------------------------------------
        SftpClient SftpConnexion;
        public string SFTPHost = string.Empty;
        public int SFTPPort = 22;
        public string SFTPFolder = string.Empty;
        public string SFTPUser = string.Empty;
        public string SFTPPassword = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public void ConnectSFTP()
        {
            if (SftpConnexion == null)
            {
                SftpConnexion = new SftpClient(SFTPHost, SFTPPort, SFTPUser, SFTPPassword);
                SftpConnexion.Connect();
                if (SftpConnexion.IsConnected)
                {

                    SftpConnexion.BufferSize = 4 * 1024; // bypass Payload error large files
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public void SendWS()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFileWS(string sAlternate, string sClassName)
        {
            SendFiles(sAlternate,
             "Environment/" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + "/Engine/Database/" + sClassName + "/",
             new string[] { "constants.php", "management.php", "synchronization.php" });
        }


        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(string sAlternate, string sFolder, string[] sWSFiles)
        {
            ConnectSFTP();
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder + "/" + sFolder;
            string[] tFolders = tDestinationFolder.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string tAssfolder = string.Empty;
            foreach (string tT in tFolders)
            {
                tAssfolder = tAssfolder + tT;
                Debug.Log(SFTPFolder + "" + tAssfolder);
                if (!SftpConnexion.Exists(SFTPFolder + tAssfolder))
                {
                    SftpConnexion.CreateDirectory(SFTPFolder + tAssfolder);
                }
                tAssfolder = tAssfolder + "/";
            }
            //SftpConnexion.CreateDirectory(SFTPFolder +tWebServiceFolder);
            string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
            string tServerRootFolder = tOwnerFolderServer + "/" + tWebServiceFolder + sAlternate + "/";
            string tUploadFile = (Application.dataPath + "..." + tServerRootFolder).Replace("Assets...Assets", "Assets");
            foreach (string sWSFile in sWSFiles)
            {
                string tUploadFilePath = tUploadFile + sFolder + sWSFile;
                string tDestination = SFTPFolder + tDestinationFolder + sWSFile;
                //Debug.Log("SFTP Upload file at  :" + tUploadFilePath);
                //Debug.Log("SFTP Upload file to  ::" + tDestination);
                using (var fileStream = new FileStream(tUploadFilePath, FileMode.Open))
                {
                    SftpConnexion.UploadFile(fileStream, tDestination, true, null);
                }
            }
            DeconnectSFTP();
        }


        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(string sAlternate, string sFolder, string sWSFile)
        {
            SendFiles(sAlternate, sFolder, new string[] { sWSFile });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            BTBBenchmark.Start();
            ConnectSFTP();
            // create web services folder
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder =  tWebServiceFolder;
            if (!SftpConnexion.Exists(SFTPFolder + tWebServiceFolder))
            {
                SftpConnexion.CreateDirectory(SFTPFolder + tWebServiceFolder);
            }
            // create web services folders and files
            foreach (string tFolder in sFolders)
            {
                if (string.IsNullOrEmpty(tFolder)==false)
                {
                    //Debug.Log("WRITE FOLDER : " + SFTPFolder + tWebServiceFolder + "/" + tFolder);
                    if (sFolderRecurssive == true)
                    {
                        string[] tFolders = tFolder.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        string tAssfolder = tWebServiceFolder + "/";
                        foreach (string tT in tFolders)
                        {
                            tAssfolder = tAssfolder + tT;
                           // Debug.Log(SFTPFolder + "" + tAssfolder);
                            if (!SftpConnexion.Exists(SFTPFolder + tAssfolder))
                            {
                                SftpConnexion.CreateDirectory(SFTPFolder + tAssfolder);
                            }
                            tAssfolder = tAssfolder + "/";
                        }
                    }
                    else
                    {
                        if (!SftpConnexion.Exists(SFTPFolder + tWebServiceFolder + "/" +tFolder))
                        {
                            SftpConnexion.CreateDirectory(SFTPFolder + tWebServiceFolder + "/" +tFolder);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string,string> tFileAndData in sFilesAndDatas)
            {
                //Debug.Log("SFTP Write file :" + SFTPFolder + tWebServiceFolder +"/"+ tFileAndData.Key);
                //SftpConnexion.WriteAllText(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key, tFileAndData.Value, Encoding.UTF8); =>>> bug dans lexecution du php ensuite!?
                byte[] tBytes = Encoding.UTF8.GetBytes(tFileAndData.Value);
                SftpConnexion.WriteAllBytes(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key, tBytes);
            }
            DeconnectSFTP();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeconnectSFTP()
        {
            if (SftpConnexion == null)
            {
                SftpConnexion.Disconnect();
                SftpConnexion.Dispose();
            }
            SftpConnexion = null;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif