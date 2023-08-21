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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCategory : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCategory() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDCategory(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() {}
        //-------------------------------------------------------------------------------------------------------------
        public bool Containts(NWDCategory sCategory)
        {
            bool rReturn = false;
            if (sCategory == this)
            {
                rReturn = true;
            }
            else
            {
                rReturn = CascadeCategoryList.ConstaintsData(sCategory);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetSiblingIndex(int sParentIndex = 0)
        {
            int rResult = -1;
            NWDCategory[] tParents = ParentCategoryList.GetReachableDatas();
            if (tParents != null && tParents.Length > sParentIndex && sParentIndex >= 0)
            {
                NWDCategory[] tSiblings = tParents[sParentIndex].ChildrenCategoryList.GetReachableDatas();
                if (tSiblings != null)
                {
                    rResult = Array.IndexOf(tSiblings, this);
                }
            }

            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
