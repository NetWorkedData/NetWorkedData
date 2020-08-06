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
    public partial class NWDRequestTokenHelper : NWDHelper<NWDRequestToken>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDRequestToken.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDRequestToken : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDAccount>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDRequestToken>().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenCreate($sUUIDHash)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global $TIME_SYNC;");
                tFile.AppendLine("$tConnexion = GetConnexionForAccount($sUUIDHash);");
                tFile.AppendLine("$tToken = NWDRequestTokenGenerateToken($sUUIDHash);");
                tFile.AppendLine("$tQuery = 'INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDRequestToken>(sEnvironment) + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DC) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DM) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DD) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().AC) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Account) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Integrity) + "`) VALUES ( \\''.$TIME_SYNC.'\\', \\''.$TIME_SYNC.'\\', \\'0\\', \\'1\\', \\''.EscapeString($tToken).'\\', \\''.EscapeString($sUUIDHash).'\\', \\'???????\\' );';");
                tFile.AppendLine("$tResult = ExecuteInConnexion($tConnexion, $tQuery);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT01));
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
                tFile.AppendLine("return $tToken;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenDeleteOldToken ($sUUIDHash, $sTimestamp, $sToken)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "delete old token"));
                tFile.AppendLine("$tConnexion = GetConnexionForAccount($sUUIDHash);");
                tFile.AppendLine("$tQuery = 'DELETE FROM `" + NWDBasisHelper.TableNamePHP<NWDRequestToken>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Account) + "` = \\''.EscapeString($sUUIDHash).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DM) + "` <= \\''.EscapeString($sTimestamp).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "` != \\''.EscapeString($sToken).'\\';';");
                tFile.AppendLine("$tResult = ExecuteInConnexion($tConnexion, $tQuery);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT14));
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenGenerateToken ($sUUIDHash)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global $TIME_SYNC;");
                tFile.AppendLine("$tRandom = $sUUIDHash.'-'.$TIME_SYNC.'-'.rand ( 1000000000 , 9999999999 ).'-0';");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
                tFile.AppendLine("return md5($tRandom);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenDeleteAllToken ($sUUIDHash)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("$tConnexion = GetConnexionForAccount($sUUIDHash);");
                tFile.AppendLine("$tQuery = 'DELETE FROM `" + NWDBasisHelper.TableNamePHP<NWDRequestToken>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Account) + "` = \\''.EscapeString($sUUIDHash).'\\';';");
                tFile.AppendLine("$tResult = ExecuteInConnexion($tConnexion, $tQuery);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT13));
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenReset ($sUUIDHash)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("NWDRequestTokenDeleteAllToken ($sUUIDHash);");
                tFile.AppendLine("respondToken(NWDRequestTokenCreate ($sUUIDHash));");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("function NWDRequestTokenIsValid ($sUUIDHash, $sToken)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global $TIME_SYNC;");
                tFile.AppendLine("global $sdki;");
                tFile.AppendLine("global $token_FirstUse;");
                tFile.AppendLine("global $RTH;");
                tFile.AppendLine("$rReturn = false;");
                tFile.AppendLine("$tConnexion = GetConnexionForAccount($sUUIDHash);");
                tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "`,`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DM) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().AC) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDRequestToken>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Account) + "` = \\''.EscapeString($sUUIDHash).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DD) + "` = \\'0\\';';");
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(sEnvironment));
                tFile.AppendLine("$tResult = SelectFromConnexion($tConnexion, $tQuery, '"+NWDError.GetErrorCode(NWDError.NWDError_RQT12)+ "', '', true);");
                tFile.AppendLine(NWDError.PHP_BenchmarkStep(sEnvironment));
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT12));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tResult['count'] == 0)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if (TestTemporaryAccount($sUUIDHash))");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$ereg_password = '/^(.{24,64})$/';");
                            tFile.AppendLine("if (paramValue('sdki', '" + NWD.K_WEB_SIGN_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "It is an account temporary"));
                                tFile.AppendLine("$tNewUuid = FindAccount($sUUIDHash, $sdki, true);");
                                tFile.AppendLine("respond_RestartWebService();");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("// not possible ... the token is too old and the base was purged since the last connexion");
                            tFile.AppendLine("$rReturn = false;");
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT90));
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else if ($tResult['count'] <= $RTH)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// ok I have some token for this user ...");
                        tFile.AppendLine("$tTokenIsValid = false;");
                        tFile.AppendLine("$tTimestamp = 0;");
                        tFile.AppendLine("$tToken = '';");
                        tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "find token: '.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "'].'"));
                                tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "'] == $sToken)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().AC) + "'] == 0)");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "find OLD token reused: '.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "'].'"));
                                        tFile.AppendLine("$token_FirstUse = false;");
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("else");
                                    tFile.AppendLine("{");
                                    {
                                        tFile.AppendLine("$token_FirstUse = true;");
                                        tFile.AppendLine("$tQueryUseToken = 'UPDATE `" + NWDBasisHelper.TableNamePHP<NWDRequestToken>(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().AC) + "` = \\'0\\' WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "` = \\''.EscapeString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "']).'\\';';");
                                        tFile.AppendLine("$tResultUseToken = ExecuteInConnexion($tConnexionSub, $tQueryUseToken);");
                                        tFile.AppendLine("if ($tResultUseToken['error'] == true)");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResultUseToken['error_log'].'"));
                                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT11));
                                        }
                                        tFile.AppendLine("}");
                                        tFile.AppendLine("else");
                                        tFile.AppendLine("{");
                                        {
                                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "find token, Use IT: '.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "'].'"));
                                        }
                                        tFile.AppendLine("}");
                                    }
                                    tFile.AppendLine("}");
                                    tFile.AppendLine("$tTokenIsValid = true;");
                                    tFile.AppendLine("$tTimestamp = $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().DM) + "'];");
                                    tFile.AppendLine("$tToken = $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDRequestToken>().Token) + "'];");
                                }
                                tFile.AppendLine("}");
                                tFile.AppendLine("else");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("// Not the good token ... newest or oldest ... don't use it to analyze");
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");

                        tFile.AppendLine("if ($tTokenIsValid==true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$rReturn = true;");
                            tFile.AppendLine("NWDRequestTokenDeleteOldToken($sUUIDHash, $tTimestamp, $tToken);");
                            tFile.AppendLine("respondToken(NWDRequestTokenCreate($sUUIDHash));");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$rReturn = false;");
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT91));
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// not possible ... the token are too number");
                        tFile.AppendLine("$rReturn = false;");
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "not possible ... the token are too number : '.$tResult['count'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RQT93));
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment));
                tFile.AppendLine("return $rReturn;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //---------------------------------------
            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif