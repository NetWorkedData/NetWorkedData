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

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassDeclare(bool sServerSynchronize, string sClassTrigramme, string sMenuName, string sDescription)
        {
            Type tType = MethodBase.GetCurrentMethod().DeclaringType;
            //Debug.Log ("tType : " + tType.Name);
            //Debug.Log ("K : " + typeof(K).Name);
            NWDDatas.Declare(typeof(K), sServerSynchronize, sClassTrigramme, sMenuName, sDescription);
            //NWDDataManager.SharedInstance().AddClassToManage (typeof(K), sServerSynchronize, sClassTrigramme, sMenuName, sDescription);

            redefineClassToUse(typeof(K), sServerSynchronize, sClassTrigramme, sMenuName, sDescription);

        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDatas Datas()
        {
            return NWDDatas.FindTypeInfos(typeof(K));
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string ClassID()
        {
            string tReturn = typeof(K).Name;
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Type ClassType()
        {
            return typeof(K);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kMenuNameType = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string MenuName()
        //{
        //    string rReturn = null;
        //    //Debug.Log ("ClassID () to find key" + ClassID ());
        //    if (kMenuNameType.ContainsKey(ClassID()))
        //    {
        //        //Debug.Log ("rReturn  find key");
        //        rReturn = kMenuNameType[ClassID()];
        //    }
        //    if (rReturn == null)
        //    {
        //        rReturn = "";
        //    }
        //    //Debug.Log ("rReturn " + rReturn);
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetMenuName(string sMenuName)
        //{
        //    if (sMenuName != null)
        //    {
        //        kMenuNameType[ClassID()] = sMenuName;
        //    }
        //    ;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //internal static Dictionary<string, Type> kClassType = new Dictionary<string, Type>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static Type ClassType()
        //{
        //    Type rReturn = null;
        //    if (kClassType.ContainsKey(ClassID()))
        //    {
        //        rReturn = kClassType[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetClassType(Type sType)
        //{
        //    if (sType != null)
        //    {
        //        kClassType[ClassID()] = sType;
        //    }
        //}

        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kClassName = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string[] AllClassName()
        //{
        //    return kClassName.Values.ToArray<string>();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static string ClassName()
        //{
        //    string rReturn = "";
        //    if (kClassName.ContainsKey(ClassID()))
        //    {
        //        rReturn = kClassName[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetClassName(string sClassName)
        //{
        //    if (sClassName != null)
        //    {
        //        kClassName[ClassID()] = sClassName;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public string MyClassName()
        //{
        //    return ClassName();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public string MyClassNamePHP()
        {
            return Datas().ClassNamePHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ClassNameUsedValue()
        {
            return Datas().ClassNamePHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kTableName = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string[] AllTableName()
        //{
        //    return kTableName.Values.ToArray<string>();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static string TableName()
        //{
        //    string rReturn = "";
        //    if (kTableName.ContainsKey(ClassID()))
        //    {
        //        rReturn = kTableName[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetTableName(string sTableName)
        //{
        //    if (sTableName != null)
        //    {
        //        kTableName[ClassID()] = sTableName;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kClassTrigramme = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string ClassTrigramme()
        //{
        //    string rReturn = "";
        //    if (kClassTrigramme.ContainsKey(ClassID()))
        //    {
        //        rReturn = kClassTrigramme[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetClassTrigramme(string sClassTrigramme)
        //{
        //    if (sClassTrigramme != null)
        //    {
        //        kClassTrigramme[ClassID()] = sClassTrigramme;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kClassDescription = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string ClassDescription()
        //{
        //    string rReturn = "";
        //    if (kClassDescription.ContainsKey(ClassID()))
        //    {
        //        rReturn = kClassDescription[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetClassDescription(string sClassDescription)
        //{
        //    if (sClassDescription != null)
        //    {
        //        kClassDescription[ClassID()] = sClassDescription;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kPrefBaseKey = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string PrefBaseKey()
        //{
        //    string rReturn = "";
        //    if (kPrefBaseKey.ContainsKey(ClassID()))
        //    {
        //        rReturn = kPrefBaseKey[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetPrefBaseKey(string skPrefBaseKey)
        //{
        //    if (skPrefBaseKey != null)
        //    {
        //        kPrefBaseKey[ClassID()] = skPrefBaseKey;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kPrefSalt = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string PrefSalt()
        //{
        //    string rReturn = "";
        //    if (kPrefSalt.ContainsKey(ClassID()))
        //    {
        //        rReturn = kPrefSalt[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetPrefSalt(string sPrefSalt)
        //{
        //    if (sPrefSalt != null)
        //    {
        //        kPrefSalt[ClassID()] = sPrefSalt;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static bool TestSaltValid()
        //{
        //    bool rReturn = false;
        //    if (PrefSalt() == "ok")
        //    {
        //        rReturn = true;
        //    }
        //    else
        //    {
        //        //Debug.Log ("!!! error in salt memorize : " + ClassNamePHP ());
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kPrefSaltA = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string PrefSaltA()
        //{
        //    string rReturn = "";
        //    if (kPrefSaltA.ContainsKey(ClassID()))
        //    {
        //        rReturn = kPrefSaltA[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetPrefSaltA(string sPrefSaltA)
        //{
        //    if (sPrefSaltA != null)
        //    {
        //        kPrefSaltA[ClassID()] = sPrefSaltA;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kPrefSaltB = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string PrefSaltB()
        //{
        //    string rReturn = "";
        //    if (kPrefSaltB.ContainsKey(ClassID()))
        //    {
        //        rReturn = kPrefSaltB[ClassID()];
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetPrefSaltB(string sPrefSaltB)
        //{
        //    if (sPrefSaltB != null)
        //    {
        //        kPrefSaltB[ClassID()] = sPrefSaltB;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, NWDBasis<K>> kObjectInEdition = new Dictionary<string, NWDBasis<K>>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis<K> ObjectInEdition()
        //{
        //    NWDBasis<K> rReturn = null;
        //    if (kObjectInEdition.ContainsKey(ClassID()))
        //    {
        //        rReturn = kObjectInEdition[ClassID()];
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void redefineClassToUse(Type sType, bool sServerSynchronize, string sClassTrigramme, string sMenuName, string sDescription = "")
        {

            string tTableName = sType.Name;
            string tClassName = sType.AssemblyQualifiedName;
            //SetClassType(sType);
            //SetTableName(tTableName);
            //SetClassTrigramme(sClassTrigramme);

            //SetPrefBaseKey(tTableName + "_");
            //SetMenuName(sMenuName);
            //SetClassDescription(sDescription);




            Datas().PrefLoad();

            AccountDependentAnalyze();

            if (NWDDataManager.SharedInstance().mTypeList.Contains(sType) == false)
            {
                NWDDataManager.SharedInstance().mTypeList.Add(sType);
            }
            if (sServerSynchronize == true)
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(sType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Add(sType);
                }
                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(sType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Remove(sType);
                }

                if (AccountDependent())
                {
                    if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == false)
                    {
                        NWDDataManager.SharedInstance().mTypeAccountDependantList.Add(sType);
                    }
                    if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == true)
                    {
                        NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(sType);
                    }
                }
                else
                {
                    if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == false)
                    {
                        NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Add(sType);
                    }
                    if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == true)
                    {
                        NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(sType);
                    }
                }

            }
            else
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(sType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Remove(sType);
                }
                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(sType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Add(sType);
                }
                if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(sType);
                }
                if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(sType);
                }
            }
            if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey(sClassTrigramme))
            {
                Debug.LogWarning("ERROR in " + tClassName + ", this trigramme '" + sClassTrigramme + "' is allreday use by another class! (" + NWDDataManager.SharedInstance().mTrigramTypeDictionary[sClassTrigramme] + ")");
            }
            else
            {
                NWDDataManager.SharedInstance().mTrigramTypeDictionary.Add(sClassTrigramme, sType);
            }

            NWDDataManager.SharedInstance().mTypeLoadedList.Add(sType);

            CreateTable();
            if (NWDDataManager.SharedInstance().NeedCopy == true)
            {
                CopyTable(/*NWDDataManager.SharedInstance().SQLiteConnectionFromBundleCopy*/);
            }
            // Invoke the Class Initialization method
            var tMethodInfo = ClassType().GetMethod("ClassInitialization", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(null, null);
            }




            //LoadTableEditor();
#if UNITY_EDITOR
            FilterTableEditor();
            Datas().PrefSave();
            //PrepareOrders(); // don't do that here: that's fake the weservice number / order
#else
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //static public string kPrefSaltValidKey = "SaltValid";
        //static public string kPrefSaltAKey = "SaltA";
        //static public string kPrefSaltBKey = "SaltB";


//        //-------------------------------------------------------------------------------------------------------------
//        protected static void PrefSave()
//        {
//            //Debug.Log ("PrefSave");
//            // reccord data to user's preferences
//            NWDAppConfiguration.SharedInstance().SetSalt(Datas().PrefBaseKey, NWDConstants.kPrefSaltAKey, PrefSaltA());
//            NWDAppConfiguration.SharedInstance().SetSalt(Datas().PrefBaseKey, NWDConstants.kPrefSaltBKey, PrefSaltB());
//            NWDAppConfiguration.SharedInstance().SetSaltValid(Datas().PrefBaseKey, NWDConstants.kPrefSaltValidKey, "ok");
//#if UNITY_EDITOR
//            //NWDAppConfiguration.SharedInstance().SaveNewCSharpFile ();
//#endif
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void PrefLoad()
        //{
        //    //Debug.Log ("PrefLoad");
        //    // load data from user's preferences
        //    SetPrefSaltA(NWDAppConfiguration.SharedInstance().GetSalt(Datas().PrefBaseKey, NWDConstants.kPrefSaltAKey, NWDConstants.kPrefSaltValidKey));
        //    SetPrefSaltB(NWDAppConfiguration.SharedInstance().GetSalt(Datas().PrefBaseKey, NWDConstants.kPrefSaltBKey, NWDConstants.kPrefSaltValidKey));
        //    SetPrefSalt(NWDAppConfiguration.SharedInstance().GetSaltValid(Datas().PrefBaseKey, NWDConstants.kPrefSaltValidKey));
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The account dependent. If this class' object is connected to an user account by a NWDReferenceType
        /// </summary>
        //public static Dictionary<Type, bool> kGameSaveDependent = new Dictionary<Type, bool>();
        /// <summary>
        /// The account dependent properties. If this class' object is connected to an user account by NWDReferenceType 
        /// with the properties informations
        /// </summary>
        //public static Dictionary<Type, PropertyInfo> kGameDependentProperties = new Dictionary<Type, PropertyInfo>();
        /// <summary>
        /// The account dependent. If this class' object is connected to an user account by a NWDReferenceType
        /// </summary>
        //public static Dictionary<string, bool> kAccountDependent = new Dictionary<string, bool>();
        /// <summary>
        /// The account dependent properties. If this class' object is connected to an user account by NWDReferenceType 
        /// with the properties informations
        /// </summary>
        //public static Dictionary<string, PropertyInfo[]> kAccountDependentProperties = new Dictionary<string, PropertyInfo[]>();
        /// <summary>
        /// The account dependent properties. If this class' object is connected to an user account by NWDReferencesListType
        ///  NWDReferencesQuantityType or NWDReferenceHashType with the properties informations
        /// </summary>
        //public static Dictionary<string, PropertyInfo[]> kAccountConnectedProperties = new Dictionary<string, PropertyInfo[]>();
        /// <summary>
        /// The class is not account dependent, then the class must be locked on game. Allways return false in editor.
        /// </summary>
        //public static Dictionary<string, bool> kLockedObject = new Dictionary<string, bool>();


        /// <summary>
        /// The asset dependent. If this class' object is connected to an asset by a NWDAssetType subclass
        /// </summary>
        //public static Dictionary<string, bool> kAssetDependent = new Dictionary<string, bool>();
        /// <summary>
        /// The asset dependent properties. If this class' object is connected to a NWDAssetType subclass 
        /// with the properties informations
        /// </summary>
        //public static Dictionary<string, PropertyInfo[]> kAssetDependentProperties = new Dictionary<string, PropertyInfo[]>();

        //-------------------------------------------------------------------------------------------------------------
        public static void AccountDependentAnalyze()
        {
            bool rAccountConnected = false;
            bool rAssetConnected = false;
            bool rLockedObject = true;
            List<PropertyInfo> tPropertyList = new List<PropertyInfo>();
            List<PropertyInfo> tPropertyListConnected = new List<PropertyInfo>();
            List<PropertyInfo> tAssetPropertyList = new List<PropertyInfo>();
            Type tType = ClassType();

            Datas().ClassGameSaveDependent= false;
            Datas().ClassGameDependentProperties = null;
            // TODO : check 
            // exception for NWDAccount table
            if (tType == typeof(NWDAccount) || tType == typeof(NWDRequestToken))
            {
                rAccountConnected = true;
            }

            foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis != null)
                {
                    if (tTypeOfThis.IsGenericType)
                    {
                        if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                tPropertyList.Add(tProp);
                                tPropertyListConnected.Add(tProp);
                                rAccountConnected = true;
                                rLockedObject = false;
                            }
                            if (tSubType == typeof(NWDGameSave))
                            {
                                Datas().ClassGameSaveDependent = true;
                                Datas().ClassGameDependentProperties = tProp;
                            }
                        }
                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>)
                                 || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>)
                               || tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                // it's not directly a NWDAccount a dependency ....
                                tPropertyListConnected.Add(tProp);
                            }
                        }
                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                // it's not directly a NWDAccount a dependency ....
                                // I don't know what I must do with in this case..
                            }
                        }
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWDAssetType)))
                    {
                        rAssetConnected = true;
                        tAssetPropertyList.Add(tProp);
                    }
                }
            }

                Datas().kAccountDependent = rAccountConnected;
            // reccord class' object is account dependent properties
            Datas().kAccountDependentProperties = tPropertyList.ToArray();

            // reccord class' object is account connected properties
            Datas().kAccountConnectedProperties = tPropertyListConnected.ToArray();


            // reccord if class' object is locked for editor

#if UNITY_EDITOR
            rLockedObject = false;
#endif
                Datas().kLockedObject= rLockedObject;

            // reccord if class' object is asset dependent
            Datas().kAssetDependent = rAssetConnected;
            Datas().kAssetDependentProperties= tAssetPropertyList.ToArray();

        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountDependent()
        {
            return Datas().kAccountDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountConnect()
        {
            return Datas().kAccountConnectedProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountDependent()
        {
            return Datas().kAccountDependent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsClassLockedObject()
        {
            return Datas().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AssetDependent()
        {
            return Datas().kAssetDependent;
        }
        //----------------------------------------------
        public static PropertyInfo[] PropertiesAssetDependent()
        {
            return Datas().kAssetDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public bool IsLockedObject() // return true during the player game
        {
            return Datas().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================