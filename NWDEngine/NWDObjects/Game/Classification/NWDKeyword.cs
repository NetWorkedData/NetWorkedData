// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:25
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    [NWDClassTrigrammeAttribute("KWD")]
    [NWDClassDescriptionAttribute("This class is used to reccord the keyword available in the game")]
    [NWDClassMenuNameAttribute("Keyword")]
    public partial class NWDKeyword : NWDBasis<NWDKeyword>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInformation("Use the internal key as keyword. If you need more complex classification use Category or Family!")]
        [NWDInspectorGroupStart("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================