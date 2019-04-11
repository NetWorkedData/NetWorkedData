//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Linq.Expressions;
using System.Reflection;
using SQLite.Attribute;
using UnityEngine;

#if UNITY_EDITOR
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static K sFictive;
        //-------------------------------------------------------------------------------------------------------------
        public static K FictiveData()
        {
            if (sFictive == null)
            {
                sFictive = NewDataWithReference("FICTIVE");
                sFictive.DeleteData();
            }
            Debug.Log("Test Fictive Data : " + NWDToolbox.ExposeProperty(() => sFictive.Reference));
            return sFictive;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDToolbox
    {
        //-------------------------------------------------------------------------------------------------------------
        // string tPropertyPath = NWDToolbox.ExposeProperty(()=>NWDAppConfiguration.SharedInstance().DevEnvironment);
        public static string ExposeProperty<T>(Expression<Func<T>> sProperty)
        {
            string tPath = string.Empty;
            MemberExpression tExp = MemberInfo(sProperty);
            if (tExp != null)
            {
                tPath = tExp.Member.DeclaringType.FullName + "." + tExp.Member.Name;
            }
            return tPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        // string tPropertyName = NWDToolbox.PropertyName(()=>NWDAppConfiguration.SharedInstance().DevEnvironment);
        public static string PropertyName<T>(Expression<Func<T>> sProperty)
        {
            string tPath = string.Empty;
            MemberExpression tExp = MemberInfo(sProperty);
            if (tExp != null)
            {
                tPath = tExp.Member.Name;
            }
            return tPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static MemberExpression MemberInfo(Expression sMethod)
        {
            MemberExpression rReturn = null;
            LambdaExpression tLambda = sMethod as LambdaExpression;
            if (tLambda != null)
            {
                if (tLambda.Body.NodeType == ExpressionType.Convert)
                {
                    rReturn = ((UnaryExpression)tLambda.Body).Operand as MemberExpression;
                }
                else if (tLambda.Body.NodeType == ExpressionType.MemberAccess)
                {
                    rReturn = tLambda.Body as MemberExpression;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void TEST()
        {
            string tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().DevEnvironment);
            string tPropertyNameB = NWDToolbox.PropertyName(() => NWDConstants.kFieldNone);
            //string tPropertyNameC = NWDToolbox.PropertyName(() => NWDConstants.ReferenceEquals);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif