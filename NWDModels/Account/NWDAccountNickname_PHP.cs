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

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountNicknameHelper : NWDHelper<NWDAccountNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            int tChallengeIndex = FakePropertyCSVindex(() => FakeData().ClusterChallenge);
            int tRangeAccess = FakePropertyCSVindex(() => FakeData().RangeAccess);
            rReturn.AppendLine("if ($sCsvList[" + tChallengeIndex + "] == " + (int)NWDClusterChallenge.RangeAccess + ")");
            rReturn.AppendLine("{");
            {
                rReturn.AppendLine("if (UniquePropertyValueFromValue(GetConnexionByRangeAccess($sCsvList[" + tRangeAccess + "]), $sCsvList[" + tRangeAccess + "], '" + PHP_TABLENAME(sEnvironment) + "', '" + FakePropertyName(() => FakeData().Nickname) + "', '" + FakePropertyName(() => FakeData().UniqueNickname) + "', $tReference) == true)");
                rReturn.AppendLine("{");
                {
                    rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "NWDClusterChallenge.RangeAccess"));
                    rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + "(GetCurrentConnexion(), $tReference);");
                }
                rReturn.AppendLine("}");
            }
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            {
                rReturn.AppendLine("if (UniquePropertyValueFromGlobalValue('" + PHP_TABLENAME(sEnvironment) + "', '" + FakePropertyName(() => FakeData().Nickname) + "', '" + FakePropertyName(() => FakeData().UniqueNickname) + "', $tReference) == true)");
                rReturn.AppendLine("{");
                {
                    rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "NWDClusterChallenge.Global"));
                    rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + "(GetCurrentConnexion(), $tReference);");
                }
                rReturn.AppendLine("}");
            }
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif