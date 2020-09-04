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
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClusterSizerContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDBasisHelper> TypeEditorList = new List<NWDBasisHelper>();
        public List<NWDBasisHelper> TypeAccountList = new List<NWDBasisHelper>();
        public Dictionary<NWDBasisHelper, double> TypeEditorAndSize = new Dictionary<NWDBasisHelper, double>();
        public Dictionary<NWDBasisHelper, double> TypeAccountedAndSize = new Dictionary<NWDBasisHelper, double>();
        public Dictionary<NWDBasisHelper, int> TypeAndQuantity = new Dictionary<NWDBasisHelper, int>();

        public Dictionary<NWDBasisHelper, int> TypeAndMin = new Dictionary<NWDBasisHelper, int>();
        public Dictionary<NWDBasisHelper, int> TypeAndMax = new Dictionary<NWDBasisHelper, int>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDClusterSizerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDClusterSizerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDClusterSizerContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void SetMinMax(Type sType, int sMin, int sMax)
        {
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            if (tHelper != null)
            {
                TypeAndMin[tHelper] = sMin;
                TypeAndMax[tHelper] = sMax;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ByPassMinMaxValue()
        {
            SetMinMax(typeof(NWDRequestToken), NWDAppConfiguration.SharedInstance().SelectedEnvironment().TokenHistoric, NWDAppConfiguration.SharedInstance().SelectedEnvironment().TokenHistoric * 2);
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void DrawType(Type sType)
        //{
        //    GUILayout.BeginVertical(/*EditorStyles.helpBox*/);
        //    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
        //    NWDGUILayout.SubSection(tHelper.ClassNamePHP);
        //    tHelper.DrawTypeInformations();
        //    GUILayout.EndVertical();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            TypeAccountedAndSize.Clear();
            TypeAndQuantity.Clear();
            TypeEditorAndSize.Clear();
            TypeEditorList.Clear();
            TypeAccountList.Clear();
            TypeAndMin.Clear();
            TypeAndMax.Clear();

            foreach (Type tType in NWDDataManager.SharedInstance().ClassNotAccountDependentList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper != null)
                {
                    if (tHelper.TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                    {
                        double tSize = tHelper.SizerCalculate();
                        TypeEditorAndSize.Add(tHelper, tSize);
                        TypeEditorList.Add(tHelper);
                        TypeAndMin.Add(tHelper, tHelper.ClusterMin);
                        TypeAndMax.Add(tHelper, tHelper.ClusterMax);
                        TypeAndQuantity.Add(tHelper, NWDProjectPrefs.GetInt("cluster_test_" + tHelper.ClassNamePHP, TypeAndMin[tHelper]));
                    }
                }
            }
            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper != null)
                {
                    if (tHelper.TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                    {
                        double tSize = tHelper.SizerCalculate();
                        TypeAccountedAndSize.Add(tHelper, tSize);
                        TypeAccountList.Add(tHelper);
                        TypeAndMin.Add(tHelper, tHelper.ClusterMin);
                        TypeAndMax.Add(tHelper, tHelper.ClusterMax);
                        TypeAndQuantity.Add(tHelper, NWDProjectPrefs.GetInt("cluster_test_" + tHelper.ClassNamePHP, TypeAndMin[tHelper]));
                    }
                }
            }

            TypeEditorList.Sort((tA, tB) => string.Compare(tA.ClassNamePHP, tB.ClassNamePHP, StringComparison.Ordinal));
            TypeAccountList.Sort((tA, tB) => string.Compare(tA.ClassNamePHP, tB.ClassNamePHP, StringComparison.Ordinal));

            ByPassMinMaxValue();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();

            NWDGUILayout.Title(NWDConstants.K_APP_CLUSTER_SIZER_TITLE);

            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            float tLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 100;

            double tTotalEditor = 0;
            double tTotalAccount = 0;

            float Giga = 1024F * 1024F * 1024F;
            float Tetra = 1024F * 1024F * 1024F * 1024F;

            NWDGUILayout.Section("System size (Giga)");
            GUILayout.BeginHorizontal();
            GUILayout.Label("System size", GUILayout.Width(160));


            int tSystem = NWDProjectPrefs.GetInt("cluster_test_System");
            int tSystemValue = EditorGUILayout.IntSlider("Giga", tSystem, 10, 50);
            if (tSystemValue != NWDProjectPrefs.GetInt("cluster_test_System"))
            {
                NWDProjectPrefs.SetInt("cluster_test_System", tSystemValue);
                tSystem = tSystemValue;
            }
            GUILayout.EndHorizontal();


            NWDGUILayout.Section("Datas editor");
            foreach (NWDBasisHelper tHelper in TypeEditorList)
            {
                GUILayout.BeginHorizontal();
                //TypeAndQuantity[tHelper] = EditorGUILayout.IntField(tHelper.ClassNamePHP, TypeAndQuantity[tHelper]);
                GUILayout.Label(tHelper.ClassNamePHP, GUILayout.Width(160));
                int tValue = EditorGUILayout.IntSlider("(" + TypeEditorAndSize[tHelper] + " octets)", TypeAndQuantity[tHelper], TypeAndMin[tHelper], TypeAndMax[tHelper]);
                if (tValue != TypeAndQuantity[tHelper])
                {
                    TypeAndQuantity[tHelper] = tValue;
                    NWDProjectPrefs.SetInt("cluster_test_" + tHelper.ClassNamePHP, tValue);
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
                int tValue = EditorGUILayout.IntSlider("(" + TypeAccountedAndSize[tHelper] + " octets)", TypeAndQuantity[tHelper], TypeAndMin[tHelper], TypeAndMax[tHelper]);
                if (tValue != TypeAndQuantity[tHelper])
                {
                    TypeAndQuantity[tHelper] = tValue;
                    NWDProjectPrefs.SetInt("cluster_test_" + tHelper.ClassNamePHP, tValue);
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

            EditorGUIUtility.labelWidth = tLabelWidth;

            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClusterSizer : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "cluster-sizer/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDClusterSizer _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDClusterSizer SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDClusterSizer), ShowAsWindow()) as NWDClusterSizer;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDClusterSizer SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
            return _kSharedInstance;
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
            if (_kSharedInstance != null)
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
            NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 500;
            NormalizeHeight = 900;
            // set title
            TitleInit(NWDConstants.K_APP_CLUSTER_SIZER_TITLE, typeof(NWDClusterSizer));
            NWDClusterSizerContent.SharedInstance().OnEnable(this);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDClusterSizerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
