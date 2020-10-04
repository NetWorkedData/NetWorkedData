//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================
#if NWD_SERVICES
//=====================================================================================================================

using System;
using UnityEngine;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountServiceHelper : NWDHelper<NWDAccountService>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAccountService class. This class is use for (complete description here).
    /// </summary>
    [NWDClassMacroAttribute("NWD_SERVICES")]
    [NWDClassTrigrammeAttribute("NWDAS")]
    [NWDClassDescriptionAttribute("Account service subscription")]
    [NWDClassMenuNameAttribute("Account service")]
    public partial class NWDAccountService : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        public NWDReferenceType<NWDServiceKey> Service {get; set;}
		public bool Active {get; set;}
		public NWDDateTimeType Start {get; set;}
		public NWDDateTimeType Finish {get; set;}
		public bool AutoRenewable {get; set;}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        // never change the constructors! they are used by the NetWorkedData Writing System
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountService()
        {
            //Debug.Log("NWDAccountService Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountService(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccountService Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif //NWD_SERVICES
//=====================================================================================================================