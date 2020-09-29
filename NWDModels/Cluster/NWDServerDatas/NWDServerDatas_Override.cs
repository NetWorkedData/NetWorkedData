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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDatas : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            base.AddonLoadedMe();
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            base.AddonUnloadMe();
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            base.AddonInsertMe();
            CheckInformations();
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when inserted.
        /// </summary>
        public override void AddonInsertedMe()
        {
            base.AddonInsertedMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            CheckInformations();
            //MySQLUser = NWDToolbox.UnixCleaner(MySQLUser);
            //MySQLBase = NWDToolbox.UnixCleaner(MySQLBase);
            //if (Range < 1)
            //{
            //    Range = 1; // 0 is reserved by old system without cluster
            //}
            //int tNexRange = Range;
            //bool tTest = false;
            //while (tTest == false)
            //{
            //    tTest = true;
            //    foreach (NWDServerDatas tServerDatas in BasisHelper().Datas)
            //    {
            //        if (tServerDatas != this)
            //        {
            //            if (tServerDatas.Range == tNexRange)
            //            {
            //                tNexRange++;
            //                tTest = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            //Range = tNexRange;
            //if (UserMax < 1)
            //{
            //    UserMax = 1;
            //}
            //List<string> tDescription = new List<string>();
            //DevSyncActive(Dev);
            //PreprodSyncActive(Preprod);
            //ProdSyncActive(Prod);
            //if (Dev == true)
            //{
            //    tDescription.Add(NWDAppConfiguration.SharedInstance().DevEnvironment.Environment);
            //}
            //if (Preprod == true)
            //{
            //    tDescription.Add(NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment);
            //}
            //if (Prod == true)
            //{
            //    tDescription.Add(NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment);
            //}
            //InternalDescription = string.Join(" / ", tDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            base.AddonUpdatedMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        //public override void AddonUpdatedMeFromWeb()
        //{
        //   base.AddonUpdatedMeFromWeb();
        //    // do something when object finish to be updated from CSV from WebService response
        //    // TODO verif if method is call in good place in good timing
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            base.AddonDuplicateMe();
            CheckInformations();
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when dupplicate.
        /// </summary>
        public override void AddonDuplicatedMe()
        {
            base.AddonDuplicatedMe();
            CheckInformations();
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            base.AddonEnableMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            base.AddonDisableMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            base.AddonTrashMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            base.AddonUnTrashMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            base.AddonDeleteMe();
            NWDClusterAnalyzer.CheckAllCluster();
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool AddonSyncForce()
        //{
        //    base.AddonSyncForce();
        //    return false;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            base.AddonWebversionUpgradeMe(sOldWebversion, sNewWebVersion);
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMeFromWeb()
        {
            base.AddonUpdatedMeFromWeb();
            NWDClusterAnalyzer.CheckAllCluster();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
