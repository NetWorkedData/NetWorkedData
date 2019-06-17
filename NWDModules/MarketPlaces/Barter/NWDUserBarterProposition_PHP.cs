//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================


#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using SQLite.Attribute;
using UnityEditor;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterPropositionHelper : NWDHelper<NWDUserBarterProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tWebModel = NWDToolbox.PropertyName(() => FictiveData().WebModel);
            string tAC = NWDToolbox.PropertyName(() => FictiveData().AC);
            string tDM = NWDToolbox.PropertyName(() => FictiveData().DM);
            string tDS = NWDToolbox.PropertyName(() => FictiveData().DS);
            string tReference = NWDToolbox.PropertyName(() => FictiveData().Reference);
            string tAccount = NWDToolbox.PropertyName(() => FictiveData().Account);
            string tEnvSync = PHP_ENV_SYNC(sEnvironment);

            string tBarterStatus = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().LimitDayTime);
            string tBarterPlace = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterPlace);
            string tWinnerProposition = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().WinnerProposition);
            string tPropositions = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().Propositions);
            string tMaxPropositions = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().MaxPropositions);
            string tPropositionsCounter = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().PropositionsCounter);
            string tBarterHash = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterHash);
            string tItemsProposed = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().ItemsProposed);

            string t_THIS_BarterRequestHash = NWDToolbox.PropertyName(() => FictiveData().BarterRequestHash);
            string t_THIS_BarterPlace = NWDToolbox.PropertyName(() => FictiveData().BarterPlace);
            string t_THIS_BarterRequest = NWDToolbox.PropertyName(() => FictiveData().BarterRequest);
            string t_THIS_BarterStatus = NWDToolbox.PropertyName(() => FictiveData().BarterStatus);

            string t_THIS_ItemsProposed = NWDToolbox.PropertyName(() => FictiveData().ItemsProposed);
            string t_THIS_ItemsSend = NWDToolbox.PropertyName(() => FictiveData().ItemsSend);

            int t_THIS_Index_BarterRequestHash = New_CSV_IndexOf(t_THIS_BarterRequestHash);
            int t_THIS_Index_BarterPlace = New_CSV_IndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_BarterRequest = New_CSV_IndexOf(t_THIS_BarterRequest);
            int t_THIS_Index_BarterStatus = New_CSV_IndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_ItemsProposed = New_CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsSend = New_CSV_IndexOf(t_THIS_ItemsSend);

            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// debut find ");
            rReturn.AppendLine("include_once ( " + NWDUserBarterRequest.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$tServerHash = '';");
            rReturn.Append("$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterRequestHash + "` FROM `" + PHP_TABLENAME(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.AppendLine("`Reference` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\';';");
            rReturn.AppendLine("$tResultStatus = " + NWD.K_SQL_CON + "->query($tQueryStatus);");
            rReturn.AppendLine("if (!$tResultStatus)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultStatus->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowStatus = $tResultStatus->fetch_assoc();");
            rReturn.AppendLine("$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];");
            rReturn.AppendLine("$tServerHash = $tRowStatus['" + t_THIS_BarterRequestHash + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");

            // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, DEAL, REFRESH, CANCELLED
            rReturn.AppendLine("if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + "($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");

            // change the statut from CSV TO NONE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && ");
            rReturn.AppendLine("($tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsSend + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterRequestHash + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterRequest + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            //"myLog('PUT TO NONE FROM EXPIRED OR ACCEPTED', __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine("}");

            // change the statut from CSV TO ACTIVE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryTrade = 'UPDATE `"  + NWDUserBarterRequest.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append(" `" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tPropositions + "` = TRIM(\\'" + NWDConstants.kFieldSeparatorA + "\\' FROM CONCAT(CONCAT(`" + tPropositions + "`,\\'" + NWDConstants.kFieldSeparatorA + "\\'),\\''.$sCsvList[0].'\\')), ");
            rReturn.Append(" `" + tPropositionsCounter + "` = `" + tPropositionsCounter + "`+1 ");
            rReturn.Append(" WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append(" AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append(" AND `" + tBarterPlace + "` = \\''.$sCsvList[" + t_THIS_Index_BarterPlace + "].'\\' ");
            rReturn.Append(" AND `" + tReference + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' ");
            rReturn.Append(" AND `" + tBarterHash + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequestHash + "].'\\' ");
            rReturn.Append(" AND `" + tLimitDayTime + "` > '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append(" AND `" + tPropositionsCounter + "` < `" + tMaxPropositions + "` ");
            rReturn.Append("';");

            rReturn.AppendLine("$tResultTrade = " + NWD.K_SQL_CON + "->query($tQueryTrade);");
            rReturn.AppendLine("$tReferences = \'\';");
            rReturn.AppendLine("$tReferencesList = \'\';");
            rReturn.AppendLine("if (!$tResultTrade)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryTrade"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// I need update the proposition too !");
            rReturn.Append("$tQueryBarterRequest = 'SELECT");
            rReturn.Append(" `" + tItemsProposed + "`");
            rReturn.Append(" FROM `" + NWDUserBarterRequest.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append(" WHERE");
            rReturn.Append(" `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_THIS_Index_BarterRequest + "]).'\\';");
            rReturn.Append("';");
            rReturn.AppendLine("$tResultBarterRequest = " + NWD.K_SQL_CON + "->query($tQueryBarterRequest);");
            rReturn.AppendLine("if (!$tResultBarterRequest)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryBarterRequest"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultBarterRequest->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowBarterRequest = $tResultBarterRequest->fetch_assoc();");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "] = $tRowBarterRequest['" + tItemsProposed + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sCsvList[" + t_THIS_Index_BarterRequest + "]);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sCsvList[" + t_THIS_Index_BarterRequest + "]);");
            rReturn.AppendLine("}");

            // change the statut from CSV TO NODEAL 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryTrade = 'UPDATE `" + NWDUserBarterRequest.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append(" `" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tPropositions + "` = TRIM(\\'" + NWDConstants.kFieldSeparatorA + "\\' FROM CONCAT(CONCAT(`" + tPropositions + "`,\\'" + NWDConstants.kFieldSeparatorA + "\\'),\\''.$sCsvList[0].'\\')), ");
            rReturn.Append(" `" + tPropositionsCounter + "` = `" + tPropositionsCounter + "`+1 ");
            rReturn.Append(" WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append(" AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append(" AND `" + tBarterPlace + "` = \\''.$sCsvList[" + t_THIS_Index_BarterPlace + "].'\\' ");
            rReturn.Append(" AND `" + tReference + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' ");
            rReturn.Append(" AND `" + tBarterHash + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequestHash + "].'\\' ");
            rReturn.Append(" AND `" + tLimitDayTime + "` > '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append(" AND `" + tPropositionsCounter + "` < `" + tMaxPropositions + "` ");
            rReturn.Append("';");
            rReturn.AppendLine("$tResultTrade = " + NWD.K_SQL_CON + "->query($tQueryTrade);");
            rReturn.AppendLine("$tReferences = \'\';");
            rReturn.AppendLine("$tReferencesList = \'\';");
            rReturn.AppendLine("if (!$tResultTrade)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryTrade"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// I need update the proposition too !");
            rReturn.Append("$tQueryBarterRequest = 'SELECT");
            rReturn.Append(" `" + tPropositionsCounter + "`, `" + tMaxPropositions + "`,`" + tItemsProposed + "`");
            rReturn.Append(" FROM `" + NWDUserBarterRequest.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append(" WHERE");
            rReturn.Append(" `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_THIS_Index_BarterRequest + "]).'\\';");
            rReturn.Append("';");
            rReturn.AppendLine("$tResultBarterRequest = " + NWD.K_SQL_CON + "->query($tQueryBarterRequest);");
            rReturn.AppendLine("if (!$tResultBarterRequest)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryBarterRequest"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultBarterRequest->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowBarterRequest = $tResultBarterRequest->fetch_assoc();");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "] = $tRowBarterRequest['" + tItemsProposed + "'];");
            rReturn.AppendLine("");
            rReturn.AppendLine("");
            rReturn.AppendLine("");
            rReturn.AppendLine("// TODO update if barter proposition == 1 to expired");
            rReturn.AppendLine("if ($tRowBarterRequest['" + tMaxPropositions + "'] == 1)\n");
            rReturn.AppendLine("{\n");
            rReturn.Append("$tQueryExpired = 'UPDATE `" + NWDUserBarterRequest.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append(" `" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tLimitDayTime + "` = '." + NWD.K_PHP_TIME_SYNC + ".', ");
            rReturn.Append(" `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append(" WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append(" AND `" + tReference + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);\n");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sCsvList[" + t_THIS_Index_BarterRequest + "]);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sCsvList[" + t_THIS_Index_BarterRequest + "]);");
            rReturn.AppendLine("}");

            // change the statut from CSV TO CANCEL 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryCancelable = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append("';");
            rReturn.AppendLine("$tResultCancelable = " + NWD.K_SQL_CON + "->query($tQueryCancelable);");
            rReturn.AppendLine("if (!$tResultCancelable)");
            rReturn.AppendLine("{");
            //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tReference);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");


            // change the statut from CSV TO FORCE // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//EXECEPTION FOR ADMIN");
            rReturn.AppendLine("}");

            // change the statut from CSV TO FORCE NONE  // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.ForceNone).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsSend + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterRequestHash + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterRequest + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");

            // OTHER
            rReturn.AppendLine("else\n");
            rReturn.AppendLine("{");
            //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            //"myLog('FINSIH ADD ON ... UPDATE FROM CSV', __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine("// finish Addon ");

            return rReturn.ToString();
        }
        //------------------------------------------------------------------------------------------------------------- 
        public override string New_AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            string t_THIS_BarterRequest = NWDToolbox.PropertyName(() => FictiveData().BarterRequest);
            int t_THIS_Index_BarterRequest = New_CSV_IndexOf(t_THIS_BarterRequest);

            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// write your php script here to update after sync on server ");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sCsvList[" + t_THIS_Index_BarterRequest + "]);");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif