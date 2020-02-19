﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:25
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
using UnityEngine;
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
            tFile.AppendLine(NWD.K_CommentSeparator);
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

            tFile.AppendLine("// Use to insert error pre-declare in JSON's respond");
            tFile.AppendLine("function " + FUNCTIONPHP_Error + "($sCode, $sInfos, $sExit=true, $sFile='', $sFunction='', $sLine='')");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ", " + K_PHP_ERR_INF + ", " + NWD.K_PATH_BASE + ";");
            tFile.AppendLine(K_PHP_ERR_BOL + " = true;");
            tFile.AppendLine(K_PHP_ERR_COD + " = $sCode;");
            tFile.AppendLine(K_PHP_ERR_INF + " = $sInfos;");
            tFile.AppendLine(FUNCTIONPHP_logReturn + "();");
            tFile.AppendLine(FUNCTIONPHP_log + "('error with code '.$sCode, $sFile, $sFunction, $sLine);");
            tFile.AppendLine(FUNCTIONPHP_logReturn + "();");
            tFile.AppendLine("if ($sExit==true)");
            tFile.AppendLine("{");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
            tFile.AppendLine("exit;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(FUNCTIONPHP_log + "('error without exit', $sFile, $sFunction, $sLine);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("// Use to insert error in JSON's respond");
            //tFile.AppendLine("function " + FUNCTIONPHP_errorInfos + "($sCode,$sInfos)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("global "+K_PHP_ERR_INF+";");
            //tFile.AppendLine(""+K_PHP_ERR_INF+" = $sInfos;");
            //tFile.AppendLine("" + FUNCTIONPHP_Error + "($sCode);");
            //tFile.AppendLine("}");

            //tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("// return error in database");
            //tFile.AppendLine("function " + FUNCTIONPHP_errorReference + "($sReference)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            //tFile.AppendLine("$tRow = '';");
            //tFile.AppendLine("$tQuery = 'SELECT * FROM `'." + NWD.K_ENV + ".'_NWDError` WHERE `Reference` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            //tFile.AppendLine("// echo($tQuery);");
            //tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //tFile.AppendLine("if (!$tResult)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("error('ERRx33');");
            //tFile.AppendLine("}");
            //tFile.AppendLine("else");
            //tFile.AppendLine("{");
            //tFile.AppendLine("if ($tResult->num_rows >= 1)");
            //tFile.AppendLine("{");
            //tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            //tFile.AppendLine("}");
            //tFile.AppendLine("}");
            //tFile.AppendLine("mysqli_free_result($tResult);");
            //tFile.AppendLine("return $tRow;");
            //tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("// return true if error in respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorDetected + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_BOL + ";");
            tFile.AppendLine("return " + K_PHP_ERR_BOL + ";");
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("// use to cancel error in respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorCancel + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ";");
            tFile.AppendLine("" + K_PHP_ERR_BOL + " = false;");
            tFile.AppendLine("" + K_PHP_ERR_COD + " = '';");
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("// insert keys and value in JSON's respond");
            tFile.AppendLine("function " + FUNCTIONPHP_errorResult + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_BOL + ", " + K_PHP_ERR_COD + ", " + K_PHP_ERR_INF + ";");
            tFile.AppendLine("if (" + K_PHP_ERR_BOL + " == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $REP;");

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
                {
                    tFile.AppendLine("unset($REP['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']);");
                }
            }

            tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_KEY + "', " + K_PHP_ERR_BOL + ");");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_CODE_KEY + "'," + K_PHP_ERR_COD + ");");
            tFile.AppendLine("if (" + K_PHP_ERR_INF + "!='')");
            tFile.AppendLine("{");
            tFile.AppendLine("respondAdd('" + NWD.K_JSON_ERROR_INFOS_KEY + "'," + K_PHP_ERR_INF + ");");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("// insert keys and value in JSON's respond");
            //tFile.AppendLine("function " + FUNCTIONPHP_errorPossibilities + "()");
            //tFile.AppendLine("{");
            //tFile.AppendLine("global $ERR_LST;");
            //tFile.AppendLine("respondAdd(errorPossibilities,$ERR_LST);");
            //tFile.AppendLine("}");

            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static string PHP_Error(string sCode, string sInfos = NWEConstants.K_EMPTY_STRING, bool sExit = true)
        {
            return FUNCTIONPHP_Error + "('" + sCode.Replace("'", "\\'") + "', '" + sInfos.Replace("'", "\\'") + "', " + sExit.ToString().ToLower() + ", __FILE__, __FUNCTION__, __LINE__);" + "\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_Error(NWDError sError, string sInfos = NWEConstants.K_EMPTY_STRING, bool sExit = true)
        {
            if (sError != null)
            {
                return "/* " + sError.Description.GetBaseString() + " */" + "\n" + PHP_Error(sError.Code, sInfos, sExit);
            }
            else
            {
                return PHP_Error("???", "", true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static string PHP_ErrorInfos(string sCode, string sInfos)
        //{
        //    return FUNCTIONPHP_errorInfos + "('" + sCode.Replace("'", "\\'") + "', '" + sInfos.Replace("'", "\\'") + "')";
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_ErrorSQL(NWDAppEnvironment sEnvironment, string sQueryRef)
        {
            if (sEnvironment.LogMode == true)
            {
                return FUNCTIONPHP_log + "('error in mysqli request : ('. " + NWD.K_SQL_CON + "->errno.')'. " + NWD.K_SQL_CON + "->error.'  in : '." + sQueryRef + ".'.', __FILE__, __FUNCTION__, __LINE__);" + "\n";
            }
            else
            {
                return "/* no log */";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static string ENGINEPHP_log(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("// log function");
            tFile.AppendLine(K_PHP_ERR_LOG + " = '';");
            tFile.AppendLine(K_PHP_ERR_LOG_CNT + " = 0;");

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + FUNCTIONPHP_ERROR_SELECT + "($sErrorReference, $sLanguage = 'BASE')");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Reference) + "'] = $sErrorReference;");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'] = 0;");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'] = 'ERR';");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'] = 'ERR999';");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'] = '';");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'] = '';");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'] = '';");
            tFile.Append("$tQueryError = 'SELECT * ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDError>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Reference) + "` = \\''.$SQL_CON->real_escape_string($sErrorReference).'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultError = $SQL_CON->query($tQueryError);");
            tFile.AppendLine("if (!$tResultError)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryError"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("ERR", "ERR01", "ERROR error", "Invalid Request.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultError->num_rows != 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("ERR", "RESC04", "ERROR error", "Invalid Reference match.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tErrorAssoc = $tResultError->fetch_assoc();");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'] = $tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Type) + "'];");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'] = $tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Domain) + "'];");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'] = $tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Code) + "'];");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'] = GetLocalizableString($tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'],$sLanguage);");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'] = GetLocalizableString($tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'],$sLanguage);");
            tFile.AppendLine("$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'] = GetLocalizableString($tErrorAssoc['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Validation) + "'],$sLanguage);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultError);");
            tFile.AppendLine("return $tError;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + FUNCTIONPHP_logReturn + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_LOG + "," + K_PHP_ERR_LOG_CNT + ";");
            tFile.AppendLine("" + K_PHP_ERR_LOG_CNT + "++;");
            tFile.AppendLine(K_PHP_ERR_LOG + ".='\\r'." + K_PHP_ERR_LOG_CNT + ";");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + FUNCTIONPHP_respond + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_LOG + ";");
            tFile.AppendLine("respondAdd('log'," + K_PHP_ERR_LOG + ");"); // TODO : replace log key by constant
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + FUNCTIONPHP_log + "($sString, $sFile, $sFunction, $sLine)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + K_PHP_ERR_LOG + "," + K_PHP_ERR_LOG_CNT + ";");
            tFile.AppendLine("" + K_PHP_ERR_LOG_CNT + "++;");
            tFile.AppendLine("$sFile = basename($sFile);");
            tFile.AppendLine("$t = round(strlen($sFile)/4);");
            tFile.AppendLine("$r = 20-strlen($sFile);");
            tFile.AppendLine(K_PHP_ERR_LOG + ".='\\r'." + K_PHP_ERR_LOG_CNT + ".' - '.$sFile.' ';");
            tFile.AppendLine("for ($i=$r;$i>0;$i--)");
            tFile.AppendLine("{");
            tFile.AppendLine(K_PHP_ERR_LOG + ".=' ';");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($sFunction!='')");
            tFile.AppendLine("{");
            tFile.AppendLine("$tLayout = $sFunction.'() line '.$sLine;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tLayout = 'line '.$sLine;");
            tFile.AppendLine("}");
            tFile.AppendLine("$r = 40-strlen($tLayout);");
            tFile.AppendLine(K_PHP_ERR_LOG + ".=$tLayout;");
            tFile.AppendLine("for ($i=$r;$i>0;$i--)");
            tFile.AppendLine("{");
            tFile.AppendLine(K_PHP_ERR_LOG + ".=' ';");
            tFile.AppendLine("}");
            tFile.AppendLine(K_PHP_ERR_LOG + ".=$sString;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_log(NWDAppEnvironment sEnvironment, string sString)
        {
            if (sEnvironment.LogMode == true)
            {
                return FUNCTIONPHP_log + "('" + sString + "', __FILE__, __FUNCTION__, __LINE__);\n";
            }
            else
            {
                return "/* no log */";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_logTrace(NWDAppEnvironment sEnvironment)
        {
            return FUNCTIONPHP_log + "('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);";
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