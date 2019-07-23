//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:50
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeFinderHelper : NWDHelper<NWDUserTradeFinder>
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

            string tTradeStatus = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().LimitDayTime);
            string tTradePlaceRequest = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradePlace);
            string tForRelationshipOnly = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().ForRelationshipOnly);
            string tRelationshipAccountReferences = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().RelationshipAccountReferences);

            string t_THIS_TradeRequestsList = NWDToolbox.PropertyName(() => FictiveData().TradeRequestsList);
            string t_THIS_TradePlace = NWDToolbox.PropertyName(() => FictiveData().TradePlace);
            string t_THIS_ForRelationshipOnly = NWDToolbox.PropertyName(() => FictiveData().ForRelationshipOnly);

            int tIndex_tTradeStatus = NWDUserTradeRequest.CSV_IndexOf(tTradeStatus);
            int tIndex_TradeRequestsList =  CSV_IndexOf(t_THIS_TradeRequestsList);
            int tIndex_TradePlace =  CSV_IndexOf(t_THIS_TradePlace);
            int tIndex_THIS_ForRelationshipOnly =  CSV_IndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 5; // minutes before stop to get the datas!

            StringBuilder rReturn = new StringBuilder();

            rReturn.AppendLine("include_once(" + NWDUserTradeRequest.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.Append("$tQueryExpired = 'SELECT " + NWDUserTradeRequest.SLQSelect() + " FROM `" + NWDUserTradeRequest.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append("AND `" + tLimitDayTime + "` < '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine("LIMIT 0, 100;';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowExpired = $tResultExpired->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowExpired = " + NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_INTERGRITY_REPLACE() + " ($tRowExpired," + tIndex_tTradeStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");");
            rReturn.AppendLine("$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);");
            rReturn.AppendLine(NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_UPDATE_DATA() + " ($tRowExpired, " + NWD.K_PHP_TIME_SYNC + ", $uuid, false);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.Append("$tQueryTrade = 'SELECT `" + tReference + "` FROM `" + NWDUserTradeRequest.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tAccount + "` != \\''." + NWD.K_SQL_CON + "->real_escape_string($uuid).'\\' ");
            rReturn.Append("AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.AppendLine("AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' ';");
            rReturn.AppendLine("if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tQueryTrade.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';");
            rReturn.AppendLine("}");
            rReturn.Append("$tQueryTrade.= '");
            rReturn.Append("AND `" + tTradePlaceRequest + "` = \\''.$sCsvList[" + tIndex_TradePlace + "].'\\' ");
            rReturn.AppendLine("AND `" + tLimitDayTime + "` > '.(" + NWD.K_PHP_TIME_SYNC + "+" + (tDelayOfRefresh * 60).ToString() + ").' ");
            // END WHERE REQUEST LIMIT START
            rReturn.AppendLine("LIMIT 0, 100;';");
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
            rReturn.AppendLine("while($tRowTrade = $tResultTrade->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tReferences[]=$tRowTrade['" + tReference + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("if (is_array($tReferences))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);");
            rReturn.AppendLine(NWDUserTradeRequest.BasisHelper().PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ($tReferences);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + tIndex_TradeRequestsList.ToString() + ", $tReferencesList);");

            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif