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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftBook : NWDBasis<NWDCraftBook>
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDCraftBook>> kIndex = new Dictionary<string, List<NWDCraftBook>>();
        private List<NWDCraftBook> kIndexList;
        //-------------------------------------------------------------------------------------------------------------
        static string IndexKey(bool sOrderIsImportant, NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        {
            string rReturn = "";
            if (sOrderIsImportant == false)
            {
                rReturn = sOrderIsImportant.ToString() + "*" + sRecipientGroup.GetReference() + "*" + string.Join("#", sItemGroupIngredient.GetSortedReferences());
            }
            else
            {
                rReturn = sOrderIsImportant.ToString() + "*" + sRecipientGroup.GetReference() + "*" + string.Join("#", sItemGroupIngredient.GetReferences());
            }
            // Use to Hash more quickly
            rReturn = BTBSecurityTools.GenerateSha(rReturn, BTBSecurityShaTypeEnum.Sha1);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = IndexKey(OrderIsImportant, RecipientGroup, ItemGroupIngredient);
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
                        kIndexList = new List<NWDCraftBook>();
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
                        kIndexList = new List<NWDCraftBook>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }
            //BTBBenchmark.Finish();
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
        static public List<NWDCraftBook> FindByIndex(NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        {
            //BTBBenchmark.Start();
            List<NWDCraftBook> rReturn = null;
            string tKey = IndexKey(true, sRecipientGroup, sItemGroupIngredient);
            string tKeyNoOrder = IndexKey(false, sRecipientGroup, sItemGroupIngredient);
            if (kIndex.ContainsKey(tKey))
            {
                rReturn = kIndex[tKey];
            }
            else if (kIndex.ContainsKey(tKeyNoOrder))
            {
                rReturn = kIndex[tKeyNoOrder];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDCraftBook FindFirstByIndex(NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        {
            //BTBBenchmark.Start();
            NWDCraftBook rObject = null;
            List<NWDCraftBook> tReturn = FindByIndex(sRecipientGroup, sItemGroupIngredient);
            if (tReturn != null)
            {
                if (tReturn.Count > 0)
                {
                    rObject = tReturn[0];
                }
            }
            //BTBBenchmark.Finish();
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================