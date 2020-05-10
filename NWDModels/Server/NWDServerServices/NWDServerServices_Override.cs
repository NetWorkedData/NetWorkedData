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
    public partial class NWDServerServices : NWDBasisUnsynchronize
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
            NWDCluster.CheckAllCluster();
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
            NWDServerDomain tServerDNS = ServerDomain.GetRawData();
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                {
                    InternalKey = tServerDNS.InternalKey + " config";
                }
            }
            Folder = NWDToolbox.UnixCleaner(Folder);
            if (string.IsNullOrEmpty(Folder))
            {
                Folder = K_Public_Webservices;
            }

            if (string.IsNullOrEmpty(Email))
            {
                Email = K_Email;
            }

            User = NWDToolbox.UnixCleaner(User);
            if (string.IsNullOrEmpty(User))
            {
                User = "wsuser" + NWDToolbox.RandomStringAlpha(3).ToLower();
            }

            if (Password != null)
            {
                Password.SetValue(NWDToolbox.UnixCleaner(Password.GetValue()));
            }

            //User = NWDToolbox.UnixCleaner(User);
            //if (Password != null)
            //{
            //    Password.SetValue(NWDToolbox.UnixCleaner(Password.GetValue()));
            //}
            //Admin_User = NWDToolbox.UnixCleaner(Admin_User);
            //if (Admin_Password != null)
            //{
            //    Admin_Password.SetValue(NWDToolbox.UnixCleaner(Admin_Password.GetValue()));
            //}

            //Root_User = NWDToolbox.UnixCleaner(Root_User);
            //if (Root_Password != null)
            //{
            //    Root_Password.SetValue(NWDToolbox.UnixCleaner(Root_Password.GetValue()));
            //}
            //#if UNITY_EDITOR
            // do something when object will be updated
            List<string> tDescription = new List<string>();
            DevSyncActive(Dev);
            PreprodSyncActive(Preprod);
            ProdSyncActive(Prod);
            if (Dev == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().DevEnvironment.Environment);
            }
            if (Preprod == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment);
            }
            if (Prod == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment);
            }
            InternalDescription = string.Join(" / ", tDescription);
            //#endif
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            base.AddonUpdatedMe();
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
            NWDCluster.CheckAllCluster();
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================