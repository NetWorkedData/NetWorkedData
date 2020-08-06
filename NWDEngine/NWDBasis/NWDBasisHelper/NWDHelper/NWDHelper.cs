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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDHelper<K> : NWDBasisHelper where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        private static K FakeDataInstance;
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type> OverrideClasseInThisSync()
        {
            List<Type> rReturn = new List<Type> { typeof(K) };
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K FakeData()
        {
            if (FakeDataInstance == null)
            {
                FakeDataInstance = NewDataWithReference<K>("FAKE");
                FakeDataInstance.DeleteData();
            }
            return FakeDataInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FakePropertyName<R>(Expression<Func<R>> sProperty)
        {
            return NWDToolbox.PropertyName(sProperty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int FakePropertyCSVindex<R>(Expression<Func<R>> sProperty)
        {
            return CSV_IndexOf<K>(FakePropertyName(sProperty));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================