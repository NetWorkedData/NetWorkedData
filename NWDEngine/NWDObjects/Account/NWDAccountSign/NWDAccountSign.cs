// =====================================================================================================================
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
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : BTBDataTypeEnumGeneric<NWDAccountSignType>
    {
        public static NWDAccountSignType DeviceID = Add(1, "DeviceID");
        public static NWDAccountSignType LoginPassword = Add(2, "LoginPassword");
        public static NWDAccountSignType Facebook = Add(3, "Facebook");
        public static NWDAccountSignType Google = Add(4, "Google");
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAccountSign class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SKD")]
    [NWDClassDescriptionAttribute("Account Sign to connect by hash of sign")]
    [NWDClassMenuNameAttribute("Account Sign")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDAccountSign : NWDBasis<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
		public NWDReferenceType<NWDAccount> Account {get; set;}
		public NWDAccountSignType SignType {get; set;}
		public string SignHash {get; set;}
		public string RescueHash {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================