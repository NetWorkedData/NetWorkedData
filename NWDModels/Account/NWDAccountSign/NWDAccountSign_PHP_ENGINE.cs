//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDAccountSign.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDError>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // --------------------------------------
            tFile.AppendLine("function RescueSignProceed($sEmail, $sLanguage, $sFyr)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $ENV, $WSBUILD, $HTTP_URL, $NWD_APP_NAM, $NWD_RES_MAIL, $NWD_SLT_STR, $NWD_SLT_END;");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("$tErrorReference = '" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescuePageError).Reference + "';");
            tFile.AppendLine("$sEmail = str_replace('%','',$sEmail);");
            tFile.Append("$tQuerySign = 'SELECT * ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string(sha1(strtolower($sEmail).'" + NWDAppEnvironment.SelectedEnvironment().SaltStart + "')).'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = $SQL_CON->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC01", "Rescue error", "Invalid reqest.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC02", "Rescue error", "No match.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultSign->num_rows == 1)");
            tFile.AppendLine("{");

            tFile.AppendLine("$tSign = $tResultSign->fetch_assoc();");
            tFile.AppendLine("$tShA = sha1(saltTemporal(" + sEnvironment.RescueDelay + ",0).$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "'].$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "']);");
            tFile.AppendLine("$tShB = sha1(saltTemporal(" + sEnvironment.RescueDelay + ",1).$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "'].$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "']);");
            tFile.AppendLine("$tShC = sha1(saltTemporal(" + sEnvironment.RescueDelay + ",-1).$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "'].$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "']);");

            tFile.AppendLine("if ($tShA == $sFyr || $tShB == $sFyr || $tShC == $sFyr)");
            tFile.AppendLine("{");
            // draw page success
            tFile.AppendLine("$tErrorReference = '" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescuePage).Reference + "';");

            // EMAIL PASSWORD PROCESS
            tFile.AppendLine("if ($tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignType) + "'] == " + NWDAccountSignType.EmailPassword.ToLong() + ")");
            tFile.AppendLine("{");
            tFile.AppendLine("$tResult = false;");
            tFile.AppendLine("while($tResult == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tPassword = RandomString(" + sEnvironment.RescuePasswordLength + ");");
            tFile.AppendLine("$tSignHash = sha1($sEmail.$tPassword.$NWD_SLT_END);");
            tFile.AppendLine("// test if hash exists");
            tFile.Append("$tQueryTest = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "` ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$SQL_CON->real_escape_string($tSignHash).'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultTest = " + NWD.K_SQL_CON + "->query($tQueryTest);");
            tFile.AppendLine("if (!$tResultTest)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryTest", NWD.K_SQL_CON));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultTest->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tResult = true;");
            tFile.Append("$tQueryUpdate = 'UPDATE `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("SET  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$tSignHash.'\\' ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string(sha1(strtolower($sEmail).'" + NWDAppEnvironment.SelectedEnvironment().SaltStart + "')).'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultUpdate = " + NWD.K_SQL_CON + "->query($tQueryUpdate);");
            tFile.AppendLine("if (!$tResultUpdate)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryUpdate", NWD.K_SQL_CON));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "']);");
            tFile.AppendLine("$tErrorToPost = " + NWDError.FUNCTIONPHP_ERROR_SELECT + "('" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescueAnswerEmail).Reference + "', $sLanguage);");
            tFile.AppendLine("$tSubject = str_replace('{APP}',$NWD_APP_NAM,$tErrorToPost['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{APP}',$NWD_APP_NAM,$tErrorToPost['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{PASSWORD}',$tPassword, $tMessage);");
            tFile.AppendLine("SendEmail($tSubject, $tMessage, $sEmail, $NWD_RES_MAIL);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            // LOGIN PASSWORD EMAIL PROCESS
            tFile.AppendLine("if ($tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignType) + "'] == " + NWDAccountSignType.LoginPasswordEmail.ToLong() + ")");
            tFile.AppendLine("{");


            tFile.AppendLine("$tResult = false;");
            tFile.AppendLine("while($tResult == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tLogin = RandomString(" + sEnvironment.RescueLoginLength + ");");
            tFile.AppendLine("$tPassword = RandomString(" + sEnvironment.RescuePasswordLength + ");");
            tFile.AppendLine("$tSignHash = sha1($tLogin.$tPassword.$NWD_SLT_END);");
            tFile.AppendLine("// test if hash exists");
            tFile.Append("$tQueryTest = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "` ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$SQL_CON->real_escape_string($tSignHash).'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultTest = " + NWD.K_SQL_CON + "->query($tQueryTest);");
            tFile.AppendLine("if (!$tResultTest)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryTest", NWD.K_SQL_CON));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultTest->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tResult = true;");
            tFile.Append("$tQueryUpdate = 'UPDATE `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("SET  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$tSignHash.'\\' ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string(sha1(strtolower($sEmail).'" + NWDAppEnvironment.SelectedEnvironment().SaltStart + "')).'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultUpdate = " + NWD.K_SQL_CON + "->query($tQueryUpdate);");
            tFile.AppendLine("if (!$tResultUpdate)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryUpdate", NWD.K_SQL_CON));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "']);");
            tFile.AppendLine("$tErrorToPost = " + NWDError.FUNCTIONPHP_ERROR_SELECT + "('" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescueAnswerLogin).Reference + "', $sLanguage);");
            tFile.AppendLine("$tSubject = str_replace('{APP}',$NWD_APP_NAM,$tErrorToPost['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{APP}',$NWD_APP_NAM,$tErrorToPost['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{PASSWORD}',$tPassword, $tMessage);");
            tFile.AppendLine("$tMessage = str_replace('{LOGIN}',$tLogin, $tMessage);");
            tFile.AppendLine("SendEmail($tSubject, $tMessage, $sEmail, $NWD_RES_MAIL);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            // draw page lose
            tFile.AppendLine("$tErrorReference = '" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescuePageError).Reference + "';");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
            tFile.AppendLine("// to much users ...");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKI : '.$sSDKI.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RESC03));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultSign);");
            tFile.AppendLine("}");




            tFile.AppendLine("$tError = " + NWDError.FUNCTIONPHP_ERROR_SELECT + "($tErrorReference, $sLanguage);");

            tFile.AppendLine("$tSubject = str_replace('{APP}',$NWD_APP_NAM,$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{APP}',$NWD_APP_NAM,$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "']);");
            tFile.AppendLine("echo('<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">');");
            tFile.AppendLine("echo('<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"'.$sLanguage.'\">');");
            tFile.AppendLine("echo('<head>');");
            tFile.AppendLine("echo('<title>'.$tSubject.'</title>');");
            tFile.AppendLine("echo('</head>');");
            tFile.AppendLine("echo('<body>');");
            tFile.AppendLine("echo($tMessage);");
            tFile.AppendLine("echo('</body>');");
            tFile.AppendLine("echo('</html>');");

            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            // --------------------------------------
            tFile.AppendLine("function RescueSign($sEmail, $sLanguage)");
            tFile.AppendLine("{");
            {
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD, $HTTP_URL, $NWD_APP_NAM, $NWD_RES_MAIL;");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("$sEmail = str_replace('%','',$sEmail);");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
                {
            tFile.Append("$tQuerySign = 'SELECT * ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string(sha1(strtolower($sEmail).'" + NWDAppEnvironment.SelectedEnvironment().SaltStart + "')).'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = " + NWD.K_SQL_CON + "->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
                    {
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RESC01));
                    }
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
                    {
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
                        {
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RESC02));
                        }
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultSign->num_rows == 1)");
            tFile.AppendLine("{");
                        {
            tFile.AppendLine("$tSign = $tResultSign->fetch_assoc();");
            // create a temporal token with secret of actual hash
            tFile.AppendLine("$tSh = sha1(saltTemporal(" + sEnvironment.RescueDelay + ",0).$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "'].$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "']);");
            tFile.AppendLine("$tUrl = $HTTP_URL.'/'.$ENV.'/" + NWD.K_STATIC_RESCUE_PHP + "?" + NWD.K_WEB_RESCUE_EMAIL_Key + "='.$sEmail.'&" + NWD.K_WEB_RESCUE_LANGUAGE_Key + "='.$sLanguage.'&" + NWD.K_WEB_RESCUE_PROOF_Key + "='.$tSh.'';");

            tFile.AppendLine("$tError = " + NWDError.FUNCTIONPHP_ERROR_SELECT + "('" + NWDError.GetErrorDomainCode(NWDError.NWDError_RescueRequest).Reference + "', $sLanguage);");
            tFile.AppendLine("$tSubject = str_replace('{APP}',$NWD_APP_NAM,$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{APP}',$NWD_APP_NAM,$tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "']);");
            tFile.AppendLine("$tMessage = str_replace('{URL}',$tUrl,$tMessage);");
            tFile.AppendLine("SendEmail($tSubject, $tMessage, $sEmail, $NWD_RES_MAIL);");
                        }
            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
                        {
            tFile.AppendLine("// to much users ...");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKI : '.$sSDKI.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_RESC03));
                        }
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultSign);");
                    }
            tFile.AppendLine("}");
                }
            tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            // --------------------------------------
            tFile.AppendLine("function CreateAccountSign($sAccountReference, $sSDKt, $sSDKv, $sSDKr, $sSDKl)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("$rReturn = false;");
                tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
                tFile.AppendLine("global $shs, $ereg_token;");
                tFile.AppendLine("global $SQL_LIST, " + NWD.K_SQL_CON_EDITOR + ";");
                tFile.AppendLine("global $ENV, $WSBUILD;");
                tFile.AppendLine("$sSDKt = str_replace('%','',str_replace('_','',$sSDKt));");
                tFile.AppendLine("$sSDKv = str_replace('%','',str_replace('_','',$sSDKv));");
                tFile.AppendLine("$sSDKr = str_replace('%','',str_replace('_','',$sSDKr));");
                tFile.AppendLine("$sSDKl = str_replace('%','',str_replace('_','',$sSDKl));");
                tFile.AppendLine("$tInternalKey = '';");
                tFile.AppendLine("$tInternalDescription = '';");
                tFile.AppendLine("$tInsertSQL='';");
                tFile.AppendLine("$tInsertSQLValue='';");
                tFile.AppendLine("$tRangeAccess = explode('-',$sAccountReference)[1];");
                tFile.AppendLine("$tConnexion = GetConnexionByRangeAccess($tRangeAccess);");
                // create a reference base on $sAccountReference and test if unique in database  ... more quickly 
                tFile.AppendLine("$tReference = referenceGenerateRange ($tConnexion, $tRangeAccess, '" + NWDBasisHelper.BasisHelper<NWDAccountSign>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "');");
                tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` (';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$tReference.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");

                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "`, '; $tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($sAccountReference).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RangeAccess) + "`, '; $tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($tRangeAccess).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignType) + "`, '; $tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($sSDKt).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "`, ';$tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($sSDKv).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "`, ';$tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($sSDKr).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().LoginHash) + "`, ';$tInsertSQLValue.= '\\''.$tConnexion->real_escape_string($sSDKl).'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "`, ';$tInsertSQLValue.= '\\'" + (int)NWDAccountSignAction.Associated + "\\', ';");

                if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
                {
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Sign\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Dev AccounSign'.$TIME_SYNC.'\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                }
                if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Certified\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Preprod AccountSign'.$TIME_SYNC.'\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                }
                if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                {
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                    tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                }
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                //tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Bundle) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
                tFile.AppendLine("$tInsertSQL.=')';");
                tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");

                tFile.AppendLine("// find the good database");
                tFile.AppendLine("$tRangeToUse = 0;");
                tFile.AppendLine("foreach ($SQL_LIST as $tRange => $tValue)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if ($tRangeAccess >= $tValue['min'] && $tRangeAccess <= $tValue['max'])");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$tRangeToUse = $tRange;");
                        tFile.AppendLine("if (isset(" + NWD.K_SQL_CON_EDITOR + "[$tRange]) == false)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("" + NWD.K_SQL_CON_EDITOR + "[$tRange] = new mysqli($tValue['host'], $tValue['user'], $tValue['password'], $tValue['database'], $tValue['port']);");
                            tFile.AppendLine("if (" + NWD.K_SQL_CON_EDITOR + "[$tRange]->connect_errno)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in MySQL connexion on '.$tValue['host'].' for '.$tValue['user'].' with password •••••••••••• on database '.$tValue['database'].'"));
                                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                                tFile.AppendLine("include_once ('" + NWD.K_STATIC_FINISH_PHP + "');");
                                tFile.AppendLine("exit;");
                            }
                            tFile.AppendLine("}");
                            tFile.AppendLine("else");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tRange.' connexion success on '.$tValue['title'].' => '.$tValue['id'].'"));
                                tFile.AppendLine("$Connexion = " + NWD.K_SQL_CON_EDITOR + "[$tRange];");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("break;");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");

                tFile.AppendLine("$tResult = $tConnexion->query($tInsertSQL);");
                tFile.AppendLine("if (!$tResult)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL", "$tConnexion"));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$rReturn = true;");
                    tFile.AppendLine("" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tConnexion, $tReference);");
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