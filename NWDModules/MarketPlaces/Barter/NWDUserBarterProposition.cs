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
using SQLite4Unity3d;
using BasicToolBox;
using SQLite.Attribute;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBPR")]
    [NWDClassDescriptionAttribute("User Barter Proposition descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Proposition")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterProposition : NWDBasis<NWDUserBarterProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Barter Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        [NWDAlias("BarterPlace")]
        public NWDReferenceType<NWDBarterPlace> BarterPlace
        {
            get; set;
        }
        [NWDAlias("BarterRequest")]
        public NWDReferenceType<NWDUserBarterRequest> BarterRequest
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Barter References", true, true, true)]
        [NWDNotEditable]
        [NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        [NWDAlias("ItemsSend")]
        public NWDReferencesQuantityType<NWDItem> ItemsSend
        {
            get; set;
        }
        [NWDAlias("BarterStatus")]
        public NWDTradeStatus BarterStatus
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("BarterRequestHash")]
        public string BarterRequestHash
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================