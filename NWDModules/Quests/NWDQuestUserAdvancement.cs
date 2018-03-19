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
    public class NWDQuestUserAdvancementConnection : NWDConnection<NWDQuestUserAdvancement>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("QUA")]
    [NWDClassDescriptionAttribute("Quest User Advancement descriptions Class")]
    [NWDClassMenuNameAttribute("Quest User Advancement")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDQuestUserAdvancement : NWDBasis<NWDQuestUserAdvancement>
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
        [NWDGroupStartAttribute("Advancement information", true, true, true)]
        [NWDTooltips("Account, reference, last dialogue see and state of the quest")]
        public NWDReferenceType<NWDAccount> AccountReference
        {
            get; set;
        }
        public NWDReferenceType<NWDQuest> QuestReference
        {
            get; set;
        }
        public NWDReferenceType<NWDDialog> LastDialogReference
        {
            get; set;
        }
        public NWDQuestState QuestState
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparator]
        [NWDGroupStartAttribute("Counter", true, true, true)]
        [NWDTooltips("Quest Counter, Finish is Success and Failed")]
        public int AcceptCounter
        {
            get; set;
        }
        public int RefuseCounter
        {
            get; set;
        }
        public int CancelCounter
        {
            get; set;
        }
        public int SuccessCounter
        {
            get; set;
        }
        public int FailCounter
        {
            get; set;
        }
        [NWDGroupSeparator]
        public int FinishCounter
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuestUserAdvancement()
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDQuestUserAdvancement(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the quest advancement.
        /// </summary>
        /// <returns>The quest advancement.</returns>
        /// <param name="sQuest">Selected Quest</param>
        /// <param name="sLastDialog">Selected Dialog</param>
        public static NWDQuestUserAdvancement AdvancementForQuest(NWDQuest sQuest, NWDDialog sLastDialog)
        {
            NWDQuestUserAdvancement rAdvancement = null;
            foreach (NWDQuestUserAdvancement tAdvancement in GetAllObjects())
            {
                if (tAdvancement.QuestReference.GetObject() == sQuest)
                {
                    rAdvancement = tAdvancement;
                    break;
                }
            }

            if (rAdvancement == null)
            {
                rAdvancement = NewObject();
                //--------------
                #if UNITY_EDITOR
                //--------------
                if (sQuest.Title != null)
                {
                    string tQuestBaseName = sQuest.Title.GetBaseString();
                    if (tQuestBaseName != null)
                    {
                        rAdvancement.InternalKey = tQuestBaseName;
                    }
                }
                rAdvancement.InternalDescription = NWDUserNickname.GetNickName() + " - " + NWDUserNickname.GetUniqueNickName();
                //--------------
                #endif
                //--------------
                rAdvancement.Tag = 20; // Data generated by User-created
                rAdvancement.QuestReference.SetReference(sQuest.Reference);
            }

            // Update Quest Advancement
            rAdvancement.QuestState = sLastDialog.QuestStep;
            switch (sLastDialog.QuestStep)
            {
                case NWDQuestState.Accept:
                    rAdvancement.AcceptCounter++;
                    break;
                case NWDQuestState.Refuse:
                    rAdvancement.RefuseCounter++;
                    break;
                case NWDQuestState.Cancel:
                    rAdvancement.CancelCounter++;
                    break;
                case NWDQuestState.Success:
                    rAdvancement.SuccessCounter++;
                    rAdvancement.FinishCounter++;
                    break;
                case NWDQuestState.Failed:
                    rAdvancement.FailCounter++;
                    rAdvancement.FinishCounter++;
                    break;
            }
            rAdvancement.LastDialogReference.SetReference(sLastDialog.Reference);
            rAdvancement.SaveModifications();

            return rAdvancement;
        }
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