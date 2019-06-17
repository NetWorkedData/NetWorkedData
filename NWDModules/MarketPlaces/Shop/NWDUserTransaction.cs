//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:34
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
    public enum NWDTransactionCheckStatut : int
    {
        NotInApp = -1,
        Unknow = 0,
        Approuved = 1,
        Refused = 2,

        Error = 9
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDTransactionType
    {
        None,
        Daily,
        Weekly,
        Monthly
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UTT")]
    [NWDClassDescriptionAttribute("User Transaction descriptions Class")]
    [NWDClassMenuNameAttribute("User Transaction")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserTransaction : NWDBasis<NWDUserTransaction>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }

        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items in transaction", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsReceived
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsSpent
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Place ", true, true, true)]
        public NWDReferenceType<NWDBarterPlace> BarterPlaceReference
        {
            get; set;
        }
        public NWDReferenceType<NWDTradePlace> TradePlaceReference
        {
            get; set;
        }
        public NWDReferenceType<NWDShop> ShopReference
        {
            get; set;
        }
        public NWDReferenceType<NWDRack> RackReference
        {
            get; set;
        }
        public NWDReferenceType<NWDPack> PackReference
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Other", true, true, true)]
        public string Platform
        {
            get; set;
        }
        public NWDReferenceType<NWDInAppPack> InAppReference
        {
            get; set;
        }
        public string InAppTransaction
        {
            get; set;
        }
        public NWDTransactionCheckStatut InAppApprouved
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================