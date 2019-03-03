//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemSlot : NWDBasis<NWDItemSlot>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ClassInitialization)]
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDItemSlot), typeof(NWDUserItemSlot), typeof(NWDItem), typeof(NWDUserOwnership) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemSlot()
        {
            //Debug.Log("NWDItemSlot Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemSlot(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItemSlot Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            RemoveFromOwnership = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AddItem(NWDItem sItem)
        {
            bool rReturn = false;
            NWDUserOwnership tOwnership = NWDUserOwnership.FindFirstByIndex(sItem.Reference);
            NWDUserItemSlot tUserSlot = NWDUserItemSlot.FindFirstByIndex(Reference);
            if (ItemLIst.ContainsObject(sItem))
            {
                if (tOwnership.Quantity > 0)
                {
                    if (tUserSlot.ItemsUsed.GetObjectsList().Count < Number)
                    {
                        tUserSlot.ItemsUsed.AddObject(sItem);
                        if (RemoveFromOwnership == true)
                        {
                            if (sItem.Uncountable == false)
                            {
                                NWDUserOwnership.RemoveItemToOwnership(sItem, 1);
                            }
                        }
                        tUserSlot.UpdateData();
                        rReturn = true;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool RemoveItem(NWDItem sItem)
        {
            bool rReturn = false;
            NWDUserItemSlot tUserSlot = NWDUserItemSlot.FindFirstByIndex(Reference);
            if (tUserSlot != null)
            {
                if (tUserSlot.ItemsUsed.ContainsObject(sItem))
                {
                    tUserSlot.ItemsUsed.RemoveObjects(new NWDItem[] { sItem });
                    if (RemoveFromOwnership == true)
                    {
                        if (sItem.Uncountable == false)
                        {
                            NWDUserOwnership.AddItemToOwnership(sItem, 1);
                        }
                    }
                    tUserSlot.UpdateData();
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================