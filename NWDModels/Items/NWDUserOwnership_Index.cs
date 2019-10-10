//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        public NWDUserOwnership FindReachableUserOwnerShip(bool sOrCreate = true)
        {
            return NWDUserOwnership.FindReachableByItem(this, sOrCreate);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserOwnership : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDItem, NWDUserOwnership> kAchievementKeyIndex = new NWDIndex<NWDItem, NWDUserOwnership>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Item.GetReference() + NWDConstants.kFieldSeparatorA + GameSave.GetReference();
                kAchievementKeyIndex.InsertData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kAchievementKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindReachableByItem(NWDItem sKey, bool sOrCreate = true)
        {
            return FindReachableByItemReference(sKey.Reference, sOrCreate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindReachableByItemReference(string sReference, bool sOrCreate = true)
        {
            string tKey = sReference + NWDConstants.kFieldSeparatorA + NWDGameSave.CurrentData().Reference;
            NWDUserOwnership rReturn = kAchievementKeyIndex.RawFirstDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDUserOwnership>();
#if UNITY_EDITOR
                NWDItem tItem = NWDBasisHelper.GetRawDataByReference<NWDItem>(sReference);
                if (tItem != null)
                {
                    if (tItem.Name != null)
                    {
                        string tItemNameBase = tItem.Name.GetBaseString();
                        if (tItemNameBase != null)
                        {
                            rReturn.InternalKey = tItemNameBase;
                        }
                    }
                }
                rReturn.InternalDescription = NWDUserNickname.GetNickname();
#endif
                rReturn.Item.SetReference(sReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.Quantity = 0;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        // OWNERSHIP AND ITEM FOR PLAYER
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item's reference.
        /// </summary>
        /// <returns>The ownership.</returns>
        /// <param name="sItemReference">S item reference.</param>
        //        public static NWDUserOwnership OwnershipForItem(string sItemReference)
        //        {
        //            NWDUserOwnership rOwnership = FindFisrtByItemReference(sItemReference);
        //            if (rOwnership == null)
        //            {
        //                rOwnership = NewData();
        //#if UNITY_EDITOR
        //                NWDItem tItem = NWDItem.GetDataByReference(sItemReference);
        //                if (tItem != null)
        //                {
        //                    if (tItem.Name != null)
        //                    {
        //                        string tItemNameBase = tItem.Name.GetBaseString();
        //                        if (tItemNameBase != null)
        //                        {
        //                            rOwnership.InternalKey = tItemNameBase;
        //                        }
        //                    }
        //                }
        //                rOwnership.InternalDescription = NWDUserNickname.GetNickname();
        //#endif
        //        rOwnership.Item.SetReference(sItemReference);
        //        rOwnership.Tag = NWDBasisTag.TagUserCreated;
        //        rOwnership.Quantity = 0;
        //        rOwnership.UpdateData(true);
        //    }
        //    return rOwnership;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the Ownership for selected item.
        /// </summary>
        /// <returns>The ownership.</returns>
        /// <param name="sItem">selected item.</param>
        //public static NWDUserOwnership OwnershipForItem(NWDItem sItem)
        //{
        //    return sItem != null ? OwnershipForItem(sItem.Reference) : null;
        //}
        /*
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserOwnership>> kIndex = new Dictionary<string, List<NWDUserOwnership>>();
        private List<NWDUserOwnership> kIndexList;
        // lors du changement si kIndexList !=  de kIndexItemReverse[item.ref + gamesave.ref] => on a changer l'item ou le gamesave 
        // je retire de la kIndexList et je cherche la nuvelle kIndexList et je la memorise et la rajoute
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (Item.GetReference() != null
                && GameSave!=null
                && GameSave.GetReference() != null  // permet aussi d'avoir indirectement l'account
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = Item.GetReference() + "*" + GameSave.GetReference();
                if (kIndexList != null)
                {
                    // I have allready index
                    if (kIndex.ContainsKey(tKey))
                    {
                        if (kIndex[tKey] == kIndexList)
                        {
                            // I am in the good index ... do nothing
                        }
                        else
                        {
                            // I Changed index! during update ?!!
                            kIndexList.Remove(this);
                            kIndexList = null;
                            kIndexList = kIndex[tKey];
                            kIndexList.Add(this);
                        }
                    }
                    else
                    {
                        kIndexList.Remove(this);
                        kIndexList = null;
                        kIndexList = new List<NWDUserOwnership>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
                else
                {
                    // I need add in index!
                    if (kIndex.ContainsKey(tKey))
                    {
                        // index exists
                        kIndexList = kIndex[tKey];
                        kIndexList.Add(this);
                    }
                    else
                    {
                        // index must be create
                        kIndexList = new List<NWDUserOwnership>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        private void RemoveFromIndex()
        {
            if (kIndexList != null)
            {
                kIndexList.Contains(this);
                {
                    kIndexList.Remove(this);
                }
                kIndexList = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserOwnership> FindByIndex(NWDItem sItem, NWDGameSave sGameSave)
        {
            List<NWDUserOwnership> rReturn = null;
            if (sItem != null && sGameSave != null)
            {
                string tKey = sItem.Reference + "*" + sGameSave.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserOwnership> FindByIndex(string sItemreference)
        {
            List<NWDUserOwnership> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserOwnership FindFirstByIndex(string sItemreference)
        {
            NWDUserOwnership rObject = null;
            List<NWDUserOwnership> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            if (rReturn != null)
            {
                if (rReturn.Count > 0)
                {
                    rObject = rReturn[0];
                }
            }
            return rObject;
        }
        */
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================