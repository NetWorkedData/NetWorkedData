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
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Obsolete("Use NWDIndexer herited class to Install() method in Helper")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDIndexInMemory : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexInMemory()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Obsolete("Use NWDIndexer herited class to Install() method in Helper")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDDeindexInMemory : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDDeindexInMemory()
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
        private Dictionary<string, string> kDataToKey = new Dictionary<string, string>();
        private Dictionary<string, string> kKeyToData = new Dictionary<string, string>();
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, TKey sKey)
        {
            if (sKey != null)
            {
                UpdateData(sData, new string[] { sKey.Reference });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string sKeyReference)
        {
            UpdateData(sData, new string[] { sKeyReference });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, TKey[] sKeys)
        {
            List<string> tList = new List<string>();
            foreach (TKey tK in sKeys)
            {
                tList.Add(tK.Reference);
            }
            UpdateData(sData, tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(TData sData)
        {
            if (sData != null)
            {
                if (string.IsNullOrEmpty(sData.Reference) == false)
                {
                    if (kDataToKey.ContainsKey(sData.Reference) == false)
                    {
                        // Cool ... do nothing
                    }
                    else
                    {
                        string[] tActualKeys = kDataToKey[sData.Reference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string tKey in tActualKeys)
                        {
                            if (kKeyToData.ContainsKey(tKey) == true)
                            {
                                if (kKeyToData[tKey].Contains(sData.Reference) == false)
                                {
                                    kKeyToData[tKey] = kKeyToData[tKey].Replace(sData.Reference, string.Empty).Replace(NWDConstants.kFieldSeparatorA + NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA).Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                                }
                            }
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(TData sData, string[] sKeyReferences)
        {
            if (sData != null)
            {
                if (string.IsNullOrEmpty(sData.Reference) == false)
                {
                    if (kDataToKey.ContainsKey(sData.Reference) == false)
                    {
                        if (sKeyReferences.Length > 0)
                        {
                            kDataToKey.Add(sData.Reference, string.Join(NWDConstants.kFieldSeparatorA, sKeyReferences));
                            foreach (string tKeyReference in sKeyReferences)
                            {
                                if (kKeyToData.ContainsKey(tKeyReference) == false)
                                {
                                    kKeyToData.Add(tKeyReference, sData.Reference);
                                }
                                else
                                {
                                    if (kKeyToData[tKeyReference].Contains(sData.Reference) == false)
                                    {
                                        kKeyToData[tKeyReference] = kKeyToData[tKeyReference] + NWDConstants.kFieldSeparatorA + sData.Reference;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string[] tActualKeys = kDataToKey[sData.Reference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> tActualKeyList = new List<string>(tActualKeys);
                        List<string> tNextKeyList = new List<string>(sKeyReferences);
                        foreach (string tAddKey in sKeyReferences)
                        {
                            if (tActualKeyList.Contains(tAddKey) == false)
                            {
                                kDataToKey[sData.Reference] = kDataToKey[sData.Reference] + NWDConstants.kFieldSeparatorA + tAddKey;
                                if (kKeyToData.ContainsKey(tAddKey) == false)
                                {
                                    kKeyToData.Add(tAddKey, sData.Reference);
                                }
                                else
                                {
                                    if (kKeyToData[tAddKey].Contains(sData.Reference) == false)
                                    {
                                        kKeyToData[tAddKey] = kKeyToData[tAddKey] + NWDConstants.kFieldSeparatorA + sData.Reference;
                                    }
                                }
                            }
                        }
                        foreach (string tOldKey in tActualKeyList)
                        {
                            if (tNextKeyList.Contains(tOldKey) == false)
                            {
                                kDataToKey[sData.Reference] = kDataToKey[sData.Reference].Replace(tOldKey, string.Empty).Replace(NWDConstants.kFieldSeparatorA + NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA).Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                                if (kKeyToData.ContainsKey(tOldKey) == true)
                                {
                                    if (kKeyToData[tOldKey].Contains(sData.Reference) == true)
                                    {
                                        kKeyToData[tOldKey] = kKeyToData[tOldKey].Replace(sData.Reference, string.Empty).Replace(NWDConstants.kFieldSeparatorA + NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA).Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
            if (kDataToKey.ContainsKey(sDataReference) == true)
            {
                string[] tResult = kDataToKey[sDataReference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                rReturn = tResult.Length;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeyOccurence(TKey sKey)
        {
            return CountKeyOccurence(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeyOccurence(string sKeyReference)
        {
            int rReturn = 0;
            if (kKeyToData.ContainsKey(sKeyReference) == true)
            {
                string[] tResult = kKeyToData[sKeyReference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                rReturn = tResult.Length;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] ReferencesDatasByKey(TKey sKey)
        {
            return ReferencesDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] ReferencesDatasByKey(string sKeyReference)
        {
            string[] tResult = new string[0];
            if (kKeyToData.ContainsKey(sKeyReference) == true)
            {
                tResult = kKeyToData[sKeyReference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            }
            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string FirstReferenceDataByKey(TKey sKey)
        {
            return FirstReferenceDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string FirstReferenceDataByKey(string sKeyReference)
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
        public TData[] RawDatasByKey(TKey sKey)
        {
            return RawDatasByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public TData[] RawDatasByKey(string sKeyReference)
        {
            string[] tResult = new string[0];
            if (kKeyToData.ContainsKey(sKeyReference) == true)
            {
                tResult = kKeyToData[sKeyReference].Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
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
        public TData FirstRawDataByKey(TKey sKey)
        {
            return FirstRawDataByKey(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public TData FirstRawDataByKey(string sKeyReference)
        {
            //string rReturn = string.Empty;
            string[] tList = ReferencesDatasByKey(sKeyReference);
            NWDBasisHelper tDataHelper = NWDBasisHelper.FindTypeInfos(typeof(TData));
            // bug when reset table
            //if (tList.Length > 0)
            //{
            //    rReturn = tList[0];
            //}
            // prevent bug
            TData rReturn = null;
            foreach (string tRef in tList)
            {
                rReturn = tDataHelper.GetDataByReference(tRef) as TData;
                if (rReturn != null)
                {
                    break;
                }
            }
            return rReturn;

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================