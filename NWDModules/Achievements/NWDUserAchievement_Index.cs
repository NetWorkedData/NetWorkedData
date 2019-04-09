//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserAchievement : NWDBasis<NWDUserAchievement>
    {
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Change for new index
        static protected NWDIndex<NWDAchievementKey, NWDUserAchievement> kAchievementKeyIndex = new NWDIndex<NWDAchievementKey, NWDUserAchievement>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Achievement.GetReference() + NWDConstants.kFieldSeparatorA + this.GameSave.GetReference();
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
        public static NWDUserAchievement FindFisrtByAchievement(NWDAchievementKey sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.Current().Reference;
            NWDUserAchievement rReturn = kAchievementKeyIndex.FindFirstByReference(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.Achievement.SetObject(sKey);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        /*
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserAchievement>> kIndex = new Dictionary<string, List<NWDUserAchievement>>();
        private List<NWDUserAchievement> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (Achievement.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = Achievement.GetReference();
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
                        kIndexList = new List<NWDUserAchievement>();
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
                        kIndexList = new List<NWDUserAchievement>();
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
        static public List<NWDUserAchievement> FindByIndex(NWDAchievementKey sSomething)
        {
            List<NWDUserAchievement> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserAchievement> FindByIndex(string sSomething)
        {
            List<NWDUserAchievement> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserAchievement FindFirstByIndex(string sSomething)
        {
            NWDUserAchievement rObject = null;
            List<NWDUserAchievement> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething;
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