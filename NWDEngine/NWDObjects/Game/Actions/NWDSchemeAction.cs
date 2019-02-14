//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("URI")]
    [NWDClassDescriptionAttribute("Action by Scheme URI")]
    [NWDClassMenuNameAttribute("Scheme Action")]
    public partial class NWDSchemeAction : NWDBasis<NWDSchemeAction>
    {
        //-------------------------------------------------------------------------------------------------------------
        //      [NWDTooltips("The name of message post to all observer objects. Example 'Raining', 'Start Quest Music', etc.")]
        //public string ActionName {get; set;}
        [NWDGroupStart("Optional", true, true, false)]
        [NWDTooltips("An additional message, it's optional and not used in standard process.")]
        public string Message
        {
            get; set;
        }
        [NWDTooltips("An additional param as string, it's optional and not used in standard process.")]
        public string Parameter
        {
            get; set;
        }
        [NWDTooltips("An additional Action, it's optional but used in standard process.")]
        public NWDReferenceType<NWDAction> Action
        {
            get; set;
        }
        [NWDGroupEnd]
       
        [NWDGroupStart("Scene", true, true, false)]
        [NWDTooltips("An additional scene to use, it's optional and not used in standard process.")]
        public NWDSceneType UseScene
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================