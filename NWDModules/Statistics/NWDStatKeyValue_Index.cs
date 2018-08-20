//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDStatKeyValue : NWDBasis<NWDStatKeyValue>
    {
        //-------------------------------------------------------------------------------------------------------------
        // Create an Index
        // must ADD InsertInIndex(); in : 
        // AddonLoadedMe()
        // AddonUpdatedMeFromWeb()
        // AddonUpdatedMe()
        // AddonDuplicateMe()
        // must ADD RemoveFromIndex(); in : 
        // AddonDeleteMe()
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDStatKeyValue>> kIndex = new Dictionary<string, List<NWDStatKeyValue>>();
        private List<NWDStatKeyValue> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (StatKey.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = StatKey.GetReference();
                if (kIndexList != null)
                {
                    // I have allready index
                    if (kIndex.ContainsKey(tKey))
                    {
                        if (kIndex[tKey] == kIndexList)
                        {
                            // I am in the good index ... do nothing
                        }
                        else
                        {
                            // I Changed index! during update ?!!
                            kIndexList.Remove(this);
                            kIndexList = null;
                            kIndexList = kIndex[tKey];
                            kIndexList.Add(this);
                        }
                    }
                    else
                    {
                        kIndexList.Remove(this);
                        kIndexList = null;
                        kIndexList = new List<NWDStatKeyValue>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
                else
                {
                    // I need add in index!
                    if (kIndex.ContainsKey(tKey))
                    {
                        // index exists
                        kIndexList = kIndex[tKey];
                        kIndexList.Add(this);
                    }
                    else
                    {
                        // index must be create
                        kIndexList = new List<NWDStatKeyValue>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveFromIndex()
        {
            if (kIndexList != null)
            {
                kIndexList.Contains(this);
                {
                    kIndexList.Remove(this);
                }
                kIndexList = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDStatKeyValue> FindByIndex(NWDStatKeyValue sDataKey)
        {
            //BTBBenchmark.Start();
            List<NWDStatKeyValue> rReturn = null;
            if (sDataKey != null)
            {
                string tKey = sDataKey.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDStatKeyValue> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDStatKeyValue> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDStatKeyValue FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDStatKeyValue rObject = null;
            List<NWDStatKeyValue> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            if (rReturn != null)
            {
                if (rReturn.Count > 0)
                {
                    rObject = rReturn[0];
                }
            }
            //BTBBenchmark.Finish();
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDStatKeyValue UserStatForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDStatKeyValue rReturn = FindFirstByIndex(sDataReference);
            NWDStatKey tKey = NWDStatKey.GetDataByReference(sDataReference);
            if (rReturn == null && tKey!=null)
            {
                rReturn = NewData(kWritingMode);
                rReturn.StatKey.SetReference(sDataReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                // init the stat with default value
                rReturn.Total = tKey.InitTotal;
                rReturn.Counter = tKey.InitCounter;
                rReturn.Last = tKey.InitLast;
                rReturn.Average = tKey.InitAverage;
                rReturn.Max = tKey.InitMax;
                // update in writing mode (default is poolThread for stats)
                rReturn.UpdateData(true, kWritingMode);
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================