//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("LCL")]
    [NWDClassDescriptionAttribute("Localization are used to localize the string of your game.\n" +
                                   "It's dependent from the \"Localization\" menu items in editor.\n" +
                                   "")]
    [NWDClassMenuNameAttribute("Localization")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDLocalization : NWDBasis<NWDLocalization>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Localization", true, true, true)]
        [NWDTooltips("The localizable value")]
        public NWDLocalizableLongTextType TextValue
        {
            get; set;
        }
        [NWDTooltips("The Key to use to replace by the value use something like {xxxxx} or #xxxx# empty if localization is not use as autoreplace value")]
        public string KeyValue
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        
        [NWDGroupStartAttribute("Development addons", true, true, true)]
        /// <summary>
        /// Gets or sets the annexe value.
        /// </summary>
        /// <value>The annexe value.</value>
        public NWDMultiType AnnexeValue
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================