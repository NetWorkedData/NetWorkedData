//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq.Expressions;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public static K GetDataByReference(string sReference, string sAccountReference = null)
        {
            BTBBenchmark.Start();
            K rReturn = null;
            int tIndex = ObjectsByReferenceList.IndexOf(sReference);
            if (tIndex >= 0)
            {
                K tObject = ObjectsList.ElementAt(tIndex) as K;
                if (tObject.IsReacheableByAccount(sAccountReference))
                {
                    rReturn = tObject;
                }
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K UniqueDataByInternalKey(string sInternalKey,
                                             bool sCreateIfNotExists = false,
                                             NWDWritingMode sWritingMode = NWDWritingMode.MainThread,
                                             string sInternalDescription = "",
                                             bool sFlushOlderDupplicate = false)
        {
            BTBBenchmark.Start();
            K rReturn = null;
            // We cannot use ObjectsByKeyList to find Internal key because the objetc is pehaps lock fo this user
            // Must use the GetAllObjects(null) and chekc the good object
            if (ObjectsByKeyList.Contains(sInternalKey) == false)
            {
                // no return :-/
            }
            else
            {
                K[] tArray = GetAllObjects(null);
                List<K> tAllList = new List<K>();
                foreach (K tObject in tArray)
                {
                    if (tObject.InternalKey == sInternalKey)
                    {
                        tAllList.Add(tObject);
                        if (sFlushOlderDupplicate == false)
                        {
                            break; // stop to look for the internal key, 
                            //it's not necessary because We will ot flush the oldest object
                        }
                    }
                }
                K[] rReturnArray = tAllList.ToArray();
                //Debug.Log("NWDBasis<K> Workflow GetObjectByInternalKeyOrCreate() rReturnArray.Length = " + rReturnArray.Length.ToString());
                if (rReturnArray.Length > 0)
                {
                    //Debug.Log("NWDBasis<K> Workflow GetObjectByInternalKeyOrCreate() I have some return");
                    rReturn = rReturnArray[0];
                }
                if (rReturnArray.Length > 1)
                {
                    //Debug.Log("!!!The InternalKey " + sInternalKey + " for " + ClassNamePHP() + " is not unique!");
                    if (sFlushOlderDupplicate == true)
                    {
                        List<K> tList = new List<K>();
                        foreach (K tObject in rReturnArray)
                        {
                            if (tObject.IsTrashed() == false)
                            {
                                tList.Add(tObject);
                            }
                        }
                        tList.Sort((x, y) => y.DM.CompareTo(x.DM));
                        for (int i = 1; i < tList.Count; i++)
                        {
                            tList[i].TrashMe();
                        }
                        rReturn = tList[0];
                    }
                }
            }
            if (rReturn == null && sCreateIfNotExists == true)
            {
                rReturn = NWDBasis<K>.NewData(sWritingMode);
                rReturn.InternalKey = sInternalKey;
                rReturn.InternalDescription = sInternalDescription;
                rReturn.UpdateData(true, sWritingMode);
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================