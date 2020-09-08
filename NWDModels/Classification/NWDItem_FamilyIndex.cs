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
    [NWDClassTrigrammeAttribute("xxf")]
    [NWDClassDescriptionAttribute("index")]
    [NWDClassMenuNameAttribute("Index Family->Item")]
    public partial class NWDIndexFamilyItem : NWDEditorIndex<NWDIndexFamilyItem, NWDFamily, NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexFamilyItem()
        {
            //Debug.Log("NWDIndexFamilyItem Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIndexFamilyItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDIndexFamilyItem Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Classification", 1)]
        public NWDReferencesListType<NWDFamily> FamilyList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Index with NWDIndexFamilyItem
        /// </summary>
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInBase]
        public void IndexInBaseFamilyIndex()
        {
            //Debug.Log("index in base");
            if (FamilyList != null)
            {
                NWDIndexFamilyItem.UpdateData(this, FamilyList.GetRawDatas());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Desindex from NWDIndexFamilyItem
        /// </summary>
        [NWDDeindexInBase]
        public void DesindexInBaseFamilyIndex()
        {
            NWDIndexFamilyItem.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindInBaseFamilyIndex(NWDFamily sFamily)
        {
            return new List<NWDItem>(NWDIndexFamilyItem.RawDatasByKey(sFamily));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
