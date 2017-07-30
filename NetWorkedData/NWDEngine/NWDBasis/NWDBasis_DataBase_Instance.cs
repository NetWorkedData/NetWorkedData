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
		//		protected static object CreateTypeInstance<D> () where D : new()
		//		{
		//			return (object)new D ();
		//		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static NWDBasis<K> NewInstance ()
		{
			return NewInstanceWithReference (null);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New instance with reference.
		/// </summary>
		/// <returns>The instance with reference.</returns>
		/// <param name="sReference">S reference.</param>
		public static NWDBasis<K> NewInstanceWithReference (string sReference)
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
			if (sReference == null || sReference == "") {
				rReturnObject.Reference = rReturnObject.NewReference ();
			} else {
				rReturnObject.Reference = sReference;
			}
			foreach (PropertyInfo tPropInfo in PropertiesAccountDependent()) {
				Debug.Log ("try to insert automatically the account reference in the NWDAccount connexion property : " + tPropInfo.Name);
				NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
				tAtt.Value=NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference;
				tPropInfo.SetValue (rReturnObject, tAtt, null);
			}
			rReturnObject.InsertMe ();
			return rReturnObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New instance from CSV.
		/// </summary>
		/// <returns>The instance from CS.</returns>
		/// <param name="sEnvironment">S environment.</param>
		/// <param name="sDataArray">S data array.</param>
		public static NWDBasis<K> NewInstanceFromCSV (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			NWDBasis<K> rReturnObject = null;
			rReturnObject = (NWDBasis<K>)Activator.CreateInstance (ClassType ());
			rReturnObject.InstanceInit ();
			rReturnObject.UpdateWithCSV (sEnvironment, sDataArray);
			NWDDataManager.SharedInstance.InsertObject (rReturnObject);
			return rReturnObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the instance in database.
		/// </summary>
		/// <returns><c>true</c>, if instance was inserted, <c>false</c> otherwise.</returns>
		/// <param name="NWDject">NW dject.</param>
		public static bool InsertInstance (NWDBasis<K> NWDject)
		{
			return NWDject.InsertMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the instance in database.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void UpdateInstance (NWDBasis<K> sObject)
		{
			sObject.UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		// TODO : dupplicate with ApplyAllModifications () : rename or remove ?!
		/// <summary>
		/// Execute updates queue.
		/// </summary>
		public static void UpdateQueueExecute ()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Applies all modifications which waiting in the updateQueue.
		/// </summary>
		public static void ApplyAllModifications ()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute ();
			#if UNITY_EDITOR
			LoadTableEditor ();
			FilterTableEditor ();
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Duplicates the instance. This dupplication create a copy of the instance but generate a new reference and integrity
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="NWDject">NW dject.</param>
		public static NWDBasis<K> DuplicateInstance (NWDBasis<K> NWDject)
		{
			return NWDject.DuplicateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Enables the instance.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void EnableInstance (NWDBasis<K> sObject)
		{
			sObject.EnableMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Disables the instance.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void DisableInstance (NWDBasis<K> sObject)
		{
			sObject.DisableMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Flushs the trash.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void FlushTrash (NWDBasis<K> sObject)
		{
			//Debug.Log ("Flush trash ... the delete this object if it's necessary");
			#if UNITY_EDITOR
			if (sObject.XX > 0 && sObject.DevSync > 0 && sObject.PreprodSync > 0 && sObject.ProdSync > 0) {
				//				Debug.Log (sObject.Reference + "Must be trashed!");
				RemoveObjectInListOfEdition (sObject);
				if (IsObjectInEdition (sObject)) {
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
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Trashs the instance.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void TrashInstance (NWDBasis<K> sObject)
		{
			sObject.TrashMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Untrash the instance.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void UnTrashInstance (NWDBasis<K> sObject)
		{
			sObject.UnTrashMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the instance.
		/// </summary>
		/// <param name="sObject">S object.</param>
		public static void DeleteInstance (NWDBasis<K> sObject)
		{
			sObject.DeleteMe ();
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance Methods

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Insert this instance in the database.
		/// </summary>
		/// <returns><c>true</c>, if me was inserted, <c>false</c> otherwise.</returns>
		/// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
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
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Determine whether this instance is modified.
		/// </summary>
		/// <returns><c>true</c> if this instance is modified; otherwise, <c>false</c>.</returns>
		public bool IsModified ()
		{
			return (this.Integrity != this.IntegrityValue ());
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Save the modification of instance. It's a alias of "UpdateMe (true)" method.
		/// More understanding method for the developer.
		/// </summary>
		public void SaveModifications ()
		{
			UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Update this instance. Change a lot of states of instance and write in database. 
		/// Object is not synchronized, integrity changed, ...
		/// </summary>
		/// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
		public void UpdateMe (bool sAutoDate = true)
		{
			//Debug.Log ("UpdateMe  " + Reference + " with/without DM ("+sAutoDate.ToString()+") and integrity reval");
			this.AddonUpdateMe (); // call override method
			// so object is prepared to be update
			if (sAutoDate == true) {
				this.DM = NWDToolbox.Timestamp ();
			}
//			this.DS = 0;
			this.DevSync = 0;
			this.PreprodSync = 0;
			this.ProdSync = 0;
			this.UpdateIntegrity ();
			NWDDataManager.SharedInstance.UpdateObject (this);
			// object was updated
			this.AddonUpdatedMe (); // call override method
			// I have one or more uploaded datas to synchronize;
			NWDGameDataManager.UnitySingleton ().NeedSynchronizeData ();
			//NWDDataManager.SharedInstance.NotificationCenter.PostNotification (new BTBNotification (NWDConstants.kUpdateDatasNotificationsKey, null));
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Update this instance only if modified. Test the modified state by integrity hash compare.
		/// </summary>
		/// <returns><c>true</c>, if me if modified was updated, <c>false</c> otherwise.</returns>
		public bool UpdateMeIfModified ()
		{
			bool tReturn = false;
			if (this.Integrity != this.IntegrityValue ()) {
				tReturn = true;
				UpdateMe ();
			}
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Update this instance later. Reccord this instance in a operation queue. Operation queue must call method to execute update.
		/// </summary>
		public void UpdateMeLater ()
		{
			NWDDataManager.SharedInstance.AddObjectToUpdateQueue (this);
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Update this instance later only if modified. Test the modified state by integrity hash compare.
		/// Reccord this instance in a operation queue. Operation queue must call method to execute update.
		/// </summary>
		public bool UpdateMeLaterIfModified ()
		{
			bool tReturn = false;
			if (this.Integrity != this.IntegrityValue ()) {
				tReturn = true;
				NWDDataManager.SharedInstance.AddObjectToUpdateQueue (this);
			}
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		//		public void SynchronizeMe ()
		//		{
		//			this.DM = NWDToolbox.Timestamp ();
		//			this.UpdateIntegrity ();
		//			NWDDataManager.SharedInstance.UpdateObject (this);
		//		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Duplicate this instance. This dupplication create a copy of this instance but generate a new reference and integrity
		/// </summary>
		/// <returns>The me.</returns>
		public NWDBasis<K> DuplicateMe ()
		{
			// Must reccord object in data base 
			UpdateMe ();
			// prepare a request from database to create a new object from original object 
			//(so , don't need to copy actual object if updated)
			NWDBasis<K> rReturnObject = null;
			IEnumerable<K> tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (x => x.ID == this.ID);
			int tCount = tEnumerable.Cast<K> ().Count<K> ();
			if (tCount == 1) {
				rReturnObject = tEnumerable.Cast<K> ().ElementAt (0);
				rReturnObject.Reference = NewReference ();
				rReturnObject.Integrity = rReturnObject.IntegrityValue ();
				rReturnObject.InsertMe ();
			}
			rReturnObject.AddonDuplicateMe (); // call override method
			return rReturnObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Enable this instance.
		/// </summary>
		public void EnableMe ()
		{
			this.AC = true;
			//this.UpdateIntegrity ();
			this.AddonEnableMe (); // call override method
			this.UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Disable this instance.
		/// </summary>
		public void DisableMe ()
		{
			this.DD = NWDToolbox.Timestamp ();
			this.AC = false;
			//this.UpdateIntegrity ();
			this.AddonDisableMe (); // call override method
			this.UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Trash this instance. It's not a real delete operation because the server must be informated about the "deleted" 
		/// state of this object.
		/// </summary>
		public void TrashMe ()
		{
			int tTimestamp = NWDToolbox.Timestamp ();
			this.DD = tTimestamp;
			this.XX = tTimestamp;
			this.AC = false;
//			this.UpdateIntegrity ();
			this.AddonTrashMe (); // call override method
			this.UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Remove from trash this instance.
		/// </summary>
		public void UnTrashMe ()
		{
			this.XX = 0;
			this.AC = false;
			this.AddonUnTrashMe (); // call override method
			this.UpdateMe ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Delete this instance. It's the real delete operation, server was not informated about the deleted state
		/// of this object and the object on server stay actived.
		/// </summary>
		public void DeleteMe ()
		{
			NWDDataManager.SharedInstance.DeleteObject (this);

		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Virtual Instance Methods

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Init the instance if it's necessary (not used by default).
		/// </summary>
		//TODO: must be rename and is not used  ... 
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

		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================