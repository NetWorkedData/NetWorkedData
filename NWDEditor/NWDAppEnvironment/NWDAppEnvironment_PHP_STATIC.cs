//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticFinishFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// FINISH");
            tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "Calculate"));
            tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "Finish"));
            tFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode == true)
            {
                tFile.AppendLine("//Insert logs for acces database debug");
                tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                tFile.AppendLine("global $SQL_ACCESS_SQL;");
                tFile.AppendLine("respondAdd('SQL_ACCESS_COUNT', $SQL_ACCESS_COUNT);");
                tFile.AppendLine("respondAdd('SQL_ACCESS_SQL', $SQL_ACCESS_SQL);");
            }
            tFile.AppendLine("// prevent include from function for exit (typical example: error('XXX', true);)");
            tFile.AppendLine("global $admin," + NWD.K_SQL_CON_EDITOR + ", $NWD_SLT_TMP, $NWD_TMA, $RRR_LOG, $REP, $WSBUILD, " + NWD.K_PHP_TIME_SYNC + ", $REF_NEEDED, $ACC_NEEDED, " + NWD.K_ENV + ", $NWD_SHA_VEC, $NWD_SHA_SEC, $NWD_SLT_STR, $NWD_SLT_END;");
            tFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode == true)
            {
                tFile.AppendLine("// add log");
                tFile.AppendLine(NWDError.FUNCTIONPHP_respond + "();");
            }
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// web-services build");
            tFile.AppendLine("respondAdd('wsbuild',$WSBUILD);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("//disconnect mysql");
            tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$tConnexion)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("mysqli_close($tConnexion);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// Insert error if necessary");
            tFile.AppendLine("" + NWDError.FUNCTIONPHP_errorResult + "();");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// server benchmark");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_AVG_KEY + "', intval(sys_getloadavg()[0]*100));");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_PERFORM_KEY + "',microtime(true)-$NWD_TMA);");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_PERFORM_REQUEST_KEY + "',microtime(true)-$_SERVER['REQUEST_TIME_FLOAT']);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// server benchmark");
            tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "Finish"));
            tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "All"));
            tFile.AppendLine("BenchmarkResult();");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("//transform respond in JSON file");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("$temporalSalt = saltTemporal($NWD_SLT_TMP, 0);");
            tFile.AppendLine("if (isset($REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("header('" + NWD.HashKey + ": '.sha1($temporalSalt.$NWD_SHA_VEC.$REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "']));");
            tFile.AppendLine("header('" + NWD.K_WEB_REQUEST_TOKEN_KEY + ": '.$REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "']);");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("$json = json_encode($REP);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("if (respondIsset('" + NWD.K_JSON_SECURE_KEY + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("header('" + NWEUnityWebService.SecureKey + ": " + NWEUnityWebService.SecureDigestKey + "');");
            if (LogMode == true)
            {
                tFile.AppendLine("$REPSCR['" + NWD.K_WEB_LOG_Key + "'] = $REP['" + NWD.K_WEB_LOG_Key + "'];");
            }
            tFile.AppendLine("$REPSCR['" + NWEUnityWebService.SecureKey + "'] = aes128Encrypt( $json, $NWD_SHA_SEC, $NWD_SHA_VEC);");
            tFile.AppendLine("$REPSCR['" + NWEUnityWebService.SecureDigestKey + "'] = sha1($NWD_SLT_STR.$REPSCR['" + NWEUnityWebService.SecureKey + "'].$NWD_SLT_END);");
            tFile.AppendLine("$json = json_encode($REPSCR);");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// write JSON");
            tFile.AppendLine("echo($json);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_FINISH_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticFunctionsFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {

            //NWEBenchmark.Start();
            string tSignAccountKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account);
            string tSignHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash);
            string tRescueHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash);
            string tAC = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC);
            string tReference = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference);
            string tBan = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Ban);


            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// Functions");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("include_once ('" + NWD.K_STATIC_RESPOND_PHP + "');");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDError>().PHP_ENGINE_PATH(this) + ");");
            tFile.AppendLine("include_once ('" + NWD.K_STATIC_VALUES_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------

            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccount>().PHP_SYNCHRONISATION_PATH(this) + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// admin ?");
            tFile.AppendLine("$admin = false;");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// ban account ?");
            tFile.AppendLine("$ban = false;");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function EscapeString($sString)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("return str_replace(array('\\\\', '\\0', '\\n', '\\r', '\\'', '\"', '\\x1a'), array('\\\\\\\\', '\\\\0', '\\\\n', '\\\\r', '\\\\\\'', '\\\\\"', '\\\\Z'), $sString); ");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function BenchmarkStart($sKey)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $BenchmarkCount;");
                tFile.AppendLine("global $BenchmarkDico;");
                tFile.AppendLine("global $BenchmarkDicoStep;");
                tFile.AppendLine("global $BenchmarkResult;");
                tFile.AppendLine("if (isset($BenchmarkDico) == false) ");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$BenchmarkDico = array();");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("if (isset($BenchmarkResult) == false) ");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$BenchmarkResult = array();");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$BenchmarkDico[$sKey] = microtime(true);");
                tFile.AppendLine("$BenchmarkDicoStep[$sKey] = microtime(true);");
                tFile.AppendLine("$R = '';");
                tFile.AppendLine("for ($i = 0;$i <$BenchmarkCount;$i++){$R.= '|    ';}");
                tFile.AppendLine("$BenchmarkResult[] = $R.' Start : '.$sKey;");
                tFile.AppendLine("$BenchmarkCount++;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function BenchmarkStep($sKey, $sInfos, $sLine)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $BenchmarkCount;");
                tFile.AppendLine("global $BenchmarkDico;");
                tFile.AppendLine("global $BenchmarkDicoStep;");
                tFile.AppendLine("global $BenchmarkResult;");
                tFile.AppendLine("if (isset($BenchmarkDicoStep[$sKey]) == false) ");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$BenchmarkResult[] = 'error no start for '.$sKey.' at line '.$sLine;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$R = '';");
                    tFile.AppendLine("for ($i = 0;$i <$BenchmarkCount-1;$i++){$R.= '|    ';}");

                    tFile.AppendLine("$BenchmarkResult[] = $R.'| '.$sInfos;");
                    tFile.AppendLine("$tT = (microtime(true)-$BenchmarkDicoStep[$sKey]);");
                    tFile.AppendLine("if ($tT < 0.001) ");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'- <color=green>'.number_format($tT,6).'s</color> Step : '.$sKey.' at line '.$sLine;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else if ($tT < 0.01) ");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'- <color=orange>'.number_format($tT,6).'s</color> Step : '.$sKey.' at line '.$sLine;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'- <color=red>'.number_format($tT,6).'s</color> Step : '.$sKey.' at line '.$sLine;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("$BenchmarkResult[] = $R.'| ';");
                    tFile.AppendLine("$BenchmarkDicoStep[$sKey] = microtime(true);");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function BenchmarkFinish($sKey)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $BenchmarkCount;");
                tFile.AppendLine("global $BenchmarkDico;");
                tFile.AppendLine("global $BenchmarkResult;");
                tFile.AppendLine("if (isset($BenchmarkDico[$sKey]) == false) ");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$BenchmarkResult[] = 'error no start for '.$sKey;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$BenchmarkCount--;");
                    tFile.AppendLine("$R = '';");
                    tFile.AppendLine("for ($i = 0;$i <$BenchmarkCount;$i++){$R.= '|    ';}");
                    tFile.AppendLine("$BenchmarkResult[] = $R.'| ';");

                    tFile.AppendLine("$tT = (microtime(true)-$BenchmarkDico[$sKey]);");
                    tFile.AppendLine("if ($tT < 0.001) ");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'<color=green>'.number_format($tT,6).'s</color> Finish : '.$sKey;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else if ($tT < 0.01) ");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'<color=orange>'.number_format($tT,6).'s</color> Finish : '.$sKey;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$BenchmarkResult[] = $R.'<color=red>'.number_format($tT,6).'s</color> Finish : '.$sKey;");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function BenchmarkResult()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $BenchmarkResult;");
                tFile.AppendLine("if (isset($BenchmarkResult) == true) ");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("respondAdd('" + NWD.K_WEB_BENCHMARK_Key + "', $BenchmarkResult);");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function GetRangeAccessForAccount($sAccountReference)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine("global $SQL_CURRENT_DATABASE, $UserRange;");
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$rReturn = explode('" + NWEConstants.K_MINUS + "',$sAccountReference)[1];");
                tFile.AppendLine("if ($rReturn == 0)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn = $UserRange;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function GetConnexionForAccount($sAccountReference)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine("global $SQL_CURRENT_DATABASE, $SQL_CURRENT_ACCESSRANGE;");
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tRangeAccess = GetRangeAccessForAccount($sAccountReference);");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return GetConnexionByRangeAccess($tRangeAccess);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function GetConnexionByRange($sRange)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_LIST, " + NWD.K_SQL_CON_EDITOR + ";");
                tFile.AppendLine("if (isset(" + NWD.K_SQL_CON_EDITOR + "[$sRange]) == false)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("" + NWD.K_SQL_CON_EDITOR + "[$sRange] = new mysqli($SQL_LIST[$sRange]['host'], $SQL_LIST[$sRange]['user'], $SQL_LIST[$sRange]['password'], $SQL_LIST[$sRange]['database'], $SQL_LIST[$sRange]['port']);");
                    tFile.AppendLine("if (" + NWD.K_SQL_CON_EDITOR + "[$sRange]->connect_errno)");
                    tFile.AppendLine("{");
                    {
                        if (NWDEditorCredentialsManager.ShowPasswordInLog == true)
                        {
                            tFile.AppendLine(NWDError.PHP_log(this, "Error in MySQL connexion on '.$SQL_LIST[$sRange]['host'].' for '.$SQL_LIST[$sRange]['user'].' with password '.$SQL_LIST[$sRange]['password'].' on database '.$SQL_LIST[$sRange]['database'].'"));
                        }
                        else
                        {
                        tFile.AppendLine(NWDError.PHP_log(this, "Error in MySQL connexion on '.$SQL_LIST[$sRange]['host'].' for '.$SQL_LIST[$sRange]['user'].' with password ??????????? on database '.$SQL_LIST[$sRange]['database'].'"));
                        }
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                        //tFile.AppendLine("include_once ('" + NWD.K_STATIC_FINISH_PHP + "');");
                        //tFile.AppendLine("exit;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$SQL_LIST[$sRange]['connexion'] = " + NWD.K_SQL_CON_EDITOR + "[$sRange];");
                        tFile.AppendLine(NWDError.PHP_log(this, "'.$sRange.' connexion success on '.$SQL_LIST[$sRange]['title'].' => '.$SQL_LIST[$sRange]['id'].'"));
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return " + NWD.K_SQL_CON_EDITOR + "[$sRange];");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function GetConnexionByRangeAccess($sRangeAccess)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_LIST, $SQL_CURRENT_DATABASE, $SQL_CURRENT_ACCESSRANGE;");
                tFile.AppendLine("$rConnexion = $SQL_CURRENT_DATABASE;");
                tFile.AppendLine("if ($sRangeAccess != $SQL_CURRENT_ACCESSRANGE)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("foreach ($SQL_LIST as $tRange => $tValue)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($sRangeAccess >= $tValue['min'] && $sRangeAccess <= $tValue['max'])");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$rConnexion = GetConnexionByRange($tRange);");
                            tFile.AppendLine("break;");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rConnexion;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function ConnectAllDatabases()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_LIST, " + NWD.K_SQL_CON_EDITOR + ";");
                tFile.AppendLine("global $K_ConnectAllDatabases;");
                tFile.AppendLine("if ($K_ConnectAllDatabases == false)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$K_ConnectAllDatabases = true;");
                    tFile.AppendLine("foreach ($SQL_LIST as $tRange => $tValue)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if (isset(" + NWD.K_SQL_CON_EDITOR + "[$tRange]) == false)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("" + NWD.K_SQL_CON_EDITOR + "[$tRange] = new mysqli($tValue['host'], $tValue['user'], $tValue['password'], $tValue['database'], $tValue['port']);");
                            tFile.AppendLine("if (" + NWD.K_SQL_CON_EDITOR + "[$tRange]->connect_errno)");
                            tFile.AppendLine("{");
                            {
                                if (NWDEditorCredentialsManager.ShowPasswordInLog == true)
                                {
                                    tFile.AppendLine(NWDError.PHP_log(this, "Error in MySQL connexion on '.$tValue['host'].' for '.$tValue['user'].' with password '.$tValue['password'].' on database '.$tValue['database'].'"));
                                }
                                else
                                {
                                    tFile.AppendLine(NWDError.PHP_log(this, "Error in MySQL connexion on '.$tValue['host'].' for '.$tValue['user'].' with password ??????????? on database '.$tValue['database'].'"));
                                }
                                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                                //tFile.AppendLine("include_once ('" + NWD.K_STATIC_FINISH_PHP + "');");
                                //tFile.AppendLine("exit;");
                            }
                            tFile.AppendLine("}");
                            tFile.AppendLine("else");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("$SQL_LIST[$tRange]['connexion'] = " + NWD.K_SQL_CON_EDITOR + "[$tRange];");
                                tFile.AppendLine(NWDError.PHP_log(this, "'.$tRange.' connexion success on '.$tValue['title'].' => '.$tValue['id'].'"));
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function GetCurrentConnexion()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_CURRENT_DATABASE;");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $SQL_CURRENT_DATABASE;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function SelectFromCurrentConnexion($sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_CURRENT_DATABASE;");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return SelectFromConnexion($SQL_CURRENT_DATABASE, $sSQL);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function ExecuteInCurrentConnexion($sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global $SQL_CURRENT_DATABASE;");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return ExecuteInConnexion($SQL_CURRENT_DATABASE, $sSQL);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function SelectFromConnexionRangeAccess($sRangeAccess, $sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tConnexion = GetConnexionByRangeAccess($sRangeAccess);");
                tFile.AppendLine("return SelectFromConnexion($tConnexion, $sSQL);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function ExecuteInConnexionRangeAccess($sRangeAccess, $sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tConnexion = GetConnexionByRangeAccess($sRangeAccess);");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return ExecuteInConnexion($tConnexion, $sSQL);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// return a array assoc structure: ");
            tFile.AppendLine("// $rReturn['error']  : true or false");
            tFile.AppendLine("// $rReturn['errno']  : the error no ($sCon->errno)");
            tFile.AppendLine("// $rReturn['error_log']  : the error explain ($sCon->error)");
            tFile.AppendLine("// $rReturn['count']  : number of rows in all result");
            tFile.AppendLine("// $rReturn['connexions'][x]  : the list of connexions use for the result of datas");
            tFile.AppendLine("// $rReturn['datas'][x][y...]  : the list of datas by connexions index used for the result of datas");
            tFile.AppendLine("function SelectFromConnexion($sCon, $sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));

                if (LogMode == true)
                {
                    tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                    tFile.AppendLine("global $SQL_ACCESS_SQL;");
                    tFile.AppendLine("$SQL_ACCESS_COUNT++;");
                    tFile.AppendLine("$SQL_ACCESS_SQL[] = $sSQL;");
                }
                tFile.AppendLine("$rReturn = array();");
                tFile.AppendLine("$rReturn['error'] = false;");
                tFile.AppendLine("$rReturn['count'] = 0;");
                tFile.AppendLine("$rReturn['datas'] = array();");
                tFile.AppendLine("$rReturn['connexions'] = array();");
                tFile.AppendLine("$rReturn['errno'] = -1;");
                tFile.AppendLine("$rReturn['error_log'] = '';");
                tFile.AppendLine(NWDError.PHP_log(this, "select in connexion request  <b> '.$sSQL.' </b>"));
                tFile.AppendLine("$tResult = $sCon->query($sSQL);");
                tFile.AppendLine("if (!$tResult)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn['error'] = true;");
                    tFile.AppendLine("$rReturn['errno'] = $sCon->errno;");
                    tFile.AppendLine("$rReturn['error_log'] = 'error in mysqli request : ('.$sCon->errno.') '.$sCon->error.'in '.$sSQL;");
                    tFile.AppendLine("return $rReturn;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn['datas'][0] = array();");
                    tFile.AppendLine("$rReturn['connexions'][0] = $sCon;");
                    tFile.AppendLine("while($tRow = $tResult->fetch_assoc())");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$rReturn['count']++;");
                        tFile.AppendLine("$rReturn['datas'][0][] = $tRow;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("mysqli_free_result($tResult);");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_log(this, "result is '.json_encode($rReturn).'"));
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// return a array assoc structure: ");
            tFile.AppendLine("// $rReturn['error']  : true or false");
            tFile.AppendLine("// $rReturn['errno']  : the error no ($sCon->errno)");
            tFile.AppendLine("// $rReturn['error_log']  : the error explain ($sCon->error)");
            tFile.AppendLine("function ExecuteInConnexion($sCon, $sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                if (LogMode == true)
                {
                    tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                    tFile.AppendLine("global $SQL_ACCESS_SQL;");
                    tFile.AppendLine("$SQL_ACCESS_COUNT++;");
                    tFile.AppendLine("$SQL_ACCESS_SQL[] = $sSQL;");
                }
                tFile.AppendLine("$rReturn = array();");
                tFile.AppendLine("$rReturn['error'] = false;");
                tFile.AppendLine("$rReturn['errno'] = -1;");
                tFile.AppendLine("$rReturn['error_log'] = '';");
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(this));
                tFile.AppendLine("$tResult = $sCon->query($sSQL);");
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(this, "'.$sSQL.'"));
                tFile.AppendLine("if (!$tResult)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn['error'] = true;");
                    tFile.AppendLine("$rReturn['errno'] = $sCon->errno;");
                    tFile.AppendLine("$rReturn['error_log'] = 'error in mysqli request : ('.$sCon->errno.') '.$sCon->error.'in '.$sSQL;");
                    tFile.AppendLine("return $rReturn;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(this));
                tFile.AppendLine(NWDError.PHP_log(this, "result is '.json_encode($rReturn).'"));
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(this));
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------

            tFile.AppendLine("// return a array assoc structure: ");
            tFile.AppendLine("// $rReturn['error']  : true or false");
            tFile.AppendLine("// $rReturn['errno']  : the error no ($tConnexion->errno)");
            tFile.AppendLine("// $rReturn['error_log']  : the error explain ($tConnexion->error)");
            tFile.AppendLine("// $rReturn['count']  : number of rows in all result");
            tFile.AppendLine("// $rReturn['connexions'][x]  : the list of connexions use for the result of datas");
            tFile.AppendLine("// $rReturn['datas'][x][y...]  : the list of datas by connexions index used for the result of datas");
            tFile.AppendLine("function SelectFromAllConnexions($sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ";");
                tFile.AppendLine("$rReturn = array();");
                tFile.AppendLine("$rReturn['error'] = false;");
                tFile.AppendLine("$rReturn['errno'] = -1;");
                tFile.AppendLine("$rReturn['error_log'] = '';");
                tFile.AppendLine("$rReturn['count'] = 0;");
                tFile.AppendLine("$rReturn['datas'] = array();");
                tFile.AppendLine("$rReturn['connexions'] = array();");
                tFile.AppendLine("ConnectAllDatabases();");
                tFile.AppendLine("$tI = 0;");
                tFile.AppendLine("foreach (" + NWD.K_SQL_CON_EDITOR + " as $tRange => $tConnexion)");
                tFile.AppendLine("{");
                {
                    if (LogMode == true)
                    {
                        tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                        tFile.AppendLine("global $SQL_ACCESS_SQL;");
                        tFile.AppendLine("$SQL_ACCESS_COUNT++;");
                        tFile.AppendLine("$SQL_ACCESS_SQL[] = $tRange.' => '.$sSQL;");
                    }
                    tFile.AppendLine(NWDError.PHP_log(this, "select in DB '.$tRange.' request  <b> '.$sSQL.' </b>"));
                    tFile.AppendLine("$tResult = $tConnexion->query($sSQL);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$rReturn['error'] = true;");
                        tFile.AppendLine("$rReturn['errno'] = $tConnexion->errno;");
                        tFile.AppendLine("$rReturn['error_log'] = 'error in mysqli request : ('.$tConnexion->errno.') '.$tConnexion->error.'in '.$sSQL;");
                        tFile.AppendLine("return $rReturn;");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$rReturn['datas'][$tI] = array();");
                        tFile.AppendLine("$rReturn['connexions'][$tI] = $tConnexion;");
                        tFile.AppendLine("while($tRow = $tResult->fetch_assoc())");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$rReturn['count']++;");
                            tFile.AppendLine("$rReturn['datas'][$tI][] = $tRow;");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("mysqli_free_result($tResult);");
                        tFile.AppendLine("$tI++;");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_log(this, "result is '.json_encode($rReturn).'"));
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// return a array assoc structure: ");
            tFile.AppendLine("// $rReturn['error']  : true or false");
            tFile.AppendLine("// $rReturn['errno']  : the error no ($tConnexion->errno)");
            tFile.AppendLine("// $rReturn['error_log']  : the error explain ($tConnexion->error)");
            tFile.AppendLine("function ExecuteInAllConnexions($sSQL)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this));
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ";");
                tFile.AppendLine("$rReturn = array();");
                tFile.AppendLine("$rReturn['error'] = false;");
                tFile.AppendLine("$rReturn['errno'] = -1;");
                tFile.AppendLine("$rReturn['error_log'] = '';");
                tFile.AppendLine("ConnectAllDatabases();");
                tFile.AppendLine("foreach (" + NWD.K_SQL_CON_EDITOR + " as $tRange => $tConnexion)");
                tFile.AppendLine("{");
                {
                    if (LogMode == true)
                    {
                        tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                        tFile.AppendLine("global $SQL_ACCESS_SQL;");
                        tFile.AppendLine("$SQL_ACCESS_COUNT++;");
                        tFile.AppendLine("$SQL_ACCESS_SQL[] = $tRange.' => '.$sSQL;");
                    }
                    tFile.AppendLine(NWDError.PHP_log(this, "execute in DB '.$tRange.' request  <b> '.$sSQL.' </b>"));
                    tFile.AppendLine("$tResult = $tConnexion->query($sSQL);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$rReturn['error'] = true;");
                        tFile.AppendLine("$rReturn['errno'] = $tConnexion->errno;");
                        tFile.AppendLine("$rReturn['error_log'] = 'error in mysqli request : ('.$tConnexion->errno.') '.$tConnexion->error.'in '.$sSQL;");
                        tFile.AppendLine("return $rReturn;");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_log(this, "result is '.json_encode($rReturn).'"));
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function GetLocalizableString($sString, $sLang='BASE')");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tLines = explode('•', $sString);");
                tFile.AppendLine("foreach ($tLines as $tline)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tKeyValues = explode(':', $tline);");
                    tFile.AppendLine("$tResult[$tKeyValues[0]] = $tKeyValues[1];");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("if (isset($tResult[$sLang]))");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("return UnprotectLocalizableString($tResult[$sLang]);");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else if (isset($tResult['BASE']))");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("return UnprotectLocalizableString($tResult['BASE']);");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("return '';");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function UnprotectLocalizableString($sString)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tString =  str_replace('@1#','•',$sString);");
                tFile.AppendLine("$tString =  str_replace('@2#',':',$tString);");
                tFile.AppendLine("$tString =  str_replace('@3#','_',$tString);");
                tFile.AppendLine("return $tString;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function SendEmail($sSubject, $sMessage, $sEmail, $sEmailFrom)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                if (NWDCluster.SelectClusterforEnvironment(this).MailBySMTP == true)
                {
                    tFile.AppendLine("require_once \"Mail.php\";");
                    tFile.AppendLine("global $SMTP_HOST, $SMTP_PORT, $SMTP_FRO, $SMTP_USER, $SMTP_PSW;");
                    tFile.AppendLine("$headers['From'] = $sEmailFrom;");
                    tFile.AppendLine("$headers['To'] = $sEmail;");
                    tFile.AppendLine("$headers['Subject'] =$sSubject;");
                    tFile.AppendLine("$headers['Content-Type'] ='text/html';");
                    tFile.AppendLine("$headers['charset'] = \"utf-8\";");
                    tFile.AppendLine("$params['sendmail_path'] = '/usr/lib/sendmail';");
                    tFile.AppendLine("// Create the mail object using the Mail::factory method");
                    tFile.AppendLine("$mail_object = Mail::factory('smtp', array (");
                    if (NWDCluster.SelectClusterforEnvironment(this).MailAuth == true)
                    {
                        tFile.AppendLine("'host' => 'ssl://'.$SMTP_HOST, ");
                    }
                    else
                    {
                        tFile.AppendLine("'host' => $SMTP_HOST, ");
                    }
                    tFile.AppendLine("'port' => $SMTP_PORT, ");
                    if (NWDCluster.SelectClusterforEnvironment(this).MailAuth == true)
                    {
                        tFile.AppendLine("'auth' => true, ");
                        tFile.AppendLine("'username' => $SMTP_USER, ");
                        tFile.AppendLine("'password' => $SMTP_PSW));");
                    }
                    //tFile.AppendLine("if (PEAR::isError($mail_object->send($sEmail, $headers, $sMessage)))");
                    tFile.AppendLine("if ($mail_object->send($sEmail, $headers, $sMessage))");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(this, "Email sent!"));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(this, "Email error ... not send!"));
                    }
                    tFile.AppendLine("}");
                }
                else
                {
                    tFile.AppendLine("global $NWD_APP_NAM;");
                    tFile.AppendLine("$headers = 'Reply-to: '.$sEmailFrom.''.\"\\n\";");
                    tFile.AppendLine("$headers .= 'From: \"'.$NWD_APP_NAM.'\" <'.$sEmailFrom.'>'.\"\\n\";");
                    tFile.AppendLine("$headers .= 'Return-path: '.$sEmailFrom.\"\\n\";");
                    tFile.AppendLine("$headers .= 'X-Mailer: PHP '.phpversion().\"\\n\";");
                    tFile.AppendLine("$headers .= 'X-Priority: 1 '.\"\\n\";");
                    tFile.AppendLine("mail($sEmail,$sSubject, $sMessage,$headers);");
                }
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function adminHashTest ($sAdminHash, $sAdminKey, $sFrequence)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$rReturn = false;");
                tFile.AppendLine("$temporalSalt = saltTemporal($sFrequence, 0);");
                tFile.AppendLine("$tHash = sha1($sAdminKey.$temporalSalt);");
                tFile.AppendLine("if ($sAdminHash == $tHash)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn = true;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$temporalSaltMinor = saltTemporal($sFrequence, -1);");
                tFile.AppendLine("$tHashMinor = sha1($sAdminKey.$temporalSaltMinor);");
                tFile.AppendLine("if ($sAdminHash == $tHashMinor)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn = true;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$temporalSaltMajor = saltTemporal($sFrequence, +1);");
                tFile.AppendLine("$tHashMajor = sha1($sAdminKey.$temporalSaltMajor);");
                tFile.AppendLine("if ($sAdminHash == $tHashMajor)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn = true;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function referenceRandomGlobal ($sPrefix)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
                tFile.AppendLine("$tTime = " + NWD.K_PHP_TIME_SYNC + "-1492711200; // Timestamp unix format");
                tFile.AppendLine("return $sPrefix.'-'.$tTime.'-'.rand ( 100000 , 999999 ).'" + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + "'; // " + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + " for Certify");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function referenceRandomWithRangeAccess ($sPrefix, $sRangeAccess)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
                tFile.AppendLine("$tTime = " + NWD.K_PHP_TIME_SYNC + "-1492711200; // Timestamp unix format");
                tFile.AppendLine("return $sPrefix.'-'.$sRangeAccess.'-'.$tTime.'-'.rand ( 100000 , 999999 ).'" + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + "'; // " + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + " for Certify");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function referenceGenerateGlobal ($sPrefix, $sTable, $sColumn)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tReference = referenceRandomGlobal($sPrefix);");
                tFile.AppendLine("$tTested = false;");
                tFile.AppendLine("ConnectAllDatabases();");
                tFile.AppendLine("while ($tTested == false)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tQuery = 'SELECT `'.$sColumn.'` FROM `'.$sTable.'` WHERE `'.$sColumn.'` LIKE \\''.EscapeString($tReference).'\\';';");
                    tFile.AppendLine("$tResult = SelectFromAllConnexions($tQuery);");
                    tFile.AppendLine("if ($tResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(this, "'.$tResult['error_log'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($tResult['count'] == 0)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tTested = true;");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tReference = referenceRandomGlobal($sPrefix);");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $tReference;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function referenceGenerateRange ($sConnexion, $sRangeAccess, $sPrefix, $sTable, $sColumn)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tReference = referenceRandomWithRangeAccess($sPrefix, $sRangeAccess);");
                tFile.AppendLine("$tTested = false;");
                tFile.AppendLine("while ($tTested == false)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tQuery = 'SELECT `'.$sColumn.'` FROM `'.$sTable.'` WHERE `'.$sColumn.'` LIKE \\''.EscapeString($tReference).'\\';';");
                    tFile.AppendLine("$tResult = SelectFromConnexion($sConnexion, $tQuery);");
                    tFile.AppendLine("if ($tResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(this, "'.$tResult['error_log'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($tResult['count'] == 0)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tTested = true;");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tReference = referenceRandomWithRangeAccess($sPrefix, $sRangeAccess);");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $tReference;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // --------------------------------------
            tFile.AppendLine("function CodeRandomSizable (int $sSize)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tMin = 1;");
                tFile.AppendLine("while ($sSize>1)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tMin = $tMin*10;");
                    tFile.AppendLine("$sSize--;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$tMax = ($tMin*10)-1;");
                tFile.AppendLine("return rand ($tMin ,$tMax );");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function RandomString($sLength = 10)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("$tCharacters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';");
                tFile.AppendLine("$tCharactersLength = strlen($tCharacters);");
                tFile.AppendLine("$tRandomString = '';");
                tFile.AppendLine("for ($i = 0; $i < $sLength; $i++)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tRandomString .= $tCharacters[rand(0, $tCharactersLength - 1)];");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $tRandomString;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function UniquePropertyValueFromValue($sConnexion, $sRangeAccess, $sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty = true)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("return UnicityPropertyValueFromValue(false, $sConnexion, $sRangeAccess, $sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty);");

            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            //---------------------------------------
            tFile.AppendLine("function UniquePropertyValueFromGlobalValue( $sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty = true)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("return UnicityPropertyValueFromValue(true, NULL, NULL, $sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty);");

            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            //---------------------------------------
            string tGlobalChar = "#";
            string tLocalCharA = "@";
            string tLocalCharB = "_";
            tFile.AppendLine("function UnicityPropertyValueFromValue($sGlobal, $sConnexion, $sRangeAccess, $sTable, $sColumnOrign, $sColumUniqueResult, $sReference, $sNeverEmpty = true)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(this));
                tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
                tFile.AppendLine("$rModified = false;");
                tFile.AppendLine("$tQuery = 'SELECT `'.$sColumnOrign.'`, `'.$sColumUniqueResult.'`, `Reference` FROM `'.$sTable.'` WHERE `Reference` = \\''.EscapeString($sReference).'\\'';");
                tFile.AppendLine("$tResult = array();");

                tFile.AppendLine("if ($sGlobal == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = SelectFromAllConnexions($tQuery);");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = SelectFromConnexion($sConnexion, $tQuery);");
                }
                tFile.AppendLine("}");

                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(this, "'.$tResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tResult['count'] > 0)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("if ($tRow[$sColumnOrign] == '' && $sNeverEmpty == true)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$tRow[$sColumnOrign] = RandomString(10);");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("$tOrigin = str_replace('" + tGlobalChar + "', '', $tRow[$sColumnOrign]);");
                                tFile.AppendLine("$tOrigin = str_replace('" + tLocalCharA + "', '', $tOrigin);");
                                tFile.AppendLine("$tOrigin = str_replace('" + tLocalCharB + "', '', $tOrigin);");
                                tFile.AppendLine("$tNick = $tOrigin.'???';");



                                tFile.AppendLine("if ($sGlobal == true)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$tNickArray = explode('" + tGlobalChar + "', $tRow[$sColumUniqueResult]);");
                                    tFile.AppendLine("if (count($tNickArray)==2)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$tCodeAc = $tNickArray[1];");
                                        tFile.AppendLine("if (preg_match ('/^([0-9]{1,12})$/', $tCodeAc))");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tNick = $tNickArray[0];");
                                        }
                                        tFile.AppendLine("}");
                                    }
                                    tFile.AppendLine("}");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("else");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$tNickArray = explode('" + tLocalCharA + "', $tRow[$sColumUniqueResult]);");
                                    tFile.AppendLine("if (count($tNickArray)==2)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$tCodeAc = $tNickArray[1];");
                                        tFile.AppendLine("if (preg_match ('/^([0-9]{1,12})$/', $tCodeAc))");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tNick = $tNickArray[0];");
                                        }
                                        tFile.AppendLine("}");
                                    }
                                    tFile.AppendLine("}");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("$tNick = str_replace('" + tGlobalChar + "', '', $tNick);");
                                tFile.AppendLine("$tNick = str_replace('" + tLocalCharA + "', '', $tNick);");
                                tFile.AppendLine("$tNick = str_replace('" + tLocalCharB + "', '', $tNick);");

                                //tFile.AppendLine("$tNickArray = explode('#',$tRow[$sColumUniqueResult]);");
                                //tFile.AppendLine("if (count($tNickArray)==2)");
                                //tFile.AppendLine("{");
                                //{
                                //    tFile.AppendLine("$tCodeAc = $tNickArray[1];");
                                //    tFile.AppendLine("if (preg_match ('/^([0-9]{1,12})$/', $tCodeAc))");
                                //    tFile.AppendLine("{");
                                //    {
                                //        tFile.AppendLine("$tNick = $tNickArray[0];");
                                //    }
                                //    tFile.AppendLine("}");
                                //}
                                //tFile.AppendLine("}");
                                //tFile.AppendLine("else");
                                //tFile.AppendLine("{");
                                //{
                                //    tFile.AppendLine("// error ");
                                //}
                                //tFile.AppendLine("}");


                                tFile.AppendLine("$tTested = false;");
                                tFile.AppendLine("$tSize = 3;");
                                tFile.AppendLine("if ($tOrigin == $tNick)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("// Nothing to do ? perhaps ... I test");
                                    tFile.AppendLine("$tQueryTest = 'SELECT `'.$sColumUniqueResult.'` FROM `'.$sTable.'` WHERE `'.$sColumUniqueResult.'` LIKE \\''.EscapeString($tRow[$sColumUniqueResult]).'\\'';");
                                    tFile.AppendLine("$tResultTest = array();");
                                    tFile.AppendLine("if ($sGlobal == true)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$tResultTest = SelectFromAllConnexions($tQueryTest);");
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("else");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$tResultTest = SelectFromConnexion($sConnexion, $tQueryTest);");
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("if ($tResultTest['error'] == true)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine(NWDError.PHP_log(this, "'.$tResultTest['error_log'].'"));
                                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("else");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("if ($tResultTest['count']  == 1)");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tTested = true;");
                                        }
                                        tFile.AppendLine("}");
                                    }
                                    tFile.AppendLine("}");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("if ($tTested == false)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("// I need change for an unique nickname");
                                    tFile.AppendLine("while ($tTested == false)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("if ($sGlobal == true)");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tPinCode = '"+tGlobalChar+"'.CodeRandomSizable($tSize++);");
                                        }
                                        tFile.AppendLine("}");
                                        tFile.AppendLine("else");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tPinCode = '" + tLocalCharA + "'.$sRangeAccess.'" + tLocalCharB + "'.CodeRandomSizable($tSize++);");
                                        }
                                        tFile.AppendLine("}");
                                        tFile.AppendLine("$tQueryTestUnique = 'SELECT `'.$sColumUniqueResult.'` FROM `'.$sTable.'` WHERE `'.$sColumUniqueResult.'` LIKE \\''.EscapeString($tOrigin).$tPinCode.'\\'';");

                                        tFile.AppendLine("$tResultTestUnique = array();");
                                        tFile.AppendLine("if ($sGlobal == true)");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tResultTestUnique = SelectFromAllConnexions($tQueryTestUnique);");
                                        }
                                        tFile.AppendLine("}");
                                        tFile.AppendLine("else");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("$tResultTestUnique = SelectFromConnexion($sConnexion, $tQueryTestUnique);");
                                        }
                                        tFile.AppendLine("}");

                                        tFile.AppendLine("if ($tResultTestUnique['error'] == true)");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine(NWDError.PHP_log(this, "'.$tResultTestUnique['error_log'].'"));
                                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                                        }
                                        tFile.AppendLine("}");
                                        tFile.AppendLine("else");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine("if ($tResultTestUnique['count'] == 0)");
                                            tFile.AppendLine("{");
                                            {
                                                tFile.AppendLine("$tTested = true;");
                                                tFile.AppendLine("// Ok I have a good PinCode I update");
                                                tFile.AppendLine("$tQueryUpdate = 'UPDATE `'.$sTable.'` SET `DM` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', `'.$sColumnOrign.'` = \\''.EscapeString($tOrigin).'\\', `'.$sColumUniqueResult.'` = \\''.EscapeString($tOrigin).$tPinCode.'\\' WHERE `Reference` = \\''.EscapeString($sReference).'\\'';");
                                                tFile.AppendLine("$tResultUpdate = false;");
                                                tFile.AppendLine("if ($sGlobal == true)");
                                                tFile.AppendLine("{");
                                                {
                                                    tFile.AppendLine("$tResultUpdate = ExecuteInAllConnexions($tQueryUpdate, '" + NWDError.GetErrorCode(NWDError.NWDError_SERVER) + "', __FUNCTION__, true);");
                                                }
                                                tFile.AppendLine("}");
                                                tFile.AppendLine("else");
                                                tFile.AppendLine("{");
                                                {
                                                    tFile.AppendLine("$tResultUpdate = ExecuteInConnexion($sConnexion, $tQueryUpdate, '" + NWDError.GetErrorCode(NWDError.NWDError_SERVER) + "', __FUNCTION__, true);");
                                                }
                                                tFile.AppendLine("}");
                                                tFile.AppendLine("if ($tResultUpdate['error'] == true)");
                                                tFile.AppendLine("{");
                                                {
                                                    tFile.AppendLine(NWDError.PHP_log(this, "'.$tResultUpdate['error_log'].'"));
                                                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                                                }
                                                tFile.AppendLine("}");
                                                tFile.AppendLine("else");
                                                tFile.AppendLine("{");
                                                {
                                                    tFile.AppendLine("$rModified = true;");
                                                    tFile.AppendLine("//pincode is update");
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
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $rModified;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_FUNCTIONS_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticRequestFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// REQUEST");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDAccount>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDRequestToken>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDVersion>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
            //tFile.AppendLine("$ereg_os = '/^(editor|unity|ios|osx|android|web|win|wp8|ps3|ps4|psp|switch)$/';");
            //tFile.AppendLine("$ereg_version = '/^([0-9]{1,2})+(\\.[0-9]{1,3})*$/';");
            //tFile.AppendLine("$ereg_lang = '/^([A-Z\\_\\-a-z]{2,7})$/';");
            //tFile.AppendLine("$ereg_UUID = '/^([A-Za-z0-9\\-]{15,48})$/';");
            ////tFile.AppendLine("$ereg_hash = '/^([A-Za-z0-9\\-]{3,48})$/';");
            //tFile.AppendLine("$ereg_hash = '/^(.*)$/';");
            //tFile.AppendLine("$ereg_token = '/^(.*)$/';");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("if (headerValue ('os', '" + NWD.K_WEB_HEADER_OS_KEY + "', $ereg_os, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA11).Code + "')) // test if os infos is valid");
            tFile.AppendLine("{");
            tFile.AppendLine("if (headerValue ('version', '" + NWD.K_WEB_HEADER_VERSION_KEY + "', $ereg_version, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA02).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA12).Code + "')) // test if version is ok");
            tFile.AppendLine("{");
            tFile.AppendLine("// I must prevent admin mode in table creation");
            //tFile.AppendLine("global $admin;");
            //tFile.AppendLine("headerBrutalValue ('adminHash', '" + NWD.AdminHashKey + "');");
            //tFile.AppendLine("$admin = adminHashTest ($adminHash, $NWD_ADM_KEY, $NWD_SLT_TMP);");
            tFile.AppendLine("global $HeaderUUID;");
            tFile.AppendLine("if ($admin==true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$versionValid = true;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$versionValid = versionTest($version);");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($versionValid == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("if (headerValue ('lang', '" + NWD.K_WEB_HEADER_LANG_KEY + "', $ereg_lang, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA03).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA13).Code + "')) // test if lang is ok");
            tFile.AppendLine("{");
            tFile.AppendLine("if (headerValue ('uuid', '" + NWD.K_WEB_UUID_KEY + "', $ereg_UUID, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA04).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA14).Code + "')) // test UUID of headers");
            tFile.AppendLine("{");
            tFile.AppendLine("$HeaderUUID = $uuid;");
            tFile.AppendLine("if (headerValue ('hash', '" + NWD.HashKey + "', $ereg_hash, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA05).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA15).Code + "')) // test hash of headers");
            tFile.AppendLine("{");
            tFile.AppendLine("headerBrutalValue ('token', '" + NWD.K_WEB_REQUEST_TOKEN_KEY + "');");
            tFile.AppendLine("$temporalSalt = saltTemporal($NWD_SLT_TMP, 0);");
            tFile.AppendLine("$tHash = sha1($os.$version.$lang.$temporalSalt.$uuid.$token);");
            tFile.AppendLine("$temporalSaltMinor = saltTemporal($NWD_SLT_TMP, -1);");
            tFile.AppendLine("$tHashMinor = sha1($os.$version.$lang.$temporalSaltMinor.$uuid.$token);");
            tFile.AppendLine("$temporalSaltMajor = saltTemporal($NWD_SLT_TMP, +1);");
            tFile.AppendLine("$tHashMajor = sha1($os.$version.$lang.$temporalSaltMajor.$uuid.$token);");
            tFile.AppendLine("$Verif = false;");
            tFile.AppendLine("if ($tHashMinor == $hash || $tHash ==$hash || $tHashMajor == $hash)");
            tFile.AppendLine("{");
            tFile.AppendLine("getParams('" + NWEUnityWebService.UnSecureKey + "', '" + NWEUnityWebService.UnSecureDigestKey + "', true, false);");
            tFile.AppendLine("if(getParams('" + NWEUnityWebService.SecureKey + "', '" + NWEUnityWebService.SecureDigestKey + "', false, true)==true)");
            tFile.AppendLine("{");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_SECURE_KEY + "',true);");
            tFile.AppendLine("}");
            //tFile.AppendLine("if ($SQL_MNG == false)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("$tQuery = 'SELECT `Reference`,`Ban` FROM `'."+NWD.K_ENV+".'_NWDAccount` WHERE `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($uuid).'\\' AND `AC` = 1;';");
            //tFile.AppendLine("$tResult = "+NWD.K_SQL_CON+"->query($tQuery);");
            //tFile.AppendLine("if (!$tResult)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("error('ACC90',true, __FILE__, __FUNCTION__, __LINE__);");
            //tFile.AppendLine("}");
            //tFile.AppendLine("else");
            //tFile.AppendLine("{");
            //tFile.AppendLine("if ($tResult->num_rows == 0)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("// if user is temporary user I must find the last letter equal to '" + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE + "'");
            //tFile.AppendLine("if (substr($uuid, -1) == '" + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE + "')");
            //tFile.AppendLine("{");
            //tFile.AppendLine("// I put order to create anonymous account if account is not resolve before action (sync, etc)");
            //tFile.AppendLine("AccountAnonymousNeeded(true);");
            //tFile.AppendLine("}");
            //tFile.AppendLine("else");
            //tFile.AppendLine("{");
            //tFile.AppendLine("// strange… an unknow account but not temporary … it's not possible");
            //tFile.AppendLine("error('ACC92',true, __FILE__, __FUNCTION__, __LINE__);");
            //tFile.AppendLine("}");
            //tFile.AppendLine("}");
            //tFile.AppendLine("else if ($tResult->num_rows == 1)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("while($tRow = $tResult->fetch_array())");
            //tFile.AppendLine("{");
            //tFile.AppendLine("if ($tRow['Ban'] > 0)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("$ban = true;");
            //tFile.AppendLine("}");
            //tFile.AppendLine("}");
            //tFile.AppendLine("}");
            //tFile.AppendLine("else //or more than one user with this UUID … strange… I push an error, user must be unique");
            //tFile.AppendLine("{");
            //tFile.AppendLine("error('ACC95',true, __FILE__, __FUNCTION__, __LINE__);");
            //tFile.AppendLine("}");
            //tFile.AppendLine("mysqli_free_result($tResult);");
            //tFile.AppendLine("}");
            //tFile.AppendLine("// I test the request token");
            //tFile.AppendLine("NWDRequestTokenIsValid($uuid,$token);");
            //tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_HEA90));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            //tFile.AppendLine("else");
            //tFile.AppendLine("{");
            //tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            //tFile.AppendLine(NWD.K_CommentSeparator);
            //tFile.AppendLine("// Ok I create a permanent account if temporary before");
            //tFile.AppendLine("AccountAnonymeGenerate();");
            //tFile.AppendLine(NWD.K_CommentSeparator);
            //tFile.AppendLine("if ($ban == true)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("error('ACC99',true, __FILE__, __FUNCTION__, __LINE__);");
            //tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_REQUEST_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void CreatePHP_StaticRequestTokenFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tFile = new StringBuilder(string.Empty);
        //    tFile.AppendLine("<?php");
        //    tFile.AppendLine(Headlines());
        //    tFile.AppendLine("// REQUEST TOKEN");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenCreate($sUUIDHash)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global "+NWD.K_SQL_CON+", "+NWD.K_ENV+", "+NWD.K_PHP_TIME_SYNC+";");
        //    tFile.AppendLine("$tToken = NWDRequestTokenGenerateToken($sUUIDHash);");
        //    tFile.AppendLine("$tInsert = "+NWD.K_SQL_CON+"->query('INSERT INTO `'."+NWD.K_ENV+".'_NWDRequestToken` (`DC`, `DM`, `DD`, `AC`, `Token`, `UUIDHash`, `Integrity`) VALUES ( \\''."+NWD.K_PHP_TIME_SYNC+".'\\', \\''."+NWD.K_PHP_TIME_SYNC+".'\\', \\'0\\', \\'1\\', \\''."+NWD.K_SQL_CON+"->real_escape_string($tToken).'\\', \\''."+NWD.K_SQL_CON+"->real_escape_string($sUUIDHash).'\\', \\'???????\\' );');");
        //    tFile.AppendLine("if (!$tInsert)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("error('RQT01',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.' in : '.$tInsert.'', __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("return $tToken;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenDeleteOldToken ($sUUIDHash, $sTimestamp, $sToken)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global "+NWD.K_SQL_CON+", "+NWD.K_ENV+";");
        //    tFile.AppendLine("myLog('delete old token', __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("$tQuery = 'DELETE FROM `'."+NWD.K_ENV+".'_NWDRequestToken` WHERE `UUIDHash` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sUUIDHash).'\\' AND `DM` <= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimestamp).'\\' AND `Token` != \\''."+NWD.K_SQL_CON+"->real_escape_string($sToken).'\\';';");
        //    tFile.AppendLine("$tResult = "+NWD.K_SQL_CON+"->query($tQuery);");
        //    tFile.AppendLine("if (!$tResult)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("error('RQT14',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("myLog('ERROR IN '.$tQuery);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenGenerateToken ($sUUIDHash)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global "+NWD.K_PHP_TIME_SYNC+";");
        //    tFile.AppendLine("$tRandom = $sUUIDHash.'-'."+NWD.K_PHP_TIME_SYNC+".'-'.rand ( 1000000000 , 9999999999 ).'-0';");
        //    tFile.AppendLine("return md5($tRandom);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenDeleteAllToken ($sUUIDHash)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global "+NWD.K_SQL_CON+", "+NWD.K_ENV+";");
        //    tFile.AppendLine("$tQuery = 'DELETE FROM `'."+NWD.K_ENV+".'_NWDRequestToken` WHERE `UUIDHash` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sUUIDHash).'\\';';");
        //    tFile.AppendLine("$tResult = "+NWD.K_SQL_CON+"->query($tQuery);");
        //    tFile.AppendLine("if (!$tResult)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("error('RQT13',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("myLog('ERROR IN '.$tQuery);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenReset ($sUUIDHash)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global $REP;");
        //    tFile.AppendLine("global $token;");
        //    tFile.AppendLine("NWDRequestTokenDeleteAllToken ($sUUIDHash);");
        //    tFile.AppendLine("$token = NWDRequestTokenCreate ($sUUIDHash);");
        //    tFile.AppendLine("$REP['"+NWD.RequestTokenKey+"']=$token;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("function NWDRequestTokenIsValid ($sUUIDHash, $sToken)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("global "+NWD.K_SQL_CON+", "+NWD.K_ENV+", "+NWD.K_PHP_TIME_SYNC+";");
        //    tFile.AppendLine("global $REP;");
        //    tFile.AppendLine("global $token;");
        //    tFile.AppendLine("global $token_FirstUse;");
        //    tFile.AppendLine("global $RTH;");
        //    tFile.AppendLine("$rReturn = false;");
        //    tFile.AppendLine("if ($sToken=='')");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$rReturn = true;");
        //    tFile.AppendLine("$token = NWDRequestTokenCreate($sUUIDHash);");
        //    tFile.AppendLine("$REP['"+NWD.RequestTokenKey+"'] = $token;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$tQuery = 'SELECT `Token`,`DM`, `AC` FROM `'."+NWD.K_ENV+".'_NWDRequestToken` WHERE `UUIDHash` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sUUIDHash).'\\' AND `DD` = \\'0\\';';");
        //    tFile.AppendLine("$tResult = "+NWD.K_SQL_CON+"->query($tQuery);");
        //    tFile.AppendLine("if (!$tResult)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("error('RQT12',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if ($tResult->num_rows == 0)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// not possible ... the token is too old and the base was purged since the last connexion");
        //    tFile.AppendLine("$rReturn = false;");
        //    tFile.AppendLine("error('RQT90',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else if ($tResult->num_rows <= $RTH)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// ok I have some token for this user ...");
        //    tFile.AppendLine("$tTokenIsValid = false;");
        //    tFile.AppendLine("$tTimestamp = 0;");
        //    tFile.AppendLine("$tToken = '';");
        //    tFile.AppendLine("while($tRow = $tResult->fetch_array())");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("myLog('find token : '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("if ($tRow['Token'] == $sToken)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if ($tRow['AC'] == 0)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("myLog('find OLD token reused: '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("$token_FirstUse = false;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$token_FirstUse = true;");
        //    tFile.AppendLine("$tQueryUseToken = 'UPDATE `'."+NWD.K_ENV+".'_NWDRequestToken` SET `AC` = \\'0\\' WHERE `Token` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tRow['Token']).'\\';';");
        //    tFile.AppendLine("$tResultUseToken = "+NWD.K_SQL_CON+"->query($tQueryUseToken);");
        //    tFile.AppendLine("if (!$tResultUseToken)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("error('RQT11',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("myLog('find token, Use IT: '.$tRow['Token'], __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("$tTokenIsValid = true;");
        //    tFile.AppendLine("$tTimestamp = $tRow['DM'];");
        //    tFile.AppendLine("$tToken = $tRow['Token'];");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// Not the good token ... newest or oldest ... don't use it to analyze");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("if ($tTokenIsValid==true)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$rReturn = true;");
        //    tFile.AppendLine("NWDRequestTokenDeleteOldToken ($sUUIDHash, $tTimestamp, $tToken);");
        //    tFile.AppendLine("$token = NWDRequestTokenCreate($sUUIDHash);");
        //    tFile.AppendLine("$REP['"+NWD.RequestTokenKey+"'] = $token;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$rReturn = false;");
        //    tFile.AppendLine("error('RQT91',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// not possible ... the token are too number");
        //    tFile.AppendLine("myLog('not possible ... the token are too number', __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("error('RQT93',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("//                  $TokenDate = '';");
        //    tFile.AppendLine("//                  $LastTokenDate = '';");
        //    tFile.AppendLine("//                  $TokenInConflict; // only one by one;");
        //    tFile.AppendLine("//                  $TokenMajor;");
        //    tFile.AppendLine("//                  while($tRow = $tResult->fetch_array())");
        //    tFile.AppendLine("//                  {");
        //    tFile.AppendLine("//                      if ($tRow['Token'] == $sToken)");
        //    tFile.AppendLine("//                      {");
        //    tFile.AppendLine("//                          $TokenDate = $tRow['Token'];");
        //    tFile.AppendLine("//                      }");
        //    tFile.AppendLine("//                      else");
        //    tFile.AppendLine("//                      {");
        //    tFile.AppendLine("//                          $TokenInConflict[] = $tRow['Token'];");
        //    tFile.AppendLine("//                      }");
        //    tFile.AppendLine("//                      if ($tRow['Token'] > $LastTokenDate)");
        //    tFile.AppendLine("//                      {");
        //    tFile.AppendLine("//                          $LastTokenDate = $tRow['Token'];");
        //    tFile.AppendLine("//                          $TokenMajor = $tRow['Token'];");
        //    tFile.AppendLine("//                      }");
        //    tFile.AppendLine("//                  }");
        //    tFile.AppendLine("//                  if ($LastTokenDate == $TokenDate)");
        //    tFile.AppendLine("//                  {");
        //    tFile.AppendLine("//                          // ok I have the last token but another session is working …");
        //    tFile.AppendLine("//                      error('RQT93',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("//                  }");
        //    tFile.AppendLine("//                  else");
        //    tFile.AppendLine("//                  {");
        //    tFile.AppendLine("//                      error('RQT94',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("//                  }");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("mysqli_free_result($tResult);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("// If no token for this UUID : new UUID connexion => valid");
        //    tFile.AppendLine("// If the token is unique (of course the last) => valid");
        //    tFile.AppendLine("// If token is not unique => not valid");
        //    tFile.AppendLine("// If token is the last : error close the other session or delete this session");
        //    tFile.AppendLine("// If token is not the last : error close the other session");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
        //    sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_REQUEST_TOKEN_PHP, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //private void CreatePHP_StaticRescueFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tFile = new StringBuilder(string.Empty);
        //    tFile.AppendLine("<?php");
        //    tFile.AppendLine(Headlines());
        //    tFile.AppendLine("// RESCUE");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("$NWD_TMA = microtime(true);");
        //    tFile.AppendLine(""+NWD.K_PHP_TIME_SYNC+" = $NWD_TMA;");
        //    tFile.AppendLine("include_once ("+NWD.K_PATH_BASE+".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
        //    tFile.AppendLine("include_once ("+NWD.K_PATH_BASE+".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_ERROR_PHP + "');");
        //    tFile.AppendLine("include_once ("+NWD.K_PATH_BASE+".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
        //    tFile.AppendLine("include_once ("+NWD.K_PATH_BASE+".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_VALUES_PHP + "');");
        //    tFile.AppendLine("include_once ("+NWD.K_PATH_BASE+".'/" + Environment + "/" + NWD.K_DB + "/NWDAccount/" + NWD.K_WS_SYNCHRONISATION + "');");
        //    tFile.AppendLine("$ereg_email = '/^([A-Z0-9a-z\\.\\_\\%\\+\\-]+@[A-Z0-9a-z\\.\\_\\%\\+\\-]+\\.[A-Za-z]{2,6})$/';");
        //    tFile.AppendLine("$ereg_password = '/^(.{24,64})$/';");
        //    tFile.AppendLine("$ereg_emailHash = '/^(.{24,64})$/';");
        //    tFile.AppendLine("$ereg_lang = '/^([A-Z\\_\\-a-z]{2,7})$/';");
        //    tFile.AppendLine(""+NWD.K_SQL_CON+" = new mysqli($SQL_HOT, $SQL_USR, $SQL_PSW, $SQL_BSE);");
        //    tFile.AppendLine("if ("+NWD.K_SQL_CON+"->connect_errno)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("exit;");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if (getValue ('lang', '"+NWD.K_WEB_HEADER_LANG_KEY+"', $ereg_lang, 'RES02', 'RES12')) // I test emailrescue");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if (getValue ('s', 's', $ereg_emailHash, 'RES03', 'RES13')) // I test emailrescue");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if (getValue ('emailrescue', 'emailrescue', $ereg_email, 'RES01', 'RES11')) // I test emailrescue");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$emailhash = sha1 ($emailrescue.$NWD_SLT_STR);");
        //    tFile.AppendLine("$tQuery = 'SELECT * FROM `'."+NWD.K_ENV+".'_NWDAccount` WHERE `ServerHash` = \\''."+NWD.K_SQL_CON+"->real_escape_string($s).'\\' AND `Email` = \\''."+NWD.K_SQL_CON+"->real_escape_string($emailhash).'\\' AND `AC` = 1;';");
        //    tFile.AppendLine("$tResult = "+NWD.K_SQL_CON+"->query($tQuery);");
        //    tFile.AppendLine("if (!$tResult)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// error('SGN70',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("if ($tResult->num_rows == 0)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// unknow user");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else if ($tResult->num_rows == 1)");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("while($tRow = $tResult->fetch_array())");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// respondAdd('rescue',true);");
        //    tFile.AppendLine("// ok I have one user");
        //    tFile.AppendLine("//TODO: send an email and process to change the password");
        //    tFile.AppendLine("$tSeed = str_split('ACDEFHJKLMNPRTUVWXY3479'); // and any other characters");
        //    tFile.AppendLine("shuffle($tSeed); // probably optional since array_is randomized; this may be redundant");
        //    tFile.AppendLine("$NewPassWord = '';");
        //    tFile.AppendLine("foreach (array_rand($tSeed, 12) as $k) ");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$NewPassWord.= $tSeed[$k];");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("$NewPassWordHash = sha1 ($NewPassWord.$NWD_SLT_END);");
        //    tFile.AppendLine("//$tQueryC = 'UPDATE `'."+NWD.K_ENV+".'_NWDAccount` SET `ServerHash` = \\'\\', `Password` = \\''.$NewPassWordHash.'\\', `DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' WHERE `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tRow['Reference']).'\\' AND `AC` = 1;';");
        //    tFile.AppendLine("// //echo $tQueryC;");
        //    tFile.AppendLine("// $tResultC = "+NWD.K_SQL_CON+"->query($tQueryC);");
        //    tFile.AppendLine("// if (!$tResultC)");
        //    tFile.AppendLine("// {");
        //    tFile.AppendLine("//  // error('SGN03',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("// }");
        //    tFile.AppendLine("// IntegrityNWDAccountReevalue ($tRow['Reference']);");
        //    tFile.AppendLine("$tError = errorReference('ERR-RESCUE-03');");
        //    tFile.AppendLine("if (isset($tError['Title']))");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$subject = str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tError['Title'], $lang));");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$subject = $NWD_APP_NAM.': password';");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("if (isset($tError['Description']))");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$message = str_replace(\"{PASSWORD}\",$NewPassWord,str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tError['Description'], $lang)));");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("$message =\"Your password was resseted to: $NewPassWord\";");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("include('Mail.php');");
        //    tFile.AppendLine("$headers['From'] = $SMTP_REPLY;");
        //    tFile.AppendLine("$headers['To'] = $emailrescue;");
        //    tFile.AppendLine("$headers['Subject'] =$subject;");
        //    tFile.AppendLine("$params['sendmail_path'] = '/usr/lib/sendmail';");
        //    tFile.AppendLine("// Create the mail object using the Mail::factory method");
        //    tFile.AppendLine("$mail_object = Mail::factory('smtp', array ('host' => $SMTP_HOST, ");
        //    tFile.AppendLine("'auth' => true, ");
        //    tFile.AppendLine("'username' => $SMTP_USER, ");
        //    tFile.AppendLine("'password' => $SMTP_PSW));");
        //    tFile.AppendLine("$mail_object->send($emailrescue, $headers, $message);");
        //    tFile.AppendLine("$tHTML = errorReference('ERR-RESCUE-02');");
        //    tFile.AppendLine("?><!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
        //    tFile.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"fr\">");
        //    tFile.AppendLine("<head>");
        //    tFile.AppendLine("<title><?php");
        //    tFile.AppendLine("if (isset($tHTML['Title']))");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("echo(str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tHTML['Title'], $lang)));");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("echo('Your password was reseted!');");
        //    tFile.AppendLine("}?></title>");
        //    tFile.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text / html; charset = UTF - 8\" />");
        //    tFile.AppendLine("<meta http-equiv=\"Content-Language\" content=\"en\" />");
        //    tFile.AppendLine("<head>");
        //    tFile.AppendLine("<body>");
        //    tFile.AppendLine("<div>");
        //    tFile.AppendLine("<?php ");
        //    tFile.AppendLine("if (isset($tHTML['Description']))");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("echo(str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tHTML['Description'], $lang)));");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("echo('Your password was reseted!');");
        //    tFile.AppendLine("}?>");
        //    tFile.AppendLine("</div>");
        //    tFile.AppendLine("</body>");
        //    tFile.AppendLine("</html><?php");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
        //    tFile.AppendLine("{");
        //    tFile.AppendLine("// to much users ...");
        //    tFile.AppendLine("// error('SGN72',true, __FILE__, __FUNCTION__, __LINE__);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("mysqli_free_result($tResult);");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else{");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("else{");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine("}");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
        //    sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_RESCUE_PHP, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticRespondFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// RESPOND");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine("global $TIME_MICRO;");
            tFile.AppendLine("global $CHANGE_USER;");
            tFile.AppendLine("$CHANGE_USER = false;");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// datas output");
            tFile.AppendLine("$REP;");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get timestamp of server compute");
            tFile.AppendLine("$REP['timestamp'] = " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine("$REP['timemicro'] = $TIME_MICRO;");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondIsset($sKey)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("return isset($REP[$sKey]);");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondAdd($sKey, $sValue)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP[$sKey] = $sValue;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondRemove($sKey)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("if (isset($REP[$sKey]))");
            tFile.AppendLine("{");
            tFile.AppendLine("unset($REP[$sKey]);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondUUID($sValue)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP, $uuid;");
            tFile.AppendLine("$uuid = $sValue;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_UUID_KEY + "'] = $sValue;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondToken($sValue)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP, $token;");
            tFile.AppendLine("$token = $sValue;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "'] = $sValue;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondNeedReloadData()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['reloaddatas'] = true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respondNewUser()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['newuser'] = true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);




            //tFile.AppendLine("function respond_SignOut()");
            //tFile.AppendLine("{");
            //tFile.AppendLine("global $REP;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_SIGNOUT_KEY + "'] = true;");
            //tFile.AppendLine("}");
            //tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("function respond_SignIn()");
            //tFile.AppendLine("{");
            //tFile.AppendLine("global $REP;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_SIGNIN_KEY + "'] = true;");
            //tFile.AppendLine("}");
            //tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respond_RestartWebService()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_RESTART_WEBSERVICE_KEY + "'] = true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respond_UserTransfert($sOldReference, $sNewReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_USER_TRANSFERT_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_PREVIEW_USER_KEY + "'] = $sOldReference;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEXT_USER_KEY + "'] = $sNewReference;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respond_NewUser($sOldReference, $sNewReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_PREVIEW_USER_KEY + "'] = $sOldReference;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEXT_USER_KEY + "'] = $sNewReference;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function respond_ChangeUser($sOldReference, $sNewReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $CHANGE_USER;");
            tFile.AppendLine("$CHANGE_USER = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_PREVIEW_USER_KEY + "'] = $sOldReference;");
            //tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEXT_USER_KEY + "'] = $sNewReference;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);











            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_RESPOND_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticStartFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// START");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("$TIME_MICRO = microtime(true); // perhaps use in instance of $TIME_STAMP in sync ");
            tFile.AppendLine("settype($TIME_MICRO, \"float\");");
            tFile.AppendLine("" + NWD.K_PHP_TIME_SYNC + " = intval($TIME_MICRO);");
            tFile.AppendLine("settype(" + NWD.K_PHP_TIME_SYNC + ", \"integer\");");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// use functions library");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "All"));
            tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "Start"));
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// connect MYSQL");

            tFile.AppendLine("// connect for editor");
            tFile.AppendLine(NWD.K_SQL_CON_EDITOR + " = array();");
            tFile.AppendLine("headerBrutalValue ('adminHash', '" + NWD.AdminHashKey + "');");
            tFile.AppendLine("$admin = adminHashTest ($adminHash, $NWD_ADM_KEY, $NWD_SLT_TMP);");
            tFile.AppendLine("if ($admin == true)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "ConnectAllDataBase"));
                tFile.AppendLine("ConnectAllDatabases();");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "ConnectAllDataBase"));
            }
            tFile.AppendLine("}");

            tFile.AppendLine("// connect for account");
            //tFile.AppendLine("// TODO : dertermine account Database and use the good range!");
            //tFile.AppendLine("$tAccountMySQL = reset($SQL_LIST);"); // remember, use string for ereg extraction from UUID
            //tFile.AppendLine("$AccountForRange = isset($_SERVER['HTTP_'.strtoupper('" + NWD.K_WEB_UUID_KEY + "')]) ? $_SERVER['HTTP_'.strtoupper('" + NWD.K_WEB_UUID_KEY + "')] : '';");
            tFile.AppendLine("if (headerValue ('AccountForRange', '" + NWD.K_WEB_UUID_KEY + "', $ereg_UUID, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA04).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA14).Code + "')) // test UUID of headers");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_log(this, " ok your account is '.$AccountForRange.'"));
                tFile.AppendLine("if (TestTemporaryAccount($AccountForRange))");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("// TODO : if account is temporary : check the first/random database with usermax under limit!");
                    tFile.AppendLine(NWDError.PHP_log(this, " ok your account is temporary"));
                    tFile.AppendLine("$tAccountMySQL = reset($SQL_LIST); // not random for this moment;");
                    tFile.AppendLine("$UserRange = mt_rand($tAccountMySQL['min'], $tAccountMySQL['max']);// not use, just to test the possible");
                    tFile.AppendLine(NWDError.PHP_log(this, " ok your account is temporary and your can perhaps be '.$UserRange.'"));
                    tFile.AppendLine("$SQL_CURRENT_DATABASE = GetConnexionByRangeAccess($UserRange);");
                    tFile.AppendLine("$SQL_CURRENT_ACCESSRANGE = $UserRange;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("// account is not temporary : check the good database!");
                    tFile.AppendLine("preg_match('/" + NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "\\-([0-9]*)\\-[0-9]*\\-[0-9]*/', $AccountForRange, $tMatches);");
                    tFile.AppendLine("$UserRange = $tMatches[1];");
                    tFile.AppendLine(NWDError.PHP_log(this, " ok your account is certified and your range is : '.$UserRange.'"));
                    tFile.AppendLine("$SQL_CURRENT_DATABASE = GetConnexionByRangeAccess($UserRange);");
                    tFile.AppendLine("$SQL_CURRENT_ACCESSRANGE = $UserRange;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("// connect database now");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "Start"));
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "Calculate"));
                tFile.AppendLine("if (isset($SQL_CURRENT_DATABASE))");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($SQL_CURRENT_DATABASE->connect_errno)");
                    tFile.AppendLine("{");
                    {
                        //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT91));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_REQUEST_PHP + "');");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT91));
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_START_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticValuesFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// VALUES");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// datas input");
            tFile.AppendLine("$dico;");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// aes128 Encrypt");
            tFile.AppendLine("function aes128Encrypt($sData, $sKey, $sVector) {");
            tFile.AppendLine("return base64_encode(openssl_encrypt($sData, 'AES-128-ECB', $sKey, OPENSSL_RAW_DATA));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// aes128 Decrypt");
            tFile.AppendLine("function aes128Decrypt($sData, $sKey, $sVector) {");
            tFile.AppendLine("return openssl_decrypt(base64_decode($sData), 'AES-128-ECB', $sKey, OPENSSL_RAW_DATA);");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// create salt temporal for hash analyze");
            tFile.AppendLine("function saltTemporal($sFrequence, $sIndex) {");
            tFile.AppendLine("if ($sFrequence < 0 || $sFrequence >= 3600)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sFrequence = 600;");
            tFile.AppendLine("}");
            tFile.AppendLine("$unixTime = time()+$sIndex*$sFrequence; // use time() exceptional");
            tFile.AppendLine("return ($unixTime-($unixTime%$sFrequence));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get value of key in JSON dico and create variable with this name");
            tFile.AppendLine("function paramValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $$varName,$dico;");
            tFile.AppendLine("$return = true;");
            tFile.AppendLine("$$varName = NULL;");
            tFile.AppendLine("$rValue = isset($dico[$key]) ? $dico[$key] : '';// in place of $dico[$key];");
            tFile.AppendLine("if (valueValidity($key, $rValue, $ereg, $errStringIfempty, $errStringifInvalid))");
            tFile.AppendLine("{");
            tFile.AppendLine("$$varName = $rValue;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("return $return;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get POST JSON value by key");
            tFile.AppendLine("function getParams($sKey, $sDigest,$sBase64, $sCrypted) {");
            tFile.AppendLine("global $dico, $NWD_SHA_SEC, $NWD_SHA_VEC, $NWD_SLT_STR, $NWD_SLT_END;");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine("$tParam = isset($_POST[$sKey]) ? $_POST[$sKey] : '';");
            tFile.AppendLine("$tDigest = isset($_POST[$sDigest]) ? $_POST[$sDigest] : '';");
            tFile.AppendLine("if ($tParam!='')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (sha1($NWD_SLT_STR.$tParam.$NWD_SLT_END) == $tDigest)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($sCrypted==true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tParam = aes128Decrypt( $tParam, $NWD_SHA_SEC, $NWD_SHA_VEC);");
            tFile.AppendLine("if ( $tParam == NULL)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            //tFile.AppendLine("errorInfos('PAR97','Data '.$sKey.' is not an json valid!');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($sBase64==true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tParam = urldecode(base64_decode($tParam));");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("$tDico = json_decode($tParam, true);");
            tFile.AppendLine("if ($tDico == NULL)");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            //tFile.AppendLine("errorInfos('PAR99','Data '.$sKey.' is not an json valid!');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$dico = $tDico;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            //tFile.AppendLine("errorInfos('PAR98','Digest for '.$sKey.' is false');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get HEADER value brutal");
            tFile.AppendLine("function headerBrutalValue ($sVarName, $sKey)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $$sVarName;");
            tFile.AppendLine("$$sVarName = isset($_SERVER['HTTP_'.strtoupper($sKey)]) ? $_SERVER['HTTP_'.strtoupper($sKey)] : '';// in place of $_SERVER[$sKey];");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get HEADER value");
            tFile.AppendLine("function headerValue ($sVarName, $sKey, $sEreg, $sErrStringIfempty, $sErrStringifInvalid)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $$sVarName;");
            tFile.AppendLine("$tReturn = true;");
            tFile.AppendLine("$$sVarName = NULL;");
            tFile.AppendLine("$tReturn = isset($_SERVER['HTTP_'.strtoupper($sKey)]) ? $_SERVER['HTTP_'.strtoupper($sKey)] : '';// in place of $_SERVER[$sKey];");
            tFile.AppendLine("if (valueValidity($sKey, $tReturn, $sEreg, $sErrStringIfempty, $sErrStringifInvalid))");
            tFile.AppendLine("{");
            tFile.AppendLine("$$sVarName = $tReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tReturn = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("return $tReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get POST value");
            tFile.AppendLine("function postValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $$varName;");
            tFile.AppendLine("$return = true;");
            tFile.AppendLine("$$varName = NULL;");
            tFile.AppendLine("$return = isset($_POST[$key]) ? $_POST[$key] : '';// in place of $_POST[$key];");
            tFile.AppendLine("if (valueValidity($key, $return, $ereg, $errStringIfempty, $errStringifInvalid))");
            tFile.AppendLine("{");
            tFile.AppendLine("$$varName = $return;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("return $return;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// get GET value");
            tFile.AppendLine("function getValue ($varName, $key, $ereg, $errStringIfempty, $errStringifInvalid)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $$varName;");
            tFile.AppendLine("$return = true;");
            tFile.AppendLine("$$varName = NULL;");
            tFile.AppendLine("$return = isset($_GET[$key]) ? $_GET[$key] : '';// in place of $_GET[$key];");
            tFile.AppendLine("if (valueValidity($key, $return, $ereg, $errStringIfempty, $errStringifInvalid))");
            tFile.AppendLine("{");
            tFile.AppendLine("$$varName = $return;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("}");
            tFile.AppendLine("return $return;");
            tFile.AppendLine("}");
            tFile.AppendLine("// -----------------");
            tFile.AppendLine("// validity of value by ereg");
            tFile.AppendLine("function valueValidity ($key, $value, $ereg, $errStringIfempty, $errStringifInvalid)");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = true;");
            tFile.AppendLine("if ($value == '' && $errStringIfempty != '')");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("if ($errStringIfempty != '')");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_log(this, " error with '.$key.' !"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            //tFile.AppendLine("errorInfos($errStringIfempty,'Value validity of `'.$key.'` (=`'.$value.'`) is empty and it is not possible');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($ereg!='' && $errStringifInvalid!='')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!preg_match ($ereg, $value))");
            tFile.AppendLine("{");
            tFile.AppendLine("$return = false;");
            tFile.AppendLine("if ($errStringifInvalid != '')");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_log(this, " error with '.$key.' !"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            //tFile.AppendLine("errorInfos($errStringifInvalid,'Value validity of `'.$key.'` (=`'.$value.'`) is not complicent with regular expression rules');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("return $return;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_VALUES_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void CreatePHPFlashMyAppFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    // TODO Don't forget to create CSS
        //    StringBuilder tFile = new StringBuilder(string.Empty);
        //    tFile.AppendLine("<?php");
        //    tFile.AppendLine(Headlines());
        //    tFile.AppendLine("// FLASH MY APP");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    // TODO REDIRECT TO FlashMyApp.com!!! AND FINISH FLASHMYAPP.COM
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
        //    sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_FLASH_PHP, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
