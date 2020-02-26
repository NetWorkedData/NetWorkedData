//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:45
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNewIndexRowType : int
    {
        Key,
        Data,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDNewIndex : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNewIndex()
        {
            //Debug.Log("NWDNewIndex Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDNewIndex(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDNewIndex Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDInternalKeyNotEditable]
    public partial class NWDEditorIndex<TIndex, TKey, TValue> : NWDNewIndex where TIndex : NWDNewIndex where TKey : NWDBasis where TValue : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNewIndexRowType Role { get; set; }
        public string KeyList { get; set; }
        public string DataList { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        protected static NWDBasisHelper Helper;
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisHelper GetHelper()
        {
            //Debug.Log("GetHelper() in basic static for " + typeof(TIndex).Name);
            if (Helper == null)
            {
                Helper = NWDBasisHelper.FindTypeInfos(typeof(TIndex));
            }
            //if (Helper == null)
            //{
            //    Debug.Log("Helper() is null?!!");
            //}
            return Helper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorIndex()
        {
            //Debug.Log("NWDFastIndex Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorIndex(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDFastIndex Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TValue sData, TKey sKey)
        {
            if (sKey != null)
            {
                UpdateData(sData, new string[] { sKey.Reference });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TValue sData, string sKeyReference)
        {
            UpdateData(sData, new string[] { sKeyReference });
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TValue sData, TKey[] sKeys)
        {
            List<string> tList = new List<string>();
            foreach (TKey tK in sKeys)
            {
                tList.Add(tK.Reference);
            }
            UpdateData(sData, tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TValue sData, string[] sKeyReferences)
        {
            GetHelper();
            if (Helper == null)
            {
                //Debug.Log("Helper is null");
            }
            if (sData != null)
            {
                if (string.IsNullOrEmpty(sData.Reference) == false)
                {
                    List<NWDBasis> tListToUpdate = new List<NWDBasis>();
                    if (Helper.DatasByReference.ContainsKey(sData.Reference) == false)
                    {
                        if (sKeyReferences.Length > 0)
                        {
                            //Debug.Log("create data for TValue");
                            NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tNewData = Helper.NewDataWithReference(sData.Reference) as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                            tNewData.Role = NWDNewIndexRowType.Data;
                            tNewData.KeyList = string.Join(NWDConstants.kFieldSeparatorA, sKeyReferences);
                            tListToUpdate.Add(tNewData);
                            // add data for each Key
                            foreach (string tKeyReference in sKeyReferences)
                            {
                                if (Helper.DatasByReference.ContainsKey(tKeyReference) == false)
                                {
                                    // create data for key
                                    NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tNewKey = Helper.NewDataWithReference(tKeyReference) as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                                    tNewKey.Role = NWDNewIndexRowType.Key;
                                    tNewKey.DataList = sData.Reference;
                                    // add the new data key to list to update
                                    tListToUpdate.Add(tNewKey);
                                }
                                else
                                {
                                    NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[tKeyReference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                                    if (tOldKey.DataList.Contains(sData.Reference) == false)
                                    {
                                        tOldKey.DataList = tOldKey.DataList + NWDConstants.kFieldSeparatorA + sData.Reference;
                                        // add the new old key to list to update
                                        tListToUpdate.Add(tOldKey);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log("Get Old data for TValue");
                        NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldData = Helper.DatasByReference[sData.Reference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                        string[] tActualKeys = tOldData.KeyList.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> tActualKeyList = new List<string>(tActualKeys);
                        List<string> tNextKeyList = new List<string>(sKeyReferences);
                        foreach (string tAddKey in sKeyReferences)
                        {
                            if (tActualKeyList.Contains(tAddKey) == false)
                            {
                                //Debug.Log("Add this tKey " + tAddKey);
                                tOldData.KeyList = tOldData.KeyList + NWDConstants.kFieldSeparatorA + tAddKey;
                                //List add new Key
                                if (Helper.DatasByReference.ContainsKey(tAddKey) == false)
                                {
                                    // create data for data
                                    NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tNewKey = Helper.NewDataWithReference(tAddKey) as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                                    tNewKey.Role = NWDNewIndexRowType.Key;
                                    tNewKey.DataList = sData.Reference;
                                    tListToUpdate.Add(tNewKey);
                                }
                                else
                                {
                                    NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[tAddKey] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                                    if (tOldKey.DataList.Contains(sData.Reference) == false)
                                    {
                                        tOldKey.DataList = tOldKey.DataList + NWDConstants.kFieldSeparatorA + sData.Reference;
                                        tListToUpdate.Add(tOldKey);
                                    }
                                }
                            }
                        }
                        foreach (string tOldKey in tActualKeyList)
                        {
                            if (tNextKeyList.Contains(tOldKey) == false)
                            {
                                //Debug.Log("Remove this tKey " + tOldKey);
                                tOldData.KeyList = tOldData.KeyList.Replace(tOldKey, string.Empty).Replace(NWDConstants.kFieldSeparatorA+ NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA).Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]) ;
                                // list remove old Key
                                if (Helper.DatasByReference.ContainsKey(tOldKey) == true)
                                {
                                    NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tRemoveKey = Helper.DatasByReference[tOldKey] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                                    if (tRemoveKey.DataList.Contains(sData.Reference) == true)
                                    {
                                        tRemoveKey.DataList = tRemoveKey.DataList.Replace(sData.Reference, string.Empty).Replace(NWDConstants.kFieldSeparatorA+ NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA).Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                                        tListToUpdate.Add(tRemoveKey);
                                    }
                                }
                            }
                        }
                        tListToUpdate.Add(tOldData);
                    }
                    // finish ... update if necessary
                    foreach (NWDBasis tUpdateData in tListToUpdate)
                    {
                        // For test start
                        //NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tUpdateDataY = (NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>)tUpdateData;
                        //tUpdateDataY.InternalKey = "K =" + tUpdateDataY.KeyList + " V =" + tUpdateDataY.DataList;
                        // For test finish
                        tUpdateData.UpdateDataIfModified();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDataOccurence(TValue sData)
        {
            return CountDataOccurence(sData.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDataOccurence(string sDataReference)
        {
            GetHelper();
            int rReturn = 0;
            if (Helper.DatasByReference.ContainsKey(sDataReference) == true)
            {
                NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[sDataReference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                if (tOldKey.DataList.Contains(sDataReference) == false)
                {
                    string[] tResult = tOldKey.KeyList.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                    rReturn = tResult.Length;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDatasForKey(TKey sKey)
        {
            return CountDatasForKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDatasForKey(string sKeyReference)
        {
            GetHelper();
            int rReturn = 0;
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    string[] tResult = tOldKey.DataList.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                    rReturn = tResult.Length;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string[] ReferencesDatasByKey(TKey sKey)
        {
            return ReferencesDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string[] ReferencesDatasByKey(string sKeyReference)
        {
            GetHelper();
            string[] tResult = new string[0];
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    tResult = tOldKey.DataList.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FirstReferenceDataByKey(TKey sKey)
        {
            return FirstReferenceDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FirstReferenceDataByKey(string sKeyReference)
        {
            string rReturn = string.Empty;
            string[] tList = ReferencesDatasByKey(sKeyReference);
            if (tList.Length > 0)
            {
                rReturn = tList[0];
            }
            return rReturn;

        }
        //-------------------------------------------------------------------------------------------------------------
        public static TValue[] RawDatasByKey(TKey sKey)
        {
            return RawDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TValue[] RawDatasByKey(string sKeyReference)
        {
            GetHelper();
            string[] tResult = new string[0];
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<NWDIndexCategorieItem, TKey, TValue>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    tResult = tOldKey.DataList.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            NWDBasisHelper tDataHelper = NWDBasisHelper.FindTypeInfos(typeof(TValue));
            List<TValue> rResult = new List<TValue>();
            foreach (string tR in tResult)
            {
                rResult.Add(tDataHelper.GetDataByReference(tR) as TValue);
            }
            return rResult.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TValue FirstRawDataByKey(TKey sKey)
        {
            return FirstRawDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TValue FirstRawDataByKey(string sKeyReference)
        {
            string rReturn = string.Empty;
            string[] tList = ReferencesDatasByKey(sKeyReference);
            if (tList.Length > 0)
            {
                rReturn = tList[0];
            }
            NWDBasisHelper tDataHelper = NWDBasisHelper.FindTypeInfos(typeof(TValue));
            return tDataHelper.GetDataByReference(rReturn) as TValue;

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================