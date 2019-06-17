//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:22
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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PCK")]
	[NWDClassDescriptionAttribute ("Pack descriptions Class")]
	[NWDClassMenuNameAttribute ("Pack")]
	public partial class NWDPack :NWDBasis <NWDPack>
	{
		//-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription { get; set; }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Item Pack in this Pack", true, true, true)]
		public NWDReferencesQuantityType<NWDItemPack> ItemPackReference { get; set; }
        public int Quantity { get; set; }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Item to Pay for this Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsToPay { get; set; }
        public NWDReferenceType<NWDInAppPack> InAppReference { get; set; }
        public bool EnableFreePack { get; set; }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds { get; set; }
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        [NWDInspectorGroupEnd]

        

        [NWDInspectorGroupStart("Availability schedule ", true, true, true)]
        [NWDTooltips("Availability schedule of this Pack")]
        public NWDDateTimeScheduleType AvailabilitySchedule { get; set; }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================