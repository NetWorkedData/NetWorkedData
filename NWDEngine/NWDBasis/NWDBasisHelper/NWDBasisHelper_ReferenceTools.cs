//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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