//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_QUEST
using System;
using System.Collections;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDCharacterEmotion
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
    public partial class NWDCharacter : NWDBasis
    {
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
        public static string GetNickname(string sReference)
        {
            NWDCharacter tCharacter = NWDBasisHelper.GetRawDataByReference<NWDCharacter>(sReference);
            string tNickname = "???";
            if (tCharacter != null)
            {
                tNickname = tCharacter.NickName.GetLocalString();
            }

            return tNickname;
        }
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
            string rText = sText.Replace("#F" + sCpt + NWEConstants.K_HASHTAG, tBstart + FirstName + tBend);

            // Replace Tag by Character Last Name
            rText = rText.Replace("#L" + sCpt + NWEConstants.K_HASHTAG, tBstart + LastName + tBend);

            // Replace Tag by Character Nickname
            rText = rText.Replace("#N" + sCpt + NWEConstants.K_HASHTAG, tBstart + NickName + tBend);

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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif