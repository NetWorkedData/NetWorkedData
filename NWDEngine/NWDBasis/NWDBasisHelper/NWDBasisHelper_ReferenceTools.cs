//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:58
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
        public void New_ChangeReferenceForAnotherInAllObjects(string sOldReference, string sNewReference)
        {
            //Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in objects of class " + ClassName ());
            New_LoadFromDatabase();
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.ChangeReferenceForAnother(sOldReference, sNewReference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        {
            New_LoadFromDatabase();
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.ChangeUser(sOldUser, sNewUser);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================