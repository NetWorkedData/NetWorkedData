//=====================================================================================================================
//
//  ideMobi 2019©
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
        [NWDInspectorGroupOrder("Classification", 0)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategory(NWDCategory sCategory)
        {
            List<NWDItem> rReturn = new List<NWDItem>();
            foreach (NWDCategory tCategories in sCategory.CascadeCategoryList.GetRawDatas())
            {
                rReturn.AddRange(NWDIndexCategorieItem.RawDatasByKey(tCategories));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategoryInverse(NWDCategory sCategory)
        {
            return new List<NWDItem>(NWDIndexCategorieItem.RawDatasByKey(sCategory));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
