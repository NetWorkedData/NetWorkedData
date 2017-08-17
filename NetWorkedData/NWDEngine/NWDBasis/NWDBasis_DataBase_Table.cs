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
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Class Methods

		//-------------------------------------------------------------------------------------------------------------
		public static void CopyTable (SQLiteConnection BundleSQLiteConnection)
		{

			#if UNITY_EDITOR
			 // nothing do to ... update bundle is not possible
			#else
			if (AccountDependent () == false) {
				// reset sync timestamp
				SynchronizationResetTimestamp (NWDAppConfiguration.SharedInstance.DevEnvironment);
				SynchronizationResetTimestamp (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
				SynchronizationResetTimestamp (NWDAppConfiguration.SharedInstance.ProdEnvironment);
				// flush object in memory and drop table of document 
				ResetTable ();
				// load data from BundleSQLiteConnection
				IEnumerable tEnumerable = BundleSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
				if (tEnumerable != null) {
					foreach (NWDBasis<K> tItem in tEnumerable) {
						AddObjectInListOfEdition (tItem);
						NWDDataManager.SharedInstance.InsertObject (tItem);
					}
				}

			}
			#endif
		}

		//-------------------------------------------------------------------------------------------------------------
		public static void CreateTable ()
		{
			NWDDataManager.SharedInstance.CreateTable (ClassType ());
		}
		//-------------------------------------------------------------------------------------------------------------
        public static void ConnectToDatabase()
        {
            NWDDataManager.SharedInstance.ConnectToDatabase();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void ResetTable ()
		{
			NWDDataManager.SharedInstance.ResetTable (ClassType ());

			#if UNITY_EDITOR

			#else
			ObjectsList = new List<object> ();
			ObjectsByReferenceList = new List<string> ();
			ObjectsByKeyList = new List<string> ();
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
        public static void PopulateTable ()
		{
			NWDDataManager.SharedInstance.PopulateTable (ClassType ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void EmptyTable ()
		{
			NWDDataManager.SharedInstance.EmptyTable (ClassType ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void DropTable ()
		{
			NWDDataManager.SharedInstance.DropTable (ClassType ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void ReInitializeTable ()
		{
			NWDDataManager.SharedInstance.ReInitializeTable (ClassType ());
		}
		//-------------------------------------------------------------------------------------------------------------
		protected static string GenerateNewSalt ()
		{
			return NWDToolbox.RandomString(UnityEngine.Random.Range (12, 24));
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================