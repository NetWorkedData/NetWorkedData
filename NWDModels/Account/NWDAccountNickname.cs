﻿//=====================================================================================================================
//
//  ideMobi 2020©
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
    /// <summary>
    /// This class create a data for account's nickname.
    /// It generate a <see cref="UniqueNickname"/> when synchronize on cluster with the challenge <see cref="ClusterChallenge"/> and the <see cref="Nickname"/>.
    /// </summary>
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ANN")]
    [NWDClassDescriptionAttribute("This class create a data for account's nickname." +
        " It generate a UniqueNickname when synchronise on the cluster with the challenge selected.")]
    [NWDClassMenuNameAttribute("Account Nickname")]
    [NWDClassClusterAttribute(1, 10)]
    public partial class NWDAccountNickname : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Connexion with player's Account (<see cref="NWDAccount"/>)
        /// </summary>
        [NWDInspectorHeader("Player Informations")]
        [NWDCertified]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        /// <summary>
        /// The <see cref="Nickname"/> filled in the form by the player in his account
        /// </summary>
        [NWDCertified]
        public string Nickname
        {
            get; set;
        }
        /// <summary>
        /// The challenge used to fill the <see cref="UniqueNickname"/>
        /// </summary>
        public NWDClusterChallenge ClusterChallenge
        {
            get; set;
        }
        /// <summary>
        /// The final unique nickname construct by <see cref="Nickname"/> and unique random string generated by the <see cref="ClusterChallenge"/>  
        /// </summary>
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