//=====================================================================================================================
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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperatingSystem : int
    {
        IOS = 0,
        OSX = 1,
        AppleTV = 2,
        Android = 3,
        WINRT = 8,
        WIN = 9,

        UNITY = 99,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDUserInfosConnection : NWDConnection<NWDUserInfos>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UIF")]
    [NWDClassDescriptionAttribute("General User Informations")]
    [NWDClassMenuNameAttribute("User Infos")]
    public partial class NWDUserInfos : NWDBasis<NWDUserInfos>
    {
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Properties
        [NWDGroupStart("Player Informations")]
        [NWDTooltips("")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDUserAvatar> Avatar
        {
            get; set;
        }
        public NWDReferenceType<NWDUserNickname> Nickname
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Localization Options")]
        public NWDLanguageType Language
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Push notification Options")]

        public NWDOperatingSystem OSLastSignIn
        {
            get; set;
        }
        public bool AcceptTradePush
        {
            get; set;
        }
        public bool AcceptBarterPush
        {
            get; set;
        }
        public bool AcceptShopPush
        {
            get; set;
        }
        public bool AcceptRelationshipPush
        {
            get; set;
        }
        public bool AcceptUserInterMessagePush
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the apple notification token for message.
        /// </summary>
        /// <value>The apple notification token.</value>
        public string AppleNotificationToken
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets the google notification token for message.
        /// </summary>
        /// <value>The google notification token.</value>
        public string GoogleNotificationToken
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Game Options")]
        public bool SFX
        {
            get; set;
        }
        public float SFXVolume
        {
            get; set;
        }
        public bool Music
        {
            get; set;
        }
        public float MusicVolume
        {
            get; set;
        }
        public NWDLocalizableStringType MusicVolumeLangu
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Last Game Informations")]
        public NWDReferenceType<NWDItem> LastItemUsedReference
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> LastItemWinReference
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> LastSpiritUsedReference
        {
            get; set;
        }
        #endregion
        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInfos()
        {
            //Debug.Log("NWDUserInfos Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserInfos Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get active user information
        /// </summary>
        public static NWDUserInfos GetUserInfoByEnvironmentOrCreate()
        {
            NWDUserInfos tUserInfos = null;
            foreach (NWDUserInfos user in FindDatas())
            {
                if (user.Account.GetReference().Equals(NWDAccount.GetCurrentAccountReference()))
                {
                    tUserInfos = user;
                    break;
                }
            }
            if (tUserInfos == null)
            {
                tUserInfos = NewData();
                tUserInfos.InternalKey = NWDAccount.GetCurrentAccountReference();
                tUserInfos.Account.SetReference(NWDAccount.GetCurrentAccountReference());
                tUserInfos.Tag = NWDBasisTag.TagUserCreated;
                tUserInfos.SaveData();
            }
            return tUserInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartOnDevice()
        {
#if UNITY_ANDROID
            OSLastSignIn = NWDOperatingSystem.Android;
            // TODO register notification token

#elif UNITY_IOS
            OSLastSignIn = NWDOperatingSystem.IOS;
            // TODO register notification token

            NotificationServices.RegisterForNotifications( NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);

            byte[] tToken = NotificationServices.deviceToken;
            if (tToken != null)
            {
                AppleNotificationToken = "%" + System.BitConverter.ToString(tToken).Replace('-', '%');
            }

#elif UNITY_STANDALONE_OSX
            OSLastSignIn = NWDOperatingSystem.OSX;
            // TODO register notification token

#elif UNITY_STANDALONE_WIN
            OSLastSignIn = NWDOperatingSystem.WIN;

#elif UNITY_WP8
            OSLastSignIn = NWDOperatingSystem.WIN;

#elif UNITY_WINRT
            OSLastSignIn = NWDOperatingSystem.WINRT;

#endif

            if (UpdateDataIfModified())
            {
                // TODO send to server immediatly
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>(){typeof(NWDUserInfos)}, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account type of active user
        /// </summary>
        //public static NWDAppEnvironmentPlayerStatut GetUserStatut(NWDUserInfos user)
        //{
        //    NWDAppEnvironmentPlayerStatut rPlayerStatut = NWDAppEnvironmentPlayerStatut.Unknow;
        //    if (user != null)
        //    {
        //        Debug.Log("--GetUserStatut : " + user.AccountType.ToString());
        //        //rPlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), user.AccountType, true);
        //        rPlayerStatut = user.AccountType;
        //    }

        //    return rPlayerStatut;
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
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
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================