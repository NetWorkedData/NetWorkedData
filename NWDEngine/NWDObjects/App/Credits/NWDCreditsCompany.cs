// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:57
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
    [NWDClassTrigrammeAttribute("CRC")]
    [NWDClassDescriptionAttribute("Credits Company")]
    [NWDClassMenuNameAttribute("Credits Company")]
    public partial class NWDCreditsCompany : NWDBasis<NWDCreditsCompany>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations")]
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