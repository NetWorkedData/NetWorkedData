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
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftBook : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftBook()
        {
            //Debug.Log("NWDCraftBook Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftBook(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCraftBook Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            OrderIsImportant = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType<NWDItem> GetAdditionalItem(NWDReferencesQuantityType<NWDItem> sItemInCraft)
        {
            NWDReferencesQuantityType<NWDItem> rReturn = new NWDReferencesQuantityType<NWDItem>();
            foreach (NWDCraftReward tAdd in AdditionalReward.GetReachableDatas())
            {
                if (tAdd.ItemConditional.IsValid(sItemInCraft))
                {
                    rReturn.AddReferencesQuantity(tAdd.GetItemRewards());
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDCraftBook GetFirstCraftBookCollision(NWDItem sItemA, NWDItem sItemB)
        //{
        //    Debug.Log("GetFirstCraftBookCollision NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.FindDatas().Length);
        //    NWDCraftBook tCraftBook = null;
        //    NWDItemGroup[] tGroupA = sItemA.ItemGroupList.GetObjects();
        //    NWDItemGroup[] tGroupB = sItemB.ItemGroupList.GetObjects();
        //    if (tGroupA.Length > 0 && tGroupB.Length > 0)
        //    {
        //        foreach (NWDItemGroup tItemA in tGroupA)
        //        {
        //            foreach (NWDItemGroup tItemB in tGroupB)
        //            {
        //                NWDReferencesArrayType<NWDItemGroup> tItems = new NWDReferencesArrayType<NWDItemGroup>();
        //                tItems.AddObject(tItemA);
        //                tItems.AddObject(tItemB);
        //                NWDCraftBook tCraftFound = NWDCraftBook.GetFirstCraftBookFor(tItems);
        //                if (tCraftFound != null)
        //                {
        //                    if (tCraftFound.AC == true)
        //                    {
        //                        tCraftBook = tCraftFound;
        //                    }
        //                }
        //                if (tCraftBook != null)
        //                {
        //                    break;
        //                }
        //            }
        //            if (tCraftBook != null)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    // Add Craftbook to ownership
        //    if (tCraftBook != null)
        //    {
        //        // Add to ownership  :-)
        //        if (tCraftBook.ItemDescription.GetObject() != null)
        //        {
        //        }
        //        NWDUserOwnership.SetItemToOwnership(tCraftBook.ItemDescription.GetObject(), 1);
        //    }
        //    return tCraftBook;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the first craft book without recipient between item's groups.
        /// </summary>
        /// <returns>The first craft book for.</returns>
        /// <param name="sItemGroupIngredient">S item group ingredient.</param>
        //public static NWDCraftBook GetFirstCraftBookFor(NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
        //{
        //    Debug.Log("GetFirstCraftBookFor NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.FindDatas().Length);
        //    NWDCraftBook tCraftBook = null;
        //    string tRecipientValue = string.Empty;
        //    bool tOrdered = true;
        //    string tAssemblyA = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToString();
        //    tOrdered = false;
        //    string tAssemblyB = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToStringSorted();
        //    string tRecipeHashA = NWESecurityTools.GenerateSha(tAssemblyA, NWESecurityShaTypeEnum.Sha1);
        //    string tRecipeHashB = NWESecurityTools.GenerateSha(tAssemblyB, NWESecurityShaTypeEnum.Sha1);
        //    foreach (NWDCraftBook tCraft in NWDCraftBook.FindDatas())
        //    {
        //        if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB)
        //        {
        //            tCraftBook = tCraft;
        //            break;
        //        }
        //    }
        //    // Add Craftbook to ownership
        //    if (tCraftBook != null)
        //    {
        //        // Add to ownership  :-)
        //        if (tCraftBook.ItemDescription.GetObject() != null)
        //        {
        //        }
        //        NWDUserOwnership.SetItemToOwnership(tCraftBook.ItemDescription.GetObject(), 1);
        //    }
        //    return tCraftBook;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the craft book with recipients and items inside.
        /// </summary>
        /// <returns>The craft book with recipients.</returns>
        /// <param name="sRecipientGroup">S recipient group.</param>
        /// <param name="sItemGroupIngredientList">S item group ingredient list.</param>
        //public static NWDCraftBook[] GetCraftBookWithRecipients(NWDReferencesListType<NWDRecipientGroup> sRecipientGroup, List<NWDReferencesArrayType<NWDItemGroup>> sItemGroupIngredientList)
        //{
        //    Debug.Log("GetCraftBookWithRecipients NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.FindDatas().Length);
        //    NWDCraftBook tReturnPrimary = null;
        //    NWDRecipientGroup tReturnRecipient = null;
        //    NWDReferencesArrayType<NWDItemGroup> tItemsGroupUsed = new NWDReferencesArrayType<NWDItemGroup>();
        //    List<NWDCraftBook> tReturnList = new List<NWDCraftBook>();
        //    // I get all recipients possibilities
        //    string[] tRecipientsArray = new string[] { string.Empty };
        //    if (sRecipientGroup != null)
        //    {
        //        if (sRecipientGroup.Value != string.Empty)
        //        {
        //            tRecipientsArray = sRecipientGroup.GetReferences();
        //        }
        //    }
        //    if (sItemGroupIngredientList.Count > 0)
        //    {
        //        // I sort by Order
        //        sItemGroupIngredientList.Sort((tA, tB) => tB.Value.Length.CompareTo(tA.Value.Length));
        //        // I get all items layout
        //        // I search the max lenght
        //        List<NWDReferencesArrayType<NWDItemGroup>> tItemGroupIngredientListMax = new List<NWDReferencesArrayType<NWDItemGroup>>();
        //        int tLenghtMax = sItemGroupIngredientList[0].Value.Length;
        //        foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in sItemGroupIngredientList)
        //        {
        //            if (tLenghtMax == sItemGroupIngredient.Value.Length)
        //            {
        //                tItemGroupIngredientListMax.Add(sItemGroupIngredient);
        //            }
        //        }
        //        foreach (string tRecipientValue in tRecipientsArray)
        //        {
        //            NWDRecipientGroup tRecipient = NWDRecipientGroup.FindDataByReference(tRecipientValue);


        //            if (tRecipient != null)
        //            {
        //                Debug.Log("GetCraftBookWithRecipients search for tRecipient = " + tRecipient.InternalKey);
        //                // I craft only max items composition ?
        //                if (tRecipient.CraftOnlyMax == true)
        //                {

        //                    foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in tItemGroupIngredientListMax)
        //                    {

        //                        bool tOrdered = true;
        //                        string tAssemblyA = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToString();
        //                        tOrdered = false;
        //                        string tAssemblyB = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToStringSorted();
        //                        string tRecipeHashA = NWESecurityTools.GenerateSha(tAssemblyA, NWESecurityShaTypeEnum.Sha1);
        //                        string tRecipeHashB = NWESecurityTools.GenerateSha(tAssemblyB, NWESecurityShaTypeEnum.Sha1);

        //                        Debug.Log("GetCraftBookWithRecipients search for tRecipient " + tRecipient.InternalKey + " Craft !!!only max!!! with " + sItemGroupIngredient.ToString() + "  => hash " + tRecipeHashA + " or " + tRecipeHashB);

        //                        foreach (NWDCraftBook tCraft in NWDCraftBook.FindDatas())
        //                        {
        //                            if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB)
        //                            {
        //                                tReturnList.Add(tCraft);
        //                                tReturnPrimary = tCraft;
        //                                tItemsGroupUsed = sItemGroupIngredient;
        //                                tReturnRecipient = tRecipient;
        //                                break;
        //                            }
        //                        }
        //                        if (tReturnPrimary != null)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in sItemGroupIngredientList)
        //                    {
        //                        bool tOrdered = true;
        //                        string tAssemblyA = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToString();
        //                        tOrdered = false;
        //                        string tAssemblyB = tOrdered.ToString() + tRecipientValue + sItemGroupIngredient.ToStringSorted();
        //                        string tRecipeHashA = NWESecurityTools.GenerateSha(tAssemblyA, NWESecurityShaTypeEnum.Sha1);
        //                        string tRecipeHashB = NWESecurityTools.GenerateSha(tAssemblyB, NWESecurityShaTypeEnum.Sha1);

        //                        Debug.Log("GetCraftBookWithRecipients search for tRecipient " + tRecipient.InternalKey + " Craft with " + sItemGroupIngredient.ToString() + "  => hash " + tRecipeHashA + " or " + tRecipeHashB);

        //                        foreach (NWDCraftBook tCraft in NWDCraftBook.FindDatas())
        //                        {
        //                            if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB)
        //                            {
        //                                tReturnList.Add(tCraft);
        //                                tReturnPrimary = tCraft;
        //                                tItemsGroupUsed = sItemGroupIngredient;
        //                                tReturnRecipient = tRecipient;
        //                                break;
        //                            }
        //                        }
        //                        if (tReturnPrimary != null)
        //                        {
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            if (tReturnPrimary != null)
        //            {
        //                break;
        //            }
        //        }

        //        // if I have a craftbook I change the Value of craft
        //        if (tReturnPrimary != null)
        //        {

        //            Debug.Log("GetCraftBookWithRecipients CRAFT FOUND !!!!  " + tReturnPrimary.InternalKey);
        //            tRecipientsArray = new string[] { tReturnRecipient.Reference };
        //            // Add Craftbook to ownership
        //            if (tReturnPrimary != null)
        //            {
        //                // Add to ownership  :-)
        //                if (tReturnPrimary.ItemDescription.GetObject() != null)
        //                {
        //                }
        //                NWDUserOwnership.SetItemToOwnership(tReturnPrimary.ItemDescription.GetObject(), 1);
        //            }
        //        }
        //        // I check all possibilities in rest of recipeint element by element (destructive mode?)
        //        foreach (string tRecipientValue in tRecipientsArray)
        //        {
        //            NWDRecipientGroup tRecipient = NWDRecipientGroup.FindDataByReference(tRecipientValue);
        //            if (tRecipient.CraftUnUsedElements == true)
        //            {

        //                NWDReferencesArrayType<NWDItemGroup> tItemsGroupUnUsed = sItemGroupIngredientList[0];
        //                tItemsGroupUnUsed.RemoveReferencesArray(tItemsGroupUsed);
        //                foreach (string tItemReference in tItemsGroupUnUsed.GetReferences())
        //                {
        //                    Debug.Log("GetCraftBookWithRecipients search modulo scrft for for tRecipient = " + tRecipient.InternalKey + " and item : " + tItemReference);
        //                    bool tOrdered = true;
        //                    string tAssemblyA = tOrdered.ToString() + tRecipient + tItemReference;
        //                    tOrdered = false;
        //                    string tAssemblyB = tOrdered.ToString() + tRecipient + tItemReference;
        //                    string tRecipeHashA = NWESecurityTools.GenerateSha(tAssemblyA, NWESecurityShaTypeEnum.Sha1);
        //                    string tRecipeHashB = NWESecurityTools.GenerateSha(tAssemblyB, NWESecurityShaTypeEnum.Sha1);
        //                    foreach (NWDCraftBook tCraft in NWDCraftBook.FindDatas())
        //                    {
        //                        if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB)
        //                        {
        //                            tReturnList.Add(tCraft);
        //                            // Add Craftbook to ownership
        //                            if (tCraft != null)
        //                            {
        //                                // Add to ownership  :-)
        //                                if (tCraft.ItemDescription.GetObject() != null)
        //                                {
        //                                }
        //                                NWDUserOwnership.SetItemToOwnership(tCraft.ItemDescription.GetObject(), 1);
        //                            }
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                // unused elements are destroyed
        //            }
        //        }
        //    }
        //    return tReturnList.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void RecalculMe()
        {
            if (ItemDescription != null)
            {
                if (ItemDescription.GetRawData() != null)
                {
                    NWDItem tItem = ItemDescription.GetRawData();
                    tItem.CraftBookAttachment.SetData(this);
                    tItem.InternalKey = "Recipe - " + tItem.InternalKey.Replace("Recipe - ", string.Empty);
                    tItem.UpdateDataIfModified();
                }
            }
            if (RecipientGroup == null)
            {
                RecipientGroup = new NWDReferenceType<NWDCraftRecipient>();
            }
            if (ItemGroupIngredient == null)
            {
                ItemGroupIngredient = new NWDReferencesArrayType<NWDItemGroup>();
            }


            //RecipeHash = IndexKey(OrderIsImportant, RecipientGroup, ItemGroupIngredient);
            if (RecipeHashesArray == null)
            {
                RecipeHashesArray = new NWDStringsArrayType();
            }
            RecipeHashesArray.SetReferences(IndexKeyByItem(OrderIsImportant, RecipientGroup, ItemGroupIngredient));

#if UNITY_EDITOR
            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            NWDDataInspector.ActiveRepaint();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
#if UNITY_EDITOR
            // TODO recalculate all sign possibilities
            // I need test all possibilities .. I use an Hack : if ordered == false I sort by Name before
            RecalculMe();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
