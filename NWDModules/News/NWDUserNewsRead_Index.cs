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
    public partial class NWDUserNewsRead : NWDBasis<NWDUserNewsRead>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDNews, NWDUserNewsRead> kAchievementKeyIndex = new NWDIndex<NWDNews, NWDUserNewsRead>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = News.GetReference() + NWDConstants.kFieldSeparatorA + this.GameSave.GetReference();
                kAchievementKeyIndex.InsertInIndex(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kAchievementKeyIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNewsRead FindFisrtByNews(NWDNews sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.Current().Reference;
            NWDUserNewsRead rReturn = kAchievementKeyIndex.FindFirstByReference(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.News.SetObject(sKey);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }

        /*
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserNewsRead>> kIndex = new Dictionary<string, List<NWDUserNewsRead>>();
        private List<NWDUserNewsRead> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (News != null)
            {
                if (News.GetReference() != null
                    && IsEnable() == true
                    && IsTrashed() == false
                    && TestIntegrity() == true)
                {
                    string tKey = News.GetReference();
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
                            kIndexList = new List<NWDUserNewsRead>();
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
                            kIndexList = new List<NWDUserNewsRead>();
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
        static public List<NWDUserNewsRead> FindByIndex(NWDNews sEventMessage)
        {
            List<NWDUserNewsRead> rReturn = null;
            if (sEventMessage != null)
            {
                string tKey = sEventMessage.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserNewsRead> FindByIndex(string sEventMessage)
        {
            List<NWDUserNewsRead> rReturn = null;
            if (sEventMessage != null)
            {
                string tKey = sEventMessage;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserNewsRead FindFirstByIndex(string sEventMessage)
        {
            NWDUserNewsRead rObject = null;
            List<NWDUserNewsRead> rReturn = null;
            if (sEventMessage != null)
            {
                string tKey = sEventMessage;
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
        }
        //-------------------------------------------------------------------------------------------------------------
        */
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================