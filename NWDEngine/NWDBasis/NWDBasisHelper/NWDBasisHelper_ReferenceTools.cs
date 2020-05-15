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
        public void TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        {
            Debug.Log("TryToChangeUserForAllObjects(string sOldUser, string sNewUser) in " + ClassNamePHP + "");
            if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            {
                // look for old data , delete on database, change reference, reccord on database
                string tOldUniqueReference = NWDAccount.GetUniqueReference(sOldUser, ClassType);
                string tNewUniqueReference = NWDAccount.GetUniqueReference(sNewUser, ClassType);
                Debug.Log("########### OK I WILL replace THE UNIQUE REFERENCE in "+ClassNamePHP+" for " + tOldUniqueReference);
                NWDTypeClass tUniqueReference = GetDataByReference(tOldUniqueReference);
                if (tUniqueReference != null)
                {
                    Debug.Log("###########  ... in " + ClassNamePHP + " for BY THE UNIQUE REFERENCE " + tNewUniqueReference);
                    tUniqueReference.DeleteData();
                    Debug.Log("###########  I delete the Data in " + ClassNamePHP + " and reinsert it with new reference!");
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