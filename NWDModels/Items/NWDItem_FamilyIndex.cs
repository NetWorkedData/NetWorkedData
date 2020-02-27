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
