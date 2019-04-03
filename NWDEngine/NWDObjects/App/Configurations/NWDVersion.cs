//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDRecommendationType define the style of a recommendation in sharing app by user message.
    /// </summary>
    public enum NWDRecommendationType : int
    {
        SMS = 0,
        Email = 1,
        EmailHTML = 2,
        //Facebook = 3,
        //Twitter = 4,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("VRS")]
    [NWDClassDescriptionAttribute("Version of game with limit and block for obsolete version. \n" +
                                  "Integrate the links to store to download this version or new version. \n" +
                                  "SMS and Email message to recommend thsi version. \n" +
                                  "Manage the Dev/Preprod/Prod environement by version. \n" +
                                  "Auto change the build version of unity project. \n")]
    [NWDClassMenuNameAttribute("Version")]
    [NWDInternalKeyNotEditableAttribute]
    [NWDNotVersionnableAttribute]
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_VERSION_INDEX = "VersionIndex";
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(K_VERSION_INDEX, "AC")]
        [NWDAddIndexed(K_VERSION_INDEX, "XX")]
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Information", true, true, true)]
        [NWDTooltips("Version reccord in database. The format is X.XX.XX")]
        [Indexed(K_VERSION_INDEX, 0)]
        public NWDVersionType Version
        {
            get; set;
        }
        public NWDColorType Cartridge
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build")]
        [Indexed(K_VERSION_INDEX, 0)]
        public bool Buildable
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build a production build (not used)")]
        public bool Editable
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Environment", true, true, true)]
        [NWDTooltips("This version can be used to build dev environement")]
        [Indexed(K_VERSION_INDEX, 0)]
        public bool ActiveDev
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build preprod environement")]
        [Indexed(K_VERSION_INDEX, 0)]
        public bool ActivePreprod
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build prod environement")]
        [Indexed(K_VERSION_INDEX, 0)]
        public bool ActiveProd
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Options", true, true, true)]
        [NWDTooltips("This version block data push")]
        public bool BlockDataUpdate
        {
            get; set;
        }
        [NWDTooltips("This version block app and show Alert")]
        public bool BlockApplication
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        //[NWDInspectorGroupStart("Alert depriciated", true, true, true)]
        //[NWDTooltips("Alert App is depriciated Title")]
        //public NWDLocalizableStringType AlertTitle
        //{
        //    get; set;
        //}
        //[NWDTooltips("Alert App is depriciated Message")]
        //public NWDLocalizableStringType AlertMessage
        //{
        //    get; set;
        //}
        //[NWDTooltips("Alert App is depriciated button text validation")]
        //public NWDLocalizableStringType AlertValidation
        //{
        //    get; set;
        //}
        //[NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Links", true, true, true)]

        [NWDTooltips("Recommendation Subject")]
        public NWDLocalizableTextType RecommendationSubject
        {
            get; set;
        }
        [NWDTooltips("Recommendation before links")]
        public NWDLocalizableStringType Recommendation
        {
            get; set;
        }
        [NWDTooltips("URL to download App in MacOS AppStore")]
        public string OSXStoreURL
        {
            get; set;
        }
        [NWDTooltips("URL to download App in iOS AppStore")]
        public string IOSStoreURL
        {
            get; set;
        }
        [NWDTooltips("URL to download App in Google Play Store")]
        public string GooglePlayURL
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Links by 'Flash By App' module ", true, true, true)]
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in MacOS AppStore")]
        public string OSXStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in iOS AppStore")]
        public string IOSStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in iOS AppStore for IPad")]
        public string IPadStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in Google Play Store")]
        public string GooglePlayID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Google Tablet Play Store")]
        public string GooglePlayTabID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Windows Phone Store")]
        public string WindowsPhoneID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Windows Store")]
        public string WindowsStoreID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Steam Store")]
        public string SteamStoreID
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================