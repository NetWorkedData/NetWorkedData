//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date        2019-4-12 18:49:11
//  Author      Kortex (Jean-François CONTART) 
//  Email       jfcontart@idemobi.com
//  Project     NetWorkedData for Unity3D
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
    public partial class NWDUserBarterRequestHelper : NWDHelper<NWDUserBarterRequest>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tWebModel = NWDToolbox.PropertyName(() => FictiveData().WebModel);
            string tAC = NWDToolbox.PropertyName(() => FictiveData().AC);
            string tDM = NWDToolbox.PropertyName(() => FictiveData().DM);
            string tDS = NWDToolbox.PropertyName(() => FictiveData().DS);
            string tReference = NWDToolbox.PropertyName(() => FictiveData().Reference);
            string tAccount = NWDToolbox.PropertyName(() => FictiveData().Account);
            string tEnvSync = PHP_ENV_SYNC(sEnvironment);

            string tBarterStatus = NWDToolbox.PropertyName(() => NWDUserBarterProposition.FictiveData().BarterStatus);
            string tBarterRequest = NWDToolbox.PropertyName(() => NWDUserBarterProposition.FictiveData().BarterRequest);
            string tBarterRequestHash = NWDToolbox.PropertyName(() => NWDUserBarterProposition.FictiveData().BarterRequestHash);
            string tItemsSend = NWDToolbox.PropertyName(() => NWDUserBarterProposition.FictiveData().ItemsSend);

            string tMaxRequestPerUser = NWDToolbox.PropertyName(() => NWDBarterPlace.FictiveData().MaxRequestPerUser);
            string tMaxPropositionsPerUser = NWDToolbox.PropertyName(() => NWDBarterPlace.FictiveData().MaxPropositionsPerUser);
            string tMaxPropositionsPerRequest = NWDToolbox.PropertyName(() => NWDBarterPlace.FictiveData().MaxPropositionsPerRequest);
            string tRequestLifeTime = NWDToolbox.PropertyName(() => NWDBarterPlace.FictiveData().RequestLifeTime);

            string t_THIS_WinnerProposition = NWDToolbox.PropertyName(() => FictiveData().WinnerProposition);
            string t_THIS_Propositions = NWDToolbox.PropertyName(() => FictiveData().Propositions);
            string t_THIS_PropositionsCounter = NWDToolbox.PropertyName(() => FictiveData().PropositionsCounter);
            string t_THIS_MaxPropositions = NWDToolbox.PropertyName(() => FictiveData().MaxPropositions);
            string t_THIS_BarterStatus = NWDToolbox.PropertyName(() => FictiveData().BarterStatus);
            string t_THIS_BarterHash = NWDToolbox.PropertyName(() => FictiveData().BarterHash);
            string t_THIS_BarterPlace = NWDToolbox.PropertyName(() => FictiveData().BarterPlace);
            string t_THIS_LimitDayTime = NWDToolbox.PropertyName(() => FictiveData().LimitDayTime);
            string t_THIS_ItemsReceived = NWDToolbox.PropertyName(() => FictiveData().ItemsReceived);
            string t_THIS_ItemsSuggested = NWDToolbox.PropertyName(() => FictiveData().ItemsSuggested);
            string t_THIS_ItemsProposed = NWDToolbox.PropertyName(() => FictiveData().ItemsProposed);

            int t_THIS_Index_WinnerProposition =  CSV_IndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_Propositions =  CSV_IndexOf(t_THIS_Propositions);
            int t_THIS_Index_PropositionsCounter =  CSV_IndexOf(t_THIS_PropositionsCounter);
            int t_THIS_Index_MaxPropositions =  CSV_IndexOf(t_THIS_MaxPropositions);
            int t_THIS_Index_BarterStatus =  CSV_IndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_BarterHash =  CSV_IndexOf(t_THIS_BarterHash);
            int t_THIS_Index_BarterPlace =  CSV_IndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_LimitDayTime =  CSV_IndexOf(t_THIS_LimitDayTime);

            int t_THIS_Index_ItemsProposed =  CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsSuggested =  CSV_IndexOf(t_THIS_ItemsSuggested);
            int t_THIS_Index_ItemsReceived =  CSV_IndexOf(t_THIS_ItemsReceived);

            StringBuilder rReturn = new StringBuilder();

            rReturn.AppendLine("//start Addon");
            rReturn.AppendLine("include_once ( " + NWDUserBarterProposition.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$tServerHash = '';");
            rReturn.AppendLine("$tServerPropositions = '';");
            rReturn.Append("$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterHash + "`, `" + t_THIS_Propositions + "` FROM `" + PHP_TABLENAME(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.AppendLine("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\';';");
            rReturn.AppendLine("$tResultStatus = " + NWD.K_SQL_CON + "->query($tQueryStatus);");
            rReturn.AppendLine("if (!$tResultStatus)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryStatus"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultStatus->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowStatus = $tResultStatus->fetch_assoc();");
            rReturn.AppendLine("$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];");
            rReturn.AppendLine("$tServerHash = $tRowStatus['" + t_THIS_BarterHash + "'];");
            rReturn.AppendLine("$tServerPropositions = $tRowStatus['" + t_THIS_Propositions + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(")");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO ACTIVE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryBarterPlace = 'SELECT");
            rReturn.Append(" `" + tMaxPropositionsPerRequest + "`,");
            rReturn.Append(" `" + tRequestLifeTime + "`");
            rReturn.Append(" FROM `" + NWDBarterPlace.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append(" WHERE");
            rReturn.AppendLine(" `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_THIS_Index_BarterPlace + "]).'\\';';");
            rReturn.AppendLine("$tResultBarterPlace = " + NWD.K_SQL_CON + "->query($tQueryBarterPlace);");
            rReturn.AppendLine("if (!$tResultBarterPlace)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryBarterPlace"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultBarterPlace->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowBarterPlace = $tResultBarterPlace->fetch_assoc();");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_LimitDayTime + "] = " + NWD.K_PHP_TIME_SYNC + " + $tRowBarterPlace['" + tRequestLifeTime + "'];");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_MaxPropositions + "]= $tRowBarterPlace['" + tMaxPropositionsPerRequest + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterHash + "] = " + NWD.K_PHP_TIME_SYNC + ".RandomString();");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_Propositions + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // change the statut from CSV TO NONE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            //" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
            //" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
            rReturn.AppendLine("))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterHash + "] = " + NWD.K_PHP_TIME_SYNC + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_Propositions + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // change the statut from CSV TO CANCEL 
            rReturn.AppendLine("else if (($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " ||");
            rReturn.AppendLine(" $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() + ") && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryCancelable = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultCancelable = " + NWD.K_SQL_CON + "->query($tQueryCancelable);");
            rReturn.AppendLine("if (!$tResultCancelable)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryCancelable"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            // START CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I need to put all propositions in Expired");
            rReturn.Append("$tQueryExpired = 'UPDATE `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.Append("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryExpired = 'SELECT `" + tReference + "` FROM `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while ($tRowExpired = $tResultExpired->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDUserBarterProposition.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tRowExpired[0]);\n");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // FINISH CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I can integrate data to expired!");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tReference);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("//stop the function!");
            //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO CANCEL 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryCancelable = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append("AND `" + t_THIS_LimitDayTime + "` < \\''." + NWD.K_PHP_TIME_SYNC + ".'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultCancelable = " + NWD.K_SQL_CON + "->query($tQueryCancelable);");
            rReturn.AppendLine("if (!$tResultCancelable)\n");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryCancelable"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            // START CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I need to put all propositions in Expired");
            rReturn.Append("$tQueryExpired = 'UPDATE `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryExpired = 'SELECT `" + tReference + "` FROM `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while ($tRowExpired = $tResultExpired->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + NWDUserBarterProposition.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tRowExpired[0]);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // FINISH CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I can integrate data to expired!");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tReference);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("//stop the function!");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO DEAL 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryDeal = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_WinnerProposition + "` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("// I need to put winner propositions to Accepted Or it's reject?");
            //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            rReturn.AppendLine("$tResultDeal = " + NWD.K_SQL_CON + "->query($tQueryDeal);");
            rReturn.AppendLine("if (!$tResultDeal)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryDeal"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// I need to put Accepted or expired in this request?");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// I need to put all propositions in Expired");
            rReturn.Append("$tQueryAccepted = 'UPDATE `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\'");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultAccepted = " + NWD.K_SQL_CON + "->query($tQueryAccepted);");
            rReturn.AppendLine("if (!$tResultAccepted)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryAccepted"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("// I need to put Accepted or expired in this request?");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tItemsSend = '';");
            rReturn.Append("$tQueryBarterProposition = 'SELECT");
            rReturn.Append(" `" + tItemsSend + "`");
            rReturn.Append(" FROM `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append(" WHERE");
            rReturn.Append(" `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_THIS_Index_WinnerProposition + "]).'\\';");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultBarterProposition = " + NWD.K_SQL_CON + "->query($tQueryBarterProposition);");
            rReturn.AppendLine("if (!$tResultBarterProposition)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryBarterProposition"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultBarterProposition->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowBarterProposition = $tResultBarterProposition->fetch_assoc();");
            rReturn.AppendLine("$tItemsSend = $tRowBarterProposition['" + tItemsSend + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.Append("$tQueryAcceptedDeal = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_ItemsReceived + "` = \\''.$tItemsSend.'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'");
            rReturn.Append("AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''.$tReference.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultAcceptedDeal = " + NWD.K_SQL_CON + "->query($tQueryAcceptedDeal);");
            rReturn.AppendLine("if (!$tResultAcceptedDeal)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryAcceptedDeal"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("" + NWDUserBarterProposition.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sCsvList[" + t_THIS_Index_WinnerProposition + "]);\n");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryExpiredDeal = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'");
            rReturn.Append("AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''.$tReference.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpiredDeal = " + NWD.K_SQL_CON + "->query($tQueryExpiredDeal);");
            rReturn.AppendLine("if (!$tResultExpiredDeal)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpiredDeal"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // START CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I need to put all propositions in Expired\n");
            rReturn.Append("$tQueryExpired = 'UPDATE `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR ");
            rReturn.Append("`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryExpired = 'SELECT `Reference` FROM `" + NWDUserBarterProposition.BasisHelper().PHP_TABLENAME(sEnvironment) + "`");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tBarterRequest + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)\n");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while ($tRowExpired = $tResultExpired->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDUserBarterProposition.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tRowExpired[0]);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // FINISH CANCEL PUT PROPOSITION TO EXPIRED
            rReturn.AppendLine("// I can integrate data to expired!");
            rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tReference);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("//stop the function!");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO FORCE // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//EXECEPTION FOR ADMIN");
            rReturn.AppendLine("}");
            // change the statut from CSV TO FORCE // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.ForceNone).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterStatus + "] = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_BarterHash + "] = " + NWD.K_PHP_TIME_SYNC + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_Propositions + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';");
            rReturn.AppendLine("$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // OTHER
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("// finish Addon");

            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif