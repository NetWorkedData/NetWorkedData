//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDIPBan class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IPB")]
    [NWDClassDescriptionAttribute("IP banned")]
    [NWDClassMenuNameAttribute("IP Ban")]
    [NWDClassClusterAttribute(1, 2048)]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDIPBan : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        //PROPERTIES
		public NWDIPType IP {get; set;}
		public NWDDateTimeUtcType Deadline {get; set;}
		public int Counter {get; set;}
        //[NWDNotEditable]
		public int CounterMaximum {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================