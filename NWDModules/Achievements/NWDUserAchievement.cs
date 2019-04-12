// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:31
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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UAC")]
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