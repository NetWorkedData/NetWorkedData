//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
                //if (File.Exists(tPathType) == true)
                //{
                //    File.Delete(tPathType);
                //}
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
                    rReturn.AppendLine("//=====================================================================================================================");
                    //rReturn.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
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

                    }
                    // NWDHelper override start
                    //rReturn.AppendLine("base.InitHelper(sType);");
                    //rReturn.AppendLine("Debug.Log(\"PLAYING MODE InitHelper()\");");
                    //rReturn.AppendLine("NWEBenchmark.Start();");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassType) + " = typeof(" + ClassNamePHP + ");");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassTableName) + " = \"" + ClassTableName + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassName) + " = \"" + ClassName + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassNamePHP) + " = \"" + ClassNamePHP + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassPrefBaseKey) + " = \"" + ClassPrefBaseKey + "\";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassTrigramme) + " = \"" + ClassTrigramme + "\";");
                    //rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassMenuName) + " = \"" + ClassMenuName + "\";");
                    //rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassDescription) + " = \"" + ClassDescription + "\";");

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassSynchronize) + " = " + ClassSynchronize.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassGameSaveDependent) + " = " + ClassGameSaveDependent.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAccountDependent) + " = " + kAccountDependent.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kLockedObject) + " = " + kLockedObject.ToString().ToLower() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependent) + " = " + kAssetDependent.ToString().ToLower() + ";");

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClusterMin) + " = " + ClusterMin.ToString() + ";");
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => ClusterMax) + " = " + ClusterMax.ToString() + ";");

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAccountDependentProperties) + ".Clear();");
                    foreach (PropertyInfo tProp in kAccountDependentProperties)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => kAccountDependentProperties) + ".Add(ClassType.GetProperty(\"" + tProp.Name + "\"));");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAccountConnectedProperties) + ".Clear();");
                    foreach (PropertyInfo tProp in kAccountConnectedProperties)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => kAccountConnectedProperties) + ".Add(ClassType.GetProperty(\"" + tProp.Name + "\"));");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependentProperties) + ".Clear();");
                    foreach (PropertyInfo tProp in kAssetDependentProperties)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => kAssetDependentProperties) + ".Add(ClassType.GetProperty(\"" + tProp.Name + "\"));");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => IndexInMemoryMethodList) + ".Clear();");
                    foreach (MethodInfo tMethodInfo in IndexInMemoryMethodList)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => IndexInMemoryMethodList) + ".Add(ClassType.GetMethod(\"" + tMethodInfo.Name + "\"));");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => DeindexInMemoryMethodList) + ".Clear();");
                    foreach (MethodInfo tMethodInfo in DeindexInMemoryMethodList)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => DeindexInMemoryMethodList) + ".Add(ClassType.GetMethod(\"" + tMethodInfo.Name + "\"));");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => IndexInBaseMethodList) + ".Clear();");
                    foreach (MethodInfo tMethodInfo in IndexInBaseMethodList)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => IndexInBaseMethodList) + ".Add(ClassType.GetMethod(\"" + tMethodInfo.Name + "\"));");
                    }

                    if (ClassGameDependentProperties != null)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => ClassGameDependentProperties) + " = ClassType.GetProperty(\"" + ClassGameDependentProperties.Name + "\");");
                    }
                    if (GameSaveMethod != null)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => GameSaveMethod) + " = ClassType.GetMethod(\"" + GameSaveMethod.Name + "\");");
                    }

                    rReturn.AppendLine(NWDToolbox.PropertyName(() => AccountMethodDico) + ".Clear();");
                    foreach (KeyValuePair<PropertyInfo, MethodInfo> tPropertyMethodInfo in AccountMethodDico)
                    {
                        rReturn.AppendLine(NWDToolbox.PropertyName(() => AccountMethodDico) + ".Add(ClassType.GetProperty(\"" + tPropertyMethodInfo.Key.Name + "\"),ClassType.GetMethod(\"" + tPropertyMethodInfo.Value.Name + "\"));");
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
                    //if (ClassType.IsConstructedGenericType == false)
                    if (ClassType.IsSubclassOf(typeof(NWDIndexByBase)) == false)
                        {
                        rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        rReturn.AppendLine("public partial class " + ClassNamePHP + " : NWDBasis");
                        rReturn.AppendLine("{");
                        rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                        // NWDBasis override
                        rReturn.AppendLine("public override void IndexInBase()");
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
                        foreach (MethodInfo tMethod in IndexInBaseMethodList)
                        {
                            rReturn.AppendLine(tMethod.Name + "();");
                            rReturn.AppendLine("NWDDataManager.SharedInstance().IndexationCounterOp++;");
                        }
                        if (tApp.OverrideCacheMethodEverywhere == false)
                        {
                            rReturn.AppendLine("}");
                            rReturn.AppendLine("else");
                            rReturn.AppendLine("{");
                            rReturn.AppendLine("base.IndexInBase();");
                            rReturn.AppendLine("}");
                        }
                        rReturn.AppendLine("}");
                        rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                        // NWDBasis override
                        rReturn.AppendLine("public override void DeindexInBase()");
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
                        foreach (MethodInfo tMethod in DeindexInBaseMethodList)
                        {
                            rReturn.AppendLine(tMethod.Name + "();");
                            rReturn.AppendLine("NWDDataManager.SharedInstance().IndexationCounterOp++;");
                        }
                        if (tApp.OverrideCacheMethodEverywhere == false)
                        {
                            rReturn.AppendLine("}");
                            rReturn.AppendLine("else");
                            rReturn.AppendLine("{");
                            rReturn.AppendLine("base.DeindexInBase();");
                            rReturn.AppendLine("}");
                        }
                        rReturn.AppendLine("}");

                        rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                        // NWDBasis override
                        rReturn.AppendLine("public override void IndexInMemory()");
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
                        foreach (MethodInfo tMethod in IndexInMemoryMethodList)
                        {
                            rReturn.AppendLine(tMethod.Name + "();");
                            rReturn.AppendLine("NWDDataManager.SharedInstance().IndexationCounterOp++;");
                        }
                        if (tApp.OverrideCacheMethodEverywhere == false)
                        {
                            rReturn.AppendLine("}");
                            rReturn.AppendLine("else");
                            rReturn.AppendLine("{");
                            rReturn.AppendLine("base.IndexInMemory();");
                            rReturn.AppendLine("}");
                        }
                        rReturn.AppendLine("}");
                        rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                        // NWDBasis override
                        rReturn.AppendLine("public override void DeindexInMemory()");
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
                        foreach (MethodInfo tMethod in DeindexInMemoryMethodList)
                        {
                            rReturn.AppendLine(tMethod.Name + "();");
                            rReturn.AppendLine("NWDDataManager.SharedInstance().IndexationCounterOp++;");
                        }
                        if (tApp.OverrideCacheMethodEverywhere == false)
                        {
                            rReturn.AppendLine("}");
                            rReturn.AppendLine("else");
                            rReturn.AppendLine("{");
                            rReturn.AppendLine("base.DeindexInMemory();");
                            rReturn.AppendLine("}");
                        }
                        rReturn.AppendLine("}");
                        rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                        rReturn.AppendLine("}");
                    }
                    rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    rReturn.AppendLine("}");
                    rReturn.AppendLine("//=====================================================================================================================");
                    rReturn.ToString();
                    string rReturnTypeFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
                    File.WriteAllText(tPathType, rReturnTypeFormatted);
                }
            }
            AssetDatabase.ImportAsset(tPathType, ImportAssetOptions.ForceUpdate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARPCallLoader()
        {
            //NWEBenchmark.St art();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine(ClassNamePHP + NWD.K_LOADER + "();");
            //NWEBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARP()
        {
            //NWEBenchmark.Start();
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            CreationCSHARP_PreCompileOption(tApp.OverrideCacheMethod);
            // Write data ...
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + typeof(NWDBasisHelper).Name + " tBasisHelper = null;");
            //rReturn.AppendLine("// = NWDBasisHelper.FindTypeInfos(typeof(" + ClassNamePHP + "));");
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
            foreach (KeyValuePair<int, List<string>> tKeyValue in WebModelPropertiesOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelPropertiesOrder) + ".Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Clear();");
            foreach (KeyValuePair<int, int> tKeyValue in WebServiceWebModel.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Add(" + tKeyValue.Key + ", " + tKeyValue.Value + ");");
                    }
                }
            }
            if (NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere == false)
            {
                rReturn.AppendLine("#if UNITY_EDITOR");
                rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.TablePrefixOld) + " = \"" + TablePrefix + "\";");
                rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelSQLOrder) + ".Clear();");
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
                rReturn.AppendLine("tBasisHelper.ModelAnalyze();");
                rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelChanged) + " = " + ClassNamePHP + ".ModelChanged();");
                rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelDegraded) + " = " + ClassNamePHP + ".ModelDegraded();");
                rReturn.AppendLine("#endif");
            }
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            //NWEBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif