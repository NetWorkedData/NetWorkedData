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
using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDPlayer
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void sessionBlock(bool result, NWDOperationResult infos);
        public static sessionBlock sessionBlockDelegate;
        public delegate void sessionBlockProgress(float progress);
        public static sessionBlockProgress sessionBlockProgressDelegate;
        public delegate void sessionBlockCancel();
        public static sessionBlockCancel sessionBlockCancelDelegate;
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the <see cref="NWDAccountInfos"/> CurrentData for the player. <see cref="NWDAccountInfos"/> is not the <see cref="NWDUserInfos"/>
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        public static NWDAccountInfos GetCurrentAccountInfos()
        {
            return NWDAccountInfos.CurrentData();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Check if active account is Associated and not set to DeviceID or None
        /// </summary>
        /// <returns>
        /// Return a boolean value
        /// </returns>
        /// <remarks>
        /// This method use <c>NWDAccountSign</c> to check if active account is Associated and have an <c>NWDAccountSignType</c> not set to DeviceID or None
        /// </remarks>
        public static bool CheckSession()
        {
            bool rResult = false;
            NWDAccountSign[] tSigns = NWDBasisHelper.GetReachableDatas<NWDAccountSign>();
            foreach (NWDAccountSign k in tSigns)
            {
                if (k.SignStatus == NWDAccountSignAction.Associated)
                {
                    if (k.SignType != NWDAccountSignType.DeviceID &&
                        k.SignType != NWDAccountSignType.None)
                    {
                        rResult = true;
                        break;
                    }
                }
            }

            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign up with block
        /// </summary>
        /// <param name="sSocialID">A social ID.</param>
        /// <param name="sSocialType">A social network type.</param>
        /// <remarks>
        /// This method Hash social ID before calling the web service
        /// </remarks>
        /// <example>
        /// <code>
        /// NWDGame.RegisterSocialNetwork("0123456789", NWDAccountSignType.Facebook);
        /// </code>
        /// </example>
        public static void RegisterSocialNetwork(string sSocialID, NWDAccountSignType sSocialType)
        {
            string tSign = NWDAccountSign.GetSignSocialHash(sSocialID);
            NWDDataManager.SharedInstance().AddWebRequestSignUpWithBlock(sSocialType, tSign, string.Empty, string.Empty, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign up with block
        /// </summary>
        /// <param name="sEmail">An email.</param>
        /// <param name="sPassword">A password.</param>
        /// <remarks>
        /// This method Hash email and password (together) then email before calling the web service
        /// </remarks>
        /// <example>
        /// <code>
        /// NWDGame.RegisterEmailPassword("email@domain.com", "password");
        /// </code>
        /// </example>
        public static void RegisterEmailPassword(string sEmail, string sPassword)
        {
            string tSign = NWDAccountSign.GetSignEmailPasswordHash(sEmail, sPassword);
            string tRescue = NWDAccountSign.GetRescueEmailHash(sEmail);
            NWDDataManager.SharedInstance().AddWebRequestSignUpWithBlock(NWDAccountSignType.EmailPassword, tSign, tRescue, string.Empty, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign up with block
        /// </summary>
        /// <param name="sEmail">An email.</param>
        /// <param name="sLogin">A user name.</param>
        /// <param name="sPassword">A password.</param>
        /// <remarks>
        /// This method Hash login and password (together) then email, login before calling the web service
        /// </remarks>
        /// <example>
        /// <code>
        /// NWDGame.RegisterLoginPasswordEmail("email@domain.com", "user", "password");
        /// </code>
        /// </example>
        public static void RegisterLoginPasswordEmail(string sEmail, string sLogin, string sPassword)
        {
            string tSign = NWDAccountSign.GetSignLoginPasswordHash(sLogin, sPassword);
            string tRescue = NWDAccountSign.GetRescueEmailHash(sEmail);
            string tLogin = NWDAccountSign.GetLoginHash(sLogin);
            NWDDataManager.SharedInstance().AddWebRequestSignUpWithBlock(NWDAccountSignType.LoginPasswordEmail, tSign, tRescue, tLogin, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign in with block
        /// </summary>
        /// <param name="sSocialID">A social ID.</param>
        /// <remarks>
        /// This method Hash social ID before calling the web service
        /// </remarks>
        /// <example>
        /// <code>
        /// NWDGame.LoginSocial("0123456789");
        /// </code>
        /// </example>
        public static void LoginSocial(string sSocialID)
        {
            string tSign = NWDAccountSign.GetSignSocialHash(sSocialID);
            NWDDataManager.SharedInstance().AddWebRequestSignInWithBlock(tSign, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign in with block
        /// </summary>
        /// <param name="sEmail">An email.</param>
        /// <param name="sPassword">A password.</param>
        /// <remarks>
        /// This method Hash email before calling the web service
        /// </remarks>
        /// <example>
        /// <code>
        /// NWDGame.LoginEmailPassword("email@domain.com", "password");
        /// </code>
        /// </example>
        public static void LoginEmailPassword(string sEmail, string sPassword)
        {
            string tSign = NWDAccountSign.GetSignEmailPasswordHash(sEmail, sPassword);
            NWDDataManager.SharedInstance().AddWebRequestSignInWithBlock(tSign, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Sign out with block
        /// </summary>
        /// <remarks>
        /// This method call <c>NWDAccount.AccountCanSignOut()</c> before doing a web request
        /// </remarks>
        /// <seealso cref="NWDAccount.AccountCanSignOut()"/>
        public static void Logout()
        {
            // Can Logout ? Active Session is not on DeviceID ?
            if (NWDAccount.AccountCanSignOut())
            {
                NWDDataManager.SharedInstance().AddWebRequestSignOutWithBlock(SessionSuccessBlock, SessionFailedBlock);
            }
            else
            {
                sessionBlockDelegate?.Invoke(true, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Social network
        /// </summary>
        /// <param name="sSocialID">An social ID.</param>
        /// <param name="sSocialType">A social network type.</param>
        /// <example>
        /// <code>
        /// NWDGame.AddSocialNetwork("0123456789", NWDAccountSignType.Facebook);
        /// </code>
        /// </example>
        public static void AddSocialNetwork(string sSocialID, NWDAccountSignType sSocialType)
        {
            NWDAccountSign.CreateAndRegisterSocialNetwork(sSocialID, sSocialType, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Create and register email password
        /// </summary>
        /// <param name="sEmail">An email.</param>
        /// <param name="sPassword">A password.</param>
        /// <example>
        /// <code>
        /// NWDGame.AddEmailPassword("email@domain.com", "password");
        /// </code>
        /// </example>
        public static void AddEmailPassword(string sEmail, string sPassword)
        {
            NWDAccountSign.CreateAndRegisterEmailPassword(sEmail, sPassword, SessionSuccessBlock, SessionFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call a web request service: Create and register login email password
        /// </summary>
        /// <param name="sLogin">A user name.</param>
        /// <param name="sEmail">An email.</param>
        /// <param name="sPassword">A password.</param>
        /// <example>
        /// <code>
        /// NWDGame.AddLoginEmailPassword("user, "email@domain.com", "password");
        /// </code>
        /// </example>
        public static void AddLoginEmailPassword(string sLogin, string sEmail, string sPassword)
        {
            NWDAccountSign.CreateAndRegisterLoginPasswordEmail(sLogin, sEmail, sPassword, SessionSuccessBlock, SessionFailedBlock);
        }
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        static void SessionSuccessBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            NWDOperationResult tInfos = sInfos as NWDOperationResult;
            sessionBlockDelegate?.Invoke(true, tInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionFailedBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            NWDOperationResult tInfos = sInfos as NWDOperationResult;
            sessionBlockDelegate?.Invoke(false, tInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionCancelBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            sessionBlockCancelDelegate?.Invoke();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void SessionProgressBlock(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
        {
            sessionBlockProgressDelegate?.Invoke(sProgress);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
