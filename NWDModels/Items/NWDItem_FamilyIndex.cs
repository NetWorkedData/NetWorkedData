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
#endif
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
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
        static protected NWDIndex<NWDFamily, NWDItem> kFamilyIndex = new NWDIndex<NWDFamily, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void IndexInMemoryFamilyIndex()
        {
            //Debug.Log("index in memory");
            if (FamilyList != null)
            {
                kFamilyIndex.UpdateData(this, FamilyList.GetRawDatas());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Desindex from NWDIndexFamilyItem
        /// </summary>
        [NWDDeindexInMemory]
        public void DesindexInMemoryFamilyIndex()
        {
            kFamilyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindInMemoryFamilyIndex(NWDFamily sFamily)
        {
            return new List<NWDItem>(kFamilyIndex.RawDatasByKey(sFamily));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
