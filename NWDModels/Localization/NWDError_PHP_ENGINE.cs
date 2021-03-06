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
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDErrorHelper : NWDHelper<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDError.PhpEngine(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            //tFile.Append(ENGINEPHP_ErrorDeclaration(sEnvironment));
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.Append(ENGINEPHP_Error(sEnvironment));
            tFile.Append(ENGINEPHP_log(sEnvironment));
            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private const string FUNCTIONPHP_Error = "error";
        //private const string FUNCTIONPHP_errorInfos = "errorInfos";
        //private const string FUNCTIONPHP_errorReference = "errorReference";
        public const string FUNCTIONPHP_errorDetected = "errorDetected";
        private const string FUNCTIONPHP_errorCancel = "errorCancel";
        public const string FUNCTIONPHP_errorResult = "errorResult";
        //private const string FUNCTIONPHP_errorPossibilities = "errorPossibilities";
        //-------------------------------------------------------------------------------------------------------------
        private const string FUNCTIONPHP_log = "myLog";
        private const string FUNCTIONPHP_logReturn = "myLogLineReturn";
        public const string FUNCTIONPHP_ERROR_SELECT = "ErrorGetRow";
        public const string FUNCTIONPHP_respond = "mylogRespond";
        private const string K_PHP_ERR_LOG = "$ERR_LOG";
        private const string K_PHP_ERR_LOG_CNT = "$ERR_LOG_CNT";
        private const string K_PHP_ERR_BOL = "$ERR_BOL";
        private const string K_PHP_ERR_COD = "$ERR_COD";
        private const string K_PHP_ERR_INF = "$ERR_INF";
        //-------------------------------------------------------------------------------------------------------------
        private static string ENGINEPHP_Error(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("// init error state");
            tFile.AppendLine(K_PHP_ERR_BOL + " = false;");
            tFile.AppendLine(K_PHP_ERR_COD + " = '';");
            tFile.AppendLine(K_PHP_ERR_INF + " = '';");

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("// Use to insert error pre-declare in JSON's respond and exit immedialty (optional)");
            tFile.AppendLine("function " + FUNCTIONPHP_Error + "($sCode, $sInfos, $sExit=true, $sDir='', $sFile='', $sFunction='', $sLine='')");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ", " + K_PHP_ERR_INF + ", " + NWD.K_PATH_BASE + ";");
                tFile.AppendLine(K_PHP_ERR_BOL + " = true;");
                tFile.AppendLine(K_PHP_ERR_COD + " = $sCode;");
                tFile.AppendLine(K_PHP_ERR_INF + " = $sInfos;");
                tFile.AppendLine(FUNCTIONPHP_logReturn + "();");
                tFile.AppendLine(FUNCTIONPHP_log + "('error with exit for code '.$sCode, $sDir, $sFile, $sFunction, $sLine);");
                tFile.AppendLine(FUNCTIONPHP_logReturn + "();");
                tFile.AppendLine("if ($sExit==true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
                    tFile.AppendLine("exit;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(FUNCTIONPHP_log + "('error without exit for code '.$sCode, $sDir, $sFile, $sFunction, $sLine);");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// return true if error in respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorDetected + "()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + K_PHP_ERR_BOL + ";");
                tFile.AppendLine("return " + K_PHP_ERR_BOL + ";");
            }
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// use to cancel error in respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorCancel + "()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ";");
                tFile.AppendLine("" + K_PHP_ERR_BOL + " = false;");
                tFile.AppendLine("" + K_PHP_ERR_COD + " = '';");
            }
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("// insert keys and value in JSON's respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorResult + "()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ", " + K_PHP_ERR_INF + ";");
                tFile.AppendLine("if (" + K_PHP_ERR_BOL + " == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("global $REP;");

                    if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
                    {
                        foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                        {
                            tFile.AppendLine("unset($REP['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']);");
                        }
                    }

                    tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_KEY + "', " + K_PHP_ERR_BOL + ");");
                    tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_CODE_KEY + "'," + K_PHP_ERR_COD + ");");
                    tFile.AppendLine("if (" + K_PHP_ERR_INF + "!='')");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_INFOS_KEY + "'," + K_PHP_ERR_INF + ");");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function " + FUNCTIONPHP_ERROR_SELECT + "($sErrorReference, $sLanguage = 'BASE')");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Reference) + "'] = $sErrorReference;");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'] = 0;");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'] = 'ERR';");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'] = 'ERR999';");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'] = '';");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'] = '';");
                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'] = '';");

                tFile.AppendLine("$tConnexion = GetCurrentDatabase();");
                tFile.Append("$tQuery = 'SELECT * ");
                tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDError>(sEnvironment) + "` ");
                tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Reference) + "` = \\''.$tConnexion->real_escape_string($sErrorReference).'\\' ");
                tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().AC) + "` = 1;");
                tFile.AppendLine("';");
                tFile.AppendLine("$tResult = SelectFromConnexion($tConnexion, $tQuery, '', '', false);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ERR01));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tResult['count'] != 1)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RESC04));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'] = $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'];");
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'] = $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'];");
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'] = $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'];");
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'] = GetLocalizableString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'], $sLanguage);");
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'] = GetLocalizableString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'], $sLanguage);");
                                tFile.AppendLine("$rError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'] = GetLocalizableString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'], $sLanguage);");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("return $rError;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // -------------------------------------

            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static string PHP_ErrorFunction(string sCode, string sInfos = NWEConstants.K_EMPTY_STRING, bool sExit = true)
        {
            return FUNCTIONPHP_Error + "('" + sCode.Replace("'", "\\'") + "', '" + sInfos.Replace("'", "\\'") + "', " + sExit.ToString().ToLower() + ", __DIR__, __FILE__, __FUNCTION__, __LINE__);";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_Error(NWDError sError, string sInfos = NWEConstants.K_EMPTY_STRING, bool sExit = true)
        {
            if (sError != null)
            {
                return "/* " + sError.Description.GetBaseString() + " */" + "\n" + PHP_ErrorFunction(sError.Code, sInfos, sExit);
            }
            else
            {
                return PHP_ErrorFunction("???", "", true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_Error(string sDomainCode, string sInfos = NWEConstants.K_EMPTY_STRING, bool sExit = true)
        {
            NWDError tError = NWDError.GetErrorDomainCode(sDomainCode);
            if (tError != null)
            {
                return "/* " + tError.Description.GetBaseString() + " */" + "\n" + PHP_ErrorFunction(tError.Code, sInfos, sExit);
            }
            else
            {
                return PHP_ErrorFunction("???", "", true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetErrorCode(string sDomainCode)
        {
            NWDError tError = NWDError.GetErrorDomainCode(sDomainCode);
            if (tError != null)
            {
                return tError.Code;
            }
            else
            {
                return "???";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [Obsolete] //TODO delete 
        public static string PHP_ErrorSQL(NWDAppEnvironment sEnvironment, string sQueryRef, string sConnexion)
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + FUNCTIONPHP_log + "('error in mysqli request : ('. " + sConnexion + "->errno.')'." + sConnexion + "->error.'  in : '." + sQueryRef + ".'.', __DIR__, __FILE__, __FUNCTION__, __LINE__);";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static string ENGINEPHP_log(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("// log function");
            tFile.AppendLine(K_PHP_ERR_LOG + " = '';");
            tFile.AppendLine(K_PHP_ERR_LOG_CNT + " = 0;");

            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function " + FUNCTIONPHP_logReturn + "()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + K_PHP_ERR_LOG + "," + K_PHP_ERR_LOG_CNT + ";");
                tFile.AppendLine("" + K_PHP_ERR_LOG_CNT + "++;");
                tFile.AppendLine(K_PHP_ERR_LOG + ".='\\r'." + K_PHP_ERR_LOG_CNT + ";");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function " + FUNCTIONPHP_respond + "()");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + K_PHP_ERR_LOG + ";");
                tFile.AppendLine("respondAdd('" + NWD.K_WEB_LOG_Key + "'," + K_PHP_ERR_LOG + ");");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function " + FUNCTIONPHP_log + "($sString, $sDir, $sFile, $sFunction, $sLine)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("global " + K_PHP_ERR_LOG + "," + K_PHP_ERR_LOG_CNT + ";");
                tFile.AppendLine("" + K_PHP_ERR_LOG_CNT + "++;");
                tFile.AppendLine("$sFile = basename($sDir).' / '.basename($sFile);");
                tFile.AppendLine("$t = round(strlen($sFile)/4);");
                tFile.AppendLine("$r = 20-strlen($sFile);");
                tFile.AppendLine(K_PHP_ERR_LOG + ".='\\r'." + K_PHP_ERR_LOG_CNT + ".' - '.$sFile.' ';");
                tFile.AppendLine("for ($i=$r;$i>0;$i--)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(K_PHP_ERR_LOG + ".=' ';");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("if ($sFunction!='')");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tLayout = $sFunction.'() line '.$sLine;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tLayout = 'line '.$sLine;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$r = 40-strlen($tLayout);");
                tFile.AppendLine(K_PHP_ERR_LOG + ".=$tLayout;");
                tFile.AppendLine("for ($i=$r;$i>0;$i--)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(K_PHP_ERR_LOG + ".=' ';");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(K_PHP_ERR_LOG + ".=' ';");
                tFile.AppendLine(K_PHP_ERR_LOG + ".=$sString;");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_log(NWDAppEnvironment sEnvironment, string sString)
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + FUNCTIONPHP_log + "('" + sString + "', __DIR__, __FILE__, __FUNCTION__, __LINE__);";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_logTrace(NWDAppEnvironment sEnvironment)
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + FUNCTIONPHP_log + "('DEBUG TRACE', __DIR__, __FILE__, __FUNCTION__, __LINE__);";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_BenchmarkStart(NWDAppEnvironment sEnvironment, string sKey = "'.__FUNCTION__.' <i>'.basename(__FILE__).'</i>")
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + "BenchmarkStart('"+sKey+"');";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_BenchmarkStep(NWDAppEnvironment sEnvironment, string sInfos = "",  string sKey = "'.__FUNCTION__.' <i>'.basename(__FILE__).'</i>")
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + "BenchmarkStep('" + sKey + "', '"+ sInfos + "', __LINE__);";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_BenchmarkFinish(NWDAppEnvironment sEnvironment, string sKey = "'.__FUNCTION__.' <i>'.basename(__FILE__).'</i>")
        {
            string rReturn = string.Empty;
            if (sEnvironment.LogMode == NWDEnvironmentLogMode.NoLog)
            {
                rReturn = rReturn + "//";
            }
            rReturn = rReturn + "BenchmarkFinish('" + sKey + "');";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_errorDetected()
        {
            return FUNCTIONPHP_errorDetected;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
