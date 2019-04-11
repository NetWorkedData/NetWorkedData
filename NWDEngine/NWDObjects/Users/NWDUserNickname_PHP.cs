//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

using BasicToolBox;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNicknameHelper : NWDHelper<NWDUserNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("if (UniquePropertyValueFromValue($ENV.'_NWDUserNickname', 'Nickname', 'UniqueNickname', $tReference) == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("IntegrityNWDUserNicknameReevalue($tReference);");
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif