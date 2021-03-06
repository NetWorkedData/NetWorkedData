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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAccountPreferenceIndexer : NWDIndexer<NWDAccountPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDAccountPreference tData = (NWDAccountPreference)sData;
            tData.InsertInLevelIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDAccountPreference tData = (NWDAccountPreference)sData;
            tData.RemoveFromLevelIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountPreference : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDPreferenceKey, NWDAccountPreference> kAchievementKeyIndex = new NWDIndex<NWDPreferenceKey, NWDAccountPreference>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable() && PreferenceKey!=null)
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
        public static NWDAccountPreference FindDataByPreferenceKey(NWDPreferenceKey sKey, bool sOrCreate = true)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDAccount.CurrentReference();
            NWDAccountPreference rReturn = kAchievementKeyIndex.FirstRawDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDAccountPreference>();
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
