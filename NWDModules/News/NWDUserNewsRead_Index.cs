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
                kAchievementKeyIndex.InsertData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kAchievementKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNewsRead FindDataByNews(NWDNews sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.CurrentData().Reference;
            NWDUserNewsRead rReturn = kAchievementKeyIndex.RawFirstDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.News.SetObject(sKey);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================