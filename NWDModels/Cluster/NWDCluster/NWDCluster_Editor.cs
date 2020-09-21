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
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCluster : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDCluster SelectClusterforEnvironment(NWDAppEnvironment sEnvironment, bool sShowAlert)
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
            if (rReturn == null)
            {
                if (sShowAlert == true)
                {
                    NWDAlert.Alert("Alert", "No active cluster for " + sEnvironment.Environment, "Cancel", null);
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
        //public override float AddonEditorHeight(float sWidth)
        //{
        //    // Height calculate for the interface addon for editor
        //    float tYadd = base.AddonEditorHeight(sWidth);
        //    tYadd += NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 100);
        //    return tYadd;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            return LayoutEditorHeight;
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
            NWDGUILayout.Separator();
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                if (GUILayout.Button("Credentials window"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                if (GUILayout.Button("Flush credentials"))
                {
                    NWDProjectCredentialsManagerContent.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                }
                NWDGUILayout.Separator();
                EditorGUILayout.HelpBox("Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                NWDGUILayout.Separator();
                GUILayout.Label("Services", NWDGUI.kBoldLabelStyle);
                if (Services == null)
                {
                    Services = new NWDReferencesListType<NWDServerServices>();
                }
                foreach (NWDServerServices tServices in Services.GetRawDatas())
                {
                    tServices.PropertiesPrevent();
                    GUILayout.Label(tServices.InternalKey);
                    GUILayout.Label("(" + tServices.InternalDescription + ")", NWDGUI.kItalicLabelStyle);
                    GUIContent tButtonTitle = null;
                    NWDServer tServer = tServices.Server.GetRawData();
                    if (tServer != null)
                    {
                        string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServices.User + " -p " + tServer.Port;
                        tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                        if (GUILayout.Button(tButtonTitle))
                        {
                            NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                        }
                        string tURL = "sftp://" + tServices.User + ":" + tServices.Secure_Password.Decrypt() + "@" + tServer.IP.GetValue() + ":" + tServer.Port + "/" + tServices.Folder;
                        tButtonTitle = new GUIContent("Try sftp directly", tURL);
                        if (GUILayout.Button(tButtonTitle))
                        {
                            Application.OpenURL(tURL);
                        }
                    }
                }
                NWDGUILayout.Separator();
                GUILayout.Label("Databases", NWDGUI.kBoldLabelStyle);
                if (DataBases == null)
                {
                    DataBases = new NWDReferencesListType<NWDServerDatas>();
                }
                foreach (NWDServerDatas tServices in DataBases.GetRawDatas())
                {
                    tServices.PropertiesPrevent();
                    GUILayout.Label(tServices.InternalKey);
                    GUILayout.Label("(" + tServices.InternalDescription + ")", NWDGUI.kItalicLabelStyle);
                    GUIContent tButtonTitle = null;
                    NWDServer tServer = tServices.Server.GetRawData();
                    if (tServer != null)
                    {
                        string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                        tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                        if (GUILayout.Button(tButtonTitle))
                        {
                            NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                        }
                    }
                    if (tServices.PhpMyAdmin == true)
                    {
                        //string tURL = "https://" + tServices.MySQLUser + ":" + tServices.MySQLPassword.GetValue() + "@" + tServices.MySQLIP.GetValue() + "/phpmyadmin/";
                        string tURL = "https://" + tServices.MySQLIP.GetValue() + "/phpmyadmin/?pma_username=" + tServices.MySQLUser + "&pma_password=" + tServices.MySQLSecurePassword.Decrypt() + "";
                        tButtonTitle = new GUIContent("Try PhpMyAdmin directly", tURL);
                        if (GUILayout.Button(tButtonTitle))
                        {
                            NWEClipboard.CopyToClipboard(tServices.MySQLSecurePassword.Decrypt());
                            Application.OpenURL(tURL);
                        }
                    }
                }
            }
            else
            {
                if (GUILayout.Button("Need credentials for actions"))
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
        public override float AddonNodalHeight(float sCardWidth)
        {
            float tYadd = base.AddonNodalHeight(sCardWidth);
            tYadd += 130;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddonNodal(Rect sRect)
        {
            base.AddonNodal(sRect);
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
