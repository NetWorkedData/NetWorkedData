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
    public class NWDDatas
    {
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassType = null;
        public string ClassName = "";
        public string ClassNamePHP = "";
        public bool ClassSynchronize;
        public string ClassTrigramme = "";
        public string ClassDescription = "";
        public string ClassMenuName = "";
        public string ClassTableName = "";
        public string ClassPrefBaseKey = "";
        public GUIContent ClassMenuNameContent = GUIContent.none;
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
        public string SaltA = "";
        public string SaltB = "";
        public string SaltOk = "";

        //public List<object> ObjectsList = new List<object>();
        //public List<string> ObjectsByReferenceList = new List<string>();
        //public List<string> ObjectsByKeyList = new List<string>();




        //-------------------------------------------------------------------------------------------------------------
        public bool mSettingsShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        //public Object kObjectInEdition;
        //public Object[] kObjectsArrayInEdition;
        //-------------------------------------------------------------------------------------------------------------
        private Texture2D Texture = null;
        public Texture2D TextureOfClass()
        {
#if UNITY_EDITOR
            if (Texture == null)
            {
                Texture2D rTexture = null;
                string[] sGUIDs = AssetDatabase.FindAssets("" + ClassNamePHP + " t:texture2D");
                foreach (string tGUID in sGUIDs)
                {
                    //Debug.Log("TextureOfClass GUID " + tGUID);
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    //Debug.Log("tPathFilename = " + tPathFilename);
                    if (tPathFilename.Equals(ClassNamePHP))
                    {
                        //Debug.Log("TextureOfClass " + tPath);
                        rTexture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    }
                }
                Texture = rTexture;
                // if null  the draw default
                if (Texture == null)
                {
                    Texture = NWDConstants.kImageRed;
                }
            }
#endif
            return Texture;
        }

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 ObjectEditorScrollPosition = Vector2.zero;
        public bool kSyncAndMoreInformations = false;


        public string m_SearchReference = "";
        public string m_SearchInternalName = "";
        public string m_SearchInternalDescription = "";

        public string m_SearchAccount = "";
        public string m_SearchGameSave = "";
        public NWDBasisTag m_SearchTag = NWDBasisTag.NoTag;

        public Vector2 m_ScrollPositionList;

        public int m_ItemPerPage = 10;

        public int m_ItemPerPageSelection = 0;

        public string[] m_ItemPerPageOptions = new string[] {
            "15", "20", "30", "40", "50", "100", "200",
        };
        public int m_PageSelected = 0;
        public int m_MaxPage = 0;

        public bool m_ShowEnable = true;
        public bool m_ShowDisable = true;
        public bool m_ShowTrashed = true;
        public bool m_ShowIntegrityError = true;

        public Vector2 m_ScrollPositionCard;
        public bool mSearchShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        //public List<string> ObjectsInEditorTableKeyList = new List<string>();
        //public List<string> ObjectsInEditorTableList = new List<string>();
        //public List<bool> ObjectsInEditorTableSelectionList = new List<bool>();
        //-------------------------------------------------------------------------------------------------------------
#endif




        //-------------------------------------------------------------------------------------------------------------
        public NWDDatas()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<Type, NWDDatas> TypesDictionary = new Dictionary<Type, NWDDatas>();

        //-------------------------------------------------------------------------------------------------------------
        public static void Declare(Type sType, bool sClassSynchronize, string sTrigrammeName, string sMenuName, string sDescription)
        {
            //Debug.Log("NWDDatas Declare for " + sType.Name + " !");
            if (sType.IsSubclassOf(typeof(NWDTypeClass)))
            {
                // find infos object if exists or create 
                NWDDatas tTypeInfos = null;
                if (TypesDictionary.ContainsKey(sType))
                {
                    tTypeInfos = TypesDictionary[sType];
                }
                else
                {
                    tTypeInfos = new NWDDatas();
                    TypesDictionary.Add(sType, tTypeInfos);
                }
                // insert basic infos
                tTypeInfos.ClassType = sType;
                tTypeInfos.ClassTableName = sType.Name;
                tTypeInfos.ClassPrefBaseKey = tTypeInfos.ClassTableName + "_";

                tTypeInfos.ClassName = sType.AssemblyQualifiedName;

                TableMapping tTableMapping = new TableMapping(sType);
                string rClassName = tTableMapping.TableName;
                tTypeInfos.ClassNamePHP = rClassName;


                // insert attributs infos
                tTypeInfos.ClassTrigramme = sTrigrammeName;
                tTypeInfos.ClassMenuName = sMenuName;
                tTypeInfos.ClassDescription = sDescription;
                tTypeInfos.ClassSynchronize = sClassSynchronize;
                // create GUI object
                tTypeInfos.ClassMenuNameContent = new GUIContent(sMenuName, tTypeInfos.TextureOfClass(), sDescription);
                // Prepare engine informlations
                tTypeInfos.ClassPrefBaseKey = sType.Name + "_";
                tTypeInfos.PropertiesArrayPrepare();
                tTypeInfos.PropertiesOrderArrayPrepare();
                tTypeInfos.SLQAssemblyOrderArrayPrepare();
                tTypeInfos.SLQAssemblyOrderPrepare();
                tTypeInfos.SLQIntegrityOrderPrepare();
                tTypeInfos.DataAssemblyPropertiesListPrepare();

                // get salt 
                tTypeInfos.PrefLoad();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PrefSave()
        {
            NWDAppConfiguration.SharedInstance().SetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltAKey, SaltA);
            NWDAppConfiguration.SharedInstance().SetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltBKey, SaltB);
            NWDAppConfiguration.SharedInstance().SetSaltValid(ClassPrefBaseKey, NWDConstants.kPrefSaltValidKey, "ok");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PrefLoad()
        {
            SaltA = NWDAppConfiguration.SharedInstance().GetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltAKey, NWDConstants.kPrefSaltValidKey);
            SaltB = NWDAppConfiguration.SharedInstance().GetSalt(ClassPrefBaseKey, NWDConstants.kPrefSaltBKey, NWDConstants.kPrefSaltValidKey);
            SaltOk = NWDAppConfiguration.SharedInstance().GetSaltValid(ClassPrefBaseKey, NWDConstants.kPrefSaltValidKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool TestSaltValid()
        {
            bool rReturn = false;
            if (SaltOk == "ok")
            {
                rReturn = true;
            }
            else
            {
                //Debug.Log ("!!! error in salt memorize : " + ClassNamePHP ());
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDatas FindTypeInfos(Type sType)
        {
            NWDDatas tTypeInfos = null;
            if (sType.IsSubclassOf(typeof(NWDTypeClass)))
            {
                if (TypesDictionary.ContainsKey(sType))
                {
                    tTypeInfos = TypesDictionary[sType];
                }
            }
            return tTypeInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Informations(Type sType)
        {
            string rReturn = "";
            NWDDatas tTypeInfos = FindTypeInfos(sType);
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
            "";
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void AllInfos ()
        //{
        //	foreach (KeyValuePair<Type,NWDTypeInfos> tTypeTypeInfos in TypesDictionary) {
        //		Type tType = tTypeTypeInfos.Key;
        //	}
        //}
        //-------------------------------------------------------------------------------------------------------------
        public PropertyInfo[] PropertiesArray;
        //-------------------------------------------------------------------------------------------------------------
        public void PropertiesArrayPrepare()
        {
            PropertiesArray = ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> PropertiesOrderArray;
        //-------------------------------------------------------------------------------------------------------------
        public void PropertiesOrderArrayPrepare()
        {
            List<string> rReturn = new List<string>();
            foreach (var tProp in PropertiesArray)
            {
                rReturn.Add(tProp.Name);
            }
            rReturn.Sort();
            PropertiesOrderArray = rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] CSVAssemblyOrderArray;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// CSV assembly order array.
        /// </summary>
        /// <returns>The assembly order array.</returns>
        public void CSVAssemblyOrderArrayPrepare()
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray);
            rReturn.Remove("Integrity");
            rReturn.Remove("Reference");
            rReturn.Remove("ID");
            rReturn.Remove("DM");
            rReturn.Remove("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            // add the good order for this element
            rReturn.Insert(0, "Reference");
            rReturn.Insert(1, "DM");
            rReturn.Insert(2, "DS");
            rReturn.Insert(3, "DevSync");
            rReturn.Insert(4, "PreprodSync");
            rReturn.Insert(5, "ProdSync");
            rReturn.Add("Integrity");
            CSVAssemblyOrderArray = rReturn.ToArray<string>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] SLQAssemblyOrderArray;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order array.
        /// </summary>
        /// <returns>The assembly order array.</returns>
        public void SLQAssemblyOrderArrayPrepare()
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray);
            rReturn.Remove("Integrity");
            rReturn.Remove("Reference");
            rReturn.Remove("ID");
            rReturn.Remove("DM");
            rReturn.Remove("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            // add the good order for this element
            rReturn.Insert(0, "DM");
            rReturn.Insert(1, "DS");
            rReturn.Insert(2, "DevSync");
            rReturn.Insert(3, "PreprodSync");
            rReturn.Insert(4, "ProdSync");
            rReturn.Add("Integrity");
            SLQAssemblyOrderArray = rReturn.ToArray<string>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string SLQAssemblyOrder;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public void SLQAssemblyOrderPrepare()
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray);
            rReturn.Remove("Integrity");
            rReturn.Remove("Reference");
            rReturn.Remove("ID");
            rReturn.Remove("DM");
            rReturn.Remove("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            // add the good order for this element
            rReturn.Insert(0, "Reference");
            rReturn.Insert(1, "DM");
            rReturn.Insert(2, "DS");
            rReturn.Insert(3, "DevSync");
            rReturn.Insert(4, "PreprodSync");
            rReturn.Insert(5, "ProdSync");
            rReturn.Add("Integrity");
            SLQAssemblyOrder = "`" + string.Join("`, `", rReturn.ToArray()) + "`";
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> SLQIntegrityOrder;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public void SLQIntegrityOrderPrepare()
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray);
            rReturn.Remove("Integrity");
            rReturn.Remove("Reference");
            rReturn.Remove("ID");
            rReturn.Remove("DM");
            rReturn.Remove("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
            rReturn.Remove("DevSync");
            rReturn.Remove("PreprodSync");
            rReturn.Remove("ProdSync");
            // add the good order for this element
            rReturn.Insert(0, "Reference");
            rReturn.Insert(1, "DM");
            SLQIntegrityOrder = rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> DataAssemblyPropertiesList;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public void DataAssemblyPropertiesListPrepare()
        {
            List<string> rReturn = new List<string>();
            rReturn.AddRange(PropertiesOrderArray);
            rReturn.Remove("Integrity"); // not include in integrity
            rReturn.Remove("Reference");
            rReturn.Remove("ID");
            rReturn.Remove("DM");
            rReturn.Remove("DS");// not include in integrity
            rReturn.Remove("ServerHash");// not include in integrity
            rReturn.Remove("ServerLog");// not include in integrity
            rReturn.Remove("DevSync");// not include in integrity
            rReturn.Remove("PreprodSync");// not include in integrity
            rReturn.Remove("ProdSync");// not include in integrity
            DataAssemblyPropertiesList = rReturn;
        }
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

        public Dictionary<string, string> EditorDatasMenu = new Dictionary<string, string>(); // reference/desciption for menu <REF>

        public List<NWDTypeClass> EditorTableDatas = new List<NWDTypeClass>(); // NWDTypeClass
        public Dictionary<NWDTypeClass, bool> EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
#endif
        // TODO Futur 



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
            EditorDatasMenu.Add("---", "");
#endif
            //BTBBenchmark.Finish();
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
            string tReference = sData.ReferenceUsedValue();
            // Anyway I check if Data is allready in datalist
            if (DatasByReference.ContainsKey(tReference) == false)
            {
                //Debug.Log("NWDDatas AddData() add data");
                // get internal key
                string tInternalKey = sData.InternalKeyValue();
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
            if (EditorDatasMenu.ContainsKey(sData.ReferenceUsedValue()) == false)
            {
                EditorDatasMenu.Add(sData.ReferenceUsedValue(), sData.DatasMenu());
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
            string tReference = sData.ReferenceUsedValue();
            // Anyway I check if Data is allready in datalist
            if (DatasByReference.ContainsKey(tReference) == true)
            {
                // get internal key
                string tInternalKey = sData.InternalKeyValue();
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
            string tReference = sData.ReferenceUsedValue();
            string tInternalKey = sData.InternalKeyValue();
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
                EditorDatasMenu[sData.ReferenceUsedValue()] = sData.DatasMenu();
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
#if UNITY_EDITOR
        public static Dictionary<string, string> EditorDatasMenu()
        {
            return Datas().EditorDatasMenu;
        }
#endif
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
            if (Datas().DatasByReference.ContainsKey(sReference))
            {
                rReturn = Datas().DatasByReference[sReference] as K;
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
                    sAccountReference = NWDAccount.GetCurrentAccountReference();
                }
                if (Datas().DatasByReference != null)
                {
                    if (Datas().DatasByReference.ContainsKey(sReference))
                    {
                        K tObject = Datas().DatasByReference[sReference] as K;
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
        public static K[] FindDatas(string sAccountReference = null, // use default account
                                NWDGameSave sGameSave = null, // use default gamesave
                                NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                )
        {
            //BTBBenchmark.Start();
            //Debug.Log("Datas() Datas count = " + Datas().Datas.Count);
            K[] rReturn = FilterDatas(Datas().Datas, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
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
                if (Datas().kAccountDependent)
                {
                    // autofill sAccountReference if necessary
                    if (string.IsNullOrEmpty(sAccountReference))
                    {
                        sAccountReference = NWDAccount.GetCurrentAccountReference();
                    }
                    //Debug.Log("chercher les data pour " + sAccountReference + " ");
                }
                if (Datas().ClassGameSaveDependent)
                {
                    if (sGameSave == null)
                    {
                        sGameSave = NWDGameSave.CurrentForAccount(sAccountReference);
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
                        if (Datas().kAccountDependent)
                        {
                            // test game save if necessary
                            if (Datas().GameSaveMethod != null && sGameSave != null)
                            {
                                string tGameIndex = sGameSave.Reference;
                                var tValue = Datas().ClassGameDependentProperties.GetValue(tDatas, null);
                                if (tValue == null)
                                {
                                    tValue = "";
                                }
                                string tSaveIndex = Datas().GameSaveMethod.Invoke(tValue, null) as string;
                                if (tSaveIndex != tGameIndex)
                                {
                                    tInsert = false;
                                }
                            }
                            if (tInsert == true)
                            {
                                tInsert = false; // research by default false and true when found first solution
                                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in Datas().AccountMethodDico)
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
        public static K FindFirstDatasByInternalKey(
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
            if (Datas().DatasByInternalKey.ContainsKey(sInternalKey) == true)
            {
                tTestList.AddRange(Datas().DatasByInternalKey[sInternalKey]);
            }
            if (Datas().kAccountDependent)
            {
                // autofill sAccountReference if necessary
                if (string.IsNullOrEmpty(sAccountReference))
                {
                    sAccountReference = NWDAccount.GetCurrentAccountReference();
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " ");
            }
            if (Datas().ClassGameSaveDependent)
            {
                if (sGameSave == null)
                {
                    sGameSave = NWDGameSave.CurrentForAccount(sAccountReference);
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
            }

            K[] rArray = FilterDatas(tTestList, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            if (sCreateIfNotExists == true && rArray.Length == 0)
            {
                //Debug.Log(" must create object !");
                if (sAccountReference == null || sAccountReference == NWDAccount.GetCurrentAccountReference())
                {
                    if (sGameSave == NWDGameSave.Current())
                    {
                        //Debug.Log("Creat Ok");
                        K rReturn = NewData(sWritingMode);
                        rReturn.InternalKey = sInternalKey;
                        rReturn.UpdateData(true, sWritingMode);
                        rArray = new K[1] { rReturn };
                    }
                    else
                    {
                        Debug.Log("create not mpossinble in another gamesave!");
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
        public static void LoadFromDatabase()
        {
            NWDDatas tTypeInfos = NWDDatas.FindTypeInfos(ClassType());
            tTypeInfos = Datas();
            // Reset the Handler of datas index
            tTypeInfos.ResetDatas();

            CreateTable();
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> LoadFromDatabase()");
            // select the good database
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (tSQLiteConnection != null)
            {
                // Create all instance from database
                IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
                // Prepare the datas
                int tCount = 0;
                if (tEnumerable != null)
                {
                    foreach (NWDBasis<K> tItem in tEnumerable)
                    {
                        tCount++;
                        tItem.LoadedFromDatabase();
                    }
                }
            }
            //Debug.Log("NWDBasis<K> LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
            //RepaintTableEditor();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool DataIntegrityState()
        {
            return TestIntegrity();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool TrashState()
        {
            if (XX > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool EnableState()
        {
            return AC;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool ReachableState()
        {
            return IsReacheableByAccount();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================