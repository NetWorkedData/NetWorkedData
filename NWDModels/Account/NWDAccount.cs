//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassSpecialAccountOnlyAttribute]
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("ACC")]
    [NWDClassDescriptionAttribute("Account descriptions Class")]
    [NWDClassMenuNameAttribute("Account")]
    [NWDClassClusterAttribute(1, 2)]
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_LOGIN_INDEX = "LoginIndex";
        const string K_SECRET_INDEX = "SecretIndex";
        const string K_SOCIAL_INDEX = "SecretIndex";
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(K_LOGIN_INDEX, "AC")]
        [NWDAddIndexed(K_SECRET_INDEX, "AC")]
        [NWDAddIndexed(K_SOCIAL_INDEX, "AC")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(NWD.K_BASIS_INDEX, "AC")]
        [NWDAddIndexed(NWD.K_BASIS_INDEX, "Reference")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Account statut")]
        [NWDTooltips("The statut of this account in process of test (normal and default are 'InGame')")]
        [NWDCertified]
        public NWDAccountEnvironment UseInEnvironment { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Account ban")]
        /// <summary>
        /// Gets or sets a value indicating whether this account <see cref="NWDEditor.NWDAccount"/> is banned.
        /// </summary>
        /// <value><c>true</c> if ban; otherwise, <c>false</c>.</value>
        [NWDTooltips("If account is banned set the unix timestamp of ban's date")]
        [NWDCertified]
        public int Ban { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
