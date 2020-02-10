//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:28
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNicknameHelper : NWDHelper<NWDUserNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("if (UniquePropertyValueFromValue('"+PHP_TABLENAME(sEnvironment)+"', '"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDAccountNickname>().Nickname)+"', '"+NWDToolbox.PropertyName(()=>NWDBasisHelper.FictiveData<NWDAccountNickname>().UniqueNickname)+"', $tReference) == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE()+"($tReference);");
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif