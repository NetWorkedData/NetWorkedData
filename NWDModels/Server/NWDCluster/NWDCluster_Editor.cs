//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCluster : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDCluster SelectClusterforEnvironment(NWDAppEnvironment sEnvironment)
        {
            NWDCluster rReturn = null;
            NWDEnvironmentType tEnvironmentType = NWDEnvironmentType.Dev;
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tEnvironmentType = NWDEnvironmentType.Dev;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tEnvironmentType = NWDEnvironmentType.Preprod;
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tEnvironmentType = NWDEnvironmentType.Prod;
            }
            foreach (NWDCluster tCluster in NWDBasisHelper.GetReachableDatas<NWDCluster>())
            {
                switch (tEnvironmentType)
                {
                    case NWDEnvironmentType.Dev:
                        {
                            if (tCluster.Dev == true)
                            {
                                rReturn = tCluster;
                            }
                        }
                        break;
                    case NWDEnvironmentType.Preprod:
                        {
                            if (tCluster.Preprod == true)
                            {
                                rReturn = tCluster;
                            }
                        }
                        break;
                    case NWDEnvironmentType.Prod:
                        {
                            if (tCluster.Prod == true)
                            {
                                rReturn = tCluster;
                            }
                        }
                        break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            // do base
            bool tNeedBeUpdate = base.AddonEdited(sNeedBeUpdate);
            if (tNeedBeUpdate == true)
            {
                // do something
            }
            return tNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            float tYadd = base.AddonEditorHeight(sWidth);
            tYadd += NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 100);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sRect">S in rect.</param>
        public override void AddonEditor(Rect sRect)
        {
            base.AddonEditor(sRect);
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 100);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Credentials window"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Flush credentials"))
                {
                    NWDProjectCredentialsManager.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                }
                tI++;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                EditorGUI.HelpBox(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                tI += 2;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Services", NWDGUI.kBoldLabelStyle);
                tI++;
                if (Services == null)
                {
                    Services = new NWDReferencesListType<NWDServerServices>();
                }
                foreach (NWDServerServices tServices in Services.GetRawDatas())
                {
                    tServices.PropertiesPrevent();
                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tServices.InternalKey);
                    tI++;
                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "(" + tServices.InternalDescription + ")", NWDGUI.kItalicLabelStyle);
                    tI++;
                    GUIContent tButtonTitle = null;
                    NWDServer tServer = tServices.Server.GetRawData();
                    if (tServer != null)
                    {
                        string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServices.User + " -p " + tServer.Port;
                        tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                        }
                        tI++;
                        string tURL = "sftp://" + tServices.User + ":" + tServices.Secure_Password.Decrypt() + "@" + tServer.IP.GetValue() + ":" + tServer.Port + "/" + tServices.Folder;
                        tButtonTitle = new GUIContent("Try sftp directly", tURL);
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            Application.OpenURL(tURL);
                        }
                        tI++;
                    }
                }
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Databases", NWDGUI.kBoldLabelStyle);
                tI++;
                if (DataBases == null)
                {
                    DataBases = new NWDReferencesListType<NWDServerDatas>();
                }
                foreach (NWDServerDatas tServices in DataBases.GetRawDatas())
                {
                    tServices.PropertiesPrevent();
                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tServices.InternalKey);
                    tI++;
                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "(" + tServices.InternalDescription + ")", NWDGUI.kItalicLabelStyle);
                    tI++;
                    GUIContent tButtonTitle = null;
                    NWDServer tServer = tServices.Server.GetRawData();
                    if (tServer != null)
                    {
                        string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                        tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                        }
                        tI++;
                    }
                    if (tServices.PhpMyAdmin == true)
                    {
                        //string tURL = "https://" + tServices.MySQLUser + ":" + tServices.MySQLPassword.GetValue() + "@" + tServices.MySQLIP.GetValue() + "/phpmyadmin/";
                        string tURL = "https://" + tServices.MySQLIP.GetValue() + "/phpmyadmin/?pma_username=" + tServices.MySQLUser + "&pma_password=" + tServices.MySQLSecurePassword.Decrypt() + "";
                        tButtonTitle = new GUIContent("Try PhpMyAdmin directly", tURL);
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            NWEClipboard.CopyToClipboard(tServices.MySQLSecurePassword.Decrypt());
                            Application.OpenURL(tURL);
                        }
                        tI++;
                    }
                }
            }
            else
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Need credentials for actions"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            float tYadd = base.AddOnNodeDrawHeight(sCardWidth);
            tYadd += 130;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect)
        {
            base.AddOnNodeDraw(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = base.AddonErrorFound();
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif