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
    [NWDClassPhpGetAddonAttribute("GetDatasNWDAvatar (0, $tRow['Sender']);\nGetDatasNWDNickname(0, $tRow['Sender']);\nGetDatasNWDAvatar (0, $tRow['Receiver']);\nGetDatasNWDNickname(0, $tRow['Receiver']);\n")]
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
        [NWDGroupStart("Publisher")]
        [NWDTooltips("The publisher sender")]
        public NWDReferenceType<NWDAccount> Sender {get; set;}
        [NWDTooltips("The publishing date")]
        public NWDDateTimeType PublicationDate {get; set;}
        [NWDTooltipsAttribute("Select characters to use in message by these tags" +
                              "\n •for Fistname : #F1# #F2# …" +
                              "\n •for Lastname : #L1# #L2# …" +
                              "\n •for Nickname : #N1# #N2# …" +
                              "")]
        public NWDReferencesListType<NWDCharacter> ReplaceCharacters {get; set;}
        [NWDTooltipsAttribute("Select items to use in message by these tags" +
                              "\n •for item name #I1# #I2# …" +
                              "\n •for quantity and item name #xI1# #xI2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItem> ReplaceItems{get; set;}
        [NWDTooltipsAttribute("Select itemgroups to use item to describe the group in message by these tags" +
                              "\n •for item to describe name #G0# #G1# …" +
                              "\n •for quantity and item to describe name #xG1# #xG2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItemGroup> ReplaceItemGroups {get; set;}
        [NWDTooltipsAttribute("Select Pack to use item to describe the pack in message by these tags" +
                              "\n •for item to describe name #P1# #P2# …" +
                              "\n •for quantity and item to describe name #xP1# #xP2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDPack> ReplacePacks {get; set;}

        [NWDTooltips("Receipt Acknowledgment : publisher see the meessage was reading")]
        public bool ReceiptAcknowledgment { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Reader")]
        [NWDTooltips("The receiver reference")]
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

        [NWDTooltips("The published message template, use @nickname@ or @nicknameid@ to replace by player nickname")]
        public NWDReferenceType<NWDMessage> Message { get; set; }

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
            string rText = sText;
            int tCounter = 0;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
            }

            // Replace the nickname
            NWDUserNickname tNickNameObject = NWDUserNickname.GetFirstObject();
            string tNickname = "";
            string tNicknameID = "";
            if (tNickNameObject != null)
            {
                tNickname = tNickNameObject.Nickname;
                tNicknameID = tNickNameObject.UniqueNickname;
            }

            rText = rText.Replace("@nickname@", tBstart + tNickname + tBend);
            rText = rText.Replace("@nicknameid@", tBstart + tNicknameID + tBend);

            NWDUserNickname tPublisherObject = PublisherNickname();
            string tPublisher = "";
            string tPublisherID = "";
            if (tPublisherObject != null)
            {
                tPublisher = tNickNameObject.Nickname;
                tPublisherID = tNickNameObject.UniqueNickname;
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


            // // replace the text
            if (ReplaceCharacters != null)
            {
                tCounter = 1;
                foreach (NWDCharacter tCharacter in ReplaceCharacters.GetObjects())
                {
                    if (tCharacter.LastName != null)
                    {
                        string tLastName = tCharacter.LastName.GetLanguageString(sLanguage);
                        if (tLastName != null)
                        {
                            rText = rText.Replace("#L" + tCounter.ToString() + "#", tBstart + tLastName + tBend);
                        }
                    }
                    if (tCharacter.FirstName != null)
                    {
                        string tFirstName = tCharacter.FirstName.GetLanguageString(sLanguage);
                        if (tFirstName != null)
                        {
                            rText = rText.Replace("#F" + tCounter.ToString() + "#", tBstart + tFirstName + tBend);
                        }
                    }
                    if (tCharacter.NickName != null)
                    {
                        string tNickName = tCharacter.NickName.GetLanguageString(sLanguage);
                        if (tNickName != null)
                        {
                            rText = rText.Replace("#N" + tCounter.ToString() + "#", tBstart + tNickName + tBend);
                        }
                    }
                    tCounter++;
                }
            }
            if (ReplaceItems != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItem, int> tKeyValue in ReplaceItems.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key;
                    if (tItem != null)
                    {
                        string tName = "";
                        string tNameOnly = "";
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameOnly = tItem.Name.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameOnly = tItem.PluralName.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        rText = rText.Replace("#I" + tCounter.ToString() + "#", tBstart + tNameOnly + tBend);
                        rText = rText.Replace("#xI" + tCounter.ToString() + "#", tBstart + tName + tBend);
                    }
                    tCounter++;
                }
            }
            if (ReplaceItemGroups != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItemGroup, int> tKeyValue in ReplaceItemGroups.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.ItemToDescribe.GetObject();
                    if (tItem != null)
                    {
                        string tName = "";
                        string tNameOnly = "";
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameOnly = tItem.Name.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameOnly = tItem.PluralName.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        rText = rText.Replace("#G" + tCounter.ToString() + "#", tBstart + tNameOnly + tBend);
                        rText = rText.Replace("#xG" + tCounter.ToString() + "#", tBstart + tName + tBend);
                    }
                    tCounter++;
                }
            }
            if (ReplacePacks != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDPack, int> tKeyValue in ReplacePacks.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.ItemToDescribe.GetObject();
                    if (tItem != null)
                    {
                        string tName = "";
                        string tNameOnly = "";
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameOnly = tItem.Name.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameOnly = tItem.PluralName.GetLanguageString(sLanguage);
                            tName = tKeyValue.Value + " " + tNameOnly;
                        }
                        rText = rText.Replace("#P" + tCounter.ToString() + "#", tBstart + tNameOnly + tBend);
                        rText = rText.Replace("#xP" + tCounter.ToString() + "#", tBstart + tName + tBend);
                    }
                    tCounter++;
                }
            }
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