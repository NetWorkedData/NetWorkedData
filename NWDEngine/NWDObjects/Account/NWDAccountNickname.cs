//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ANN")]
    [NWDClassDescriptionAttribute("Account Nickname")]
    [NWDClassMenuNameAttribute("Account Nickname")]
    public partial class NWDAccountNickname : NWDBasis<NWDAccountNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDHeader("Player Informations")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public string Nickname
        {
            get; set;
        }
        [NWDNotEditable]
        public string UniqueNickname
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================