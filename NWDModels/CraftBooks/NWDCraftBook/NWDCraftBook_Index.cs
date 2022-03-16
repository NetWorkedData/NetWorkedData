//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_CRAFTBOOK
using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDCraftBookIndexer : NWDIndexer<NWDCraftBook>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDCraftBook tData = (NWDCraftBook)sData;
            tData.InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDCraftBook tData = (NWDCraftBook)sData;
            tData.RemoveFromIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftBook : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDCraftBook>> kIndex = new Dictionary<string, List<NWDCraftBook>>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDCraftBook> CraftBookForItem(NWDItem sRecipient, NWDReferencesArrayType<NWDItem> sItems)
        {
            List<NWDCraftBook> rReturn = new List<NWDCraftBook>();
            foreach (string tHash in IndexKeyForItem(sRecipient, sItems))
            {
                Debug.Log(" CraftBookForItem() tHash : " + tHash);
                if (kIndex.ContainsKey(tHash))
                {
                    foreach (NWDCraftBook Craft in kIndex[tHash])
                    {
                        rReturn.Add(Craft);
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDCraftBook FindFirstByIndex(NWDItem sRecipient, NWDReferencesArrayType<NWDItem> sItems)
        {
            NWDCraftBook rReturn = null;
            List<NWDCraftBook> tFoundList = CraftBookForItem(sRecipient, sItems);
            if (tFoundList.Count > 0)
            {
                rReturn = tFoundList[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static string[] IndexKeyForItem(NWDItem sRecipient, NWDReferencesArrayType<NWDItem> sItems)
        {
            List<string> rReturn = new List<string>();
            rReturn.Add(("A*" + sRecipient.Reference + "*" + string.Join(NWEConstants.K_HASHTAG, sItems.GetSortedReferences())).Replace("ITM", ""));
            rReturn.Add(("B*" + sRecipient.Reference + "*" + string.Join(NWEConstants.K_HASHTAG, sItems.GetReferences())).Replace("ITM", ""));
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        static string[] IndexKeyByItem(bool sOrderIsImportant, NWDReferenceType<NWDCraftRecipient> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        {
            List<string> rReturn = new List<string>();
            NWDCraftRecipient tRecipientGroup = sRecipientGroup.GetRawData();
            if (tRecipientGroup != null)
            {
                foreach (NWDItem tRecipient in tRecipientGroup.ItemGroup.GetRawData().ItemList.GetRawDatas())
                {
                    if (sOrderIsImportant == false)
                    {
                        foreach (string tSign in GetSignature(sItemGroupIngredient.GetSortedReferences(), false))
                        {
                            rReturn.Add(("A*" + tRecipient.Reference + "*" + tSign).Replace("ITM", ""));
                        }
                    }
                    else
                    {
                        foreach (string tSign in GetSignature(sItemGroupIngredient.GetReferences(), true))
                        {
                            rReturn.Add(("B*" + tRecipient.Reference + "*" + tSign).Replace("ITM", ""));
                        }
                    }
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        static string[] GetSignature(string[] sItemGroupIngredient, bool sOrderIsImportant)
        {
            Debug.Log("GetSignature()");
            List<List<string>> Final = new List<List<string>>();
            foreach (string tA in sItemGroupIngredient)
            {
                NWDItemGroup tItemGroup = NWDBasisHelper.GetRawDataByReference<NWDItemGroup>(tA);
                if (tItemGroup != null)
                {
                    NWDItem[] tItems = tItemGroup.ItemList.GetRawDatas();
                    List<List<string>> FinalIntermediares = new List<List<string>>();
                    foreach (NWDItem tItem in tItems)
                    {
                        if (Final.Count > 0)
                        {
                            // continue loop
                            foreach (List<string> tPreview in Final)
                            {
                                List<string> tNewList = new List<string>(tPreview);
                                tNewList.Add(tItem.Reference);
                                FinalIntermediares.Add(tNewList);
                            }
                        }
                        else
                        {
                            // start loop
                            List<string> tNewList = new List<string>();
                            tNewList.Add(tItem.Reference);
                            FinalIntermediares.Add(tNewList);
                        }
                    }
                    Final = FinalIntermediares;
                }
            }
            List<string> rResult = new List<string>();
            foreach (List<string> tLL in Final)
            {
                if (sOrderIsImportant == false)
                {
                    tLL.Sort();
                }
                rResult.Add((string.Join(NWEConstants.K_HASHTAG, tLL)).Replace("ITM", ""));
            }
            return rResult.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //NWDBenchmark.Start();
            if (IsEnable() == true
                && IsTrashed() == false
                && IntegrityIsValid() == true)
            {
                if (RecipeHashesArray != null)
                {
                    foreach (string tHash in RecipeHashesArray.GetReferences())
                    {
                        //Debug.Log("InsertInIndex reference =" + Reference + " tHash = "+ tHash);
                        if (kIndex.ContainsKey(tHash))
                        {
                            List<NWDCraftBook> tList = kIndex[tHash];
                            if (tList.Contains(this))
                            {
                                // I am in the good index ... do nothing
                            }
                            else
                            {
                                // I Changed index! during update ?!!
                                tList.Add(this);
                            }
                        }
                        else
                        {
                            List<NWDCraftBook> tList = new List<NWDCraftBook>();
                            kIndex.Add(tHash, tList);
                            tList.Add(this);
                        }
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromIndex()
        {
            if (RecipeHashesArray != null)
            {
                foreach (string tHash in RecipeHashesArray.GetReferences())
                {
                    if (kIndex.ContainsKey(tHash))
                    {
                        List<NWDCraftBook> tList = kIndex[tHash];
                        tList.Remove(this);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
