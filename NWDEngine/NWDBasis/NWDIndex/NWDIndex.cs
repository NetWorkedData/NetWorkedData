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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDIndexInsert : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexInsert()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDIndexRemove : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexRemove()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDIndexUpdate : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexUpdate()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDIndexSimple<TData> where TData : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<string, List<TData>> kIndex = new Dictionary<string, List<TData>>();
        private Dictionary<string, List<List<TData>>> kIndexInversed = new Dictionary<string, List<List<TData>>>();
        private List<TData> kDataList = new List<TData>();
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string sKeyReference)
        {
            RemoveData(sData);
            InsertData(sData, sKeyReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string[] sKeyReferences)
        {
            RemoveData(sData);
            InsertData(sData, sKeyReferences);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, string sKeyReference)
        {
            if (string.IsNullOrEmpty(sKeyReference) == false)
            {
                if (kDataList.Contains(sData) == false)
                {
                    kDataList.Add(sData);
                }
                if (kIndex.ContainsKey(sKeyReference) == false)
                {
                    kIndex.Add(sKeyReference, new List<TData>());
                }
                kIndex[sKeyReference].Add(sData);
                if (kIndexInversed.ContainsKey(sData.Reference) == false)
                {
                    kIndexInversed.Add(sData.Reference, new List<List<TData>>());
                }
                kIndexInversed[sData.Reference].Add(kIndex[sKeyReference]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, string[] sKeyReferences)
        {
            foreach (string tKeyReference in sKeyReferences)
            {
                InsertData(sData, tKeyReference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(TData sData)
        {
            if (kDataList.Contains(sData))
            {
                kDataList.RemoveAll(x => x == sData);
                foreach (List<TData> tIndex in kIndexInversed[sData.Reference])
                {
                    tIndex.RemoveAll(x => x == sData);
                }
                kIndexInversed.Remove(sData.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDatas()
        {
            int rReturn = kIndexInversed.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDataOccurence(TData sData)
        {
            return CountDataOccurence(sData.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDataOccurence(string sDataReference)
        {
            int rReturn = 0;
            if (kIndexInversed.ContainsKey(sDataReference))
            {
                rReturn = kIndexInversed[sDataReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeys()
        {
            return kIndex.Count;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDatasForKey(string sKeyReference)
        {
            int rReturn = 0;
            if (kIndex.ContainsKey(sKeyReference))
            {
                rReturn = kIndex[sKeyReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TData> RawDatas()
        {
            return new List<TData>(kDataList);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TData> RawDatasByKey(string sKeyReference)
        {
            List<TData> rReturn;
            if (kIndex.ContainsKey(sKeyReference) == true)
            {
                rReturn = kIndex[sKeyReference];
            }
            else
            {
                rReturn = new List<TData>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public TData RawFirstDataByKey(string sKeyReference)
        {
            List<TData> tList = RawDatasByKey(sKeyReference);
            TData rReturn = null;
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDIndex<TKey, TData> where TKey : NWDTypeClass where TData : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<string, List<TData>> kIndex = new Dictionary<string, List<TData>>();
        private Dictionary<string, List<List<TData>>> kIndexInversed = new Dictionary<string, List<List<TData>>>();
        private List<TData> kDataList = new List<TData>();
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, TKey sKey)
        {
            RemoveData(sData);
            InsertData(sData, sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string sKeyReference)
        {
            RemoveData(sData);
            InsertData(sData, sKeyReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, TKey[] sKeys)
        {
            RemoveData(sData);
            InsertData(sData, sKeys);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string[] sKeyReferences)
        {
            RemoveData(sData);
            InsertData(sData, sKeyReferences);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, TKey sKey)
        {
            if (sKey != null)
            {
                InsertData(sData, sKey.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, string sKeyReference)
        {
            //Debug.Log("NWDIndex InsertData()");
            if (string.IsNullOrEmpty(sKeyReference) == false)
            {
                if (kDataList.Contains(sData) == false)
                {
                    kDataList.Add(sData);
                }
                if (kIndex.ContainsKey(sKeyReference) == false)
                {
                    kIndex.Add(sKeyReference, new List<TData>());
                }
                kIndex[sKeyReference].Add(sData);
                if (kIndexInversed.ContainsKey(sData.Reference) == false)
                {
                    kIndexInversed.Add(sData.Reference, new List<List<TData>>());
                }
                kIndexInversed[sData.Reference].Add(kIndex[sKeyReference]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, TKey[] sKeys)
        {
            foreach (TKey tKey in sKeys)
            {
                InsertData(sData, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertData(TData sData, string[] sKeyReferences)
        {
            foreach (string tKeyReference in sKeyReferences)
            {
                InsertData(sData, tKeyReference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(TData sData)
        {
            if (kDataList.Contains(sData))
            {
                kDataList.RemoveAll(x => x == sData);
                if (kIndexInversed.ContainsKey(sData.Reference))
                {
                    foreach (List<TData> tIndex in kIndexInversed[sData.Reference])
                    {
                        tIndex.RemoveAll(x => x == sData);
                    }
                    kIndexInversed.Remove(sData.Reference);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDatas()
        {
            int rReturn = kIndexInversed.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDataOccurence(TData sData)
        {
            return CountDataOccurence(sData.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDataOccurence(string sDataReference)
        {
            int rReturn = 0;
            if (kIndexInversed.ContainsKey(sDataReference))
            {
                rReturn = kIndexInversed[sDataReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeys()
        {
            return kIndex.Count;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDatasForKey(TKey sKey)
        {
            return CountDatasForKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountDatasForKey(string sKeyReference)
        {
            int rReturn = 0;
            if (kIndex.ContainsKey(sKeyReference))
            {
                rReturn = kIndex[sKeyReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TData> RawDatas()
        {
            return new List<TData>(kDataList);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TData> RawDatasByKey(TKey sKey)
        {
            return RawDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TData> RawDatasByKey(string sKeyReference)
        {
            List<TData> rReturn;
            if (kIndex.ContainsKey(sKeyReference) == true)
            {
                rReturn = kIndex[sKeyReference];
            }
            else
            {
                rReturn = new List<TData>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public TData RawFirstDataByKey(TKey sKey)
        {
            return RawFirstDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public TData RawFirstDataByKey(string sKeyReference)
        {
            List<TData> tList = RawDatasByKey(sKeyReference);
            TData rReturn = null;
            if (tList.Count > 0)
            {
                rReturn = tList[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
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
    public partial class NWDEditorIndex<TKey, TData> : NWDNewIndex where TKey : NWDBasis where TData : NWDBasis
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
            if (Helper == null)
            {
                return NWDBasisHelper.FindTypeInfos(typeof(NWDEditorIndex<TKey, TData>));
            }
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
        public static void UpdateData(TData sData, TKey sKey)
        {
            if (sKey != null)
            {
                UpdateData(sData, new string[] { sKey.Reference });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TData sData, string sKeyReference)
        {
            UpdateData(sData, new string[] { sKeyReference });
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TData sData, TKey[] sKeys)
        {
            List<string> tList = new List<string>();
            foreach (TKey tK in sKeys)
            {
                tList.Add(tK.Reference);
            }
            UpdateData(sData, tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateData(TData sData, string[] sKeyReferences)
        {
            if (sData != null)
            {
                if (string.IsNullOrEmpty(sData.Reference) == false)
                {
                    if (sKeyReferences.Length > 0)
                    {
                        List<NWDBasis> tListToUpdate = new List<NWDBasis>();
                        if (Helper.DatasByReference.ContainsKey(sData.Reference) == false)
                        {
                            // create data for data
                            NWDEditorIndex<TKey, TData> tNewData = Helper.NewDataWithReference(sData.Reference) as NWDEditorIndex<TKey, TData>;
                            tNewData.Role = NWDNewIndexRowType.Data;
                            tNewData.KeyList = string.Join(" ", sKeyReferences);
                            tListToUpdate.Add(tNewData);
                            // add data for each Key
                            foreach (string tKeyReference in sKeyReferences)
                            {
                                if (Helper.DatasByReference.ContainsKey(tKeyReference) == false)
                                {
                                    // create data for key
                                    NWDEditorIndex<TKey, TData> tNewKey = Helper.NewDataWithReference(tKeyReference) as NWDEditorIndex<TKey, TData>;
                                    tNewKey.Role = NWDNewIndexRowType.Key;
                                    tNewKey.DataList = sData.Reference;
                                    // add the new data key to list to update
                                    tListToUpdate.Add(tNewKey);
                                }
                                else
                                {
                                    NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[tKeyReference] as NWDEditorIndex<TKey, TData>;
                                    if (tOldKey.DataList.Contains(sData.Reference) == false)
                                    {
                                        tOldKey.DataList = tOldKey.DataList + " " + sData.Reference;
                                        // add the new old key to list to update
                                        tListToUpdate.Add(tOldKey);
                                    }
                                }
                            }

                        }
                        else
                        {
                            // get old data for data
                            NWDEditorIndex<TKey, TData> tOldData = Helper.DatasByReference[sData.Reference] as NWDEditorIndex<TKey, TData>;
                            string[] tActualKeys = tOldData.KeyList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            List<string> tActualKeyList = new List<string>(tActualKeys);
                            List<string> tNextKeyList = new List<string>(sKeyReferences);
                            foreach (string tAddKey in sKeyReferences)
                            {
                                if (tActualKeyList.Contains(tAddKey) == false)
                                {
                                    tOldData.KeyList = tOldData.KeyList + " " + tAddKey;
                                    //List add new Key
                                    if (Helper.DatasByReference.ContainsKey(tAddKey) == false)
                                    {
                                        // create data for data
                                        NWDEditorIndex<TKey, TData> tNewKey = Helper.NewDataWithReference(tAddKey) as NWDEditorIndex<TKey, TData>;
                                        tNewKey.Role = NWDNewIndexRowType.Key;
                                        tNewKey.DataList = sData.Reference;
                                        tListToUpdate.Add(tNewKey);
                                    }
                                    else
                                    {
                                        NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[tAddKey] as NWDEditorIndex<TKey, TData>;
                                        if (tOldKey.DataList.Contains(sData.Reference) == false)
                                        {
                                            tOldKey.DataList = tOldKey.DataList + " " + sData.Reference;
                                            tListToUpdate.Add(tOldKey);
                                        }
                                    }
                                }
                            }
                            foreach (string tOldKey in tActualKeyList)
                            {
                                if (tNextKeyList.Contains(tOldKey) == true)
                                {
                                    tOldData.KeyList = tOldData.KeyList.Replace(tOldKey, "").Replace("  ", " ").Trim();
                                    // list remove old Key
                                    if (Helper.DatasByReference.ContainsKey(tOldKey) == true)
                                    {
                                        NWDEditorIndex<TKey, TData> tRemoveKey = Helper.DatasByReference[tOldKey] as NWDEditorIndex<TKey, TData>;
                                        if (tRemoveKey.DataList.Contains(sData.Reference) == true)
                                        {
                                            tRemoveKey.DataList = tRemoveKey.DataList.Replace(sData.Reference, "").Replace("  ", " ").Trim();
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
                            tUpdateData.UpdateDataIfModified();
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void InsertData(TData sData, TKey sKey)
        //{
        //    if (sKey != null)
        //    {
        //        InsertData(sData, sKey.Reference);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void InsertData(TData sData, string sKeyReference)
        //{
        //    //Debug.Log("NWDIndex InsertData()");
        //    // insert Data in index for inverse search
        //    if (sData != null)
        //    {
        //        if (string.IsNullOrEmpty(sData.Reference) == false)
        //        {
        //            if (string.IsNullOrEmpty(sKeyReference) == false)
        //            {
        //                if (Helper.DatasByReference.ContainsKey(sData.Reference) == false)
        //                {
        //                    // create data for data
        //                    NWDEditorIndex<TKey, TData> tNewData = Helper.NewDataWithReference(sData.Reference) as NWDEditorIndex<TKey, TData>;
        //                    tNewData.Role = NWDNewIndexRowType.Data;
        //                    tNewData.KeyList = sKeyReference;
        //                    tNewData.UpdateData();
        //                }
        //                else
        //                {
        //                    NWDEditorIndex<TKey, TData> tOldData = Helper.DatasByReference[sData.Reference] as NWDEditorIndex<TKey, TData>;
        //                    tOldData.Role = NWDNewIndexRowType.Data;
        //                    if (tOldData.KeyList.Contains(sKeyReference) == false)
        //                    {
        //                        tOldData.KeyList = tOldData.KeyList + " " + sKeyReference;
        //                        tOldData.UpdateData();
        //                    }
        //                }
        //                // insert Key in index for normal search
        //                if (Helper.DatasByReference.ContainsKey(sKeyReference) == false)
        //                {
        //                    // create data for data
        //                    NWDEditorIndex<TKey, TData> tNewKey = Helper.NewDataWithReference(sKeyReference) as NWDEditorIndex<TKey, TData>;
        //                    tNewKey.Role = NWDNewIndexRowType.Key;
        //                    tNewKey.DataList = sData.Reference;
        //                    tNewKey.UpdateData();
        //                }
        //                else
        //                {
        //                    NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<TKey, TData>;
        //                    tOldKey.Role = NWDNewIndexRowType.Key;
        //                    if (tOldKey.DataList.Contains(sKeyReference) == false)
        //                    {
        //                        tOldKey.DataList = tOldKey.DataList + " " + sKeyReference;
        //                        tOldKey.UpdateData();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void InsertData(TData sData, TKey[] sKeys)
        //{
        //    foreach (TKey tKey in sKeys)
        //    {
        //        InsertData(sData, tKey);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void InsertData(TData sData, string[] sKeyReferences)
        //{
        //    foreach (string tKeyReference in sKeyReferences)
        //    {
        //        InsertData(sData, tKeyReference);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void RemoveData(TData sData)
        //{
        //    if (sData != null)
        //    {
        //        if (string.IsNullOrEmpty(sData.Reference) == false)
        //        {
        //            if (Helper.DatasByReference.ContainsKey(sData.Reference) == false)
        //            {
        //                // create data for data
        //                NWDEditorIndex<TKey, TData> tFoundData = Helper.NewDataWithReference(sData.Reference) as NWDEditorIndex<TKey, TData>;
        //                tFoundData.Role = NWDNewIndexRowType.Data;
        //                foreach (string tKeyReference in tFoundData.KeyList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
        //                {
        //                    if (Helper.DatasByReference.ContainsKey(tKeyReference) == true)
        //                    {
        //                        NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[tKeyReference] as NWDEditorIndex<TKey, TData>;
        //                        tOldKey.Role = NWDNewIndexRowType.Key;
        //                        if (tOldKey.DataList.Contains(sData.Reference) == true)
        //                        {
        //                            tOldKey.DataList = tOldKey.DataList.Replace(sData.Reference, "").Replace("  ", " ").Trim();
        //                            tOldKey.UpdateData();
        //                        }
        //                    }
        //                }
        //                tFoundData.KeyList = string.Empty;
        //                tFoundData.UpdateData();
        //            }
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDataOccurence(TData sData)
        {
            return CountDataOccurence(sData.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CountDataOccurence(string sDataReference)
        {
            int rReturn = 0;
            if (Helper.DatasByReference.ContainsKey(sDataReference) == true)
            {
                NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[sDataReference] as NWDEditorIndex<TKey, TData>;
                if (tOldKey.DataList.Contains(sDataReference) == false)
                {
                    string[] tResult = tOldKey.KeyList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
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
            int rReturn = 0;
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<TKey, TData>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    string[] tResult = tOldKey.DataList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
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
            string[] tResult = new string[0];
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<TKey, TData>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    tResult = tOldKey.DataList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
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
        public static TData[] RawDatasByKey(TKey sKey)
        {
            return RawDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TData[] RawDatasByKey(string sKeyReference)
        {
            string[] tResult = new string[0];
            if (Helper.DatasByReference.ContainsKey(sKeyReference) == true)
            {
                NWDEditorIndex<TKey, TData> tOldKey = Helper.DatasByReference[sKeyReference] as NWDEditorIndex<TKey, TData>;
                if (tOldKey.DataList.Contains(sKeyReference) == false)
                {
                    tResult = tOldKey.DataList.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            NWDBasisHelper tDataHelper = NWDBasisHelper.FindTypeInfos(typeof(TData));
            List<TData> rResult = new List<TData>();
            foreach (string tR in tResult)
            {
                rResult.Add(tDataHelper.GetDataByReference(tR) as TData);
            }
            return rResult.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TData FirstRawDataByKey(TKey sKey)
        {
            return FirstRawDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static TData FirstRawDataByKey(string sKeyReference)
        {
            string rReturn = string.Empty;
            string[] tList = ReferencesDatasByKey(sKeyReference);
            if (tList.Length > 0)
            {
                rReturn = tList[0];
            }
            NWDBasisHelper tDataHelper = NWDBasisHelper.FindTypeInfos(typeof(TData));
            return tDataHelper.GetDataByReference(rReturn) as TData;

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================