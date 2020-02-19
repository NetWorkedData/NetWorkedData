﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:12
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
//using BasicToolBox;
using UnityEditor;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static string GetAccountsForConfig(NWDAccountEnvironment sEnvironment)
        //{
        //    string rReturn = string.Empty;
        //    List<string> tList = new List<string>();
        //    switch (sEnvironment)
        //    {
        //        case NWDAccountEnvironment.Dev:
        //            {
        //                foreach (NWDAccount tObject in NWDBasisHelper.BasisHelper<NWDAccount>().Datas)
        //                {
        //                    if (tObject.UseInEnvironment == NWDAccountEnvironment.Dev)
        //                    {
        //                        tList.Add(tObject.InternalKey + NWDConstants.kFieldSeparatorB + tObject.Email + NWDConstants.kFieldSeparatorC + tObject.Password + NWDConstants.kFieldSeparatorC + tObject.Reference);
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDAccountEnvironment.Preprod:
        //            {
        //                foreach (NWDAccount tObject in NWDBasisHelper.BasisHelper<NWDAccount>().Datas)
        //                {
        //                    if (tObject.UseInEnvironment == NWDAccountEnvironment.Preprod)
        //                    {
        //                        tList.Add(tObject.InternalKey + NWDConstants.kFieldSeparatorB + tObject.Email + NWDConstants.kFieldSeparatorC + tObject.Password + NWDConstants.kFieldSeparatorC + tObject.Reference);
        //                    }
        //                }
        //            }
        //            break;
        //    }
        //    rReturn = string.Join(NWDConstants.kFieldSeparatorA, tList.ToArray());
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        // TOO DANGEROUS FONCTION ...
        //public static NWDAccount Current()
        //{
        //    NWDAccount rAccount = null;
        //    string tAccountReference = CurrentReference();
        //    if (BasisHelper().DatasByReference.ContainsKey(tAccountReference))
        //    {
        //        rAccount = BasisHelper().DatasByReference[tAccountReference] as NWDAccount;
        //    }
        //    return rAccount;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string kInternalLogin; //TODO : change prefiuxe by p
        private string kInternalPassword; //TODO : change prefiuxe by p
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            NWDAccountSign[] tSigns = NWDAccountSign.GetCorporateDatasAssociated(Reference);
            int tRow = tSigns.Length;
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 20);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = Reference;
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tI++;
            //bool tAddEditor = true;
            //bool tAddPlayer = true;
            foreach (NWDAccountSign tSign in tSigns)
            {
                //if (tSign.SignType == NWDAccountSignType.DeviceID && tSign.SignAction== NWDAccountSignAction.Associated)
                //{
                //    tAddPlayer = false;
                //}
                //if (tSign.SignType == NWDAccountSignType.EditorID && tSign.SignAction== NWDAccountSignAction.Associated)
                //{
                //tAddEditor = true;
                //}
                bool tActive = true;
                List<string> tEnvironment = new List<string>();

                if (tSign.DevSync >= 0)
                {
                    tEnvironment.Add(NWDConstants.K_DEVELOPMENT_NAME);
                    if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment)
                    {
                        tActive = false;
                    }
                }
                if (tSign.PreprodSync >= 0)
                {
                    tEnvironment.Add(NWDConstants.K_PREPRODUCTION_NAME);
                    if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                    {
                        tActive = false;
                    }
                }
                if (tSign.ProdSync >= 0)
                {
                    tEnvironment.Add(NWDConstants.K_PRODUCTION_NAME);
                    if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                    {
                        tActive = false;
                    }
                }
                EditorGUI.BeginDisabledGroup(tActive);
                if (GUI.Button(tMatrix[0, tI], new GUIContent("Sign with " + tSign.SignType.ToString() + " " + string.Join(" ", tEnvironment), tSign.SignHash), NWDGUI.kMiniButtonStyle))
                {
                    NWDDataManager.SharedInstance().AddWebRequestSignIn(tSign.SignHash);
                }
                EditorGUI.EndDisabledGroup();
                if (GUI.Button(tMatrix[1, tI], new GUIContent("Edit", tSign.SignHash), NWDGUI.kMiniButtonStyle))
                {
                    NWDBasisHelper.BasisHelper<NWDAccountSign>().SetObjectInEdition(tSign);
                }
                tI++;
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Add sign"))
            {
                NWDAccountSign tSign = NWDBasisHelper.NewData<NWDAccountSign>();
                tSign.Account.SetReference(Reference);
                tSign.SaveData();
                NWDBasisHelper.BasisHelper<NWDAccountSign>().SetObjectInEdition(tSign);
                NWDBasisHelper.BasisHelper<NWDAccountSign>().ChangeScroolPositionToSelection();
            }
            tI++;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            EditorGUI.BeginDisabledGroup(!NWDAccount.AccountCanSignOut());
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Log Out"))
            {
                NWDDataManager.SharedInstance().AddWebRequestSignOut();
            }
            EditorGUI.EndDisabledGroup();
            tI++;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Test SignUp Fake Random"))
            {
                NWDDataManager.SharedInstance().AddWebRequestSignUp(NWDAccountSignType.Fake, NWDToolbox.RandomStringUnix(32), null, null);
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Test SignUp email password"))
            {
                NWDDataManager.SharedInstance().AddWebRequestSignUp(NWDAccountSignType.EmailPassword, NWDToolbox.RandomStringUnix(32), NWDAccountSign.GetRescueEmailHash(NWDAppEnvironment.SelectedEnvironment().RescueEmail), null);
            }
            tI++;
            /*
                //Debug.Log ("AddonEditor");
                float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);


            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;

            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;


            // Draw the interface addon for editor
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = Reference;
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;


            kInternalLogin = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Email to hash", kInternalLogin, tTextFieldStyle);
            tY += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            kInternalPassword = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Password to hash", kInternalPassword, tTextFieldStyle);
            tY += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            float tWidthTiers = (tWidth - NWDGUI.kFieldMarge * 1) / 2.0f;

            EditorGUI.BeginDisabledGroup(kInternalLogin == string.Empty || kInternalPassword == string.Empty || kInternalLogin == null || kInternalPassword == null);

            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                this.Email = NWESecurityTools.GenerateSha(kInternalLogin + tEnvironmentDev.SaltStart, NWESecurityShaTypeEnum.Sha1);
                this.Password = NWESecurityTools.GenerateSha(kInternalPassword + tEnvironmentDev.SaltEnd, NWESecurityShaTypeEnum.Sha1);
                this.InternalDescription = "Account for Dev test (" + kInternalLogin + " / " + kInternalPassword + ")";
                kInternalLogin = string.Empty;
                kInternalPassword = string.Empty;
                this.UpdateDataIfModified();
            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDGUI.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                this.Email = NWESecurityTools.GenerateSha(kInternalLogin + tEnvironmentPreprod.SaltStart, NWESecurityShaTypeEnum.Sha1);
                this.Password = NWESecurityTools.GenerateSha(kInternalPassword + tEnvironmentPreprod.SaltEnd, NWESecurityShaTypeEnum.Sha1);
                this.InternalDescription = "Account for Preprod test (" + kInternalLogin + " / " + kInternalPassword + ")";
                kInternalLogin = string.Empty;
                kInternalPassword = string.Empty;
                this.UpdateDataIfModified();
            }

            //			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDGUI.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //				this.Email = NWESecurityTools.generateSha (kInternalLogin + tEnvironmentProd.SaltStart, NWESecurityShaTypeEnum.Sha1);
            //				this.Password = NWESecurityTools.generateSha (kInternalPassword + tEnvironmentProd.SaltEnd, NWESecurityShaTypeEnum.Sha1);
            //				this.InternalDescription = "Account for Prod test (" + kInternalLogin + " / " + kInternalPassword + ")";
            //				kInternalLogin = "";
            //				kInternalPassword = "";
            //				this.UpdateMeIfModified();
            //			}

            tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            NWEOperationBlock tSuccessOrFailed = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
            {
                if (NWDAppEnvironmentSync.IsSharedInstance())
                {
                    NWDAppEnvironmentSync.SharedInstance().Repaint();
                };
                if (NWDAppEnvironmentSync.IsSharedInstance())
                {
                    NWDAppEnvironmentSync.SharedInstance().Repaint();
                };
            };

            EditorGUI.BeginDisabledGroup(kInternalLogin == string.Empty || kInternalLogin == null);

            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "Rescue dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Rescue", tSuccessOrFailed, null, null, null, tEnvironmentDev);
                sOperation.Action = "rescue";
                sOperation.EmailRescue = kInternalLogin;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDGUI.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "Rescue preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Rescue", tSuccessOrFailed, null, null, null, tEnvironmentPreprod);
                sOperation.Action = "rescue";
                sOperation.EmailRescue = kInternalLogin;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            //			if (GUI.Button (new Rect (tX + (tWidthTiers + NWDGUI.kFieldMarge) * 2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Rescue", null, null, null, null, tEnvironmentProd);
            //				sOperation.Action = "rescue";
            //				sOperation.Email = Email;
            //				NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (sOperation, true);
            //			}

            tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(Email == string.Empty || Password == string.Empty);
            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;

                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Sign-in", tSuccessOrFailed, tSuccessOrFailed, null, null, tEnvironmentDev);
                sOperation.Action = "signin";
                sOperation.EmailHash = Email;
                sOperation.PasswordHash = Password;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);

            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDGUI.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;

                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Sign-in", tSuccessOrFailed, tSuccessOrFailed, null, null, tEnvironmentPreprod);
                sOperation.Action = "signin";
                sOperation.EmailHash = Email;
                sOperation.PasswordHash = Password;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            //			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDGUI.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //
            //				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Sign-in", null, null, null, null, tEnvironmentProd);
            //				sOperation.Action = "signin";
            //				sOperation.EmailHash = Email;
            //				sOperation.PasswordHash = Password;
            //				NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (sOperation, true);
            //			}
            tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            EditorGUI.EndDisabledGroup();
            tY += NWDGUI.kFieldMarge;
            // Tool box of account connected objects
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Connected NWDBasis Objects", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            /*
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                var tMethodInfo = tType.GetMethod("NEW_FindDatas", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    object[] tObjectsArray = tMethodInfo.Invoke(null, new object[] { this.Reference }) as object[];
                    if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight),
                                   " Trash " + tObjectsArray.Length + " " + tType.Name,
                                   tMiniButtonStyle))
                    {
                        // TODO Trash this object
                        var tMethodTrashInfo = tType.GetMethod("TrashAllObjects", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodTrashInfo != null)
                        {
                            tMethodTrashInfo.Invoke(null, new object[] { this.Reference });
                        }
                    }
                    tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
            }*/
            ;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {

            NWDAccountSign[] tSigns = NWDAccountSign.GetCorporateDatasAssociated(Reference);
            int tRow = tSigns.Length;
            float tYadd = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, tRow + 20);
            return tYadd;

            ////Debug.Log ("AddonEditorHeight");
            //GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            //tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

            //GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            //tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

            //GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            //tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

            //float tY = NWDGUI.kFieldMarge;

            //tY += NWDGUI.kFieldMarge * 2;
            //tY += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += NWDGUI.kFieldMarge * 2;
            //tY += tLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            //tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            ///*foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            //{
            //    tY += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //}*/
            //return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif