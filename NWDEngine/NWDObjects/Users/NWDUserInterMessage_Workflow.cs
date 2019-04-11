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
        public delegate void messageBlock(bool error, NWDOperationResult result);
        public messageBlock messageBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInterMessage()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInterMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            PublicationDate.SetDateTime(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInterMessage CreateNewMessageWith(NWDMessage sMessage,
                                          string sReceiver,
                                          int sPushDelayInSeconds = 0,
                                          NWDReferencesListType<NWDCharacter> sReplaceCharacters = null,
                                          NWDReferencesQuantityType<NWDItem> sReplaceItems = null,
                                          NWDReferencesQuantityType<NWDItemPack> sReplaceItemPack = null,
                                          NWDReferencesQuantityType<NWDPack> sReplacePacks = null
                                         )
        {
            NWDUserInterMessage rInterMessage = NewData();

            // Set Sender
            string tPublisher = NWDAppEnvironment.SelectedEnvironment().PlayerAccountReference;
            rInterMessage.Sender.SetReference(tPublisher);
            rInterMessage.PublicationDate.SetDateTime(DateTime.Now.AddSeconds(sPushDelayInSeconds));

            // Set Receiver
            rInterMessage.Receiver.SetReference(sReceiver);

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
            rInterMessage.Message.SetObject(sMessage);
            rInterMessage.ReplaceSenderNickname = NWDUserNickname.GetNickname();
            rInterMessage.ReplaceCharacters = sReplaceCharacters;
            rInterMessage.ReplaceItems = sReplaceItems;
            rInterMessage.ReplaceItemPacks = sReplaceItemPack;
            rInterMessage.ReplacePacks = sReplacePacks;
            #if UNITY_EDITOR
            rInterMessage.InternalKey = NWDUserNickname.GetNickname() + " - " + sMessage.Title.GetBaseString();
            #endif
            rInterMessage.Tag = NWDBasisTag.TagUserCreated;
            rInterMessage.SaveData();

            return rInterMessage;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInterMessage[] FindSenderDatas()
        {
            List<NWDUserInterMessage> rList = new List<NWDUserInterMessage>();
            NWDUserInterMessage[] tMessages = FindDatas();
            foreach (NWDUserInterMessage tMessage in tMessages)
            {
                if (tMessage.Sender.GetReference() == NWDAccount.CurrentReference())
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
                if (tMessage.Receiver.GetReference() == NWDAccount.CurrentReference())
                {
                    rList.Add(tMessage);
                }
            }

            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SendMessage()
        {
            // Push System
            //TODO : set a push system here, not implemented yet

            //Ask server to generate a new Code Pin
            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (messageBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    messageBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (messageBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    messageBlockDelegate(true, tResult);
                }
            };

            // Sync NWDUserRelationship
            SynchronizationFromWebService(tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string MessageRichText(bool sBold = true)
        {
            string rReturn = string.Empty;
            NWDMessage tMessage = Message.GetObject();
            if (tMessage != null)
            {
                rReturn = tMessage.Description.GetLocalString();
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
                rReturn = tMessage.Description.GetLanguageString(sLanguage);
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
        public NWDUserNickname PublisherNickname()
        {
            return NWDUserNickname.GetFirstData(Sender.GetReference(), null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname ReceiverNickname()
        {
            return NWDUserNickname.GetFirstData(Receiver.GetReference(), null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar PublisherAvatar()
        {
            return NWDUserAvatar.GetFirstData(Sender.GetReference(),NWDGameSave.SelectCurrentDataForAccount(Sender.GetReference()));
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar ReceiverAvatar()
        {
            return NWDUserAvatar.GetFirstData(Receiver.GetReference(), NWDGameSave.SelectCurrentDataForAccount(Receiver.GetReference()));
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
            string rText = NWDLocalization.Enrichment(sText, sLanguage, sBold);
            rText = NWDUserNickname.Enrichment(rText, sBold);
            rText = NWDAccountNickname.Enrichment(rText, sLanguage, sBold);

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