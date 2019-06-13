// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:11
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
    public partial class NWDUserBarterRequestHelper : NWDHelper<NWDUserBarterRequest>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {

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



            //string tBarterStatus = NWDUserBarterProposition.FindAliasName("BarterStatus");
            //string tBarterRequest = NWDUserBarterProposition.FindAliasName("BarterRequest");
            //string tBarterRequestHash = NWDUserBarterProposition.FindAliasName("BarterRequestHash");
            //string tItemsSend = NWDUserBarterProposition.FindAliasName("ItemsSend");

            //string tMaxRequestPerUser = NWDBarterPlace.FindAliasName("MaxRequestPerUser");
            //string tMaxPropositionsPerUser = NWDBarterPlace.FindAliasName("MaxPropositionsPerUser");
            //string tMaxPropositionsPerRequest = NWDBarterPlace.FindAliasName("MaxPropositionsPerRequest");
            //string tRequestLifeTime = NWDBarterPlace.FindAliasName("RequestLifeTime");

            //string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            //string t_THIS_Propositions = FindAliasName("Propositions");
            //string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");
            //string t_THIS_MaxPropositions = FindAliasName("MaxPropositions");
            //string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            //string t_THIS_BarterHash = FindAliasName("BarterHash");
            //string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            //string t_THIS_LimitDayTime = FindAliasName("LimitDayTime");
            //string t_THIS_ItemsReceived = FindAliasName("ItemsReceived");
            //string t_THIS_ItemsSuggested = FindAliasName("ItemsSuggested");
            //string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");

            int t_THIS_Index_WinnerProposition = New_CSV_IndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_Propositions = New_CSV_IndexOf(t_THIS_Propositions);
            int t_THIS_Index_PropositionsCounter = New_CSV_IndexOf(t_THIS_PropositionsCounter);
            int t_THIS_Index_MaxPropositions = New_CSV_IndexOf(t_THIS_MaxPropositions);
            int t_THIS_Index_BarterStatus = New_CSV_IndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_BarterHash = New_CSV_IndexOf(t_THIS_BarterHash);
            int t_THIS_Index_BarterPlace = New_CSV_IndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_LimitDayTime = New_CSV_IndexOf(t_THIS_LimitDayTime);

            int t_THIS_Index_ItemsProposed = New_CSV_IndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsSuggested = New_CSV_IndexOf(t_THIS_ItemsSuggested);
            int t_THIS_Index_ItemsReceived = New_CSV_IndexOf(t_THIS_ItemsReceived);




            string sScript = "" +
                "// start Addon \n" +
                "include_once ( "+NWD.K_PATH_BASE+".'/'."+NWD.K_ENV+".'/" + NWD.K_DB + "/" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tServerPropositions = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterHash + "`, `" + t_THIS_Propositions + "` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
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
                                   "$tServerHash = $tRowStatus['" + t_THIS_BarterHash + "'];\n" +
                                "$tServerPropositions = $tRowStatus['" + t_THIS_Propositions + "'];\n" +
                            "}\n" +
                       "}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                //" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                //" || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                ")\n" +
                    "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryBarterPlace = 'SELECT" +
                        //" `" + tMaxRequestPerUser + "`," +
                        //" `" + tMaxPropositionsPerUser + "`," +
                        " `" + tMaxPropositionsPerRequest + "`," +
                        " `" + tRequestLifeTime + "`" +
                        " FROM `'."+NWD.K_ENV+".'_" + NWDBarterPlace.BasisHelper().ClassNamePHP + "`" +
                        " WHERE" +
                        " `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_THIS_Index_BarterPlace + "]).'\\';';" +
                        //"myLog('tQueryBarterPlace : '.$tQueryBarterPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultBarterPlace = "+NWD.K_SQL_CON+"->query($tQueryBarterPlace);\n" +
                        "if (!$tResultBarterPlace)\n" +
                            "{\n" +
                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryBarterPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultBarterPlace->num_rows == 1)\n" +
                                    "{\n" +
                                        //"myLog('FIND THE BARTERPLACE', __FILE__, __FUNCTION__, __LINE__);\n" +
                                        "$tRowBarterPlace = $tResultBarterPlace->fetch_assoc();\n" +
                                        "$sReplaces[" + t_THIS_Index_LimitDayTime + "] = "+NWD.K_PHP_TIME_SYNC+" + $tRowBarterPlace['" + tRequestLifeTime + "'];\n" +
                                        "$sReplaces[" + t_THIS_Index_MaxPropositions + "]= $tRowBarterPlace['" + tMaxPropositionsPerRequest + "'];\n" +
                                    //"$sReplaces[" + t_THIS_Index_Propositions + "]= $tRowBarterPlace['" + t_THIS_BarterHash + "'];\n" +
                                    //"$sReplaces[" + t_THIS_Index_Propositions + "]= $tRowBarterPlace['" + t_THIS_Propositions + "'];\n" +
                                    "}\n" +
                            "}\n" +
                        "$sReplaces[" + t_THIS_Index_BarterHash + "] = "+NWD.K_PHP_TIME_SYNC+".RandomString();\n" +
                        "$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (" +
                "$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
                " || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
                "))\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_BarterHash + "] = "+NWD.K_PHP_TIME_SYNC+";\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO CANCEL 
                "else if (($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " ||" +
                " $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() + ") && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryCancelable = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
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
                                        // START CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I need to put all propositions in Expired\n" +
                                        "$tQueryExpired = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                                "';\n" +
                                                "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                                "if (!$tResultExpired)\n" +
                                                    "{\n" +
                                                        //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                        "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "while ($tRowExpired = $tResultExpired->fetch_row())\n" +
                                                            "{\n" +
                                                                //"myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // FINISH CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I can integrate data to expired!\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "//stop the function!\n" +
                        //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryCancelable = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        "AND `" + t_THIS_LimitDayTime + "` < \\''."+NWD.K_PHP_TIME_SYNC+".'\\' " +
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
                                        // START CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I need to put all propositions in Expired\n" +
                                        "$tQueryExpired = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                                "';\n" +
                                                "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                                "if (!$tResultExpired)\n" +
                                                    "{\n" +
                                                        //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                        "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "while ($tRowExpired = $tResultExpired->fetch_row())\n" +
                                                            "{\n" +
                                                                //"myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // FINISH CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I can integrate data to expired!\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "//stop the function!\n" +
                        //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO DEAL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryDeal = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + t_THIS_WinnerProposition + "` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                        "';" +
                        "// I need to put winner propositions to Accepted Or it's reject?\n" +
                        //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultDeal = "+NWD.K_SQL_CON+"->query($tQueryDeal);\n" +
                        "if (!$tResultDeal)\n" +
                            "{\n" +
                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                //"error('UBRRx31',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "// I need to put Accepted or expired in this request?\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                    "// I need to put all propositions in Expired\n" +
                                        "$tQueryAccepted = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                        "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\'" +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\' " +
                                        "';\n" +
                                        "$tResultAccepted = "+NWD.K_SQL_CON+"->query($tQueryAccepted);" +
                                        "if (!$tResultAccepted)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryAccepted.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "// I need to put Accepted or expired in this request?\n" +
                                                "$tNumberOfRow = 0;\n" +
                                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                                "if ($tNumberOfRow == 1)\n" +
                                                    "{\n" +
                                                        "$tItemsSend = '';\n" +
                                                        "$tQueryBarterProposition = 'SELECT" +
                                                        " `" + tItemsSend + "`" +
                                                        " FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                        " WHERE" +
                                                        " `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_THIS_Index_WinnerProposition + "]).'\\';" +
                                                        "';\n" +
                                                        //"myLog('tQueryBarterProposition : '.$tQueryBarterProposition.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        "$tResultBarterProposition = "+NWD.K_SQL_CON+"->query($tQueryBarterProposition);\n" +
                                                        "if (!$tResultBarterProposition)\n" +
                                                            "{\n" +
                                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryBarterProposition.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                            "}\n" +
                                                        "else" +
                                                            "{\n" +
                                                                "if ($tResultBarterProposition->num_rows == 1)\n" +
                                                                    "{\n" +
                                                                        //"myLog('FIND THE USER BARTER PROPOSITION', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                        "$tRowBarterProposition = $tResultBarterProposition->fetch_assoc();\n" +
                                                                        "$tItemsSend = $tRowBarterProposition['" + tItemsSend + "'];\n" +
                                                                    "}\n" +
                                                            "}\n" +
                                                        "$tQueryAcceptedDeal = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`" + t_THIS_ItemsReceived + "` = \\''.$tItemsSend.'\\', " +
                                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
                                                        "AND `Reference` = \\''.$tReference.'\\' " +
                                                        "';\n" +
                                                        "$tResultAcceptedDeal = "+NWD.K_SQL_CON+"->query($tQueryAcceptedDeal);" +
                                                        "if (!$tResultAcceptedDeal)\n" +
                                                            "{\n" +
                                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryAcceptedDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                            "}\n" +
                                                        "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_WinnerProposition + "]);\n" +
                                                    "}\n" +
                                                "else\n" +
                                                    "{\n" +
                                                        "$tQueryExpiredDeal = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
                                                        "AND `Reference` = \\''.$tReference.'\\' " +
                                                        "';\n" +
                                                        "$tResultExpiredDeal = "+NWD.K_SQL_CON+"->query($tQueryExpiredDeal);" +
                                                        "if (!$tResultExpiredDeal)\n" +
                                                            "{\n" +
                                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpiredDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // START CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I need to put all propositions in Expired\n" +
                                        "$tQueryExpired = 'UPDATE `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tBarterRequest + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                                "';\n" +
                                                "$tResultExpired = "+NWD.K_SQL_CON+"->query($tQueryExpired);" +
                                                "if (!$tResultExpired)\n" +
                                                    "{\n" +
                                                        //"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                        //"error('SERVER',true, __FILE__, __FUNCTION__, __LINE__);\n" +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER) +
                                                        "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "while ($tRowExpired = $tResultExpired->fetch_row())\n" +
                                                            "{\n" +
                                                                //"myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                        // FINISH CANCEL PUT PROPOSITION TO EXPIRED
                                        "// I can integrate data to expired!\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue ($tReference);\n" +
                                    "}\n" +
                            "}\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "//stop the function!\n" +
                        //"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "//EXECEPTION FOR ADMIN\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.ForceNone).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_BarterStatus + "] = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_BarterHash + "] = "+NWD.K_PHP_TIME_SYNC+";\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // OTHER
                "else\n" +
                      "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
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