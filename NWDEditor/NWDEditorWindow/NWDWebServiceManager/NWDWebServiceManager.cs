//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
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
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDWebServiceManagerContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDWebServiceManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDWebServiceManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDWebServiceManagerContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawForWebservice(int sWebservices)
        {
            bool tUnused = false;
            if (sWebservices == NWDAppConfiguration.SharedInstance().WebBuild)
            {
                EditorGUILayout.HelpBox("This WebService is the active webservice for this build!", MessageType.Warning);
            }
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                if (NWDBasisHelper.FindTypeInfos(tType).WebModelSQLOrder.ContainsKey(sWebservices) == true)
                {
                    tUnused = true;
                    break;
                }
            }
            NWDBasisHelper tDatasToTest = NWDBasisHelper.FindTypeInfos(typeof(NWDParameter));
            if (tUnused == false)
            {
                NWDGUILayout.Label("" + NWDAppConfiguration.SharedInstance().WebFolder + "_" + sWebservices.ToString("0000") + " not in config (without modification than preview webservice)");
            }
            else
            {
                NWDGUILayout.Label("" + NWDAppConfiguration.SharedInstance().WebFolder + "_" + sWebservices.ToString("0000") + " in config");
            }

            EditorGUI.BeginDisabledGroup(sWebservices == 0);
            // insert management ...


            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Dev", NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateDev))
            {
                if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDAppConfiguration.SharedInstance().DevEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Maitenance)
                    {
                        if (GUILayout.Button("Maintenance", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.SetMaintenance(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is in maintenance", NWDGUI.KTableSearchTitle);
                    }
                    if (NWDAppConfiguration.SharedInstance().DevEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Obsolete)
                    {
                        if (GUILayout.Button("Obsolete", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.SetObsolete(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is obsolete", NWDGUI.KTableSearchTitle);
                    }
                    if (NWDAppConfiguration.SharedInstance().DevEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Active)
                    {
                        if (GUILayout.Button("Activate", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.SetActivate(sWebservices);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is active", NWDGUI.KTableSearchTitle);
                    }
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                    GUILayout.FlexibleSpace();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Preprod", NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGeneratePreprod))
            {
                if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Maitenance)
                    {
                        if (GUILayout.Button("Maintenance", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetMaintenance(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is in maintenance", NWDGUI.KTableSearchTitle);
                    }

                    if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Obsolete)
                    {
                        if (GUILayout.Button("Obsolete", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetObsolete(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is in obsolete", NWDGUI.KTableSearchTitle);
                    }
                    if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Active)
                    {
                        if (GUILayout.Button("Activate", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.SetActivate(sWebservices);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is active", NWDGUI.KTableSearchTitle);
                    }
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                    GUILayout.FlexibleSpace();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(/*GUILayout.MinWidth(tWidthThird),*/ GUILayout.ExpandWidth(true));
            GUILayout.Label("Prod", NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateProd))
            {
                if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDAppConfiguration.SharedInstance().ProdEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Maitenance)
                    {
                        if (GUILayout.Button("Maintenance", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.SetMaintenance(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is in maintenance", NWDGUI.KTableSearchTitle);
                    }
                    if (NWDAppConfiguration.SharedInstance().ProdEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Obsolete)
                    {
                        if (GUILayout.Button("Obsolete", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.SetObsolete(sWebservices, true);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is obsolete", NWDGUI.KTableSearchTitle);
                    }
                    if (NWDAppConfiguration.SharedInstance().ProdEnvironment.GetWebservicesStateByKey(sWebservices) != NWDWebServiceState.Active)
                    {
                        if (GUILayout.Button("Activate", NWDGUI.KTableSearchButton))
                        {
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.SetActivate(sWebservices);
                        }
                    }
                    else
                    {
                        GUILayout.Label("Is active", NWDGUI.KTableSearchTitle);
                    }
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                    GUILayout.FlexibleSpace();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();




            EditorGUI.EndDisabledGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                }
            }
            // Begin scroll view
            NWDGUILayout.Title("WebServices management");
            //NWDGUILayout.Informations("Be careful when making changes!");
            //NWDGUILayout.Line();
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // Launcher

            NWDGUILayout.Section("WebServices actual");
            EditorGUILayout.LabelField(" WebBuild used ", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(" WebBuild max", NWDAppConfiguration.SharedInstance().WebBuildMax.ToString());

            NWDGUILayout.Section("WebServices management");
            Dictionary<int, bool> tWSList = new Dictionary<int, bool>();
            tWSList.Add(0, true);
            foreach (KeyValuePair<int, bool> tWS in NWDAppConfiguration.SharedInstance().WSList)
            {
                if (tWSList.ContainsKey(tWS.Key) == false)
                {
                    tWSList.Add(tWS.Key, tWS.Value);
                }
            }
            List<int> tWSListUsable = new List<int>();
            List<string> tWSListUsableString = new List<string>();
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                if (tWS.Value == true)
                {
                    tWSListUsable.Add(tWS.Key);
                    tWSListUsableString.Add(NWDAppConfiguration.SharedInstance().WebFolder + "_" + tWS.Key.ToString("0000"));
                }
            }
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                NWDGUILayout.SubSection("Webservice " + tWS.Key.ToString("0000"));
                DrawForWebservice(tWS.Key);
            }


            NWDGUILayout.Section("WebServices next generate");
            //if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
            {
                NWDGUI.BeginRedArea();
                if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
                {
                    if (GUILayout.Button("Generate next WS" + (NWDAppConfiguration.SharedInstance().WebBuildMax + 1).ToString("0000"), NWDGUI.KTableSearchButton))
                    {
                        NWDDataManager.SharedInstance().CreatePHPAllClass(true, false);
                    }
                }
                else
                {
                    NWDGUI.BeginRedArea();
                    if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                    {
                        NWDProjectCredentialsManager.SharedInstanceFocus();
                    }
                    NWDGUI.EndRedArea();
                }
                NWDGUI.EndRedArea();
            }
            NWDGUILayout.Section("WebServices re-generate");

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label(NWDAppConfiguration.SharedInstance().DevEnvironment.Environment, NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateDev))
            {
                if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (GUILayout.Button("Regenerate " + NWDAppConfiguration.SharedInstance().WebBuildMax, NWDGUI.KTableSearchButton))
                    {
                        NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false); ;
                    }
                    NWDGUI.EndRedArea();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label(NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment, NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGeneratePreprod))
            {
                if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (GUILayout.Button("Regenerate " + NWDAppConfiguration.SharedInstance().WebBuildMax, NWDGUI.KTableSearchButton))
                    {
                        NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false); ;
                    }
                    NWDGUI.EndRedArea();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label(NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment, NWDGUI.KTableSearchTitle);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateProd))
            {
                if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (GUILayout.Button("Regenerate " + NWDAppConfiguration.SharedInstance().WebBuildMax, NWDGUI.KTableSearchButton))
                    {
                        NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, false); ;
                    }
                    NWDGUI.EndRedArea();
                }
            }
            else
            {
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                NWDGUI.EndRedArea();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
            //// finish with reccord red button
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            NWDGUI.BeginRedArea();
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDEditorWindow.GenerateCSharpFile();
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The <see cref="NWDWebServiceManager"/> is an editor window to parameter <see cref="NetWorkedData"/> in the application final compile.
    /// </summary>
    public class NWDWebServiceManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "app-configuration-manager/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDWebServiceManager _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDWebServiceManager"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDWebServiceManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                //_kSharedInstance = EditorWindow.GetWindow(typeof(NWDWebServiceManager)) as NWDWebServiceManager;
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDWebServiceManager), ShowAsWindow()) as NWDWebServiceManager;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all <see cref="NWDWebServiceManager"/>.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDWebServiceManager));
            foreach (NWDWebServiceManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDWebServiceManager"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDWebServiceManager SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 380;
            NormalizeHeight = 900;
            // set title
            TitleInit(NWDConstants.K_WEBSERVICE_MANAGEMENT_TITLE, typeof(NWDWebServiceManager));
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDWebServiceManagerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
