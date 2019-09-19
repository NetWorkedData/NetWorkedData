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
    public partial class NWDExampleHelper : NWDHelper<NWDExample>
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public override void  ErrorRegenerate()
        {
            //NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "01", "your error 01", "your description", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
            //NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "02", "your error 02", "your description", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
            NWDMessage.CreateGenericMessage(ClassNamePHP, ClassTrigramme + "01", "your message 01", "your description", "OK", "Cancel", NWDMessageType.InGame, NWDBasisTag.TagInternal);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override void  ClassInitialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type>  OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDExample)/*, typeof(NWDUserNickname), etc*/ };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================