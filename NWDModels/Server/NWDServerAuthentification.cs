//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Renci.SshNet;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerAuthentification
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Host;
        public int Port;
        public string Folder;
        public string User;
        public string Password;
        public int BalanceLoad;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerAuthentification(string sHost, int sPort, string sFolder, string sUser, string sPassword, int BalanceLoad)
        {
            Host = NWDToolbox.CleanDNS(sHost);
            Port = sPort;
            Folder = sFolder.TrimEnd('/').TrimStart('/') + "/";
            User = sUser;
            Password = sPassword;
        }
        //-------------------------------------------------------------------------------------------------------------
        SftpClient SftpConnexion;
        //-------------------------------------------------------------------------------------------------------------
        public void ConnectSFTP()
        {
            //NWEBenchmark.Start();
            if (SftpConnexion == null)
            {
                SftpConnexion = new SftpClient(Host, Port, User, Password);
                SftpConnexion.Connect();
                if (SftpConnexion.IsConnected)
                {
                    SftpConnexion.BufferSize = 4 * 1024; // bypass Payload error large files
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeconnectSFTP()
        {
            //NWEBenchmark.Start();
            if (SftpConnexion == null)
            {
                SftpConnexion.Disconnect();
                SftpConnexion.Dispose();
            }
            SftpConnexion = null;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(NWDAppEnvironment sEnvironment, bool sMaintenance)
        {
            //NWEBenchmark.Start();
            SetHTACCESS(sEnvironment, sMaintenance, false);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(NWDAppEnvironment sEnvironment, bool sObsolete)
        {
            //NWEBenchmark.Start();
            SetHTACCESS(sEnvironment, false, sObsolete);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate(NWDAppEnvironment sEnvironment)
        {
            //NWEBenchmark.Start();
            SetHTACCESS(sEnvironment, false, false);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetHTACCESS(NWDAppEnvironment sEnvironment, bool sMaintenance, bool sObsolete)
        {
            //NWEBenchmark.Start();
            // connect SFTP
            ConnectSFTP();
            // prepare the destination
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder + "/" + sEnvironment.Environment + "/";
            string tDestination = Folder + tDestinationFolder + NWD.K_HTACCESS;
            // delete existing file 
            if (SftpConnexion.Exists(tDestination))
            {
                SftpConnexion.DeleteFile(tDestination);
            }
            // rewrite one of new htaccess or nothing
            if (sMaintenance)
            {
                byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\nRewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule . " + NWD.K_MAINTENANCE_PHP + "");
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
            }
            else if (sObsolete)
            {
                byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\n#RewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule . " + NWD.K_OBSOLETE_PHP + "");
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
            }
            //SFTPHost will close
            DeconnectSFTP();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(NWDAppEnvironment sEnvironment, string sFolder, string sAlternate, string[] sWSFiles)
        {
            //NWEBenchmark.Start();
            ConnectSFTP();
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder + "/" + sFolder;
            string[] tFolders = tDestinationFolder.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string tAssfolder = string.Empty;
            foreach (string tT in tFolders)
            {
                tAssfolder = tAssfolder + tT;
                Debug.Log(Folder + "" + tAssfolder);
                if (!SftpConnexion.Exists(Folder + tAssfolder))
                {
                    SftpConnexion.CreateDirectory(Folder + tAssfolder);
                }
                tAssfolder = tAssfolder + "/";
            }
            string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
            string tServerRootFolder = tOwnerFolderServer + "/" + tWebServiceFolder + sAlternate + "/";
            string tUploadFile = (Application.dataPath + "..." + tServerRootFolder).Replace("Assets...Assets", "Assets");
            foreach (string sWSFile in sWSFiles)
            {
                string tUploadFilePath = tUploadFile + sFolder + sWSFile;
                string tDestination = Folder + tDestinationFolder + sWSFile;
                if (SftpConnexion.Exists(tDestination))
                {
                    SftpConnexion.DeleteFile(tDestination);
                }
                string tText = File.ReadAllText(tUploadFilePath);
                byte[] tBytes = Encoding.UTF8.GetBytes(tText);
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
            }
            DeconnectSFTP();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(NWDAppEnvironment sEnvironment, string sAlternate, string sFolder, string sWSFile)
        {
            //NWEBenchmark.Start();
            SendFiles(sEnvironment, sAlternate, sFolder, new string[] { sWSFile });
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            //NWEBenchmark.Start();
            float tCountClass = sFolders.Count + sFilesAndDatas.Count + 1.0F;
            float tOperation = 1.0F;
            string tTitle = "Send file on server " + Host;
            EditorUtility.DisplayProgressBar(tTitle, "Open connection", tOperation++ / tCountClass);
            ConnectSFTP();
            foreach (string tFolder in sFolders)
            {
                EditorUtility.DisplayProgressBar(tTitle, "Create folder " + tFolder, tOperation++ / tCountClass);
                if (string.IsNullOrEmpty(tFolder) == false)
                {
                    if (sFolderRecurssive == true)
                    {
                        string[] tFolders = tFolder.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        string tAssfolder = string.Empty;
                        foreach (string tT in tFolders)
                        {
                            tAssfolder = tAssfolder + tT;
                            if (!SftpConnexion.Exists(Folder + tAssfolder))
                            {
                                SftpConnexion.CreateDirectory(Folder + tAssfolder);
                            }
                            tAssfolder = tAssfolder + "/";
                        }
                    }
                    else
                    {
                        if (!SftpConnexion.Exists(Folder + tFolder))
                        {
                            SftpConnexion.CreateDirectory(Folder + tFolder);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, string> tFileAndData in sFilesAndDatas)
            {
                if (SftpConnexion.Exists(Folder + tFileAndData.Key))
                {
                    SftpConnexion.DeleteFile(Folder + tFileAndData.Key);
                }
                byte[] tBytes = Encoding.UTF8.GetBytes(tFileAndData.Value);
                SftpConnexion.WriteAllBytes(Folder + tFileAndData.Key, tBytes);
                EditorUtility.DisplayProgressBar(tTitle, "write file " + tFileAndData.Key, tOperation++ / tCountClass);
            }
            EditorUtility.DisplayProgressBar(tTitle, "Close connection", 1.0F);
            DeconnectSFTP();
            EditorUtility.ClearProgressBar();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
