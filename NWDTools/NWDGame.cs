//=====================================================================================================================
//
//  ideMobi copyright 2020
//
//  Date        2020-4-23 10:30:00
//  Author      Dolwen (Jérôme DEMYTTENAERE) 
//  Email       jerome.demyttenaere@gmail.com
//  Project     NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDGame
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
        /// Return if NetWorkedData Engine is ready or not!
        /// </summary>
        /// <returns>
        /// Return true if data is ready and indexed
        /// </returns>
        public static bool Ready()
        {
            return NWDDataManager.SharedInstance().DatasAreReady();
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
