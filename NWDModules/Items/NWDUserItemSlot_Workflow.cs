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
                    NWDItem tItemNone = tSlot.ItemNone.GetObject();
                    if (tItemNone != null)
                    {
                        for (int tI = 0; tI < tSlot.Number; tI++)
                        {
                            rOwnership.ItemsUsed.AddObject(tItemNone);
                        }
                    }
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
        private void CheckList()
        {
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            List<NWDItem> tList = ItemsUsed.GetObjectsList();
            if (tSlot != null)
            {
                NWDItem tNoneItem = tSlot.ItemNone.GetObject();
                if (tNoneItem != null)
                {
                    while (tList.Count <= tSlot.Number)
                    {
                        tList.Add(tNoneItem);
                    }
                    ItemsUsed.SetObjects(tList.ToArray());
                    while (tList.Count > tSlot.Number)
                    {
                        RemoveItem(tList.Count-1);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AddItem(NWDItem sItem, int sIndex)
        {
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            bool rReturn = false;
            List<NWDItem> tList = ItemsUsed.GetObjectsList();
            if (tSlot != null)
            {
                NWDItem tNoneItem = tSlot.ItemNone.GetObject();
                if (sIndex < tList.Count)
                {
                    if (tList[sIndex] != null && tList[sIndex] != tNoneItem)
                    {
                        NWDUserOwnership.AddItemToOwnership(tList[sIndex], 1);
                    }
                    if (sItem != null)
                    {
                        NWDUserOwnership.RemoveItemToOwnership(sItem, 1);
                        tList[sIndex] = sItem;
                    }
                    else
                    {
                        tList[sIndex] = tNoneItem;
                    }
                    if (tNoneItem != null)
                    {
                        while (tList.Count < tSlot.Number)
                        {
                            tList.Add(tNoneItem);
                        }
                    }
                    ItemsUsed.SetObjects(tList.ToArray());
                    UpdateDataIfModified();
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool RemoveItem(int sIndex)
        {
            NWDItemSlot tSlot = ItemSlot.GetObjectAbsolute();
            bool rReturn = false;
            List<NWDItem> tList = ItemsUsed.GetObjectsList();
            if (tSlot != null)
            {
                if (sIndex < tList.Count)
                {
                    NWDItem tNoneItem = tSlot.ItemNone.GetObject();
                    if (tList[sIndex] != null && tList[sIndex] != tNoneItem)
                    {
                        NWDUserOwnership.AddItemToOwnership(tList[sIndex], 1);
                    }
                    if (tNoneItem != null)
                    {
                        tList[sIndex] = tNoneItem;
                        while (tList.Count < tSlot.Number)
                        {
                            tList.Add(tNoneItem);
                        }
                    }
                    else
                    {
                        tList.RemoveAt(sIndex);
                    }
                    ItemsUsed.SetObjects(tList.ToArray());
                    UpdateDataIfModified();
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem GetItem(int sIndex)
        {
            NWDItem rReturn = null;
            List<NWDItem> tList = ItemsUsed.GetObjectsList();
            if (sIndex < tList.Count)
            {
                rReturn = tList[sIndex];
                NWDItemSlot tSlot = ItemSlot.GetObject();
                if (rReturn == tSlot.ItemNone.GetObject())
                {
                    rReturn = null;
                }
            }
            else
            {
                rReturn = null;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
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
            CheckList();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        //public override void AddonUpdatedMeFromWeb()
        //{
        //    // do something when object finish to be updated from CSV from WebService response
        //    // TODO verif if method is call in good place in good timing
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonSyncForce()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================