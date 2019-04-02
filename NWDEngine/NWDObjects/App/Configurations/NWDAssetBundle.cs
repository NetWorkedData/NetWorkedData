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
    [NWDClassTrigrammeAttribute("BDL")]
    [NWDClassDescriptionAttribute("Asset Bundle Unity Managament")]
    [NWDClassMenuNameAttribute("Asset Bundle")]
    public partial class NWDAssetBundle : NWDBasis<NWDAssetBundle>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Bundle Unity3D loader")]
        public string BundleName
        {
            set; get;
        }
        public NWDPrefabType PrefabBundle
        {
            set; get;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================