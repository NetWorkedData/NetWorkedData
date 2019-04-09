//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
    public interface INWDIndex<TKey, TObject>
    {
        void InsertInIndex(TObject sObject, TKey sKey);
        void InsertInIndex(TObject sObject, TKey[] sKeys);
        void RemoveFromIndex(TObject sObject);
        int CountObjects();
        int CountReferenceToObject(TObject sObject);
        int CountKeys();
        int CountKey(TKey sKey);
        List<TObject> FindByKey(TKey sKey);
        TObject FindFirstByKey(TKey sKey);
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class BTBIndex<K, O>
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<K, List<O>> kIndex = new Dictionary<K, List<O>>();
        private Dictionary<O, List<List<O>>> kIndexInversed = new Dictionary<O, List<List<O>>>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(O sObject, K sKey)
        {
            RemoveFromIndex(sObject);
            if (kIndex.ContainsKey(sKey) == false)
            {
                kIndex.Add(sKey, new List<O>());
            }
            kIndex[sKey].Add(sObject);
            if (kIndexInversed.ContainsKey(sObject) == false)
            {
                kIndexInversed.Add(sObject, new List<List<O>>());
            }
            kIndexInversed[sObject].Add(kIndex[sKey]);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(O sObject, K[] sKeys)
        {
            RemoveFromIndex(sObject);
            foreach (K tKey in sKeys)
            {
                if (kIndex.ContainsKey(tKey) == false)
                {
                    kIndex.Add(tKey, new List<O>());
                }
                kIndex[tKey].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject) == false)
                {
                    kIndexInversed.Add(sObject, new List<List<O>>());
                }
                kIndexInversed[sObject].Add(kIndex[tKey]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromIndex(O sObject)
        {
            if (kIndexInversed.ContainsKey(sObject))
            {
                foreach (List<O> tIndex in kIndexInversed[sObject])
                {
                    tIndex.Remove(sObject);
                }
                kIndexInversed.Remove(sObject);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<O> FindByKey(K sKey)
        {
            return FindByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjects()
        {
            int rReturn = kIndexInversed.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountReferenceToObject(O sObject)
        {
            int rReturn = 0;
            if (kIndexInversed.ContainsKey(sObject))
            {
                rReturn = kIndexInversed[sObject].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeys()
        {
            int rReturn = kIndex.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKey(K sKey)
        {
            int rReturn = 0;
            if (kIndex.ContainsKey(sKey))
            {
                rReturn = kIndex[sKey].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public O FindFirstByKey(K sKey)
        {
            return FindFirstByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDIndex<K, O> where K : NWDTypeClass where O : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<string, List<O>> kIndex = new Dictionary<string, List<O>>();
        private Dictionary<string, List<List<O>>> kIndexInversed = new Dictionary<string, List<List<O>>>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(O sObject, K sKey)
        {
            RemoveFromIndex(sObject);
            if (sKey != null)
            {
                InsertInIndex(sObject, sKey.ReferenceValue());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(O sObject, string sReference)
        {
            RemoveFromIndex(sObject);
            if (string.IsNullOrEmpty(sReference) == false)
            {
                if (kIndex.ContainsKey(sReference) == false)
                {
                    kIndex.Add(sReference, new List<O>());
                }
                kIndex[sReference].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject.ReferenceValue()) == false)
                {
                    kIndexInversed.Add(sObject.ReferenceValue(), new List<List<O>>());
                }
                kIndexInversed[sObject.ReferenceValue()].Add(kIndex[sReference]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(O sObject, K[] sKeys)
        {
            RemoveFromIndex(sObject);
            foreach (K tKey in sKeys)
            {
                if (kIndex.ContainsKey(tKey.ReferenceValue()) == false)
                {
                    kIndex.Add(tKey.ReferenceValue(), new List<O>());
                }
                kIndex[tKey.ReferenceValue()].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject.ReferenceValue()) == false)
                {
                    kIndexInversed.Add(sObject.ReferenceValue(), new List<List<O>>());
                }
                kIndexInversed[sObject.ReferenceValue()].Add(kIndex[tKey.ReferenceValue()]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromIndex(O sObject)
        {
            if (kIndexInversed.ContainsKey(sObject.ReferenceValue()))
            {
                foreach (List<O> tIndex in kIndexInversed[sObject.ReferenceValue()])
                {
                    tIndex.Remove(sObject);
                }
                kIndexInversed.Remove(sObject.ReferenceValue());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjects()
        {
            int rReturn = kIndexInversed.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjectOccurence(O sObject)
        {
            return CountObjectReferenceOccurence(sObject.ReferenceValue());
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjectReferenceOccurence(string sReference)
        {
            int rReturn = 0;
            if (kIndexInversed.ContainsKey(sReference))
            {
                rReturn = kIndexInversed[sReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeys()
        {
            return kIndex.Count;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeyOccurence(K sKey)
        {
            return CountKeyReferenceOccurence(sKey.ReferenceValue());
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountKeyReferenceOccurence(string sReference)
        {
            int rReturn = 0;
            if (kIndex.ContainsKey(sReference))
            {
                rReturn = kIndex[sReference].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<O> FindByKey(K sKey)
        {
            return FinByReference(sKey.ReferenceValue());
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<O> FinByReference(string sReference)
        {
            List<O> rReturn;
            if (kIndex.ContainsKey(sReference) == true)
            {
                rReturn = kIndex[sReference];
            }
            else
            {
                rReturn = new List<O>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public O FindFirstByKey(K sKey)
        {
            return FindFirstByReference(sKey.ReferenceValue());
        }
        //-------------------------------------------------------------------------------------------------------------
        public O FindFirstByReference(string sReference)
        {
            List<O> tList = FinByReference(sReference);
            O rReturn = null;
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