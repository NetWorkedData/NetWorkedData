//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:47
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("LCL")]
    [NWDClassDescriptionAttribute("Localization are used to localize the string of your game.\n" +
                                   "It's dependent from the \"Localization\" menu items in editor.\n" +
                                   "")]
    [NWDClassMenuNameAttribute("Localization")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Localization", true, true, true)]
        [NWDInformation("Use Dialog or Message for more complex localization.")]
        
        [NWDCertified]
        [NWDTooltips("The localizable value")]
        public NWDLocalizableLongTextType TextValue
        {
            get; set;
        }

        [NWDCertified]
        [NWDTooltips("The Key to use to replace by the value use something like {xxxxx} or #xxxx# empty if localization is not use as autoreplace value")]
        public string KeyValue
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================