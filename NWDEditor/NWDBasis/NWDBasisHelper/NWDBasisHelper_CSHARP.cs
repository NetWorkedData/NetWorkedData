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
using UnityEditor;
using System.Reflection;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public string TablePrefixOld = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public void CreationCSHARP_PreCompileOption(bool sCreate)
        {
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            string tCompileFolderPath = NWDToolbox.FindCompileConfigurationFolder();
            string tPathType = tCompileFolderPath + "/" + ClassNamePHP + "_override.cs";
            InitHelper(ClassType, true);
            if (sCreate == false)
            {
                File.WriteAllText(tPathType, "");
            }
            else
            {
                if (ClassNamePHP != "NWDBasis")
                {
                    string tHelperName = this.GetType().Name;

                    if (tHelperName == "NWDBasisHelper")
                    {
                        tHelperName = ClassNamePHP + "Helper";
                    }
                    DateTime tTime = DateTime.UtcNow;
                    string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
                    string tYearString = NWDToolbox.DateTimeYYYY(tTime);

                    StringBuilder rReturn = new StringBuilder(string.Empty);
                    foreach (NWDClassMacroAttribute tMacro in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
                    {
                        rReturn.AppendLine("#if " + tMacro.Macro);
                    }
                    rReturn.AppendLine("//=====================================================================================================================");
                    rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
                    rReturn.AppendLine(NWD.K_CommentCreator);
                    rReturn.AppendLine("//=====================================================================================================================");
                    rReturn.AppendLine("using System;");
                    rReturn.AppendLine("using System.Collections;");
                    rReturn.AppendLine("using System.Collections.Generic;");
                    rReturn.AppendLine("using UnityEngine;");
                    rReturn.AppendLine("using System.Reflection;");
                    rReturn.AppendLine("//=====================================================================================================================");
                    rReturn.AppendLine("namespace NetWorkedData");
                    rReturn.AppendLine("{");
                    rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    rReturn.AppendLine("public partial class " + tHelperName + " : NWDHelper<" + ClassNamePHP + ">");
                    rReturn.AppendLine("{");
                    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                    rReturn.AppendLine("protected override NWDTypeClass CreateInstance_Bypass(bool sInsertInNetWorkedData, bool sStupid, PropertyInfo[] sPropertyInfo)");
                    rReturn.AppendLine("{");

#if NWD_NEVER_NULL_DATATYPE
                    rReturn.AppendLine("" + ClassNamePHP + " rReturn = new " + ClassNamePHP + "(sInsertInNetWorkedData) {");
                    foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataType)) ||
                        tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeInt)) ||
                        tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeEnum)) ||
                        tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeMask)) ||
                        tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            if (tProp.PropertyType.IsGenericType)
                            {
                                List<string> tArguments = new List<string>();
                                foreach (Type tTypeArg in tProp.PropertyType.GenericTypeArguments)
                                {
                                    tArguments.Add(tTypeArg.Name);
                                }
                                rReturn.AppendLine("" + tProp.Name + " = new " + tProp.PropertyType.Name.Replace("`" + tArguments.Count + "", "<" + string.Join(", ", tArguments) + ">") + "(),");
                            }
                            else
                            {
                                rReturn.AppendLine("" + tProp.Name + " = new " + tProp.PropertyType.Name + "(),");
                            }
                        }
                    }
                    rReturn.AppendLine("};");
#else
                        rReturn.AppendLine("" + ClassNamePHP + " rReturn = new " + ClassNamePHP + "(sInsertInNetWorkedData);");
                        rReturn.AppendLine("rReturn.PropertiesMinimal();");
#endif

                    rReturn.AppendLine("return rReturn;");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                    rReturn.AppendLine("public override void InitHelper(Type sType, bool sBase = false)");
                    rReturn.AppendLine("{");
                    rReturn.AppendLine("if (sBase == false)");
                    rReturn.AppendLine("{");
                    if (tApp.OverrideCacheMethodEverywhere == false)
                    {
                        if (tApp.OverrideCacheMethodInPlayMode == false)
                        {
                            rReturn.AppendLine("if (Application.isEditor == false)");
                            rReturn.AppendLine("{");
                        }
                        else
                        {
                            rReturn.AppendLine("if (Application.isEditor == false || Application.isPlaying == true)");
                            rReturn.AppendLine("{");
                        }
                    }
                    else
                    {
                        rReturn.AppendLine("#if UNITY_EDITOR");
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassMenuName) + " = \"" + ClassMenuName + "\";");
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassDescription) + " = \"" + ClassDescription + "\";");
                        rReturn.AppendLine("#endif");
                    }
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => TemplateHelper) + ".SetClassType(typeof(" + ClassNamePHP + "));");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassType) + " = typeof(" + ClassNamePHP + ");");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassTableName) + " = \"" + ClassTableName + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassName) + " = \"" + ClassName + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassNamePHP) + " = \"" + ClassNamePHP + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassPrefBaseKey) + " = \"" + ClassPrefBaseKey + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassTrigramme) + " = \"" + ClassTrigramme + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kLockedObject) + " = " + kLockedObject.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependent) + " = " + kAssetDependent.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClusterMin) + " = " + ClusterMin.ToString() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClusterMax) + " = " + ClusterMax.ToString() + ";");
                    List<string> tPropertiesArrayInString = new List<string>();
                    foreach (PropertyInfo tInf in PropertiesArray)
                    {
                        tPropertiesArrayInString.Add("ClassType.GetProperty(\"" + tInf.Name + "\")");
                    }
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => PropertiesArray) + " = new PropertyInfo[]{" + string.Join(",\n ", tPropertiesArrayInString) + "};");
                    List<string> tNWDPropertiesArrayInString = new List<string>();
                    foreach (PropertyInfo tInf in NWDDataPropertiesArray)
                    {
                        tNWDPropertiesArrayInString.Add("ClassType.GetProperty(\"" + tInf.Name + "\")");
                    }
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => NWDDataPropertiesArray) + " = new PropertyInfo[]{" + string.Join(",\n ", tNWDPropertiesArrayInString) + "};");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependentProperties) + ".Clear();");
                    foreach (PropertyInfo tProp in kAssetDependentProperties)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependentProperties) + ".Add(ClassType.GetProperty(\"" + tProp.Name + "\"));");
                    }
                    // NWDHelper override finish
                    if (tApp.OverrideCacheMethodEverywhere == false)
                    {
                        rReturn.AppendLine("}");
                        rReturn.AppendLine("else");
                        rReturn.AppendLine("{");
                        rReturn.AppendLine("base.InitHelper(sType, true);");
                        rReturn.AppendLine("}");
                    }
                    else
                    {
                        rReturn.AppendLine("base.InitHelper(sType, true);");
                    }
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("else");
                    rReturn.AppendLine("{");
                    rReturn.AppendLine("base.InitHelper(sType, true);");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("//=====================================================================================================================");
                    foreach (NWDClassMacroAttribute tMacro in ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
                    {
                        rReturn.AppendLine("#endif //" + tMacro.Macro);
                    }
                    rReturn.ToString();
                    string rReturnTypeFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
                    File.WriteAllText(tPathType, rReturnTypeFormatted);
                }
            }
            //try
            //{
            //    AssetDatabase.ImportAsset(tPathType, ImportAssetOptions.ForceUpdate);
            //    AssetDatabase.ImportAsset(NWDToolbox.FindCompileConfigurationFolder(), ImportAssetOptions.ForceUpdate);
            //}
            //catch (IOException e)
            //{
            //    Debug.LogException(e);
            //    throw;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARPCallLoader()
        {
            //NWDBenchmark.St art();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine(ClassNamePHP + NWD.K_LOADER + "();");
            //NWDBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARP()
        {
            //NWDBenchmark.Start();
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            PrefLoad();
            CreationCSHARP_PreCompileOption(tApp.OverrideCacheMethod);
            // Write data ...
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + typeof(NWDBasisHelper).Name + " tBasisHelper = null;");
            rReturn.AppendLine("tBasisHelper = " + typeof(NWDBasisHelper).Name + ".FindTypeInfos(\"" + ClassNamePHP + "\");");
            rReturn.AppendLine("if (tBasisHelper!=null)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.TablePrefix) + " = \"" + TablePrefix + "\";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltStart) + " = \"" + SaltStart.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltEnd) + " = \"" + SaltEnd.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltValid) + " = true;"); // salt was reccord because loaded :-p
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.LastWebBuild) + " = " + LastWebBuild + ";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelPropertiesOrder) + ".Clear();");
            ModelAnalyze();
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                foreach (KeyValuePair<int, List<string>> tKeyValue in WebModelPropertiesOrder.OrderBy(x => x.Key))
                {
                    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                    {
                        if (tApp.WSList[tKeyValue.Key] == true)
                        {
                            if (tKeyValue.Key < LastWebBuild)
                            {
                                rReturn.AppendLine("#if UNITY_EDITOR");
                            }
                            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelPropertiesOrder) + ".Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                            if (tKeyValue.Key < LastWebBuild)
                            {
                                rReturn.AppendLine("#endif");
                            }
                        }
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Clear();");

            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                foreach (KeyValuePair<int, int> tKeyValue in WebServiceWebModel.OrderBy(x => x.Key))
                {
                    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                    {
                        if (tApp.WSList[tKeyValue.Key] == true)
                        {
                            if (tKeyValue.Key < LastWebBuild)
                            {
                                rReturn.AppendLine("#if UNITY_EDITOR");
                            }
                            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Add(" + tKeyValue.Key + ", " + tKeyValue.Value + ");");
                            if (tKeyValue.Key < LastWebBuild)
                            {
                                rReturn.AppendLine("#endif");
                            }
                        }
                    }
                }
            }
            if (NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere == false)
            {
                rReturn.AppendLine("#if UNITY_EDITOR");
                if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                {
                    rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.TablePrefixOld) + " = \"" + TablePrefix + "\";");
                }
                else
                {
                    rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.TablePrefixOld) + " = \"\";");
                }
                rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelSQLOrder) + ".Clear();");

                if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                {
                    foreach (KeyValuePair<int, string> tKeyValue in WebModelSQLOrder.OrderBy(x => x.Key))
                    {
                        if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                        {
                            if (tApp.WSList[tKeyValue.Key] == true)
                            {
                                rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelSQLOrder) + ".Add(" + tKeyValue.Key + ", \"" + tKeyValue.Value.Replace("\"", "\\\"") + "\");");
                            }
                        }
                    }
                }
                rReturn.AppendLine("tBasisHelper.ModelAnalyze();");
                rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelChanged) + " = " + ClassNamePHP + ".ModelChanged();");
                rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelDegraded) + " = " + ClassNamePHP + ".ModelDegraded();");
                rReturn.AppendLine("#endif");
            }
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            //NWDBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
