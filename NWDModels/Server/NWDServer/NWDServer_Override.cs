//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
    public partial class NWDServer : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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
            InternalKey = "Unused config";
            if (string.IsNullOrEmpty(DomainNameServer) ==false)
            {
                    InternalKey = DomainNameServer + " config";
            }
            Admin_User = NWDToolbox.UnixCleaner(Admin_User);
            if (Admin_Password != null)
            {
                Admin_Password.SetValue(NWDToolbox.UnixCleaner(Admin_Password.GetValue()));
            }
            Root_User = NWDToolbox.UnixCleaner(Root_User);
            if (Root_Password != null)
            {
                Root_Password.SetValue(NWDToolbox.UnixCleaner(Root_Password.GetValue()));
            }
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
            Port = 22;
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
            NWDClusterAnalyzer.CheckAllCluster();
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
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================