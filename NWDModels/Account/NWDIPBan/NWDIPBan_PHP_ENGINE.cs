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
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDIPBan.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBan : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {

            string tEnvSync = NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_ENV_SYNC(sEnvironment);

            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            //tFile.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function IPBanOk()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                if (sEnvironment.IPBanActive == true)
                {
                    tFile.AppendLine("$tIPBanMaxTentative = " + sEnvironment.IPBanMaxTentative.ToString() + ";");
                    tFile.AppendLine("$tIPBanTimer = " + sEnvironment.IPBanTimer.ToString() + ";");
                    tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
                    tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE ");
                    tFile.Append("\\''.EscapeString($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` AND ");
                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` >= `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().CounterMaximum) + "` AND ");
                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` >= '.EscapeString(" + NWD.K_PHP_TIME_SYNC + ").' AND ");
                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1");
                    tFile.AppendLine(";';");
                    //tFile.AppendLine("$tResult = SelectFromAllDatabase($tQuery, '', '', false);"); // not necessary, the ipban are copy everywhere
                    tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery, '', '', false);");
                    tFile.AppendLine("if ($tResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in " + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + " ... perhaps no table ?"));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($tResult['count'] > 0)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "YOU ARE BANNED!!!"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_IPB01));
                            tFile.AppendLine("return false;");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
                }
                tFile.AppendLine("return true;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function IPBanAdd()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                if (sEnvironment.IPBanActive == true)
                {
                    tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
                    tFile.AppendLine("$tIPBanMaxTentative = " + sEnvironment.IPBanMaxTentative.ToString() + ";");
                    tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE");
                    tFile.Append(" \\''.EscapeString($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` AND");
                    tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1");
                    tFile.AppendLine(";';");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));

                    tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                    tFile.AppendLine("if ($tResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in " + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + " ... perhaps no table ?"));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($tResult['count'] == 0)");
                        tFile.AppendLine("{");
                        {
                            // Create new iPban ... or reuse old one
                            // insert for the moment! delete all after ... more simple to dev! ... 
                            tFile.AppendLine("$tDeadline = " + NWD.K_PHP_TIME_SYNC + " + " + sEnvironment.IPBanTimer.ToString() + ";");

                            tFile.AppendLine("$tReference = referenceGenerateGlobal ('" + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "');");
                            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` (';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$tReference.'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");

                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "`, '; $tInsertSQLValue.= '\\''.EscapeString($_SERVER['REMOTE_ADDR']).'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().RangeAccess) + "`, '; $tInsertSQLValue.= '\\''.EscapeString($tRangeAccess).'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().CounterMaximum) + "`, '; $tInsertSQLValue.= '\\''.EscapeString($tIPBanMaxTentative).'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "`, ';$tInsertSQLValue.= '\\'1\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "`, ';$tInsertSQLValue.= '\\''.EscapeString($tDeadline).'\\', ';");

                            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
                            {
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Sign\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Dev AccounSign'.$TIME_SYNC.'\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                            }
                            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                            {
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Certified\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Preprod AccountSign'.$TIME_SYNC.'\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                            }
                            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                            {
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                            }
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                            //tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Bundle) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
                            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
                            tFile.AppendLine("$tInsertSQL.=')';");
                            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
                            tFile.AppendLine("$tResult = ExecuteInAllConnexions($tInsertSQL);");
                            tFile.AppendLine("if ($tResult['error'] == true)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in " + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + " ... perhaps no table ?"));
                            }
                            tFile.AppendLine("}");

                            // delete all old data
                            tFile.AppendLine("// Update");
                            tFile.AppendLine("$tQueryDelete = 'DELETE FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` < '." + NWD.K_PHP_TIME_SYNC + ".'';");
                            tFile.AppendLine("$tResultDelete = ExecuteInAllConnexions($tQueryDelete);");
                            tFile.AppendLine("if ($tResultDelete['error'] == true)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in " + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + " ... perhaps no table ?"));
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            // Reuse this old ipban
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "'] ++;");
                                    tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "'] <= " + NWD.K_PHP_TIME_SYNC + ")");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "'] = 1;");
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("// Update");
                                    tFile.AppendLine("$tDeadline = " + NWD.K_PHP_TIME_SYNC + " + " + sEnvironment.IPBanTimer.ToString() + ";");
                                    tFile.Append("$tQueryUpdate = 'UPDATE `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` SET ");
                                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DS) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\' , ");
                                    tFile.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\' , ");
                                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` = '.$tDeadline.' , ");
                                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` = '.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "'].' ");
                                    tFile.Append("WHERE ");
                                    tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "` = '.EscapeString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "']).';");
                                    tFile.AppendLine(";';");
                                    tFile.AppendLine("$tResultUpdate = ExecuteInAllConnexions($tQueryUpdate);");
                                    tFile.AppendLine("if ($tResultUpdate['error'] == true)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in " + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + " ... perhaps no table ?"));
                                    }
                                    tFile.AppendLine("}");
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
