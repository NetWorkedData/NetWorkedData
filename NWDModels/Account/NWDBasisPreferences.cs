//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    ////[NWDClassServerSynchronizeAttribute(false)]// ALLWAYS FALSE! Never sync this data! It's local only
    [NWDClassTrigrammeAttribute("BSP")]
    [NWDClassDescriptionAttribute("Basis Preferences Class! Never use by yourself in game!")]
    [NWDClassMenuNameAttribute("Basis Preferences")]
    //public partial class NWDBasisPreferences : NWDBasisAccountUnsynchronize
    public partial class NWDBasisPreferences : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        //[NWDCertified]
        //public string Environment
        //{
        //    get; set;
        //}
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
