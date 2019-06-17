//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:51:6
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

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDRelationshipPlaceHelper : NWDHelper<NWDRelationshipPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
public override List<Type> New_OverrideClasseInThisSync()
        {
            List<Type> tClasses = new List<Type> { typeof(NWDUserRelationship), typeof(NWDAccountRelationship), typeof(NWDRelationshipPlace) };
            foreach (NWDRelationshipPlace tPlace in NWDRelationshipPlace.GetReachableDatas())
            {
                tClasses.AddRange(tPlace.ClassesSharedToStartRelation.GetClassesTypeList());
                tClasses.AddRange(tPlace.ClassesShared.GetClassesTypeList());
            }
            return tClasses;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================