﻿// =====================================================================================================================
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
// =====================================================================================================================

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
public partial class NWDUserBarterPropositionHelper : NWDHelper<NWDUserBarterProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {

            //string tTradeHash = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeHash);

            string tBarterStatus = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().LimitDayTime);
            string tBarterPlace = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterPlace);
            //string tBarterRequest = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterRequest);
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


            //string tBarterStatus = NWDUserBarterRequest.FindAliasName("BarterStatus");
            //string tLimitDayTime = NWDUserBarterRequest.FindAliasName("LimitDayTime");
            //string tBarterPlace = NWDUserBarterRequest.FindAliasName("BarterPlace");
            //string tBarterRequest = NWDUserBarterRequest.FindAliasName("BarterRequest");
            //string tWinnerProposition = NWDUserBarterRequest.FindAliasName("WinnerProposition");
            //string tPropositions = NWDUserBarterRequest.FindAliasName("Propositions");
            //string tMaxPropositions = NWDUserBarterRequest.FindAliasName("MaxPropositions");
            //string tPropositionsCounter = NWDUserBarterRequest.FindAliasName("PropositionsCounter");
            //string tBarterHash = NWDUserBarterRequest.FindAliasName("BarterHash");
            //string tItemsProposed = NWDUserBarterRequest.FindAliasName("ItemsProposed");

            //string t_THIS_BarterRequestHash = FindAliasName("BarterRequestHash");
            //string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            //string t_THIS_BarterRequest = FindAliasName("BarterRequest");
            //string t_THIS_BarterStatus = FindAliasName("BarterStatus");

            //string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            //string t_THIS_ItemsSend = FindAliasName("ItemsSend");


            int t_THIS_Index_BarterRequestHash = New_CSV_IndexOf(t_THIS_BarterRequestHash);
            int t_THIS_Index_BarterPlace = New_CSV_IndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_BarterRequest = New_CSV_IndexOf(t_THIS_BarterRequest);
            int t_THIS_Index_BarterStatus = New_CSV_IndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_ItemsProposed = New_CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsSend = New_CSV_IndexOf(t_THIS_ItemsSend);

            string sScript = "" +
                "// debut find \n" +
                "include_once ( "+NWD.K_PATH_BASE+".'/'."+NWD.K_ENV+".'/" + NWD.K_DB + "/" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterRequestHash + "` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
                "WHERE " +
                "`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\';';" +
                "$tResultStatus = "+NWD.K_SQL_CON+"->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                    "{\n" +
                        //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                    "}\n" +
                "else" +
                    "{\n" +
                        "if ($tResultStatus->num_rows == 1)\n" +
                            "{\n" +
                                "$tRowStatus = $tResultStatus->fetch_assoc();\n" +
                                "$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];\n" +
                                "$tServerHash = $tRowStatus['" + t_THIS_BarterRequestHash + "'];\n" +
                            "}\n" +
                    "}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, DEAL, REFRESH, CANCELLED
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                //" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() + ")\n" +
                    "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && " +
                "($tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
                " || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
                "))\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSend + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_BarterRequestHash + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_BarterRequest + "]='';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                        //"myLog('PUT TO NONE FROM EXPIRED OR ACCEPTED', __FILE__, __FUNCTION__, __LINE__);\n" +
                    "}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryTrade = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` SET " +
                        " `DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `" + tPropositions + "` = TRIM(\\'" + NWDConstants.kFieldSeparatorA + "\\' FROM CONCAT(CONCAT(`" + tPropositions + "`,\\'" + NWDConstants.kFieldSeparatorA + "\\'),\\''.$sCsvList[0].'\\')), " +
                        " `" + tPropositionsCounter + "` = `" + tPropositionsCounter + "`+1 " +
                        " WHERE `AC`= \\'1\\' " +
                        " AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        " AND `" + tBarterPlace + "` = \\''.$sCsvList[" + t_THIS_Index_BarterPlace + "].'\\' " +
                        " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' " +
                        " AND `" + tBarterHash + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequestHash + "].'\\' " +
                        " AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        " AND `" + tPropositionsCounter + "` < `" + tMaxPropositions + "` " +
                        "';\n" +
                        //"myLog('tQueryTrade : '. $tQueryTrade, __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultTrade = "+NWD.K_SQL_CON+"->query($tQueryTrade);\n" +
                        "$tReferences = \'\';\n" +
                        "$tReferencesList = \'\';\n" +
                        "if (!$tResultTrade)\n" +
                            "{\n" +
                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryTrade.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                            "}\n" +
                        "else\n" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "// I need update the proposition too !\n" +
                                        //   "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Waiting).ToString() + "\');\n" +

                                        "$tQueryBarterRequest = 'SELECT" +
                                        " `" + tItemsProposed + "`" +
                                        " FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "`" +
                                        " WHERE" +
                                        " `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_THIS_Index_BarterRequest + "]).'\\';" +
                                        "';\n" +
                                        //"myLog('tQueryBarterPlace : '.$tQueryBarterRequest.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "$tResultBarterRequest = "+NWD.K_SQL_CON+"->query($tQueryBarterRequest);\n" +
                                        "if (!$tResultBarterRequest)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryBarterRequest.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                                                NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "if ($tResultBarterRequest->num_rows == 1)\n" +
                                                    "{\n" +
                                                        //"myLog('FIND THE USER BARTER REQUEST', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "$tRowBarterRequest = $tResultBarterRequest->fetch_assoc();\n" +
                                                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "] = $tRowBarterRequest['" + tItemsProposed + "'];\n" +
                                                    "}\n" +
                                            "}\n" +
                                        "$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +

                                        //"myLog('I need update the proposition waiting', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "Integrity" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n" +
                                    "}\n" +
                                "else\n" +
                                    "{\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');\n" +
                                        //"myLog('I need update the proposition refused ... too late!', __FILE__, __FUNCTION__, __LINE__);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n" +
                    "}\n" +

                // change the statut from CSV TO NODEAL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryTrade = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` SET " +
                        " `DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                        " `" + tPropositions + "` = TRIM(\\'" + NWDConstants.kFieldSeparatorA + "\\' FROM CONCAT(CONCAT(`" + tPropositions + "`,\\'" + NWDConstants.kFieldSeparatorA + "\\'),\\''.$sCsvList[0].'\\')), " +
                        " `" + tPropositionsCounter + "` = `" + tPropositionsCounter + "`+1 " +
                        " WHERE `AC`= \\'1\\' " +
                        " AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        " AND `" + tBarterPlace + "` = \\''.$sCsvList[" + t_THIS_Index_BarterPlace + "].'\\' " +
                        " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' " +
                        " AND `" + tBarterHash + "` = \\''.$sCsvList[" + t_THIS_Index_BarterRequestHash + "].'\\' " +
                        " AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        " AND `" + tPropositionsCounter + "` < `" + tMaxPropositions + "` " +
                        "';\n" +
                        //"myLog('tQueryTrade : '. $tQueryTrade, __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultTrade = "+NWD.K_SQL_CON+"->query($tQueryTrade);\n" +
                        "$tReferences = \'\';\n" +
                        "$tReferencesList = \'\';\n" +
                        "if (!$tResultTrade)\n" +
                            "{\n" +
                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryTrade.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                            "}\n" +
                        "else\n" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "// I need update the proposition too !\n" +
                                        //   "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Waiting).ToString() + "\');\n" +

                                        "$tQueryBarterRequest = 'SELECT" +
                                        " `" + tPropositionsCounter + "`, `" + tMaxPropositions + "`,`" + tItemsProposed + "`" +
                                        " FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "`" +
                                        " WHERE" +
                                        " `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_THIS_Index_BarterRequest + "]).'\\';" +
                                        "';\n" +
                                        //"myLog('tQueryBarterPlace : '.$tQueryBarterRequest.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "$tResultBarterRequest = "+NWD.K_SQL_CON+"->query($tQueryBarterRequest);\n" +
                                        "if (!$tResultBarterRequest)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryBarterRequest.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                                                NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "if ($tResultBarterRequest->num_rows == 1)\n" +
                                                    "{\n" +
                                                        //"myLog('FIND THE USER BARTER REQUEST', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "$tRowBarterRequest = $tResultBarterRequest->fetch_assoc();\n" +
                                                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "] = $tRowBarterRequest['" + tItemsProposed + "'];\n" +
                                                        "" +
                                                        "" +
                                                        "" +
                                                        "// TODO update if barter proposition == 1 to expired" +
                                                        "if ($tRowBarterRequest['" + tMaxPropositions + "'] == 1)\n" +
                                                        "{\n" +
                                                            "$tQueryExpired = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` SET " +
                                                            " `DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                                                            " `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                                                            " `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\'," +
                                                            " `" + tLimitDayTime + "` = '."+NWD.K_PHP_TIME_SYNC+".', " +
                                                            " `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                            " WHERE `AC`= \\'1\\' " +
                                                            " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' " +
                                                            "';\n" +
                                                            //"myLog('tQueryExpired : '. $tQueryExpired, __FILE__, __FUNCTION__, __LINE__);\n" +
                                                            "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);\n" +
                                                        "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        "$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Expired).ToString() + ";\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +

                                        //"myLog('I need update the proposition waiting', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "Integrity" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n" +
                                    "}\n" +
                                "else\n" +
                                    "{\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');\n" +
                                        //"myLog('I need update the proposition refused ... too late!', __FILE__, __FUNCTION__, __LINE__);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n" +
                    "}\n" +


                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryCancelable = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        "';" +
                        "$tResultCancelable = "+NWD.K_SQL_CON+"->query($tQueryCancelable);\n" +
                        "if (!$tResultCancelable)\n" +
                            "{\n" +
                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                                 NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +


                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "//EXECEPTION FOR ADMIN\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE NONE  // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.ForceNone).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSend + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_BarterRequestHash + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_BarterRequest + "]='';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // OTHER
                "else\n" +
                    "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +
                //"myLog('FINSIH ADD ON ... UPDATE FROM CSV', __FILE__, __FUNCTION__, __LINE__);\n" +
                "// finish Addon \n";

            return sScript;
        }
        //------------------------------------------------------------------------------------------------------------- 
        public override string New_AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            string t_THIS_BarterRequest = NWDToolbox.PropertyName(() => FictiveData().BarterRequest);
            //string t_THIS_BarterRequest = FindAliasName("BarterRequest");
            int t_THIS_Index_BarterRequest = New_CSV_IndexOf(t_THIS_BarterRequest);

            return "// write your php script here to update after sync on server\n " +
                "GetDatas" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif