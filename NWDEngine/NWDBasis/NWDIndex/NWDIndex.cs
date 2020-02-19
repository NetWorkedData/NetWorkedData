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
}
//=====================================================================================================================