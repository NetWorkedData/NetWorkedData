// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:54
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterFinder : NWDBasis<NWDUserBarterFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDBarterPlace, NWDUserBarterFinder> kBarterPlaceIndex = new NWDIndex<NWDBarterPlace, NWDUserBarterFinder>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInBarterPlaceIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kBarterPlaceIndex.InsertData(this, BarterPlace.GetObject());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromBarterPlaceIndex()
        {
            // Remove from the actual indexation
            kBarterPlaceIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserBarterFinder> FindByBarterPlace(NWDBarterPlace sKey)
        {
            return kBarterPlaceIndex.RawDatasByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterFinder FindFirstByBarterPlace(NWDBarterPlace sKey)
        {
            return kBarterPlaceIndex.RawFirstDataByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------








            /*
        //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserBarterFinder>> kIndex = new Dictionary<string, List<NWDUserBarterFinder>>();
        private List<NWDUserBarterFinder> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        private void InsertInIndex()
        {
            if (BarterPlace.GetReference() != null
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = BarterPlace.GetReference();
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
                        kIndexList = new List<NWDUserBarterFinder>();
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
                        kIndexList = new List<NWDUserBarterFinder>();
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
        static public List<NWDUserBarterFinder> FindByIndex(NWDBarterPlace sSomething)
        {
            List<NWDUserBarterFinder> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserBarterFinder> FindByIndex(string sSomething)
        {
            List<NWDUserBarterFinder> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserBarterFinder FindFirstByIndex(string sSomething)
        {
            NWDUserBarterFinder rObject = null;
            List<NWDUserBarterFinder> rReturn = null;
            if (sSomething != null)
            {
                string tKey = sSomething;
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
        */
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================