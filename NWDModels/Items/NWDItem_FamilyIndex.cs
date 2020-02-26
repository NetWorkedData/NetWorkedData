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
        [NWDIndexUpdate]
        public void InsertInFamilyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                if (FamilyList != null)
                {
                    // Re-add ! but for wichn Family?
                    foreach (NWDFamily tFamily in FamilyList.GetRawDatas())
                    {
                        // Re-add !
                        NWDIndexFamilyItem.UpdateData(this, tFamily);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByFamily(NWDFamily sFamily)
        {
            return new List<NWDItem>(NWDIndexFamilyItem.RawDatasByKey(sFamily));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
