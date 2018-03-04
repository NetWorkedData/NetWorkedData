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
    public enum NWDCharacterEmotion : int
    {
        Normal = 0,
        Afraid = 1,
        Angry = 2,
        Annoyed = 3,
        Bored = 4,
        Confident = 5,
        Confused = 6,
        Disgusted = 7,
        Embarrassed = 8,
        Excited = 9,
        Frustrated = 10,
        Happy = 11,
        Hopeful = 12,
        Hurt = 13,
        Lonely = 14,
        Overwhelmed = 15,
        Poisoned = 16,
        Possesses = 17,
        Proud = 18,
        Relaxed = 19,
        Sad = 20,
        Shy = 21,
        Silly = 22,
        Surprised = 23,
        Thoughtful = 24,
        Tired = 25,
        Worried = 26,

        // Add new emotion here ... start at 30

    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public class NWDCharacterConnection : NWDConnection<NWDCharacter>
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CHR")]
    [NWDClassDescriptionAttribute("Character descriptions Class")]
    [NWDClassMenuNameAttribute("Character")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDCharacter : NWDBasis<NWDCharacter>
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
        [NWDSeparator]
        [NWDGroupStartAttribute("Identity", true, true, true)]

        public NWDLocalizableStringType FirstName
        {
            get; set;
        }

        public NWDLocalizableStringType LastName
        {
            get; set;
        }

        public NWDLocalizableStringType NickName
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparator]
        [NWDGroupStartAttribute("Dialog tempo", true, true, true)]

        public float SentenceSpeed
        {
            get; set;
        }
        public float DotLatence
        {
            get; set;
        }
        public float DotCommaLatence
        {
            get; set;
        }
        public float CommaLatence
        {
            get; set;
        }
        public float ExclamationLatence
        {
            get; set;
        }
        public float InterrogationLatence
        {
            get; set;
        }
        public float TripleDotLatence
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparator]
        [NWDGroupStartAttribute("Render", true, true, true)]
        public NWDPrefabType NormalState
        {
            get; set;
        }
        public NWDPrefabType AfraidState
        {
            get; set;
        }
        public NWDPrefabType AngryState
        {
            get; set;
        }
        public NWDPrefabType AnnoyedState
        {
            get; set;
        }
        public NWDPrefabType BoredState
        {
            get; set;
        }
        public NWDPrefabType ConfidentState
        {
            get; set;
        }
        public NWDPrefabType ConfusedState
        {
            get; set;
        }
        public NWDPrefabType DisgustedState
        {
            get; set;
        }
        public NWDPrefabType EmbarrassedState
        {
            get; set;
        }
        public NWDPrefabType ExcitedState
        {
            get; set;
        }
        public NWDPrefabType FrustratedState
        {
            get; set;
        }
        public NWDPrefabType HappyState
        {
            get; set;
        }
        public NWDPrefabType HopefulState
        {
            get; set;
        }
        public NWDPrefabType HurtState
        {
            get; set;
        }
        public NWDPrefabType LonelyState
        {
            get; set;
        }
        public NWDPrefabType OverwhelmedState
        {
            get; set;
        }
        public NWDPrefabType PoisonedState
        {
            get; set;
        }
        public NWDPrefabType PossessesState
        {
            get; set;
        }
        public NWDPrefabType ProudState
        {
            get; set;
        }
        public NWDPrefabType RelaxedState
        {
            get; set;
        }
        public NWDPrefabType SadState
        {
            get; set;
        }
        public NWDPrefabType ShyState
        {
            get; set;
        }
        public NWDPrefabType SillyState
        {
            get; set;
        }
        public NWDPrefabType SurprisedState
        {
            get; set;
        }
        public NWDPrefabType ThoughtfulState
        {
            get; set;
        }
        public NWDPrefabType TiredState
        {
            get; set;
        }
        public NWDPrefabType WorrieState
        {
            get; set;
        }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDCharacter()
        {
            //Debug.Log("NWDCharacter Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCharacter(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCharacter Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
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
        public override void Initialization()
        {
            SentenceSpeed = 1.0F;
            DotLatence = 0.25F;
            DotCommaLatence = 0.25F;
            CommaLatence = 0.15F;
            ExclamationLatence = 0.25F;
            InterrogationLatence = 0.505F;
            TripleDotLatence = 1.0F;
            UpdateMe();
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
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 350.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 200f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            DrawPreviewTexture2D(new Rect(sRect.x + NWDConstants.kFieldMarge, sRect.y + NWDConstants.kFieldMarge, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize));
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