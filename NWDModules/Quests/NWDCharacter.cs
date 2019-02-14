//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CHR")]
    [NWDClassDescriptionAttribute("Character descriptions Class")]
    [NWDClassMenuNameAttribute("Character")]
    public partial class NWDCharacter : NWDBasis<NWDCharacter>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList { get; set; }
        public NWDReferencesListType<NWDCategory> CategoryList { get; set; }
        public NWDReferencesListType<NWDFamily> FamilyList { get; set; }
        public NWDReferencesListType<NWDKeyword> KeywordList { get; set; }
        [NWDGroupEndAttribute]

       

        [NWDGroupStartAttribute("Identity", true, true, true)]
        public NWDLocalizableStringType civility { get; set; }
        public NWDLocalizableStringType Job { get; set; }
        public NWDLocalizableStringType FirstName { get; set; }
        public NWDLocalizableStringType LastName { get; set; }
        public NWDLocalizableStringType NickName { get; set; }
        [NWDGroupEndAttribute]

       

        [NWDGroupStartAttribute("Dialog tempo", true, true, true)]

        public float SentenceSpeed { get; set; }
        public float DotLatence { get; set; }
        public float DotCommaLatence { get; set; }
        public float CommaLatence { get; set; }
        public float ExclamationLatence { get; set; }
        public float InterrogationLatence { get; set; }
        public float TripleDotLatence { get; set; }
        [NWDGroupEndAttribute]

       

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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================