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
    public partial class NWDCraftBookAdd : NWDBasis<NWDCraftBookAdd>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftBookAdd()
        {
            //Debug.Log("NWDCraftBookAdd Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftBookAdd(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCraftBookAdd Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDCraftBookAdd), typeof(NWDCraftBook), typeof(NWDRecipientGroup) };
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public NWDReferencesQuantityType<NWDItem> GetItemRewards()
        {
            NWDReferencesQuantityType<NWDItem> tListOfItemInBag = new NWDReferencesQuantityType<NWDItem>();
            if (RewardsNumber <= 0)
            {
                tListOfItemInBag.AddReferencesQuantity(ItemsRewards);
            }
            else
            {
                tListOfItemInBag.AddObjectsList(ItemsRewards.GetObjectsByRandom(RewardsNumber));
            }
            return tListOfItemInBag;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================