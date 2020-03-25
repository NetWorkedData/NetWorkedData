//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:47
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Linq.Expressions;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
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