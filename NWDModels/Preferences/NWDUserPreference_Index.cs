//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserPreference : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDPreferenceKey, NWDUserPreference> kAchievementKeyIndex = new NWDIndex<NWDPreferenceKey, NWDUserPreference>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = PreferenceKey.GetReference() + NWDConstants.kFieldSeparatorA + this.Account.GetReference();
                kAchievementKeyIndex.UpdateData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kAchievementKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserPreference FindDataByPreferenceKey(NWDPreferenceKey sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDAccount.CurrentReference();
            NWDUserPreference rReturn = kAchievementKeyIndex.FirstRawDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDUserPreference>();
                rReturn.InternalKey = sKey.InternalKey;
                rReturn.PreferenceKey.SetData(sKey);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.Value.SetValue(sKey.Default.Value);
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================