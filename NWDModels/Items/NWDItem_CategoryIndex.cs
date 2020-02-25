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
    public partial class NWDIndexCategorieItem : NWDEditorIndex <NWDCategory,NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexCategorieItem()
        {
            //Debug.Log("NWDItem Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexCategorieItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItem Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisHelper GetHelper()
        {
            if (Helper == null)
            {
                return NWDBasisHelper.FindTypeInfos(typeof(NWDIndexCategorieItem));
            }
            return Helper;
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
        static protected NWDIndex<NWDCategory, NWDItem> kCategoryIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInCategoryIndex()
        {
            //Debug.Log("InsertInCategoryIndex(" + InternalKey + ")");
            // Re-add to the actual indexation ?
            if (IsUsable())
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
                kCategoryIndex.InsertData(this, tCategoriesList.ToArray());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
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
        [NWDIndexInsert]
        public void InsertInCategoryInverseIndex()
        {
            //Debug.Log("InsertInCategoryIndex("+InternalKey+")");
            // Re-add to the actual indexation ?
            if (IsUsable())
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
                kCategoryInverseIndex.InsertData(this, tCategoriesList.ToArray());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromCategoryInverseIndex()
        {
            // Remove from the actual indexation
            kCategoryInverseIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategoryInverse(NWDCategory sCategory)
        {
            return kCategoryInverseIndex.RawDatasByKey(sCategory);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
