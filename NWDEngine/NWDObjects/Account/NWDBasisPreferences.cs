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
    [NWDClassServerSynchronizeAttribute(false)]
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
