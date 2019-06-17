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
using BasicToolBox;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountHelper : NWDHelper<NWDAccount>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDAccount.New_PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis<NWDAccount>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string New_PhpEngine(NWDAppEnvironment sEnvironment)
        {
            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + BasisHelper().ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function IPBanOk()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function IPBanAdd()");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function TestTemporaryAccount($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("if (substr($sReference, -1) == 'T')");
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
            tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference) + "`,`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Ban) + "` FROM `" + TableNamePHP(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\' AND `" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1;';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC90));
            //tFile.AppendLine("error('ACC90',true, __FILE__, __FUNCTION__, __LINE__);");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("// if user is temporary user I must find the last letter equal to 'T'");
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
            tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Ban) + "'] > 0)");
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


            tFile.AppendLine("function FindAccount($sReference, $sSDKI, $sCanCreate = true)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("$tReference = $sReference;");
            tFile.AppendLine("if (IPBanOk() == true)");
            tFile.AppendLine("{");
            tFile.Append("$tQuerySign = 'SELECT `" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().Account) + "` ");
            tFile.Append("FROM `" + NWDAccountSign.TableNamePHP(sEnvironment) + "` ");
            tFile.Append("WHERE `" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignHash) + "` = \\''.$SQL_CON->real_escape_string($sSDKI).'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignHash) + "` != \\'\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().SignStatus) + "` = \\'"+((int)NWDAccountSignAction.Associated).ToString()+"\\' ");
            tFile.Append("AND `" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().AC) + "` = 1;");
            tFile.AppendLine("';");
            tFile.AppendLine("$tResultSign = $SQL_CON->query($tQuerySign);");
            tFile.AppendLine("if (!$tResultSign)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySign"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN15));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSign->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($sCanCreate == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("CreateAccount($tReference, $sSDKI);");
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
            tFile.AppendLine("$tReference = $tRowSign['" + NWDToolbox.PropertyName(() => NWDAccountSign.FictiveData().Account) + "'];");
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
            tFile.AppendLine("return $tReference;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function CreateAccount($sOldUUID, $sSDKI)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("global $ACC_TMP, $TIME_SYNC, $ACC_NEED_USER_TRANSFERT;");
            tFile.AppendLine("global $shs, $ereg_token;");
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global $ENV, $WSBUILD;");
            tFile.AppendLine("global $SQL_NWDAccount_WebService;");
            tFile.AppendLine("if (TestTemporaryAccount($sOldUUID))");
            tFile.AppendLine("{");
            tFile.AppendLine("$tInternalKey = '';");
            tFile.AppendLine("$tInternalDescription = '';");
            tFile.AppendLine("$tNewUUID = referenceGenerate ('" + NWDAccount.BasisHelper().ClassTrigramme + "', '" + NWDAccount.TableNamePHP(sEnvironment) + "', '" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "');");
            tFile.AppendLine("$tInsertSQL='';");
            tFile.AppendLine("$tInsertSQLValue='';");
            tFile.AppendLine("$tInsertSQL.='INSERT INTO `" + NWDAccount.TableNamePHP(sEnvironment) + "` (';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string($tNewUUID).'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().ServerHash) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().ServerLog) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DM) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DS) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Dev account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DevSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().InternalKey) + "`, '; $tInsertSQLValue.= '\\'Anonymous Certified\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().InternalDescription) + "`, '; $tInsertSQLValue.= '\\''.$SQL_CON->real_escape_string('Preprod account').'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().PreprodSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().ProdSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
            }
            if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DevSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().PreprodSync) + "`, '; $tInsertSQLValue.= '\\'-1\\', ';");
                tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().ProdSync) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            }
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "`, '; $tInsertSQLValue.= '\\'1\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Ban) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DC) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().DD) + "`, '; $tInsertSQLValue.= '\\''.$TIME_SYNC.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().InError) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().CheckList) + "`, '; $tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Preview) + "`, '; $tInsertSQLValue.= '\\'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Tag) + "`, '; $tInsertSQLValue.= '\\'" + ((int)NWDBasisTag.TagServerCreated).ToString() + "\\', '; // " + NWDBasisTag.TagServerCreated.ToString() + "");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().UseInEnvironment) + "`, ';$tInsertSQLValue.= '\\'0\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().WebModel) + "`, '; $tInsertSQLValue.= '\\''.$SQL_" + NWDAccount.BasisHelper().ClassNamePHP + "_WebService.'\\', ';");
            tFile.AppendLine("$tInsertSQL.='`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().XX) + "` '; $tInsertSQLValue.= '\\'0\\'';");
            tFile.AppendLine("$tInsertSQL.=')';");
            tFile.AppendLine("$tInsertSQL.=' VALUES ('.$tInsertSQLValue.');';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tInsertSQL);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tInsertSQL"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC91));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDAccount.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() +"($tNewUUID);");
            tFile.AppendLine("respond_UserTransfert($sOldUUID, $tNewUUID);");
            tFile.AppendLine("respondUUID($tNewUUID);");
            //tFile.AppendLine("NWDRequestTokenIsValid($tNewUUID,'');");
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

            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif