//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountStatKeyValue : NWDBasis<NWDAccountStatKeyValue>
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDAccountStatKeyValue>> kIndex = new Dictionary<string, List<NWDAccountStatKeyValue>>();
        private List<NWDAccountStatKeyValue> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonIndexMe()
        {
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            RemoveFromIndex();
        }
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
                        kIndexList = new List<NWDAccountStatKeyValue>();
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
                        kIndexList = new List<NWDAccountStatKeyValue>();
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
        static public List<NWDAccountStatKeyValue> FindByIndex(NWDAccountStatKeyValue sDataKey)
        {
            //BTBBenchmark.Start();
            List<NWDAccountStatKeyValue> rReturn = null;
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
        static public List<NWDAccountStatKeyValue> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDAccountStatKeyValue> rReturn = null;
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
        static public NWDAccountStatKeyValue FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDAccountStatKeyValue rObject = null;
            List<NWDAccountStatKeyValue> rReturn = null;
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
        public static NWDAccountStatKeyValue UserStatForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDAccountStatKeyValue rReturn = FindFirstByIndex(sDataReference);
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