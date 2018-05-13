//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BasicToolBox;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================

namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD account panel. A script for NWDAccountPanel refab to show the informations about wiwh account is use in the 
	/// game. 
	/// </summary>
    public class NWDAccountPanel : NWDCallBack
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The name of environment.
		/// </summary>
		public Text TextEnvironment;
		/// <summary>
		/// The account's reference.
		/// </summary>
		public Text TextAccount;
		/// <summary>
		/// The token's value.
		/// </summary>
		public Text TextToken;
		/// <summary>
		/// The anonymous account's reference.
		/// </summary>
		public Text TextAnonymousAccount;
		/// <summary>
		/// The text anonymous token ? .
		/// </summary>
		public Text TextAnonymousToken;
		/// <summary>
		/// The text of web result.
		/// </summary>
		public Text TextWebResult;
		/// <summary>
		/// The text of network statut.
		/// </summary>
		public Text TextNetworkResult;
        /// <summary>
        /// Dropdown menu with list of account
        /// </summary>
        public Dropdown DropdownAccountList;
        /// <summary>
        /// Dropdown menu with list of account
        /// </summary>
        public Dropdown DropdownLocalizationList;
        /// <summary>
        /// The text for login
        /// </summary>
        public Text TextLogin;
        /// <summary>
        /// The text for logout
        /// </summary>
        public Text TextLogout;
        /// <summary>
        /// The text for synchronize
        /// </summary>
        public Text TextSynchronize;
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDAccounTest> AccountList;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to test Synchronization
        /// </summary>
        public void SynchronizeTest()
		{
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock (
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Synchronize Success " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Synchronize Error " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Synchronize Cancel " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Synchronize Progress " + ShowError(sResult);
				}
			);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Use to test Log-in
		/// </summary>
		public void LogInTest()
		{
            // Get selected account
            int key = DropdownAccountList.value;
            if (key > 0)
            {
                NWDAccounTest tAccount = AccountList[key];
                string Login = tAccount.EmailHash;
                string Password = tAccount.PasswordHash;

                NWDDataManager.SharedInstance().AddWebRequestSignTestWithBlock(Login, Password,
                    delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult)
                    {
                        TextWebResult.text = " Sign In Success " + ShowError(sResult);
                    },
                    delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult)
                    {
                        TextWebResult.text = " Sign In Error " + ShowError(sResult);
                    },
                    delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult)
                    {
                        TextWebResult.text = " Sign In Cancel " + ShowError(sResult);
                    },
                    delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult)
                    {
                        TextWebResult.text = " Sign In Progress " + ShowError(sResult);
                    });
            }
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Use to test Log-out
		/// </summary>
		public void LogOutTest()
		{
			NWDDataManager.SharedInstance().AddWebRequestSignOutWithBlock (
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Out Success " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Out Error " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Out Cancel " + ShowError(sResult);
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                   TextWebResult.text = " Sign Out Progress " + ShowError(sResult);
				});
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to test Log-out
        /// </summary>
        public void SignUpTest()
        {
            string tEmail = "Test" + NWDToolbox.Timestamp().ToString() + "-" + UnityEngine.Random.Range(100000, 999999).ToString() + "@idemobi.com";
            string tPassword = "Pass" + UnityEngine.Random.Range(100000, 999999).ToString();
            Debug.Log("Sign-up with " + tEmail + " and " + tPassword);
            NWDDataManager.SharedInstance().AddWebRequestSignUpWithBlock(tEmail,tPassword,tPassword,
                delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Up Success " + ShowError(sResult);
                },
                delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Up Error " + ShowError(sResult);
                },
                delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Up Cancel " + ShowError(sResult);
                },
                delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
                    TextWebResult.text = " Sign Up Progress " + ShowError(sResult);
                });
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Create a Temporary Account
        /// </summary>
        public void TemporaryAccount()
        {
            NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            tAppEnvironment.ResetSession();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAccount()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
		//-------------------------------------------------------------------------------------------------------------
		// Use this for initialization
		void Start()
		{
			Debug.Log("START NWDAccountPanel");
            BTBNotificationManager.SharedInstance().AddObserver (this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (BTBNotification sNotification)
            {
				if (TextNetworkResult!=null)
				{
                    TextNetworkResult.text = "<color=green><b>ON LINE</b></color>";
				}
			});
            BTBNotificationManager.SharedInstance().AddObserver (this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (BTBNotification sNotification)
            {
				if (TextNetworkResult!=null)
				{
				    TextNetworkResult.text = "<color=red><b>OFF LINE</b></color>";
				}
			});
            BTBNotificationManager.SharedInstance().AddObserver (this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (BTBNotification sNotification)
            {
				if (TextNetworkResult!=null)
				{
					TextNetworkResult.text = "<color=orange><b>UNKNOW</b></color>";
				}
			});
            InitLocalizationList();
            InitAccountList();
            SynchronizeTest();

            //NWDLocalization.AutoLocalize(TextLogin);
            //NWDLocalization.AutoLocalize(TextSynchronize);
            //NWDLocalization.AutoLocalize(TextLogout);
		}
		//-------------------------------------------------------------------------------------------------------------
		// Use this for destroy
		void OnDestroy ()
		{
			BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		// Update is called once per frame
		void Update ()
		{
            NWDAppEnvironment tApp = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            NWDUserInfos tActiveUser = NWDUserInfos.GetUserInfoByEnvironmentOrCreate(tApp);

            TextEnvironment.text = tApp.Environment;

            TextAccount.text = tApp.PlayerAccountReference + "\n" + tApp.PlayerStatut + "\n(" + tActiveUser.FirstName + " " + tActiveUser.LastName + ")";
            TextToken.text = tApp.RequesToken;

            TextAnonymousAccount.text = tApp.AnonymousPlayerAccountReference;
			TextAnonymousToken.text = "????";
        }
        //-------------------------------------------------------------------------------------------------------------
        void InitLocalizationList()
        {
            // Create List array
            List<string> tOptions = new List<string>();
            List<string> tOptionsResult = new List<string>();

            // Clear the menu
            DropdownLocalizationList.ClearOptions();

            Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
            foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
            {
                bool tContains = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Contains(tKeyValue.Value);
                if (tContains)
                {
                    tOptions.Add(tKeyValue.Key);
                    tOptionsResult.Add(tKeyValue.Value);
                }
            }

            // Add options to the menu
            DropdownLocalizationList.AddOptions(tOptions);

            int tlocalizableIndex = tOptionsResult.IndexOf(NWDDataManager.SharedInstance().PlayerLanguage);

            // Set active option
            DropdownLocalizationList.value = tlocalizableIndex;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeLanguageAction()
        {
            Debug.Log("start with NWDDataManager.SharedInstance().PlayerLanguage = " + NWDDataManager.SharedInstance().PlayerLanguage);
            List<string> tOptionsResult = new List<string>();
            Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
            foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
            {
                bool tContains = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Contains(tKeyValue.Value);
                if (tContains)
                {
                    tOptionsResult.Add(tKeyValue.Value);
                }
            }
            NWDDataManager.SharedInstance().PlayerLanguageSave(tOptionsResult[DropdownLocalizationList.value]);
            Debug.Log("finish with NWDDataManager.SharedInstance().PlayerLanguage = " + NWDDataManager.SharedInstance().PlayerLanguage);
        }
        //-------------------------------------------------------------------------------------------------------------
        void InitAccountList()
        {
            NWDAppEnvironment tApp = NWDAppConfiguration.SharedInstance().SelectedEnvironment();

            // Create List array
            List<string> tOptions = new List<string>();

            // Clear the menu
            DropdownAccountList.ClearOptions();

            // Active option
            int tActiveAccountIndex = 0;
            int tCpt = 0;

            // Init all options
            AccountList = NWDAccount.GetTestsAccounts();
            foreach (NWDAccounTest acc in AccountList)
            {
                tOptions.Add(acc.InternalKey);
                if (acc.Reference.Equals(tApp.PlayerAccountReference))
                {
                    tActiveAccountIndex = tCpt;
                }
                tCpt++;
            }

            // Add options to the menu
            DropdownAccountList.AddOptions(tOptions);

            // Set active option
            DropdownAccountList.value = tActiveAccountIndex;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebOffLineAction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SelectAccountInEditorAction()
        {
            #if UNITY_EDITOR
            EditorWindow tEditorWindow = EditorWindow.focusedWindow;
            string tAccountReference = NWDAppEnvironment.SelectedEnvironment().PlayerAccountReference;
            Debug.Log("tAccountReference = " + tAccountReference);
            NWDAccount tAccount = NWDAccount.GetObjectAbsoluteByReference(tAccountReference);
            NWDAccount.SetObjectInEdition(tAccount);
            tEditorWindow.Focus();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebBlankAction()
        {
            Debug.Log("WebBlankAction()");
            NWDDataManager.SharedInstance().AddWebRequestBlankWithBlock();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FalseTokenAction()
        {
            Debug.Log("NWDAppEnvironment.SelectedEnvironment().RequesToken = " + NWDAppEnvironment.SelectedEnvironment().RequesToken);
            NWDAppEnvironment.SelectedEnvironment().RequesToken = NWDToolbox.RandomStringUnix(16);
            Debug.Log("NWDAppEnvironment.SelectedEnvironment().RequesToken FALSIFIED = " + NWDAppEnvironment.SelectedEnvironment().RequesToken);
            SynchronizeTest();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void HttpErrorAction()
        {
            Debug.Log("HttpErrorAction()");
            NWDDataManager.SharedInstance().AddWebRequestNoPageWithBlock();
        }
        //-------------------------------------------------------------------------------------------------------------
        string ShowError(BTBOperationResult sResult)
        {
            NWDOperationResult tResult = (NWDOperationResult)sResult;
            string tDescription = "";
            if (tResult.errorDesc != null)
            {
                string tErrorDesc = "";
                if( tResult.errorDesc.Description != null)
                {
                    tErrorDesc = tResult.errorDesc.Description.GetLocalString();
                }
                tDescription = ": (" + tResult.errorCode + ") " + tErrorDesc;
            }

            return tDescription;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
