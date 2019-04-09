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
    public class BTBIndex<TKey, TObject>
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<TKey, List<TObject>> kIndex = new Dictionary<TKey, List<TObject>>();
        private Dictionary<TObject, List<List<TObject>>> kIndexInversed = new Dictionary<TObject, List<List<TObject>>>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(TObject sObject, TKey sKey)
        {
            RemoveFromIndex(sObject);
            if (kIndex.ContainsKey(sKey) == false)
            {
                kIndex.Add(sKey, new List<TObject>());
            }
            kIndex[sKey].Add(sObject);
            if (kIndexInversed.ContainsKey(sObject) == false)
            {
                kIndexInversed.Add(sObject, new List<List<TObject>>());
            }
            kIndexInversed[sObject].Add(kIndex[sKey]);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(TObject sObject, TKey[] sKeys)
        {
            RemoveFromIndex(sObject);
            foreach (TKey tKey in sKeys)
            {
                if (kIndex.ContainsKey(tKey) == false)
                {
                    kIndex.Add(tKey, new List<TObject>());
                }
                kIndex[tKey].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject) == false)
                {
                    kIndexInversed.Add(sObject, new List<List<TObject>>());
                }
                kIndexInversed[sObject].Add(kIndex[tKey]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromIndex(TObject sObject)
        {
            if (kIndexInversed.ContainsKey(sObject))
            {
                foreach (List<TObject> tIndex in kIndexInversed[sObject])
                {
                    tIndex.Remove(sObject);
                }
                kIndexInversed.Remove(sObject);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TObject> FindByKey(TKey sKey)
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
        public int CountReferenceToObject(TObject sObject)
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
        public int CountKey(TKey sKey)
        {
            int rReturn = 0;
            if (kIndex.ContainsKey(sKey))
            {
                rReturn = kIndex[sKey].Count;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public TObject FindFirstByKey(TKey sKey)
        {
            return FindFirstByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDIndex<TKey, TObject> where TKey : NWDTypeClass where TObject : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<string, List<TObject>> kIndex = new Dictionary<string, List<TObject>>();
        private Dictionary<string, List<List<TObject>>> kIndexInversed = new Dictionary<string, List<List<TObject>>>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(TObject sObject, TKey sKey)
        {
            RemoveFromIndex(sObject);
            if (sKey != null)
            {
                InsertInIndex(sObject, sKey.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(TObject sObject, string sReference)
        {
            RemoveFromIndex(sObject);
            if (string.IsNullOrEmpty(sReference) == false)
            {
                if (kIndex.ContainsKey(sReference) == false)
                {
                    kIndex.Add(sReference, new List<TObject>());
                }
                kIndex[sReference].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject.Reference) == false)
                {
                    kIndexInversed.Add(sObject.Reference, new List<List<TObject>>());
                }
                kIndexInversed[sObject.Reference].Add(kIndex[sReference]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex(TObject sObject, TKey[] sKeys)
        {
            RemoveFromIndex(sObject);
            foreach (TKey tKey in sKeys)
            {
                if (kIndex.ContainsKey(tKey.Reference) == false)
                {
                    kIndex.Add(tKey.Reference, new List<TObject>());
                }
                kIndex[tKey.Reference].Add(sObject);
                if (kIndexInversed.ContainsKey(sObject.Reference) == false)
                {
                    kIndexInversed.Add(sObject.Reference, new List<List<TObject>>());
                }
                kIndexInversed[sObject.Reference].Add(kIndex[tKey.Reference]);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromIndex(TObject sObject)
        {
            if (kIndexInversed.ContainsKey(sObject.Reference))
            {
                foreach (List<TObject> tIndex in kIndexInversed[sObject.Reference])
                {
                    tIndex.Remove(sObject);
                }
                kIndexInversed.Remove(sObject.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjects()
        {
            int rReturn = kIndexInversed.Count;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int CountObjectOccurence(TObject sObject)
        {
            return CountObjectReferenceOccurence(sObject.Reference);
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
        public int CountKeyOccurence(TKey sKey)
        {
            return CountKeyReferenceOccurence(sKey.Reference);
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
        public List<TObject> FindByKey(TKey sKey)
        {
            return FinByReference(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<TObject> FinByReference(string sReference)
        {
            List<TObject> rReturn;
            if (kIndex.ContainsKey(sReference) == true)
            {
                rReturn = kIndex[sReference];
            }
            else
            {
                rReturn = new List<TObject>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public TObject FindFirstByKey(TKey sKey)
        {
            return FindFirstByReference(sKey.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public TObject FindFirstByReference(string sReference)
        {
            List<TObject> tList = FinByReference(sReference);
            TObject rReturn = null;
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