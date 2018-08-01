//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDBasisFilter is use to filter the datas in workflow methodes.
    /// </summary>
    public enum NWDDatasFilter : int
    {
        /// <summary>
        /// All datas, quickly access.
        /// </summary>
        All,
        /// <summary>
        /// The reachable datas by the current user, quickly access.
        /// Return the editor and account dependant datas 
        /// </summary>
        Reachable,
        /// <summary>
        /// The reachable datas by the current user, slow access.
        /// Not trashed, not disabled, for current account!
        /// Return the editor and account dependant datas 
        /// </summary>
        ReachableAndEnable,
        /// <summary>
        /// The reachable datas by the current user but disabled, slow access.
        /// For current account but disabled.
        /// </summary>
        ReachableAndDisabled,
        /// <summary>
        /// The reachable datas by the current user but trashed, slow access.
        /// For current account but trashed.
        /// </summary>
        ReachableAndTrashed,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================