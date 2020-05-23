//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================



//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("RQT")]
    [NWDClassDescriptionAttribute("RequestToken descriptions Class")]
    [NWDClassMenuNameAttribute("RequestToken")]
    [NWDClassClusterAttribute(1, 6)]
    public partial class NWDRequestToken : NWDBasisAccountRestricted
    {
        const string K_TOKEN_DELETE_INDEX = "K_TOKEN_DELETE_INDEX";
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexedAttribut(K_TOKEN_DELETE_INDEX)]
        [NWDAddIndexed(K_TOKEN_DELETE_INDEX, "DM")]
        [NWDAddIndexed(NWD.K_REQUEST_TOKEN_INDEX, "AC")]
        [NWDIndexedAttribut(NWD.K_REQUEST_TOKEN_INDEX)]
        [NWDCertified]
        public string UUIDHash
        {
            get; set;
        }
        [NWDIndexedAttribut(K_TOKEN_DELETE_INDEX)]
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
