//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
    public partial class NWDItem : NWDBasis<NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDCategory, NWDItem> kCategoryIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInCategoryIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
                {
                // Re-add ! but for wichn categories?
                List<NWDCategory> tCategoriesList = new List<NWDCategory>();
                foreach (NWDCategory tCategories in CategoryList.GetObjectsAbsolute())
                {
                    foreach (NWDCategory tSubCategories in tCategories.CascadeCategoryList.GetObjects())
                    {
                        if (tCategoriesList.Contains(tSubCategories) == false)
                        {
                            tCategoriesList.Add(tSubCategories);
                        }
                    }
                }
                // Re-add !
                kCategoryIndex.InsertInIndex(this, tCategoriesList.ToArray());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromCategoryIndex()
        {
            // Remove from the actual indexation
            kCategoryIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        // NWDItem.kCategoryIndex.FindByKey(sCategory); => NWDItem.FindByCategory(NWDCategory sCategory)
        public static List<NWDItem> FindByCategory(NWDCategory sCategory)
        {
            return kCategoryIndex.FindByKey(sCategory); 
        }
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDFamily, NWDItem> kFamilyIndex = new NWDIndex<NWDFamily, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInFamilyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add ! but for wichn Family?
                foreach (NWDFamily tFamily in FamilyList.GetObjectsAbsolute())
                {
                    // Re-add !
                    kFamilyIndex.InsertInIndex(this, tFamily);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromFamilyIndex()
        {
            // Remove from the actual indexation
            kFamilyIndex.RemoveFromIndex(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByFamily(NWDCategory sCategory)
        {
            return kCategoryIndex.FindByKey(sCategory);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
