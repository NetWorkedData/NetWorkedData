//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CRD")]
    [NWDClassDescriptionAttribute("Credits Company")]
    [NWDClassMenuNameAttribute("Credits Company")]
    public partial class NWDCreditsCompany : NWDBasis<NWDCreditsCompany>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Informations")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDSpriteType Logo
        {
            get; set;
        }
        public NWDLocalizableStringType Website
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================