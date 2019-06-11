// =====================================================================================================================
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
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeFinderHelper : NWDHelper<NWDUserTradeFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
       public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tTradeStatus = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().LimitDayTime);
            string tTradePlaceRequest = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradePlace);
            string tForRelationshipOnly = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().ForRelationshipOnly);
            string tRelationshipAccountReferences = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().RelationshipAccountReferences);

            string t_THIS_TradeRequestsList = NWDToolbox.PropertyName(() => FictiveData().TradeRequestsList);
            string t_THIS_TradePlace = NWDToolbox.PropertyName(() => FictiveData().TradePlace);
            string t_THIS_ForRelationshipOnly = NWDToolbox.PropertyName(() => FictiveData().ForRelationshipOnly);

            //string tTradeStatus = NWDUserTradeRequest.FindAliasName("TradeStatus");
            //string tLimitDayTime = NWDUserTradeRequest.FindAliasName("LimitDayTime");
            //string tTradePlaceRequest = NWDUserTradeRequest.FindAliasName("TradePlace");
            //string tForRelationshipOnly = NWDUserTradeRequest.FindAliasName("ForRelationshipOnly");
            //string tRelationshipAccountReferences = NWDUserTradeRequest.FindAliasName("RelationshipAccountReferences");

            //string t_THIS_TradeRequestsList = FindAliasName("TradeRequestsList");
            //string t_THIS_TradePlace = FindAliasName("TradePlace");
            //string t_THIS_ForRelationshipOnly = FindAliasName("ForRelationshipOnly");

            int tIndex_tTradeStatus = NWDUserTradeRequest.CSV_IndexOf(tTradeStatus);
            int tIndex_TradeRequestsList = New_CSV_IndexOf(t_THIS_TradeRequestsList);
            int tIndex_TradePlace = New_CSV_IndexOf(t_THIS_TradePlace);
            int tIndex_THIS_ForRelationshipOnly = New_CSV_IndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 5; // minutes before stop to get the datas!
            string sScript = "" +
                "// start Addon \n" +
                "include_once("+NWD.K_PATH_BASE+".'/'."+NWD.K_ENV+".'/" + NWD.K_DB + "/" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "$tQueryExpired = 'SELECT " + NWDUserTradeRequest.SLQSelect() + " FROM `'."+NWD.K_ENV+".'_" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tLimitDayTime + "` < '."+NWD.K_PHP_TIME_SYNC+".' " +
                "AND `WebModel` <= '.$WSBUILD.' " +
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryExpired : '. $tQueryExpired, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);\n" +
                "if (!$tResultExpired)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRFx31',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "while($tRowExpired = $tResultExpired->fetch_row())\n" +
                "{\n" +
                "myLog('tReferences need be cancelled : '. $tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tRowExpired = Integrity" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + "Replace ($tRowExpired," + tIndex_tTradeStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");\n" +
                "$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);\n" +
                "UpdateData" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + " ($tRowExpired, "+NWD.K_PHP_TIME_SYNC+", $uuid, false);\n" +
                "}\n" +
                //"mysqli_free_result($tResultExpired);\n" +
                "}\n" +

                "$tQueryTrade = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + "` " +
                // WHERE REQUEST
                "WHERE `AC`= \\'1\\' " +
                "AND `Account` != \\''."+NWD.K_SQL_CON+"->real_escape_string($uuid).'\\' " +
                "AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' ';\n" +
                "if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')\n" +
                "{\n" +
                "$tQueryTrade.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';\n" +
                "}\n" +
                "$tQueryTrade.= '" +
                "AND `" + tTradePlaceRequest + "` = \\''.$sCsvList[" + tIndex_TradePlace + "].'\\' " +
                "AND `" + tLimitDayTime + "` > '.("+NWD.K_PHP_TIME_SYNC+"+" + (tDelayOfRefresh * 60).ToString() + ").' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                // END WHERE REQUEST LIMIT START
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryTrade : '. $tQueryTrade, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultTrade = "+NWD.K_SQL_CON+"->query($tQueryTrade);\n" +
                "$tReferences = \'\';\n" +
                "$tReferencesList = \'\';\n" +
                "if (!$tResultTrade)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryTrade.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRFx31',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "while($tRowTrade = $tResultTrade->fetch_assoc())\n" +
                "{\n" +
                "myLog('tReferences found : '. $tRowTrade['Reference'], __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tReferences[]=$tRowTrade['Reference'];\n" +
                "}\n" +
                //"mysqli_free_result($tRowTrade);\n" +
                "if (is_array($tReferences))\n" +
                "{\n" +
                "$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);\n" +
                "GetDatas" + NWDUserTradeRequest.BasisHelper().ClassNamePHP + "ByReferences ($tReferences);\n" +
                "}\n" +
                "}\n" +
                "myLog('tReferencesList : '. $tReferencesList, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$sCsvList = Integrity" + ClassNamePHP + "Replace ($sCsvList, " + tIndex_TradeRequestsList.ToString() + ", $tReferencesList);\n" +
                "// finish Addon \n";

            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif