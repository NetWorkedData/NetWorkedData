//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ULS")]
    [NWDClassDescriptionAttribute("Level's Score descriptions Class")]
    [NWDClassMenuNameAttribute("Level's Score")]
    public partial class NWDUserLevelScore : NWDBasis<NWDUserLevelScore>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDLevel> Level
        {
            get; set;
        }
        public int NumberOfPlay
        {
            get; set;
        }
        public int NumberOfFinish
        {
            get; set;
        }
        public int NumberOfCancel
        {
            get; set;
        }
        public int NumberOfFailed
        {
            get; set;
        }
        public float BestScore
        {
            get; set;
        }
        public float MiddleScore
        {
            get; set;
        }
        public int NumberOfStars
        {
            get; set;
        }
        public NWDDateTimeType DateLastGame
        {
            get; set;
        }
        public NWDDateTimeType DateLastSuccess
        {
            get; set;
        }
        public NWDDateTimeType DateLastFailed
        {
            get; set;
        }
        public int Ranking
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================