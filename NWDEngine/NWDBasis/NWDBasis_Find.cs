//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        // find objects and get objects from database
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> ObjectsList = new List<object>();
        public static List<string> ObjectsByReferenceList = new List<string>();
        public static List<string> ObjectsByKeyList = new List<string>(); // TODO Rename ObjectsByInternalKeyList
                                                                          //-------------------------------------------------------------------------------------------------------------
        #region Basis finder
        //-------------------------------------------------------------------------------------------------------------
        //public static List<K> FindObjectsInDataBaseByAccount(string sAccountReference)
        //{
        //    List<K> rReturnObject = new List<K>();
        //    if (AccountDependent())
        //    {
        //        //SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //        PropertyInfo[] tAccountProperties = kAccountDependentProperties[NWDBasis<K>.ClassName()];
        //        string tWhere = "";
        //        List<string> tWhereList = new List<string>();
        //        foreach (PropertyInfo tProp in tAccountProperties)
        //        {
        //            tWhereList.Add("( `" + tProp.Name + "` LIKE \'" + sAccountReference + "\')");
        //        }
        //        tWhere += string.Join(" OR ", tWhereList.ToArray());
        //        //IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K>().Where(tWhere);
        //        //foreach (K tobject in tEnumerable)
        //        //{
        //        //    rReturnObject.Add(tobject);
        //        //}
        //    }
        //    return rReturnObject;
        //}
        //-------------------------------------------------------------------------------------------------------------
        private static NWDBasis<K> FindObjectInDataBaseByReference(string sReference)
        {

            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }

            NWDBasis<K> rReturnObject = null;
            IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K>().Where(x => x.Reference == sReference); // Normal but exceptional using of SQLITE
            int tCount = tEnumerable.Cast<K>().Count<K>();
            if (tCount == 1)
            {
                rReturnObject = tEnumerable.Cast<K>().ElementAt(0);
            }
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Reacheable
        //-------------------------------------------------------------------------------------------------------------
        //		public bool IsAccountDependent (string sAccountReference = null)
        //		{
        //			bool rReturn = false;
        //			if (sAccountReference == null || sAccountReference == "") {
        //				sAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment ().PlayerAccountReference;
        //			}
        //			if (AccountDependent ()) {
        //				foreach (PropertyInfo tProp in PropertiesAccountDependent()) {
        //					var tValue = tProp.GetValue (this, null);
        //					var tMethodInfo = tValue.GetType ().GetMethod ("ToString", BindingFlags.Public | BindingFlags.Instance);
        //					if (tMethodInfo != null) {
        //						string tValueToString = tMethodInfo.Invoke (tValue, null) as string;
        //						if (tValueToString.Contains (sAccountReference)) {
        //							rReturn = true;
        //						}
        //					}
        //				}
        //			} else {
        //			}
        //			return rReturn;
        //		}

        //-------------------------------------------------------------------------------------------------------------
        public bool IsReacheableByAccount(string sAccountReference = null)
        {
            bool rReturn = false;
            if (XX == 0)
            {
                if (AccountDependent())
                {
                    // is account dependency : get all value and test
                    if (sAccountReference == null || sAccountReference == "") // TODO : replace by  string.IsNullOrEmpty
                    {
                        sAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
                    }
                    foreach (PropertyInfo tProp in PropertiesAccountConnect())
                    {
                        // find property and method to access
                        var tValue = tProp.GetValue(this, null);
                        if (tValue != null)
                        {
                            MethodInfo tMethodInfo = tValue.GetType().GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                            if (tMethodInfo != null)
                            {
                                // get value of account reference
                                string tValueToString = tMethodInfo.Invoke(tValue, null) as string;
                                if (tValueToString.Contains(sAccountReference))
                                {
                                    // account reference in value
                                    rReturn = true;
                                    // add break !
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // non account dependency return acces is true.
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Static Get Object
        //-------------------------------------------------------------------------------------------------------------
        //TODO : RENAME with instance or basis 
        public static NWDBasis<K> InstanceByReference(string sReference)
        {
            NWDBasis<K> rObject = null;
            if (ObjectsByReferenceList.Contains(sReference))
            {
                int tObjectIndex = ObjectsByReferenceList.IndexOf(sReference);
                rObject = (NWDBasis<K>)ObjectsList.ElementAt(tObjectIndex);
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------

        //TODO : RENAME with instance or basis 
        //		public static NWDBasis<K> GetObjectByInternalKey (string sInternalKey)
        //		{
        //			NWDBasis<K> rObject = null;
        //			foreach (NWDBasis<K> tObject in ObjectsList) {
        //				if (tObject.InternalKey == sInternalKey) {
        //					if (tObject.IsReacheableByAccount (NWDAppConfiguration.SharedInstance().SelectedEnvironment ().PlayerAccountReference)) {
        //						rObject = tObject;
        //						break;
        //					}
        //				}
        //			}
        //			return rObject;
        //		}
        //TODO : RENAME with instance or basis 
        //		public static NWDBasis<K> GetObjectByInternalKeyOrCreate (string sInternalKey, string sInternalDescription = "")
        //		{
        //			NWDBasis<K> rObject = GetObjectByInternalKey (sInternalKey);
        //			if (rObject == null) {
        //				rObject = NWDBasis<K>.NewInstance ();
        //				RemoveObjectInListOfEdition (rObject);
        //				rObject.InternalKey = sInternalKey;
        //				rObject.InternalDescription = sInternalDescription;
        //				rObject.UpdateMe ();
        //				AddObjectInListOfEdition (rObject);
        //			}
        //			return rObject;
        //		}
        //TODO : RENAME with instance or basis 
        //		public static NWDBasis<K> GetObjectByReferenceOrCreate (string sReference, string sInternalKey = "", string sInternalDescription = "")
        //		{
        //			NWDBasis<K> rObject = null;
        //			if (ObjectsByReferenceList.Contains (sReference)) {
        //				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
        //				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
        //			}
        //			if (rObject == null) {
        //				rObject = NWDBasis<K>.NewInstance ();
        //				RemoveObjectInListOfEdition (rObject);
        //				rObject.Reference = sReference;
        //				rObject.InternalKey = sInternalKey;
        //				rObject.InternalDescription = sInternalDescription;
        //				rObject.UpdateMe ();
        //				AddObjectInListOfEdition (rObject);
        //			}
        //			return rObject;
        //		}

        //
        //		public static NWDBasis<K> GetObjectByReferenceLimited (string sReference)
        //		{
        //			NWDBasis<K> rObject = null;
        //			if (ObjectsByReferenceList.Contains (sReference)) {
        //				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
        //				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
        //			}
        //			return rObject;
        //		}
        //TODO : RENAME with instance or basis 
        //		public static NWDBasis<K> GetObjectInObjectsByReferenceList (string sReference)
        //		{
        //			NWDBasis<K> rObject = null;
        //			if (ObjectsByReferenceList.Contains (sReference)) {
        //				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
        //				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
        //			}
        //			return rObject;
        //		}
        //		//TODO : RENAME with instance or basis 
        //		public static NWDBasis<K> GetObjectInObjectsByKeyList (string sKey)
        //		{
        //			NWDBasis<K> rObject = null;
        //			if (ObjectsByKeyList.Contains (sKey)) {
        //				int tObjectIndex = ObjectsByKeyList.IndexOf (sKey);
        //				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
        //			}
        //			return rObject;
        //		}

        #endregion
    }
}
//=====================================================================================================================