//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
        public override void New_ErrorRegenerate()
        {
            NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "01", "your error 01", "your description", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "02", "your error 02", "your description", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
            NWDMessage.CreateGenericMessage(ClassNamePHP, ClassTrigramme + "01", "your message 01", "your description", "OK", "Cancel", NWDMessageType.InGame, NWDBasisTag.TagInternal);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override void New_ClassInitialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type> New_OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDExample)/*, typeof(NWDUserNickname), etc*/ };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================