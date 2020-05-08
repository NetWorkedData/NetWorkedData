//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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
        public override List<Type> OverrideClasseInThisSync()
        {
            List<Type> rReturn = new List<Type> { typeof(K) };
            //Debug.Log("New_OverrideClasseInThisSync first override : " + string.Join(" ", rReturn));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static K FakeDataInstance;
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