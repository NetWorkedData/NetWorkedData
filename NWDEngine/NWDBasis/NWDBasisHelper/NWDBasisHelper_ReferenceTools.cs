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
#if UNITY_EDITOR
        public void ChangeReferenceForAnotherInAllObjects(string sOldReference, string sNewReference)
        {
            //Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in objects of class " + ClassName ());
            LoadFromDatabase(string.Empty);
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.ChangeReferenceForAnother(sOldReference, sNewReference);
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        {
            //Debug.Log("##### TryToChangeUserForAllObjects");
            if (kAccountDependent == true)
            {
                foreach (NWDTypeClass tObject in Datas)
                {
                    tObject.ChangeUser(sOldUser, sNewUser);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================