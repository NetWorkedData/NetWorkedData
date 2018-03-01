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
        Step,
        Stop,
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
    public class NWDDialogConnexion : NWDConnexion<NWDDialog>
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
        public NWDReferencesQuantityType<NWDItemGroup> ItemGroupsRequired
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsRequired
        {
            get; set;
        }
        public NWDDialogAnswerType AnswerType
        {
            get; set;
        } // default, cancel, ok ...
        public NWDDialogState AnswerState
        {
            get; set;
        } // sequent, step, finish
        public NWDQuestState QuestStep
        {
            get; set;
        }
        public NWDLocalizableStringType Answer
        {
            get; set;
        }
        [NWDGroupEnd]
        [NWDSeparator]
        [NWDGroupStartAttribute("Dialog", true, true, true)]
        public NWDReferenceType<NWDCharacter> CharacterReference
        {
            get; set;
        }
        public NWDCharacterEmotion CharacterEmotion
        {
            get; set;
        }
        public NWDLocalizableTextType Dialog
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

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog()
        {
            Debug.Log("NWDDialog Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialog(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            Debug.Log("NWDDialog Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
        public override float AddOnNodeDrawHeight()
        {
            return 200f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUI.Label(sRect, InternalDescription, EditorStyles.wordWrappedLabel);

            GUIStyle tStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            tStyle.richText = true;
            string tText = "";
            // if answer
            string tAnswer = Answer.GetBaseString();
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
                tCharacterName = tCharacter.FirstName.GetBaseString() + " " + tCharacter.LastName.GetBaseString();
            }
            string tDialog = Dialog.GetBaseString();
            tText += "<b>" + tCharacterName + "</b> says [" + tCharacterEmotion + "]: \n\n \"<i>" + tDialog + "</i>\"\n\n";

            if (sPropertysGroup == true)
            {
                // check answer
                NWDDialog[] tDialogs = NextDialogs.GetObjects();
                int tI = tDialogs.Length;
                foreach (NWDDialog tAnswerDialog in tDialogs)
                {
                    //tText += "<b> Answer : "+tI+" </b>"+tAnswerDialog.Answer.GetBaseString()+"\n";
                    Color tBackgroundColor = GUI.backgroundColor;
                    if (tAnswerDialog.AnswerState == NWDDialogState.Stop)
                    {
                        GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
                    }
                    if (GUI.Button(new Rect(sRect.x, sRect.y + sRect.height - tI * (NWDConstants.HeightButton + NWDConstants.kFieldMarge), sRect.width, NWDConstants.HeightButton), tAnswerDialog.Answer.GetBaseString()))
                    {
                        NWDDataInspector.InspectNetWorkedData(tAnswerDialog, false, false);
                    }
                    tI--;
                    GUI.backgroundColor = tBackgroundColor;
                }
            }

            // draw resume
            GUI.Label(sRect, tText, tStyle);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodePropertyDraw(string sPpropertyName, Rect sRect)
        {
            GUIStyle tBox = new GUIStyle(EditorStyles.helpBox);
            tBox.alignment = TextAnchor.MiddleLeft;
            GUI.Label(sRect, sPpropertyName + " : " + InternalKey, EditorStyles.miniLabel);
            //GUI.Label(sRect, sPpropertyName+ "<"+ClassNamePHP() + "> "+InternalKey, EditorStyles.wordWrappedLabel);
            //GUI.Box(sRect, sPpropertyName + "<" + ClassNamePHP() + "> " + InternalKey, tBox);

            float tButtonWidth = 150.0F;

            Color tBackgroundColor = GUI.backgroundColor;
            if (AnswerState == NWDDialogState.Stop)
            {
                GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            }
            if (GUI.Button(new Rect(sRect.x + sRect.width - tButtonWidth, sRect.y , tButtonWidth, NWDConstants.HeightButton), Answer.GetBaseString()))
            {
                NWDDataInspector.InspectNetWorkedData(this, false, false);
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