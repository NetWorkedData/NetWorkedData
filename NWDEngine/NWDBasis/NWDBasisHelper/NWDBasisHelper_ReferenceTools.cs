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
            LoadFromDatabase(string.Empty, false);
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.ChangeReferenceForAnother(sOldReference, sNewReference);
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        {
            if (kAccountDependent == true)
            {
                // look for old data , delete on database, change reference, reccord on database
                string tOldUniqueReference = NWDAccount.GetUniqueReference(sOldUser, ClassType);
                string tNewUniqueReference = NWDAccount.GetUniqueReference(sNewUser, ClassType);
                Debug.Log("########### OK I WILL replace THE UNIQUE REFERENCE " + tOldUniqueReference);
                NWDTypeClass tUniqueReference = GetDataByReference(sOldUser);
                if (tUniqueReference != null)
                {
                    Debug.Log("###########  ... BY THE UNIQUE REFERENCE " + tNewUniqueReference);
                    tUniqueReference.DeleteData();
                    Debug.Log("###########  I deleteThe Data");
                    NWDDataManager.SharedInstance().DataQueueExecute();
                    tUniqueReference.Reference = tNewUniqueReference;
                    tUniqueReference.InsertData();
                    //tUniqueReference.EnableData();
                }
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