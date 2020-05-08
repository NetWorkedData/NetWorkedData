//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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
            rReturn.AppendLine("if ($sCsvList[" + tChallengeIndex + "] == " + (int)NWDClusterChallenge.RangeAccess + ")");
            rReturn.AppendLine("{");
            {
                rReturn.AppendLine("if (UniquePropertyValueFromValue('" + PHP_TABLENAME(sEnvironment) + "', '" + FakePropertyName(() => FakeData().Nickname) + "', '" + FakePropertyName(() => FakeData().UniqueNickname) + "', $tReference) == true)");
                rReturn.AppendLine("{");
                {
                    rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "NWDClusterChallenge.RangeAccess"));
                    rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tReference);");
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
                    rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tReference);");
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