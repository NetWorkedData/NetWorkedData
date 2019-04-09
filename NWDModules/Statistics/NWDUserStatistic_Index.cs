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
    public partial class NWDUserStatistic : NWDBasis<NWDUserStatistic>
    {

        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDStatisticKey, NWDUserStatistic> kStatisticKeyIndex = new NWDIndex<NWDStatisticKey, NWDUserStatistic>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = StatKey.GetReference() + NWDConstants.kFieldSeparatorA + this.GameSave.GetReference();
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
        public static NWDUserStatistic FindFisrtByStatistic(NWDStatisticKey sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.Current().Reference;
            NWDUserStatistic rReturn = kStatisticKeyIndex.FindFirstByReference(tKey);
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
        // TODO : Change for new index
        static protected NWDIndex<NWDStatisticKey, NWDUserStatistic> kStatisticKeyIndex = new NWDIndex<NWDStatisticKey, NWDUserStatistic>();
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDUserStatistic>> kIndex = new Dictionary<string, List<NWDUserStatistic>>();
        private List<NWDUserStatistic> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (StatKey.GetReference() != null
                && GameSave != null
                && GameSave.GetReference() != null  // permet aussi d'avoir indirectement l'account
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = StatKey.GetReference() + "*" + GameSave.GetReference();
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
                        kIndexList = new List<NWDUserStatistic>();
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
                        kIndexList = new List<NWDUserStatistic>();
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
        static public List<NWDUserStatistic> FindByIndex(NWDAccountStatistic sDataKey, NWDGameSave sGameSave)
        {
            //BTBBenchmark.Start();
            List<NWDUserStatistic> rReturn = null;
            if (sDataKey != null && sGameSave != null)
            {
                string tKey = sDataKey.Reference + "*" + sGameSave.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserStatistic> FindByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            List<NWDUserStatistic> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserStatistic FindFirstByIndex(string sDataKeyReference)
        {
            //BTBBenchmark.Start();
            NWDUserStatistic rObject = null;
            List<NWDUserStatistic> rReturn = null;
            if (sDataKeyReference != null)
            {
                string tKey = sDataKeyReference + "*" + NWDGameSave.Current().Reference;
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
        public static NWDUserStatistic UserStatForKey(string sDataReference)
        {
            //BTBBenchmark.Start();
            NWDUserStatistic rReturn = FindFirstByIndex(sDataReference);
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
                rReturn.Min = tKey.InitMin;
                rReturn.Max = tKey.InitMax;
                // update in writing mode (default is poolThread for stats)
                rReturn.UpdateData(true, kWritingMode);
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        */
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================