// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:4
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterProposition : NWDBasis<NWDUserBarterProposition>
    {

        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDBarterPlace, NWDUserBarterProposition> kBarterPlaceIndex = new NWDIndex<NWDBarterPlace, NWDUserBarterProposition>();
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
        public static List<NWDUserBarterProposition> FindByBarterPlace(NWDBarterPlace sKey)
        {
            return kBarterPlaceIndex.RawDatasByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterProposition FindFirstByBarterPlace(NWDBarterPlace sKey)
        {
            return kBarterPlaceIndex.RawFirstDataByKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        /*

            //static NWDWritingMode kWritingMode = NWDWritingMode.ByDefaultLocal;
        static Dictionary<string, List<NWDUserBarterProposition>> kIndex = new Dictionary<string, List<NWDUserBarterProposition>>();
        private List<NWDUserBarterProposition> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonIndexMe()
        {
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            RemoveFromIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
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
                        kIndexList = new List<NWDUserBarterProposition>();
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
                        kIndexList = new List<NWDUserBarterProposition>();
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
        static public List<NWDUserBarterProposition> FindByIndex(NWDBarterPlace sSomething)
        {
            List<NWDUserBarterProposition> rReturn = null;
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
        static public List<NWDUserBarterProposition> FindByIndex(string sSomething)
        {
            List<NWDUserBarterProposition> rReturn = null;
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
        static public NWDUserBarterProposition FindFirstByIndex(string sSomething)
        {
            NWDUserBarterProposition rObject = null;
            List<NWDUserBarterProposition> rReturn = null;
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