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
#if NWD_USER_IDENTITY
using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserAvatarIndexer : NWDIndexer<NWDUserAvatar>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserAvatar> kIndex = new NWDIndex<NWDGameSave, NWDUserAvatar>();
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDUserAvatar tData = (NWDUserAvatar)sData;
            if (tData.IsUsable())
            {
                kIndex.UpdateData(tData, tData.GameSave.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDUserAvatar tData = (NWDUserAvatar)sData;
            kIndex.RemoveData(tData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserAvatar FindFirstDataByKey(NWDGameSave sKeyValue)
        {
            return kIndex.FirstRawDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserAvatar : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserAvatar CurrentData(bool sOrCreate = true)
        {

            NWDUserAvatar rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserAvatar));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                rReturn = NWDUserAvatarIndexer.FindFirstDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserAvatar>();
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