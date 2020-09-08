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

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class StringIndexKeyComparer : IEqualityComparer<string>
    {
        //-------------------------------------------------------------------------------------------------------------
        const int _multiplier = 89;
        //-------------------------------------------------------------------------------------------------------------
        public bool Equals(string x, string y)
        {
            return x == y;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetHashCode(string obj)
        {
            int result = 0;
            if (obj == null)
            {
                return 0;
            }
            int length = obj.Length;
            if (length > 0)
            {
                char let1 = obj[0];
                char let2 = obj[length - 1];
                int part1 = let1 + length;
                result = (_multiplier * part1) + let2 + length;
            }
            return result;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
