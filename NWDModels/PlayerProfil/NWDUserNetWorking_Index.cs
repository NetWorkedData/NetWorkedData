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
#if NWD_USER_NETWORKING
using System;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserNetWorkingIndexer : NWDIndexer<NWDUserNetWorking>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDUserNetWorking tData = (NWDUserNetWorking)sData;
            tData.InsertInTipKeyIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDUserNetWorking tData = (NWDUserNetWorking)sData;
            tData.RemoveFromTipKeyIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNetWorking : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserNetWorking> kIndex = new NWDIndex<NWDGameSave, NWDUserNetWorking>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInTipKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kIndex.UpdateData(this, this.GameSave.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromTipKeyIndex()
        {
            // Remove from the actual indexation
            kIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNetWorking CurrentData(bool sOrCreate = true)
        {
            NWDUserNetWorking rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserNetWorking));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                 rReturn = kIndex.FirstRawDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserNetWorking>();
                    rReturn.GameSave.SetData(NWDGameSave.CurrentData());
                    rReturn.UpdateData();
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif