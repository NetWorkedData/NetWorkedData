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
    public enum NWDQuestType : int
    {
        Unique = 0,
        Multiple = 1,
        Infiny = 9,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDQuestConnection : NWDConnection<NWDQuest>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("QST")]
    [NWDClassDescriptionAttribute("Quest descriptions Class")]
    [NWDClassMenuNameAttribute("Quest")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDQuest : NWDBasis<NWDQuest>
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
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> Categories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> Families
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> Keywords
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Type of quest", true, true, true)]
        [NWDTooltips("Determine if quest is replayable or not")]
        public NWDQuestType Type
        {
            get; set;
        }
        [NWDIf("Type",1)]
        public int Number
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Availability schedule", true, true, true)]
        [NWDTooltips("Determine the availability schedule of this quest")]
        public NWDDateTimeScheduleType AvailabilitySchedule
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Quest's Description (in Quest's Book)", true, true, true)]
        [NWDEntitled("Title", "Title of the quest in the description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableStringType SubTitle
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Items required to start quest", true, true, true)]
        public NWDReferencesQuantityType<NWDItemGroup> ItemGroupsRequired
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsRequired
        {
            get; set;
        }
        [NWDTooltips("If you have not the requiered item")]
        public NWDReferenceType<NWDDialog> NoItemDialogReference { get; set; }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Quest First Dialog", true, true, true)]
        [NWDEntitled("Normal Dialog")]
        public NWDReferenceType<NWDDialog> DialogReference
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Items wanted to finish quest", true, true, true)]
        public NWDReferencesQuantityType<NWDItemGroup> ItemGroupsWanted
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsWanted
        {
            get; set;
        }
        [NWDEntitled("Alternate Dialog")]
        [NWDTooltips("If you have allready the wanted item")]
        public NWDReferenceType<NWDDialog> AlternateDialogReference
        {
            get; set;
        } // to start with ListOfItemsAsked
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Quest reward", true, true, true)]
        public NWDReferencesQuantityType<NWDPack> PackRewards
        {
            get; set;
        }
        public int NumberOfRewards
        {
            get; set;
        }
        public bool ItemsAskedRemove
        {
            get; set;
        } // if quest is finish Item asked are remove from chest
          //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuest()
        {
            //Debug.Log("NWDQuest Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDQuest Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
            return 250.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            tStyle.richText = true;
            string tQuestTitle = Title.GetBaseString();
            string tQuestDescription = Description.GetBaseString();
            string tText = "" + InternalDescription + "\n\n<b>Title : </b>\n" + tQuestTitle + "\n\n<b>Description : </b>\n"+ tQuestDescription+"\n";
            GUI.Label(sRect, tText, tStyle);
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