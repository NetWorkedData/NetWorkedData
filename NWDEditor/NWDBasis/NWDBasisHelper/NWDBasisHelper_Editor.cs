//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBasisEditorDatasSortType : int
    {
        //-------------------------------------------------------------------------------------------------------------
        None,
        BySelectAscendant,
        BySelectDescendant,
        ByIDAscendant,
        ByIDDescendant,
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
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool mSettingsShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> EditorDatasMenu = new Dictionary<string, string>(); // reference/desciption for menu <REF>

        public List<NWDTypeClass> EditorTableDatas = new List<NWDTypeClass>(); // NWDTypeClass
        public Dictionary<NWDTypeClass, bool> EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 ObjectEditorScrollPosition = Vector2.zero;
        public bool kSyncAndMoreInformations = false;

        //// TODO move in basichelper
        //public bool SearchActions = true;
        //// TODO move in basichelper
        //public bool RowActions = true;
        //// TODO move in basichelper
        //public bool TableActions = true;

        public string ActionsPrefkey()
        {
            string tKey = string.Empty;
            if (NWDAppConfiguration.SharedInstance().EditorTableCommun == false)
            {
                tKey = ClassNamePHP;
            }
            return tKey;
        }
        const string kSearchEditorState= "SearchEditorState";
        const string kTableEditorState = "TableEditorState";
        const string kRowEditorState = "RowEditorState";
        public bool SearchActions()
        {
            return EditorPrefs.GetBool(ActionsPrefkey()+ kSearchEditorState);
        }
        public bool RowActions()
        {
            return EditorPrefs.GetBool(ActionsPrefkey()+ kRowEditorState);
        }
        public bool TableActions()
        {
            return EditorPrefs.GetBool(ActionsPrefkey()+ kTableEditorState);
        }
        public void SetSearchActions(bool sValue)
        {
            EditorPrefs.SetBool(ActionsPrefkey()+ kSearchEditorState, sValue);
        }
        public void SetRowActions(bool sValue)
        {
            EditorPrefs.SetBool(ActionsPrefkey()+ kRowEditorState, sValue);
        }
        public void SetTableActions(bool sValue)
        {
            EditorPrefs.SetBool(ActionsPrefkey()+ kTableEditorState, sValue);
        }








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
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Resources/Textures/NWDExample.psd";
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D TextureOfClass()
        {
            if (Texture == null)
            {
                Texture2D rTexture = null;
                string[] sGUIDs = AssetDatabase.FindAssets(ClassNamePHP + " t:texture2D");
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
                    Texture = NWDGUI.kImageDefaultIcon;
                }
            }
            return Texture;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SelectScript()
        {
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
        } 
        //-------------------------------------------------------------------------------------------------------------
        public void RowAnalyze()
        {
            if (RowAnalyzed == false)
            {
                foreach (NWDTypeClass tData in Datas)
                {
                    tData.AnalyzeData();
                }
                RowAnalyzed = true;
                SortType = (NWDBasisEditorDatasSortType)EditorPrefs.GetInt(ClassNamePHP + "_SortEditor");
                SortEditorTableDatas();
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public void SortEditorTableDatas()
        {
            // first sort to order result constant
            EditorTableDatas.Sort((x, y) => x.AnalyzeID.CompareTo(y.AnalyzeID));
            // reccord the new pref!
            EditorPrefs.SetInt(ClassNamePHP + "_SortEditor", (int)SortType);
            // procced!
            switch (SortType)
            {
                case NWDBasisEditorDatasSortType.None:
                    {
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByIDAscendant:
                    {
                        //Allready did! at first line!
                        //EditorTableDatas.Sort((x, y) => x.AnalyzeID.CompareTo(y.AnalyzeID));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByIDDescendant:
                    {
                        EditorTableDatas.Sort((x, y) => y.AnalyzeID.CompareTo(x.AnalyzeID));
                    }
                    break;
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
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ModelAnalyze()
        {
            NWDAliasMethod.InvokeClassMethod(ClassType, NWDConstants.M_ModelAnalyze);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif