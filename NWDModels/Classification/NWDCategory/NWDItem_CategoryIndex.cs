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
#if NWD_CLASSIFICATION
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassMacro("NWD_CLASSIFICATION")]
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
            if( sCategory != null)
            {
                if(sCategory.CascadeCategoryList != null)
                {
                    NWDCategory[] tArrays = sCategory.CascadeCategoryList.GetRawDatas();
                    foreach (NWDCategory tCategories in tArrays)
                    {
                        rReturn.AddRange(NWDIndexCategorieItem.RawDatasByKey(tCategories));
                    }
                }
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
#endif
