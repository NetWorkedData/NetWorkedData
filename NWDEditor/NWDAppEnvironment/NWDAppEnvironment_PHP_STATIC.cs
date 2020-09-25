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
using System.Collections.Generic;
using System.Text;
//=====================================================================================================================
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticFinishFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// FINISH");
            tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "Calculate"));
            tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "Finish"));
            tFile.AppendLine("global $admin;");
            tFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode != NWDEnvironmentLogMode.NoLog)
            {
                tFile.AppendLine("if ($admin == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(this, "WS as Admin"));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(this, "WS as Player"));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("//Insert logs for acces database debug");
                tFile.AppendLine("global $SQL_ACCESS_COUNT;");
                tFile.AppendLine("global $SQL_ACCESS_SQL;");
                tFile.AppendLine("respondAdd('SQL_ACCESS_COUNT', $SQL_ACCESS_COUNT);");
                tFile.AppendLine("respondAdd('SQL_ACCESS_SQL', $SQL_ACCESS_SQL);");
            }
            tFile.AppendLine("// prevent include from function for exit (typical example: error('XXX', true);)");
            tFile.AppendLine("global $admin," + NWD.K_SQL_CON_EDITOR + ", $NWD_SLT_TMP, $NWD_TMA, $RRR_LOG, $REP, $WSBUILD, " + NWD.K_PHP_TIME_SYNC + ", $REF_NEEDED, $ACC_NEEDED, " + NWD.K_ENV + ", $NWD_SHA_VEC, $NWD_SHA_SEC, $NWD_SLT_STR, $NWD_SLT_END;");

            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// web-services build");
            tFile.AppendLine("respondAdd('wsbuild',$WSBUILD);");
            // add geoip for test
            tFile.AppendLine("if (isset($_SERVER[\"GEOIP_CONTINENT_CODE\"]))");
            tFile.AppendLine("{");
            tFile.AppendLine("$continent = $_SERVER[\"GEOIP_CONTINENT_CODE\"];");
            tFile.AppendLine("}");
            tFile.AppendLine("elseif(function_exists(\"geoip_continent_code_by_name\") && isset($_SERVER['REMOTE_ADDR']))");
            tFile.AppendLine("{");
            tFile.AppendLine("$continent = geoip_continent_code_by_name($_SERVER['REMOTE_ADDR']);");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$continent = 'unknown';");
            tFile.AppendLine("}");
            tFile.AppendLine("respondAdd('continent',$continent);");
            // finish geoip for test
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("//disconnect mysql");
            tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$tConnexion)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_log(this, "close a sqlite connexion"));
                tFile.AppendLine("mysqli_close($tConnexion);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode != NWDEnvironmentLogMode.NoLog)
            {
                tFile.AppendLine("// add log");
                tFile.AppendLine(NWDError.FUNCTIONPHP_respond + "();");
            }
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
            {
                tFile.AppendLine("header('" + NWD.HashKey + ": '.sha1($temporalSalt.$NWD_SHA_VEC.$REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "']));");
                tFile.AppendLine("header('" + NWD.K_WEB_REQUEST_TOKEN_KEY + ": '.$REP['" + NWD.K_WEB_REQUEST_TOKEN_KEY + "']);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("$json = json_encode($REP);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("if (respondIsset('" + NWD.K_JSON_SECURE_KEY + "'))");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("header('" + NWEUnityWebService.SecureKey + ": " + NWEUnityWebService.SecureDigestKey + "');");
                if (LogMode != NWDEnvironmentLogMode.NoLog)
                {
                    tFile.AppendLine("$REPSCR['" + NWD.K_WEB_LOG_Key + "'] = $REP['" + NWD.K_WEB_LOG_Key + "'];");
                }
                tFile.AppendLine("$REPSCR['" + NWEUnityWebService.SecureKey + "'] = aes128Encrypt( $json, $NWD_SHA_SEC, $NWD_SHA_VEC);");
                tFile.AppendLine("$REPSCR['" + NWEUnityWebService.SecureDigestKey + "'] = sha1($NWD_SLT_STR.$REPSCR['" + NWEUnityWebService.SecureKey + "'].$NWD_SLT_END);");
                tFile.AppendLine("$json = json_encode($REPSCR);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// write JSON");
            tFile.AppendLine("echo($json);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_FINISH_PHP, tFileFormatted);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticFunctionsFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
            //string tSignAccountKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account);
            //string tSignHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash);
            //string tRescueHashKey = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash);
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
            tFile.AppendLine("function HashSign($sValue)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global $NWD_SLT_END;");
                tFile.AppendLine("return sha1($sValue.$NWD_SLT_END);");
                //tFile.AppendLine("return $sValue;");
            }
            tFile.AppendLine("}");
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
                        if (NWDProjectCredentialsManagerContent.ShowPasswordInLog == true)
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
                                if (NWDProjectCredentialsManagerContent.ShowPasswordInLog == true)
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

                if (LogMode != NWDEnvironmentLogMode.NoLog)
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
                if (LogMode != NWDEnvironmentLogMode.NoLog)
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
                    if (LogMode != NWDEnvironmentLogMode.NoLog)
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
                    if (LogMode != NWDEnvironmentLogMode.NoLog)
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
                if (NWDCluster.SelectClusterforEnvironment(this, false).MailBySMTP == true)
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
                    if (NWDCluster.SelectClusterforEnvironment(this, false).MailAuth == true)
                    {
                        tFile.AppendLine("'host' => 'ssl://'.$SMTP_HOST, ");
                    }
                    else
                    {
                        tFile.AppendLine("'host' => $SMTP_HOST, ");
                    }
                    tFile.AppendLine("'port' => $SMTP_PORT, ");
                    if (NWDCluster.SelectClusterforEnvironment(this, false).MailAuth == true)
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
                tFile.AppendLine("global $admin;");
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
                tFile.AppendLine("$admin = $rReturn;");
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
                                            tFile.AppendLine("$tPinCode = '" + tGlobalChar + "'.CodeRandomSizable($tSize++);");
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
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_FUNCTIONS_PHP, tFileFormatted);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticRequestFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            tFile.AppendLine("// REQUEST");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDAccount>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDRequestToken>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDVersion>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("{");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("if (headerValue ('os', '" + NWD.K_WEB_HEADER_OS_KEY + "', $ereg_os, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA11).Code + "')) // test if os infos is valid");
            tFile.AppendLine("{");
            tFile.AppendLine("if (headerValue ('version', '" + NWD.K_WEB_HEADER_VERSION_KEY + "', $ereg_version, '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA02).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_HEA12).Code + "')) // test if version is ok");
            tFile.AppendLine("{");
            tFile.AppendLine("// I must prevent admin mode in table creation");
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
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_HEA90));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_REQUEST_PHP, tFileFormatted);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticRespondFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
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
            tFile.AppendLine("function respond_RestartWebService()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_RESTART_WEBSERVICE_KEY + "'] = true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function respond_UserTransfert($sOldReference, $sNewReference, $sSalt)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_USER_TRANSFERT_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_PREVIEW_USER_KEY + "'] = $sOldReference;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEXT_USER_KEY + "'] = $sNewReference;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_SALT_KEY + "'] = $sSalt;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function respond_NewUser($sOldReference, $sNewReference, $sSalt)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_SALT_KEY + "'] = $sSalt;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function respond_ChangeUser($sOldReference, $sNewReference, $sSalt)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(this));
            tFile.AppendLine(NWDError.PHP_log(this, "$sOldReference = '.$sOldReference.'"));
            tFile.AppendLine(NWDError.PHP_log(this, "$sNewReference = '.$sNewReference.'"));
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $CHANGE_USER;");
            tFile.AppendLine("$CHANGE_USER = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_ACTION_NEW_USER_KEY + "'] = true;");
            tFile.AppendLine("$REP['" + NWD.K_WEB_SALT_KEY + "'] = $sSalt;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_RESPOND_PHP, tFileFormatted);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticStartFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
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
            tFile.AppendLine("global $admin;");
            tFile.AppendLine("headerBrutalValue ('adminHash', '" + NWD.AdminHashKey + "');");

            tFile.AppendLine("if ($adminHash != '')");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("$admin = adminHashTest ($adminHash, $NWD_ADM_KEY, $NWD_SLT_TMP);");
                tFile.AppendLine("if ($admin == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_BenchmarkStart(this, "ConnectAllDataBase"));
                    tFile.AppendLine("ConnectAllDatabases();");
                    tFile.AppendLine(NWDError.PHP_BenchmarkFinish(this, "ConnectAllDataBase"));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ADMIN));
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("// connect for account");
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
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT91));
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_STATIC_START_PHP, tFileFormatted);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHP_StaticValuesFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
