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
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersionHelper : NWDHelper<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDVersion.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// VERSION");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDVersion>().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function versionTest($sVersion)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $admin;");
                tFile.AppendLine("$rReturn = true;");
                tFile.AppendLine("if ($admin == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("return $rReturn;");
                }
                tFile.AppendLine("}");
                if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
                {
                    tFile.AppendLine("$tQuery = 'SELECT * FROM `" + NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Version) + "` = \\''.EscapeString($sVersion).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Buildable) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().ActiveDev) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().XX) + "` = 0 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().AC) + "` = 1;';");
                }
                if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    tFile.AppendLine("$tQuery = 'SELECT * FROM `" + NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Version) + "` = \\''.EscapeString($sVersion).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Buildable) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().ActivePreprod) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().XX) + "` = 0 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().AC) + "` = 1;';");
                }
                if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                {
                    tFile.AppendLine("$tQuery = 'SELECT * FROM `" + NWDBasisHelper.TableNamePHP<NWDVersion>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Version) + "` = \\''.EscapeString($sVersion).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().Buildable) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().ActiveProd) + "` = 1 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().XX) + "` = 0 AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().AC) + "` = 1;';");
                }
                tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery, '', '', false);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA00));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tResult['count'] == 1)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().BlockDataUpdate) + "'] == 1)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA99));
                                    tFile.AppendLine("$rReturn = false;");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().BlockApplication) + "'] == 1)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA01));
                                    tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL) + "']);");
                                    tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL) + "']);");
                                    tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL) + "']);");
                                    tFile.AppendLine("$rReturn = false;");
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_GVA02));
                        tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().OSXStoreURL) + "']);");
                        tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().IOSStoreURL) + "']);");
                        tFile.AppendLine("respondAdd('" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL) + "',$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDVersion>().GooglePlayURL) + "']);");
                        tFile.AppendLine("$rReturn = false;");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
