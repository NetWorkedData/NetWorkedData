﻿//=====================================================================================================================
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
    public class NWDDialogConnection : NWDConnection<NWDDialog>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("DLG")]
    [NWDClassDescriptionAttribute("Dialog descriptions Class")]
    [NWDClassMenuNameAttribute("Dialog")]
    public partial class NWDDialog : NWDBasis<NWDDialog>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------

        [NWDGroupStartAttribute("Availability schedule ", true, true, true)]
        [NWDTooltips("Availability schedule of this Dialog")]
        public NWDDateTimeScheduleType AvailabilitySchedule
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Reply for previous Dialog (optional)", true, true, true)]
        [NWDTooltipsAttribute("The list and quantity of ItemGroup required to show this answer and this dialog")]
        public NWDReferencesConditionalType<NWDItemGroup> RequiredItemGroups
        {
            get; set;
        }
        [NWDTooltipsAttribute("The list and quantity of Item required to show this answer and this dialog")]
        public NWDReferencesConditionalType<NWDItem> RequiredItems
        {
            get; set;
        }
        [NWDTooltipsAttribute("The type of button to use for this answer : " +
                              "\n •None, " +
                              "\n •Default, " +
                              "\n •Cancel, " +
                              "\n •Validate, " +
                              "\n •Destructive, " +
                              "\n •etc." +
                              "")]
        public NWDDialogAnswerType AnswerType
        {
            get; set;
        }
        [NWDTooltipsAttribute("The type of answer " +
                              "\n •Normal (choose answer),  " +
                              "\n •Sequent (just continue next, perhaps not show button)," +
                              "\n •Step ( the next time restart quest here), " +
                              "\n •Reset delete the last refertence answer used" +
                              "")]
        public NWDDialogState AnswerState
        {
            get; set;
        }
        [NWDTooltipsAttribute("The random limit in range to [0-1] ")]
        //[NWDIf("AnswerState", (int)NWDDialogState.Random)]
        [NWDFloatSlider(0.0F, 1.0F)]
        public float RandomFrequency
        {
            get; set;
        }
        [NWDTooltipsAttribute("This answer change the quest state to ... " +
                              "\n •None (do nothing),  " +
                              "\n •Start, " +
                              "\n •StartAlternate, " +
                              "\n •Accept, " +
                              "\n •Refuse, " +
                              "\n •Success, " +
                              "\n •Cancel, " +
                              "\n •Failed" +
                              "")]
        public NWDQuestState QuestStep
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select language and write your answer in this language")]
        public NWDLocalizableStringType Answer
        {
            get; set;
        }
        [NWDTooltips("Add an action to this answer")]
        public NWDReferencesListType<NWDAction> ActionOnAnswer
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Dialog", true, true, true)]
        [NWDTooltipsAttribute("Select the character who says the dialog")]
        public NWDReferenceType<NWDCharacter> CharacterReference
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select the character emotion to illustrate this Dialog")]
        public NWDCharacterEmotion CharacterEmotion
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select the character position in screen")]
        public NWDCharacterPositionType CharacterPosition
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select the Bubble style to content this Dialog" +
                              "\n •Speech," +
                              "\n •Whisper," +
                              "\n •Thought," +
                              "\n •Scream, " +
                              "")]
        public NWDBubbleStyleType BubbleStyle
        {
            get; set;
        }
        [NWDTooltips("Add an action to this dialog")]
        public NWDReferencesListType<NWDAction> ActionOnDialog
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select language and write your dialog in this language " +
                              "\nreplace by user : @nickname@ (old system)" +
                              "\nreplace by user : @nicknameid@ (old system)" +
                              "\nreplace by user : #Nickname#" +
                              "\nreplace by user : #Nicknameid#" +
                              "")]
        public NWDLocalizableTextType Dialog
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select characters to use in dialog by these tags" +
                              "\n •for Fistname : #F1# #F2# …" +
                              "\n •for Lastname : #L1# #L2# …" +
                              "\n •for Nickname : #N1# #N2# …" +
                              "")]
        public NWDReferencesListType<NWDCharacter> ReplaceCharacters
        {
            get; set;
        }

        [NWDTooltipsAttribute("Select items to use in message by these tags" +
                              "\n •for item name singular #I1# #I2# …" +
                              "\n •for item name plural #I1s# #I2s# …" +
                              "\n •for quantity and item name #xI1# #xI2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItem> ReplaceItems
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select itemgroups to use item to describe the group in message by these tags" +
                              "\n •for item to describe name singular #G1# #G2# …" +
                              "\n •for item to describe name plural #G1s# #G2s# …" +
                              "\n •for quantity and item to describe name #xG1# #xG2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDItemGroup> ReplaceItemGroups
        {
            get; set;
        }
        [NWDTooltipsAttribute("Select Pack to use item to describe the pack in message by these tags" +
                              "\n •for item to describe name singular #P1# #P2# …" +
                              "\n •for item to describe name plural #P1s# #P2s# …" +
                              "\n •for quantity and item to describe name #xP1# #xP2# …" +
                              "")]
        public NWDReferencesQuantityType<NWDPack> ReplacePacks
        {
            get; set;
        }
        public NWDReferenceType<NWDYoghurtLyric> YoghurtLyric {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("List of next dialogs (and replies)", true, true, true)]
        public NWDReferencesListType<NWDDialog> NextDialogs
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Option Quest", true, true, true)]
        [NWDTooltipsAttribute("The quest launched after this dialog")]
        public NWDReferenceType<NWDQuest> NextQuest
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("For the quest'sBook", true, true, true)]
        [NWDTooltipsAttribute("The resume to write in the quest book (optional)")]
        public NWDLocalizableTextType Resume
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
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
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
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
            string rText = NWDAccountNickname.Enrichment(sText, sLanguage, sBold); // add nickname, nickname id etc...  
            int tCounter = 0;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
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
            if (sReplaceItems != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItem, int> tKeyValue in sReplaceItems.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key;
                    if (tItem != null)
                    {
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
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
                        rText = rText.Replace("#I" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#I" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xI" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
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
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
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
                        rText = rText.Replace("#G" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#G" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xG" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
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
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
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
                        rText = rText.Replace("#P" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#P" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xP" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
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
        static private bool ImageLoaded = false;
        static public Texture2D kImageSequent = null;
        static public Texture2D kImageStep = null;
        static public Texture2D kImageNormal = null;
        static public Texture2D kImageRandom = null;
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadImages()
        {
            //Debug.Log("STATIC STEConstants LoadImages()");
            if (ImageLoaded == false)
            {
                ImageLoaded = true;
                kImageSequent = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceSequent.psd"));
                kImageStep = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceStep.psd"));
                kImageNormal = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceNormal.psd"));
                kImageRandom = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceRandom.psd"));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
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
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 350.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            GUIStyle tBubuleStyle = new GUIStyle(GUI.skin.box);
            tBubuleStyle.fontSize = 14;
            tBubuleStyle.richText = true;
            string tLangue = NWDNodeEditor.SharedInstance().GetLanguage();
            //string tDialog = Dialog.GetLanguageString(tLangue);
            string tDialog = DialogRichTextForLanguage(tLangue);
            float tText = tBubuleStyle.CalcHeight(new GUIContent(tDialog), sCardWidth - NWDConstants.kFieldMarge * 2 - NWDConstants.kPrefabSize);

            NWDDialog[] tDialogs = NextDialogs.GetObjects();
            float tAnswers = tDialogs.Length * NWDNodeEditor.SharedInstance().GetHeightProperty();

            return NWDConstants.kFieldMarge * 3 + NWDConstants.kPrefabSize + tText + tAnswers;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            LoadImages();

            //GUI.Label(sRect, InternalDescription, EditorStyles.wordWrappedLabel);

            Color tBackgroundColor = GUI.backgroundColor;

            GUIStyle tStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            tStyle.richText = true;
            string tText = "";
            // if answer
            string tLangue = NWDNodeEditor.SharedInstance().GetLanguage();

            string tAnswer = AnswerRichTextForLanguage(tLangue);
            if (tAnswer != "")
            {
                tText += "For the answer : \"" + tAnswer + "\"…\n";
            }
            // quest change
            if (QuestStep != NWDQuestState.None)
            {
                tText += "The Quest change to state : " + QuestStep.ToString() + ".\n";
            }
            // dialog
            NWDCharacter tCharacter = CharacterReference.GetObject();
            string tCharacterName = "unknow";
            string tCharacterEmotion = CharacterEmotion.ToString();
            if (tCharacter != null)
            {
                tCharacterName = tCharacter.FirstName.GetLanguageString(tLangue) + " " + tCharacter.LastName.GetBaseString();
                tCharacter.DrawPreviewTexture2D(new Vector2(sRect.x, sRect.y));
            }
            tText += "<b>" + tCharacterName + "</b> says (" + tLangue + ") [" + tCharacterEmotion + "]:";

            // draw resume
            GUI.Label(new Rect(sRect.x + NWDConstants.kPrefabSize + NWDConstants.kFieldMarge, sRect.y, sRect.width - NWDConstants.kPrefabSize - NWDConstants.kFieldMarge, sRect.height), tText, tStyle);


            // draw dialog
            //string tDialog = Dialog.GetLanguageString(tLangue);
            string tDialog = DialogRichTextForLanguage(tLangue);
            GUIStyle tBubuleStyle = new GUIStyle(GUI.skin.box);
            tBubuleStyle.fontSize = 14;
            tBubuleStyle.richText = true;
            GUI.backgroundColor = Color.white;
            GUI.Box(new Rect(sRect.x + NWDConstants.kPrefabSize - NWDConstants.kFieldMarge * 3,
                             sRect.y + NWDConstants.kPrefabSize - NWDConstants.kFieldMarge * 3,
                             sRect.width - NWDConstants.kPrefabSize + NWDConstants.kFieldMarge * 3,
                             sRect.height - NWDConstants.kPrefabSize + NWDConstants.kFieldMarge * 3), tDialog, tBubuleStyle);
            GUI.backgroundColor = tBackgroundColor;


            if (sPropertysGroup == true)
            {
                // check answer
                NWDDialog[] tDialogs = NextDialogs.GetObjects();
                int tI = tDialogs.Length;
                foreach (NWDDialog tAnswerDialog in tDialogs)
                {
                    //tText += "<b> Answer : "+tI+" </b>"+tAnswerDialog.Answer.GetBaseString()+"\n";
                    //if (tAnswerDialog.AnswerState == NWDDialogState.Stop)
                    //{
                    //    GUI.backgroundColor = NWDConstants.K_BLUE_BUTTON_COLOR;
                    //}
                    //else 
                    //    if (tAnswerDialog.AnswerState == NWDDialogState.Step)
                    //{
                    //    GUI.backgroundColor = NWDConstants.K_GREEN_BUTTON_COLOR;
                    //}
                    //else if (tAnswerDialog.AnswerState == NWDDialogState.Sequent)
                    //{
                    //    GUI.backgroundColor = NWDConstants.K_GRAY_BUTTON_COLOR;
                    //}
                    string tAnswerDialogAnswer = tAnswerDialog.AnswerRichTextForLanguage(tLangue, false);
                    GUIContent tContent = new GUIContent(tAnswerDialogAnswer);
                    switch (tAnswerDialog.AnswerState)
                    {
                        case NWDDialogState.Sequent:
                            {
                                tContent = new GUIContent(tAnswerDialogAnswer, kImageSequent);
                            }
                            break;
                        case NWDDialogState.Step:
                            {
                                tContent = new GUIContent(tAnswerDialogAnswer, kImageStep);
                            }
                            break;
                        case NWDDialogState.Normal:
                            {
                                tContent = new GUIContent(tAnswerDialogAnswer, kImageNormal);
                            }
                            break;
                    }


                    if (GUI.Button(new Rect(sRect.x + NWDConstants.kPrefabSize,
                                            sRect.y + sRect.height - tI * (NWDConstants.HeightButton + NWDConstants.kFieldMarge),
                                            sRect.width - NWDConstants.kFieldMarge - NWDConstants.kPrefabSize,
                                            NWDConstants.HeightButton),
                                   tContent))
                    {
                        NWDDataInspector.InspectNetWorkedData(tAnswerDialog, false, false);
                    }
                    tI--;
                    GUI.backgroundColor = tBackgroundColor;
                }
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodePropertyDraw(string sPpropertyName, Rect sRect)
        {
            LoadImages();
            GUIStyle tBox = new GUIStyle(EditorStyles.helpBox);
            tBox.alignment = TextAnchor.MiddleLeft;
            GUI.Label(sRect, sPpropertyName + " : " + InternalKey, EditorStyles.miniLabel);
            //GUI.Label(sRect, sPpropertyName+ "<"+ClassNamePHP() + "> "+InternalKey, EditorStyles.wordWrappedLabel);
            //GUI.Box(sRect, sPpropertyName + "<" + ClassNamePHP() + "> " + InternalKey, tBox);

            string tLangue = NWDNodeEditor.SharedInstance().GetLanguage();

            //float tButtonWidth = 150.0F;

            Color tBackgroundColor = GUI.backgroundColor;

            //if (AnswerState == NWDDialogState.Stop)
            //{
            //    GUI.backgroundColor = NWDConstants.K_BLUE_BUTTON_COLOR;
            //}
            //else 
            //    if (AnswerState == NWDDialogState.Step)
            //{
            //    GUI.backgroundColor = NWDConstants.K_GREEN_BUTTON_COLOR;
            //}
            //else if (AnswerState == NWDDialogState.Sequent)
            //{
            //    GUI.backgroundColor = NWDConstants.K_GRAY_BUTTON_COLOR;
            //}


            //string tAnswer = Answer.GetLanguageString(tLangue);
            //string tAnswer = AnswerRichTextForLanguage(tLangue, false);
            //GUIContent tContent = new GUIContent(tAnswer);
            //GUIContent tContent = null;
            //switch (AnswerState)
            //{
            //    case NWDDialogState.Sequent:
            //        {
            //            tContent = new GUIContent(tAnswer, kImageSequent);
            //        }
            //        break;
            //    case NWDDialogState.Step:
            //        {
            //            tContent = new GUIContent(tAnswer, kImageStep);
            //        }
            //        break;
            //    default:
            //    case NWDDialogState.Normal:
            //        {
            //            tContent = new GUIContent(tAnswer, kImageNormal);
            //        }
            //        break;
            //}
            // if (string.IsNullOrEmpty(tAnswer) == false)
            {
                //if (GUI.Button(new Rect(sRect.x + sRect.width - tButtonWidth - NWDConstants.kFieldMarge, sRect.y, tButtonWidth, NWDConstants.HeightButton), tContent))
                //{
                //    NWDDataInspector.InspectNetWorkedData(this, false, false);
                //}
            }
            GUI.backgroundColor = tBackgroundColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override Color AddOnNodeColor()
        {

            return Color.white;
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