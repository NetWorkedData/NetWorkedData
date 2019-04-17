// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:29
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(false)]// ALLWAYS FALSE! Never sync this data! It's local only
    [NWDClassTrigrammeAttribute("BSP")]
    [NWDClassDescriptionAttribute("Basis Preferences Class! Never use by yourself in game!")]
    [NWDClassMenuNameAttribute("Basis Preferences")]
    public partial class NWDBasisPreferences : NWDBasis<NWDBasisPreferences>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDCertified]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        [NWDCertified]
        public string Environment
        {
            get; set;
        }
        [NWDCertified]
        public string StringValue
        {
            get; set;
        }
        [NWDCertified]
        public int IntValue
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
