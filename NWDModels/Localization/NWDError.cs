//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDErrorType : int
    {
        LogVerbose = 0, // No alert but write in log
        LogWarning = 1, // No alert but write in log in warning

        InGame = 10, // Use in game alert NWENotification Post notification

        Alert = 20, // System Dialog
        Critical = 30,  // System Dialog and Quit

        Upgrade = 40,  // Upgrade required and Quit

        UnityEditor = 98, // For Unity NWD Editor
        Ignore = 99, // Do Nothing
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDErrorBlock(NWDError sError);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute(NWDError.ErrorTrigram)]
    [NWDClassDescriptionAttribute("Error descriptions Class")]
    [NWDClassMenuNameAttribute("Errors")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDError : NWDBasis
    {
     public const string ErrorTrigram = "ERR";
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        //public bool DiscoverItYourSelf { get; set; }
        [NWDInspectorGroupStart("Informations", true, true, true)] //ok
        [NWDTooltips("Type and priority of error")]
        public NWDErrorType Type
        {
            get; set;
        }
        [NWDTooltips("Domain of error")]
        public string Domain
        {
            get; set;
        }
        [NWDTooltips("Code of error in the selected Domain")]
        public string Code
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Description", true, true, true)] // ok
        [NWDTooltips("Title of error message")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        [NWDTooltips("Content of error message")]
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        [NWDTooltips("Validation text of error message")]
        public NWDLocalizableStringType Validation
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================