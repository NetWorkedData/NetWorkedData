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
#region table management
		public static void CreateTable ()
		{
			NWDDataManager.SharedInstance.CreateTable (ClassType ());
		}

        public static void ConnectToDatabase()
        {
            NWDDataManager.SharedInstance.ConnectToDatabase();
        }

#if UNITY_EDITOR
        public static void PopulateTable ()
		{
			NWDDataManager.SharedInstance.PopulateTable (ClassType ());
		}

		public static void EmptyTable ()
		{
			NWDDataManager.SharedInstance.EmptyTable (ClassType ());
		}

		public static void DropTable ()
		{
			NWDDataManager.SharedInstance.DropTable (ClassType ());
		}

		public static void ReInitializeTable ()
		{
			NWDDataManager.SharedInstance.ReInitializeTable (ClassType ());
		}

		public static void ResetTable ()
		{
			NWDDataManager.SharedInstance.ResetTable (ClassType ());
		}

		protected static string GenerateNewSalt ()
		{
			return NWDToolbox.RandomString(UnityEngine.Random.Range (12, 24));
		}
#endif
#endregion

		#region object management
		// create new object

		protected static object CreateTypeInstance<D> () where D : new()
		{
			return (object)new D ();
		}

		public static NWDBasis<K> NewInstance ()
		{
			NWDBasis<K> rReturnObject = null;
			rReturnObject = (NWDBasis<K>)Activator.CreateInstance (ClassType ());
			rReturnObject.InstanceInit ();
			rReturnObject.AC = true;
			rReturnObject.DM = NWDToolbox.Timestamp ();
			rReturnObject.DC = NWDToolbox.Timestamp ();
			rReturnObject.DS = 0;
			rReturnObject.DD = 0;
			rReturnObject.DevSync = 0;
			rReturnObject.PreprodSync = 0;
			rReturnObject.ProdSync = 0;
			rReturnObject.Reference = rReturnObject.NewReference ();
			rReturnObject.InsertMe ();
			return rReturnObject;
		}

		public static NWDBasis<K> NewInstanceWithReference (string sReference)
		{
			NWDBasis<K> rReturnObject = null;
			rReturnObject = (NWDBasis<K>)Activator.CreateInstance (ClassType ());
			rReturnObject.InstanceInit ();
			rReturnObject.AC = true;
//			rReturnObject.InGame = true;
			rReturnObject.DM = NWDToolbox.Timestamp ();
			rReturnObject.DC = NWDToolbox.Timestamp ();
			rReturnObject.DS = 0;
			rReturnObject.DD = 0;
			rReturnObject.DevSync = 0;
			rReturnObject.PreprodSync = 0;
			rReturnObject.ProdSync = 0;
			rReturnObject.Reference = sReference;
			rReturnObject.InsertMe ();
			return rReturnObject;
		}



		public static NWDBasis<K> NewInstanceFromCSV (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			NWDBasis<K> rReturnObject = null;
			rReturnObject = (NWDBasis<K>)Activator.CreateInstance (ClassType ());
			rReturnObject.InstanceInit ();
			rReturnObject.UpdateWithCSV (sEnvironment, sDataArray);
			NWDDataManager.SharedInstance.InsertObject (rReturnObject);
			return rReturnObject;
		}

		public virtual void InstanceInit ()
		{
			Type tType = ClassType ();
			foreach (var tPropertyInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tPropertyInfo.PropertyType;
				if (tTypeOfThis.IsSubclassOf (typeof(BTBDataType))) {
					var tObject = Activator.CreateInstance (tTypeOfThis);
					var tMethodInfo = tObject.GetType ().GetMethod ("SetString", BindingFlags.Public | BindingFlags.Instance);
					if (tMethodInfo != null) {
						tMethodInfo.Invoke (tObject, new object[]{ "" });
					}
					tPropertyInfo.SetValue (this, tObject, null);
				}
			}
		}
		// insert (in database)

		public static bool InsertInstance (NWDBasis<K> NWDject)
		{
			return NWDject.InsertMe ();
		}

		public bool InsertMe (bool sAutoDate = true)
		{
			bool rReturn = false;
			if (NWDBasis<K>.FindObjectInDataBaseByReference (this.Reference) == null) {
				if (sAutoDate == true) {
					this.DC = NWDToolbox.Timestamp ();
					this.DM = NWDToolbox.Timestamp ();
//					this.DS = 0;
					this.DevSync = 0;
					this.PreprodSync = 0;
					this.ProdSync = 0;
					this.AddonInsertMe ();
				}
				this.UpdateIntegrity ();
				NWDDataManager.SharedInstance.InsertObject (this);
				AddObjectInListOfEdition (this);
				rReturn = true;
			} else {
				// error this reference allready exist
			}
			return rReturn;
		}
		public virtual void AddonInsertMe ()
		{

		}

		// update (in database)

		public static void UpdateInstance (NWDBasis<K> sObject)
		{
			sObject.UpdateMe ();
		}

		public bool IsModified ()
		{
			return (this.Integrity != this.IntegrityValue ());
		}

		public bool UpdateMeIfModified ()
		{
			bool tReturn = false;
			if (this.Integrity != this.IntegrityValue ()) {
				tReturn = true;
				UpdateMe ();
			}
			return tReturn;
		}

		public void UpdateMeLater ()
		{
			this.AddonUpdateMe ();
			NWDDataManager.SharedInstance.AddObjectToUpdateQueue(this);
		}

		public void SaveModifications () {
			UpdateMe ();
		}

		public void UpdateMe (bool sAutoDate = true)
		{
			//Debug.Log ("UpdateMe  " + Reference + " with/without DM ("+sAutoDate.ToString()+") and integrity reval");
			this.AddonUpdateMe ();
			if (sAutoDate == true) {
				this.DM = NWDToolbox.Timestamp ();
			}
//			this.DS = 0;
			this.DevSync = 0;
			this.PreprodSync = 0;
			this.ProdSync = 0;
			this.UpdateIntegrity ();
			NWDDataManager.SharedInstance.UpdateObject (this);
			this.Updated();
			// I have one or more uploaded datas to synchronize;
			NWDGameDataManager.UnitySingleton ().NeedSynchronizeData ();
			//NWDDataManager.SharedInstance.NotificationCenter.PostNotification (new BTBNotification (NWDConstants.kUpdateDatasNotificationsKey, null));
		}

		public void SynchronizeMe ()
		{
			this.DM = NWDToolbox.Timestamp ();
			this.UpdateIntegrity ();
			NWDDataManager.SharedInstance.UpdateObject (this);
		}

		public virtual void AddonUpdateMe ()
		{

		}

		public virtual void Updated ()
		{

		}

		public static void ApplyAllModifications ()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute ();
			#if UNITY_EDITOR
			LoadTableEditor ();
			FilterTableEditor ();
			#endif
		}

		// dupplicate (in database)

		public static NWDBasis<K> DuplicateInstance (NWDBasis<K> NWDject)
		{
			return NWDject.DuplicateMe ();
		}

		public NWDBasis<K> DuplicateMe ()
		{
			NWDBasis<K> rReturnObject = null;
			IEnumerable<K> tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (x => x.ID == this.ID);
			int tCount = tEnumerable.Cast<K> ().Count<K> ();
			if (tCount == 1) {
				rReturnObject = tEnumerable.Cast<K> ().ElementAt (0);
				rReturnObject.Reference = NewReference ();
				rReturnObject.Integrity = rReturnObject.IntegrityValue ();
				rReturnObject.InsertMe ();
			}
			rReturnObject.AddonDuplicateMe ();
			return rReturnObject;
		}

		public virtual void AddonDuplicateMe ()
		{
			
		}

		// enable (in database)

		public static void EnableInstance (NWDBasis<K> sObject)
		{
			sObject.EnableMe ();
		}

		public void EnableMe ()
		{
			this.AC = true;
			//this.UpdateIntegrity ();
			this.AddonEnableMe ();
			this.UpdateMe ();
		}

		public virtual void AddonEnableMe ()
		{

		}

		// disable (in database)

		public static void DisableInstance (NWDBasis<K> sObject)
		{
			sObject.DisableMe ();
		}

		public void DisableMe ()
		{
			this.DD = NWDToolbox.Timestamp ();
			this.AC = false;
			//this.UpdateIntegrity ();
			this.AddonDisableMe ();
			this.UpdateMe ();
		}
		public virtual void AddonDisableMe ()
		{

		}


//		// enable in game  (in database)
//
//		public static void EnableInGameObject (NWDBasis<K> sObject)
//		{
//			sObject.EnableInGameMe ();
//		}
//
//		public void EnableInGameMe ()
//		{
//			this.InGame = true;
//			//this.UpdateIntegrity ();
//			this.AddonEnableInGameMe ();
//			this.UpdateMe ();
//		}
//
//		public virtual void AddonEnableInGameMe ()
//		{
//
//		}
//
//		// disable in game  (in database)
//
//		public static void DisableInGameObject (NWDBasis<K> sObject)
//		{
//			sObject.DisableInGameMe ();
//		}
//
//		public void DisableInGameMe ()
//		{
//			this.InGame = false;
//			//this.UpdateIntegrity ();
//			this.AddonDisableInGameMe ();
//			this.UpdateMe ();
//		}
//		public virtual void AddonDisableInGameMe ()
//		{
//
//		}
		// trash (in database)

		public static void TrashInstance (NWDBasis<K> sObject)
		{
			sObject.TrashMe ();
		}

		public void TrashMe ()
		{
			int tTimestamp = NWDToolbox.Timestamp ();
			this.DD = tTimestamp;
			this.XX = tTimestamp;
			this.AC = false;
//			this.UpdateIntegrity ();
			this.AddonTrashMe ();
			this.UpdateMe ();
		}
		public virtual void AddonTrashMe ()
		{

		}

		// untrash (in database)

		public static void UnTrashInstance (NWDBasis<K> sObject)
		{
			sObject.UnTrashMe ();
		}

		public void UnTrashMe ()
		{
			this.XX = 0;
			this.AC = false;
			this.AddonUnTrashMe ();
			this.UpdateMe ();
		}
		public virtual void AddonUnTrashMe ()
		{

		}

		public static void FlushTrash (NWDBasis<K> sObject)
		{
			//Debug.Log ("Flush trash ... the delete this object if it's necessary");
			#if UNITY_EDITOR
			if (sObject.XX > 0 && sObject.DevSync>0 && sObject.PreprodSync>0 && sObject.ProdSync>0) 
			{
				//				Debug.Log (sObject.Reference + "Must be trashed!");
				RemoveObjectInListOfEdition (sObject);
				if (IsObjectInEdition (sObject))
				{
					SetObjectInEdition (null);
				}
				sObject.DeleteMe ();
			}
			#else
			if (sObject.XX > 0) 
			{
//				Debug.Log (sObject.Reference + "Must be trashed!");
				RemoveObjectInListOfEdition (sObject);
				sObject.DeleteMe ();
			}
			#endif
		}

		// delete (in database)

		public static void DeleteInstance (NWDBasis<K> sObject)
		{
			sObject.DeleteMe ();
		}

		public void DeleteMe ()
		{
			NWDDataManager.SharedInstance.DeleteObject (this);

		}

		#endregion

		#region objects finder

		// find objects and get objects from database


		public static List<object> ObjectsList = new List<object> ();
		public static List<string> ObjectsByReferenceList = new List<string> ();
		public static List<string> ObjectsByInternalKeyList = new List<string> ();
		public static List<string> ObjectsByKeyList = new List<string> ();


		public static IEnumerable<K> GetAllBasisObjects ()
		{
			return NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ();
		}

		public static NWDBasis<K> FindObjectInDataBaseByReference (string sReference)
		{
			NWDBasis<K> rReturnObject = null;
			IEnumerable<K> tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (x => x.Reference == sReference);
			int tCount = tEnumerable.Cast<K> ().Count<K> ();
			if (tCount == 1) {
				rReturnObject = tEnumerable.Cast<K> ().ElementAt (0);
			}
			return rReturnObject;
		}




		#endregion
	}
}
//=====================================================================================================================