//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Text;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeRequestHelper : NWDHelper<NWDUserTradeRequest>
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

            string t_THIS_TradeStatus = NWDToolbox.PropertyName(() => FictiveData().TradeStatus);
            string t_THIS_TradeHash = NWDToolbox.PropertyName(() => FictiveData().TradeHash);
            string t_THIS_WinnerProposition = NWDToolbox.PropertyName(() => FictiveData().WinnerProposition);
            string t_THIS_ItemsProposed = NWDToolbox.PropertyName(() => FictiveData().ItemsProposed);
            string t_THIS_ItemsAsked = NWDToolbox.PropertyName(() => FictiveData().ItemsAsked);

            int t_THIS_Index_TradeStatus =  CSV_IndexOf(t_THIS_TradeStatus);
            int t_THIS_Index_TradeHash =  CSV_IndexOf(t_THIS_TradeHash);
            int t_THIS_Index_WinnerProposition =  CSV_IndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_ItemsProposed =  CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsAsked =  CSV_IndexOf(t_THIS_ItemsAsked);

            StringBuilder rReturn = new StringBuilder();

            // get the actual state
            rReturn.AppendLine("$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$tServerHash = '';");
            rReturn.Append("$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeHash + "` FROM `" + NWDUserTradeRequest.TableNamePHP(sEnvironment) + "` ");
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
            rReturn.AppendLine("$tServerStatut = $tRowStatus['" + t_THIS_TradeStatus + "'];");
            rReturn.AppendLine("$tServerHash = $tRowStatus['" + t_THIS_TradeHash + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED
            rReturn.AppendLine("if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO ACTIVE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_TradeHash + "] = " + NWD.K_PHP_TIME_SYNC + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_TradeStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // change the statut from CSV TO NONE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_TradeHash + "] = " + NWD.K_PHP_TIME_SYNC + ";");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsAsked + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // change the statut from CSV TO CANCEL 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryCancelable = 'UPDATE `" + NWDUserTradeRequest.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
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
            rReturn.AppendLine(PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($tReference);");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("//stop the function!");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // change the statut from CSV TO FORCE // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//EXECEPTION FOR ADMIN");
            rReturn.AppendLine("}");
            // OTHER
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);\n");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif