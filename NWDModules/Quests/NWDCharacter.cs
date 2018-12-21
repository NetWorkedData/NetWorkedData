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
    public class NWDCharacterConnection : NWDConnection<NWDCharacter>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CHR")]
    [NWDClassDescriptionAttribute("Character descriptions Class")]
    [NWDClassMenuNameAttribute("Character")]
    public partial class NWDCharacter : NWDBasis<NWDCharacter>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList { get; set; }
        public NWDReferencesListType<NWDCategory> CategoryList { get; set; }
        public NWDReferencesListType<NWDFamily> FamilyList { get; set; }
        public NWDReferencesListType<NWDKeyword> KeywordList { get; set; }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Identity", true, true, true)]
        public NWDLocalizableStringType civility { get; set; }
        public NWDLocalizableStringType Job { get; set; }
        public NWDLocalizableStringType FirstName { get; set; }
        public NWDLocalizableStringType LastName { get; set; }
        public NWDLocalizableStringType NickName { get; set; }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Dialog tempo", true, true, true)]

        public float SentenceSpeed { get; set; }
        public float DotLatence { get; set; }
        public float DotCommaLatence { get; set; }
        public float CommaLatence { get; set; }
        public float ExclamationLatence { get; set; }
        public float InterrogationLatence { get; set; }
        public float TripleDotLatence { get; set; }
        [NWDGroupEndAttribute]

        [NWDGroupSeparator]

        [NWDGroupStartAttribute("Render", true, true, true)]
        public NWDSpriteType Portrait { get; set; }
        public NWDSpriteType NormalState { get; set; }
        public NWDSpriteType AfraidState { get; set; }
        public NWDSpriteType AngryState { get; set; }
        public NWDSpriteType AnnoyedState { get; set; }
        public NWDSpriteType BoredState { get; set; }
        public NWDSpriteType ConfidentState { get; set; }
        public NWDSpriteType ConfusedState { get; set; }
        public NWDSpriteType DisgustedState { get; set; }
        public NWDSpriteType EmbarrassedState { get; set; }
        public NWDSpriteType ExcitedState { get; set; }
        public NWDSpriteType FrustratedState { get; set; }
        public NWDSpriteType HappyState { get; set; }
        public NWDSpriteType HopefulState { get; set; }
        public NWDSpriteType HurtState { get; set; }
        public NWDSpriteType LonelyState { get; set; }
        public NWDSpriteType OverwhelmedState { get; set; }
        public NWDSpriteType PoisonedState { get; set; }
        public NWDSpriteType PossessesState { get; set; }
        public NWDSpriteType ProudState { get; set; }
        public NWDSpriteType RelaxedState { get; set; }
        public NWDSpriteType SadState { get; set; }
        public NWDSpriteType ShyState { get; set; }
        public NWDSpriteType SillyState { get; set; }
        public NWDSpriteType SurprisedState { get; set; }
        public NWDSpriteType ThoughtfulState { get; set; }
        public NWDSpriteType TiredState { get; set; }
        public NWDSpriteType WorriedState { get; set; }
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
        public static string GetNickname(string sReference)
        {
            NWDCharacter tCharacter = GetDataByReference(sReference);
            string tNickname = "???";
            if (tCharacter != null)
            {
                tNickname = tCharacter.NickName.GetLocalString();
            }

            return tNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public string GetNickname()
        {
            return NickName.GetLocalString();
        }
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
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, int sCpt = 0, string sLanguage = null, bool sBold = true)
        {
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            // Replace Tag by Character First Name
            string rText = sText.Replace("#F" + sCpt + BTBConstants.K_HASHTAG, tBstart + FirstName + tBend);

            // Replace Tag by Character Last Name
            rText = rText.Replace("#L" + sCpt + BTBConstants.K_HASHTAG, tBstart + LastName + tBend);

            // Replace Tag by Character Nickname
            rText = rText.Replace("#N" + sCpt + BTBConstants.K_HASHTAG, tBstart + NickName + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite GetSpriteForEmotion(NWDCharacterEmotion sEmotion)
        {
            Sprite rSprite = NormalState.ToSprite();
            Sprite rSpriteEmotion = null;
            switch (sEmotion)
            {
                case NWDCharacterEmotion.Normal :
                    {
                        rSpriteEmotion = NormalState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Afraid:
                    {
                        rSpriteEmotion = AfraidState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Angry:
                    {
                        rSpriteEmotion = AngryState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Annoyed:
                    {
                        rSpriteEmotion = AnnoyedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Bored:
                    {
                        rSpriteEmotion = BoredState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Confident:
                    {
                        rSpriteEmotion = ConfidentState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Confused:
                    {
                        rSpriteEmotion = ConfusedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Disgusted:
                    {
                        rSpriteEmotion = DisgustedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Embarrassed:
                    {
                        rSpriteEmotion = EmbarrassedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Excited:
                    {
                        rSpriteEmotion = ExcitedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Frustrated:
                    {
                        rSpriteEmotion = FrustratedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Happy:
                    {
                        rSpriteEmotion = HappyState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Hopeful:
                    {
                        rSpriteEmotion = HopefulState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Hurt:
                    {
                        rSpriteEmotion = HurtState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Lonely:
                    {
                        rSpriteEmotion = LonelyState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Overwhelmed:
                    {
                        rSpriteEmotion = OverwhelmedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Poisoned:
                    {
                        rSpriteEmotion = PoisonedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Possesses:
                    {
                        rSpriteEmotion = PossessesState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Proud:
                    {
                        rSpriteEmotion = ProudState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Relaxed:
                    {
                        rSpriteEmotion = RelaxedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Sad:
                    {
                        rSpriteEmotion = SadState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Shy:
                    {
                        rSpriteEmotion = ShyState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Silly:
                    {
                        rSpriteEmotion = SillyState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Surprised:
                    {
                        rSpriteEmotion = SurprisedState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Thoughtful  :
                    {
                        rSpriteEmotion = ThoughtfulState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion.Tired  :
                    {
                        rSpriteEmotion = TiredState.ToSprite();
                    }
                    break;
                case NWDCharacterEmotion. Worried  :
                    {
                        rSpriteEmotion = WorriedState.ToSprite();
                    }
                    break;
            }
            if (rSpriteEmotion != null)
            {
                rSprite = rSpriteEmotion;
            }
            return rSprite;
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