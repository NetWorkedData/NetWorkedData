//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserGuild : NWDBasis<NWDUserGuild>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tGuildStatus = NWDUserGuildSubcription.FindAliasName("GuildStatus");
            string tGuildRequest = NWDUserGuildSubcription.FindAliasName("GuildRequest");
            string tGuildRequestHash = NWDUserGuildSubcription.FindAliasName("GuildRequestHash");
            string tItemsSend = NWDUserGuildSubcription.FindAliasName("ItemsSend");

            string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            string t_THIS_Propositions = FindAliasName("Propositions");
            string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");
            string t_THIS_MaxPropositions = FindAliasName("MaxPropositions");
            string t_THIS_GuildStatus = FindAliasName("GuildStatus");
            string t_THIS_GuildHash = FindAliasName("GuildHash");
            string t_THIS_GuildPlace = FindAliasName("GuildPlace");
            string t_THIS_LimitDayTime = FindAliasName("LimitDayTime");

            int t_THIS_Index_WinnerProposition = CSV_IndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_Propositions = CSV_IndexOf(t_THIS_Propositions);
            int t_THIS_Index_PropositionsCounter = CSV_IndexOf(t_THIS_PropositionsCounter);
            int t_THIS_Index_MaxPropositions = CSV_IndexOf(t_THIS_MaxPropositions);
            int t_THIS_Index_GuildStatus = CSV_IndexOf(t_THIS_GuildStatus);
            int t_THIS_Index_GuildHash = CSV_IndexOf(t_THIS_GuildHash);
            int t_THIS_Index_GuildPlace = CSV_IndexOf(t_THIS_GuildPlace);
            int t_THIS_Index_LimitDayTime = CSV_IndexOf(t_THIS_LimitDayTime);

            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            int t_THIS_Index_ItemsProposed = CSV_IndexOf(t_THIS_ItemsProposed);
            string t_THIS_ItemsSuggested = FindAliasName("ItemsSuggested");
            int t_THIS_Index_ItemsSuggested = CSV_IndexOf(t_THIS_ItemsSuggested);
            string t_THIS_ItemsReceived = FindAliasName("ItemsReceived");
            int t_THIS_Index_ItemsReceived = CSV_IndexOf(t_THIS_ItemsReceived);



            string tMaxRequestPerUser = NWDGuildPlace.FindAliasName("MaxRequestPerUser");
            string tMaxPropositionsPerUser = NWDGuildPlace.FindAliasName("MaxPropositionsPerUser");
            string tMaxPropositionsPerRequest = NWDGuildPlace.FindAliasName("MaxPropositionsPerRequest");
            string tRequestLifeTime = NWDGuildPlace.FindAliasName("RequestLifeTime");

            string sScript = "" +
                "// start Addon \n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tServerPropositions = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_GuildStatus + "`, `" + t_THIS_GuildHash + "`, `" + t_THIS_Propositions + "` FROM `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` " +
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';" +
                "$tResultStatus = $SQL_CON->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                    "{\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "error('SERVER');\n" +
                    "}\n" +
                "else" +
                    "{\n" +
                           "if ($tResultStatus->num_rows == 1)\n" +
                            "{\n" +
                                   "$tRowStatus = $tResultStatus->fetch_assoc();\n" +
                                   "$tServerStatut = $tRowStatus['" + t_THIS_GuildStatus + "'];\n" +
                                   "$tServerHash = $tRowStatus['" + t_THIS_GuildHash + "'];\n" +
                                "$tServerPropositions = $tRowStatus['" + t_THIS_Propositions + "'];\n" +
                            "}\n" +
                       "}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
                "if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Sync).ToString() +
                " || $sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                ")\n" +
                    "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryGuildPlace = 'SELECT" +
                        //" `" + tMaxRequestPerUser + "`," +
                        //" `" + tMaxPropositionsPerUser + "`," +
                        " `" + tMaxPropositionsPerRequest + "`," +
                        " `" + tRequestLifeTime + "`" +
                        " FROM `'.$ENV.'_" + NWDGuildPlace.BasisHelper().ClassNamePHP + "`" +
                        " WHERE" +
                        " `Reference` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_THIS_Index_GuildPlace + "]).'\\';';" +
                        "myLog('tQueryGuildPlace : '.$tQueryGuildPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultGuildPlace = $SQL_CON->query($tQueryGuildPlace);\n" +
                        "if (!$tResultGuildPlace)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryGuildPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultGuildPlace->num_rows == 1)\n" +
                                    "{\n" +
                                        "myLog('FIND THE GuildPLACE', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "$tRowGuildPlace = $tResultGuildPlace->fetch_assoc();\n" +
                                        "$sReplaces[" + t_THIS_Index_LimitDayTime + "] = $TIME_SYNC + $tRowGuildPlace['" + tRequestLifeTime + "'];\n" +
                                        "$sReplaces[" + t_THIS_Index_MaxPropositions + "]= $tRowGuildPlace['" + tMaxPropositionsPerRequest + "'];\n" +
                                        //"$sReplaces[" + t_THIS_Index_Propositions + "]= $tRowGuildPlace['" + t_THIS_GuildHash + "'];\n" +
                                        //"$sReplaces[" + t_THIS_Index_Propositions + "]= $tRowGuildPlace['" + t_THIS_Propositions + "'];\n" +
                                    "}\n" +
                            "}\n" +
                        "$sReplaces[" + t_THIS_Index_GuildHash + "] = $TIME_SYNC.RandomString();\n" +
                        "$sReplaces[" + t_THIS_Index_GuildStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (" +
                "$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
                " || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
                "))\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_GuildHash + "] = $TIME_SYNC;\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryCancelable = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        "';" +
                        "$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
                        "if (!$tResultCancelable)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        // START CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I need to put all propositions in Expired\n" +
                                        "$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tGuildRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tGuildRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tGuildRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                                "AND `" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tGuildRequestHash + "` = \\''.$tServerHash.'\\' " +
                                                "';\n" +
                                                "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                                "if (!$tResultExpired)\n" +
                                                    "{\n" +
                                                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "error('SERVER');\n" +
                                                        "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "while ($tRowExpired = $tResultExpired->fetch_row())\n" +
                                                            "{\n" +
                                                                "myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "Integrity" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // FINISH CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I can integrate data to expired!\n" +
                                        "Integrity" + BasisHelper().ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
                        "//stop the function!\n" +
                        "myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO DEAL 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        "';" +
                        "// I need to put winner propositions to Accepted Or it's reject?\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultDeal = $SQL_CON->query($tQueryDeal);\n" +
                        "if (!$tResultDeal)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('UBRRx31');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "// I need to put Accepted or expired in this request?\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                    "// I need to put all propositions in Expired\n" +
                                        "$tQueryAccepted = 'UPDATE `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tGuildRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND `" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\'" +
                                        "AND `" + tGuildRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\' " +
                                        "';\n" +
                                        "$tResultAccepted = $SQL_CON->query($tQueryAccepted);" +
                                        "if (!$tResultAccepted)\n" +
                                            "{\n" +
                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAccepted.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "// I need to put Accepted or expired in this request?\n" +
                                                "$tNumberOfRow = 0;\n" +
                                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                                "if ($tNumberOfRow == 1)\n" +
                                                    "{\n" +
                                                        "$tItemsSend = '';\n" +
                                                        "$tQueryGuildProposition = 'SELECT" +
                                                        " `" + tItemsSend + "`" +
                                                        " FROM `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "`" +
                                                        " WHERE" +
                                                        " `Reference` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_THIS_Index_WinnerProposition + "]).'\\';" +
                                                        "';\n" +
                                                        "myLog('tQueryGuildProposition : '.$tQueryGuildProposition.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "$tResultGuildProposition = $SQL_CON->query($tQueryGuildProposition);\n" +
                                                        "if (!$tResultGuildProposition)\n" +
                                                            "{\n" +
                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryGuildProposition.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "error('SERVER');\n" +
                                                            "}\n" +
                                                        "else" +
                                                            "{\n" +
                                                                "if ($tResultGuildProposition->num_rows == 1)\n" +
                                                                    "{\n" +
                                                                        "myLog('FIND THE USER Guild PROPOSITION', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                        "$tRowGuildProposition = $tResultGuildProposition->fetch_assoc();\n" +
                                                                        "$tItemsSend = $tRowGuildProposition['" + tItemsSend + "'];\n" +
                                                                    "}\n" +
                                                            "}\n" +
                                                        "$tQueryAcceptedDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + t_THIS_ItemsReceived + "` = \\''.$tItemsSend.'\\', " +
                                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_GuildHash + "` = \\''.$tServerHash.'\\' " +
                                                        "AND `Reference` = \\''.$tReference.'\\' " +
                                                        "';\n" +
                                                        "$tResultAcceptedDeal = $SQL_CON->query($tQueryAcceptedDeal);" +
                                                        "if (!$tResultAcceptedDeal)\n" +
                                                            "{\n" +
                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAcceptedDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "error('SERVER');\n" +
                                                            "}\n" +
                                                        "Integrity" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_WinnerProposition + "]);\n" +
                                                    "}\n" +
                                                "else\n" +
                                                    "{\n" +
                                                        "$tQueryExpiredDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_GuildStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_GuildHash + "` = \\''.$tServerHash.'\\' " +
                                                        "AND `Reference` = \\''.$tReference.'\\' " +
                                                        "';\n" +
                                                        "$tResultExpiredDeal = $SQL_CON->query($tQueryExpiredDeal);" +
                                                        "if (!$tResultExpiredDeal)\n" +
                                                            "{\n" +
                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpiredDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "error('SERVER');\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // START CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I need to put all propositions in Expired\n" +
                                        "$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tGuildRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tGuildRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tGuildRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                                "AND `" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tGuildRequestHash + "` = \\''.$tServerHash.'\\' " +
                                                "';\n" +
                                                "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                                "if (!$tResultExpired)\n" +
                                                    "{\n" +
                                                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "error('SERVER');\n" +
                                                        "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "while ($tRowExpired = $tResultExpired->fetch_row())\n" +
                                                            "{\n" +
                                                                "myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "Integrity" + NWDUserGuildSubcription.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // FINISH CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I can integrate data to expired!\n" +
                                        "Integrity" + BasisHelper().ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
                        "//stop the function!\n" +
                        "myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "//EXECEPTION FOR ADMIN\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_GuildStatus + "] == " + ((int)NWDTradeStatus.ForceNone).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_GuildStatus + "] = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_GuildHash + "] = $TIME_SYNC;\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +
                // OTHER
                "else\n" +
                      "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +
                "// finish Addon \n";

            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif