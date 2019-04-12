// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    public partial class NWDUserItemSlot : NWDBasis<NWDUserItemSlot>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //------------------------------------------------------------------------------------------------------------- 
        public static NWDUserItemSlot UserSlotForSlot(string sItemReference)
        {
            NWDUserItemSlot rOwnership = FindFirstByIndex(sItemReference);
            if (rOwnership == null)
            {
                NWDItemSlot tSlot = NWDItemSlot.GetDataByReference(sItemReference);
                if (tSlot != null)
                {
                    rOwnership = NewData(kWritingMode);
                    rOwnership.ItemSlot.SetReference(sItemReference);
                    rOwnership.Tag = NWDBasisTag.TagUserCreated;
                    rOwnership.UpdateData(true, kWritingMode);
                }
            }
            return rOwnership;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserItemSlot()
        {
            //Debug.Log("NWDUserItemSlot Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserItemSlot(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserItemSlot Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Initialization
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDItem> ItemPossibilities()
        {
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            List<NWDItem> rResult = new List<NWDItem>();
            if (tSlot != null)
            {
                rResult = tSlot.ItemPossibilities();
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDItem> CheckedList(NWDItemSlot sSlot, NWDItem sNoneItem)
        {
            List<NWDItem> rResult = new List<NWDItem>();
            if (sSlot != null)
            {
                List<NWDItem> rItemUsedList = ItemsUsed.GetObjectsList();
                List<NWDItem> rItemAuthorizedList = new List<NWDItem>();
                rItemAuthorizedList = sSlot.ItemGroup.GetObject().ItemList.GetObjectsList();
                foreach (NWDItem tItem in rItemUsedList)
                {
                    if (tItem != sNoneItem)
                    {
                        if (rItemAuthorizedList.Contains(tItem))
                        {
                            rResult.Add(tItem);
                        }
                        else
                        {
                            rResult.Add(sNoneItem);
                        }
                    }
                }
                while (rResult.Count < sSlot.Number)
                {
                    rResult.Add(sNoneItem);
                }
                while (rResult.Count > sSlot.Number)
                {
                    int sIndex = rResult.Count - 1;
                    if (sSlot.FromOwnership == true)
                    {
                        NWDUserOwnership.AddItemToOwnership(rResult[sIndex], 1);
                    }
                    rResult.RemoveAt(sIndex);
                }
                ItemsUsed.SetObjects(rResult.ToArray());
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool SetItem(NWDItem sItem, int sIndex)
        {
            bool rReturn = false;
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            if (tSlot.ItemGroup.GetObject().ItemList.GetObjectsList().Contains(sItem) == false)
            {
                sItem = null;
            }
            if (tSlot != null)
            {
                NWDItem tNoneItem = tSlot.ItemNone.GetObjectAbsolute();
                List<NWDItem> tList = CheckedList(tSlot, tNoneItem);
                // index is possibles
                if (sIndex < tList.Count)
                {
                    if (tSlot.FromOwnership == true)
                    {
                        if (tList[sIndex] != null && tList[sIndex] != tNoneItem)
                        {
                            NWDUserOwnership.AddItemToOwnership(tList[sIndex], 1);
                        }
                    }
                    if (sItem != null)
                    {
                        if (tSlot.FromOwnership == true)
                        {
                            NWDUserOwnership.RemoveItemToOwnership(sItem, 1);
                        }
                        tList[sIndex] = sItem;
                    }
                    else
                    {
                        tList[sIndex] = tNoneItem;
                    }
                    ItemsUsed.SetObjects(tList.ToArray());
                    UpdateDataIfModified();
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AddItem(NWDItem sItem, int sIndex)
        {
            return SetItem(sItem, sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool RemoveItem(int sIndex)
        {
            return SetItem(null, sIndex);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem GetItem(int sIndex)
        {
            NWDItem rReturn = null;
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            if (tSlot != null)
            {
                NWDItem tNoneItem = tSlot.ItemNone.GetObjectAbsolute();
                List<NWDItem> tList = CheckedList(tSlot, tNoneItem);
                if (sIndex < tList.Count)
                {
                    rReturn = tList[sIndex];
                    if (rReturn == tSlot.ItemNone.GetObject())
                    {
                        rReturn = null;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            Debug.Log("AddonUpdateMe");
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
#if UNITY_EDITOR
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            if (tSlot != null)
            {
                if (tSlot.ItemNone != null)
                {
                    NWDItem tNoneItem = tSlot.ItemNone.GetObjectAbsolute();
                    CheckedList(tSlot, tNoneItem);
                }   
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================