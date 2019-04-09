//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserLevelScore : NWDBasis<NWDUserLevelScore>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDLevel, NWDUserLevelScore> kLevelIndex = new NWDIndex<NWDLevel, NWDUserLevelScore>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Level.GetReference() + NWDConstants.kFieldSeparatorA + this.GameSave.GetReference();
                kLevelIndex.InsertInIndex(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kLevelIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserLevelScore FindFisrtByLevel(NWDLevel sKey, bool sOrCreate = false)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.Current().Reference;
            NWDUserLevelScore rReturn = kLevelIndex.FindFirstByReference(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.Level.SetObject(sKey);
                rReturn.UpdateData();
            }
            return rReturn;
        }

        /*
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserLevelScore>> kIndex = new Dictionary<string, List<NWDUserLevelScore>>();
        private List<NWDUserLevelScore> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (Level.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = Level.GetReference();
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
                        kIndexList = new List<NWDUserLevelScore>();
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
                        kIndexList = new List<NWDUserLevelScore>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }
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
        static public List<NWDUserLevelScore> FindByIndex(NWDLevel sLevel)
        {
            List<NWDUserLevelScore> rReturn = null;
            if (sLevel != null)
            {
                string tKey = sLevel.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserLevelScore> FindByIndex(string sLevelReference)
        {
            List<NWDUserLevelScore> rReturn = null;
            if (sLevelReference != null)
            {
                string tKey = sLevelReference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserLevelScore FindFirstByIndex(string sLevelReference)
        {
            NWDUserLevelScore rObject = null;
            List<NWDUserLevelScore> rReturn = null;
            if (sLevelReference != null)
            {
                string tKey = sLevelReference;
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
            return rObject;
        }*/
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================