//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:13
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWBBenchmarkResultHelper : NWDHelper<NWBBenchmarkResult>
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public override void  ErrorRegenerate()
        {
            //NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "01", "your error 01", "your description", "OK", NWDErrorType.InGame, NWDBundledBasisTag.TagInternal);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override void  ClassInitialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type>  OverrideClasseInThisSync()
        {
            return new List<Type>
            {
                typeof(NWBBenchmarkResult),
                //typeof(NWDOtehrType),
            };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================