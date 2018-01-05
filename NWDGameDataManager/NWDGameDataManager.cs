//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using BasicToolBox;
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    #region Public Enums
    public enum SignAction
    {
        signin,
        signup,
        facebook,
        google,
        session
    }
    #endregion
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Class use for all social interaction, like login, logout, signin, signup
    /// </summary>
    public partial class NWDGameDataManager : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        //Login Callback
        public delegate void signBlock(bool value, NWDOperationResult result);
        public signBlock signBlockDelegate;

        //Google Callback
        public delegate void googleSignBlock(bool value);
        public googleSignBlock googleSignBlockDelegate;

        //Session Callback
        public delegate void sessionBlock(bool value, NWDOperationResult result);
        public sessionBlock sessionBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        //Facebook & Google
        private const string FB_KEY = "facebook_logged_in";
        private const string GP_KEY = "google_logged_in";
        //-------------------------------------------------------------------------------------------------------------
        public void SocialFirstInit()
        {
            Debug.Log("Social:init");
            if (BTBPrefsManager.ShareInstance().getBool(GP_KEY) && !BTBSocialManager.UnitySingleton().isLogged())
            {
                googleSignIn();
            }

            if (BTBPrefsManager.ShareInstance().getBool(FB_KEY))
            {
                LoginFacebook();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoginGoogle()
        {
            if (BTBSocialManager.UnitySingleton().isLogged())
            {
                BTBSocialManager.UnitySingleton().ShowAchievement();
                ILocalUser user = BTBSocialManager.UnitySingleton().GetUserInfo();

                if (user != null)
                {
                    string userInfo = "Username: " + user.userName + "\nUser ID: " + user.id + "\nIsUnderage: " + user.underage;
                    Debug.Log(userInfo);
                }

                requestLogin(SignAction.google, "", "", user.id);
            }
            else
            {
                googleSignIn();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoginFacebook()
        {
            #if UNITY_FACEBOOK
            if (!FacebookManager.UnitySingleton().isLogged())
            {
                FacebookManager.UnitySingleton().authenticateBlockDelegate = delegate (bool result)
                {
                    if (result)
                    {
                        Debug.Log("Authentication successful");
                        BTBPrefsManager.ShareInstance().set(FB_KEY, true);
                    }
                    else
                    {
                        Debug.Log("Authentication failed");
                        BTBPrefsManager.ShareInstance().set(FB_KEY, false);
                    }
                };
                FacebookManager.UnitySingleton().Login();
            }
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Logout()
        {
            if (BTBSocialManager.UnitySingleton().isLogged())
            {
                BTBSocialManager.UnitySingleton().Logout();
                BTBPrefsManager.ShareInstance().set(GP_KEY, false);
            }

            #if UNITY_FACEBOOK
            if (FacebookManager.UnitySingleton().isLogged())
            {
                FacebookManager.UnitySingleton().Logout();
                BTBPrefsManager.ShareInstance().set(FB_KEY, false);
            }
            #endif

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;

                if (signBlockDelegate != null)
                {
                    signBlockDelegate(true, tInfos);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;
                
                if (sessionBlockDelegate != null)
                {
                    sessionBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance.AddWebRequestSignOutWithBlock(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterEmailPassword(string email, string confirmEmail, string password)
        {
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                if (sessionBlockDelegate != null)
                {
                    sessionBlockDelegate(true, tInfos);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                if (sessionBlockDelegate != null)
                {
                    sessionBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance.AddWebRequestSignUpWithBlock(email.ToLower(), confirmEmail.ToLower(), password, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoginEmailPassword(string email, string password)
        {
            requestLogin(SignAction.signin, email, password);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckSession()
        {
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                bool tResult = true;
                if(tInfos.sign == NWDAppEnvironmentPlayerStatut.Anonymous)
                {
                    tResult = false;
                }

                if (sessionBlockDelegate != null)
                {
                    sessionBlockDelegate(tResult, tInfos);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                if (sessionBlockDelegate != null )
                {
                    sessionBlockDelegate(false, tInfos);
                }
            };
			BTBOperationBlock tProgress = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
			{
                //Debug.Log("---BTBOperationBlock Progress:" + bProgress.ToString());
			};
            NWDDataManager.SharedInstance.AddWebRequestSessionWithBlock(tSuccess, tFailed, null, tProgress);
        }
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        private void googleSignIn()
        {
            BTBSocialManager.UnitySingleton().authenticateBlockDelegate = delegate (bool result, ILocalUser user)
            {
                if (result)
                {
                    Debug.Log("Social:Authentication successful");
                    BTBPrefsManager.ShareInstance().set(GP_KEY, true);

                    if (user != null)
                    {
                        string userInfo = "Username: " + user.userName + "\nUser ID: " + user.id + "\nIsUnderage: " + user.underage;
                        Debug.Log(userInfo);
                    }

                    requestLogin(SignAction.google, "", "", user.id);
                }
                else
                {
                    Debug.Log("Social:Authentication failed");
                    BTBPrefsManager.ShareInstance().set(GP_KEY, false);

                    requestLogin(SignAction.google);
                }
            };
            BTBSocialManager.UnitySingleton().Authenticate();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void requestLogin(SignAction action, string email = "", string password = "", string id = "")
        {            
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;

                if (signBlockDelegate != null)
                {
                    signBlockDelegate(true, tInfos);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;

                if (signBlockDelegate != null)
                {
                    signBlockDelegate(false, tInfos);
                }
            };
            switch(action)
            {
                case SignAction.signin:
                    NWDDataManager.SharedInstance.AddWebRequestSignInWithBlock(email.ToLower(), password, tSuccess, tFailed);
                    break;
                case SignAction.google:
                    NWDDataManager.SharedInstance.AddWebRequestLogGoogleWithBlock(id, tSuccess, tFailed);
                    break;
                case SignAction.facebook:
                    NWDDataManager.SharedInstance.AddWebRequestLogFacebookWithBlock(id, tSuccess, tFailed);
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================