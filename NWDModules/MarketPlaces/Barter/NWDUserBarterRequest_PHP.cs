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
    public partial class NWDUserBarterRequest : NWDBasis<NWDUserBarterRequest>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tBarterStatus = NWDUserBarterProposition.FindAliasName("BarterStatus");
            string tBarterRequest = NWDUserBarterProposition.FindAliasName("BarterRequest");
            string tBarterRequestHash = NWDUserBarterProposition.FindAliasName("BarterRequestHash");

            string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            string t_THIS_Propositions = FindAliasName("Propositions");
            string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");
            string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            string t_THIS_BarterHash = FindAliasName("BarterHash");

            int t_THIS_Index_WinnerProposition = CSV_IndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_Propositions = CSV_IndexOf(t_THIS_Propositions);
            int t_THIS_Index_PropositionsCounter = CSV_IndexOf(t_THIS_PropositionsCounter);
            int t_THIS_Index_BarterStatus = CSV_IndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_BarterHash = CSV_IndexOf(t_THIS_BarterHash);

            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            int t_THIS_Index_ItemsProposed = CSV_IndexOf(t_THIS_ItemsProposed);
            string t_THIS_ItemsSuggested = FindAliasName("ItemsSuggested");
            int t_THIS_Index_ItemsSuggested = CSV_IndexOf(t_THIS_ItemsSuggested);
            string t_THIS_ItemsReceived = FindAliasName("ItemsReceived");
            int t_THIS_Index_ItemsReceived = CSV_IndexOf(t_THIS_ItemsReceived);

            string sScript = "" +
                "// start Addon \n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tServerPropositions = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterHash + "`, `" + t_THIS_Propositions + "` FROM `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` " +
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
                                   "$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];\n" +
                                   "$tServerHash = $tRowStatus['" + t_THIS_BarterHash + "'];\n" +
                                "$tServerPropositions = $tRowStatus['" + t_THIS_Propositions + "'];\n" +
                            "}\n" +
                       "}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                ")\n" +
                    "{\n" +
                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$sReplaces[" + t_THIS_Index_BarterHash + "] = $TIME_SYNC;\n" +
                           "$sReplaces[" + t_THIS_Index_BarterStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
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
                        "$sReplaces[" + t_THIS_Index_BarterHash + "] = $TIME_SYNC;\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
                        "$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
                        "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                        "$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryCancelable = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
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
                                        "$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
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
                                                                "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
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
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\' " +
                        "WHERE " +
                        "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
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
                                        "$tQueryAccepted = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\'" +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
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
                                                        "$tQueryAcceptedDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
                                                        "AND `Reference` = \\''.$tReference.'\\' " +
                                                        "';\n" +
                                                        "$tResultAcceptedDeal = $SQL_CON->query($tQueryAcceptedDeal);" +
                                                        "if (!$tResultAcceptedDeal)\n" +
                                                            "{\n" +
                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAcceptedDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "error('SERVER');\n" +
                                                            "}\n" +
                                                    "}\n" +
                                                "else\n" +
                                                    "{\n" +
                                                        "$tQueryExpiredDeal = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
                                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                        "WHERE " +
                                                        "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
                                                        "AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
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
                                        "$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "` SET " +
                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                        "WHERE " +
                                        "`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                        "AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
                                        "`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
                                        "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
                                        "';\n" +
                                        "$tResultExpired = $SQL_CON->query($tQueryExpired);" +
                                        "if (!$tResultExpired)\n" +
                                            "{\n" +
                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "`" +
                                                "WHERE " +
                                                "`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                                                "AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
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
                                                                "Integrity" + NWDUserBarterProposition.BasisHelper().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
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
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                    "//EXECEPTION FOR ADMIN\n" +
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