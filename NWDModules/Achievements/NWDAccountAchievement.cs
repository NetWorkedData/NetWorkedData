//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:14
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AAC")]
    [NWDClassDescriptionAttribute("Account Achievement")]
    [NWDClassMenuNameAttribute("Account Achievement")]
    public partial class NWDAccountAchievement : NWDBasis<NWDAccountAchievement>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account {get; set;}
		public NWDReferenceType<NWDAchievementKey> Achievement {get; set;}
		public bool Achieved {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================