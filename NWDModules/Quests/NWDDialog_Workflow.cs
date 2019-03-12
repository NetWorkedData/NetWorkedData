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
    [Serializable]
    public enum NWDQuestState : int
    {
        None = 0,
        Start = 1,
        StartAlternate = 2,
        Accept = 3,
        Refuse = 4,
        Rewarding = 8, // the reward must be choose and distribute .... after the quest is success
        Success = 5,
        Cancel = 6,
        Fail = 7,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDDialogState : int
    {
        Normal = 0,     // normal dialog ... nothing to do
        Sequent = 1,    // sequent dialog ... the dialog is reccord as last dialog and try to navigate to the next dialog on restart
        Step = 2,       // step dialog ... the dialog is reccord as last dialog and used on restart
        Reset = 3,      // the last dialog will be reset
        // Cross = 4,      // Go to the next dialog immediatly
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDDialogAnswerType : int
    {
        None,
        Default,
        Cancel,
        Validate,
        Destructive,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDBubbleStyleType : int
    {
        Speech,
        Whisper,
        Thought,
        Scream,
        Narrative,
        Divine,
        Subconscient,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDCharacterPositionType : int
    {
        Left,
        Middle,
        Right,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDialog : NWDBasis<NWDDialog>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog()
        {
            //Debug.Log("NWDDialog Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDDialog Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            RandomFrequency = 1.0F;
            //Answer = new NWDLocalizableStringType();
            //Answer.AddBaseString("");
            //Dialog = new NWDLocalizableTextType();
            //Dialog.AddBaseString("");
            //Resume = new NWDLocalizableTextType();
            //Resume.AddBaseString("");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog ReturnRealDialog(NWDUserQuestAdvancement tQuestUserAdvancement = null)
        {
            NWDDialog rDialog = this;
            if (string.IsNullOrEmpty(Dialog.GetBaseString()) || this.AnswerState == NWDDialogState.Sequent)
            {
                // Sequent dialog
                // no dialog .. it's strange, perhaps dialog is use to fork dialog to another dialog or to fork quest to another quest 
                List<NWDDialog> tNextDialog = GetNextDialogs();
                if (tNextDialog.Count() > 0)
                {
                    rDialog = tNextDialog[0];
                }
            }
            return rDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDDialog> GetValidDialogs(List<NWDDialog> sDialogsList)
        {
            List<NWDDialog> rDialogList = new List<NWDDialog>();
            foreach (NWDDialog tDialog in sDialogsList)
            {
                if (tDialog.AvailabilitySchedule.AvailableNowInGameTime())
                {
                    if (NWDUserOwnership.ConditionalItemGroups(tDialog.RequiredItemGroups))
                    {
                        if (NWDUserOwnership.ConditionalItems(tDialog.RequiredItems))
                        {
                            if (tDialog.RandomFrequency < 1.0F)
                            {
                                float tRandom = UnityEngine.Random.Range(0.0F, 1.0F);
                                //Debug.Log("NWDDialog GetNextDialogs tRandom = " + tRandom.ToString());
                                if (tRandom <= tDialog.RandomFrequency)
                                {
                                    rDialogList.Add(tDialog);
                                }
                            }
                            else
                            {
                                rDialogList.Add(tDialog);
                            }
                        }
                    }
                }
            }
            return rDialogList;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDDialog GetFirstValidDialogs(List<NWDDialog> sDialogsList)
        {
            NWDDialog rDialog = null;
            List<NWDDialog> tDialogPossibilities = GetValidDialogs(sDialogsList);
            if (tDialogPossibilities.Count() > 0)
            {
                rDialog = tDialogPossibilities[0];
            }
            return rDialog;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText,
                                        string sLanguage = null,
                                        NWDReferencesListType<NWDCharacter> sReplaceCharacters = null,
                                        NWDReferencesQuantityType<NWDItem> sReplaceItems = null,
                                        NWDReferencesQuantityType<NWDItemGroup> sReplaceItemGroups = null,
                                        NWDReferencesQuantityType<NWDPack> sReplacePacks = null,
                                        bool sBold = true)
        {
            string rText = NWDUserNickname.Enrichment(sText, sLanguage, sBold); // add nickname, nickname id etc...  
            int tCounter = 0;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            // // replace referecen in text
            if (sReplaceCharacters != null)
            {
                tCounter = 1;
                foreach (NWDCharacter tCharacter in sReplaceCharacters.GetObjects())
                {
                    if (tCharacter.LastName != null)
                    {
                        string tLastName = tCharacter.LastName.GetLanguageString(sLanguage);
                        if (tLastName != null)
                        {
                            rText = rText.Replace("#L" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tLastName + tBend);
                        }
                    }
                    if (tCharacter.FirstName != null)
                    {
                        string tFirstName = tCharacter.FirstName.GetLanguageString(sLanguage);
                        if (tFirstName != null)
                        {
                            rText = rText.Replace("#F" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tFirstName + tBend);
                        }
                    }
                    if (tCharacter.NickName != null)
                    {
                        string tNickName = tCharacter.NickName.GetLanguageString(sLanguage);
                        if (tNickName != null)
                        {
                            rText = rText.Replace("#N" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNickName + tBend);
                        }
                    }
                    tCounter++;
                }
            }
            if (sReplaceItems != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItem, int> tKeyValue in sReplaceItems.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key;
                    if (tItem != null)
                    {
                        string tNameQuantity = string.Empty;
                        string tNameSingular = string.Empty;
                        string tNamePlural = string.Empty;
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#I" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#I" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xI" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            if (sReplaceItemGroups != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItemGroup, int> tKeyValue in sReplaceItemGroups.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.DescriptionItem.GetObject();
                    if (tItem != null)
                    {
                        string tNameQuantity = string.Empty;
                        string tNameSingular = string.Empty;
                        string tNamePlural = string.Empty;
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#G" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#G" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xG" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            if (sReplacePacks != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDPack, int> tKeyValue in sReplacePacks.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.DescriptionItem.GetObject();
                    if (tItem != null)
                    {
                        string tNameQuantity = string.Empty;
                        string tNameSingular = string.Empty;
                        string tNamePlural = string.Empty;
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#P" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#P" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xP" + tCounter.ToString() + BTBConstants.K_HASHTAG, tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------//-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rReturn = NWDDialog.Enrichment(sText, NWDDataManager.SharedInstance().PlayerLanguage, ReplaceCharacters, ReplaceItems, ReplaceItemGroups, ReplacePacks, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDDialog> GetNextDialogs()
        {
            List<NWDDialog> rDialogList = GetValidDialogs(NextDialogs.GetObjectsList());
            // check if Next quest is valid ... and if Quest is master or not 
            //NWDQuest tNextQuest = NextQuest.GetObject();
            //if (tNextQuest != null)
            //{
            //    NWDDialog tDialogQuest = NWDQuestUserAdvancement.FirstDialogOnShowQuest(tNextQuest);
            //    if (tDialogQuest != null)
            //    {
            //        rDialogList.Add(tDialogQuest);
            //    }
            //}
            return rDialogList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AnswerRichText(bool sBold = true)
        {
            string rReturn = Answer.GetLocalString();
            rReturn = this.Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AnswerRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = Answer.GetLanguageString(sLanguage);
            rReturn = this.Enrichment(rReturn, sLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DialogRichText(bool sBold = true)
        {
            string rReturn = Dialog.GetLocalString();
            rReturn = this.Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DialogRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = Dialog.GetLanguageString(sLanguage);
            rReturn = this.Enrichment(rReturn, sLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================