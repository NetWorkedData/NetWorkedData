//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //public enum NWDBasisType : int
    //{
    //    /// <summary>
    //    /// not synchronizable instance 
    //    /// </summary>
    //    UnsyncClass = -1,
    //    /// <summary>
    //    /// synchronizable instance by editor
    //    /// </summary>
    //    EditorClass = 0,
    //    /// <summary>
    //    /// synchronizable instance by account
    //    /// </summary>
    //    AccountClass = 1,
    //    /// <summary>
    //    /// synchronizable instance by accounts with gamesave 
    //    /// </summary>
    //    AccountGameSaveClass = 2,
    //    /// <summary>
    //    /// unsynchronizable instance with account limit in device
    //    /// </summary>
    //    AccountUnsyncClass = 3,
    //    /// <summary>
    //    /// unsynchronizable instance with account limit in device but sync in editor
    //    /// </summary>
    //    AccountRestrictedClass = 4,
    //    /// <summary>
    //    /// synchronizable instance by multi account
    //    /// </summary>
    //    MultiAccountClass = 6, // TODO Cluster development
    //    /// <summary>
    //    /// synchronizable instance by multi accounts with gamesave 
    //    /// </summary>
    //    MultiAccountGameSaveClass = 7, // TODO Cluster development
    //    /// <summary>
    //    /// Not defined : It's impossible!
    //    /// </summary>
    //    NotDefine = 99,
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        //public NWDBasisType BasisType;
        //-------------------------------------------------------------------------------------------------------------
        public PropertyInfo[] PropertiesArray;
        public PropertyInfo[] NWDDataPropertiesArray;
        //-------------------------------------------------------------------------------------------------------------
        bool DatasLoaded = false;
        bool DataIndexed = false;
        double Sizer; // the max size of instance
        //-------------------------------------------------------------------------------------------------------------
        public int IndexInMemoryAllObjects()
        {
            DataIndexed = false;
            int tRow = 0;
            if (IndexInMemoryMethodList.Count > 0)
            {
                foreach (NWDTypeClass tObject in Datas)
                {
                    tObject.IndexInMemory();
                    tRow++;
                }
            }
            DataIndexed = true;
#if UNITY_EDITOR
            //Debug.Log("NWDBasisHelper " + ClassNamePHP + " IndexAll() row indexed : " + tRow + " rows.");
#endif
            return tRow;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void IndexInBaseAllObjects()
        {
            DataIndexed = false;
            //int tRow = 0;
            if (IndexInMemoryMethodList.Count > 0)
            {
                foreach (NWDTypeClass tObject in Datas)
                {
                    tObject.IndexInBase();
                    //tRow++;
                }
            }
            DataIndexed = true;
#if UNITY_EDITOR
            //Debug.Log("NWDBasisHelper " + ClassNamePHP + " IndexAll() row indexed : " + tRow + " rows.");
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsIndexed()
        {
            return DataIndexed;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsLoaded()
        {
            return DatasLoaded;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void RepaintTableEditor()
        {
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RepaintInspectorEditor()
        {
            NWDDataInspector.ActiveRepaint();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeAssetPath(string sOldPath, string sNewPath)
        {
            if (kAssetDependent == true)
            {
                foreach (NWDTypeClass tObject in Datas)
                {
                    tObject.ChangeAssetPathMe(sOldPath, sNewPath);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass GetDataByReference(string sReference)
        {
            NWDTypeClass rReturn = null;
            if (sReference != null)
            {
                if (DatasByReference.ContainsKey(sReference))
                {
                    rReturn = DatasByReference[sReference];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> ClasseInThisSync()
        {
            List<Type> rReturn = null;
            //if (Application.isPlaying == true)
            //{
            rReturn = OverrideClasseInThisSync();
            if (rReturn.Contains(ClassType) == false)
            {
                rReturn.Add(ClassType);
            }
            //}
            //else
            //{
            //    rReturn = new List<Type>();
            //    rReturn.Add(ClassType);
            //}
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisHelper()
        {
            NWDLauncher.Launch();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ClassLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassType = null;
        //public Type ConnexionType = null;
        public string ClassName = string.Empty;
        public string ClassNamePHP = string.Empty;
        public string TablePrefix = string.Empty;
        //public bool ClassSynchronize;
        public string ClassTrigramme = string.Empty;
        public string ClassDescription = string.Empty;
        public string ClassMenuName = string.Empty;
        public string ClassTableName = string.Empty;
        public string ClassPrefBaseKey = string.Empty;
        public GUIContent ClassMenuNameContent = null;
        //-------------------------------------------------------------------------------------------------------------
        public bool kLockedObject; // false if account dependant but bypass in editor mode (allways false to authorize sync)
        //-------------------------------------------------------------------------------------------------------------
        //public bool ClassGameSaveDependent = false;
        //public PropertyInfo ClassGameDependentProperties;
        //public MethodInfo GameSaveMethod;
        //-------------------------------------------------------------------------------------------------------------
        //public bool kAccountDependent = false;
        //public List<PropertyInfo> kAccountDependentProperties = new List<PropertyInfo>();
        //public List<PropertyInfo> kAccountConnectedProperties = new List<PropertyInfo>();
        //public Dictionary<PropertyInfo, MethodInfo> AccountMethodDico = new Dictionary<PropertyInfo, MethodInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public bool kAssetDependent;
        public List<PropertyInfo> kAssetDependentProperties = new List<PropertyInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public int LastWebBuild = 0;
        //-------------------------------------------------------------------------------------------------------------
        public bool WebModelChanged = false;
        public bool WebModelDegraded = false;
        public List<string> WebModelDegradationList = new List<string>();
        public Dictionary<int, int> WebServiceWebModel = new Dictionary<int, int>();
        public Dictionary<int, List<string>> WebModelPropertiesOrder = new Dictionary<int, List<string>>();
        public Dictionary<int, string> WebModelSQLOrder = new Dictionary<int, string>();
        //-------------------------------------------------------------------------------------------------------------
        public string SaltStart = string.Empty;
        public string SaltEnd = string.Empty;
        public bool SaltValid = false;
        //-------------------------------------------------------------------------------------------------------------
        public List<MethodInfo> IndexInMemoryMethodList = new List<MethodInfo>();
        public List<MethodInfo> DeindexInMemoryMethodList = new List<MethodInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public List<MethodInfo> IndexInBaseMethodList = new List<MethodInfo>();
        public List<MethodInfo> DeindexInBaseMethodList = new List<MethodInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public int ClusterMin;
        public int ClusterMax;
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<Type, NWDBasisHelper> TypesDictionary = new Dictionary<Type, NWDBasisHelper>();
        public static Dictionary<string, NWDBasisHelper> StringsDictionary = new Dictionary<string, NWDBasisHelper>(new StringIndexKeyComparer());

        public NWDTemplateHelper TemplateHelper = new NWDTemplateHelper();
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InitHelper(Type sType, bool sBase = false)
        {
            NWEBenchmark.Start();

            TemplateHelper.SetClassType(sType);

            bool rAccountConnected = false;

            // define type class and synchronizable option
            //BasisType =  NWDBasisType.EditorClass;
            if (sType.IsSubclassOf(typeof(NWDBasisUnsynchronize)))
            {
                //BasisType = NWDBasisType.UnsyncClass;
                rAccountConnected = true; // to reccord in AccountDatabase
                //ClassSynchronize = false;
            }
            else if (sType.IsSubclassOf(typeof(NWDBasisAccountDependent)))
            {
                //BasisType = NWDBasisType.AccountClass;
                rAccountConnected = true;  // to reccord in AccountDatabase
                //ClassSynchronize = true;
            }
            else if (sType.IsSubclassOf(typeof(NWDBasisGameSaveDependent)))
            {
                //BasisType = NWDBasisType.AccountGameSaveClass;
                rAccountConnected = true;  // to reccord in AccountDatabase
                //ClassSynchronize = true;
            }
            else if (sType.IsSubclassOf(typeof(NWDBasisAccountUnsynchronize)))
            {
                //BasisType = NWDBasisType.AccountUnsyncClass;
                rAccountConnected = true;  // to reccord in AccountDatabase
                //ClassSynchronize = true;
            }
            else if (sType.IsSubclassOf(typeof(NWDBasisAccountRestricted)))
            {
                //BasisType = NWDBasisType.AccountRestrictedClass;
                rAccountConnected = true;  // to reccord in AccountDatabase
                //ClassSynchronize = false;
            }
            else 
            {
                //BasisType = NWDBasisType.EditorClass;
                //ClassSynchronize = true;
            }


            //if (sType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true).Length > 0)
            //{
            //    NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)sType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true)[0];
            //    tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
            //}

            string tClassTrigramme = "XXX";
            if (sType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true).Length > 0)
            {
                NWDClassTrigrammeAttribute tTrigrammeAttribut = (NWDClassTrigrammeAttribute)sType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true)[0];
                tClassTrigramme = tTrigrammeAttribut.Trigramme;
                if (string.IsNullOrEmpty(tClassTrigramme))
                {
                    tClassTrigramme = "EEE";
                }
            }
#if UNITY_EDITOR
            string tDescription = "No description!";
            if (sType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true).Length > 0)
            {
                NWDClassDescriptionAttribute tDescriptionAttribut = (NWDClassDescriptionAttribute)sType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true)[0];
                tDescription = tDescriptionAttribut.Description;
                if (string.IsNullOrEmpty(tDescription))
                {
                    tDescription = "Empty description!";
                }
            }
            string tMenuName = sType.Name + " menu";
            if (sType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true).Length > 0)
            {
                NWDClassMenuNameAttribute tMenuNameAttribut = (NWDClassMenuNameAttribute)sType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true)[0];
                tMenuName = tMenuNameAttribut.MenuName;
                if (string.IsNullOrEmpty(tMenuName))
                {
                    tMenuName = sType.Name + " menu";
                }
            }
            ClassMenuName = tMenuName;
            ClassDescription = tDescription;
#endif

            //NWEBenchmark.Step();
            // insert basic infos
            ClassType = sType;
            ClassTableName = sType.Name;

            ClassName = sType.AssemblyQualifiedName;
            string rClassName = sType.Name;
            ClassNamePHP = rClassName;
            ClassPrefBaseKey = ClassNamePHP + "_";
            ClassTrigramme = tClassTrigramme;
            NWEBenchmark.Step();
            // TODO:  ... too long! that take 0.006s ... it's too much!

            foreach (MethodInfo tMethod in sType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tMethod.GetCustomAttributes(typeof(NWDIndexInMemory), true).Length > 0)
                {
                    if (IndexInMemoryMethodList.Contains(tMethod) == false)
                    {
                        IndexInMemoryMethodList.Add(tMethod);
                    }
                }
                if (tMethod.GetCustomAttributes(typeof(NWDDeindexInMemory), true).Length > 0)
                {
                    if (DeindexInMemoryMethodList.Contains(tMethod) == false)
                    {
                        DeindexInMemoryMethodList.Add(tMethod);
                    }
                }
                if (tMethod.GetCustomAttributes(typeof(NWDIndexInBase), true).Length > 0)
                {
                    if (IndexInBaseMethodList.Contains(tMethod) == false)
                    {
                        IndexInBaseMethodList.Add(tMethod);
                    }
                }
                if (tMethod.GetCustomAttributes(typeof(NWDDeindexInBase), true).Length > 0)
                {
                    if (DeindexInBaseMethodList.Contains(tMethod) == false)
                    {
                        DeindexInBaseMethodList.Add(tMethod);
                    }
                }
            }


            //NWEBenchmark.Step();
            PrefLoad();
            // NWEBenchmark.Step();
            bool rAssetConnected = false;
            bool rLockedObject = true;
            List<PropertyInfo> tPropertyList = new List<PropertyInfo>();
            List<PropertyInfo> tPropertyListConnected = new List<PropertyInfo>();
            List<PropertyInfo> tAssetPropertyList = new List<PropertyInfo>();
            Dictionary<PropertyInfo, MethodInfo> tAccountMethodList = new Dictionary<PropertyInfo, MethodInfo>();

            //ClassGameSaveDependent = false;
            //ClassGameDependentProperties = null;
            //GameSaveMethod = null;
            // exception for NWDAccount table
            //NWDClassUnityEditorOnlyAttribute tServerOnlyAttribut = (NWDClassUnityEditorOnlyAttribute)sType.GetCustomAttribute(typeof(NWDClassUnityEditorOnlyAttribute), true);
            //if (tServerOnlyAttribut != null)
            //{
            //    rAccountConnected = true;
            //}
            NWEBenchmark.Step();
            PropertiesArray = sType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<PropertyInfo> tDataPropertiesArray = new List<PropertyInfo>();
            foreach (PropertyInfo tProp in sType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis != null)
                {
                    if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        tDataPropertiesArray.Add(tProp);
                    }

                    if (tTypeOfThis.IsGenericType)
                    {
                        if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                {
                                    tPropertyList.Add(tProp);
                                    tPropertyListConnected.Add(tProp);
                                    MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                    tAccountMethodList.Add(tProp, tMethod);
                                    rAccountConnected = true;
                                    rLockedObject = false;
                                }
                                else
                                {
                                    Debug.Log("ignore account dependance for " + tTypeOfThis.Name +" in " + sType.Name);
                                }
                            }
                            //if (tSubType == typeof(NWDGameSave))
                            //{
                            //    ClassGameSaveDependent = true;
                            //    ClassGameDependentProperties = tProp;
                            //    MethodInfo tGameSaveMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                            //    GameSaveMethod = tGameSaveMethod;
                            //}
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
                                if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                {
                                    tPropertyListConnected.Add(tProp);
                                    MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                    tAccountMethodList.Add(tProp, tMethod);
                                    rAccountConnected = true;
                                    rLockedObject = false;
                                }
                                else
                                {
                                    Debug.Log("ignore account dependance for " + tTypeOfThis.Name + " in " + sType.Name);
                                }
                            }
                        }
                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                {
                                    // it's not directly a NWDAccount a dependency ....
                                    // I don't know what I must do with in this case..
                                }
                                else
                                {
                                    Debug.Log("ignore account dependance for " + tTypeOfThis.Name + " in " + sType.Name);
                                }
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
            NWEBenchmark.Step();
            NWDDataPropertiesArray = tDataPropertiesArray.ToArray();
            //kAccountDependent = rAccountConnected;
            // reccord class' object is account dependent properties
            //kAccountDependentProperties = tPropertyList;
            // reccord class' object is account connected properties
            //kAccountConnectedProperties = tPropertyListConnected;
            //AccountMethodDico = tAccountMethodList;
            // reccord if class' object is locked for editor
#if UNITY_EDITOR
            rLockedObject = false;
#endif
            kLockedObject = rLockedObject;
            // reccord if class' object is asset dependent
            kAssetDependent = rAssetConnected;
            kAssetDependentProperties = tAssetPropertyList;
            NWDClassClusterAttribute tClusterAttribute = null;
            //NWEBenchmark.Step();
            if (sType.GetCustomAttributes(typeof(NWDClassClusterAttribute), true).Length > 0)
            {
                tClusterAttribute = (NWDClassClusterAttribute)sType.GetCustomAttributes(typeof(NWDClassClusterAttribute), true)[0];
            }
            if (tClusterAttribute != null)
            {
                ClusterMin = tClusterAttribute.Min;
                ClusterMax = tClusterAttribute.Max;
            }
            else
            {
                ClusterMin = 0;
                ClusterMax = 2048;
            }
            //NWEBenchmark.Step();
            NWEBenchmark.Finish(true, ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstallHelper()
        {
            NWEBenchmark.Start();
            if (StringsDictionary.ContainsKey(ClassNamePHP))
            {
                Debug.LogWarning(ClassNamePHP + " already in StringsDictionary!");
            }
            else
            {
                StringsDictionary.Add(ClassNamePHP, this);
            }
            if (NWDDataManager.SharedInstance().mTypeList.Contains(ClassType) == false)
            {
                NWDDataManager.SharedInstance().mTypeList.Add(ClassType);
            }
            //if (kAccountDependent)
            if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent || TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(ClassType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeAccountDependantList.Add(ClassType);
                }
                if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(ClassType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Remove(ClassType);
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Contains(ClassType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Add(ClassType);
                }
                if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(ClassType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeAccountDependantList.Remove(ClassType);
                }
            }


            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(ClassType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Add(ClassType);
                }
                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Remove(ClassType);
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(ClassType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Remove(ClassType);
                }
                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Add(ClassType);
                }
            }
            if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey(ClassTrigramme))
            {
                Debug.LogWarning("ERROR in " + ClassType.AssemblyQualifiedName + ", this trigramme '" + ClassTrigramme + "' is already use by another class! (" + NWDDataManager.SharedInstance().mTrigramTypeDictionary[ClassTrigramme] + ")");
            }
            else
            {
                NWDDataManager.SharedInstance().mTrigramTypeDictionary.Add(ClassTrigramme, ClassType);
            }
            if (NWDDataManager.SharedInstance().mTypeLoadedList.Contains(ClassType) == false)
            {
                NWDDataManager.SharedInstance().mTypeLoadedList.Add(ClassType);
            }
            NWEBenchmark.Finish(true, "mixte mode " + ClassNamePHP);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper Declare(Type sType, Type sTypeHelper)
        {
            //NWEBenchmark.Start();
            NWDBasisHelper tTypeInfos = null;
            if (sType.IsSubclassOf(typeof(NWDTypeClass)))
            {


                if (TypesDictionary.ContainsKey(sType))
                {
                    Debug.LogWarning(sType.Name + " already in TypesDictionary");
                    tTypeInfos = TypesDictionary[sType];
                }
                else
                {
                    Type tTypeHelper = sTypeHelper;
                    if (tTypeHelper != null)
                    {
                        tTypeInfos = Activator.CreateInstance(tTypeHelper) as NWDBasisHelper;
                    }
                    else
                    {
                        tTypeInfos = new NWDBasisHelper();
                    }
                    //NWEBenchmark.Step();
                    tTypeInfos.InitHelper(sType);
                    //NWEBenchmark.Step();
                    tTypeInfos.InstallHelper();
                    //NWEBenchmark.Step();
                    tTypeInfos.ClassInitialization();
                    //NWEBenchmark.Step();
#if UNITY_EDITOR
                    tTypeInfos.LoadEditorPrefererences();
                    //NWEBenchmark.Step();
#endif
                    tTypeInfos.ClassLoaded = true;
                    TypesDictionary.Add(sType, tTypeInfos);
                }
            }
            //NWEBenchmark.Finish(true, "Declare NWDBasisHelper " + sType.Name);
            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PrefLoad()
        {
            if (string.IsNullOrEmpty(SaltStart) || string.IsNullOrEmpty(SaltEnd))
            {
                SaltStart = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                SaltEnd = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                SaltValid = false;
            }
            if (LastWebBuild > NWDAppConfiguration.SharedInstance().WebBuildMax)
            {
                LastWebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper FindTypeInfos(Type sType)
        {
            NWDBasisHelper tTypeInfos = null;
            if (TypesDictionary.ContainsKey(sType))
            {
                tTypeInfos = TypesDictionary[sType];
            }
            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper FindTypeInfos(string sClassNamePHP)
        {
            NWDBasisHelper tTypeInfos = null;
            if (StringsDictionary.ContainsKey(sClassNamePHP))
            {
                tTypeInfos = StringsDictionary[sClassNamePHP];
            }
            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Informations(Type sType)
        {
            string rReturn = string.Empty;
            NWDBasisHelper tTypeInfos = FindTypeInfos(sType);
            if (tTypeInfos == null)
            {
                rReturn = "unknow";
            }
            else
            {
                rReturn = tTypeInfos.Informationss();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Informationss()
        {
            return "ClassName = '" + ClassName + "' " +
            "TrigrammeName = '" + ClassTrigramme + "' " +
            //"ServerSynchronize = '" + ClassSynchronize + "' " +
            "ClassDescription = '" + ClassDescription + "' " +
            "MenuName = '" + ClassMenuName + "' " +
            string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void AllInfos ()
        //{
        //	foreach (KeyValuePair<Type,NWDTypeInfos> tTypeTypeInfos in TypesDictionary) {
        //		Type tType = tTypeTypeInfos.Key;
        //	}
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public PropertyInfo[] PropertiesArray;
        ////-------------------------------------------------------------------------------------------------------------
        //public void PropertiesArrayPrepare()
        //{
        //    PropertiesArray = ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public List<string> PropertiesOrderArray;
        ////-------------------------------------------------------------------------------------------------------------
        //public void PropertiesOrderArrayPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    foreach (var tProp in PropertiesArray)
        //    {
        //        rReturn.Add(tProp.Name);
        //    }
        //    rReturn.Sort();
        //    PropertiesOrderArray = rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string[] CSVAssemblyOrderArray;
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// CSV assembly order array.
        ///// </summary>
        ///// <returns>The assembly order array.</returns>
        //public void CSVAssemblyOrderArrayPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    rReturn.AddRange(PropertiesOrderArray);
        //    rReturn.Remove("Integrity");
        //    rReturn.Remove("Reference");
        //    rReturn.Remove("ID");
        //    rReturn.Remove("DM");
        //    rReturn.Remove("DS");
        //    rReturn.Remove("ServerHash");
        //    rReturn.Remove("ServerLog");
        //    rReturn.Remove("DevSync");
        //    rReturn.Remove("PreprodSync");
        //    rReturn.Remove("ProdSync");
        //    // add the good order for this element
        //    rReturn.Insert(0, "Reference");
        //    rReturn.Insert(1, "DM");
        //    rReturn.Insert(2, "DS");
        //    rReturn.Insert(3, "DevSync");
        //    rReturn.Insert(4, "PreprodSync");
        //    rReturn.Insert(5, "ProdSync");
        //    rReturn.Add("Integrity");
        //    CSVAssemblyOrderArray = rReturn.ToArray<string>();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string[] SLQAssemblyOrderArray;
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// SLQs the assembly order array.
        ///// </summary>
        ///// <returns>The assembly order array.</returns>
        //public void SLQAssemblyOrderArrayPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    rReturn.AddRange(PropertiesOrderArray);
        //    rReturn.Remove("Integrity");
        //    rReturn.Remove("Reference");
        //    rReturn.Remove("ID");
        //    rReturn.Remove("DM");
        //    rReturn.Remove("DS");
        //    rReturn.Remove("ServerHash");
        //    rReturn.Remove("ServerLog");
        //    rReturn.Remove("DevSync");
        //    rReturn.Remove("PreprodSync");
        //    rReturn.Remove("ProdSync");
        //    // add the good order for this element
        //    rReturn.Insert(0, "DM");
        //    rReturn.Insert(1, "DS");
        //    rReturn.Insert(2, "DevSync");
        //    rReturn.Insert(3, "PreprodSync");
        //    rReturn.Insert(4, "ProdSync");
        //    rReturn.Add("Integrity");
        //    SLQAssemblyOrderArray = rReturn.ToArray<string>();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string SLQAssemblyOrder;
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// SLQs the assembly order.
        ///// </summary>
        ///// <returns>The assembly order.</returns>
        //public void SLQAssemblyOrderPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    rReturn.AddRange(PropertiesOrderArray);
        //    rReturn.Remove("Integrity");
        //    rReturn.Remove("Reference");
        //    rReturn.Remove("ID");
        //    rReturn.Remove("DM");
        //    rReturn.Remove("DS");
        //    rReturn.Remove("ServerHash");
        //    rReturn.Remove("ServerLog");
        //    rReturn.Remove("DevSync");
        //    rReturn.Remove("PreprodSync");
        //    rReturn.Remove("ProdSync");
        //    // add the good order for this element
        //    rReturn.Insert(0, "Reference");
        //    rReturn.Insert(1, "DM");
        //    rReturn.Insert(2, "DS");
        //    rReturn.Insert(3, "DevSync");
        //    rReturn.Insert(4, "PreprodSync");
        //    rReturn.Insert(5, "ProdSync");
        //    rReturn.Add("Integrity");
        //    SLQAssemblyOrder = "`" + string.Join("`, `", rReturn.ToArray()) + "`";
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public List<string> SLQIntegrityOrder;
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// SLQs the assembly order.
        ///// </summary>
        ///// <returns>The assembly order.</returns>
        //public void SLQIntegrityOrderPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    rReturn.AddRange(PropertiesOrderArray);
        //    rReturn.Remove("Integrity");
        //    rReturn.Remove("Reference");
        //    rReturn.Remove("ID");
        //    rReturn.Remove("DM");
        //    rReturn.Remove("DS");
        //    rReturn.Remove("ServerHash");
        //    rReturn.Remove("ServerLog");
        //    rReturn.Remove("DevSync");
        //    rReturn.Remove("PreprodSync");
        //    rReturn.Remove("ProdSync");
        //    // add the good order for this element
        //    rReturn.Insert(0, "Reference");
        //    rReturn.Insert(1, "DM");
        //    SLQIntegrityOrder = rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public List<string> DataAssemblyPropertiesList;
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// SLQs the assembly order.
        ///// </summary>
        ///// <returns>The assembly order.</returns>
        //public void DataAssemblyPropertiesListPrepare()
        //{
        //    List<string> rReturn = new List<string>();
        //    rReturn.AddRange(PropertiesOrderArray);
        //    rReturn.Remove("Integrity"); // not include in integrity
        //    rReturn.Remove("Reference");
        //    rReturn.Remove("ID");
        //    rReturn.Remove("DM");
        //    rReturn.Remove("DS");// not include in integrity
        //    rReturn.Remove("ServerHash");// not include in integrity
        //    rReturn.Remove("ServerLog");// not include in integrity
        //    rReturn.Remove("DevSync");// not include in integrity
        //    rReturn.Remove("PreprodSync");// not include in integrity
        //    rReturn.Remove("ProdSync");// not include in integrity
        //    DataAssemblyPropertiesList = rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The datas list.
        /// </summary>
        public List<NWDTypeClass> Datas = new List<NWDTypeClass>();
        /// <summary>
        /// The datas by reference.
        /// </summary>
        public Dictionary<string, NWDTypeClass> DatasByReference = new Dictionary<string, NWDTypeClass>(new StringIndexKeyComparer());
        /// <summary>
        /// The datas by internal key. Return list of datas.
        /// </summary>
        public Dictionary<string, List<NWDTypeClass>> DatasByInternalKey = new Dictionary<string, List<NWDTypeClass>>(new StringIndexKeyComparer());
        /// <summary>
        /// The datas by reverse internal key. You must check if string InternalKey was changed ... in case change the DatasByInternalKey too!
        /// </summary>
        public Dictionary<NWDTypeClass, string> DatasByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
        /// <summary>
        /// The datas list. Reachable by the current account.
        /// </summary>
        //public List<NWDTypeClass> DatasReachable = new List<NWDTypeClass>();
        ///// <summary>
        ///// The datas by reference. Reachable by the current account.
        ///// </summary>
        //public Dictionary<string, NWDTypeClass> DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
        ///// <summary>
        ///// The datas by internal key. Return list of datas reachable by the current account.
        ///// </summary>
        //public Dictionary<string, List<NWDTypeClass>> DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
        ///// <summary>
        ///// The datas by reverse internal key. Reachable by the current account. You must check if string InternalKey was changed ... in case change the DatasReachableByInternalKey too!
        ///// </summary>
        //public Dictionary<NWDTypeClass, string> DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();


#if UNITY_EDITOR
        //public Dictionary<NWDTypeClass, string> DatasInEditor = new Dictionary<NWDTypeClass, string>();
        //public Dictionary<NWDTypeClass, bool> DatasInEditorSelection = new Dictionary<NWDTypeClass, bool>();
        //public List<string> DatasInEditorReferenceList = new List<string>();
        //public List<string> DatasInEditorRowDescriptionList = new List<string>();
        //public List<bool> DatasInEditorSelectionList = new List<bool>();

        //public Dictionary<string, string> EditorDatasMenu = new Dictionary<string, string>(); // reference/desciption for menu <REF>

        //public List<NWDTypeClass> EditorTableDatas = new List<NWDTypeClass>(); // NWDTypeClass
        //public Dictionary<NWDTypeClass, bool> EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
        //// TODO Futur 

        ////-------------------------------------------------------------------------------------------------------------
        //public void ModelAnalyze()
        //{
        //    NWDAliasMethod.InvokeClassMethod(ClassType, NWDConstants.M_ModelAnalyze);
        //}
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void ResetDatas()
        {
            //NWEBenchmark.Start();
            DatasLoaded = false;
            Datas.Clear();
            DatasByReference.Clear();
            DatasByInternalKey.Clear();
            DatasByReverseInternalKey.Clear();
#if UNITY_EDITOR
            SetObjectInEdition(null);
            NWDDataInspector.InspectNetWorkedData(null, true, false);
            EditorTableDatas.Clear();
            EditorTableDatasSelected.Clear();
            EditorDatasMenu.Clear();
            EditorDatasMenu.Add("---", string.Empty);
#endif
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AllDatabaseIsIndexed()
        {
            return NWDDataManager.SharedInstance().DatasAreIndexed();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AllDatabaseIsLoaded()
        {
            return (NWDDataManager.SharedInstance().DataAccountLoaded == true && NWDDataManager.SharedInstance().DataEditorLoaded == true);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void UserChangedReloadDatas()
        //{
        //    RedefineReachableDatas();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //private void RedefineReachableDatas()
        //{
        //    DatasReachable = new List<NWDTypeClass>();
        //    DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
        //    DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
        //    DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
        //    foreach (NWDTypeClass tData in Datas)
        //    {
        //        bool tDataIsValid = tData.DataIntegrityState();
        //        if (tDataIsValid == true)
        //        {
        //            AddDataReachable(tData);
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //private void AddDataReachable(NWDTypeClass sData)
        //{
        //    NWEBenchmark.Start();
        //    if (sData.ReachableState() == true)
        //    {
        //        string tReference = sData.ReferenceUsedValue();
        //        // Anyway I check if Data is already in datalist
        //        if (DatasReachableByReference.ContainsKey(tReference) == false)
        //        {
        //            // get internal key
        //            string tInternalKey = sData.InternalKeyValue();
        //            // Anyway I add Data in datalist
        //            DatasReachable.Add(sData);
        //            DatasReachableByReference.Add(tReference, sData);
        //            if (DatasReachableByInternalKey.ContainsKey(tInternalKey) == true)
        //            {
        //                DatasReachableByInternalKey[tInternalKey].Add(sData);
        //            }
        //            else
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                tList.Add(sData);
        //                DatasReachableByInternalKey.Add(tInternalKey, tList);
        //            }
        //            DatasReachableByReverseInternalKey.Add(sData, tInternalKey);
        //        }
        //    }
        //    NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void AddData(NWDTypeClass sData)
        {
            //NWEBenchmark.Start();
            // get reference
            string tReference = sData.Reference;
            // Anyway I check if Data is already in datalist
            if (DatasByReference.ContainsKey(tReference) == false)
            {
                //Debug.Log("NWDDatas AddData() add data");
                // get internal key
                // Anyway I add Data in datalist
                Datas.Add(sData);
                DatasByReference.Add(tReference, sData);

                string tInternalKey = sData.InternalKey;
                if (DatasByInternalKey.ContainsKey(tInternalKey) == true)
                {
                    DatasByInternalKey[tInternalKey].Add(sData);
                }
                else
                {
                    List<NWDTypeClass> tList = new List<NWDTypeClass>();
                    tList.Add(sData);
                    DatasByInternalKey.Add(tInternalKey, tList);
                }
                if (DatasByReverseInternalKey.ContainsKey(sData) == false)
                {
                    DatasByReverseInternalKey.Add(sData, tInternalKey);
                }

            }
            else
            {
                //Debug.LogWarning("Try to add twice data!");
            }
            // Ok now I add datas in editor table list
#if UNITY_EDITOR

            if (EditorTableDatas.Contains(sData) == false)
            {
                EditorTableDatas.Add(sData);
            }
            if (EditorTableDatasSelected.ContainsKey(sData) == false)
            {
                EditorTableDatasSelected.Add(sData, false);
            }
            if (EditorDatasMenu.ContainsKey(sData.Reference) == false)
            {
                EditorDatasMenu.Add(sData.Reference, sData.DatasMenu());
            }
#endif
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void RemoveDataReachable(NWDTypeClass sData)
        //{
        //    NWEBenchmark.Start();
        //    string tReference = sData.ReferenceUsedValue();
        //    // Anyway I check if Data is already in datalist
        //    if (DatasReachableByReference.ContainsKey(tReference) == true)
        //    {
        //        // get internal key
        //        string tInternalKey = sData.InternalKeyValue();
        //        // Anyway I add Remove in datalist
        //        DatasReachable.Remove(sData);
        //        DatasReachableByReference.Remove(tReference);
        //        if (DatasReachableByInternalKey.ContainsKey(tInternalKey) == true)
        //        {
        //            DatasReachableByInternalKey[tInternalKey].Remove(sData);
        //            if (DatasReachableByInternalKey[tInternalKey].Count == 0)
        //            {
        //                DatasReachableByInternalKey.Remove(tInternalKey);
        //            }
        //        }
        //        DatasReachableByReverseInternalKey.Remove(sData);
        //    }
        //    NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(NWDTypeClass sData)
        {
            //Debug.Log("NWDDatas RemoveData()");
            //NWEBenchmark.Start();
            // get reference
            string tReference = sData.Reference;
            // Anyway I check if Data is already in datalist
            if (DatasByReference.ContainsKey(tReference) == true)
            {
                // get internal key
                string tInternalKey = sData.InternalKey;
                // Anyway I add Data in datalist
                //int tIndex = Datas.IndexOf(sData);
                Datas.Remove(sData);
                DatasByReference.Remove(tReference);
                if (DatasByInternalKey.ContainsKey(tInternalKey) == true)
                {
                    DatasByInternalKey[tInternalKey].Remove(sData);
                    if (DatasByInternalKey[tInternalKey].Count == 0)
                    {
                        DatasByInternalKey.Remove(tInternalKey);
                    }
                }
                DatasByReverseInternalKey.Remove(sData);
                // Ok now I need to remove it in reachable data
                //RemoveDataReachable(sData);
            }
            else
            {
                Debug.LogWarning("Try to remove an unreferenced data!");
            }
#if UNITY_EDITOR
            // remove object in editor table
            //int tIndexB = DatasInEditorReferenceList.IndexOf(tReference);
            //if (tIndexB >= 0 && tIndexB < DatasInEditorReferenceList.Count())
            //{
            //    DatasInEditorReferenceList.RemoveAt(tIndexB);
            //}
            //DatasInEditorRowDescriptionList.RemoveAt(tIndex);
            //DatasInEditorSelectionList.RemoveAt(tIndex);

            /*NEW*/
            if (EditorTableDatas.Contains(sData) == true)
            {
                EditorTableDatas.Remove(sData);
            }
            if (EditorTableDatasSelected.ContainsKey(sData) == true)
            {
                EditorTableDatasSelected.Remove(sData);
            }
            if (EditorDatasMenu.ContainsKey(tReference) == true)
            {
                EditorDatasMenu.Remove(tReference);
            }
            /*NEW*/
#endif
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void UpdateDataReachable(NWDTypeClass sData)
        //{
        //    string tReference = sData.ReferenceUsedValue();
        //    string tInternalKey = sData.InternalKeyValue();
        //    string tOldInternalKey = DatasReachableByReverseInternalKey[sData];
        //    if (tOldInternalKey != tInternalKey)
        //    {
        //        if (DatasReachableByReference.ContainsKey(tReference) == true)
        //        {
        //            DatasReachable.Remove(sData);
        //            DatasReachableByReference.Remove(tReference);
        //            if (DatasReachableByInternalKey.ContainsKey(tOldInternalKey) == true)
        //            {
        //                DatasReachableByInternalKey[tOldInternalKey].Remove(sData);
        //                if (DatasReachableByInternalKey[tOldInternalKey].Count == 0)
        //                {
        //                    DatasReachableByInternalKey.Remove(tOldInternalKey);
        //                }
        //            }
        //            DatasReachableByReverseInternalKey.Remove(sData);
        //        }
        //        AddDataReachable(sData);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(NWDTypeClass sData)
        {
            //Debug.Log("NWDDatas UpdateData()");
            string tReference = sData.Reference;
            string tInternalKey = sData.InternalKey;
            string tOldInternalKey = "";
            if (DatasByReverseInternalKey.ContainsKey(sData))
            {
                tOldInternalKey = DatasByReverseInternalKey[sData];
            }
            if (tOldInternalKey != tInternalKey)
            {
                //int tIndex = Datas.IndexOf(sData);
                // remove internal Key in list
                if (DatasByInternalKey.ContainsKey(tOldInternalKey) == true)
                {
                    DatasByInternalKey[tOldInternalKey].Remove(sData);
                    if (DatasByInternalKey[tOldInternalKey].Count == 0)
                    {
                        DatasByInternalKey.Remove(tOldInternalKey);
                    }
                }
                if (DatasByReverseInternalKey.ContainsKey(sData))
                {
                    DatasByReverseInternalKey.Remove(sData);
                }
                // add internal Key in list
                if (DatasByInternalKey.ContainsKey(tInternalKey) == true)
                {
                    DatasByInternalKey[tInternalKey].Add(sData);
                }
                else
                {
                    List<NWDTypeClass> tList = new List<NWDTypeClass>();
                    tList.Add(sData);
                    DatasByInternalKey.Add(tInternalKey, tList);
                }
                DatasByReverseInternalKey.Add(sData, tInternalKey);

                //UpdateDataReachable(sData);

            }
#if UNITY_EDITOR
            // remove object in editor table

            //DatasInEditorRowDescriptionList.RemoveAt(tIndex);
            //DatasInEditorSelectionList.RemoveAt(tIndex);

            //DatasInEditorRowDescriptionList.Insert(tIndex, tInternalKey + " <" + tReference + ">");
            //DatasInEditorSelectionList.Insert(tIndex, false);

            //int tIndexB = DatasInEditorReferenceList.IndexOf(tReference);
            //if (tIndexB >= 0 && tIndexB < DatasInEditorReferenceList.Count())
            //{
            //    DatasInEditorReferenceList.RemoveAt(tIndexB);
            //    // Active to auto remove on filter
            //    // if (sObject.Tag == (int)m_SearchTag)
            //    {
            //        DatasInEditorReferenceList.Insert(tIndexB, tReference);
            //    }
            //}
            //else
            //{
            //    // if (sObject.Tag == m_SearchTag)
            //    {
            //        DatasInEditorReferenceList.Add(tReference);
            //    }
            //}

            /*NEW*/
            if (EditorTableDatas.Contains(sData) == true)
            {
                // nothing ... 
            }
            if (EditorTableDatasSelected.ContainsKey(sData) == true)
            {

            }
            if (EditorDatasMenu.ContainsKey(tReference) == true)
            {
                EditorDatasMenu[sData.Reference] = sData.DatasMenu();
            }
            /*NEW*/
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass[] FindDatas(string sAccountReference = null,
        //                                NWDGameSave sGameSave = null,
        //                                bool sEnable = true,
        //                                bool sTrashed = false,
        //                                bool sIntegrity = true)
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass[] GetAllDatas(NWDDatasFilter sFilter)
        //{
        //    NWEBenchmark.Start();
        //    NWDTypeClass[] rReturn;
        //    switch (sFilter)
        //    {
        //        case NWDDatasFilter.ReachableAndDisabled:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                foreach (NWDTypeClass tDatas in DatasReachable)
        //                {
        //                    if (tDatas.EnableState() == false)
        //                    {
        //                        tList.Add(tDatas);
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndTrashed:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                foreach (NWDTypeClass tDatas in DatasReachable)
        //                {
        //                    if (tDatas.TrashState() == true)
        //                    {
        //                        tList.Add(tDatas);
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndEnable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                foreach (NWDTypeClass tDatas in DatasReachable)
        //                {
        //                    if (tDatas.EnableState() == true)
        //                    {
        //                        tList.Add(tDatas);
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.All:
        //            {
        //                rReturn = Datas.ToArray();
        //            }
        //            break;
        //        default:
        //        case NWDDatasFilter.Reachable:
        //            {
        //                rReturn = DatasReachable.ToArray();
        //            }
        //            break;
        //    }
        //    NWEBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass[] GetDatasByInternalKey(string sInternalKey, NWDDatasFilter sFilter)
        //{
        //    NWEBenchmark.Start();
        //    NWDTypeClass[] rReturn = null;
        //    switch (sFilter)
        //    {
        //        case NWDDatasFilter.ReachableAndDisabled:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.EnableState() == false)
        //                        {
        //                            tList.Add(tDatas);
        //                        }
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndTrashed:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.TrashState() == true)
        //                        {
        //                            tList.Add(tDatas);
        //                        }
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndEnable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.EnableState() == true)
        //                        {
        //                            tList.Add(tDatas);
        //                        }
        //                    }
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        case NWDDatasFilter.All:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    tList = DatasByInternalKey[sInternalKey];
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //        default:
        //        case NWDDatasFilter.Reachable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    tList = DatasReachableByInternalKey[sInternalKey];
        //                }
        //                rReturn = tList.ToArray();
        //            }
        //            break;
        //    }
        //    NWEBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass GetFirstDatasByInternalKey(string sInternalKey, NWDDatasFilter sFilter)
        //{
        //    NWEBenchmark.Start();
        //    NWDTypeClass rReturn = null;
        //    switch (sFilter)
        //    {
        //        case NWDDatasFilter.ReachableAndDisabled:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.EnableState() == false)
        //                        {
        //                            rReturn = tDatas;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndTrashed:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.TrashState() == true)
        //                        {
        //                            rReturn = tDatas;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndEnable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
        //                    {
        //                        if (tDatas.EnableState() == true)
        //                        {
        //                            rReturn = tDatas;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.All:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    tList = DatasByInternalKey[sInternalKey];
        //                }
        //                if (tList.Count > 0)
        //                {
        //                    rReturn = tList[0];
        //                }
        //            }
        //            break;
        //        default:
        //        case NWDDatasFilter.Reachable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
        //                {
        //                    tList = DatasReachableByInternalKey[sInternalKey];
        //                }
        //                if (tList.Count > 0)
        //                {
        //                    rReturn = tList[0];
        //                }
        //            }
        //            break;
        //    }
        //    NWEBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass GetDataByReference(string sReference, NWDDatasFilter sFilter)
        //{
        //    // TODO
        //    NWEBenchmark.Start();
        //    NWDTypeClass rReturn = null;
        //    switch (sFilter)
        //    {
        //        case NWDDatasFilter.ReachableAndDisabled:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByReference.ContainsKey(sReference))
        //                {
        //                    rReturn = DatasReachableByReference[sReference];
        //                    if (rReturn.EnableState() == true)
        //                    {
        //                        rReturn = null;
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndTrashed:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByReference.ContainsKey(sReference))
        //                {
        //                    rReturn = DatasReachableByReference[sReference];
        //                    if (rReturn.TrashState() == false)
        //                    {
        //                        rReturn = null;
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.ReachableAndEnable:
        //            {
        //                List<NWDTypeClass> tList = new List<NWDTypeClass>();
        //                if (DatasReachableByReference.ContainsKey(sReference))
        //                {
        //                    rReturn = DatasReachableByReference[sReference];
        //                    if (rReturn.EnableState() == false)
        //                    {
        //                        rReturn = null;
        //                    }
        //                }
        //            }
        //            break;
        //        case NWDDatasFilter.All:
        //            {
        //                if (DatasByReference.ContainsKey(sReference))
        //                {
        //                    rReturn = DatasByReference[sReference];
        //                }
        //            }
        //            break;
        //        default:
        //        case NWDDatasFilter.Reachable:
        //            {
        //                if (DatasReachableByReference.ContainsKey(sReference))
        //                {
        //                    rReturn = DatasReachableByReference[sReference];
        //                }
        //            }
        //            break;
        //    }
        //    NWEBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static string SynchronizeKeyData = "data";
        //public static string SynchronizeKeyDataCount = "rowCount";
        //public static string SynchronizeKeyTimestamp = "sync";
        //public static string SynchronizeKeyLastTimestamp = "last";
        //public static string SynchronizeKeyInWaitingTimestamp = "waiting";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================