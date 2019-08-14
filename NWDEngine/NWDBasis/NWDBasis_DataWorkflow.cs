//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:57
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
using System.Linq.Expressions;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllDatas(string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (K tObject in NWDBasis.Datas().ObjectsList)
        //    {
        //        if (tObject.IsReacheableByAccount(sAccountReference))
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllDatasInGameSave(string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (K tObject in NWDBasis.Datas().ObjectsList)
        //    {
        //        if (tObject.IsReacheableByAccount(sAccountReference))
        //        {
        //            if (tObject.InGameSaveState())
        //            {
        //                rReturn.Add(tObject);
        //            }
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Gets the data by reference.
        ///// </summary>
        ///// <returns>The data by reference.</returns>
        ///// <param name="sReference">S reference.</param>
        ///// <param name="sAccountReference">S account reference.</param>
        //public static K GetDataByReference(string sReference, string sAccountReference = null)
        //{
        //    BTBBenchmark.Start();
        //    K rReturn = null;
        //    int tIndex = Datas().ObjectsByReferenceList.IndexOf(sReference);
        //    if (tIndex >= 0)
        //    {
        //        K tObject = Datas().ObjectsList.ElementAt(tIndex) as K;
        //        if (tObject.IsReacheableByAccount(sAccountReference))
        //        {
        //            rReturn = tObject;
        //        }
        //    }
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get Unique data by internal key.
        ///// </summary>
        ///// <returns>The data by internal key.</returns>
        ///// <param name="sInternalKey">S internal key.</param>
        ///// <param name="sCreateIfNotExists">If set to <c>true</c> s create if not exists.</param>
        ///// <param name="sWritingMode">S writing mode.</param>
        ///// <param name="sInternalDescription">S internal description.</param>
        ///// <param name="sFlushOlderDupplicate">If set to <c>true</c> s flush older dupplicate.</param>
        //public static K UniqueDataByInternalKey(string sInternalKey,
        //                                     bool sCreateIfNotExists = false,
        //                                     NWDWritingMode sWritingMode = NWDWritingMode.ByDefault,
        //                                     string sInternalDescription = "", 
        //                                     string sAccountReference = null,
        //                                     bool sFlushOlderDupplicate = false)
        //{
        //    BTBBenchmark.Start();
        //    K rReturn = null;
        //    // We cannot use ObjectsByKeyList to find Internal key because the objetc is pehaps lock fo this user
        //    // Must use the GetAllObjects(null) and chekc the good object
        //    if (Datas().ObjectsByKeyList.Contains(sInternalKey) == false)
        //    {
        //        // no return :-/
        //    }
        //    else
        //    {
        //        K[] tArray = GetAllObjects(sAccountReference);
        //        List<K> tAllList = new List<K>();
        //        foreach (K tObject in tArray)
        //        {
        //            if (tObject.InternalKey == sInternalKey)
        //            {
        //                tAllList.Add(tObject);
        //                if (sFlushOlderDupplicate == false)
        //                {
        //                    break; // stop to look for the internal key, 
        //                    //it's not necessary because We will ot flush the oldest object
        //                }
        //            }
        //        }
        //        K[] rReturnArray = tAllList.ToArray();
        //        //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate() rReturnArray.Length = " + rReturnArray.Length.ToString());
        //        if (rReturnArray.Length > 0)
        //        {
        //            //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate() I have some return");
        //            rReturn = rReturnArray[0];
        //        }
        //        if (rReturnArray.Length > 1)
        //        {
        //            //Debug.Log("!!!The InternalKey " + sInternalKey + " for " + ClassNamePHP() + " is not unique!");
        //            if (sFlushOlderDupplicate == true)
        //            {
        //                List<K> tList = new List<K>();
        //                foreach (K tObject in rReturnArray)
        //                {
        //                    if (tObject.IsTrashed() == false)
        //                    {
        //                        tList.Add(tObject);
        //                    }
        //                }
        //                tList.Sort((x, y) => y.DM.CompareTo(x.DM));
        //                for (int i = 1; i < tList.Count; i++)
        //                {
        //                    tList[i].TrashData();
        //                }
        //                rReturn = tList[0];
        //            }
        //        }
        //    }
        //    if (rReturn == null && sCreateIfNotExists == true)
        //    {
        //        rReturn = NWDBasisHelper.NewData<K>(sWritingMode);
        //        rReturn.InternalKey = sInternalKey;
        //        rReturn.InternalDescription = sInternalDescription;
        //        rReturn.UpdateData(true, sWritingMode);
        //    }
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================