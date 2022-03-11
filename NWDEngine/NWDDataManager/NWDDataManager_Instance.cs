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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Constructor of <see cref="NWDDataManager"/> instance
        /// </summary>
        private NWDDataManager()
        {
            PlayerLanguage = NWDDataLocalizationManager.SystemLanguageString();
            PlayerLanguage = NWDDataLocalizationManager.CheckLocalization(PlayerLanguage);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Destructor of <see cref="NWDDataManager"/> instance
        /// </summary>
        ~NWDDataManager()
        {
            SharedInstance().DataQueueExecute();
            NWENotificationManager.SharedInstance().RemoveAll();
            NWDLauncher.ResetLauncher();
            Debug.Log("Veuillez accepter mon décés!");
            OnBeforeAssemblyReload();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the shared instance of <see cref="NWDDataManager"/>
        /// </summary>
        /// <returns></returns>
        public static NWDDataManager SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new NWDDataManager();
            }
            return kSharedInstance;
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
