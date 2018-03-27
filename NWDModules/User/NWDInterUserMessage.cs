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
    /// <summary>
    /// NWDInterUserMessageConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
    /// Use like :
    /// public class MyScriptInGame : MonoBehaviour
    /// { 
    /// [NWDConnectionAttribut (true, true, true, true)] // optional
    /// public NWDInterUserMessageConnection MyNetWorkedData;
    /// }
    /// </summary>
    [Serializable]
    public class NWDInterUserMessageConnection : NWDConnection<NWDInterUserMessage>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IUM")]
    [NWDClassDescriptionAttribute("Post message to user to user ")]
    [NWDClassMenuNameAttribute("User Inter Message")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    [NWDClassMenuNameAttribute("User Inter Message")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD example class. This class is use for (complete description here)
    /// </summary>
    public partial class NWDInterUserMessage : NWDBasis<NWDInterUserMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your static properties
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        //PROPERTIES
        [NWDGroupStart("Sender")]
        [NWDTooltips("The Sender account reference")]
        [NWDNeedAvatarAttribute()]
        [NWDNeedNicknameAttribute()]
        public NWDReferenceType<NWDAccount> Sender {get; set;}
        [NWDTooltips("The publishing date")]
        public NWDDateTimeType PublicationDate {get; set;}
        [NWDTooltips("Receipt Acknowledgment : publisher see the meessage was reading")]
        public bool ReceiptAcknowledgment { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Receiver")]
        [NWDTooltips("The Receiver account reference")]
        [NWDNeedAvatarAttribute()]
        [NWDNeedNicknameAttribute()]
        public NWDReferenceType<NWDAccount> Receiver {get; set;}
        [NWDTooltips("The message was show at date")]
        public NWDDateTimeType DistributeDate {get; set;}
        [NWDTooltips("The message was show ")]
        public bool Distribute { get; set; }
        [NWDTooltips("The message was read")]
        public bool Read {get; set;}
        [NWDTooltips("Don't trash message, just archived to hide the message")]
        public bool Archived { get; set;}
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Message")]
        [NWDTooltips("The Message")]
        public NWDReferenceType<NWDMessage> Message { get; set; }
        [NWDTooltipsAttribute("Select characters to use in message by these tags" +
                      "\n •for Fistname : #F1# #F2# …" +
                      "\n •for Lastname : #L1# #L2# …" +
                      "\n •for Nickname : #N1# #N2# …" +
                      "")]
        public NWDReferencesListType<NWDCharacter> ReplaceCharacters { get; set; }
        [NWDTooltipsAttribute("Select items to use in message by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for quantity and item name #xI0# #xI1# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItem> ReplaceItems { get; set; }
        [NWDTooltipsAttribute("Select itemgroups to use item to describe the group in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG0# #xG1# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItemGroup> ReplaceItemGroups { get; set; }
        [NWDTooltipsAttribute("Select Pack to use item to describe the pack in message by these tags" +
                              "\n •for item to describe name #P0# #P1# …" +
                              "\n •for quantity and item to describe name #xP0# #xP1# …" +
                              "")]
        public NWDReferencesQuantityType<NWDPack> ReplacePacks { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Push system")]
        [NWDTooltips("The published message template")]
        public NWDReferenceType<NWDMessage> PushMessage { get; set; }
        [NWDTooltips("The message in jsons for Android pushing")]
        public string PushAndroid { get; set; }
        [NWDTooltips("The message in jsons for Apple pushing")]
        public string PushApple { get; set; }
        [NWDTooltips("The ultimate date to push the message")]
        public NWDDateTimeType PushDate { get; set; }
        [NWDTooltips("The message was pushed")]
        public bool Push { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Original Inter User Message")]
        public NWDReferenceType<NWDInterUserMessage> OriginalMessage { get; set;}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDInterUserMessage()
        {
            //Debug.Log("NWDInterUserMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDInterUserMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDInterUserMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static void SendMessage(NWDMessage sMessage, string sReceiver,
                                bool sNow = true,
                                NWDMessage sPushMessage = null,
                                int sPushDelayInSeconds = 3600,
                                NWDReferencesListType<NWDCharacter> sReplaceCharacters = null,
                                NWDReferencesQuantityType<NWDItem> sReplaceItems = null,
                                NWDReferencesQuantityType<NWDItemGroup> sReplaceItemGroups = null,
                                NWDReferencesQuantityType<NWDPack> sReplacePacks = null
                               )
        {
            NWDInterUserMessage tInterMessage = NWDInterUserMessage.NewObject();
            tInterMessage.Message.SetObject(sMessage);
            // put players
            string tPublisher = NWDAppEnvironment.SelectedEnvironment().PlayerAccountReference;
            tInterMessage.Sender.SetReference(tPublisher);
            tInterMessage.Receiver.SetReference(sReceiver);
            // inserrt the replacacble element
            tInterMessage.ReplaceCharacters = sReplaceCharacters;
            tInterMessage.ReplaceItems = sReplaceItems;
            tInterMessage.ReplaceItemGroups = sReplaceItemGroups;
            tInterMessage.ReplacePacks = sReplacePacks;
            // add datetime
            tInterMessage.PublicationDate.SetDateTime(DateTime.Now);
            // prepare push ?
            tInterMessage.PushMessage.SetObject(sPushMessage);
            tInterMessage.PublicationDate.SetDateTime(DateTime.Now.AddSeconds(sPushDelayInSeconds));
            if (sPushMessage!=null)
            {
                // TODO prepare push json
            }
            // send message now?
            if (sNow == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDInterUserMessage) }, true);
            }
            else
            {
                // send message on the next sync.
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MessageRichText(bool sBold = true)
        {
            string rReturn = "";
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Message.GetLocalString();
                rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MessageRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = "";
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Message.GetLanguageString(sLanguage);
                rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TitleRichText(bool sBold = true)
        {
            string rReturn = "";
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Title.GetLocalString();
                rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TitleRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = "";
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Title.GetLanguageString(sLanguage);
                rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname PublisherNickname()
        {
            return NWDUserNickname.GetFirstObject(Sender.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname ReceiverNickname()
        {
            return NWDUserNickname.GetFirstObject(Receiver.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAvatar PublisherAvatar()
        {
            return NWDAvatar.GetFirstObject(Sender.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAvatar ReceiverAvatar()
        {
            return NWDAvatar.GetFirstObject(Receiver.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, string sLanguage, bool sBold = true)
        {
            string rText = NWDToolbox.Enrichment(sText, sLanguage, ReplaceCharacters, ReplaceItems, ReplaceItemGroups, ReplacePacks, sBold);
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
            }
            NWDUserNickname tPublisherObject = PublisherNickname();
            string tPublisher = "";
            string tPublisherID = "";
            if (tPublisherObject != null)
            {
                tPublisher = tPublisherObject.Nickname;
                tPublisherID = tPublisherObject.UniqueNickname;
            }

            rText = rText.Replace("@publisher@", tBstart + tPublisher + tBend);
            rText = rText.Replace("@publisherid@", tBstart + tPublisherID + tBend);

            NWDUserNickname tReceiverObject = ReceiverNickname();
            string tReceiver = "";
            string tReceiverID = "";
            if (tReceiverObject != null)
            {
                tReceiver = tReceiverObject.Nickname;
                tReceiverID = tReceiverObject.UniqueNickname;
            }

            rText = rText.Replace("@receiver@", tBstart + tReceiver + tBend);
            rText = rText.Replace("@receiverid@", tBstart + tReceiverID + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
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
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================