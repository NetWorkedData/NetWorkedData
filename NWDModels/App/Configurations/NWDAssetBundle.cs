//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:38
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("BDL")]
    [NWDClassDescriptionAttribute("Asset Bundle Unity Managament")]
    [NWDClassMenuNameAttribute("Asset Bundle")]
    public partial class NWDAssetBundle : NWDBasis
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