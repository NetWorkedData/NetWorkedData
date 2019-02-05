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
    public enum NWDUserNetWorkingState : int
    {
        Unknow = -1,
        OffLine = 0,
        OnLine = 1,

        NotDisturbe = 2,
        Masked = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNW")]
    [NWDClassDescriptionAttribute("User statut on Network")]
    [NWDClassMenuNameAttribute("User Net Working")]
    public partial class NWDUserNetWorking : NWDBasis<NWDUserNetWorking>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesListType<NWDAccount> Account
        {
            get; set;
        }
        public NWDDateTimeType NextUpdate
        {
            get; set;
        }
        public bool NotDisturbe
        {
            get; set;
        }
        public bool Masked
        {
            get; set;
        }
        // perhaps add some stats 
        public int TotalPlay
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================