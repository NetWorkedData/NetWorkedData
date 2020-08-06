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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
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
        //    foreach (NWDBasis tObject in NWDBasisHelper.BasisHelper<K>().Datas)
        //    {
        //        tObject.ChangeReferenceForAnother(sOldReference, sNewReference);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_TryToChangeUserForAllObjects)]
        //public static void TryToChangeUserForAllObjects(string sOldUser, string sNewUser)
        //{
        //    BasisHelper().New_LoadFromDatabase();
        //    foreach (NWDBasis tObject in NWDBasisHelper.BasisHelper<K>().Datas)
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
            string tUUID = NWEConstants.K_MINUS + sUUID;
            tUUID = tUUID.Replace(NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM, string.Empty);
            //tUUID = tUUID.Replace(NWDAccount.K_ACCOUNT_FROM_EDITOR, string.Empty); // Je ne remplace pas le E de l'accompte ... ainsi je verrai les References crée sur un compte Editor non vérifié
            //tUUID = tUUID.Replace(NWDAccount.K_ACCOUNT_NEW_SUFFIXE, string.Empty); // Je ne remplace pas le Z de l'accompte ... ainsi je verrai les References crée sur un compte obligatoirement new non vérifié
            tUUID = tUUID.Replace(NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE, string.Empty);
            tUUID = tUUID.Replace(NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE, string.Empty);
            tUUID = tUUID.Replace(NWEConstants.K_MINUS, string.Empty);
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
                sUUID = UUIDTransformForReference(sUUID) + NWEConstants.K_MINUS;
            }
            int tTime = NWDToolbox.Timestamp() - 1492711200; // je compte depuis le 20 avril 2017 à 18h00:00
            while (tValid == false)
            {
                rReturn = /*BasisHelper().ClassTrigramme + NWEConstants.K_MINUS +*/ sUUID + tTime.ToString() + NWEConstants.K_MINUS + UnityEngine.Random.Range(100, 999).ToString();
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
                //rReturn = BasisHelper().ClassTrigramme + NWEConstants.K_SHORT_REFERENCE + UnityEngine.Random.Range(1111, 9999).ToString();

                int tTime = NWDToolbox.Timestamp() - 1588636800; // je compte depuis le 5 mai 2020 à 00h00:00
                rReturn = /*BasisHelper().ClassTrigramme + NWEConstants.K_MINUS +*/ tTime.ToString() + NWEConstants.K_MINUS + UnityEngine.Random.Range(111111, 999999).ToString();
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
        /// Test the reference already exists.
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
            //if (AccountDependent() == true)
            if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            {
                return NewReferenceFromUUID(NWDAccount.CurrentReference());
            }
            else
            {
                //return NewReferenceFromUUID(string.Empty);
                return NewShortReference();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Regenerates the reference.
        /// </summary>
        //        public void RegenerateNewReference(bool sShort = false)
        //        {
        //#if UNITY_EDITOR
        //            //TODO : dupplicate if synchronized and trash old data!
        //            //DuplicateData();
        //            string tOldReference = Reference;
        //            string tNewReference = NewReference();
        //            if (sShort == true)
        //            {
        //                tNewReference = NewShortReference();
        //            }
        //            NWDDataManager.SharedInstance().DataQueueExecute();
        //            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
        //            {
        //                NWDBasisHelper basisHelper = NWDBasisHelper.FindTypeInfos(tType);
        //                basisHelper.LoadFromDatabase(string.Empty, true);
        //                basisHelper.ChangeReferenceForAnotherInAllObjects(tOldReference, tNewReference);
        //            }
        //            Reference = tNewReference;
        //            UpdateData();
        //            BasisHelper().LoadFromDatabase(string.Empty, true);
        //            //BasisHelper().SortEditorTableDatas();
        //            BasisHelper().RestaureDataInEditionByReference(tNewReference);
        //            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());

        //            if (BasisHelper().ConnexionType != null)
        //            {
        //                UnityEngine.Object[] tAllObjects = Resources.FindObjectsOfTypeAll(BasisHelper().ConnexionType);
        //                foreach (UnityEngine.Object tObject in tAllObjects)
        //                {
        //                    Debug.Log("tObject find for connexion with " + BasisHelper().ClassNamePHP + " with path ....");
        //                    // TODO : find solution to change the serialization connection
        //                }
        //            }
        //#endif
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public void RegenerateNewShortReference()
        //        {
        //#if UNITY_EDITOR
        //            RegenerateNewReference(true);
        //#endif
        //        }
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
                            if (tTestChange != null)
                            {
                                tTestChange.ChangeReferenceForAnother(sOldReference, sNewReference);
                                tProp.SetValue(this, tTestChange, null);
                            }
                        }

                        if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                        {
                            NWDReferenceMultiple tTestChange = tProp.GetValue(this, null) as NWDReferenceMultiple;
                            if (tTestChange != null)
                            {
                                tTestChange.ChangeReferenceForAnother(sOldReference, sNewReference);
                                tProp.SetValue(this, tTestChange, null);
                            }
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
        //public override void ChangeUser(string sOldUser, string sNewUser)
        //{
        //    //Debug.Log("##### ChangeUser" + Reference + " sOldUser " + sOldUser + " sNewUser " + sNewUser);
        //    if (IntegrityIsValid() == true)
        //    {
        //        if (AccountDependent() == true)
        //        {
        //            //Debug.Log("##### NEED CHANGE THE ACCOUNT "+Reference + " Old integrity = "+ Integrity);
        //            foreach (PropertyInfo tProp in BasisHelper().kAccountConnectedProperties)
        //            {
        //                Type tTypeOfThis = tProp.PropertyType;
        //                if (tTypeOfThis != null)
        //                {
        //                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceSimple)))
        //                    {
        //                        NWDReferenceSimple tTestChange = tProp.GetValue(this, null) as NWDReferenceSimple;
        //                        if (tTestChange != null)
        //                        {
        //                            tTestChange.ChangeReferenceForAnother(sOldUser, sNewUser);
        //                            tProp.SetValue(this, tTestChange, null);
        //                        }
        //                    }

        //                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
        //                    {
        //                        NWDReferenceMultiple tTestChange = tProp.GetValue(this, null) as NWDReferenceMultiple;
        //                        if (tTestChange != null)
        //                        {
        //                            tTestChange.ChangeReferenceForAnother(sOldUser, sNewUser);
        //                            tProp.SetValue(this, tTestChange, null);
        //                        }
        //                    }

        //                    //NWDReferenceType<NWDAccount> tObject = tProp.GetValue(this, null) as NWDReferenceType<NWDAccount>;
        //                    //if (tObject != null)
        //                    //{
        //                    //    tObject.ChangeReferenceForAnother(sOldUser, sNewUser);
        //                    //}
        //                }
        //            }
        //            if (UpdateDataIfModified())
        //            {
        //                Debug.Log("##### ChangeUser success : " + BasisHelper().ClassNamePHP + " Reference = " + Reference);
        //            }
        //            //Debug.Log("##### NEED CHANGE THE ACCOUNT " + Reference + " Newintegrity = " + Integrity);
        //        }
        //        else
        //        {
        //            Debug.Log("##### ChangeUser not account dependant");
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("##### ChangeUser integrity false : " +BasisHelper().ClassNamePHP + " Reference = "+ Reference);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================