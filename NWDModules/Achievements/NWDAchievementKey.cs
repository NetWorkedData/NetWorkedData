// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:24
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAchievementFlag : int
    {
        stepOne = 1 << 0,
        stepTwo = 1 << 1,
        stepThree = 1 << 2,
        stepFour = 1 << 3,
        stepFive = 1 << 4,
        stepSix = 1 << 5,
        stepSeven = 1 << 6,
        stepHeight = 1 << 7,
        stepNine = 1 << 8,
        stepTen = 1 << 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ACK")]
    [NWDClassDescriptionAttribute("Achievement")]
    [NWDClassMenuNameAttribute("Achievement")]
    public partial class NWDAchievementKey : NWDBasis<NWDAchievementKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        public NWDGameDomain Domain
        {
            get; set;
        }
        public bool PermanentAcquisition
        {
            get; set;
        }
        public int Style
        {
            get; set;
        }
        public float Total
        {
            get; set;
        }
        public float ProportionalValue
        {
            get; set;
        }
        public NWDReferencesConditionalType<NWDItem> ItemToActive
        {
            get; set;
        }

        [NWDFlagsEnum]
        public NWDAchievementFlag MatchRequired
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================