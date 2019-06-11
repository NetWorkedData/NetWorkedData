// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:1
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void New_ClassInitialization()
        {
             //Debug.Log("ClassInitialization() base method (" + GetType().FullName + ")");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void New_ClassDatasAreLoaded()
        {
            //Debug.Log("ClassDatasAreLoaded() base method (" + GetType().FullName + ")");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual List<Type> New_OverrideClasseInThisSync()
        {
            //Debug.Log("New_OverrideClasseInThisSync() base method (" + GetType().FullName + ")");
            return new List<Type>() { ClassType };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================