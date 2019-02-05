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
    [NWDClassTrigrammeAttribute("SUA")]
    [NWDClassDescriptionAttribute("User Achievement")]
    [NWDClassMenuNameAttribute("User Achievement")]
    public partial class NWDUserAchievement : NWDBasis<NWDUserAchievement>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account {get; set;}
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
		public NWDReferenceType<NWDAchievementKey> Achievement {get; set;}
		public bool Achieved {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================