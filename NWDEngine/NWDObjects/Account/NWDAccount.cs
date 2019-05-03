// =====================================================================================================================
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
// =====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("ACC")]
    [NWDClassDescriptionAttribute("Account descriptions Class")]
    [NWDClassMenuNameAttribute("Account")]
    public partial class NWDAccount : NWDBasis<NWDAccount>
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
        [NWDAddIndexed(K_BASIS_INDEX, "AC")]
        [NWDAddIndexed(K_BASIS_INDEX, "Reference")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Account statut")]
        [NWDTooltips("The statut of this account in process of test (normal and default are 'InGame')")]
        [NWDCertified]
        public NWDAccountEnvironment UseInEnvironment
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       
        //[NWDInspectorGroupStart("Account sign-in/up")]
        ///// <summary>
        ///// Gets or sets the SecretKey to restaure anonymous account.
        ///// </summary>
        ///// <value>The login.</value>
        //[NWDTooltips("The secret key to re-authentify the anonyme account")]
        //[NWDInDevelopment("Need to be test in production")]
        //[Indexed(K_SECRET_INDEX, 1)]
        //public string SecretKey
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// Gets or sets the email.
        ///// </summary>
        ///// <value>The login is an email.</value>
        //[NWDTooltips("Hash of email for the appropriate environment")]
        //[NWDCertified]
        //[Indexed(K_LOGIN_INDEX, 1)]
        //public string Email
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// Gets or sets the password.
        ///// </summary>
        ///// <value>The password.</value>
        //[NWDTooltips("Hash of password for the appropriate environment")]
        //[NWDCertified]
        //[Indexed(K_LOGIN_INDEX, 1)]
        //public string Password
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// Gets or sets the Facebook Identifiant.
        ///// </summary>
        ///// <value>The facebook I.</value>
        //[NWDTooltips("FacebookID")]
        //[Indexed(K_SOCIAL_INDEX, 1)]
        //public string FacebookID
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// Gets or sets the Google Identifiant.
        ///// </summary>
        ///// <value>The google I.</value>
        //[NWDTooltips("GoogleID")]
        //[Indexed(K_SOCIAL_INDEX, 1)]
        //public string GoogleID
        //{
        //    get; set;
        //}
        //[NWDInspectorGroupEnd]
       
        //[NWDInspectorGroupStart("Account push notification")]
        ///// <summary>
        ///// Gets or sets the apple notification token for message.
        ///// </summary>
        ///// <value>The apple notification token.</value>
        //[Obsolete("See NWDAccountInfos")]
        //public string AppleNotificationToken
        //{
        //    get; set;
        //}

        ///// <summary>
        ///// Gets or sets the google notification token for message.
        ///// </summary>
        ///// <value>The google notification token.</value>
        //[Obsolete("See NWDAccountInfos")]
        //public string GoogleNotificationToken
        //{
        //    get; set;
        //}
        //[NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Account ban")]
        /// <summary>
        /// Gets or sets a value indicating whether this account <see cref="NWDEditor.NWDAccount"/> is banned.
        /// </summary>
        /// <value><c>true</c> if ban; otherwise, <c>false</c>.</value>
        [NWDTooltips("If account is banned set the unix timestamp of ban's date")]
        [NWDCertified]
        public int Ban
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
