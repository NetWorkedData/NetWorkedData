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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserInfosIndexer : NWDIndexer<NWDUserInfos>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserInfos> kIndex = new NWDIndex<NWDGameSave, NWDUserInfos>();
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDUserInfos tData = (NWDUserInfos)sData;
            if (tData.IsUsable())
            {
                kIndex.UpdateData(tData, tData.GameSave.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDUserInfos tData = (NWDUserInfos)sData;
            kIndex.RemoveData(tData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos FindFirstDataByKey(NWDGameSave sKeyValue)
        {
            return kIndex.FirstRawDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos CurrentData(bool sOrCreate = true)
        {
            NWDUserInfos rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserInfos));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                rReturn = NWDUserInfosIndexer.FindFirstDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserInfos>();
                    rReturn.GameSave.SetData(NWDGameSave.CurrentData());
                    rReturn.UpdateData();
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDUserInfos CurrentData()
        {
            string tGameSaveReference = NWDGameSave.CurrentData().Reference;
            NWDUserInfos tInfos = NWDBasisHelper.GetRawDataByReference<NWDUserInfos>(tGameSaveReference);
            if (tInfos == null)
            {
                tInfos = NWDBasisHelper.NewDataWithReference<NWDUserInfos>(tGameSaveReference);
                tInfos.SaveData();
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================