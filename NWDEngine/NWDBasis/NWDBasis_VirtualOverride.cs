//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Virtual Instance Methods
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just after loaded from database.
		/// </summary>
		public virtual void AddonLoadedMe ()
		{
			// do something when object was loaded
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before unload from memory.
		/// </summary>
		public virtual void AddonUnloadMe ()
		{
			// do something when object will be unload
			// TODO verif if method is call in good place in good timing
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when insert me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonInsertMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when update me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonUpdateMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonUpdatedMe ()
		{

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me. Can be ovverride in herited Class.
        /// </summary>
        public virtual void AddonVersionMe()
        {

        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated me from Web. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonUpdatedMeFromWeb ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when duplicate me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonDuplicateMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when enable me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonEnableMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when disable me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonDisableMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when trash me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonTrashMe ()
		{

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when untrahs me. Can be ovverride in herited Class.
		/// </summary>
		public virtual void AddonUnTrashMe ()
		{

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when untrahs me. Can be ovverride in herited Class.
        /// </summary>
        public virtual void AddonDeleteMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddonIndexMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddonDesindexMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool AddonSyncForce()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        //{
        //    return "\n";
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static string AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        //{
        //    return "\n";
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static string AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
        //{
        //    return "\n";
        //}
        //-------------------------------------------------------------------------------------------------------------
        public virtual void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================