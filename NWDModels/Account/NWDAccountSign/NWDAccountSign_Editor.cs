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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        string Login;
        string Email;
        string Password;
        string Social;

        const int kEditorLign = 40;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override void AddonEditor(Rect sRect)
        {
            NWDGUILayout.Separator();

            bool tActive = false;
            List<string> tEnvironment = new List<string>();

            if (DevSync >= 0)
            {
                tEnvironment.Add(NWDConstants.K_DEVELOPMENT_NAME);
                if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment)
                {
                    tActive = true;
                }
            }
            if (PreprodSync >= 0)
            {
                tEnvironment.Add(NWDConstants.K_PREPRODUCTION_NAME);
                if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    tActive = true;
                }
            }
            if (ProdSync >= 0)
            {
                tEnvironment.Add(NWDConstants.K_PRODUCTION_NAME);
                if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                {
                    tActive = true;
                }
            }

            if (tActive == false)
            {
                GUILayout.Label("Not active in this environment");
            }
            else
            {

                GUILayout.Label("To associate with device", NWDGUI.kBoldLabelStyle);
                // start
                EditorGUI.BeginDisabledGroup(SignStatus == NWDAccountSignAction.Associated);
                if (GUILayout.Button("Associate Editor Secret Key", NWDGUI.kMiniButtonStyle))
                {
                    RegisterDeviceEditor();
                }
                if (GUILayout.Button("Associate Player Secret Key", NWDGUI.kMiniButtonStyle))
                {
                    RegisterDevicePlayer();
                }
                EditorGUI.EndDisabledGroup();
                // end

                NWDGUILayout.Separator();
                GUILayout.Label("To associate with standard account", NWDGUI.kBoldLabelStyle);
                Email = EditorGUILayout.TextField("Email", Email);
                Login = EditorGUILayout.TextField("Login", Login);
                Password = EditorGUILayout.TextField("Password", Password);

                EditorGUILayout.LabelField("futur SignHash l/p", GetSignLoginPasswordHash(Login, Password));
                EditorGUILayout.LabelField("futur SignHash e/p", GetSignEmailPasswordHash(Email, Password));
                EditorGUILayout.LabelField("futur RescueHash", GetRescueEmailHash(Email));
                EditorGUILayout.LabelField("futur LoginHash", GetLoginHash(Login));

                // start
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || SignStatus == NWDAccountSignAction.Associated);
                if (GUILayout.Button("Associate Email Password", NWDGUI.kMiniButtonStyle))
                {
                    RegisterEmailPassword(Email, Password);
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountSign) });
                }
                EditorGUI.EndDisabledGroup();
                // end

                // start
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Login) || SignStatus == NWDAccountSignAction.Associated);
                if (GUILayout.Button("Associate Login Password Email", NWDGUI.kMiniButtonStyle))
                {
                    RegisterEmailLoginPassword(Email, Login, Password);
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountSign) });
                }
                EditorGUI.EndDisabledGroup();
                // end

                // start
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(Email) || SignStatus != NWDAccountSignAction.Associated);
                if (GUILayout.Button("Rescue by Email", NWDGUI.kMiniButtonStyle))
                {
                    Debug.Log("email rescue hash = " + NWDAccountSign.GetRescueEmailHash(Email));
                    NWDDataManager.SharedInstance().AddWebRequestRescue(Email);
                }
                EditorGUI.EndDisabledGroup();
                // end

                // start
                EditorGUI.BeginDisabledGroup(SignStatus != NWDAccountSignAction.Associated);
                if (GUILayout.Button("Associate Delete", NWDGUI.kMiniButtonStyle))
                {
                    RegisterDelete();
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountSign) });
                }
                EditorGUI.EndDisabledGroup();
                // end 

                // start
                //EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(Login) ||
                //    string.IsNullOrEmpty(Password)  ||
                //    string.IsNullOrEmpty(Email)
                //    );
                if (GUILayout.Button("Select similar rescue", NWDGUI.kMiniButtonStyle))
                {
                    NWDAccountSign tSignFound = null;
                    string tRescueWanted = NWESecurityTools.GenerateSha(GetRescueEmailHash(Email) + NWDAppEnvironment.SelectedEnvironment().SaltEnd, NWESecurityShaTypeEnum.Sha1);
                    Debug.Log("tRescueWanted = " + tRescueWanted);
                    foreach (NWDAccountSign tAccountSign in BasisHelper().Datas)
                    {
                        if (string.IsNullOrEmpty(Email) == false)
                        {
                            if (tAccountSign.RescueHash == tRescueWanted)
                            {
                                tSignFound = tAccountSign;
                                break;
                            }
                        }
                    }
                    if (tSignFound != null)
                    {
                        Debug.Log("tRescueWanted = " + tRescueWanted + " find the reference : " + tSignFound.Reference + " for account " + tSignFound.Account.GetReference());
                        BasisHelper().SetObjectInEdition(tSignFound);
                    }
                    else
                    {
                        Debug.Log("tRescueWanted = " + tRescueWanted + " NOT FOUND REFERENCE ");
                    }

                }
                //EditorGUI.EndDisabledGroup();
                // end

                NWDGUILayout.Separator();
                GUILayout.Label("To associate with social token", NWDGUI.kBoldLabelStyle);
                Social = EditorGUILayout.TextField("Social", Social);

                // start
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(Social));
                if (GUILayout.Button("Associate FacebookID", NWDGUI.kMiniButtonStyle))
                {
                    RegisterSocialNetwork(Social, NWDAccountSignType.Facebook);
                }
                if (GUILayout.Button("Associate GoogleID", NWDGUI.kMiniButtonStyle))
                {
                    RegisterSocialNetwork(Social, NWDAccountSignType.Google);
                }
                EditorGUI.EndDisabledGroup();
                // end

                // start
                EditorGUI.BeginDisabledGroup(SignStatus != NWDAccountSignAction.Associated);
                if (GUILayout.Button("Associate Delete", NWDGUI.kMiniButtonStyle))
                {
                    RegisterDelete();
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountSign) });
                }
                EditorGUI.EndDisabledGroup();
                // end

                NWDGUILayout.Separator();
                GUILayout.Label("Test the sign", NWDGUI.kBoldLabelStyle);

                // start
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(SignHash));
                if (GUILayout.Button("Test Sign in", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataManager.SharedInstance().AddWebRequestSignIn(SignHash);
                }
                if (GUILayout.Button("Test Sign in with error", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataManager.SharedInstance().AddWebRequestSignIn(SignHash + "a");
                }
                if (GUILayout.Button("Sign OUT", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataManager.SharedInstance().AddWebRequestSignOut();
                }
                if (GUILayout.Button("Reset Session", NWDGUI.kMiniButtonStyle))
                {
                    NWDAppEnvironment.SelectedEnvironment().ResetPreferences();
                }
                //if (GUILayout.Button("Rescue", NWDGUI.kMiniButtonStyle))
                //{
                //    NWDDataManager.SharedInstance().AddWebRequestSignOut();
                //}
                EditorGUI.EndDisabledGroup();
                // end

                NWDGUILayout.Separator();
                GUILayout.Label("Hard or not hard sign ?", NWDGUI.kBoldLabelStyle);
                if (GUILayout.Button("Crack estimation", NWDGUI.kMiniButtonStyle))
                {
                    NWEPassAnalyseWindow.SharedInstance().AnalyzePassword(SignHash);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddonNodal(Rect sRect)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
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
