//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserPreferenceIndexer : NWDIndexer<NWDUserPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDUserPreference tData = (NWDUserPreference)sData;
            tData.InsertInLevelIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDUserPreference tData = (NWDUserPreference)sData;
            tData.RemoveFromLevelIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserPreference : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDPreferenceKey, NWDUserPreference> kAchievementKeyIndex = new NWDIndex<NWDPreferenceKey, NWDUserPreference>();
        //-------------------------------------------------------------------------------------------------------------
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