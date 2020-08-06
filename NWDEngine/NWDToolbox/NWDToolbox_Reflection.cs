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
using System.Linq.Expressions;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDToolbox
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PropertyName<T>(Expression<Func<T>> sProperty)
        {
            NWDBenchmark.QuickStart();
            string tPath = string.Empty;
            MemberExpression tExp = MemberInfo(sProperty);
            if (tExp != null)
            {
                tPath = tExp.Member.Name;
            }
            NWDBenchmark.QuickFinish();
            return tPath;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static MemberExpression MemberInfo(Expression sMethod)
        {
            //NWDBenchmark.QuickStart();
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
            //NWDBenchmark.QuickFinish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================