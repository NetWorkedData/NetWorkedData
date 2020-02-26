//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("xxc")]
    [NWDClassDescriptionAttribute("index")]
    [NWDClassMenuNameAttribute("Index Categorie->Item")]
    public partial class NWDIndexCategorieItem : NWDEditorIndex<NWDIndexCategorieItem, NWDCategory, NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexCategorieItem()
        {
            //Debug.Log("NWDIndexCategorieItem Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexCategorieItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDIndexCategorieItem Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        //[NWDInspectorGroupStart("Classification", true, true, true)]
        [NWDInspectorGroupOrder("Classification", 0)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New index with NWDNewIndex NWDBasis
        /// </summary>
        [NWDIndexInBase]
        public void IndexInSQLCategorieItem()
        {
            // Re-add ! but for wichn categories?
            List<NWDCategory> tCategoriesList = new List<NWDCategory>();
            if (CategoryList != null)
            {
                foreach (NWDCategory tCategories in CategoryList.GetRawDatas())
                {
                    if (tCategoriesList.Contains(tCategories) == false)
                    {
                        tCategoriesList.Add(tCategories);
                    }
                }
            }
            NWDIndexCategorieItem.UpdateData(this, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Remove from index with NWDNewIndex NWDBasis
        /// </summary>
        [NWDDeindexInBase]
        public void DesindexInSQLCategorieItem()
        {
            NWDIndexCategorieItem.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByIndexCategorieItem(NWDCategory sCategory)
        {
            List<NWDItem> rReturn = new List<NWDItem>(NWDIndexCategorieItem.RawDatasByKey(sCategory));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDCategory, NWDItem> kCategoryIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInCategoryIndex()
        {
            // Re-add ! but for wichn categories?
            List<NWDCategory> tCategoriesList = new List<NWDCategory>();
            if (CategoryList != null)
            {
                foreach (NWDCategory tCategories in CategoryList.GetRawDatas())
                {
                    if (tCategoriesList.Contains(tCategories) == false)
                    {
                        tCategoriesList.Add(tCategories);
                    }
                }
            }
            kCategoryIndex.UpdateData(this, tCategoriesList.ToArray());
            NWDIndexCategorieItem.UpdateData(this, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromCategoryIndex()
        {
            // Remove from the actual indexation
            kCategoryIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategory(NWDCategory sCategory)
        {
            List<NWDItem> rReturn = new List<NWDItem>();
            foreach (NWDCategory tCategories in sCategory.CascadeCategoryList.GetRawDatas())
            {
                rReturn.AddRange(kCategoryIndex.RawDatasByKey(tCategories));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDCategory, NWDItem> kCategoryInverseIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInCategoryInverseIndex()
        {
            // Re-add ! but for wichn categories?
            List<NWDCategory> tCategoriesList = new List<NWDCategory>();
            if (CategoryList != null)
            {
                foreach (NWDCategory tCategories in CategoryList.GetRawDatas())
                {
                    foreach (NWDCategory tSubCategories in tCategories.CascadeCategoryList.GetReachableDatas())
                    {
                        if (tCategoriesList.Contains(tSubCategories) == false)
                        {
                            tCategoriesList.Add(tSubCategories);
                        }
                    }
                }
            }
            // Re-add !
            kCategoryInverseIndex.UpdateData(this, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromCategoryInverseIndex()
        {
            // Remove from the actual indexation
            kCategoryInverseIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategoryInverse(NWDCategory sCategory)
        {
            return new List<NWDItem>(kCategoryInverseIndex.RawDatasByKey(sCategory));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
