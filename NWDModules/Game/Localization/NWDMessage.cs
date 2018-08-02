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
    public class NWDMessageConnection : NWDConnection<NWDMessage>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("MES")]
    [NWDClassDescriptionAttribute("Message descriptions Class")]
    [NWDClassMenuNameAttribute("Messages")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        //public bool DiscoverItYourSelf { get; set; }
        [NWDGroupStartAttribute("Informations", true, true, true)] //ok
        public string Domain
        {
            get; set;
        }
        public string Code
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Description", true, true, true)] // ok
        public NWDLocalizableStringType Title
        {
            get; set;
        } // TODO : rename by Title ?
        public NWDLocalizableStringType Message
        {
            get; set;
        } // TODO : rename by Description ?
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("User choose", true, true, true)] // ok
        public bool HasValidButton
        {
            get; set;
        }
        public NWDLocalizableStringType ValidText
        {
            get; set;
        }
        public NWDReferenceType<NWDAction> ValidAction
        {
            get; set;
        }
        public bool HasCancelButton
        {
            get; set;
        }
        public NWDLocalizableStringType CancelText
        {
            get; set;
        }
        public NWDReferenceType<NWDAction> CancelAction
        {
            get; set;
        }
        //public NWDLocalizableStringType Cancel { get; set; }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage()
        {
            //Debug.Log("NWDMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage GetMessageWithCode(string sCode)
        {
            NWDMessage rReturn = null;
            foreach (NWDMessage tObject in NWDMessage.Datas().ObjectsList)
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
        public static NWDMessage GetMessageWithDomainAndCode(string sDomain, string sCode)
        {
            NWDMessage rReturn = null;
            foreach (NWDMessage tObject in NWDMessage.Datas().ObjectsList)
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
        public static NWDMessage CreateGenericMessage(string sDomain, string sCode, string sTitle, string sDescription)
        {
            string tReference = "MES-" + sDomain + "-" + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDMessage tError = NWDMessage.NEW_GetDataByReference(tReference);
            //NWDMessage tError = InstanceByReference(tReference) as NWDMessage;
            if (tError == null)
            {
                tError = NWDBasis<NWDMessage>.NewData();
                //RemoveObjectInListOfEdition(tError);
                tError.Reference = tReference;
                //				tError.InternalKey = Domain + " : " + sCode;
                tError.InternalDescription = sDescription;
                // domain code
                tError.Domain = sDomain;
                tError.Code = sCode;
                // title
                NWDLocalizableStringType tTitle = new NWDLocalizableStringType();
                tTitle.Value = "BASE:" + sTitle;
                tError.Title = tTitle;
                // description
                NWDLocalizableStringType tDescription = new NWDLocalizableStringType();
                tDescription.Value = "BASE:" + sDescription;
                tError.Message = tDescription;
                // add-on edited
                tError.AddonEdited(true);
                // reccord
                tError.UpdateData();
                //AddObjectInListOfEdition(tError);
            }
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
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
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================