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
    /// NWDUserInfosConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnectionAttribut (true, true, true, true)] // optional
    /// public NWDUserInfosConnection MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class NWDUserInfosConnection : NWDConnection<NWDUserInfos>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UFO")]
    [NWDClassDescriptionAttribute("General User Informations")]
    [NWDClassMenuNameAttribute("User Infos")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class NWDUserInfos : NWDBasis<NWDUserInfos>
    {
        //#warning YOU MUST FOLLOW THIS INSTRUCTIONS
        //-------------------------------------------------------------------------------------------------------------
        // YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
        // YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
        // YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Player Informations")]
        [NWDTooltips("")]
        public NWDReferenceType<NWDAccount> AccountReference
        {
            get; set;
        }
        //[NWDEnumString(new string[] {"Temporary","Anonymous","LoginPassword","Facebook","Google","Unknow"})]
        public NWDAppEnvironmentPlayerStatut AccountType
        {
            get; set;
        }
        public NWDTextureType Avatar
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }
        public string LastName
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
        [NWDGroupStart("Account push notification")]
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
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
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
        public static NWDUserInfos GetUserInfoByEnvironmentOrCreate(NWDAppEnvironment env)
        {
            NWDUserInfos tUserInfos = null;
            foreach (NWDUserInfos user in GetAllObjects())
            {
                if (user.AccountReference.GetReference().Equals(env.PlayerAccountReference))
                {
                    tUserInfos = user;
                    break;
                }
            }
            if (tUserInfos == null)
            {
                tUserInfos = NewObject();
                tUserInfos.InternalKey = env.PlayerAccountReference;
                tUserInfos.AccountReference.SetReference(env.PlayerAccountReference);
                tUserInfos.AccountType = env.PlayerStatut;
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
#elif UNITY_STANDALONE_OSX
            OSLastSignIn = NWDOperatingSystem.OSX;
            // TODO register notification token
#elif UNITY_WP8
            OSLastSignIn = NWDOperatingSystem.WIN;
#elif UNITY_WINRT
            OSLastSignIn = NWDOperatingSystem.WINRT;
#else
#endif
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

    //-------------------------------------------------------------------------------------------------------------
    #region Connection NWDUserInfos with Unity MonoBehavior
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWDUserInfos connection.
    /// In your MonoBehaviour Script connect object with :
    /// <code>
    ///	[NWDConnectionAttribut(true,true, true, true)]
    /// public NWDUserInfosConnection MyNWDUserInfosObject;
    /// </code>
    /// </summary>
    //-------------------------------------------------------------------------------------------------------------
    // CONNEXION STRUCTURE METHODS
    //-------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------
    //public class NWDUserInfosMonoBehaviour: NWDMonoBehaviour<NWDUserInfosMonoBehaviour> {}
    #endregion
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================