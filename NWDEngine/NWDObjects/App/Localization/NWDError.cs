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

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDErrorType : int
    {
        LogVerbose = 0, // No alert but write in log
        LogWarning = 1, // No alert but write in log in warning

        InGame = 10, // Use in game alert BTBNotification Post notification

        Alert = 20, // System Dialog
        Critical = 30,  // System Dialog and Quit

        Ignore = 99, // Do Nothing
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDErrorBlock(NWDError sError);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour
    /// {
    ///     NWDConnectionAttribut (true, true, true, true)] // optional
    ///     public NWDExampleConnection MyNetWorkedData;
    ///     public void UseData()
    ///     {
    ///         NWDExample tObject = MyNetWorkedData.GetObject();
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDErrorConnection : NWDConnection<NWDError>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ERR")]
    [NWDClassDescriptionAttribute("Error descriptions Class")]
    [NWDClassMenuNameAttribute("Errors")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDError : NWDBasis<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        //public bool DiscoverItYourSelf { get; set; }
        [NWDGroupStartAttribute("Informations", true, true, true)] //ok
        [NWDTooltips("Type and priority of error")]
        public NWDErrorType Type
        {
            get; set;
        }
        [NWDTooltips("Domain of error")]
        public string Domain
        {
            get; set;
        }
        [NWDTooltips("Code of error in the selected Domain")]
        public string Code
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Description", true, true, true)] // ok
        [NWDTooltips("Title of error message")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        [NWDTooltips("Content of error message")]
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDTooltips("Validation text of error message")]
        public NWDLocalizableStringType Validation
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDError()
        {
            //Debug.Log("NWDError Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDError(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDError Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError GetErrorWithCode(string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDError.FindDatas())
            {
                if (tObject.Code == sCode)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError GetErrorWithDomainAndCode(string sDomain, string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDError.FindDatas())
            {
                if (tObject.Code == sCode && tObject.Domain == sDomain)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError PostNotificationErrorWithDomainAndCode(string sDomain, string sCode)
        {
            NWDError rReturn = NWDError.GetErrorWithDomainAndCode(sDomain, sCode);
            if (rReturn != null)
            {
                rReturn.PostNotificationError();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Enrichment the specified sText, with sLanguage and sBold.
        /// </summary>
        /// <returns>The enrichment.</returns>
        /// <param name="sText">S text.</param>
        /// <param name="sLanguage">S language.</param>
        /// <param name="sBold">If set to <c>true</c> s bold.</param>
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = NWDAccountNickname.Enrichment(sText, sLanguage, sBold);
            rText = NWDLocalization.Enrichment(rText, sLanguage, sBold);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError CreateGenericError(string sDomain, string sCode, string sTitle, string sDescription, string sValidation, NWDErrorType sType = NWDErrorType.LogVerbose, NWDBasisTag sTag = NWDBasisTag.TagInternal)
        {
            BTBBenchmark.Start();
            string tReference = "ERR-" + sDomain + "-" + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDError tError = NWDError.GetDataByReference(tReference);
            if (tError != null && tError.IsTrashed())
            {
                tError = null;
            }
            if (tError == null)
            {
                tError = NWDBasis<NWDError>.NewDataWithReference(tReference, true);
                //RemoveObjectInListOfEdition(tError);
                tError.Reference = tReference;
                // tError.InternalKey = Domain + " : " + sCode;
                tError.InternalDescription = sDescription;
                tError.Tag = sTag;
                // domain code
                tError.Domain = sDomain;
                tError.Code = sCode;
                // title
                NWDLocalizableStringType tTitle = new NWDLocalizableStringType();
                tTitle.AddBaseString(sTitle);
                tError.Title = tTitle;
                // description
                NWDLocalizableTextType tDescription = new NWDLocalizableTextType();
                tDescription.AddBaseString(sDescription);
                tError.Description = tDescription;
                // description
                NWDLocalizableStringType tValidation = new NWDLocalizableStringType();
                tValidation.AddBaseString(sValidation);
                tError.Validation = tValidation;
                // type of alert
                tError.Type = sType;
                // add-on edited
                tError.AddonEdited(true);
                // reccord
                tError.UpdateData(true, NWDWritingMode.QueuedMainThread);
                //AddObjectInListOfEdition(tError);
            }
            BTBBenchmark.Finish();
            return tError;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_ERROR, this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotificationError()
        {
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_ERROR, this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowNativeAlert(BTBAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDErrorType tType = Type;

            // NWDErrorType set by compile environment
#if UNITY_IOS
            // NO CHANGE
#elif UNITY_ANDROID
            // NO CHANGE
#elif UNITY_STANDALONE_OSX
            tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_WIN
            tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_LINUX
            tType = NWDErrorType.InGame;
#else
            tType = NWDErrorType.InGame;
#endif

            switch (tType)
            {
                case NWDErrorType.Alert:
                    {
                        BTBAlert.Alert(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString(), sCompleteBlock);
                    }
                    break;
                case NWDErrorType.Critical:
                    {
                        BTBAlert.Alert(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString(), sCompleteBlock);
                    }
                    break;
                case NWDErrorType.Ignore:
                    {
                        // Do nothing
                    }
                    break;
                case NWDErrorType.InGame:
                    {
                        // Do nothing
                    }
                    break;
                case NWDErrorType.LogVerbose:
                    {
                        Debug.Log("ALERT! " + Title.GetLocalString() + " : " + Description.GetLocalString());
                    }
                    break;
                case NWDErrorType.LogWarning:
                    {
                        Debug.LogWarning("WARNING! " + Title.GetLocalString() + " : " + Description.GetLocalString());
                    }
                    break;
            }
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
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                if (Domain == null || Domain == "")
                {
                    Domain = "Unknow";
                }
                InternalKey = Domain + " : " + Code;
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
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