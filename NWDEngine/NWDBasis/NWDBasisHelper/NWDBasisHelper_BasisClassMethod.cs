//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:54
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
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;
using BasicToolBox;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string TableNamePHP<T>(NWDAppEnvironment sEnvironment) where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = BasisHelper<T>();
            return tHelper.PHP_TABLENAME(sEnvironment);
        }
        
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper BasisHelper<T>() where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(T));
            if (rHelper == null)
            {
                Debug.LogWarning("ERROR NWDBasisHelper.FindTypeInfos(typeof(K)) NOT RETURN FOR " + typeof(T).Name);
            }
            return tHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================