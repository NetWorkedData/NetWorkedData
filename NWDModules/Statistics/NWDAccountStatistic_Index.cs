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
    public partial class NWDAccountStatistic : NWDBasis<NWDAccountStatistic>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDStatisticKey, NWDAccountStatistic> kStatisticKeyIndex = new NWDIndex<NWDStatisticKey, NWDAccountStatistic>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = StatKey.GetReference() + NWDConstants.kFieldSeparatorA + this.Account.GetReference();
                kStatisticKeyIndex.InsertInIndex(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kStatisticKeyIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountStatistic FindFisrtByStatistic(NWDStatisticKey sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDAccount.CurrentReference();
            NWDAccountStatistic rReturn = kStatisticKeyIndex.FindFirstByReference(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.StatKey.SetObject(sKey);
                // init the stat with default value
                rReturn.Total = sKey.InitTotal;
                rReturn.Counter = sKey.InitCounter;
                rReturn.Last = sKey.InitLast;
                rReturn.Average = sKey.InitAverage;
                rReturn.Max = sKey.InitMax;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        /*
       
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDAccountStatistic>> kIndex = new Dictionary<string, List<NWDAccountStatistic>>();
        private List<NWDAccountStatistic> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
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
                        kIndexList = new List<NWDAccountStatistic>();
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
                        kIndexList = new List<NWDAccountStatistic>();
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
        [NWDIndexRemove]
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
        static public List<NWDAccountStatistic> FindByIndex(NWDAccountStatistic sDataKey)
        {
            //BTBBenchmark.Start();
            List<NWDAccountStatistic> rReturn = null;
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
        static public List<NWDAccountStatistic> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDAccountStatistic> rReturn = null;
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
        static public NWDAccountStatistic FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDAccountStatistic rObject = null;
            List<NWDAccountStatistic> rReturn = null;
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
        public static NWDAccountStatistic UserStatForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDAccountStatistic rReturn = FindFirstByIndex(sDataReference);
            NWDStatisticKey tKey = NWDStatisticKey.GetDataByReference(sDataReference);
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
        }*/
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================