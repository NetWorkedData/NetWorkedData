//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:51:31
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

using BasicToolBox;
using UnityEngine.Analytics;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserStatisticHelper : NWDHelper<NWDUserStatistic>
    {
        //-------------------------------------------------------------------------------------------------------------
public override List<Type>  OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserStatistic), typeof(NWDAccountStatistic), typeof(NWDStatisticKey) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================