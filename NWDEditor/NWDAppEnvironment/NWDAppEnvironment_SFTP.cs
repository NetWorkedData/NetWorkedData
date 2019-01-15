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
             "Environment/" + Environment + "/Engine/Database/" + sClassName + "/",
             new string[] { "constants.php", "management.php", "synchronization.php" });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(bool sMaintenance)
        {
            SetHTACCESS(sMaintenance, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(bool sObsolete)
        {
            SetHTACCESS(false, sObsolete);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate()
        {
            SetHTACCESS(false, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetHTACCESS(bool sMaintenance, bool sObsolete)
        {
            // connect SFTP
            ConnectSFTP();
            // prepare the destination
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder + "/";
            string tDestination = SFTPFolder + tDestinationFolder + ".htaccess";
            // delete existing file 
            if (SftpConnexion.Exists(tDestination))
            {
                SftpConnexion.DeleteFile(tDestination);
            }
            // rewrite one of new htaccess or nothing
            if (sMaintenance)
            {
                byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\nRewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule ([a-zA-Z0-9\\_]+)\\/(.*) ./maintenance.php");
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
            }
            else if (sObsolete)
            {
                byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\n#RewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule ([a-zA-Z0-9\\_]+)\\/(.*) ./obsolete.php");
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
            }
            //SFTPHost will close
            DeconnectSFTP();
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
                //Debug.Log("SFTP Upload file at \n" + tUploadFilePath + "\nto \n" + tDestination);
                if (SftpConnexion.Exists(tDestination))
                {
                    SftpConnexion.DeleteFile(tDestination);
                }
                //FileStream tFileStream = new FileStream(tUploadFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                //SftpConnexion.UploadFile(tFileStream, tDestination, true, null);
                string tText = File.ReadAllText(tUploadFilePath);
                //Debug.Log("tText = " + tText);
                byte[] tBytes = Encoding.UTF8.GetBytes(tText);
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
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
            //BTBBenchmark.Start();
            ConnectSFTP();
            // create web services folder
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder;
            if (!SftpConnexion.Exists(SFTPFolder + tWebServiceFolder))
            {
                SftpConnexion.CreateDirectory(SFTPFolder + tWebServiceFolder);
            }
            // create web services folders and files
            foreach (string tFolder in sFolders)
            {
                if (string.IsNullOrEmpty(tFolder) == false)
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
                        if (!SftpConnexion.Exists(SFTPFolder + tWebServiceFolder + "/" + tFolder))
                        {
                            SftpConnexion.CreateDirectory(SFTPFolder + tWebServiceFolder + "/" + tFolder);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, string> tFileAndData in sFilesAndDatas)
            {
                //Debug.Log("SFTP Write file :" + SFTPFolder + tWebServiceFolder +"/"+ tFileAndData.Key);
                //SftpConnexion.WriteAllText(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key, tFileAndData.Value, Encoding.UTF8); =>>> bug dans lexecution du php ensuite!?
                if (SftpConnexion.Exists(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key))
                {
                    SftpConnexion.DeleteFile(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key);
                }
                byte[] tBytes = Encoding.UTF8.GetBytes(tFileAndData.Value);
                SftpConnexion.WriteAllBytes(SFTPFolder + tWebServiceFolder + "/" + tFileAndData.Key, tBytes);
            }
            DeconnectSFTP();
            //BTBBenchmark.Finish();
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