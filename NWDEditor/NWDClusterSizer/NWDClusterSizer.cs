//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClusterSizer : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        public List<NWDBasisHelper> TypeEditorList = new List<NWDBasisHelper>();
        public List<NWDBasisHelper> TypeAccountList = new List<NWDBasisHelper>();
        public Dictionary<NWDBasisHelper, double> TypeEditorAndSize = new Dictionary<NWDBasisHelper, double>();
        public Dictionary<NWDBasisHelper, double> TypeAccountedAndSize = new Dictionary<NWDBasisHelper, double>();
        public Dictionary<NWDBasisHelper, int> TypeAndQuantity = new Dictionary<NWDBasisHelper, int>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDClusterSizer kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDClusterSizer SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDClusterSizer)) as NWDClusterSizer;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDClusterSizer SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDClusterSizer));
            foreach (NWDClusterSizer tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstanced()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            TypeAccountedAndSize.Clear();
            TypeAndQuantity.Clear();
            TypeEditorAndSize.Clear();
            TypeEditorList.Clear();
            TypeAccountList.Clear();
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_CLUSTER_SIZER_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDClusterSizer).Name + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDClusterSizer).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeNotAccountDependantList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper != null)
                {
                    double tSize = tHelper.SizerCalculate();
                    TypeEditorAndSize.Add(tHelper, tSize);
                    TypeEditorList.Add(tHelper);
                    TypeAndQuantity.Add(tHelper, EditorPrefs.GetInt("cluster_test_" + tHelper.ClassNamePHP, 0));
                }
            }
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper != null)
                {
                    double tSize = tHelper.SizerCalculate();
                    TypeAccountedAndSize.Add(tHelper, tSize);
                    TypeAccountList.Add(tHelper);
                    TypeAndQuantity.Add(tHelper, EditorPrefs.GetInt("cluster_test_" + tHelper.ClassNamePHP, 0));
                }
            }


            TypeEditorList.Sort((tA, tB) => string.Compare(tA.ClassNamePHP, tB.ClassNamePHP, StringComparison.Ordinal));
            TypeAccountList.Sort((tA, tB) => string.Compare(tA.ClassNamePHP, tB.ClassNamePHP, StringComparison.Ordinal));

            MinValue();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void SetMin(Type sType, int sMin)
        {
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            if (tHelper != null)
            {
                if (EditorPrefs.GetInt("cluster_test_" + tHelper.ClassNamePHP) < 1)
                {
                    TypeAndQuantity[tHelper] = 1;
                    EditorPrefs.SetInt("cluster_test_" + tHelper.ClassNamePHP, TypeAndQuantity[tHelper]);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void MinValue()
        {
            SetMin(typeof(NWDAccount), 1);
            SetMin(typeof(NWDAccountInfos), 1);
            SetMin(typeof(NWDGameSave), 1);
            SetMin(typeof(NWDUserInfos), 1);
            SetMin(typeof(NWDAccountPreference), NWDDataManager.SharedInstance().ClassDataLoaded * 6);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DrawType(Type sType)
        {
            GUILayout.BeginVertical(/*EditorStyles.helpBox*/);
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            NWDGUILayout.SubSection(tHelper.ClassNamePHP);
            tHelper.DrawTypeInformations();
            GUILayout.EndVertical();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title(NWDConstants.K_APP_CLUSTER_SIZER_TITLE);
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            float tLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;

            double tTotalEditor = 0;
            double tTotalAccount = 0;

            float Giga = 1024F * 1024F * 1024F;
            float Tetra = 1024F * 1024F * 1024F * 1024F;

            NWDGUILayout.Section("System size (Giga)");
            GUILayout.BeginHorizontal();
            GUILayout.Label("System size", GUILayout.Width(160));


            int tSystem = EditorPrefs.GetInt("cluster_test_System");
            int tSystemValue = EditorGUILayout.IntSlider("Giga", tSystem, 10, 50);
            if (tSystemValue != EditorPrefs.GetInt("cluster_test_System"))
            {
                EditorPrefs.SetInt("cluster_test_System", tSystemValue);
                tSystem = tSystemValue;
            }
            GUILayout.EndHorizontal();


            NWDGUILayout.Section("Datas editor");
            foreach (NWDBasisHelper tHelper in TypeEditorList)
            {
                GUILayout.BeginHorizontal();
                //TypeAndQuantity[tHelper] = EditorGUILayout.IntField(tHelper.ClassNamePHP, TypeAndQuantity[tHelper]);
                GUILayout.Label(tHelper.ClassNamePHP, GUILayout.Width(160));
                int tValue = EditorGUILayout.IntSlider("(" + TypeEditorAndSize[tHelper] + " octets)", TypeAndQuantity[tHelper], 0, 2048);
                if (tValue != TypeAndQuantity[tHelper])
                {
                    TypeAndQuantity[tHelper] = tValue;
                    EditorPrefs.SetInt("cluster_test_" + tHelper.ClassNamePHP, tValue);
                    MinValue();
                }
                tTotalEditor += TypeAndQuantity[tHelper] * TypeEditorAndSize[tHelper];
                GUILayout.EndHorizontal();
            }

            NWDGUILayout.Section("Datas player");
            foreach (NWDBasisHelper tHelper in TypeAccountList)
            {
                GUILayout.BeginHorizontal();
                //TypeAndQuantity[tHelper] = EditorGUILayout.IntField(tHelper.ClassNamePHP, TypeAndQuantity[tHelper]);
                GUILayout.Label(tHelper.ClassNamePHP, GUILayout.Width(160));
                int tValue = EditorGUILayout.IntSlider("(" + TypeAccountedAndSize[tHelper] + " octets)", TypeAndQuantity[tHelper], 0, 2048);
                if (tValue != TypeAndQuantity[tHelper])
                {
                    TypeAndQuantity[tHelper] = tValue;
                    EditorPrefs.SetInt("cluster_test_" + tHelper.ClassNamePHP, tValue);
                    MinValue();
                }
                tTotalAccount += TypeAndQuantity[tHelper] * TypeAccountedAndSize[tHelper];
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

            string tFormatData = "N2";
            string tFormat = "N0";

            double tOctetEditor = tTotalEditor;
            double tKiloOctetEditor = (float)tOctetEditor / 1024F;
            double tMegaOctetEditor = (float)tKiloOctetEditor / 1024F;

            double tOctetAccount = tTotalAccount;
            double tKiloOctetAccount = (float)tOctetAccount / 1024F;
            double tMegaOctetAccount = (float)tKiloOctetAccount / 1024F;

            NWDGUILayout.Title("Results");

            NWDGUILayout.Section("Editor result");
            //EditorGUILayout.LabelField("o", tOctetEditor.ToString(tFormatData).Replace(",", " ") + " octects");
            EditorGUILayout.LabelField("Size ", tKiloOctetEditor.ToString(tFormatData).Replace(",", " ") + " Ko" + " or " + tMegaOctetEditor.ToString(tFormatData).Replace(",", " ") + " Mo");

            NWDGUILayout.Section("Player result");
            //EditorGUILayout.LabelField("o", tOctetAccount.ToString(tFormat).Replace(",", " ") + " octects");
            EditorGUILayout.LabelField("Size ", tKiloOctetAccount.ToString(tFormatData).Replace(",", " ") + " Ko" + " or " + tMegaOctetAccount.ToString(tFormatData).Replace(",", " ") + " Mo");

            NWDGUILayout.Section("Cluster disk and users expected");

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("40Go", Mathf.FloorToInt(((40F * Giga) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("80Go", Mathf.FloorToInt(((80F * Giga) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("500Go", Mathf.FloorToInt(((500 * Giga) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("1To", Mathf.FloorToInt(((1F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("2To", Mathf.FloorToInt(((2F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("4To", Mathf.FloorToInt(((4F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("8To", Mathf.FloorToInt(((8F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("12To", Mathf.FloorToInt(((12F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("32To", Mathf.FloorToInt(((32F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");
            EditorGUILayout.LabelField("64To", Mathf.FloorToInt(((64F * Tetra) - tSystem * Giga - (float)tOctetEditor) / (float)tOctetAccount).ToString(tFormat).Replace(",", " ") + " users");

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            //NWEBenchmark.Finish();
            EditorGUIUtility.labelWidth = tLabelWidth;


        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
