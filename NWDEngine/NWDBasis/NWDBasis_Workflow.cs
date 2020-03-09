//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:23
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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        //public static K NewObject()
        //{
        //    //Debug.Log("NWDBasis <K> NewObject()");
        //    K rReturn = NWDBasis.NewInstance() as K;
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //#if UNITY_EDITOR
        //        //-------------------------------------------------------------------------------------------------------------
        //        public static K NewObjectWithReference(string sReference)
        //        {
        //            K rReturn = NWDBasis.NewInstanceWithReference(sReference) as K;
        //            return rReturn;
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //#endif
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] NEW_GetAllDatasInCurrentSaveGame()
        //{
        //    return FindDatas(NWDAccount.GetCurrentAccountReference(), NWDGameSave.Current());
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Finds the datas. = ANCIEN GetAllObjects()
        /// </summary>
        /// <returns>The datas.</returns>
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllObjects(string sAccountReference = null)
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
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllEnableObjects(string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (K tObject in NWDBasis.Datas().ObjectsList)
        //    {
        //        if (tObject.IsReacheableByAccount(sAccountReference) && tObject.IsEnable())
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static K GetFirstObject(string sAccountReference = null)
        //{
        //    K rReturn = null;
        //    foreach (K tObject in NWDBasis.Datas().ObjectsList)
        //    {
        //        if (tObject.IsReacheableByAccount(sAccountReference))
        //        {
        //            rReturn = tObject;
        //            break;
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static K GetFirstEnableObject(string sAccountReference = null)
        //{
        //    K rReturn = null;
        //    foreach (K tObject in NWDBasis.Datas().ObjectsList)
        //    {
        //        if (tObject.IsReacheableByAccount(sAccountReference) && tObject.IsEnable())
        //        {
        //            rReturn = tObject;
        //            break;
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
//        public static K[] TrashAllObjects(string sAccountReference = null)
//        {
//            List<K> rReturn = new List<K>();
//            foreach (K tObject in NWDBasis.FindDatas(sAccountReference))
//            {
//                tObject.TrashData();
//                rReturn.Add(tObject);
//            }
//#if UNITY_EDITOR
//            BasisHelper().New_RepaintTableEditor();
//            BasisHelper().New_RepaintInspectorEditor();
//#endif
        //    return rReturn.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static K GetObjectByReference(string sReference, string sAccountReference = null)
        //{
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
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K GetObjectAbsoluteByReference(string sReference)
        //{
        //    K rReturn = null;
        //    int tIndex = Datas().ObjectsByReferenceList.IndexOf(sReference);
        //    if (tIndex >= 0)
        //    {
        //        K tObject = Datas().ObjectsList.ElementAt(tIndex) as K;
        //            rReturn = tObject;
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetObjectsAbsoluteByReferences(string[] sReferences)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (string tReference in sReferences)
        //    {
        //        K tObject = GetObjectAbsoluteByReference(tReference);
        //        if (tObject != null)
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetObjectsByReferences(string[] sReferences, string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (string tReference in sReferences)
        //    {
        //        K tObject = GetObjectByReference(tReference, sAccountReference);
        //        if (tObject != null)
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetAllObjectsByInternalKey(string sInternalKey, string sAccountReference = null)
        //{
        //    K[] tArray = GetAllObjects(sAccountReference);
        //    List<K> tAllList = new List<K>();
        //    foreach (K tObject in tArray)
        //    {
        //        if (tObject.InternalKey == sInternalKey)
        //        {
        //            tAllList.Add(tObject);
        //        }
        //    }
        //    return tAllList.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K GetObjectByInternalKey(string sInternalKey, bool sFlushOlderDupplicate = false, string sAccountReference = null)
        //{
        //    //K[] rReturnArray = GetAllObjectsByInternalKey(sInternalKey, sAccountReference);
        //    K[] tArray = GetAllObjects(sAccountReference);
        //    List<K> tAllList = new List<K>();
        //    foreach (K tObject in tArray)
        //    {
        //        if (tObject.InternalKey == sInternalKey)
        //        {
        //            tAllList.Add(tObject);
        //        }
        //    }
        //    K[] rReturnArray = tAllList.ToArray();
        //    K rReturn = null;
        //    if (rReturnArray.Length > 0)
        //    {
        //        rReturn = rReturnArray[0];
        //    }
        //    if (rReturnArray.Length > 1)
        //    {
        //        //Debug.LogWarning("The InternalKey " + sInternalKey + " for " + ClassNamePHP() + " is not unique!");
        //        if (sFlushOlderDupplicate == true)
        //        {
        //            List<K> tList = new List<K>();
        //            foreach (K tObject in rReturnArray)
        //            {
        //                if (tObject.IsTrashed() == false)
        //                {
        //                    tList.Add(tObject);
        //                }
        //            }
        //            //tList.Sort((x, y) => y.DM.CompareTo(x.DM));  //older is delete
        //            tList.Sort((x, y) => x.DM.CompareTo(y.DM)); // newer is delete
        //            for (int i = 1; i < tList.Count; i++)
        //            {
        //                tList[i].TrashData();
        //            }
        //        }
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K GetObjectByInternalKeyOrCreate(string sInternalKey, string sInternalDescription = "", bool sFlushOlderDupplicate = false, string sAccountReference = null)
        //{
        //    //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate()");
        //    //K[] rReturnArray = GetAllObjectsByInternalKey(sInternalKey, sAccountReference);
        //    K[] tArray = GetAllObjects(sAccountReference);
        //    List<K> tAllList = new List<K>();
        //    foreach (K tObject in tArray)
        //    {
        //        if (tObject.InternalKey == sInternalKey)
        //        {
        //            tAllList.Add(tObject);
        //        }
        //    }
        //    K[] rReturnArray = tAllList.ToArray();
        //    K rReturn = null;
        //    //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate() rReturnArray.Length = " + rReturnArray.Length.ToString());
        //    if (rReturnArray.Length > 0)
        //    {
        //        //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate() I have some return");
        //        rReturn = rReturnArray[0];
        //    }
        //    if (rReturnArray.Length > 1)
        //    {
        //        //Debug.Log("!!!The InternalKey " + sInternalKey + " for " + ClassNamePHP() + " is not unique!");
        //        if (sFlushOlderDupplicate == true)
        //        {
        //            List<K> tList = new List<K>();
        //            foreach (K tObject in rReturnArray)
        //            {
        //                if (tObject.IsTrashed() == false)
        //                {
        //                    tList.Add(tObject);
        //                }
        //            }
        //            tList.Sort((x, y) => y.DM.CompareTo(x.DM));
        //            for (int i = 1; i < tList.Count; i++)
        //            {
        //                tList[i].TrashData();
        //            }
        //            rReturn = tList[0];
        //        }
        //    }
        //    if (rReturn == null)
        //    {
        //        //Debug.Log("NWDBasis Workflow GetObjectByInternalKeyOrCreate() I have a return");
        //        rReturn = NWDBasisHelper.NewData<K>();
        //        rReturn.InternalKey = sInternalKey;
        //        rReturn.InternalDescription = sInternalDescription;
        //        rReturn.InsertData();
        //        //AddObjectInListOfEdition(rReturn);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetObjectsByInternalKeys(string[] sInternalKeys, bool sFlushOlderDupplicate = false, string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (string tInternalKey in sInternalKeys)
        //    {
        //        K tObject = GetObjectByInternalKey(tInternalKey, sFlushOlderDupplicate, sAccountReference);
        //        if (tObject != null)
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ////TODO : GetAllObjectByInternalKeys(string[] sInternalKey)
        ////-------------------------------------------------------------------------------------------------------------
        //public static K[] GetObjectsByInternalKeysOrCreate(string[] sInternalKeys, string sInternalDescription = "", bool sFlushOlderDupplicate = false, string sAccountReference = null)
        //{
        //    List<K> rReturn = new List<K>();
        //    foreach (string tInternalKey in sInternalKeys)
        //    {
        //        K tObject = GetObjectByInternalKeyOrCreate(tInternalKey, sInternalDescription, sFlushOlderDupplicate, sAccountReference);
        //        if (tObject != null)
        //        {
        //            rReturn.Add(tObject);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        // TODO: must be tested
        //public static K[] Where(Expression<Func<K, bool>> predExpr, string sAccountReference = null)
        //{
        //    SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //    if (AccountDependent())
        //    {
        //        tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //    }
        //    IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K>().Where(predExpr);
        //    List<K> tAllReferences = new List<K>();
        //    foreach (K tItem in tEnumerable)
        //    {
        //        int tIndex = ObjectsByReferenceList.IndexOf(tItem.Reference);
        //        K tObject = ObjectsList.ElementAt(tIndex) as K;
        //        if (tObject.IsReacheableByAccount(sAccountReference))
        //        {
        //            tAllReferences.Add(tObject);
        //        }
        //    }
        //    return tAllReferences.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static K[] SelectOrderedList(K[] sArray, Comparison<K> sComparison)
        //{
        //    List<K> tList = new List<K>();
        //    tList.AddRange(sArray);
        //    tList.Sort(sComparison);
        //    return tList.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Methods
        //-------------------------------------------------------------------------------------------------------------
        //public bool IsEnable()
        //{
        //    return AC;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool IsTrashed()
        //{
        //    if (XX > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public bool IsUsable()
        //{
        //    if (AC == true && XX <= 0 && TestIntegrity() == true)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDBasisTag GetTag()
        //{
        //    return Tag;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetTag(NWDBasisTag sTag)
        //{
        //    Tag = sTag;
        //    UpdateDataIfModified();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================