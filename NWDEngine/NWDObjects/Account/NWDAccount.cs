﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAccounTest
    {
        public string Reference;
        public string InternalKey;
        public string EmailHash;
        public string PasswordHash;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountEnvironment : int
    {
        InGame = 0, // player state (Prod)
        Dev = 1,    // dev state
        Preprod = 2, // preprod state
        //Prod = 3, NEVER COPY ACCOUNT IN PROD !!!!
        None = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAccount. It's an abstract class ... not usable in play mode.
    /// </summary>
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("ACC")]
    [NWDClassDescriptionAttribute("Account descriptions Class")]
    [NWDClassMenuNameAttribute("Account")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis<NWDAccount>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Account statut")]
        [NWDTooltips("The statut of this account in process of test (normal and default is 'InGame')")]
        public NWDAccountEnvironment UseInEnvironment
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDGroupSeparator]
        [NWDGroupStart("Account sign-in/up")]
        /// <summary>
        /// Gets or sets the SecretKey to restaure anonymous account.
        /// </summary>
        /// <value>The login.</value>
        [NWDTooltips("The secret key to re-authentify the anonyme account")]
        public string SecretKey
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The login is an email.</value>
        [NWDTooltips("Hash of email for the appropriate environment")]
        public string Email
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [NWDTooltips("Hash of password for the appropriate environment")]
        public string Password
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Facebook Identifiant.
        /// </summary>
        /// <value>The facebook I.</value>
        [NWDTooltips("FacebookID")]
        public string FacebookID
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the Google Identifiant.
        /// </summary>
        /// <value>The google I.</value>
        [NWDTooltips("GoogleID")]
        public string GoogleID
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDGroupSeparator]
        [NWDGroupStart("Account push notification")]
        /// <summary>
        /// Gets or sets the apple notification token for message.
        /// </summary>
        /// <value>The apple notification token.</value>
        [Obsolete("See NWDAccountInfos")]
        public string AppleNotificationToken
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the google notification token for message.
        /// </summary>
        /// <value>The google notification token.</value>
        [Obsolete("See NWDAccountInfos")]
        public string GoogleNotificationToken
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDGroupSeparator]
        [NWDGroupStart("Account ban")]
        /// <summary>
        /// Gets or sets a value indicating whether this account <see cref="NWDEditor.NWDAccount"/> is banned.
        /// </summary>
        /// <value><c>true</c> if ban; otherwise, <c>false</c>.</value>
        [NWDTooltips("If account is ban set the unix timestamp of ban's date")]
        public int Ban
        {
            get; set;
        }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount()
        {
            //Debug.Log("NWDAccount Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccount Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static string GetCurrentAccountReference()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetCurrentAnonymousAccountReference()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().AnonymousPlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static string GetAccountsForConfig(NWDAccountEnvironment sEnvironment)
        {
            string rReturn = "";
            List<string> tList = new List<string>();
            switch (sEnvironment)
            {
                case NWDAccountEnvironment.Dev:
                    {
                        foreach (NWDAccount tObject in NWDAccount.Datas().Datas)
                        {
                            if (tObject.UseInEnvironment == NWDAccountEnvironment.Dev)
                            {
                                tList.Add(tObject.InternalKey + NWDConstants.kFieldSeparatorB + tObject.Email + NWDConstants.kFieldSeparatorC + tObject.Password + NWDConstants.kFieldSeparatorC + tObject.Reference);
                            }
                        }
                    }
                    break;
                case NWDAccountEnvironment.Preprod:
                    {
                        foreach (NWDAccount tObject in NWDAccount.Datas().Datas)
                        {
                            if (tObject.UseInEnvironment == NWDAccountEnvironment.Preprod)
                            {
                                tList.Add(tObject.InternalKey + NWDConstants.kFieldSeparatorB + tObject.Email + NWDConstants.kFieldSeparatorC + tObject.Password + NWDConstants.kFieldSeparatorC + tObject.Reference);
                            }
                        }
                    }
                    break;
            }
            rReturn = string.Join(NWDConstants.kFieldSeparatorA, tList.ToArray());
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDAccounTest> GetTestsAccounts()
        {
            List<NWDAccounTest> rReturn = new List<NWDAccounTest>();
            string tValue = NWDAppConfiguration.SharedInstance().SelectedEnvironment().AccountsForTests;
            if (tValue != null && tValue != "")
            {
                string[] tValueArray = tValue.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tValueArrayLine in tValueArray)
                {
                    string[] tLineValue = tValueArrayLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        string tAccountKey = tLineValue[0];
                        string tText = tLineValue[1];
                        string[] tInfos = tText.Split(new string[] { NWDConstants.kFieldSeparatorC }, StringSplitOptions.RemoveEmptyEntries);
                        if (tInfos.Length == 3)
                        {
                            NWDAccounTest tAccount = new NWDAccounTest();
                            tAccount.InternalKey = tAccountKey;
                            tAccount.EmailHash = tInfos[0];
                            tAccount.PasswordHash = tInfos[1];
                            tAccount.Reference = tInfos[2];
                            rReturn.Add(tAccount);
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            UseInEnvironment = NWDAccountEnvironment.InGame;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetAccountReference()
        {
            return Reference;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccount CurrentAccount()
        {
            NWDAccount rAccount = null;
            string tAccountReference = GetCurrentAccountReference();
            if (Datas().DatasByReference.ContainsKey(tAccountReference))
            {
                rAccount = Datas().DatasByReference[tAccountReference] as NWDAccount;
            }
            return rAccount;
        }
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
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
        public override float AddonEditor(Rect sInRect)
        {
            //Debug.Log ("AddonEditor");
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);


            EditorGUI.DrawRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1), NWDConstants.kRowColorLine);
            tY += NWDConstants.kFieldMarge * 2;

            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;


            // Draw the interface addon for editor
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDDatas.FindTypeInfos(tType).m_SearchAccount = Reference;
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;


            kInternalLogin = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Email to hash", kInternalLogin, tTextFieldStyle);
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            kInternalPassword = EditorGUI.TextField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Password to hash", kInternalPassword, tTextFieldStyle);
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            float tWidthTiers = (tWidth - NWDConstants.kFieldMarge * 1) / 2.0f;

            EditorGUI.BeginDisabledGroup(kInternalLogin == "" || kInternalPassword == "" || kInternalLogin == null || kInternalPassword == null);

            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                this.Email = BTBSecurityTools.GenerateSha(kInternalLogin + tEnvironmentDev.SaltStart, BTBSecurityShaTypeEnum.Sha1);
                this.Password = BTBSecurityTools.GenerateSha(kInternalPassword + tEnvironmentDev.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
                this.InternalDescription = "Account for Dev test (" + kInternalLogin + " / " + kInternalPassword + ")";
                kInternalLogin = "";
                kInternalPassword = "";
                this.UpdateDataIfModified();
            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                this.Email = BTBSecurityTools.GenerateSha(kInternalLogin + tEnvironmentPreprod.SaltStart, BTBSecurityShaTypeEnum.Sha1);
                this.Password = BTBSecurityTools.GenerateSha(kInternalPassword + tEnvironmentPreprod.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
                this.InternalDescription = "Account for Preprod test (" + kInternalLogin + " / " + kInternalPassword + ")";
                kInternalLogin = "";
                kInternalPassword = "";
                this.UpdateDataIfModified();
            }

            //			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDConstants.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //				this.Email = BTBSecurityTools.generateSha (kInternalLogin + tEnvironmentProd.SaltStart, BTBSecurityShaTypeEnum.Sha1);
            //				this.Password = BTBSecurityTools.generateSha (kInternalPassword + tEnvironmentProd.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
            //				this.InternalDescription = "Account for Prod test (" + kInternalLogin + " / " + kInternalPassword + ")";
            //				kInternalLogin = "";
            //				kInternalPassword = "";
            //				this.UpdateMeIfModified();
            //			}

            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            BTBOperationBlock tSuccessOrFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (NWDEditorMenu.kNWDAppEnvironmentChooser != null)
                {
                    NWDEditorMenu.kNWDAppEnvironmentChooser.Repaint();
                };
                if (NWDEditorMenu.kNWDAppEnvironmentSync != null)
                {
                    NWDEditorMenu.kNWDAppEnvironmentSync.Repaint();
                };
            };

            EditorGUI.BeginDisabledGroup(kInternalLogin == "" || kInternalLogin == null);

            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "Rescue dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Rescue", tSuccessOrFailed, null, null, null, tEnvironmentDev);
                sOperation.Action = "rescue";
                sOperation.EmailRescue = kInternalLogin;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "Rescue preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Rescue", tSuccessOrFailed, null, null, null, tEnvironmentPreprod);
                sOperation.Action = "rescue";
                sOperation.EmailRescue = kInternalLogin;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            //			if (GUI.Button (new Rect (tX + (tWidthTiers + NWDConstants.kFieldMarge) * 2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Rescue", null, null, null, null, tEnvironmentProd);
            //				sOperation.Action = "rescue";
            //				sOperation.Email = Email;
            //				NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (sOperation, true);
            //			}

            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(Email == "" || Password == "");
            if (GUI.Button(new Rect(tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn dev", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance().DevEnvironment;

                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Sign-in", tSuccessOrFailed, tSuccessOrFailed, null, null, tEnvironmentDev);
                sOperation.Action = "signin";
                sOperation.EmailHash = Email;
                sOperation.PasswordHash = Password;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);

            }

            if (GUI.Button(new Rect(tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn preprod", tMiniButtonStyle))
            {
                NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;

                NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create("Editor Account Sign-in", tSuccessOrFailed, tSuccessOrFailed, null, null, tEnvironmentPreprod);
                sOperation.Action = "signin";
                sOperation.EmailHash = Email;
                sOperation.PasswordHash = Password;
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);
            }

            //			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDConstants.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn prod", tMiniButtonStyle)) {
            //				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
            //
            //				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Sign-in", null, null, null, null, tEnvironmentProd);
            //				sOperation.Action = "signin";
            //				sOperation.EmailHash = Email;
            //				sOperation.PasswordHash = Password;
            //				NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (sOperation, true);
            //			}
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            EditorGUI.EndDisabledGroup();
            tY += NWDConstants.kFieldMarge;
            // Tool box of account connected objects
            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Connected NWDBasis Objects", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
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
                    tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                }
            }*/
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            //Debug.Log ("AddonEditorHeight");
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), 100);

            float tY = NWDConstants.kFieldMarge;

            tY += NWDConstants.kFieldMarge * 2;
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += NWDConstants.kFieldMarge * 2;
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
