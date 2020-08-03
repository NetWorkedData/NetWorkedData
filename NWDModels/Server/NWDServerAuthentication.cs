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
#endif
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using Renci.SshNet;
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerAuthentication
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Host;
        public int Port;
        public string Folder;
        public string User;
        public string Password;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerAuthentication(string sHost, int sPort, string sFolder, string sUser, string sPassword)
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
        public bool ConnectSFTP()
        {
            Debug.Log("NWDServerAuthentication ConnectSFTP()");
            bool rReturn = false;
            //NWDBenchmark.Start();
            if (SftpConnexion == null)
            {
                SftpConnexion = new SftpClient(Host, Port, User, Password);
                try
                {
                    SftpConnexion.Connect();
                }
                catch (Exception e)
                {
                    Debug.Log("An exception has been caught " + e.ToString());
                }
                if (SftpConnexion.IsConnected)
                {
                    Debug.Log("NWDServerAuthentication ConnectSFTP() connected");
                    rReturn = true;
                    SftpConnexion.BufferSize = 4 * 1024; // bypass Payload error large files
                }
                else
                {
                    Debug.Log("NWDServerAuthentication ConnectSFTP() not connected");
                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeconnectSFTP()
        {
            Debug.Log("NWDServerAuthentication DeconnectSFTP()");
            //NWDBenchmark.Start();
            if (SftpConnexion != null)
            {
                try
                {
                    SftpConnexion.Disconnect();
                    Debug.Log("NWDServerAuthentication ConnectSFTP() disconnect");
                }
                catch (Exception e)
                {
                    Debug.Log("An exception has been caught " + e.ToString());
                }
                try
                {
                    SftpConnexion.Dispose();
                    Debug.Log("NWDServerAuthentication ConnectSFTP() dispose");
                }
                catch (Exception e)
                {
                    Debug.Log("An exception has been caught " + e.ToString());
                }
            }
            SftpConnexion = null;
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetMaintenance(NWDAppEnvironment sEnvironment, bool sMaintenance)
        {
            //NWDBenchmark.Start();
            SetHTACCESS(sEnvironment, sMaintenance, false);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObsolete(NWDAppEnvironment sEnvironment, bool sObsolete)
        {
            //NWDBenchmark.Start();
            SetHTACCESS(sEnvironment, false, sObsolete);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetActivate(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            SetHTACCESS(sEnvironment, false, false);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetHTACCESS(NWDAppEnvironment sEnvironment, bool sMaintenance, bool sObsolete)
        {
            //NWDBenchmark.Start();
            // connect SFTP
            if (ConnectSFTP())
            {
                // prepare the destination
                string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                string tDestinationFolder = tWebServiceFolder + "/" + sEnvironment.Environment + "/";
                string tDestination = Folder + tDestinationFolder + NWD.K_HTACCESS;
                // delete existing file 
                if (SftpConnexion.Exists(tDestination))
                {
                    try
                    {
                        SftpConnexion.DeleteFile(tDestination);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("An exception has been caught " + e.ToString());
                    }
                }
                // rewrite one of new htaccess or nothing
                if (sMaintenance)
                {
                    byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\nRewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule . " + NWD.K_MAINTENANCE_PHP + "");
                    try
                    {
                        SftpConnexion.WriteAllBytes(tDestination, tBytes);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("An exception has been caught " + e.ToString());
                    }
                }
                else if (sObsolete)
                {
                    byte[] tBytes = Encoding.UTF8.GetBytes("RewriteEngine on\n#RewriteCond %{HTTP:ADMINHASH} ^$\nRewriteRule . " + NWD.K_OBSOLETE_PHP + "");
                    try
                    {
                        SftpConnexion.WriteAllBytes(tDestination, tBytes);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("An exception has been caught " + e.ToString());
                    }
                }
                //SFTPHost will close
                DeconnectSFTP();
            }
            //NWDBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void TestSFTPWrite()
        {
            //NWDBenchmark.Start();
            // prepare the destination
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tDestinationFolder = tWebServiceFolder + "/";
            string tDestination = Folder + tDestinationFolder + "test.hmtl";
            // delete existing file 
            if (SftpConnexion.Exists(tDestination))
            {
                try
                {
                    SftpConnexion.DeleteFile(tDestination);
                }
                catch (Exception e)
                {
                    Debug.Log("An exception has been caught " + e.ToString());
                }
            }
            // rewrite one of test
            byte[] tBytes = Encoding.UTF8.GetBytes("Test successed!");
            try
            {
                SftpConnexion.WriteAllBytes(tDestination, tBytes);
                Debug.Log("NWDServerAuthentication TestSFTPWrite() Test successed!");
            }
            catch (Exception e)
            {
                Debug.Log("An exception has been caught " + e.ToString());
            }
            //NWDBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void SendFiles(NWDAppEnvironment sEnvironment, string sFolder, string sAlternate, string[] sWSFiles)
        {
            //NWDBenchmark.Start();
            if (ConnectSFTP())
            {
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
                        try
                        {
                            SftpConnexion.CreateDirectory(Folder + tAssfolder);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("An exception has been caught " + e.ToString());
                        }
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
                        try
                        {
                            SftpConnexion.DeleteFile(tDestination);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("An exception has been caught " + e.ToString());
                        }
                    }
                    string tText = File.ReadAllText(tUploadFilePath);
                    byte[] tBytes = Encoding.UTF8.GetBytes(tText);
                    try
                    {
                        SftpConnexion.WriteAllBytes(tDestination, tBytes);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("An exception has been caught " + e.ToString());
                    }
                }
                DeconnectSFTP();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFile(NWDAppEnvironment sEnvironment, string sAlternate, string sFolder, string sWSFile)
        {
            //NWDBenchmark.Start();
            SendFiles(sEnvironment, sAlternate, sFolder, new string[] { sWSFile });
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendFolderAndFiles(List<string> sFolders, Dictionary<string, string> sFilesAndDatas, bool sFolderRecurssive = false)
        {
            //NWDBenchmark.Start();
            float tCountClass = sFolders.Count + sFilesAndDatas.Count + 1.0F;
            float tOperation = 1.0F;
            string tTitle = "Send file on server " + Host;
            if (ConnectSFTP())
            {
                EditorUtility.DisplayProgressBar(tTitle, "Open connection", tOperation++ / tCountClass);
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
                                    try
                                    {
                                        SftpConnexion.CreateDirectory(Folder + tAssfolder);
                                    }
                                    catch (Exception e)
                                    {
                                        Debug.Log("An exception has been caught " + e.ToString());
                                    }
                                }
                                tAssfolder = tAssfolder + "/";
                            }
                        }
                        else
                        {
                            if (!SftpConnexion.Exists(Folder + tFolder))
                            {
                                try
                                {
                                    SftpConnexion.CreateDirectory(Folder + tFolder);
                                }
                                catch (Exception e)
                                {
                                    Debug.Log("An exception has been caught " + e.ToString());
                                }
                            }
                        }
                    }
                }
                foreach (KeyValuePair<string, string> tFileAndData in sFilesAndDatas)
                {
                    if (SftpConnexion.Exists(Folder + tFileAndData.Key))
                    {
                        try
                        {
                            SftpConnexion.DeleteFile(Folder + tFileAndData.Key);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("An exception has been caught " + e.ToString());
                        }
                    }
                    byte[] tBytes = Encoding.UTF8.GetBytes(tFileAndData.Value);
                    try
                    {
                        SftpConnexion.WriteAllBytes(Folder + tFileAndData.Key, tBytes);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("An exception has been caught " + e.ToString());
                    }
                    EditorUtility.DisplayProgressBar(tTitle, "write file " + tFileAndData.Key, tOperation++ / tCountClass);
                }
                EditorUtility.DisplayProgressBar(tTitle, "Close connection", 1.0F);
                try
                {
                    DeconnectSFTP();
                }
                catch (Exception e)
                {
                    Debug.Log("An exception has been caught " + e.ToString());
                }
                EditorUtility.ClearProgressBar();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
