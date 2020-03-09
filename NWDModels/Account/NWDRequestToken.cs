﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:32
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassUnityEditorOnlyAttribute]
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("RQT")]
    [NWDClassDescriptionAttribute("RequestToken descriptions Class")]
    [NWDClassMenuNameAttribute("RequestToken")]
    [NWDClassClusterAttribute(1, 6)]
    public partial class NWDRequestToken : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_TOKEN_INDEX = "TokenIndex";
        //-------------------------------------------------------------------------------------------------------------
        [NWDAddIndexed(K_TOKEN_INDEX,"AC")]
        [NWDAddIndexed(K_TOKEN_INDEX, "DM")]
        [NWDAddIndexed(K_TOKEN_INDEX, "DD")]
        //-------------------------------------------------------------------------------------------------------------
        //[Indexed(K_TOKEN_INDEX, 0)]
        [NWDCertified]
        public NWDReferenceType<NWDAccount> UUIDHash
        {
            get; set;
        }
        //[Indexed(K_TOKEN_INDEX, 1)]
        [NWDCertified]
        public string Token
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
