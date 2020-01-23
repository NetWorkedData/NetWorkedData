//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:23
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ANN")]
    [NWDClassDescriptionAttribute("Account Nickname")]
    [NWDClassMenuNameAttribute("Account Nickname")]
    [NWDClassClusterAttribute(1, 10)]
    public partial class NWDAccountNickname : NWDBasis
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