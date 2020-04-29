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
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountHelper : NWDHelper<NWDAccount>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDAccount.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        const string ServerFakeAccount = "ACC-999-123456789-123456789" + K_ACCOUNT_TEMPORARY_SUFFIXE;
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            //tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + BasisHelper().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccount>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("function IPBanOk()");
            //tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            //tFile.AppendLine("return true;");
            //tFile.AppendLine("}");
            //tFile.AppendLine(NWD.K_CommentSeparator);

            //tFile.AppendLine("function IPBanAdd()");
            //tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            //tFile.AppendLine("}");
            //tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function TestTemporaryAccount($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("if (substr($sReference, -1) == '" + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("else if (substr($sReference, -1) == '" + NWDAccount.K_ACCOUNT_NEW_SUFFIXE + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function TestCreateAccount($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("if (substr($sReference, -1) == '" + NWDAccount.K_ACCOUNT_NEW_SUFFIXE + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function TestBanAccount($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("$rBan = false;");
            tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "`,`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Ban) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDAccount>(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1;';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC90));
            //tFile.AppendLine("error('ACC90',true, __FILE__, __FUNCTION__, __LINE__);");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("// if user is temporary user I must find the last letter equal to '" + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE + "'");
            tFile.AppendLine("if (TestTemporaryAccount($sReference))");
            tFile.AppendLine("{");
            tFile.AppendLine("// normal ... unknow user!");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("// strange… an unknow account but not temporary … it's not possible");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC92));
            //tFile.AppendLine("error('ACC92',true, __FILE__, __FUNCTION__, __LINE__);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_array())");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Ban) + "'] > 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("$rBan = true;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC99));
            //tFile.AppendLine("error('ACC99',true, __FILE__, __FUNCTION__, __LINE__);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this UUID … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC95));
            //tFile.AppendLine("error('ACC95',true, __FILE__, __FUNCTION__, __LINE__);");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rBan;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function FindSDKI($sSDKt, $sSDKv, $sSDKr, $sSDKl)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
            tFile.Append("$tQuerySign = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "`");
            tFile.Append(" FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "`");
            tFile.Append(" WHERE ");
            tFile.Append("(");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$SQL_CON->real_escape_string($sSDKv).'\\' ");
            tFile.Append(" AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append(")");
            tFile.Append(" OR ");
            tFile.Append("( `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` = \\''.$SQL_CON->real_escape_string($sSDKr).'\\' ");
            tFile.Append(" AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'\\' ");
            tFile.Append(" AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().RescueHash) + "` != \\'" + NWDAccountSign.K_NO_HASH + "\\' ");
            tFile.Append(")");
            tFile.Append(" OR ");
            tFile.Append("(");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().LoginHash) + "` = \\''.$SQL_CON->real_escape_string($sSDKl).'\\' ");
            tFile.Append(" AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().LoginHash) + "` != \\'\\' ");
            tFile.Append(" AND  `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().LoginHash) + "` != \\'" + NWDAccountSign.K_NO_HASH + "\\' ");
            tFile.Append(")");
            tFile.Append(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = " + NWD.K_SQL_CON + "->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN15));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("CreateAccount('" + ServerFakeAccount + "', $sSDKt, $sSDKv, $sSDKr, $sSDKl);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Need create an account sign valid!"));
            tFile.AppendLine("CreateAccountSign($uuid, $sSDKt, $sSDKv, $sSDKr, $sSDKl);");
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultSign->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN07));
            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
            tFile.AppendLine("// to much users ...");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKv : '.$sSDKv.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKr : '.$sSDKr.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN18));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultSign);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);






            tFile.AppendLine("function FindAccount($sReference, $sSDKI, $sCanCreate = true)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("$tReference = $sReference;");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("if (TestCreateAccount($sReference) == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("CreateAccount($tReference, " + NWDAccountSignType.DeviceID.ToLong() + ", $sSDKI, '-', '-');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.Append("$tQuerySign = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "`");
            tFile.Append(" FROM `" + NWDBasisHelper.TableNamePHP<NWDAccountSign>(sEnvironment) + "`");
            tFile.Append(" WHERE");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` = \\''.$SQL_CON->real_escape_string($sSDKI).'\\' ");
            tFile.Append(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignHash) + "` != \\'\\' ");
            tFile.Append(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().SignStatus) + "` = \\'" + ((int)NWDAccountSignAction.Associated).ToString() + "\\' ");
            tFile.Append(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = " + NWD.K_SQL_CON + "->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN15));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($sCanCreate == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("CreateAccount($tReference, " + NWDAccountSignType.DeviceID.ToLong() + ", $sSDKI, '-', '-');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("IPBanAdd();");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN16));
            tFile.AppendLine("}");
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN16));
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKI : '.$sSDKI.' NO Row"));
            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResultSign->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("while ($tRowSign = $tResultSign->fetch_array())");
            tFile.AppendLine("{");
            tFile.AppendLine("$tReference = $tRowSign['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountSign>().Account) + "'];");
            tFile.AppendLine("if(TestBanAccount($tReference) == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("// second authentification later ... here");
            tFile.AppendLine("respondUUID($tReference);");
            tFile.AppendLine("respond_ChangeUser($sReference, $tReference);");
            tFile.AppendLine("NWDRequestTokenReset($tReference);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else //or more than one user with this email … strange… I push an error, user must be unique");
            tFile.AppendLine("{");
            tFile.AppendLine("// to much users ...");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKI : '.$sSDKI.' Too Mush Row"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN18));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResultSign);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("return $tReference;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function CreateAccount($sOldUUID, $sSDKt, $sSDKv, $sSDKr, $sSDKl)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$sOldUUID = '.$sOldUUID.'"));
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
            tFile.AppendLine("global $shs, $ereg_token;");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $UserRange, $SQL_LIST;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("if (TestTemporaryAccount($sOldUUID))");
            tFile.AppendLine("{");

            tFile.AppendLine("// find the database with place");


            tFile.AppendLine("// loop sqlite");
            tFile.AppendLine("$tFindPlace = false;");
            tFile.AppendLine("foreach ($SQL_LIST as $tRange => $tValue)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine("// close actual msqli");
                tFile.AppendLine("mysqli_close(" + NWD.K_SQL_CON + ");");
                tFile.AppendLine("" + NWD.K_SQL_CON + " = new mysqli($tValue['host'], $tValue['user'], $tValue['password'], $tValue['database'], $tValue['port']);");
                tFile.AppendLine("if (" + NWD.K_SQL_CON + "->connect_errno)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SQL00));
                    tFile.AppendLine("include_once ('" + NWD.K_STATIC_FINISH_PHP + "');");
                    tFile.AppendLine("exit;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tRange.' connexion success on '.$tValue['title'].' => '.$tValue['id'].'"));
                }
                tFile.AppendLine("}");

                tFile.AppendLine("// have you place?");
                tFile.AppendLine("$tPlaceFree = $tValue['maxuser'];");
                tFile.AppendLine("$tQuery = 'SELECT COUNT(*) FROM " + NWDBasisHelper.BasisHelper<NWDAccount>().PHP_TABLENAME(sEnvironment) + ";';");
                tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
                tFile.AppendLine("if (!$tResult)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", NWD.K_SQL_CON));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("while($tRow = $tResult->fetch_array())");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$tPlaceFree = $tRow['COUNT(*)'];");
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, " place use in database '.$tQuery.' = >  '.$tPlaceFree.'"));
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");

                tFile.AppendLine("if ($tPlaceFree < $tValue['maxuser'])");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tFindPlace = true;");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, " find place in '.$tValue['title'].' => '.$tValue['id'].' with usermax = '.$tValue['maxuser'].'"));
                    tFile.AppendLine("// Yes you have place ... good!");
                    tFile.AppendLine("$UserRange = mt_rand($tValue['min'], $tValue['max']);// not use, just to test the possible");
                    tFile.AppendLine("break;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, " no place in '.$tValue['title'].' => '.$tValue['id'].' with usermax = '.$tValue['maxuser'].'"));
                }
                tFile.AppendLine("}");

            }
            tFile.AppendLine("}");

            tFile.AppendLine("if ($tFindPlace == false)");
            tFile.AppendLine("{");
            {
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_DISKFULL));
            }
            tFile.AppendLine("}");

            tFile.AppendLine("$tInternalKey = '';");
            tFile.AppendLine("$tInternalDescription = '';");
            tFile.AppendLine("$tNewUUID = referenceSecondGenerate ('" + NWDBasisHelper.BasisHelper<NWDAccount>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDAccount>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "');");
            tFile.AppendLine("$tInsertSQL='';");
            tFile.AppendLine("$tInsertSQLValue='';");
            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDAccount>(sEnvironment) + "` (';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($tNewUUID).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Dev account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Preprod account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            }
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Ban) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            //tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Bundle) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().UseInEnvironment) + "`, ';$tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
            tFile.AppendLine("$tInsertSQL.=')';");
            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tInsertSQL);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC91));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDBasisHelper.BasisHelper<NWDAccount>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tNewUUID);");
            tFile.AppendLine("if ($sOldUUID !='" + ServerFakeAccount + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("respond_UserTransfert($sOldUUID, $tNewUUID);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sSDKv : '.$sSDKv.' NO Row"));
            tFile.AppendLine("CreateAccountSign($tNewUUID, $sSDKt, $sSDKv, $sSDKr, $sSDKl);");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("respond_NewUser($sOldUUID, $tNewUUID);");
            tFile.AppendLine("}");
            tFile.AppendLine("respondUUID($tNewUUID);");
            tFile.AppendLine("NWDRequestTokenReset($tNewUUID);");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC97));
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            /*
            tFile.AppendLine("function GenerateNewAccount()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
            tFile.AppendLine("global $shs, $ereg_token;");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("$tInternalKey = '';");
            tFile.AppendLine("$tInternalDescription = '';");
            tFile.AppendLine("$tNewUUID = referenceSecondGenerate ('" + NWDBasisHelper.BasisHelper<NWDAccount>().ClassTrigramme + "', '" + NWDBasisHelper.TableNamePHP<NWDAccount>(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "');");
            tFile.AppendLine("$rReturn = $tNewUUID;");
            tFile.AppendLine("$tInsertSQL='';");
            tFile.AppendLine("$tInsertSQLValue='';");
            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDBasisHelper.TableNamePHP<NWDAccount>(sEnvironment) + "` (';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($tNewUUID).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Dev account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified'.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Preprod account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            }
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Ban) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            //tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Bundle) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().UseInEnvironment) + "`, ';$tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().WebModel) + "`, '; $tInsertSQLValue.= '\\'" + NWDBasisHelper.BasisHelper<NWDAccount>().LastWebBuild + "\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
            tFile.AppendLine("$tInsertSQL.=')';");
            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
            tFile.AppendLine("$tResult = "+ NWD.K_SQL_CON + "->query($tInsertSQL);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL", NWD.K_SQL_CON));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC91));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDBasisHelper.BasisHelper<NWDAccount>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tNewUUID);");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            */


            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif