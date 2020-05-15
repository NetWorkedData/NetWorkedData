//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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