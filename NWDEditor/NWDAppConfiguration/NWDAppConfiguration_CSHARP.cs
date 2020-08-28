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
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// In anticipation of future development we allow to choose the development language to use on the server.
    /// </summary>
    public enum NWDServerLanguage
    {
        PHP = 10,
        /* Java = 20,*/
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppConfiguration : NWDApp
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate automatically the C# files of configurations 
        /// </summary>
        /// <param name="sEnvironment"></param>
        public void GenerateCSharpFile(NWDAppEnvironment sEnvironment)
        {
            NWDBenchmark.Start();
            if (NWDLauncher.LauncherIsReady())
            {
                // save datas
                NWDDataManager.SharedInstance().DataQueueExecute();
                // generate file editor configuration in specific folder
                GenerateCSharpFile_Editor(sEnvironment);
                // generate file launcher configuration in specific folder
                GenerateCSharpFile_Launcher(sEnvironment);
                // generate file classes restaure configurations in specific folder
                GenerateCSharpFile_Restaure(sEnvironment);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate automatically the C# file of editor configurations in specific folder 
        /// </summary>
        /// <param name="sEnvironment"></param>
        private void GenerateCSharpFile_Editor(NWDAppEnvironment sEnvironment)
        {
            NWDBenchmark.Start();
            string tFindPrivateConfigurationFolder = NWDToolbox.FindPrivateConfigurationFolder();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("#if NWD_VERBOSE");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("#define NWD_LOG");
            rReturn.AppendLine("#define NWD_BENCHMARK");
            rReturn.AppendLine("#elif DEBUG");
            rReturn.AppendLine("#define NWD_LOG");
            rReturn.AppendLine("#define NWD_BENCHMARK");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("//=====================================================================================================================");

            rReturn.AppendLine("using UnityEngine;");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("namespace NetWorkedData");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("public partial class NWDAppConfiguration : NWDApp");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Restaure the configurations ");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("[NWDAppConfigurationRestaure()]");
            rReturn.AppendLine("public bool RestaureEditorConfigurations()");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.BuilderUser) + " = \"" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_USER_BUILDER, "(user)") + "\";");
#if UNITY_EDITOR_OSX
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.CompileOn) + " = \"Mac\";");
#else
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.CompileOn) + " = \"Window/Linux\";");
#endif
            rReturn.AppendLine("//Fort editor preferences");
            rReturn.AppendLine("//Environments select\n");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.DevEnvironment) + "." + NWDToolbox.PropertyName(() => this.DevEnvironment.Selected) + " = " + this.DevEnvironment.Selected.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PreprodEnvironment) + "." + NWDToolbox.PropertyName(() => this.PreprodEnvironment.Selected) + " = " + this.PreprodEnvironment.Selected.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.ProdEnvironment) + "." + NWDToolbox.PropertyName(() => this.ProdEnvironment.Selected) + " = " + this.ProdEnvironment.Selected.ToString().ToLower() + ";");
            rReturn.AppendLine("return true;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//=====================================================================================================================");
            string tPathPrivate = tFindPrivateConfigurationFolder + "/NWDConfigurations_Editor.cs";
            string rReturnPrivateFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
            File.WriteAllText(tPathPrivate, rReturnPrivateFormatted);
            try
            {
                AssetDatabase.ImportAsset(tPathPrivate, ImportAssetOptions.ForceUpdate);
                AssetDatabase.ImportAsset(NWDToolbox.FindPrivateConfigurationFolder(), ImportAssetOptions.ForceUpdate);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                throw;
            }

            NWDBenchmark.Finish();

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate automatically the C# file of configurations of launcher
        /// </summary>
        /// <param name="sEnvironment"></param>
        private void GenerateCSharpFile_Launcher(NWDAppEnvironment sEnvironment)
        {
            NWDBenchmark.Start();

            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);

            string tOwnerConfigurationFolderPath = NWDToolbox.FindOwnerConfigurationFolder();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("#if NWD_VERBOSE");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("#define NWD_LOG");
            rReturn.AppendLine("#define NWD_BENCHMARK");
            rReturn.AppendLine("#elif DEBUG");
            rReturn.AppendLine("#define NWD_LOG");
            rReturn.AppendLine("#define NWD_BENCHMARK");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("using System.Collections;");
            rReturn.AppendLine("using System.Collections.Generic;");
            rReturn.AppendLine("using UnityEngine;");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("namespace NetWorkedData");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("public partial class NWDAppConfiguration : NWDApp");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Restaure the configurations ");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("public override bool RestaureConfigurations ()");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebFolder) + " = \"" + WebFolder + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PurgeOldAccountDatabase) + " = " + PurgeOldAccountDatabase.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.LauncherClassAccountStep) + " = " + NWDDataManager.SharedInstance().ClassAccountDependentList.Count() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.LauncherClassEditorStep) + " = " + NWDDataManager.SharedInstance().ClassNotAccountDependentList.Count() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebFolder) + " = \"" + WebFolder + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TablePrefixe) + " = \"" + TablePrefixe + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebBuild) + " = " + WebBuild + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebBuildMax) + " = " + WebBuildMax + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.DatabasePrefix) + " = \"" + DatabasePrefix + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPass) + " = \"" + NWDToolbox.SaltCleaner(EditorPass) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPassA) + " = \"" + NWDToolbox.SaltCleaner(EditorPassA) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPassB) + " = \"" + NWDToolbox.SaltCleaner(EditorPassB) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSalt) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSalt) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSaltA) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSaltA) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSaltB) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSaltB) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PreloadDatas) + " = " + PreloadDatas.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.BundleDatas) + " = " + BundleDatas.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AutoDeleteTrashDatas) + " = " + AutoDeleteTrashDatas.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.LauncherBenchmark) + " = " + LauncherBenchmark.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.LauncherFaster) + " = " + LauncherFaster.ToString() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.ProjetcLanguage) + " = \"" + ProjetcLanguage + "\";");
            foreach (KeyValuePair<string, string> tEntry in BundleName.OrderBy(x => x.Key))
            {
                rReturn.AppendLine(NWDToolbox.PropertyName(() => this.BundleName) + "[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";");
            }
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WSList) + " = new Dictionary<int, bool>();");
            foreach (KeyValuePair<int, bool> tWS in WSList.OrderBy(x => x.Key))
            {
                if (tWS.Value == true)
                {
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WSList) + ".Add(" + tWS.Key + "," + tWS.Value.ToString().ToLower() + ");");
                }
                else
                {
                    string tWebServiceFolder = NWDAppConfiguration.SharedInstance().OldWebServiceFolder(tWS.Key);
                    string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                    if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == true)
                    {
                        AssetDatabase.DeleteAsset(tOwnerServerFolderPath + "/" + tWebServiceFolder);
                    }
                }
            }
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + " = new Dictionary<int, string>();");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(-1,\"No Tag\");"); // -1
            for (int tI = 0; tI <= NWDAppConfiguration.SharedInstance().TagNumberUser; tI++)
            {
                if (TagList.ContainsKey(tI) == true)
                {
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + tI + ",\"" + TagList[tI] + "\");");
                }
            }
            int tTag = TagNumberUser + 1;
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Internal Created\");"); // 11
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 12
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 13
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 14
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Test for Preprod\");"); // 15 
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Test for Dev\");"); // 16
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Admin Created\");"); // 17
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Device Created\");"); // 18
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Server Created\");"); // 19
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"User Created\");"); // 20
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"TO DELETE\");"); // 21
            rReturn.AppendLine("//Environments restaure\n");
            rReturn.Append(DevEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(PreprodEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(ProdEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.DataLocalizationManager) + "." + NWDToolbox.PropertyName(() => this.DataLocalizationManager.LanguagesString) + " = \"" + this.DataLocalizationManager.LanguagesString + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AnonymousDeviceConnected) + " = " + AnonymousDeviceConnected.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.OverrideCacheMethod) + " = " + OverrideCacheMethod.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.OverrideCacheMethodInPlayMode) + " = " + OverrideCacheMethodInPlayMode.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.OverrideCacheMethodEverywhere) + " = " + OverrideCacheMethodEverywhere.ToString().ToLower() + ";");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.SlackWebhookURL) + " = \"" + SlackWebhookURL + "\";");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("RestaureEditorConfigurations();");
            rReturn.AppendLine("return true;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//=====================================================================================================================");
            string tPath = tOwnerConfigurationFolderPath + "/NWDConfigurations.cs";
            string rReturnFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
            File.WriteAllText(tPath, rReturnFormatted);
            try
            {
                AssetDatabase.ImportAsset(tPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.ImportAsset(NWDToolbox.FindOwnerConfigurationFolder(), ImportAssetOptions.ForceUpdate);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                throw;
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate automatically the C# file to restaure configurations
        /// </summary>
        /// <param name="sEnvironment"></param>
        private void GenerateCSharpFile_Restaure(NWDAppEnvironment sEnvironment)
        {
            NWDBenchmark.Start();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            StringBuilder rReturn = new StringBuilder(string.Empty);
            string tOwnerConfigurationFolderPath = NWDToolbox.FindOwnerConfigurationFolder();
            StringBuilder rReturnType = new StringBuilder(string.Empty);
            rReturnType.AppendLine("//=====================================================================================================================");
            rReturnType.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturnType.AppendLine(NWD.K_CommentCreator);
            rReturnType.AppendLine("//=====================================================================================================================");
            rReturnType.AppendLine("#if NWD_VERBOSE");
            rReturnType.AppendLine("#if UNITY_EDITOR");
            rReturnType.AppendLine("#define NWD_LOG");
            rReturnType.AppendLine("#define NWD_BENCHMARK");
            rReturnType.AppendLine("#elif DEBUG");
            rReturnType.AppendLine("#define NWD_LOG");
            rReturnType.AppendLine("#define NWD_BENCHMARK");
            rReturnType.AppendLine("#endif");
            rReturnType.AppendLine("#endif");
            rReturnType.AppendLine("//=====================================================================================================================");
            rReturnType.AppendLine("using System.Collections;");
            rReturnType.AppendLine("using System.Collections.Generic;");
            rReturnType.AppendLine("using System.Reflection;");
            rReturnType.AppendLine("using UnityEngine;");
            rReturnType.AppendLine("//=====================================================================================================================");
            rReturnType.AppendLine("namespace NetWorkedData");
            rReturnType.AppendLine("{");
            rReturnType.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturnType.AppendLine("public partial class NWDAppConfiguration : NWDApp");
            rReturnType.AppendLine("{");
            rReturnType.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturnType.AppendLine("/// <summary>");
            rReturnType.AppendLine("/// Restaure the configurations Types by Types");
            rReturnType.AppendLine("/// </summary>");
            rReturnType.AppendLine("public override bool RestaureTypesConfigurations()");
            rReturnType.AppendLine("{");
            rReturnType.AppendLine("if (NWDLauncher.ActiveBenchmark == true) {NWDBenchmark.Start();};");

            rReturnType.AppendLine("foreach (MethodInfo tMethod in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))");
            rReturnType.AppendLine("{");
            {
                rReturnType.AppendLine("if (tMethod.GetCustomAttributes(typeof(" + typeof(NWDAppConfigurationRestaure).Name + "), true).Length > 0)");
                rReturnType.AppendLine("{");
                {
                    rReturnType.AppendLine("tMethod.Invoke(this, null);");
                }
                rReturnType.AppendLine("}");
            }
            rReturnType.AppendLine("}");

            List<Type> tAllTypes = new List<Type>(NWDLauncher.AllNetWorkedDataTypes);
            tAllTypes.Sort((tA, tB) => string.Compare(tA.Name, tB.Name, StringComparison.Ordinal));
            rReturnType.AppendLine("if (NWDLauncher.ActiveBenchmark == true) {NWDBenchmark.Finish();};");
            rReturnType.AppendLine("return true;");
            rReturnType.AppendLine("}");
            rReturnType.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturnType.AppendLine("}");
            rReturnType.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturnType.AppendLine("}");
            rReturnType.AppendLine("//=====================================================================================================================");
            string tPathType = tOwnerConfigurationFolderPath + "/NWDConfigurations_Classes.cs";
            string rReturnTypeFormatted = NWDToolbox.CSharpFormat(rReturnType.ToString());
            File.WriteAllText(tPathType, rReturnTypeFormatted);
            foreach (Type tType in tAllTypes)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                if (tDatas != null)
                {
                    StringBuilder rReturnClass = new StringBuilder(string.Empty);
                    foreach (NWDClassMacroAttribute tMacro in tDatas.ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
                    {
                        rReturnClass.AppendLine("#if " + tMacro.Macro);
                    }
                    rReturnClass.AppendLine("//=====================================================================================================================");
                    rReturnClass.AppendLine(NWD.K_CommentCopyright + tYearString);
                    rReturnClass.AppendLine(NWD.K_CommentCreator);
                    rReturnClass.AppendLine("//=====================================================================================================================");
                    rReturnClass.AppendLine("#if NWD_VERBOSE");
                    rReturnClass.AppendLine("#if UNITY_EDITOR");
                    rReturnClass.AppendLine("#define NWD_LOG");
                    rReturnClass.AppendLine("#define NWD_BENCHMARK");
                    rReturnClass.AppendLine("#elif DEBUG");
                    rReturnClass.AppendLine("#define NWD_LOG");
                    rReturnClass.AppendLine("#define NWD_BENCHMARK");
                    rReturnClass.AppendLine("#endif");
                    rReturnClass.AppendLine("#endif");
                    rReturnClass.AppendLine("//=====================================================================================================================");
                    rReturnClass.AppendLine("using System.Collections;");
                    rReturnClass.AppendLine("using System.Collections.Generic;");
                    rReturnClass.AppendLine("using UnityEngine;");
                    rReturnClass.AppendLine("//=====================================================================================================================");
                    rReturnClass.AppendLine("namespace NetWorkedData");
                    rReturnClass.AppendLine("{");
                    rReturnClass.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    rReturnClass.AppendLine("public partial class NWDAppConfiguration : NWDApp");
                    rReturnClass.AppendLine("{");
                    rReturnClass.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                    rReturnClass.AppendLine("/// <summary>");
                    rReturnClass.AppendLine("/// Restaure the configurations for " + tDatas.ClassNamePHP + "");
                    rReturnClass.AppendLine("/// </summary>");
                    rReturnClass.AppendLine("[" + typeof(NWDAppConfigurationRestaure).Name + "]");
                    rReturnClass.AppendLine(tDatas.CreationCSHARP());
                    rReturnClass.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                    rReturnClass.AppendLine("}");
                    rReturnClass.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    rReturnClass.AppendLine("}");
                    rReturnClass.AppendLine("//=====================================================================================================================");
                    foreach (NWDClassMacroAttribute tMacro in tDatas.ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
                    {
                        rReturnClass.AppendLine("#endif //" + tMacro.Macro);
                    }
                    string tPathTypeClass = tOwnerConfigurationFolderPath + "/NWDConfigurations_" + tDatas.ClassNamePHP + ".cs";
                    string rReturnClassFormatted = NWDToolbox.CSharpFormat(rReturnClass.ToString());
                    File.WriteAllText(tPathTypeClass, rReturnClassFormatted);
                }
            }
            try
            {
                AssetDatabase.ImportAsset(tPathType, ImportAssetOptions.ForceUpdate);
                AssetDatabase.ImportAsset(NWDToolbox.FindOwnerConfigurationFolder(), ImportAssetOptions.ForceUpdate);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                throw;
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif