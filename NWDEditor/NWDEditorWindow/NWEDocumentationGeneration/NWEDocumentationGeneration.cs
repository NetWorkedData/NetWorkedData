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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //  https://stackoverflow.com/questions/15602606/programmatically-get-summary-comments-at-runtime
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEDocumentationGenerationComputer
    {
        //-------------------------------------------------------------------------------------------------------------
        const string NO_DESCRIPTION = "No description";
        //-------------------------------------------------------------------------------------------------------------
        List<string> NamesList = new List<string>();
        List<string> PublicMethods = new List<string>();
        List<string> PrivateMethods = new List<string>();
        List<string> PublicProperties = new List<string>();
        List<string> PrivateProperties = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public static Texture2D DeCompress(Texture2D sSource)
        {
            RenderTexture tRenderTexture = RenderTexture.GetTemporary(sSource.width, sSource.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(sSource, tRenderTexture);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = tRenderTexture;
            Texture2D readableText = new Texture2D(sSource.width, sSource.height);
            readableText.ReadPixels(new Rect(0, 0, tRenderTexture.width, tRenderTexture.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(tRenderTexture);
            return readableText;
        }
        //-------------------------------------------------------------------------------------------------------------
        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string tAttributeMarkDown(Attribute sInfos)
        {
            StringBuilder rMarkDown = new StringBuilder();
            string tDescription = NO_DESCRIPTION;
            rMarkDown.Append("|" + sInfos.GetType().Name + "|" + tDescription + "| ");
            return rMarkDown.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string PropertiesInfoMarkDown(Type sType, BindingFlags sFlags)
        {
            PropertyInfo[] tProperties = sType.GetProperties(sFlags).OrderBy(x => x.Name).ToArray();
            StringBuilder rMarkDown = new StringBuilder();
            if (tProperties.Length > 0)
            {
                if (sFlags.HasFlag(BindingFlags.Public))
                {
                    if (sFlags.HasFlag(BindingFlags.Static))
                    {
                        rMarkDown.AppendLine("## Static public properties");
                    }
                    else
                    {
                        rMarkDown.AppendLine("## Instance public properties");
                    }
                    rMarkDown.AppendLine("");
                }
                if (sFlags.HasFlag(BindingFlags.NonPublic))
                {
                    if (sFlags.HasFlag(BindingFlags.Static))
                    {
                        rMarkDown.AppendLine("## Static private properties");
                    }
                    else
                    {
                        rMarkDown.AppendLine("## Instance private properties");
                    }
                    rMarkDown.AppendLine("");
                }
                foreach (PropertyInfo tInfos in tProperties)
                {
                    rMarkDown.AppendLine(PropertyInfoMarkDown(tInfos, sFlags.HasFlag(BindingFlags.Instance)));
                }
            }
            return rMarkDown.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string MethodsInfoMarkDown(Type sType, BindingFlags sFlags)
        {
            MethodInfo[] tProperties = sType.GetMethods(sFlags).OrderBy(x => x.Name).ToArray();
            StringBuilder rMarkDown = new StringBuilder();
            if (tProperties.Length > 0)
            {
                if (sFlags.HasFlag(BindingFlags.Public))
                {
                    if (sFlags.HasFlag(BindingFlags.Static))
                    {
                        rMarkDown.AppendLine("## Static public methods");
                    }
                    else
                    {
                        rMarkDown.AppendLine("## Instance public methods");
                    }
                    rMarkDown.AppendLine("");
                }
                if (sFlags.HasFlag(BindingFlags.NonPublic))
                {
                    if (sFlags.HasFlag(BindingFlags.Static))
                    {
                        rMarkDown.AppendLine("## Static private methods");
                    }
                    else
                    {
                        rMarkDown.AppendLine("## Instance private methods");
                    }
                    rMarkDown.AppendLine("");
                }
                foreach (MethodInfo tInfos in tProperties)
                {
                    rMarkDown.AppendLine(MethodInfoMarkDown(tInfos, sFlags.HasFlag(BindingFlags.Instance)));
                }
            }
            return rMarkDown.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string TypeName(Type sType)
        {
            string rReturn = string.Empty;
            if (sType != null)
            {
                if (sType.IsGenericType)
                {
                    List<string> tArguments = new List<string>();
                    foreach (Type tTypeArg in sType.GenericTypeArguments)
                    {
                        tArguments.Add(TypeName(tTypeArg));
                    }
                    rReturn = sType.Name.Replace("`" + tArguments.Count, "");
                    if (NamesList.Contains(rReturn))
                    {
                        rReturn = "[" + rReturn + "](./" + rReturn + ".md)";
                    }
                    rReturn = rReturn + "<" + string.Join(", ", tArguments) + ">";
                }
                else
                {
                    rReturn = sType.Name;
                    if (NamesList.Contains(rReturn))
                    {
                        rReturn = "[" + rReturn + "](./" + rReturn + ".md)";
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string PropertyInfoMarkDown(PropertyInfo sInfos, bool sStatic)
        {
            StringBuilder rMarkDown = new StringBuilder();
            rMarkDown.AppendLine("<a name=\"" + sInfos.Name + "\"></a>");
            MethodInfo setMethod = sInfos.GetSetMethod();
            if (setMethod != null)
            {
                if (sStatic)
                {
                    rMarkDown.AppendLine("#### Property static public " + TypeName(sInfos.PropertyType) + " " + sInfos.Name);
                }
                else
                {
                    rMarkDown.AppendLine("#### Property public " + TypeName(sInfos.PropertyType) + " " + sInfos.Name);
                }
            }
            else
            {
                if (sStatic)
                {
                    rMarkDown.AppendLine("#### Property static private " + TypeName(sInfos.PropertyType) + " " + sInfos.Name);
                }
                else

                {
                    rMarkDown.AppendLine("#### Property private " + TypeName(sInfos.PropertyType) + " " + sInfos.Name);
                }
            }
            rMarkDown.AppendLine("");
            string tDescription = NO_DESCRIPTION;
            rMarkDown.AppendLine("" + tDescription + "");
            rMarkDown.AppendLine("");
            rMarkDown.AppendLine("| Type | Description |");
            rMarkDown.AppendLine("|---|---|");
            string tDescriptionType = NO_DESCRIPTION;
            rMarkDown.AppendLine("|" + TypeName(sInfos.PropertyType) + "|" + tDescriptionType + "|");

            if (sInfos.GetCustomAttributes(true).Length > 0)
            {
                rMarkDown.AppendLine("");
                rMarkDown.AppendLine("| Attribut | Paramaters |");
                rMarkDown.AppendLine("|---|---|");
                foreach (Attribute tAttribute in sInfos.GetCustomAttributes(true))
                {
                    rMarkDown.AppendLine(tAttributeMarkDown(tAttribute));
                }
            }
            if (setMethod != null)
            {
                PublicProperties.Add("[" + sInfos.Name + "](#" + sInfos.Name + ")");
            }
            else
            {
                PrivateProperties.Add("[" + sInfos.Name + "](#" + sInfos.Name + ")");
            }
            return rMarkDown.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string MethodInfoMarkDown(MethodInfo sInfos, bool sStatic)
        {
            StringBuilder rMarkDown = new StringBuilder();
            rMarkDown.AppendLine("<a name=\"" + sInfos.Name + "\"></a>");
            if (sInfos.IsPublic)
            {
                if (sStatic)
                {
                    rMarkDown.Append("#### Method static public " + TypeName(sInfos.ReturnType).Replace("Void", "void") + " " + sInfos.Name + "(");
                }
                else
                {
                    rMarkDown.Append("#### Method public " + TypeName(sInfos.ReturnType).Replace("Void", "void") + " " + sInfos.Name + "(");
                }
            }
            else
            {
                if (sStatic)
                {
                    rMarkDown.Append("#### Method static private " + TypeName(sInfos.ReturnType).Replace("Void", "void") + " " + sInfos.Name + "(");
                }
                else
                {
                    rMarkDown.Append("#### Method private " + TypeName(sInfos.ReturnType).Replace("Void", "void") + " " + sInfos.Name + "(");
                }
            }
            foreach (ParameterInfo tParam in sInfos.GetParameters())
            {
                rMarkDown.Append(TypeName(tParam.ParameterType) + " " + tParam.Name + ", ");
            }
            rMarkDown.AppendLine(")");
            rMarkDown.Replace(", )", ")");
            rMarkDown.AppendLine("");
            string tDescription = NO_DESCRIPTION;
            rMarkDown.AppendLine("" + tDescription + "");
            rMarkDown.AppendLine("");
            rMarkDown.AppendLine("| Return Type | Description |");
            rMarkDown.AppendLine("|---|---|");
            string tDescriptionType = NO_DESCRIPTION;
            rMarkDown.AppendLine("|" + sInfos.ReturnType.Name + "|" + tDescriptionType + "|");
            if (sInfos.GetParameters().Length > 0)
            {
                rMarkDown.AppendLine("");
                rMarkDown.AppendLine("| Parameter | Type | Description |");
                rMarkDown.AppendLine("|---|---|---|");
            }
            foreach (ParameterInfo tParam in sInfos.GetParameters())
            {
                string tDescriptionParam = NO_DESCRIPTION;
                rMarkDown.AppendLine("| " + tParam.Name + " |" + TypeName(tParam.ParameterType) + "|" + tDescriptionParam + "|");
            }

            if (sInfos.GetCustomAttributes(true).Length > 0)
            {
                rMarkDown.AppendLine("");
                rMarkDown.AppendLine("| Attribut | Paramaters |");
                rMarkDown.AppendLine("|---|---|");
                foreach (Attribute tAttribute in sInfos.GetCustomAttributes(true))
                {
                    rMarkDown.AppendLine(tAttributeMarkDown(tAttribute));
                }
            }
            if (sInfos.IsPublic)
            {
                PublicMethods.Add("[" + sInfos.Name + "](#" + sInfos.Name + ")");
            }
            else
            {
                PrivateMethods.Add("[" + sInfos.Name + "](#" + sInfos.Name + ")");
            }
            return rMarkDown.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Compute()
        {
            Debug.Log("Compute");
            string ProjectDocDirectory = Application.dataPath;
            ProjectDocDirectory = ProjectDocDirectory.Replace("/Assets", "/MarkDown");
            Directory.CreateDirectory(ProjectDocDirectory);
            Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "NetWorkedData");
            for (int i = 0; i < typelist.Length; i++)
            {
                Type tType = typelist[i];
                if (tType.IsGenericType)
                {
                    List<string> tArguments = new List<string>();
                    NamesList.Add(tType.Name.Replace("`" + tArguments.Count, ""));
                }
                else
                {
                    NamesList.Add(tType.Name);
                }
            }
            // Ok I have All Name
            for (int i = 0; i < typelist.Length; i++)
            {
                Type tType = typelist[i];

                string PathName = "error";
                if (tType.IsGenericType)
                {
                    List<string> tArguments = new List<string>();
                    PathName = tType.Name.Replace("`" + tArguments.Count, "");
                }
                else
                {
                    PathName = tType.Name;
                }
                StringBuilder rMarkDown = new StringBuilder();
                rMarkDown.Append("# " + TypeName(tType));
                rMarkDown.Append(" : " + TypeName(tType.BaseType));
                rMarkDown.AppendLine("");
                rMarkDown.AppendLine("-------Summary-------");
                rMarkDown.AppendLine("");

                if (tType.IsSubclassOf(typeof(NWDBasis)) && tType != typeof(NWDBasis))
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    if (tHelper != null)
                    {
                        rMarkDown.AppendLine("## Particularities");
                        rMarkDown.AppendLine("");
                        rMarkDown.AppendLine("| Particularity | Parameters |");
                        rMarkDown.AppendLine("|---|---|");
                        if (tHelper.TextureOfClass() != null)
                        {
                            Texture2D tIcon = tHelper.TextureOfClass();
                            Texture2D tTexture = DeCompress(tIcon);
                            byte[] tBytes = tTexture.EncodeToPNG();
                            File.WriteAllBytes(ProjectDocDirectory + "/" + PathName + ".png", tBytes);
                            rMarkDown.AppendLine("|Icon|![ICON](./" + PathName + ".png)|");
                            rMarkDown.AppendLine("|Icon|<img style=\"width:64px\" src=\"./" + PathName + ".png\"></img >|");
                        }
                        rMarkDown.AppendLine("|Class Name|" + tHelper.ClassName + "|");
                        rMarkDown.AppendLine("|Class Menu Name|" + tHelper.ClassMenuName + "|");
                        rMarkDown.AppendLine("|Class Description|" + tHelper.ClassDescription + "|");
                        rMarkDown.AppendLine("|Class Trigramme|" + tHelper.ClassTrigramme + "|");


                        if (tHelper.TemplateHelper != null)
                        {
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateDeviceDatabase)) + "|" + tHelper.TemplateHelper.GetDeviceDatabase().ToString() + "|");
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateClusterDatabase)) + "|" + tHelper.TemplateHelper.GetSynchronizable().ToString() + "|");
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateBundlisable)) + "|" + tHelper.TemplateHelper.GetBundlisable().ToString() + "|");
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateAccountDependent)) + "|" + tHelper.TemplateHelper.GetAccountDependent().ToString() + "|");
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateGameSaveDependent)) + "|" + tHelper.TemplateHelper.GetGamesaveDependent().ToString() + "|");
                            rMarkDown.AppendLine("|" + TypeName(typeof(NWDTemplateGameSaveDependent)) + "|" + tHelper.TemplateHelper.GetGamesaveDependent().ToString() + "|");
                        }
                    }
                }

                //if (tType.GetCustomAttributes(true).Length > 0)
                //{
                //    rMarkDown.AppendLine("");
                //    rMarkDown.AppendLine("| Attributes | Parameters |");
                //    rMarkDown.AppendLine("|---|---|");
                //    foreach (Attribute tAttribute in tType.GetCustomAttributes(true))
                //    {
                //        rMarkDown.AppendLine(tAttributeMarkDown(tAttribute));
                //    }
                //}


                if (tType.IsEnum)
                {
                    rMarkDown.AppendLine("## Enum values");
                    rMarkDown.AppendLine("");
                    rMarkDown.AppendLine("|Name|Value|Description|");
                    rMarkDown.AppendLine("|---|---|---|");
                    string[] tNames = Enum.GetNames(tType);
                    for (int tI = 0; tI < tNames.Length; tI++)
                    {
                        object tV = Enum.Parse(tType, tNames[tI], false);
                        string tVi = Enum.Format(tType, tV, "d");
                        if (tType.GetCustomAttributes<FlagsAttribute>().Count() > 0)
                        {
                            tVi = Enum.Format(tType, tV, "x");
                        }
                        rMarkDown.AppendLine("|" + tNames[tI] + " | " + tVi + " | description |");
                    }
                    //int[] tIns = Enum.GetValues(tType) as int[];
                    //for (int tI = 0; tI < tNames.Length; tI++)
                    //{
                    //    if (tIns.Length < tI)
                    //    {
                    //        rMarkDown.AppendLine("|" + tNames[tI] + " | " + tIns[tI] + " | --- description --- |");
                    //    }
                    //    else
                    //    {
                    //        rMarkDown.AppendLine("|" + tNames[tI] + " | error | --- description --- |");
                    //    }
                    //}
                }
                else
                {
                    if (tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Length > 0 ||
                        tType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly).Length > 0)
                    {
                        //rMarkDown.AppendLine("## Static");
                        //rMarkDown.AppendLine("");
                        rMarkDown.Append(PropertiesInfoMarkDown(tType, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(PropertiesInfoMarkDown(tType, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(MethodsInfoMarkDown(tType, BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(MethodsInfoMarkDown(tType, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly));
                    }

                    if (tType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length > 0 ||
                        tType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length > 0)
                    {
                        //rMarkDown.AppendLine("## Instance");
                        //rMarkDown.AppendLine("");
                        rMarkDown.Append(PropertiesInfoMarkDown(tType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(PropertiesInfoMarkDown(tType, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(MethodsInfoMarkDown(tType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                        rMarkDown.Append(MethodsInfoMarkDown(tType, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                    }
                }
                // ok insert summary
                StringBuilder tSummary = new StringBuilder();
                if (PublicMethods.Count > 0 || PrivateMethods.Count > 0 || PublicProperties.Count > 0 || PrivateProperties.Count > 0)
                {
                    tSummary.AppendLine("## Summary");
                    tSummary.AppendLine("");
                    if (PublicProperties.Count > 0 || PrivateProperties.Count > 0)
                    {
                        tSummary.AppendLine("### Properties");
                        tSummary.AppendLine("|Public|Private|");
                        tSummary.AppendLine("|---|---|");
                        int tMax = Math.Max(PublicProperties.Count, PrivateProperties.Count);
                        for (int tk = PublicProperties.Count; tk < tMax; tk++)
                        {
                            PublicProperties.Add("");
                        }
                        for (int tk = PrivateProperties.Count; tk < tMax; tk++)
                        {
                            PrivateProperties.Add("");
                        }
                        for (int tk = 0; tk < tMax; tk++)
                        {
                            tSummary.AppendLine("|" + PublicProperties[tk] + "|" + PrivateProperties[tk] + "|");
                        }
                    }
                    if (PublicMethods.Count > 0 || PrivateMethods.Count > 0)
                    {
                        tSummary.AppendLine("### Methods");
                        tSummary.AppendLine("|Public|Private|");
                        tSummary.AppendLine("|---|---|");
                        int tMax = Math.Max(PublicMethods.Count, PrivateMethods.Count);
                        for (int tk = PublicMethods.Count; tk < tMax; tk++)
                        {
                            PublicMethods.Add("");
                        }
                        for (int tk = PrivateMethods.Count; tk < tMax; tk++)
                        {
                            PrivateMethods.Add("");
                        }
                        for (int tk = 0; tk < tMax; tk++)
                        {
                            tSummary.AppendLine("|" + PublicMethods[tk] + "|" + PrivateMethods[tk] + "|");
                        }
                    }
                }
                rMarkDown.Replace("-------Summary-------", tSummary.ToString());
                PublicMethods.Clear();
                PrivateMethods.Clear();
                PublicProperties.Clear();
                PrivateProperties.Clear();
                File.WriteAllText(ProjectDocDirectory + "/" + PathName + ".md", rMarkDown.ToString());
                Debug.Log(rMarkDown.ToString());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEDocumentationGenerationContent : NWDEditorWindowContent
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
        private static NWEDocumentationGenerationContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        private NWEDocumentationGenerationComputer Computer = new NWEDocumentationGenerationComputer();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWEDocumentationGenerationContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWEDocumentationGenerationContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AnalyzePassword(string sPassword)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
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
            NWDGUILayout.Title("Code Source analyzer");
            if (GUILayout.Button("Analyze"))
            {
                Computer.Compute();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEDocumentationGenerationWindow : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "password-tester/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWEDocumentationGenerationWindow _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstance"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWEDocumentationGenerationWindow SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWEDocumentationGenerationWindow), ShowAsWindow()) as NWEDocumentationGenerationWindow;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWEDocumentationGenerationWindow"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all <see cref="NWEDocumentationGenerationWindow"/>.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWEDocumentationGenerationWindow));
            foreach (NWEDocumentationGenerationWindow tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 350;
            NormalizeHeight = 700;
            // set title
            TitleInit("Password analyzer", typeof(NWEDocumentationGenerationWindow));
            NWEDocumentationGenerationContent.SharedInstance().OnEnable(this);
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
            NWEDocumentationGenerationContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
