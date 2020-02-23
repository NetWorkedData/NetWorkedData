////=====================================================================================================================
////
////  ideMobi 2019©
////
////  Date		2019-4-12 18:48:26
////  Author		Kortex (Jean-François CONTART) 
////  Email		jfcontart@idemobi.com
////  Project 	NetWorkedData for Unity3D
////
////  All rights reserved by ideMobi
////
////=====================================================================================================================

//using System;
//using System.Reflection;
//using System.Collections.Generic;
//using UnityEngine;

////=====================================================================================================================
//namespace NetWorkedData
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    public partial class NWDItem : NWDBasis
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        [NWDInspectorGroupOrder("Classification", 1)]
//        public NWDReferencesListType<NWDFamily> FamilyList
//        {
//            get; set;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        static protected NWDIndex<NWDFamily, NWDItem> kFamilyIndex = new NWDIndex<NWDFamily, NWDItem>();
//        //-------------------------------------------------------------------------------------------------------------
//        [NWDIndexInsert]
//        public void InsertInFamilyIndex()
//        {
//            // Re-add to the actual indexation ?
//            if (IsUsable())
//            {
//                if (FamilyList != null)
//                {
//                    // Re-add ! but for wichn Family?
//                    foreach (NWDFamily tFamily in FamilyList.GetRawDatas())
//                    {
//                        // Re-add !
//                        kFamilyIndex.InsertData(this, tFamily);
//                    }
//                }
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        [NWDIndexRemove]
//        public void RemoveFromFamilyIndex()
//        {
//            // Remove from the actual indexation
//            kFamilyIndex.RemoveData(this);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static List<NWDItem> FindByFamily(NWDFamily sFamily)
//        {
//            return kFamilyIndex.RawDatasByKey(sFamily);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
////=====================================================================================================================
