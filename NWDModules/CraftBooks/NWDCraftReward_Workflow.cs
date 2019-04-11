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
    public partial class NWDCraftReward : NWDBasis<NWDCraftReward>
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
                tListOfItemInBag.AddObjectsList(ItemBatch.GetObjectsByRandom(Quantity));
            }
            return tListOfItemInBag;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================