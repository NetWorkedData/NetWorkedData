//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using BasicToolBox;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserInterMessageConnection : NWDConnection<NWDUserInterMessage>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInterMessage : NWDBasis<NWDUserInterMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInterMessage()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInterMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            PublicationDate.SetDateTime(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SendMessage(NWDMessage sMessage,
                                       string sReceiver,
                                       BTBOperationBlock sSuccessBlock = null,
                                       BTBOperationBlock sErrorBlock = null,
                                       int sPushDelayInSeconds = 0,
                                       NWDReferencesListType<NWDCharacter> sReplaceCharacters = null,
                                       NWDReferencesQuantityType<NWDItem> sReplaceItems = null,
                                       NWDReferencesQuantityType<NWDItemPack> sReplaceItemPack = null,
                                       NWDReferencesQuantityType<NWDPack> sReplacePacks = null
                                      )
        {
            NWDUserInterMessage tInterMessage = NewData();

            // Set Sender
            string tPublisher = NWDAppEnvironment.SelectedEnvironment().PlayerAccountReference;
            tInterMessage.Sender.SetReference(tPublisher);
            tInterMessage.PublicationDate.SetDateTime(DateTime.Now.AddSeconds(sPushDelayInSeconds));

            // Set Receiver
            tInterMessage.Receiver.SetReference(sReceiver);

            // Add Replaceable object if any set in Message
            /*if (sMessage.AttachmentItemPack != null)
            {
                Dictionary<NWDItemPack, int> tItemPacks = sMessage.AttachmentItemPack.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItemPack, int> pair in tItemPacks)
                {
                    NWDItemPack tItemPack = pair.Key;
                    int tItemPackQte = pair.Value;
                    if (sReplaceItemPack == null)
                    {
                        sReplaceItemPack = new NWDReferencesQuantityType<NWDItemPack>();
                    }
                    sReplaceItemPack.AddObjectQuantity(tItemPack, tItemPackQte);
                }
            }*/

            // Set Message and insert the Replaceable object
            tInterMessage.Message.SetObject(sMessage);
            tInterMessage.ReplaceSenderNickname = NWDAccountNickname.GetNickname();
            tInterMessage.ReplaceCharacters = sReplaceCharacters;
            tInterMessage.ReplaceItems = sReplaceItems;
            tInterMessage.ReplaceItemPacks = sReplaceItemPack;
            tInterMessage.ReplacePacks = sReplacePacks;

            // Push System
            //TODO : set a push system here, not implemented yet

            #if UNITY_EDITOR
            tInterMessage.InternalKey = NWDAccountNickname.GetNickname() + " - " + sMessage.Title.GetBaseString();
            #endif

            tInterMessage.Tag = NWDBasisTag.TagUserCreated;
            tInterMessage.SaveData();

            // Send message
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>() { typeof(NWDUserInterMessage) }, sSuccessBlock, sErrorBlock);

            // Delay System
            //TODO : set a WebRequest with a delay, not implemented yet
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInterMessage[] FindSenderDatas()
        {
            List<NWDUserInterMessage> rList = new List<NWDUserInterMessage>();
            NWDUserInterMessage[] tMessages = FindDatas();
            foreach (NWDUserInterMessage tMessage in tMessages)
            {
                if (tMessage.Sender.GetReference() == NWDAccount.GetCurrentAccountReference())
                {
                    rList.Add(tMessage);
                }
            }

            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInterMessage[] FindReceiverDatas()
        {
            List<NWDUserInterMessage> rList = new List<NWDUserInterMessage>();
            NWDUserInterMessage[] tMessages = FindDatas();
            foreach (NWDUserInterMessage tMessage in tMessages)
            {
                if (tMessage.Receiver.GetReference() == NWDAccount.GetCurrentAccountReference())
                {
                    rList.Add(tMessage);
                }
            }

            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MessageRichText(bool sBold = true)
        {
            string rReturn = string.Empty;
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Message.GetLocalString();
                rReturn = Enrichment(rReturn, null, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MessageRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = string.Empty;
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
            string rReturn = string.Empty;
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Title.GetLocalString();
                rReturn = Enrichment(rReturn, null, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TitleRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = string.Empty;
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Title.GetLanguageString(sLanguage);
                rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountNickname PublisherNickname()
        {
            return NWDAccountNickname.GetFirstData(Sender.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountNickname ReceiverNickname()
        {
            return NWDAccountNickname.GetFirstData(Receiver.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar PublisherAvatar()
        {
            return NWDUserAvatar.GetFirstData(Sender.GetReference(),NWDGameSave.CurrentForAccount(Sender.GetReference()));
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar ReceiverAvatar()
        {
            return NWDUserAvatar.GetFirstData(Receiver.GetReference(), NWDGameSave.CurrentForAccount(Receiver.GetReference()));
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            // Replace Tag by user Nickname
            string rText = NWDAccountNickname.Enrichment(sText, sLanguage, sBold);

            // Replace Tag by sender Nickname
            rText = rText.Replace("#SenderNickname#", tBstart + ReplaceSenderNickname + tBend);
            //rText = rText.Replace("#SenderUniqueNickname#", tBstart + tPublisherID + tBend);

            // Replace Tag by Characters
            int tCpt = 0;
            foreach(NWDCharacter k in ReplaceCharacters.GetObjects())
            {
                rText = k.Enrichment(rText, tCpt, sLanguage, sBold);
                tCpt++;
            }

            // Replace Tag by Items
            tCpt = 0;
            foreach (NWDItem k in ReplaceItems.GetObjects())
            {
                rText = k.Enrichment(rText, tCpt, sLanguage, sBold);
                tCpt++;
            }

            // Replace Tag by Items Groups
            tCpt = 0;
            foreach (NWDItemPack k in ReplaceItemPacks.GetObjects())
            {
                rText = k.Enrichment(rText, tCpt, sLanguage, sBold);
                tCpt++;
            }

            // Replace Tag by Pack
            tCpt = 0;
            foreach (NWDPack k in ReplacePacks.GetObjects())
            {
                rText = k.Enrichment(rText, tCpt, sLanguage, sBold);
                tCpt++;
            }

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================