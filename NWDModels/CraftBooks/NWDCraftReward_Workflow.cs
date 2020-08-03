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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftReward : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftReward()
        {
            //Debug.Log("NWDCraftBookAdd Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftReward(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCraftBookAdd Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public NWDReferencesQuantityType<NWDItem> GetItemRewards()
        {
            NWDReferencesQuantityType<NWDItem> tListOfItemInBag = new NWDReferencesQuantityType<NWDItem>();
            if (Quantity <= 0)
            {
                tListOfItemInBag.AddReferencesQuantity(ItemBatch);
            }
            else
            {
                tListOfItemInBag.AddDatasList(ItemBatch.GetRawDatasByRandom(Quantity));
            }
            return tListOfItemInBag;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================