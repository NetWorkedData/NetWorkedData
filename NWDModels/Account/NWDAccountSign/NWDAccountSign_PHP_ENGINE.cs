//=====================================================================================================================
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
    public partial class NWDAccountSign : NWDBasis
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


            tFile.AppendLine("function RescueSign($sEmail, $sLanguage)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            // on cherche une correspondance
            // si on trouve on cherche le message a envoyer
            // on va creer un token valid 60 minutes ... ensuite byebye (peut-etre le rajputer comme parametres dans l'environement?)
            // NWDError.NWDError_RescueRequest
            // on envoie le message via le compte de sercours prévu (dans l'environement)

            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD, $HTTP_URL;");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
            tFile.Append("$tQuerySign = 'SELECT * ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string(sha1($sEmail.'" + NWDAppEnvironment.SelectedEnvironment().SaltStart + "').'\\' ");
            tFile.Append("AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = $SQL_CON->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC01", "Rescue error", "Invalid reqest.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC02", "Rescue error", "No match.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultSign->num_rows == 1)");
            tFile.AppendLine("{");

            tFile.Append("$tQueryError = 'SELECT * ");
            tFile.Append("FROM `" + NWDBasisHelper.TableNamePHP<NWDError>(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Reference) + "` = \\''.$SQL_CON->real_escape_string('" + NWDError.NWDError_RescueRequest.Reference + ".'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultError = $SQL_CON->query($tQueryError);");
            tFile.AppendLine("if (!$tResultError)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryError"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC03", "Rescue error", "Invalid message.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultError->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC04", "Rescue error", "No message match.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultError->num_rows >= 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tSign = $tResultSign->fetch_assoc");
            // create a temporal token with secret of actual hash
            tFile.AppendLine("$tSh = sha1(saltTemporal(3600,0).$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "'].$tSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "']);");
            tFile.AppendLine("$tUrl = $HTTP_URL.'/"+NWD.K_STATIC_RESCUE_PHP+"?"+NWD.K_WEB_RESCUE_EMAIL_Key+"='.$sEmail.'&"+NWD.K_WEB_RESCUE_LANGUAGE_Key+"='.$sLanguage.'&"+NWD.K_WEB_RESCUE_PROOF_Key+"='.$tSh.'';");

            tFile.AppendLine("$tError = $tResultError->fetch_assoc");
            tFile.AppendLine("$tSubject = str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Title) + "'], $sLanguage));");
            tFile.AppendLine("$tMessage = str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'], $sLanguage));");
            tFile.AppendLine("$tMessage = str_replace(\"{URL}\",$tUrl,$tMessage);");

            tFile.AppendLine("$tMessage = str_replace(\"{APP}\",$NWD_APP_NAM,GetLocalizableString($tError['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDError>().Description) + "'], $sLanguage));");
            tFile.AppendLine("SendEmail($tSubject, $tMessage, $sEmail)");

            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
            tFile.AppendLine("// to much users ...");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKI : '.$sSDKI.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.CreateGenericError("RESC", "RESC03", "Rescue error", "Too much match.", "OK", NWDErrorType.Alert)));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultSign);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function CreateAccountSign($sAccountReference, $sSDKt, $sSDKv, $sSDKr)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
            tFile.AppendLine("global $shs, $ereg_token;");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("$tInternalKey = '';");
            tFile.AppendLine("$tInternalDescription = '';");
            tFile.AppendLine("$tInsertSQL='';");
            tFile.AppendLine("$tInsertSQLValue='';");
            tFile.AppendLine("$tReference = referenceGenerate ('" + NWDBasisHelper.BasisHelper<NWDAccountSign>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "');");
            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "` (';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$tReference.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");

            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sAccountReference).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignType) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKt).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "`, ';$tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKv).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "`, ';$tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($sSDKr).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "`, ';$tInsertSQLValue.= '\\'" + (int)NWDAccountSignAction.Associated + "\\', ';");

            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'SignUp Sign\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\'Dev AccounSignt'.$TIME_SYNC.'\\', ';");
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
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
            tFile.AppendLine("$tInsertSQL.=')';");
            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tInsertSQL);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL"));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine("" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tReference);");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
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