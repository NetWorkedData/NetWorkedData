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

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Linq.Expressions;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBasisEditorDatasSortType
    {
        //-------------------------------------------------------------------------------------------------------------
        None,
        BySelectAscendant,
        BySelectDescendant,
        //ByIDAscendant,
        //ByIDDescendant,
        ByPrefabAscendant,
        ByPrefabDescendant,
        ByInternalKeyAscendant,
        ByInternalKeyDescendant,
        BySyncAscendant,
        BySyncDescendant,
        ByDevSyncAscendant,
        ByDevSyncDescendant,
        ByPreprodSyncAscendant,
        ByPreprodSyncDescendant,
        ByProdSyncAscendant,
        ByProdSyncDescendant,
        ByStatutAscendant,
        ByStatutDescendant,
        ByReferenceAscendant,
        ByReferenceDescendant,
        ByChecklistAscendant,
        ByChecklistDescendant,
        ByModelAscendant,
        ByModelDescendant,
        ByAgeAscendant,
        ByAgeDescendant,
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool mSettingsShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> EditorDatasMenu = new Dictionary<string, string>(new StringIndexKeyComparer()); // reference/desciption for menu <REF>

        public List<NWDTypeClass> EditorTableDatas = new List<NWDTypeClass>(); // NWDTypeClass
        public Dictionary<NWDTypeClass, bool> EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 ObjectEditorScrollPosition = Vector2.zero;
        public bool kSyncAndMoreInformations = false;
        public NWDTypeClass mObjectInEdition;
        //-------------------------------------------------------------------------------------------------------------
        public void LoadEditorPrefererences()
        {
            //Debug.Log("LoadEditorPrefererences()");
            RowZoom = NWDProjectPrefs.GetFloat(ActionsPrefkey(() => RowZoom), 1.0F);

            m_ShowEnable = NWDProjectPrefs.GetBool(ActionsPrefkey(() => m_ShowEnable), true);
            m_ShowDisable = NWDProjectPrefs.GetBool(ActionsPrefkey(() => m_ShowDisable), true);
            m_ShowTrashed = NWDProjectPrefs.GetBool(ActionsPrefkey(() => m_ShowTrashed), true);
            m_ShowIntegrityError = NWDProjectPrefs.GetBool(ActionsPrefkey(() => m_ShowIntegrityError), true);
            m_ItemPerPageSelection = NWDProjectPrefs.GetInt(ActionsPrefkey(() => m_ItemPerPageSelection), 1);


            RowActions = NWDProjectPrefs.GetBool(ActionsPrefkey(() => RowActions), true);
            TableActions = NWDProjectPrefs.GetBool(ActionsPrefkey(() => TableActions), true);
            SearchActions = NWDProjectPrefs.GetBool(ActionsPrefkey(() => SearchActions), true);
            InspectorActions = NWDProjectPrefs.GetBool(ActionsPrefkey(() => InspectorActions), true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SaveEditorPrefererences()
        {
            NWDProjectPrefs.SetFloat(ActionsPrefkey(() => RowZoom), RowZoom);

            NWDProjectPrefs.SetBool(ActionsPrefkey(() => m_ShowEnable), m_ShowEnable);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => m_ShowDisable), m_ShowDisable);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => m_ShowTrashed), m_ShowTrashed);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => m_ShowIntegrityError), m_ShowIntegrityError);
            NWDProjectPrefs.SetInt(ActionsPrefkey(() => m_ItemPerPageSelection), m_ItemPerPageSelection);

            NWDProjectPrefs.SetBool(ActionsPrefkey(() => RowActions), RowActions);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => SearchActions), SearchActions);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => TableActions), TableActions);
            NWDProjectPrefs.SetBool(ActionsPrefkey(() => InspectorActions), InspectorActions);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string ActionsPrefkey<T>(Expression<Func<T>> sProperty)
        {
            //NWDBenchmark.Start();
            string tKey = "NWDBasisHelper_"; // prevent herited class
            //if (NWDAppConfiguration.SharedInstance().EditorTableCommun == true)
            //{
            //    tKey = tKey + NWDToolbox.PropertyName(sProperty);
            //}
            //else
            {
                tKey = tKey + ClassNamePHP + NWDToolbox.PropertyName(sProperty); ;
            }
            //Debug.Log("ActionsPrefkey() : " + tKey);
            //NWDBenchmark.Finish();
            return tKey;
        }
        //-------------------------------------------------------------------------------------------------------------
        //const string kSearchEditorKey = "kSearchEditorKey";
        //const string kTableEditorKey = "kTableEditorKey";
        //const string kRowActionKey = "kRowActionKey";
        //const string kFilterNumberKey = "kFilterNumberKey";
        //const string kZoomKey = "kZoomKey";
        //const string kFilterEnabledKey = "kFilterEnabledKey";
        //const string kFilterDisabledKey = "kFilterDisabledKey";
        //const string kFilterTrasedKey = "kFilterTrasedKey";
        //const string kFilterCorruptedZoomKey = "kFilterCorruptedZoomKey";
        //-------------------------------------------------------------------------------------------------------------
        //public bool SearchActions()
        //{
        //    return EditorPrefs.GetBool(ActionsPrefkey() + kSearchEditorKey);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool RowActions()
        //{
        //    return EditorPrefs.GetBool(ActionsPrefkey() + kRowActionKey);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool TableActions()
        //{
        //    return EditorPrefs.GetBool(ActionsPrefkey() + kTableEditorKey);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetSearchActions(bool sValue)
        //{
        //    EditorPrefs.SetBool(ActionsPrefkey() + kSearchEditorKey, sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetRowActions(bool sValue)
        //{
        //    EditorPrefs.SetBool(ActionsPrefkey() + kRowActionKey, sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetTableActions(bool sValue)
        //{
        //    EditorPrefs.SetBool(ActionsPrefkey() + kTableEditorKey, sValue);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public bool SearchActions = true;
        public bool RowActions = true;
        public bool TableActions = true;
        public bool InspectorActions = true;
        //-------------------------------------------------------------------------------------------------------------

        public NWDBasisEditorDatasSortType SortType = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
        public float RowZoom = 1.0F;
        public string m_SearchReference = string.Empty;
        public string m_SearchInternalName = string.Empty;
        public string m_SearchInternalDescription = string.Empty;

        public string m_SearchAccount = string.Empty;
        public string m_SearchGameSave = string.Empty;
        public NWDBasisTag m_SearchTag = NWDBasisTag.NoTag;
        public NWDBasisCheckList m_SearchCheckList = new NWDBasisCheckList();

        public Vector2 m_ScrollPositionList;

        public int m_ItemPerPage = 10;

        public int m_ItemPerPageSelection = 0;

        public string[] m_ItemPerPageOptions = new string[] {
            "15", "20", "30", "40", "50", "100", "200", "300", "400", "500"
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
        private Texture2D Texture = null;

        public bool RowAnalyzed = false;
        //-------------------------------------------------------------------------------------------------------------
        public void ResetIconByDefaultIcon()
        {
            //NWDBenchmark.Start();
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/NWDExample.psd";
            string tLookFor = ClassNamePHP + "";
            //Debug.Log("Loook for :" + tLookFor);
            string[] sGUIDs = AssetDatabase.FindAssets(tLookFor);
            string tPathString = null;
            foreach (string tGUID in sGUIDs)
            {
                string tTemp = AssetDatabase.GUIDToAssetPath(tGUID);
                //Debug.Log("Scrip at :" + tTemp);
                if (Path.GetFileName(tTemp) == ClassNamePHP + ".png" || Path.GetFileName(tTemp) == ClassNamePHP + ".psd")
                {
                    tPathString = tTemp;
                }
            }
            //Debug.Log("Scrip find at :" + tPathString);
            if (string.IsNullOrEmpty(tPathString) == false)
            {
                // replace file
                if (Path.GetExtension(tPathString) == "png")
                {
                    File.Delete(tPathString);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    File.Copy(tIconPath, tPathFilename + ".psd");
                    AssetDatabase.ImportAsset(tPathFilename);
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(tPathFilename);
                }
                else
                {
                    File.WriteAllBytes(tPathString, File.ReadAllBytes(tIconPath));
                    AssetDatabase.ImportAsset(tPathString);
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(tPathString);
                }
            }
            else
            {
                string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder();
                // create directories
                Directory.CreateDirectory(tOwnerClassesFolderPath + "/" + ClassNamePHP);
                Directory.CreateDirectory(tOwnerClassesFolderPath + "/" + ClassNamePHP + "/Editor");
                // copy file
                string tPathFilename = tOwnerClassesFolderPath + "/" + ClassNamePHP + "/Editor/" + ClassNamePHP + ".psd";
                File.Copy(tIconPath, tPathFilename);
                AssetDatabase.ImportAsset(tPathFilename);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(tPathFilename);
            }
            Texture = null;
            TextureOfClass();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D TextureOfClass()
        {
            //NWDBenchmark.Start();
            if (Texture == null)
            {
                string tIconName = EditorGUIUtility.isProSkin ? ClassNamePHP + "_pro" : ClassNamePHP;
                string[] sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture2D");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconName))
                    {
                        Texture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
                // if null find just no pro icon ? 
                if (Texture == null)
                {
                    sGUIDs = AssetDatabase.FindAssets(ClassNamePHP + " t:texture2D");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(ClassNamePHP))
                        {
                            Texture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                            break;
                        }
                    }
                }
                // ok draw default ?
                if (Texture == null)
                {
                    tIconName = EditorGUIUtility.isProSkin ? typeof(NWDExample).Name + "_pro" : typeof(NWDExample).Name;
                    sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture2D");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(tIconName))
                        {
                            Texture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                            break;
                        }
                    }
                }
            }
            //NWDBenchmark.Finish();
            return Texture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SelectScript()
        {
            //NWDBenchmark.Start();
            string tLookFor = ClassNamePHP + " t:script";
            //Debug.Log("Loook for :"+ tLookFor);
            string[] sGUIDs = AssetDatabase.FindAssets(tLookFor);
            string tPathString = null;
            foreach (string tGUID in sGUIDs)
            {
                string tTemp = AssetDatabase.GUIDToAssetPath(tGUID);
                //Debug.Log("Scrip at :" + tTemp);
                if (Path.GetFileName(tTemp) == ClassNamePHP + ".cs")
                {
                    tPathString = tTemp;
                }
            }
            if (string.IsNullOrEmpty(tPathString) == false)
            {
                //Debug.Log("Scrip find at :" + tPathString);
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(tPathString);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RowAnalyze()
        {
            //NWDBenchmark.Start();
            if (RowAnalyzed == false)
            {
                foreach (NWDTypeClass tData in Datas)
                {
                    tData.AnalyzeData();
                }
                RowAnalyzed = true;
                SortType = (NWDBasisEditorDatasSortType)NWDProjectPrefs.GetInt(ClassNamePHP + "_SortEditor");
                SortEditorTableDatas();
            }
            //NWDBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void SortEditorTableDatas()
        {
            //NWDBenchmark.Start();
            // first sort to order result constant
            //EditorTableDatas.Sort((x, y) => x.AnalyzeID.CompareTo(y.AnalyzeID));
            // reccord the new pref!
            NWDProjectPrefs.SetInt(ClassNamePHP + "_SortEditor", (int)SortType);
            // procced!
            switch (SortType)
            {
                case NWDBasisEditorDatasSortType.None:
                    {
                    }
                    break;
                //case NWDBasisEditorDatasSortType.ByIDAscendant:
                //    {
                //        //Already did! at first line!
                //        //EditorTableDatas.Sort((x, y) => x.AnalyzeID.CompareTo(y.AnalyzeID));
                //    }
                //    break;
                //case NWDBasisEditorDatasSortType.ByIDDescendant:
                //    {
                //        EditorTableDatas.Sort((x, y) => y.AnalyzeID.CompareTo(x.AnalyzeID));
                //    }
                //    break;
                case NWDBasisEditorDatasSortType.ByPrefabAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzePrefab.CompareTo(y.AnalyzePrefab));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByPrefabDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzePrefab.CompareTo(x.AnalyzePrefab));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByInternalKeyAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => string.Compare(x.DatasMenu(), y.DatasMenu(), StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByInternalKeyDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => string.Compare(y.DatasMenu(), x.DatasMenu(), StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySyncAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeSync.CompareTo(y.AnalyzeSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySyncDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeSync.CompareTo(x.AnalyzeSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByDevSyncAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeDevSync.CompareTo(y.AnalyzeDevSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByDevSyncDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeDevSync.CompareTo(x.AnalyzeDevSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByPreprodSyncAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzePreprodSync.CompareTo(y.AnalyzePreprodSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByPreprodSyncDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzePreprodSync.CompareTo(x.AnalyzePreprodSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByProdSyncAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeProdSync.CompareTo(y.AnalyzeProdSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByProdSyncDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeProdSync.CompareTo(x.AnalyzeProdSync));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByStatutAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeStateInfos.CompareTo(y.AnalyzeStateInfos));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByStatutDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeStateInfos.CompareTo(x.AnalyzeStateInfos));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByReferenceAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => string.Compare(x.Reference, y.Reference, StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByReferenceDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => string.Compare(y.Reference, x.Reference, StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySelectAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeSelected.CompareTo(y.AnalyzeSelected));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySelectDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeSelected.CompareTo(x.AnalyzeSelected));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByChecklistAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeChecklist.CompareTo(y.AnalyzeChecklist));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByChecklistDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeChecklist.CompareTo(x.AnalyzeChecklist));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByModelAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.AnalyzeModel.CompareTo(y.AnalyzeModel));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByModelDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeModel.CompareTo(x.AnalyzeModel));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByAgeAscendant:
                    {
                        EditorTableDatas.Sort((x, y) => x.DM.CompareTo(y.DM));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByAgeDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.DM.CompareTo(x.DM));
                    }
                    break;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDError Error(string sXCode, string sDescription)
        {
            //Debug.Log("NWDBasisHelper Error()");
            return NWDError.CreateGenericError(ClassTableName + NWEConstants.K_MINUS + ClassTrigramme + sXCode, "Error in " + ClassTableName, sDescription, "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RefreshAllWindows()
        {
            NWDModelManager.Refresh();
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
            NWDDataInspector.Refresh();
            NWDNodeEditor.Refresh();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif