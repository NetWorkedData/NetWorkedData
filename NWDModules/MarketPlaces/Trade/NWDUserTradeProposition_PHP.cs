//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:56
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using SQLite.Attribute;
using UnityEngine;
using BasicToolBox;
using System.Collections.Generic;
using System;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradePropositionHelper : NWDHelper<NWDUserTradeProposition>
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

            string tTradeHash = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeHash);
            string tTradeStatus = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().LimitDayTime);
            string tTradePlace = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradePlace);
            string tWinnerProposition = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().WinnerProposition);

            string t_THIS_TradeRequestHash = NWDToolbox.PropertyName(() => FictiveData().TradeRequestHash);
            string t_THIS_TradePlace = NWDToolbox.PropertyName(() => FictiveData().TradePlace);
            string t_THIS_TradeRequest = NWDToolbox.PropertyName(() => FictiveData().TradeRequest);
            string t_THIS_TradeStatus = NWDToolbox.PropertyName(() => FictiveData().TradeStatus);
            string t_THIS_ItemsProposed = NWDToolbox.PropertyName(() => FictiveData().ItemsProposed);
            string t_THIS_ItemsAsked = NWDToolbox.PropertyName(() => FictiveData().ItemsAsked);

            int t_THIS_Index_tTradeRequestHash =  CSV_IndexOf(t_THIS_TradeRequestHash);
            int t_THIS_Index_TradePlace =  CSV_IndexOf(t_THIS_TradePlace);
            int t_THIS_Index_TradeRequest =  CSV_IndexOf(t_THIS_TradeRequest);
            int t_THIS_Index_TradeStatus =  CSV_IndexOf(t_THIS_TradeStatus);
            int t_THIS_Index_ItemsProposed =  CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsAsked =  CSV_IndexOf(t_THIS_ItemsAsked);

            StringBuilder rReturn = new StringBuilder();

            rReturn.AppendLine("include_once ( " + NWDUserTradeRequest.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$tServerRequestHash = '';");
            rReturn.Append("$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeRequestHash + "` FROM `" + NWDUserTradeProposition.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\';';");
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
            rReturn.AppendLine("$tRowStatus = $tResultStatus->fetch_assoc();\n");
            rReturn.AppendLine("$tServerStatut = $tRowStatus['" + t_THIS_TradeStatus + "'];");
            rReturn.AppendLine("$tServerRequestHash = $tRowStatus['" + t_THIS_TradeRequestHash + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            // change the statut from CSV TO WAITING, ACCEPTED, CANCEL, EXPIRED
            rReturn.AppendLine("if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            // change the statut from CSV TO NONE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && ");
            rReturn.AppendLine("($tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString());
            rReturn.AppendLine(" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_ItemsAsked + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_tTradeRequestHash + "]='';");
            rReturn.AppendLine("$sReplaces[" + t_THIS_Index_TradeRequest + "]='';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            // change the statut from CSV TO ACTIVE 
            rReturn.AppendLine("else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryTrade = 'UPDATE `" + NWDUserTradeRequest.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append(" `" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',");
            rReturn.Append(" `" + tWinnerProposition + "` = \\''.$sCsvList[0].'\\',");
            rReturn.Append(" `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\'");
            rReturn.Append(" WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append(" AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append(" AND `" + tTradePlace + "` = \\''.$sCsvList[" + t_THIS_Index_TradePlace + "].'\\' ");
            rReturn.Append(" AND `" + tReference + "` = \\''.$sCsvList[" + t_THIS_Index_TradeRequest + "].'\\' ");
            rReturn.Append(" AND `" + tTradeHash + "` = \\''.$sCsvList[" + t_THIS_Index_tTradeRequestHash + "].'\\' ");
            rReturn.Append(" AND `" + tLimitDayTime + "` > '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.AppendLine("';");
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
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + t_THIS_Index_TradeStatus + ", \'" + ((int)NWDTradeStatus.Accepted).ToString() + "\');");
            rReturn.AppendLine(NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sCsvList[" + t_THIS_Index_TradeRequest + "]);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + t_THIS_Index_TradeStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');");
            rReturn.AppendLine("}");
            rReturn.AppendLine(NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sCsvList[" + t_THIS_Index_TradeRequest + "]);");
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
        public override string AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            string t_THIS_TradeRequest = NWDToolbox.PropertyName(() => FictiveData().TradeRequest);
            int t_THIS_Index_TradeRequest =  CSV_IndexOf(t_THIS_TradeRequest);
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("// write your php script here to update after sync on server");
            rReturn.AppendLine(NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sCsvList[" + t_THIS_Index_TradeRequest + "]);");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("$REP['" + ClassName + " Special'] ='success!!!';");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif