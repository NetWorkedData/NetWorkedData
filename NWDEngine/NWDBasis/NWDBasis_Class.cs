// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:51
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

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasis()
        {
            //Debug.Log("NWDBasis Static Class Constructor()");
            NWDTypeLauncher.Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_ClassDeclare)]
        //public static void ClassDeclare()
        //{
        //   // Debug.Log("ClassDeclare for " + typeof(K).Name);
        //    //BTBBenchmark.Start();
        //    //BTBBenchmark.Start("ClassDeclare step 1");
        //    Type tActualType = typeof(K);
        //    bool tServerSynchronize = true;
        //    if (tActualType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true).Length > 0)
        //    {
        //        NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)tActualType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true)[0];
        //        tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
        //    }
        //    string tClassTrigramme = "XXX";
        //    if (tActualType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true).Length > 0)
        //    {
        //        NWDClassTrigrammeAttribute tTrigrammeAttribut = (NWDClassTrigrammeAttribute)tActualType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true)[0];
        //        tClassTrigramme = tTrigrammeAttribut.Trigramme;
        //        if (string.IsNullOrEmpty(tClassTrigramme))
        //        {
        //            tClassTrigramme = "EEE";
        //        }
        //    }
        //    string tDescription = "No description!";
        //    if (tActualType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true).Length > 0)
        //    {
        //        NWDClassDescriptionAttribute tDescriptionAttribut = (NWDClassDescriptionAttribute)tActualType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true)[0];
        //        tDescription = tDescriptionAttribut.Description;
        //        if (string.IsNullOrEmpty(tDescription))
        //        {
        //            tDescription = "Empty description!";
        //        }
        //    }
        //    string tMenuName = tActualType.Name + " menu";
        //    if (tActualType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true).Length > 0)
        //    {
        //        NWDClassMenuNameAttribute tMenuNameAttribut = (NWDClassMenuNameAttribute)tActualType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true)[0];
        //        tMenuName = tMenuNameAttribut.MenuName;
        //        if (string.IsNullOrEmpty(tMenuName))
        //        {
        //            tMenuName = tActualType.Name + " menu";
        //        }
        //    }
        //    //BTBBenchmark.Finish("ClassDeclare step 1");
        //    //BTBBenchmark.Start("ClassDeclare step 2");
        //    NWDBasisHelper.Declare(typeof(K), tServerSynchronize, tClassTrigramme, tMenuName, tDescription);
        //    //BTBBenchmark.Finish("ClassDeclare step 2");
        //    //BTBBenchmark.Start("ClassDeclare step 3");
        //    AccountDependentAnalyze();
        //    //BTBBenchmark.Finish("ClassDeclare step 3");
        //    //BTBBenchmark.Start("ClassDeclare step 4");
        //    if (NWDDataManager.SharedInstance().mTypeList.Contains(tActualType) == false)
        //    {
        //        NWDDataManager.SharedInstance().mTypeList.Add(tActualType);
        //    }

        //    if (AccountDependent())
        //    {
        //        if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(tActualType) == false)
        //        {
        //            NWDDataManager.SharedInstance().mTypeAccountDependantList.Add(tActualType);
        //        }
        //        if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(tActualType) == true)
        //        {
        //            NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(tActualType);
        //        }
        //    }
        //    else
        //    {
        //        if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(tActualType) == false)
        //        {
        //            NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Add(tActualType);
        //        }
        //        if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(tActualType) == true)
        //        {
        //            NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(tActualType);
        //        }
        //    }

        //    if (tServerSynchronize == true)
        //    {
        //        if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(tActualType) == false)
        //        {
        //            NWDDataManager.SharedInstance().mTypeSynchronizedList.Add(tActualType);
        //        }
        //        if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(tActualType) == true)
        //        {
        //            NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Remove(tActualType);
        //        }
        //    }
        //    else
        //    {
        //        if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(tActualType) == true)
        //        {
        //            NWDDataManager.SharedInstance().mTypeSynchronizedList.Remove(tActualType);
        //        }
        //        if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(tActualType) == false)
        //        {
        //            NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Add(tActualType);
        //        }
        //    }
        //    if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey(tClassTrigramme))
        //    {
        //        Debug.LogWarning("ERROR in " + tActualType.AssemblyQualifiedName + ", this trigramme '" + tClassTrigramme + "' is already use by another class! (" + NWDDataManager.SharedInstance().mTrigramTypeDictionary[tClassTrigramme] + ")");
        //    }
        //    else
        //    {
        //        NWDDataManager.SharedInstance().mTrigramTypeDictionary.Add(tClassTrigramme, tActualType);
        //    }
        //    NWDDataManager.SharedInstance().mTypeLoadedList.Add(tActualType);
        //    //BTBBenchmark.Finish("ClassDeclare step 4");
        //    //BTBBenchmark.Start("ClassDeclare step 5");

        //    // TODO : Change to remove invoke!
        //    //var tMethodInfo = ClassType().GetMethod("ClassInitialization", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //    //NWDAliasMethod.InvokeClassMethod(ClassType(),NWDConstants.M_ClassInitialization);
        //    //var tMethodInfo = ClassType().GetMethod(tMethodAlias, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //    //if (tMethodInfo != null)
        //    //{
        //    //    tMethodInfo.Invoke(null, null);
        //    //}
        //    BasisHelper().New_ClassInitialization();


        //    BasisHelper().ClassLoaded = true;
        //    //BTBBenchmark.Finish("ClassDeclare step 5");
        //    //BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.GetBasisHelper)]
        public static NWDBasisHelper BasisHelper()
        {
            NWDBasisHelper rHelper= NWDBasisHelper.FindTypeInfos(typeof(K));
            if (rHelper == null)
            {
                Debug.LogWarning("ERROR NWDBasisHelper.FindTypeInfos(typeof(K)) NOT RETURN FOR " + typeof(K).Name);
            }
            return rHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : REMOVE?!!
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
        /*
        public static string FindAliasName(string sAlias)
        {

            return NWDAlias.FindAliasName(ClassType(), sAlias);
        }
        */
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
        //public string MyClassNamePHP()
        //{
        //    return Datas().ClassNamePHP;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public override string ClassNameUsedValue()
        //{
        //    return Datas().ClassNamePHP;
        //}
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
        //    return Datas().SaltOk;
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
        //        public static void redefineClassToUse(Type sType, bool sServerSynchronize, string sClassTrigramme, string sMenuName, string sDescription = "")
        //        {

        //            Debug.Log("NWDBasis<K> redefineClassToUse tType() : " + typeof(K).Name);

        //            string tTableName = sType.Name;
        //            string tClassName = sType.AssemblyQualifiedName;
        //            //SetClassType(sType);
        //            //SetTableName(tTableName);
        //            //SetClassTrigramme(sClassTrigramme);

        //            //SetPrefBaseKey(tTableName + "_");
        //            //SetMenuName(sMenuName);
        //            //SetClassDescription(sDescription);




        //            Datas().PrefLoad();

        //            AccountDependentAnalyze();



        //            if (NWDDataManager.SharedInstance().mTypeList.Contains(sType) == false)
        //            {
        //                NWDDataManager.SharedInstance().mTypeList.Add(sType);
        //            }
        //            if (sServerSynchronize == true)
        //            {
        //                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(sType) == false)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Add(sType);
        //                }
        //                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(sType) == true)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Remove(sType);
        //                }

        //                if (AccountDependent())
        //                {
        //                    if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == false)
        //                    {
        //                        NWDDataManager.SharedInstance().mTypeAccountDependantList.Add(sType);
        //                    }
        //                    if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == true)
        //                    {
        //                        NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(sType);
        //                    }
        //                }
        //                else
        //                {
        //                    if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == false)
        //                    {
        //                        NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Add(sType);
        //                    }
        //                    if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == true)
        //                    {
        //                        NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(sType);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(sType) == true)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Remove(sType);
        //                }
        //                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(sType) == false)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Add(sType);
        //                }
        //                if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(sType) == true)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(sType);
        //                }
        //                if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(sType) == true)
        //                {
        //                    NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(sType);
        //                }
        //            }
        //            if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey(sClassTrigramme))
        //            {
        //                Debug.LogWarning("ERROR in " + sType.AssemblyQualifiedName + ", this trigramme '" + sClassTrigramme + "' is allreday use by another class! (" + NWDDataManager.SharedInstance().mTrigramTypeDictionary[sClassTrigramme] + ")");
        //            }
        //            else
        //            {
        //                NWDDataManager.SharedInstance().mTrigramTypeDictionary.Add(sClassTrigramme, sType);
        //            }

        //            NWDDataManager.SharedInstance().mTypeLoadedList.Add(sType);


        //            //if (NWDDataManager.SharedInstance().NeedCopy == true)
        //            //{
        //            //    CopyTable(/*NWDDataManager.SharedInstance().SQLiteConnectionFromBundleCopy*/);
        //            //}
        //            // Invoke the Class Initialization method
        //            var tMethodInfo = ClassType().GetMethod("ClassInitialization", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //            if (tMethodInfo != null)
        //            {
        //                tMethodInfo.Invoke(null, null);
        //            }




        ////            //LoadTableEditor();
        ////            LoadFromDatabase();

        ////#if UNITY_EDITOR
        ////            FilterTableEditor();
        ////            Datas().PrefSave();
        ////            //PrepareOrders(); // don't do that here: that's fake the weservice number / order
        ////#else
        ////#endif
        //}
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
            //BTBBenchmark.Start();
            bool rAccountConnected = false;
            bool rAssetConnected = false;
            bool rLockedObject = true;
            List<PropertyInfo> tPropertyList = new List<PropertyInfo>();
            List<PropertyInfo> tPropertyListConnected = new List<PropertyInfo>();
            List<PropertyInfo> tAssetPropertyList = new List<PropertyInfo>();
            Dictionary<PropertyInfo, MethodInfo> tAccountMethodList = new Dictionary<PropertyInfo, MethodInfo>();
            Type tType = ClassType();

            BasisHelper().ClassGameSaveDependent = false;
            BasisHelper().ClassGameDependentProperties = null;
            BasisHelper().GameSaveMethod = null;
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
                                MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                tAccountMethodList.Add(tProp, tMethod);
                                rAccountConnected = true;
                                rLockedObject = false;
                            }
                            if (tSubType == typeof(NWDGameSave))
                            {
                                BasisHelper().ClassGameSaveDependent = true;
                                BasisHelper().ClassGameDependentProperties = tProp;
                                MethodInfo tGameSaveMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                BasisHelper().GameSaveMethod = tGameSaveMethod;
                            }
                        }
                        else if (
                            tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple))
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAmountType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAverageType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesConditionalType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>)
                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesRangeType<>)
                        )
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                // it's not directly a NWDAccount a dependency ....
                                tPropertyListConnected.Add(tProp);
                                MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                tAccountMethodList.Add(tProp, tMethod);
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

            BasisHelper().kAccountDependent = rAccountConnected;
            // reccord class' object is account dependent properties
            BasisHelper().kAccountDependentProperties = tPropertyList.ToArray();

            // reccord class' object is account connected properties
            BasisHelper().kAccountConnectedProperties = tPropertyListConnected.ToArray();
            BasisHelper().AccountMethodDico = tAccountMethodList;

            // reccord if class' object is locked for editor

#if UNITY_EDITOR
            rLockedObject = false;
#endif
            BasisHelper().kLockedObject = rLockedObject;

            // reccord if class' object is asset dependent
            BasisHelper().kAssetDependent = rAssetConnected;
            BasisHelper().kAssetDependentProperties = tAssetPropertyList.ToArray();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountDependent()
        {
            return BasisHelper().kAccountDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountConnected()
        {
            return BasisHelper().kAccountConnectedProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountConnected()
        {
            bool rReturn = false;
            if (BasisHelper().kAccountConnectedProperties.Length > 0)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountDependent()
        {
            return BasisHelper().kAccountDependent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool GameSaveDependent()
        {
            return BasisHelper().ClassGameSaveDependent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsClassLockedObject()
        {
            return BasisHelper().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AssetDependent()
        {
            return BasisHelper().kAssetDependent;
        }
        //----------------------------------------------
        public static PropertyInfo[] PropertiesAssetDependent()
        {
            return BasisHelper().kAssetDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool DatabaseIsLoaded()
        {
            bool rLoaded = true;
            if (AccountDependent() == true && NWDDataManager.SharedInstance().DataAccountLoaded == false)
            {
                rLoaded = false;
            }
            else if (AccountDependent() == false && NWDDataManager.SharedInstance().DataEditorLoaded == false)
            {
                rLoaded = false;
            }
            return rLoaded;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsLockedObject() // return true during the player game
        {
            return BasisHelper().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================