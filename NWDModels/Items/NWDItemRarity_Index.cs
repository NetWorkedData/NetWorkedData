//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:34
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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemRarity : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Change for new index
        static protected NWDIndex<NWDItem, NWDItemRarity> kItemIndex = new NWDIndex<NWDItem, NWDItemRarity>();
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDItemRarity>> kIndex = new Dictionary<string, List<NWDItemRarity>>();
        private List<NWDItemRarity> kIndexList;
        // lors du changement si kIndexList !=  de kIndexItemReverse[item.ref + gamesave.ref] => on a changer l'item ou le gamesave 
        // je retire de la kIndexList et je cherche la nuvelle kIndexList et je la memorise et la rajoute
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (ItemReference.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && IntegrityIsValid() == true)
            {
                string tKey = ItemReference.GetReference();
                if (kIndexList != null)
                {
                    // I have already index
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
                        kIndexList = new List<NWDItemRarity>();
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
                        kIndexList = new List<NWDItemRarity>();
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
        static public List<NWDItemRarity> FindByIndex(NWDItem sItem)
        {
            List<NWDItemRarity> rReturn = null;
            if (sItem != null)
            {
                string tKey = sItem.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDItemRarity> FindByIndex(string sItemreference)
        {
            List<NWDItemRarity> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDItemRarity FindFirstByIndex(string sItemreference)
        {
            NWDItemRarity rObject = null;
            List<NWDItemRarity> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference;
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
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================