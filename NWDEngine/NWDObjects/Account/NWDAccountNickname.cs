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
        [NWDInspectorHeader("Player Informations")]
        [NWDCertified]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        [NWDCertified]
        public string Nickname
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [NWDTooltips("Unique Nickname is determine by the server. It's guarantied as unique!")]
        public string UniqueNickname
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================