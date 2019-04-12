// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:51:17
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    [NWDClassTrigrammeAttribute("AST")]
    [NWDClassDescriptionAttribute("Account Stat")]
    [NWDClassMenuNameAttribute("Account Stat")]
    public partial class NWDAccountStatistic : NWDBasis<NWDAccountStatistic>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDStatisticKey> StatKey
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]

        [NWDInspectorGroupStart("Values")]
        public double Total
        {
            get; set;
        }
        public double Counter
        {
            get; set;
        }
        public double Average
        {
            get; set;
        }
        public double AverageWithParent
        {
            get; set;
        }
        public double Last
        {
            get; set;
        }
        public double Max
        {
            get; set;
        }
        public double Min
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================