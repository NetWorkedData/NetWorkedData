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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void New_IndexAll()
        {
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.Index();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_RepaintTableEditor()
        {
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_RepaintInspectorEditor()
        {
            NWDDataInspector.ActiveRepaint();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_ChangeAssetPath(string sOldPath, string sNewPath)
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
        public NWDTypeClass New_GetDataByReference(string sReference)
        {
            NWDTypeClass rReturn = null;
            if (DatasByReference.ContainsKey(sReference))
            {
                rReturn = DatasByReference[sReference];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> New_ClasseInThisSync()
        {
            List<Type> rReturn = New_OverrideClasseInThisSync();
            //rReturn = new List<Type>(rReturn.Distinct<Type>());
            Debug.Log("New_ClasseInThisSync calculate : " + string.Join(" ", rReturn));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDHelper<K> : NWDBasisHelper where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public K FictiveData()
        {
            return NWDBasis<K>.FictiveData();
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type> New_OverrideClasseInThisSync()
        {
            List<Type> rReturn = new List<Type> { typeof(K) };
            Debug.Log("New_OverrideClasseInThisSync first override : " + string.Join(" ", rReturn));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public override void New_LoadFromDatabase()
        //        {

        //#if UNITY_EDITOR
        //            RowAnalyzed = false;
        //#endif
        //            ResetDatas();
        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (kAccountDependent)
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }
        //            if (tSQLiteConnection != null)
        //            {
        //                if (tSQLiteConnection.IsValid())
        //                {
        //                    List<K> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + ClassNamePHP);
        //                    int tCount = 0;
        //                    // Prepare the datas
        //                    if (tSelect != null)
        //                    {
        //                        foreach (NWDBasis<K> tItem in tSelect)
        //                        {
        //                            tCount++;
        //                            tItem.LoadedFromDatabase();
        //                        }
        //                    }
        //                }
        //            }
        //            //Debug.Log("NWDBasis<K> LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
        //#if UNITY_EDITOR
        //            New_FilterTableEditor();
        //            New_RepaintTableEditor();
        //#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisHelper()
        {
            //Debug.Log("NWDDatas Static Class Constructor()");
            NWDTypeLauncher.Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ClassLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassType = null;
        public Type ConnexionType = null;
        public string ClassName = string.Empty;
        public string ClassNamePHP = string.Empty;
        public bool ClassSynchronize;
        public string ClassTrigramme = string.Empty;
        public string ClassDescription = string.Empty;
        public string ClassMenuName = string.Empty;
        public string ClassTableName = string.Empty;
        public string ClassPrefBaseKey = string.Empty;
        public GUIContent ClassMenuNameContent = null;
        //-------------------------------------------------------------------------------------------------------------
        public bool kLockedObject; // false if account dependant but bypass in editor mode (allways false to authorize sync)
        //-------------------------------------------------------------------------------------------------------------
        public bool ClassGameSaveDependent = false;
        public PropertyInfo ClassGameDependentProperties;
        public MethodInfo GameSaveMethod;
        //-------------------------------------------------------------------------------------------------------------
        public bool kAccountDependent = false;
        public PropertyInfo[] kAccountDependentProperties;
        public PropertyInfo[] kAccountConnectedProperties;
        public Dictionary<PropertyInfo, MethodInfo> AccountMethodDico = new Dictionary<PropertyInfo, MethodInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public bool kAssetDependent;
        public PropertyInfo[] kAssetDependentProperties;
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
        public List<MethodInfo> IndexInsertMethodList = new List<MethodInfo>();
        public List<MethodInfo> IndexRemoveMethodList = new List<MethodInfo>();
        //-------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //public List<string> ObjectsInEditorTableKeyList = new List<string>();
        //public List<string> ObjectsInEditorTableList = new List<string>();
        //public List<bool> ObjectsInEditorTableSelectionList = new List<bool>();
        //-------------------------------------------------------------------------------------------------------------
#endif




        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<Type, NWDBasisHelper> TypesDictionary = new Dictionary<Type, NWDBasisHelper>();
        public static Dictionary<string, NWDBasisHelper> StringsDictionary = new Dictionary<string, NWDBasisHelper>();

        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper Declare(Type sType)
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDDatas Declare for " + sType.Name + " !");

            NWDBasisHelper tTypeInfos = null;

            bool tServerSynchronize = true;
            if (sType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true).Length > 0)
            {
                NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)sType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true)[0];
                tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
            }
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


            if (sType.IsSubclassOf(typeof(NWDTypeClass)))
            {

                //BTBBenchmark.Start("Declare() step A");
                // find infos object if exists or create 
                if (TypesDictionary.ContainsKey(sType))
                {
                    Debug.LogWarning(sType.Name + " allready in TypesDictionary");
                    tTypeInfos = TypesDictionary[sType];
                }
                else
                {
                    //TODO : find all NWDHelper and generic compare!
                    //Debug.Log("sType.Name + Helper = " + sType.Name + "Helper");
                    //Type tTypeHelper = Type.GetType("NetWorkedData." + sType.Name + "Helper");

                    Type tTypeHelper = null;
                    Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                    Type[] tAllHelperDTypes = (from System.Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisHelper)) select type).ToArray();
                    foreach (Type tPossibleHelper in tAllHelperDTypes)
                    {
                        if (tPossibleHelper.ContainsGenericParameters == false)
                        {
                            if (tPossibleHelper.BaseType.GenericTypeArguments.Contains(sType))
                            {
                                tTypeHelper = tPossibleHelper;
                                break;
                            }
                        }
                    }
                    if (tTypeHelper != null)
                    {
                        tTypeInfos = Activator.CreateInstance(tTypeHelper) as NWDBasisHelper;
                        //Debug.Log("<color=green> YES </color> create "+ tTypeInfos.GetType().Name + " instance for " + sType.Name + " ");
                    }
                    else
                    {
                        tTypeInfos = new NWDBasisHelper();
                        //Debug.Log("<color=red> NO </color> create "+ tTypeInfos.GetType().Name + " instance for " + sType.Name + " ");
                    }
                    TypesDictionary.Add(sType, tTypeInfos);
                }
                // insert basic infos
                tTypeInfos.ClassType = sType;
                tTypeInfos.ClassTableName = sType.Name;

                tTypeInfos.ClassName = sType.AssemblyQualifiedName;
                //BTBBenchmark.Finish("Declare() step A");
                //BTBBenchmark.Start("Declare() step B");

                TableMapping tTableMapping = new TableMapping(sType);
                string rClassName = tTableMapping.TableName;
                tTypeInfos.ClassNamePHP = rClassName;
                tTypeInfos.ClassPrefBaseKey = tTypeInfos.ClassNamePHP + "_";
                if (StringsDictionary.ContainsKey(rClassName))
                {
                    Debug.LogWarning(rClassName + " allready in StringsDictionary!");
                }
                else
                {
                    StringsDictionary.Add(rClassName, tTypeInfos);
                }
                //BTBBenchmark.Finish("Declare() step B");
                //BTBBenchmark.Start("Declare() step C");
                // insert attributs infos
                tTypeInfos.ClassTrigramme = tClassTrigramme;
                tTypeInfos.ClassMenuName = tMenuName;
                tTypeInfos.ClassDescription = tDescription;
                tTypeInfos.ClassSynchronize = tServerSynchronize;

                //foreach (MethodInfo tMethod in sType.GetMethods(BindingFlags.Instance))
                foreach (MethodInfo tMethod in sType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                {
                    //if (sType.Name == "NWDItem")
                    //{
                    //    Debug.Log("<color=blue>tMethod</color> " + tMethod.Name + " "+ tMethod.GetCustomAttributes(typeof(NWDIndexInsert), true).Length);
                    //}
                    if (tMethod.GetCustomAttributes(typeof(NWDIndexInsert), true).Length > 0)
                    {
                        tTypeInfos.IndexInsertMethodList.Add(tMethod);
                    }
                    if (tMethod.GetCustomAttributes(typeof(NWDIndexRemove), true).Length > 0)
                    {
                        tTypeInfos.IndexRemoveMethodList.Add(tMethod);
                    }
                }
                //BTBBenchmark.Finish("Declare() step C");
                //BTBBenchmark.Start("Declare() step D");
                // create GUI object
#if UNITY_EDITOR
                // tTypeInfos.ClassMenuNameContent = new GUIContent(sMenuName, tTypeInfos.TextureOfClass(), sDescription);
#endif
                //BTBBenchmark.Finish("Declare() step D");
                //BTBBenchmark.Start("Declare() step E");
                // Prepare engine informlations
                //tTypeInfos.ClassPrefBaseKey = sType.Name + "_";
                //tTypeInfos.PropertiesArrayPrepare();
                //tTypeInfos.PropertiesOrderArrayPrepare();
                //tTypeInfos.SLQAssemblyOrderArrayPrepare();
                //tTypeInfos.SLQAssemblyOrderPrepare();
                //tTypeInfos.SLQIntegrityOrderPrepare();
                //tTypeInfos.DataAssemblyPropertiesListPrepare();

                //BTBBenchmark.Finish("Declare() step E");
                //BTBBenchmark.Start("Declare() step F");
                // get salt 
                tTypeInfos.PrefLoad();
                //BTBBenchmark.Finish("Declare() step F");
                //BTBBenchmark.Finish();









                //BTBBenchmark.Start();
                bool rAccountConnected = false;
                bool rAssetConnected = false;
                bool rLockedObject = true;
                List<PropertyInfo> tPropertyList = new List<PropertyInfo>();
                List<PropertyInfo> tPropertyListConnected = new List<PropertyInfo>();
                List<PropertyInfo> tAssetPropertyList = new List<PropertyInfo>();
                Dictionary<PropertyInfo, MethodInfo> tAccountMethodList = new Dictionary<PropertyInfo, MethodInfo>();


                tTypeInfos.ClassGameSaveDependent = false;
                tTypeInfos.ClassGameDependentProperties = null;
                tTypeInfos.GameSaveMethod = null;
                // exception for NWDAccount table
                if (sType == typeof(NWDAccount) || sType == typeof(NWDRequestToken))
                {
                    rAccountConnected = true;
                }

                foreach (PropertyInfo tProp in sType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
                                    tTypeInfos.ClassGameSaveDependent = true;
                                    tTypeInfos.ClassGameDependentProperties = tProp;
                                    MethodInfo tGameSaveMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
                                    tTypeInfos.GameSaveMethod = tGameSaveMethod;
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

                tTypeInfos.kAccountDependent = rAccountConnected;
                // reccord class' object is account dependent properties
                tTypeInfos.kAccountDependentProperties = tPropertyList.ToArray();

                // reccord class' object is account connected properties
                tTypeInfos.kAccountConnectedProperties = tPropertyListConnected.ToArray();
                tTypeInfos.AccountMethodDico = tAccountMethodList;

                // reccord if class' object is locked for editor

#if UNITY_EDITOR
                rLockedObject = false;
#endif
                tTypeInfos.kLockedObject = rLockedObject;

                // reccord if class' object is asset dependent
                tTypeInfos.kAssetDependent = rAssetConnected;
                tTypeInfos.kAssetDependentProperties = tAssetPropertyList.ToArray();
                //BTBBenchmark.Finish();















                if (NWDDataManager.SharedInstance().mTypeList.Contains(sType) == false)
            {
                NWDDataManager.SharedInstance().mTypeList.Add(sType);
            }

            if (tTypeInfos.kAccountDependent)
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

            if (tServerSynchronize == true)
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(sType) == false)
                {
                    NWDDataManager.SharedInstance().mTypeSynchronizedList.Add(sType);
                }
                if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(sType) == true)
                {
                    NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Remove(sType);
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
            }
            if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey(tClassTrigramme))
            {
                Debug.LogWarning("ERROR in " + sType.AssemblyQualifiedName + ", this trigramme '" + tClassTrigramme + "' is already use by another class! (" + NWDDataManager.SharedInstance().mTrigramTypeDictionary[tClassTrigramme] + ")");
            }
            else
            {
                NWDDataManager.SharedInstance().mTrigramTypeDictionary.Add(tClassTrigramme, sType);
            }
            NWDDataManager.SharedInstance().mTypeLoadedList.Add(sType);

                tTypeInfos.New_ClassInitialization();

                tTypeInfos.ClassLoaded = true;
            }

            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void PrefSave()
        //{
        //    NWDAppConfiguration.SharedInstance().SetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltAKey, SaltA);
        //    NWDAppConfiguration.SharedInstance().SetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltBKey, SaltB);
        //    NWDAppConfiguration.SharedInstance().SetSaltValid(ClassPrefBaseKey, NWDConstants.kPrefSaltValidKey, "ok");
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void PrefLoad()
        {
            if (string.IsNullOrEmpty(SaltStart) || string.IsNullOrEmpty(SaltEnd))
            {
                //Debug.Log("Generate Salt for " + ClassNamePHP);
                SaltStart = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                SaltEnd = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                SaltValid = false;
            }
            //SaltA = NWDAppConfiguration.SharedInstance().GetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltAKey, NWDConstants.kPrefSaltValidKey);
            //SaltB = NWDAppConfiguration.SharedInstance().GetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltBKey, NWDConstants.kPrefSaltValidKey);
            //SaltOk = NWDAppConfiguration.SharedInstance().GetSaltValid(ClassPrefBaseKey, NWDConstants.kPrefSaltValidKey);

            //SaltStart = SaltA;
            //SaltEnd = SaltB;
            //SaltValid = true;
        }

        //-------------------------------------------------------------------------------------------------------------
        //public void SaltRegenerate()
        //{
        //    PrefLoad();
        //    PrefSave();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public bool TestSaltValid()
        //{
        //    bool rReturn = false;
        //    if (SaltOk == "ok")
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
        public static NWDBasisHelper FindTypeInfos(Type sType)
        {
            NWDBasisHelper tTypeInfos = null;
            //if (sType.IsSubclassOf(typeof(NWDTypeClass)))
            //{
                if (TypesDictionary.ContainsKey(sType))
                {
                    tTypeInfos = TypesDictionary[sType];
                }
            //}
            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper FindTypeInfos(string sTypeName)
        {
            NWDBasisHelper tTypeInfos = null;
            if (StringsDictionary.ContainsKey(sTypeName))
            {
                tTypeInfos = StringsDictionary[sTypeName];
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
            "ServerSynchronize = '" + ClassSynchronize + "' " +
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
        public Dictionary<string, NWDTypeClass> DatasByReference = new Dictionary<string, NWDTypeClass>();
        /// <summary>
        /// The datas by internal key. Return list of datas.
        /// </summary>
        public Dictionary<string, List<NWDTypeClass>> DatasByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
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
            //Debug.Log("ResetDatas()");
            //BTBBenchmark.Start();
            // all datas prepare handler
            Datas = new List<NWDTypeClass>();
            DatasByReference = new Dictionary<string, NWDTypeClass>();
            DatasByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
            DatasByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
            // reachable datas prepare handler
            //DatasReachable = new List<NWDTypeClass>();
            //DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
            //DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
            //DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
#if UNITY_EDITOR
            // editor datas prepare handler
            //DatasInEditorRowDescriptionList = new List<string>();
            //DatasInEditorSelectionList = new List<bool>();
            //DatasInEditorReferenceList = new List<string>();

            EditorTableDatas = new List<NWDTypeClass>();
            EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();

            // use in pop menu in edition of NWD inspector...
            EditorDatasMenu = new Dictionary<string, string>();
            EditorDatasMenu.Add("---", string.Empty);
#endif
            //BTBBenchmark.Finish();
        }


        //-------------------------------------------------------------------------------------------------------------
        public bool DatabaseIsLoaded()
        {
            bool rLoaded = true;
            if (kAccountDependent == true && NWDDataManager.SharedInstance().DataAccountLoaded == false)
            {
                rLoaded = false;
            }
            else if (kAccountDependent == false && NWDDataManager.SharedInstance().DataEditorLoaded == false)
            {
                rLoaded = false;
            }
            return rLoaded;
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
        //    BTBBenchmark.Start();
        //    if (sData.ReachableState() == true)
        //    {
        //        string tReference = sData.ReferenceUsedValue();
        //        // Anyway I check if Data is allready in datalist
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
        //    BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void AddData(NWDTypeClass sData)
        {
            //Debug.Log("NWDDatas AddData()");
            //BTBBenchmark.Start();
            // get reference
            string tReference = sData.Reference;
            // Anyway I check if Data is allready in datalist
            if (DatasByReference.ContainsKey(tReference) == false)
            {
                //Debug.Log("NWDDatas AddData() add data");
                // get internal key
                string tInternalKey = sData.InternalKey;
                // Anyway I add Data in datalist
                Datas.Add(sData);
                DatasByReference.Add(tReference, sData);
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

                // Ok now I check if I need to install it in reachable data
                //bool tDataIsValid = sData.DataIntegrityState();
                //if (tDataIsValid == true)
                //{
                //    AddDataReachable(sData);
                //}
                //else
                //{
                //    Debug.LogWarning("Try to add not integrity data!");
                //}

                //Debug.Log("NWDDatas AddData() add data Datas count = " + Datas.Count);
            }
            else
            {
                //Debug.LogWarning("Try to add twice data!");
            }
            // Ok now I add datas in editor table list
#if UNITY_EDITOR
            // add load object in editor table
            //if (DatasInEditorReferenceList.Contains(tReference) == false)
            //{
            //    // Active to auto remove on filter
            //    // if (sObject.Tag == (int)m_SearchTag)
            //    {
            //        DatasInEditorReferenceList.Add(tReference);
            //    }
            //}
            //DatasInEditorRowDescriptionList.Add(tInternalKey + " <" + tReference + ">");
            //DatasInEditorSelectionList.Add(false);


            /*NEW*/
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
            /*NEW*/


#endif
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void RemoveDataReachable(NWDTypeClass sData)
        //{
        //    BTBBenchmark.Start();
        //    string tReference = sData.ReferenceUsedValue();
        //    // Anyway I check if Data is allready in datalist
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
        //    BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(NWDTypeClass sData)
        {
            //Debug.Log("NWDDatas RemoveData()");
            //BTBBenchmark.Start();
            // get reference
            string tReference = sData.Reference;
            // Anyway I check if Data is allready in datalist
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
            //BTBBenchmark.Finish();
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
        //    BTBBenchmark.Start();
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
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass[] GetDatasByInternalKey(string sInternalKey, NWDDatasFilter sFilter)
        //{
        //    BTBBenchmark.Start();
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
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass GetFirstDatasByInternalKey(string sInternalKey, NWDDatasFilter sFilter)
        //{
        //    BTBBenchmark.Start();
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
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass GetDataByReference(string sReference, NWDDatasFilter sFilter)
        //{
        //    // TODO
        //    BTBBenchmark.Start();
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
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------

        //public static string SynchronizeKeyData = "data";
        //public static string SynchronizeKeyDataCount = "rowCount";
        //public static string SynchronizeKeyTimestamp = "sync";
        //public static string SynchronizeKeyLastTimestamp = "last";
        //public static string SynchronizeKeyInWaitingTimestamp = "waiting";
        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
//#if UNITY_EDITOR
//        public static Dictionary<string, string> EditorDatasMenu()
//        {
//            return BasisHelper().EditorDatasMenu;
//        }
//#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// News the get all datas. IT S A GLOBAL ACCESS!!!!
        /// </summary>
        /// <returns>The get all datas.</returns>
        /// AllDatas
        //public static K[] NEW_GetAllDatas()
        //{
        //    return Datas().Datas.ToArray() as K[];
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// News the get all datas. IT S A GLOBAL ACCESS!!!!
        /// </summary>
        /// <returns>The get all datas.</returns>
        //private static K[] NEW_GetAllDatasByInternalKey(string sInternalKey)
        //{
        //    K[] rReturn = new K[0];
        //    if (Datas().DatasByInternalKey.ContainsKey(sInternalKey) == true)
        //    {
        //        rReturn = Datas().DatasByInternalKey[sInternalKey].ToArray() as K[];
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// News the get data by reference. IT S A GLOBAL ACCESS!!!!
        /// </summary>
        /// <returns>The get data by reference.</returns>
        /// <param name="sReference">S reference.</param>
        public static K GetDataByReference(string sReference)
        {
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                // TODO : fait lagguer la connection : trouver une solution
                //rReturn = LoadDataByReference(sReference);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// News the get data by reference.
        /// </summary>
        /// <returns>The get data by reference.</returns>
        /// <param name="sReference">S reference.</param>
        public static K FindDataByReference(string sReference, string sAccountReference = null)
        {
            K rReturn = null;
            if (string.IsNullOrEmpty(sReference) == false)
            {
                if (string.IsNullOrEmpty(sAccountReference))
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                if (BasisHelper().DatasByReference != null)
                {
                    if (BasisHelper().DatasByReference.ContainsKey(sReference))
                    {
                        K tObject = BasisHelper().DatasByReference[sReference] as K;
                        if (tObject.IsReacheableByAccount(sAccountReference))
                        {
                            rReturn = tObject;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static K[] AllDatas()
        {
            //BTBBenchmark.Start();
            K[] rReturn = BasisHelper().Datas.ToArray() as K[];
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static K[] FindDatas(string sAccountReference = null, // use default account
                                NWDGameSave sGameSave = null, // use default gamesave
                                NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                )
        {
            //BTBBenchmark.Start();
            //Debug.Log("Datas() Datas count = " + Datas().Datas.Count);
            K[] rReturn = FilterDatas(BasisHelper().Datas, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static K[] FilterDatas(List<NWDTypeClass> sDatasArray,
                                string sAccountReference = null, // use default account
                                NWDGameSave sGameSave = null,// use default gamesave
                                NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                )
        {
            List<K> rList = new List<K>();
            //Debug.Log("chercher les data ");
            if (sDatasArray != null)
            {
                if (BasisHelper().kAccountDependent)
                {
                    // autofill sAccountReference if necessary
                    if (string.IsNullOrEmpty(sAccountReference))
                    {
                        sAccountReference = NWDAccount.CurrentReference();
                    }
                    //Debug.Log("chercher les data pour " + sAccountReference + " ");
                }
                if (BasisHelper().ClassGameSaveDependent)
                {
                    if (sGameSave == null)
                    {
                        sGameSave = NWDGameSave.SelectCurrentDataForAccount(sAccountReference);
                    }
                    //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
                }


                foreach (K tDatas in sDatasArray)
                {
                    bool tInsert = true;

                    switch (sTrashed)
                    {
                        case NWDSwitchTrashed.NoTrashed:
                            {
                                if (tDatas.IsTrashed() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchTrashed.Trashed:
                            {
                                if (tDatas.IsTrashed() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }

                    switch (sEnable)
                    {
                        case NWDSwitchEnable.Disable:
                            {
                                if (tDatas.IsEnable() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchEnable.Enable:
                            {
                                if (tDatas.IsEnable() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }

                    switch (sIntegrity)
                    {
                        case NWDSwitchIntegrity.Cracked:
                            {
                                if (tDatas.TestIntegrity() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchIntegrity.Integrity:
                            {
                                if (tDatas.TestIntegrity() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }
                    if (tInsert == true)
                    {
                        if (BasisHelper().kAccountDependent)
                        {
                            // test game save if necessary
                            if (BasisHelper().GameSaveMethod != null && sGameSave != null)
                            {
                                string tGameIndex = sGameSave.Reference;
                                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(tDatas, null);
                                if (tValue == null)
                                {
                                    tValue = string.Empty;
                                }
                                string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                                if (tSaveIndex != tGameIndex)
                                {
                                    tInsert = false;
                                }
                            }
                            if (tInsert == true)
                            {
                                tInsert = false; // research by default false and true when found first solution
                                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                                {
                                    var tValue = tInfos.Key.GetValue(tDatas, null);
                                    string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
                                    if (tAccountValue.Contains(sAccountReference))
                                    {
                                        tInsert = true;
                                        break; // I fonud one solution! this user can see this informations
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (tInsert == true)
                    {
                        rList.Add(tDatas);
                    }
                }
            }
            else
            {
                //Debug.Log("chercher les data a un tableau vide");
            }
            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K FindFirstDataByInternalKey(
                                        string sInternalKey,
                                        bool sCreateIfNotExists = false,
                                         NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal,
                                        string sAccountReference = null,
                                        NWDGameSave sGameSave = null,
                                        NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                        NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                        NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                       )
        {
            K rReturn = null;

            K[] rDatas = FindDatasByInternalKey(sInternalKey, sCreateIfNotExists, sWritingMode, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] FindDatasByInternalKey(
                                        string sInternalKey,
                                        bool sCreateIfNotExists = false,
            NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal,
                                        string sAccountReference = null,
                                        NWDGameSave sGameSave = null,
                                        NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                        NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                        NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                       )
        {
            List<NWDTypeClass> tTestList = new List<NWDTypeClass>();
            sInternalKey = NWDToolbox.TextProtect(sInternalKey);
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey) == true)
            {
                tTestList.AddRange(BasisHelper().DatasByInternalKey[sInternalKey]);
            }
            if (BasisHelper().kAccountDependent)
            {
                // autofill sAccountReference if necessary
                if (string.IsNullOrEmpty(sAccountReference))
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " ");
            }
            if (BasisHelper().ClassGameSaveDependent)
            {
                if (sGameSave == null)
                {
                    sGameSave = NWDGameSave.SelectCurrentDataForAccount(sAccountReference);
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
            }

            K[] rArray = FilterDatas(tTestList, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            if (sCreateIfNotExists == true && rArray.Length == 0)
            {
                //Debug.Log(" must create object !");
                if (sAccountReference == null || sAccountReference == NWDAccount.CurrentReference())
                {
                    if (sGameSave == NWDGameSave.CurrentData())
                    {
                        //Debug.Log("Creat Ok");
                        K rReturn = NewData(sWritingMode);
                        rReturn.InternalKey = sInternalKey;
                        rReturn.UpdateData(true, sWritingMode);
                        rArray = new K[1] { rReturn };
                    }
                    else
                    {
                        Debug.Log("create not possinble in another gamesave!");
                    }
                }
                else
                {
                    Debug.Log("create not possible with another account!");
                }
            }
            return rArray;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_IndexAll)]
        //public static void IndexAll()
        //{
        //    NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
        //    foreach (NWDTypeClass tObject in tTypeInfos.Datas)
        //    {
        //        tObject.Index();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        [NWDAliasMethod(NWDConstants.M_LoadFromDatabase)]
        //        public static void LoadFromDatabase()
        //        {
        //            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
        //            tTypeInfos = BasisHelper();
        //#if UNITY_EDITOR
        //            tTypeInfos.RowAnalyzed = false;
        //#endif
        //            tTypeInfos.ResetDatas();
        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (AccountDependent())
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }
        //            if (tSQLiteConnection != null)
        //            {
        //                if (tSQLiteConnection.IsValid())
        //                {
        //                    List<K> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + tTypeInfos.ClassNamePHP);
        //                    int tCount = 0;
        //                    // Prepare the datas
        //                    if (tSelect != null)
        //                    {
        //                        foreach (NWDBasis<K> tItem in tSelect)
        //                        {
        //                            tCount++;
        //                            tItem.LoadedFromDatabase();
        //                        }
        //                    }
        //                }
        //            }
        //            //Debug.Log("NWDBasis<K> LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
        //#if UNITY_EDITOR
        //            BasisHelper().New_FilterTableEditor();
        //            BasisHelper().New_RepaintTableEditor();
        //#endif
        //    //BTBBenchmark.Finish("LoadFromDatabase " + tTypeInfos.ClassNamePHP);
        //}
        //-------------------------------------------------------------------------------------------------------------
//        public static void UnloadDataByReference(string sReference)
//        {
//            //Debug.Log("UnloadDataByReference(" + sReference + ")");
//            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
//            tTypeInfos = BasisHelper();
//            if (tTypeInfos.DatasByReference.ContainsKey(sReference))
//            {
//                NWDTypeClass tData = tTypeInfos.DatasByReference[sReference];
//                tData.Desindex(); // call override method
//                tTypeInfos.RemoveData(tData);
//                tData.Delete();
//            }
//#if UNITY_EDITOR
//            BasisHelper().New_FilterTableEditor();
//            BasisHelper().New_RepaintTableEditor();
//#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static K LoadDataByReference(string sReference)
        {
            Debug.Log("LoadDataByReference(" + sReference + ")");
            BTBBenchmark.Start();
            K rReturn = null;
            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
            tTypeInfos = BasisHelper();
            if (tTypeInfos.DatasByReference.ContainsKey(sReference) == false)
            {
                SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
                if (AccountDependent())
                {
                    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
                }
                if (tSQLiteConnection != null)
                {
                    if (tSQLiteConnection.IsValid())
                    {
                        List<K> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "` = '" + sReference + "';");
                        if (tSelect != null)
                        {
                            foreach (NWDBasis<K> tItem in tSelect)
                            {
                                rReturn = tItem as K;
                                tItem.LoadedFromDatabase();
#if UNITY_EDITOR
                                tItem.RowAnalyze();
#endif
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().New_FilterTableEditor();
            BasisHelper().New_RepaintTableEditor();
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadDataToSync(NWDAppEnvironment sEnvironment)
        {
            BTBBenchmark.Start();
            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
            tTypeInfos = BasisHelper();
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (tSQLiteConnection != null)
            {
                if (tSQLiteConnection.IsValid())
                {
                    //SQLiteCommand tCommand = tSQLiteConnection.CreateCommand("SELECT `Reference` FROM " + tTypeInfos.ClassNamePHP + " WHERE `"+ sEnvironment.Environment + "Sync` = '0' OR `"+ sEnvironment.Environment + "Sync` = '1';");
                    //List<string> tSelect = tCommand.ExecuteQuery<string>();
                    string tQuery = "SELECT `" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "` FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + sEnvironment.Environment + "Sync` = '0' OR `" + sEnvironment.Environment + "Sync` = '1';";
                    //Debug.Log(tQuery);
                    SQLiteCommand tCreateCommand = tSQLiteConnection.CreateCommand(tQuery);
                    List<NWDTypeClassReference> tSelect = tCreateCommand.ExecuteQuery<NWDTypeClassReference>();
                    if (tSelect != null)
                    {
                        foreach (NWDTypeClassReference tReference in tSelect)
                        {
                            //Debug.Log("tReference = " + tReference.Reference);
                            if (tReference.Reference != null)
                            {
                                GetDataByReference(tReference.Reference);
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().New_FilterTableEditor();
            BasisHelper().New_RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool DataIntegrityState()
        {
            return TestIntegrity();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool TrashState()
        //{
        //    if (XX > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void TrashAction()
        {
            TrashData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateIntegrityAction()
        {
            //UpdateIntegrity();
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool EnableState()
        //{
        //    return AC;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public override bool ReachableState()
        //{
        //    return IsReacheableByAccount();
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================