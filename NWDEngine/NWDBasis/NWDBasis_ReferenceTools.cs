// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Changes the reference for another in all objects.
        /// </summary>
        /// <param name="sOldReference">S old reference.</param>
        /// <param name="sNewReference">S new reference.</param>
        /// 
        //[NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnotherInAllObjects)]
        //public static void ChangeReferenceForAnotherInAllObjects(string sOldReference, string sNewReference)
        //{
        //    //Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in objects of class " + ClassName ());
        //    BasisHelper().New_LoadFromDatabase();
        //    foreach (NWDBasis<K> tObject in NWDBasis<K>.BasisHelper().Datas)
        //    {
        //        tObject.ChangeReferenceForAnother(sOldReference, sNewReference);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_TryToChangeUserForAllObjects)]
        //public static void TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        //{
        //    BasisHelper().New_LoadFromDatabase();
        //    foreach (NWDBasis<K> tObject in NWDBasis<K>.BasisHelper().Datas)
        //    {
        //        tObject.ChangeUser(sOldUser, sNewUser);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// UUID transform for reference.
        /// </summary>
        /// <returns>The transform for reference.</returns>
        /// <param name="sUUID">UUID.</param>
        public string UUIDTransformForReference(string sUUID)
        {
            string tUUID = BTBConstants.K_MINUS + sUUID;
            tUUID = tUUID.Replace("ACC", string.Empty);
            tUUID = tUUID.Replace("S", string.Empty);
            tUUID = tUUID.Replace("C", string.Empty);
            //tUUID = tUUID.Replace ("T", ""); // Je ne remplace pas le T de l'accompte ... ainsi je verrai les References crée sur un compte temporaire non vérifié
            tUUID = tUUID.Replace(BTBConstants.K_MINUS, string.Empty);
            return tUUID;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New reference from UUID.
        /// </summary>
        /// <returns>The reference from UUID.</returns>
        /// <param name="sUUID">UUID.</param>
        public string NewReferenceFromUUID(string sUUID)
        {
            string rReturn = string.Empty;
            bool tValid = false;
            if (sUUID != null && sUUID != string.Empty)
            {
                sUUID = UUIDTransformForReference(sUUID) + BTBConstants.K_MINUS;
            }
            //int tTimeRef = 0;
            //int.TryParse(sUUID, out tTimeRef);
            //int tTime = NWDToolbox.Timestamp() - tTimeRef;
            int tTime = NWDToolbox.Timestamp() - 1492711200; // je compte depuis le 20 avril 2017 à 18h00:00
            while (tValid == false)
            {
                rReturn = BasisHelper().ClassTrigramme + BTBConstants.K_MINUS + sUUID + tTime.ToString() + BTBConstants.K_MINUS + UnityEngine.Random.Range(100, 999).ToString();
                tValid = TestReference(rReturn);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string NewShortReference()
        {
            string rReturn = string.Empty;
            bool tValid = false;
            while (tValid == false)
            {
                rReturn = BasisHelper().ClassTrigramme + BTBConstants.K_SHORT_REFERENCE + UnityEngine.Random.Range(1111, 9999).ToString();
                tValid = TestReference(rReturn);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //		/// <summary>
        //		/// Updates the reference without change repercsusion security.
        //		/// </summary>
        //		/// <param name="sOldUser">S old user.</param>
        //		/// <param name="sNewUser">S new user.</param>
        //		public void UpdateReference (string sOldUser, string sNewUser)
        //		{
        //			string tReference = Reference.Replace (UUIDTransformForReference (sOldUser), UUIDTransformForReference (sNewUser));
        //			bool tValid = TestReference (tReference);
        //			while (tValid == false) {
        //				tReference = tReference + UnityEngine.Random.Range (100, 999);
        //				tValid = TestReference (tReference);
        //			}
        //		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Test the reference allready exists.
        /// </summary>
        /// <returns><c>true</c>, if reference was tested, <c>false</c> otherwise.</returns>
        /// <param name="sReference">reference.</param>
        public bool TestReference(string sReference)
        {
            //SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            //if (AccountDependent())
            //{
            //    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            //}
            //bool rValid = false;
            //IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K>().Where(x => x.Reference == sReference);
            //int tCount = tEnumerable.Cast<K>().Count<K>();
            //if (tCount == 0)
            //{
            //    rValid = true;
            //}
            bool rValid = !BasisHelper().DatasByReference.ContainsKey(sReference);
            return rValid;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New reference. If account dependent this UUID of Player Account is integrate in Reference generation
        /// </summary>
        /// <returns>The reference.</returns>
        public override string NewReference()
        {
            if (AccountDependent() == true)
            {
                return NewReferenceFromUUID(NWDAccount.CurrentReference());
            }
            else
            {
                return NewReferenceFromUUID(string.Empty);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Regenerates the reference.
        /// </summary>
        public void RegenerateNewReference(bool sShort = false)
        {
#if UNITY_EDITOR
            //TODO : dupplicate if synchronized and trash old data!
            //DuplicateData();
            string tOldReference = Reference;
            string tNewReference = NewReference();
            if (sShort == true)
            {
                tNewReference = NewShortReference();
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_ChangeReferenceForAnotherInAllObjects);
                //if (tMethodInfo != null)
                //{
                //    tMethodInfo.Invoke(null, new object[] { tOldReference, tNewReference });
                //}

                BasisHelper().New_ChangeReferenceForAnotherInAllObjects(tOldReference, tNewReference);


            }
            Reference = tNewReference;
            UpdateData();
            BasisHelper().New_LoadFromDatabase();
            //BasisHelper().SortEditorTableDatas();
            BasisHelper().New_RestaureDataInEditionByReference(tNewReference);
            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());

            if (BasisHelper().ConnexionType != null)
            {
                UnityEngine.Object[] tAllObjects = Resources.FindObjectsOfTypeAll(BasisHelper().ConnexionType);
                foreach (UnityEngine.Object tObject in tAllObjects)
                {
                    Debug.Log("tObject find for connexion with " + BasisHelper().ClassNamePHP + " with path ....");
                    // TODO : find solution to change the serialization connection
                }
            }
#endif
        }
        public void RegenerateNewShortReference()
        {
#if UNITY_EDITOR
            RegenerateNewReference(true);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : rename ... yhaht change refertence in properties not the object reference!
        public override void ChangeReferenceForAnother(string sOldReference, string sNewReference)
        {
            bool rModify = false;
            Type tType = ClassType();
            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis != null)
                {
                    //if (tTypeOfThis.IsGenericType)
                    {

                        if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                        {
                            NWDReferenceSimple tTestChange = tProp.GetValue(this, null) as NWDReferenceSimple;
                            tTestChange.ChangeReferenceForAnother(sOldReference, sNewReference);
                            tProp.SetValue(this, tTestChange, null);
                        }

                        if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                        {
                            NWDReferenceMultiple tTestChange = tProp.GetValue(this, null) as NWDReferenceMultiple;
                            tTestChange.ChangeReferenceForAnother(sOldReference, sNewReference);
                            tProp.SetValue(this, tTestChange, null);
                        }

                        //if (

                        //    tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)) ||
                        //    tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple))

                        //    //tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>) ||
                        //    //tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>) ||
                        //    //tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>) ||
                        //    //tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>)
                        //    )
                        //{
                        //    // TODO : Change to remove invoke!
                        //    //							Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in Property " + tProp.Name);

                        //    MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tTypeOfThis, NWDConstants.M_ChangeReferenceForAnother);
                        //    //var tMethodInfo = tTypeOfThis.GetMethod("ChangeReferenceForAnother", BindingFlags.Public | BindingFlags.Instance);
                        //    if (tMethodInfo != null)
                        //    {
                        //        var tNext = tProp.GetValue(this, null);
                        //        if (tNext == null)
                        //        {
                        //            tNext = Activator.CreateInstance(tTypeOfThis);
                        //        }
                        //        //								Debug.LogVerbose ("tNext preview = " + tNext);
                        //        string tChanged = tMethodInfo.Invoke(tNext, new object[] { sOldReference, sNewReference }) as string;
                        //        if (tChanged == "YES")
                        //        {
                        //            //									Debug.LogVerbose ("tNext changed = " + tNext);
                        //            tProp.SetValue(this, tNext, null);
                        //            rModify = true;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            if (rModify == true)
            {
                //				Debug.LogVerbose ("I WAS UPDATED");
                UpdateData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeUser(string sOldUser, string sNewUser)
        {
            if (TestIntegrity() == true)
            {
                if (AccountDependent() == true)
                {
                    //Debug.Log("##### NEED CHANGE THE ACCOUNT "+Reference + " Old integrity = "+ Integrity);
                    foreach (PropertyInfo tProp in PropertiesAccountConnected())
                    {
                        Type tTypeOfThis = tProp.PropertyType;
                        if (tTypeOfThis != null)
                        {
                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
                            {
                                NWDReferenceSimple tTestChange = tProp.GetValue(this, null) as NWDReferenceSimple;
                                tTestChange.ChangeReferenceForAnother(sOldUser, sNewUser);
                                tProp.SetValue(this, tTestChange, null);
                            }

                            if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                            {
                                NWDReferenceMultiple tTestChange = tProp.GetValue(this, null) as NWDReferenceMultiple;
                                tTestChange.ChangeReferenceForAnother(sOldUser, sNewUser);
                                tProp.SetValue(this, tTestChange, null);
                            }

                            //NWDReferenceType<NWDAccount> tObject = tProp.GetValue(this, null) as NWDReferenceType<NWDAccount>;
                            //if (tObject != null)
                            //{
                            //    tObject.ChangeReferenceForAnother(sOldUser, sNewUser);
                            //}
                        }
                    }
                    UpdateDataIfModified();
                    //Debug.Log("##### NEED CHANGE THE ACCOUNT " + Reference + " Newintegrity = " + Integrity);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================