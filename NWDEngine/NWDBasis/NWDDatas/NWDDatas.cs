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
    /// <summary>
    /// NWDBasisFilter is use to filter the datas in workflow methodes.
    /// </summary>
    public enum NWDBasisFilter : int
    {
        /// <summary>
        /// All datas, quickly access.
        /// </summary>
        All,
        /// <summary>
        /// The reachable datas by the current user, quickly access.
        /// Return the editor and account dependant datas 
        /// </summary>
        Reachable,
        /// <summary>
        /// The reachable datas by the current user, slow access.
        /// Not trashed, not disabled, for current account!
        /// Return the editor and account dependant datas 
        /// </summary>
        ReachableAndEnable,
        /// <summary>
        /// The reachable datas by the current user but disabled, slow access.
        /// For current account but disabled.
        /// </summary>
        ReachableAndDisabled,
        /// <summary>
        /// The reachable datas by the current user but trashed, slow access.
        /// For current account but trashed.
        /// </summary>
        ReachableAndTrashed,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatas
    {
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassType = null;
        public string ClassName = "";
        public string ClassNamePHP = "";
        public bool ServerSynchronize;
        public string TrigrammeName = "";
        public string ClassDescription = "";
        public string MenuName = "";
        public string TableName = "";
        public string PrefBaseKey = "";
        public GUIContent MenuNameContent = GUIContent.none;
        //-------------------------------------------------------------------------------------------------------------

        static public string kPrefSaltValidKey = "SaltValid";
        static public string kPrefSaltAKey = "SaltA";
        static public string kPrefSaltBKey = "SaltB";

        public string SaltA = "";
        public string SaltB = "";
        public string SaltOk = "";

        public List<object> ObjectsList = new List<object>();
        public List<string> ObjectsByReferenceList = new List<string>();
        public List<string> ObjectsByKeyList = new List<string>();

        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //public Object kObjectInEdition;
        //public Object[] kObjectsArrayInEdition;
        //-------------------------------------------------------------------------------------------------------------
        static Texture2D Texture = null;
        public Texture2D TextureOfClass()
        {
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
            return Texture;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string m_SearchReference = "";
        public string m_SearchInternalName = "";
        public string m_SearchInternalDescription = "";
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
        public List<string> ObjectsInEditorTableKeyList = new List<string>();
        public List<string> ObjectsInEditorTableList = new List<string>();
        public List<bool> ObjectsInEditorTableSelectionList = new List<bool>();
        //-------------------------------------------------------------------------------------------------------------
#endif




        //-------------------------------------------------------------------------------------------------------------
        public NWDDatas()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<Type, NWDDatas> TypesDictionary = new Dictionary<Type, NWDDatas>();

        //-------------------------------------------------------------------------------------------------------------
        public static void Declare(Type sType, bool sServerSynchronize, string sTrigrammeName, string sMenuName, string sDescription)
        {
            Debug.Log("NWDTypeInfos Declare");
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
                tTypeInfos.TableName = sType.Name;
                tTypeInfos.PrefBaseKey = tTypeInfos.TableName + "_";

                tTypeInfos.ClassName = sType.AssemblyQualifiedName;

                TableMapping tTableMapping = new TableMapping(sType);
                string rClassName = tTableMapping.TableName;
                tTypeInfos.ClassNamePHP = rClassName;


                // insert attributs infos
                tTypeInfos.TrigrammeName = sTrigrammeName;
                tTypeInfos.MenuName = sMenuName;
                tTypeInfos.ClassDescription = sDescription;
                tTypeInfos.ServerSynchronize = sServerSynchronize;
                // create GUI object
                tTypeInfos.MenuNameContent = new GUIContent(sMenuName, sDescription);
                // Prepare engine informlations
                tTypeInfos.PrefBaseKey = sType.Name + "_";
                tTypeInfos.PropertiesArrayPrepare();
                tTypeInfos.PropertiesOrderArrayPrepare();
                tTypeInfos.SLQAssemblyOrderArrayPrepare();
                tTypeInfos.SLQAssemblyOrderPrepare();
                tTypeInfos.SLQIntegrityOrderPrepare();
                tTypeInfos.DataAssemblyPropertiesListPrepare();
            }
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
            "TrigrammeName = '" + TrigrammeName + "' " +
            "ServerSynchronize = '" + ServerSynchronize + "' " +
            "ClassDescription = '" + ClassDescription + "' " +
            "MenuName = '" + MenuName + "' " +
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
        public bool kAccountDependent;
        public PropertyInfo[] kAccountDependentProperties;
        public PropertyInfo[] kAccountConnectedProperties;
        public bool kLockedObject;
        public bool kAssetDependent;
        public PropertyInfo[] kAssetDependentProperties;
        //-------------------------------------------------------------------------------------------------------------

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
        public List<NWDTypeClass> DatasReachable = new List<NWDTypeClass>();
        /// <summary>
        /// The datas by reference. Reachable by the current account.
        /// </summary>
        public Dictionary<string, NWDTypeClass> DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
        /// <summary>
        /// The datas by internal key. Return list of datas reachable by the current account.
        /// </summary>
        public Dictionary<string, List<NWDTypeClass>> DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
        /// <summary>
        /// The datas by reverse internal key. Reachable by the current account. You must check if string InternalKey was changed ... in case change the DatasReachableByInternalKey too!
        /// </summary>
        public Dictionary<NWDTypeClass, string> DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
#if UNITY_EDITOR
        public List<string> DatasInEditorTableKeyList = new List<string>();
        public List<bool> DatasInEditorTableSelectionList = new List<bool>();
        public List<string> DatasInEditorTableList = new List<string>();
#endif
        // TODO Futur 
        //-------------------------------------------------------------------------------------------------------------
        public void ResetDatas()
        {
            BTBBenchmark.Start();
            // all datas prepare handler
            Datas = new List<NWDTypeClass>();
            DatasByReference = new Dictionary<string, NWDTypeClass>();
            DatasByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
            DatasByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
            // reachable datas prepare handler
            DatasReachable = new List<NWDTypeClass>();
            DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
            DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
            DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
#if UNITY_EDITOR
            // editor datas prepare handler
            DatasInEditorTableKeyList = new List<string>();
            DatasInEditorTableSelectionList = new List<bool>();
            DatasInEditorTableList = new List<string>();
#endif
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UserChangedReloadDatas()
        {
            RedefineReachableDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RedefineReachableDatas()
        {
            DatasReachable = new List<NWDTypeClass>();
            DatasReachableByReference = new Dictionary<string, NWDTypeClass>();
            DatasReachableByInternalKey = new Dictionary<string, List<NWDTypeClass>>();
            DatasReachableByReverseInternalKey = new Dictionary<NWDTypeClass, string>();
            foreach (NWDTypeClass tData in Datas)
            {
                bool tDataIsValid = tData.DataIntegrityState();
                if (tDataIsValid == true)
                {
                    AddDataReachable(tData);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AddDataReachable(NWDTypeClass sData)
        {
            BTBBenchmark.Start();
            if (sData.ReachableState() == true)
            {
                string tReference = sData.ReferenceUsedValue();
                // Anyway I check if Data is allready in datalist
                if (DatasReachableByReference.ContainsKey(tReference) == false)
                {
                    // get internal key
                    string tInternalKey = sData.InternalKeyValue();
                    // Anyway I add Data in datalist
                    DatasReachable.Add(sData);
                    DatasReachableByReference.Add(tReference, sData);
                    if (DatasReachableByInternalKey.ContainsKey(tInternalKey) == true)
                    {
                        DatasReachableByInternalKey[tInternalKey].Add(sData);
                    }
                    else
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        tList.Add(sData);
                        DatasReachableByInternalKey.Add(tInternalKey, tList);
                    }
                    DatasReachableByReverseInternalKey.Add(sData, tInternalKey);
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddData(NWDTypeClass sData)
        {
            BTBBenchmark.Start();
            // get reference
            string tReference = sData.ReferenceUsedValue();
            // Anyway I check if Data is allready in datalist
            if (DatasByReference.ContainsKey(tReference) == false)
            {
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
                DatasByReverseInternalKey.Add(sData, tInternalKey);

                // Ok now I check if I need to install it in reachable data
                bool tDataIsValid = sData.DataIntegrityState();
                if (tDataIsValid == true)
                {
                    AddDataReachable(sData);
                }
                else
                {
                    Debug.LogWarning("Try to add not integrity data!");
                }
                // Ok now I add datas in editor table list
#if UNITY_EDITOR
                // add load object in editor table
                if (DatasInEditorTableList.Contains(tReference) == false)
                {
                    // Active to auto remove on filter
                    // if (sObject.Tag == (int)m_SearchTag)
                    {
                        DatasInEditorTableList.Add(tReference);
                    }
                }
                DatasInEditorTableKeyList.Add(tInternalKey + " <" + tReference + ">");
                DatasInEditorTableSelectionList.Add(false);
#endif
            }
            else
            {
                Debug.LogWarning("Try to add twice data!");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveDataReachable(NWDTypeClass sData)
        {
            BTBBenchmark.Start();
            string tReference = sData.ReferenceUsedValue();
            // Anyway I check if Data is allready in datalist
            if (DatasReachableByReference.ContainsKey(tReference) == true)
            {
                // get internal key
                string tInternalKey = sData.InternalKeyValue();
                // Anyway I add Remove in datalist
                DatasReachable.Remove(sData);
                DatasReachableByReference.Remove(tReference);
                if (DatasReachableByInternalKey.ContainsKey(tInternalKey) == true)
                {
                    DatasReachableByInternalKey[tInternalKey].Remove(sData);
                    if (DatasReachableByInternalKey[tInternalKey].Count == 0)
                    {
                        DatasReachableByInternalKey.Remove(tInternalKey);
                    }
                }
                DatasReachableByReverseInternalKey.Remove(sData);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(NWDTypeClass sData)
        {
            BTBBenchmark.Start();
            // get reference
            string tReference = sData.ReferenceUsedValue();
            // Anyway I check if Data is allready in datalist
            if (DatasByReference.ContainsKey(tReference) == true)
            {
                // get internal key
                string tInternalKey = sData.InternalKeyValue();
                // Anyway I add Data in datalist
                int tIndex = Datas.IndexOf(sData);
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
                RemoveDataReachable(sData);
#if UNITY_EDITOR
                // remove object in editor table
                int tIndexB = DatasInEditorTableList.IndexOf(tReference);
                if (tIndexB >= 0 && tIndexB < DatasInEditorTableList.Count())
                {
                    DatasInEditorTableList.RemoveAt(tIndexB);
                }
                ObjectsInEditorTableKeyList.RemoveAt(tIndex);
                ObjectsInEditorTableSelectionList.RemoveAt(tIndex);
#endif
            }
            else
            {
                Debug.LogWarning("Try to remove an unreferenced data!");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataReachable(NWDTypeClass sData)
        {
            string tReference = sData.ReferenceUsedValue();
            string tInternalKey = sData.InternalKeyValue();
            string tOldInternalKey = DatasReachableByReverseInternalKey[sData];
            if (tOldInternalKey != tInternalKey)
            {
                if (DatasReachableByReference.ContainsKey(tReference) == true)
                {
                    DatasReachable.Remove(sData);
                    DatasReachableByReference.Remove(tReference);
                    if (DatasReachableByInternalKey.ContainsKey(tOldInternalKey) == true)
                    {
                        DatasReachableByInternalKey[tOldInternalKey].Remove(sData);
                        if (DatasReachableByInternalKey[tOldInternalKey].Count == 0)
                        {
                            DatasReachableByInternalKey.Remove(tOldInternalKey);
                        }
                    }
                    DatasReachableByReverseInternalKey.Remove(sData);
                }
                AddDataReachable(sData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(NWDTypeClass sData)
        {
            string tReference = sData.ReferenceUsedValue();
            string tInternalKey = sData.InternalKeyValue();
            string tOldInternalKey = DatasByReverseInternalKey[sData];
            if (tOldInternalKey != tInternalKey)
            {
                int tIndex = Datas.IndexOf(sData);
                // remove internal Key in list
                if (DatasByInternalKey.ContainsKey(tOldInternalKey) == true)
                {
                    DatasByInternalKey[tOldInternalKey].Remove(sData);
                    if (DatasByInternalKey[tOldInternalKey].Count == 0)
                    {
                        DatasByInternalKey.Remove(tOldInternalKey);
                    }
                }
                DatasByReverseInternalKey.Remove(sData);
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

                UpdateDataReachable(sData);

                #if UNITY_EDITOR
                // remove object in editor table

                DatasInEditorTableKeyList.RemoveAt(tIndex);
                DatasInEditorTableSelectionList.RemoveAt(tIndex);

                DatasInEditorTableKeyList.Insert(tIndex, tInternalKey + " <" + tReference + ">");
                DatasInEditorTableSelectionList.Insert(tIndex, false);

                int tIndexB = DatasInEditorTableList.IndexOf(tReference);
                if (tIndexB >= 0 && tIndexB < DatasInEditorTableList.Count())
                {
                    DatasInEditorTableList.RemoveAt(tIndexB);
                    // Active to auto remove on filter
                    // if (sObject.Tag == (int)m_SearchTag)
                    {
                        DatasInEditorTableList.Insert(tIndexB, tReference);
                    }
                }
                else
                {
                    // if (sObject.Tag == m_SearchTag)
                    {
                        DatasInEditorTableList.Add(tReference);
                    }
                }
                #endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass[] GetAllDatas(NWDBasisFilter sFilter)
        {
            BTBBenchmark.Start();
            NWDTypeClass[] rReturn;
            switch (sFilter)
            {
                case NWDBasisFilter.ReachableAndDisabled:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        foreach (NWDTypeClass tDatas in DatasReachable)
                        {
                            if (tDatas.EnableState() == false)
                            {
                                tList.Add(tDatas);
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.ReachableAndTrashed:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        foreach (NWDTypeClass tDatas in DatasReachable)
                        {
                            if (tDatas.TrashState() == true)
                            {
                                tList.Add(tDatas);
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.ReachableAndEnable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        foreach (NWDTypeClass tDatas in DatasReachable)
                        {
                            if (tDatas.EnableState() == true)
                            {
                                tList.Add(tDatas);
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.All:
                    {
                        rReturn = Datas.ToArray();
                    }
                    break;
                default:
                case NWDBasisFilter.Reachable:
                    {
                        rReturn = DatasReachable.ToArray();
                    }
                    break;
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass[] GetDatasByInternalKey(string sInternalKey, NWDBasisFilter sFilter)
        {
            BTBBenchmark.Start();
            NWDTypeClass[] rReturn = null;
            switch (sFilter)
            {
                case NWDBasisFilter.ReachableAndDisabled:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.EnableState() == false)
                                {
                                    tList.Add(tDatas);
                                }
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.ReachableAndTrashed:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.TrashState() == true)
                                {
                                    tList.Add(tDatas);
                                }
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.ReachableAndEnable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.EnableState() == true)
                                {
                                    tList.Add(tDatas);
                                }
                            }
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                case NWDBasisFilter.All:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasByInternalKey.ContainsKey(sInternalKey))
                        {
                            tList = DatasByInternalKey[sInternalKey];
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
                default:
                case NWDBasisFilter.Reachable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            tList = DatasReachableByInternalKey[sInternalKey];
                        }
                        rReturn = tList.ToArray();
                    }
                    break;
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass GetFirstDatasByInternalKey(string sInternalKey, NWDBasisFilter sFilter)
        {
            BTBBenchmark.Start();
            NWDTypeClass rReturn = null;
            switch (sFilter)
            {
                case NWDBasisFilter.ReachableAndDisabled:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.EnableState() == false)
                                {
                                    rReturn = tDatas;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.ReachableAndTrashed:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.TrashState() == true)
                                {
                                    rReturn = tDatas;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.ReachableAndEnable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            foreach (NWDTypeClass tDatas in DatasReachableByInternalKey[sInternalKey])
                            {
                                if (tDatas.EnableState() == true)
                                {
                                    rReturn = tDatas;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.All:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasByInternalKey.ContainsKey(sInternalKey))
                        {
                            tList = DatasByInternalKey[sInternalKey];
                        }
                        if (tList.Count > 0)
                        {
                            rReturn = tList[0];
                        }
                    }
                    break;
                default:
                case NWDBasisFilter.Reachable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByInternalKey.ContainsKey(sInternalKey))
                        {
                            tList = DatasReachableByInternalKey[sInternalKey];
                        }
                        if (tList.Count > 0)
                        {
                            rReturn = tList[0];
                        }
                    }
                    break;
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass GetDataByReference(string sReference, NWDBasisFilter sFilter)
        {
            // TODO
            BTBBenchmark.Start();
            NWDTypeClass rReturn = null;
            switch (sFilter)
            {
                case NWDBasisFilter.ReachableAndDisabled:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByReference.ContainsKey(sReference))
                        {
                            rReturn = DatasReachableByReference[sReference];
                            if (rReturn.EnableState() == true)
                            {
                                rReturn = null;
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.ReachableAndTrashed:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByReference.ContainsKey(sReference))
                        {
                            rReturn = DatasReachableByReference[sReference];
                            if (rReturn.TrashState() == false)
                            {
                                rReturn = null;
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.ReachableAndEnable:
                    {
                        List<NWDTypeClass> tList = new List<NWDTypeClass>();
                        if (DatasReachableByReference.ContainsKey(sReference))
                        {
                            rReturn = DatasReachableByReference[sReference];
                            if (rReturn.EnableState() == false)
                            {
                                rReturn = null;
                            }
                        }
                    }
                    break;
                case NWDBasisFilter.All:
                    {
                        if (DatasByReference.ContainsKey(sReference))
                        {
                            rReturn = DatasByReference[sReference];
                        }
                    }
                    break;
                default:
                case NWDBasisFilter.Reachable:
                    {
                        if (DatasReachableByReference.ContainsKey(sReference))
                        {
                            rReturn = DatasReachableByReference[sReference];
                        }
                    }
                    break;
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------

        public static string SynchronizeKeyData = "data";
        public static string SynchronizeKeyDataCount = "rowCount";
        public static string SynchronizeKeyTimestamp = "sync";
        public static string SynchronizeKeyLastTimestamp = "last";
        public static string SynchronizeKeyInWaitingTimestamp = "waiting";
        //-------------------------------------------------------------------------------------------------------------


        public static bool mSettingsShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        public static bool mForceSynchronization = false;
        //-------------------------------------------------------------------------------------------------------------

        public static Vector2 m_ObjectEditorScrollPosition = Vector2.zero; // not obligation
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------
                                                                           //-------------------------------------------------------------------------------------------------------------


    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadFromDatabase()
        {
            BTBBenchmark.Start();
            Debug.Log("NWDBasis<K> LoadFromDatabase()");
            // select the good database
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            // Create all instance from database
            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);

            NWDDatas tTypeInfos = NWDDatas.FindTypeInfos(ClassType());
            // Reset the Handler of datas index
            tTypeInfos.ResetDatas();
            // Prepare the datas
            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    tItem.InDatabase = true;
                    tItem.FromDatabase = true;
                    tItem.WritingPending = NWDWritingPending.InDatabase;
                    tItem.AddonLoadedMe();
                    #if UNITY_EDITOR
                    tItem.ErrorCheck();
                    #endif

                    // Add in handler
                    tTypeInfos.AddData(tItem);
                }
            }
            BTBBenchmark.Finish();
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