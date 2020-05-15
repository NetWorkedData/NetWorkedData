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
    public class NWDItemIndexer : NWDIndexer
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDIndex<NWDCategory, NWDItem> kCategoryIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        static public NWDIndex<NWDCategory, NWDItem> kCategoryInverseIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        static public void Install()
        {
            Debug.Log("NWDItemIndexer Install()");
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserOwnership));
            //------------ in memory
            tHelper.IndexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("InsertInCategoryIndex"));
            tHelper.DeindexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("RemoveFromCategoryIndex"));
            //------------ in memory
            tHelper.IndexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("InsertInCategoryInverseIndex"));
            tHelper.DeindexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("RemoveFromCategoryInverseIndex"));
            //------------ in base
            tHelper.IndexInBaseMethodList.Add(tHelper.ClassType.GetMethod("IndexInSQLCategorieItem"));
            tHelper.DeindexInBaseMethodList.Add(tHelper.ClassType.GetMethod("DesindexInSQLCategorieItem"));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        //[NWDIndexInBase]
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
        //[NWDDeindexInBase]
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
        //[NWDIndexInMemory]
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
            NWDItemIndexer.kCategoryIndex.UpdateData(this, tCategoriesList.ToArray());
            NWDIndexCategorieItem.UpdateData(this, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDDeindexInMemory]
        public void RemoveFromCategoryIndex()
        {
            // Remove from the actual indexation
            NWDItemIndexer.kCategoryIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategory(NWDCategory sCategory)
        {
            List<NWDItem> rReturn = new List<NWDItem>();
            foreach (NWDCategory tCategories in sCategory.CascadeCategoryList.GetRawDatas())
            {
                rReturn.AddRange(NWDItemIndexer.kCategoryIndex.RawDatasByKey(tCategories));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDIndexInMemory]
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
            NWDItemIndexer.kCategoryInverseIndex.UpdateData(this, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDDeindexInMemory]
        public void RemoveFromCategoryInverseIndex()
        {
            // Remove from the actual indexation
            NWDItemIndexer.kCategoryInverseIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategoryInverse(NWDCategory sCategory)
        {
            return new List<NWDItem>(NWDItemIndexer.kCategoryInverseIndex.RawDatasByKey(sCategory));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
