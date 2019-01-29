//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BasicToolBox;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PRM")]
    [NWDClassDescriptionAttribute("Parameters of game. You can use this class to create Parameters of your game. \n" +
                                   "Parameters are set for all user. Use InternalKey to find them and use them. \n" +
                                   "")]
    [NWDClassMenuNameAttribute("Parameters")]
    public partial class NWDParameter : NWDBasis<NWDParameter>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Value(s)", true, true, true)]
        public NWDLocalizableType LocalizableString
        {
            get; set;
        }
        public NWDMultiType Value
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================