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
        public List<NWDItem> ItemPossibilities()
        {
            List<NWDItem> rItemsPossibilities = ItemGroup.GetObject().OwnershipIntersection();
            NWDItem tItemNone = ItemNone.GetObject();
            if (tItemNone != null)
            {
                rItemsPossibilities.Insert(0, tItemNone);
            }
            return rItemsPossibilities;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UserAddItem(NWDItem sItem, int sIndex)
        {
            NWDUserItemSlot tUserSlot = NWDUserItemSlot.FindFirstByIndex(Reference);
            return tUserSlot.AddItem(sItem, sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UserRemoveItem(int sIndex)
        {
            NWDUserItemSlot tUserSlot = NWDUserItemSlot.FindFirstByIndex(Reference);
            return tUserSlot.RemoveItem(sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem UserGetItem(int sIndex)
        {
            NWDUserItemSlot tUserSlot = NWDUserItemSlot.FindFirstByIndex(Reference);
            return tUserSlot.GetItem(sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================