//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:41
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftBook : NWDBasis<NWDCraftBook>
    {
        //-------------------------------------------------------------------------------------------------------------
        //static NWDWritingMode kWritingMode = NWDWritingMode.PoolThread;
        static Dictionary<string, List<NWDCraftBook>> kIndex = new Dictionary<string, List<NWDCraftBook>>();
        //-------------------------------------------------------------------------------------------------------------
        //static string IndexKey(bool sOrderIsImportant, NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        //{
        //    string rReturn = string.Empty;
        //    if (sOrderIsImportant == false)
        //    {
        //        rReturn = sOrderIsImportant.ToString() + "*" + sRecipientGroup.GetReference() + "*" + string.Join(BTBConstants.K_HASHTAG, sItemGroupIngredient.GetSortedReferences());
        //    }
        //    else
        //    {
        //        rReturn = sOrderIsImportant.ToString() + "*" + sRecipientGroup.GetReference() + "*" + string.Join(BTBConstants.K_HASHTAG, sItemGroupIngredient.GetReferences());
        //    }
        //    // Use to Hash more quickly
        //    rReturn = BTBSecurityTools.GenerateSha(rReturn, BTBSecurityShaTypeEnum.Sha1);
        //    return rReturn;
        //}
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
            rReturn.Add(("A*" + sRecipient.Reference + "*" + string.Join(BTBConstants.K_HASHTAG, sItems.GetSortedReferences())).Replace("ITM", ""));
            rReturn.Add(("B*" + sRecipient.Reference + "*" + string.Join(BTBConstants.K_HASHTAG, sItems.GetReferences())).Replace("ITM", ""));
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        static string[] IndexKeyByItem(bool sOrderIsImportant, NWDReferenceType<NWDCraftRecipient> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        {
            List<string> rReturn = new List<string>();
            NWDCraftRecipient tRecipientGroup = sRecipientGroup.GetData();
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
            List< List<string>> Final = new List<List<string>>();
            foreach (string tA in sItemGroupIngredient)
            {
                NWDItemGroup tItemGroup = NWDItemGroup.GetRawDataByReference(tA);
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
                rResult.Add((string.Join(BTBConstants.K_HASHTAG, tLL)).Replace("ITM", ""));
            }
            return rResult.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
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
            else
            {
                RemoveFromIndex();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
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
        //static public List<NWDCraftBook> FindByIndex(NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        //{
        //    //BTBBenchmark.Start();
        //    List<NWDCraftBook> rReturn = null;
        //    string tKey = IndexKey(true, sRecipientGroup, sItemGroupIngredient);
        //    string tKeyNoOrder = IndexKey(false, sRecipientGroup, sItemGroupIngredient);
        //    if (kIndex.ContainsKey(tKey))
        //    {
        //        rReturn = kIndex[tKey];
        //    }
        //    else if (kIndex.ContainsKey(tKeyNoOrder))
        //    {
        //        rReturn = kIndex[tKeyNoOrder];
        //    }
        //    //BTBBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //static public NWDCraftBook FindFirstByIndex(NWDReferenceType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        //{
        //    //BTBBenchmark.Start();
        //    NWDCraftBook rObject = null;
        //    List<NWDCraftBook> tReturn = FindByIndex(sRecipientGroup, sItemGroupIngredient);
        //    if (tReturn != null)
        //    {
        //        if (tReturn.Count > 0)
        //        {
        //            rObject = tReturn[0];
        //        }
        //    }
        //    //BTBBenchmark.Finish();
        //    return rObject;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //static public NWDCraftBook FindFirstByIndex(NWDRecipientGroup sRecipientGroup, NWDReferencesArrayType<NWDItem> sItemIngredient)
        //{
        //    //BTBBenchmark.Start();
        //    NWDCraftBook rObject = null;
        //    NWDReferenceType<NWDRecipientGroup> tRecipent = new NWDReferenceType<NWDRecipientGroup>();
        //    NWDReferencesArrayType<NWDItemGroup> tIngredients = new NWDReferencesArrayType<NWDItemGroup>();
        //    tRecipent.SetObject(sRecipientGroup);
        //    // TODO : RECOMPOSE ALL POSSIBILITIES!


        //    foreach (NWDItem tItem in sItemIngredient.GetObjects())
        //    {
        //        if (tItem.ItemGroupList.GetObjects().Length > 0)
        //        {
        //            tIngredients.AddObject(tItem.ItemGroupList.GetObjects()[0]);
        //        }
        //    }
        //    List<NWDCraftBook> tReturn = FindByIndex(tRecipent, tIngredients);
        //    if (tReturn != null)
        //    {
        //        if (tReturn.Count > 0)
        //        {
        //            rObject = tReturn[0];
        //        }
        //    }
        //    //BTBBenchmark.Finish();
        //    return rObject;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================