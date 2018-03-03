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
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDQuestState : int
    {
        None,
        Start,
        StartAlternate,
        Accept,
        Refuse,
        Success,
        Cancel,
        Failed,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDDialogState : int
    {
        Sequent,
        Normal,
        Step,
        //Stop,
        Random,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDDialogAnswerType : int
    {
        None,
        Default,
        Cancel,
        Validate,
        Destructive,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum NWDBubbleStyleType : int
    {
        Speech,
        Whisper,
        Thought,
        Scream,

    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDDialogConnection : NWDConnection<NWDDialog>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("DLG")]
    [NWDClassDescriptionAttribute("Dialog descriptions Class")]
    [NWDClassMenuNameAttribute("Dialog")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDDialog : NWDBasis<NWDDialog>
    {
        //-------------------------------------------------------------------------------------------------------------
        //#warning YOU MUST FOLLOW THIS INSTRUCTIONS
        //-------------------------------------------------------------------------------------------------------------
        // YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
        // YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
        // YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDGroupStartAttribute("Reply for preview Dialog (optional)", true, true, true)]
        [NWDTooltipsAttribute("The list and quantity of ItemGroup required to show this answer and this dialog")]
        public NWDReferencesQuantityType<NWDItemGroup> ItemGroupsRequired
        {
            get; set;
        }
        [NWDTooltipsAttribute("The list and quantity of Item required to show this answer and this dialog")]
        public NWDReferencesQuantityType<NWDItem> ItemsRequired
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
                              "\n •Sequent (just continue next, perhaps not show button)," +
                              "\n •Normal (choose answer),  " +
                              "\n •Step ( the next time restart quest here), " +
                              //"\n •Stop (stop the dialog after this answer), " +
                              "\n •Random (randomly show answer)" +
                              "")]
        public NWDDialogState AnswerState
        {
            get; set;
        }
        [NWDTooltipsAttribute("The random limit in range to [0-1] ")]
        [NWDIf("AnswerState", (int)NWDDialogState.Random)]
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
        [NWDGroupEnd]
        [NWDSeparator]
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
        [NWDTooltipsAttribute("Select language and write your dialog in this language")]
        public NWDLocalizableTextType Dialog
        {
            get; set;
        }

        [NWDTooltipsAttribute("Select characters to use in dialog by these tags" +
                              "\n •for Fistname : #F0# #F1# …" +
                              "\n •for Lastname : #L0# #L1# …" +
                              "\n •for Nickname : #N0# #N1# …" +
                              "")]
        public NWDReferencesListType<NWDCharacter> ReplaceCharacters
        {
            get; set;
        }

        [NWDTooltipsAttribute("Select items to use in dialog by these tags" +
                              "\n •for item name #I0# #I1# …" +
                              "\n •for item plural name #IS0# #IS1# …" +
                              "")]
        public NWDReferencesListType<NWDItem> ReplaceItems
        {
            get; set;
        }

        [NWDTooltipsAttribute("Select itemgroups to use item to describe the group in dialog by these " +
                              "\n •for item to describe name tags #G0# #G1# …" +
                              "\n •for item to describe plural name #GS0# #GS1# …" +
                              "")]
        public NWDReferencesListType<NWDItemGroup> ReplaceItemGroups
        {
            get; set;
        }

        [NWDGroupEndAttribute]
        [NWDSeparator]
        [NWDGroupStartAttribute("List of next dialogs (and replies)", true, true, true)]
        public NWDReferencesListType<NWDDialog> NextDialogs
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparator]
        [NWDGroupStartAttribute("Option Quest", true, true, true)]
        [NWDTooltipsAttribute("The quest launched after this dialog")]
        public NWDReferenceType<NWDQuest> NextQuest
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
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AnswerRichText(bool sBold = true)
        {
            string rReturn = Answer.GetLocalString();
            rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string AnswerRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = Answer.GetLanguageString(sLanguage);
            rReturn = Enrichment(rReturn, sLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DialogRichText(bool sBold = true)
        {
            string rReturn = Dialog.GetLocalString();
            rReturn = Enrichment(rReturn, NWDDataManager.SharedInstance().PlayerLanguage, sBold);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DialogRichTextForLanguage(string sLanguage, bool sBold = true)
        {
            string rReturn = Dialog.GetLanguageString(sLanguage);
            rReturn = Enrichment(rReturn, sLanguage, sBold);
            return rReturn;
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
            if (ReplaceCharacters != null)
            {
                tCounter = 0;
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
                tCounter = 0;
                foreach (NWDItem tItem in ReplaceItems.GetObjects())
                {
                    if (tItem.Name != null)
                    {
                        string tName = tItem.Name.GetLanguageString(sLanguage);
                        if (tName != null)
                        {
                            rText = rText.Replace("#I" + tCounter.ToString() + "#", tBstart + tName + tBend);
                        }
                    }
                    if (tItem.PluralName != null)
                    {
                        string tPluralName = tItem.PluralName.GetLanguageString(sLanguage);
                        if (tPluralName != null)
                        {
                            rText = rText.Replace("#IS" + tCounter.ToString() + "#", tBstart + tPluralName + tBend);
                        }
                    }
                    tCounter++;
                }
            }
            if (ReplaceItemGroups != null)
            {
                tCounter = 0;
                foreach (NWDItemGroup tItemGroup in ReplaceItemGroups.GetObjects())
                {
                    NWDItem tItem = tItemGroup.ItemToDescribe.GetObject();
                    if (tItem != null)
                    {
                        if (tItem.Name != null)
                        {
                            string tName = tItem.Name.GetLanguageString(sLanguage);
                            if (tName != null)
                            {
                                rText = rText.Replace("#G" + tCounter.ToString() + "#", tBstart + tName + tBend);
                            }
                        }
                        if (tItem.PluralName != null)
                        {
                            string tPluralName = tItem.PluralName.GetLanguageString(sLanguage);
                            if (tPluralName != null)
                            {
                                rText = rText.Replace("#GS" + tCounter.ToString() + "#", tBstart + tPluralName + tBend);
                            }
                        }
                    }
                    tCounter++;
                }
            }
            return rText;
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

            GUI.Label(sRect, InternalDescription, EditorStyles.wordWrappedLabel);

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
                        case NWDDialogState.Random:
                            {
                                tContent = new GUIContent(tAnswerDialogAnswer, kImageRandom);
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

            float tButtonWidth = 150.0F;

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
            string tAnswer = AnswerRichTextForLanguage(tLangue, false);
            GUIContent tContent = new GUIContent(tAnswer);
            switch (AnswerState)
            {
                case NWDDialogState.Sequent:
                    {
                        tContent = new GUIContent(tAnswer, kImageSequent);
                    }
                    break;
                case NWDDialogState.Step:
                    {
                        tContent = new GUIContent(tAnswer, kImageStep);
                    }
                    break;
                case NWDDialogState.Normal:
                    {
                        tContent = new GUIContent(tAnswer, kImageNormal);
                    }
                    break;
                case NWDDialogState.Random:
                    {
                        tContent = new GUIContent(tAnswer, kImageRandom);
                    }
                    break;
            }
            // if (string.IsNullOrEmpty(tAnswer) == false)
            {
                if (GUI.Button(new Rect(sRect.x + sRect.width - tButtonWidth - NWDConstants.kFieldMarge, sRect.y, tButtonWidth, NWDConstants.HeightButton), tContent))
                {
                    NWDDataInspector.InspectNetWorkedData(this, false, false);
                }
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
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================