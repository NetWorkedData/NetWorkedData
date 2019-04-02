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
    public partial class NWDUserGuildFinder : NWDBasis<NWDUserGuildFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tGuildStatus = NWDUserGuild.FindAliasName("GuildStatus");
            string tLimitDayTime = NWDUserGuild.FindAliasName("LimitDayTime");
            string tGuildPlaceRequest = NWDUserGuild.FindAliasName("GuildPlace");
            string tForRelationshipOnly = NWDUserGuild.FindAliasName("ForRelationshipOnly");
            string tRelationshipAccountReferences = NWDUserGuild.FindAliasName("RelationshipAccountReferences");
            int tIndex_tGuildStatus = NWDUserGuild.CSV_IndexOf(tGuildStatus);

            string t_THIS_GuildRequestsList = FindAliasName("GuildRequestsList");
            string t_THIS_GuildPlace = FindAliasName("GuildPlace");
            string t_THIS_ForRelationshipOnly = FindAliasName("ForRelationshipOnly");
            string t_THIS_MaxPropositions = FindAliasName("MaxPropositions");
            string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");

            int tIndex_GuildRequestsList = CSV_IndexOf(t_THIS_GuildRequestsList);
            int tIndex_GuildPlace = CSV_IndexOf(t_THIS_GuildPlace);
            int tIndex_THIS_ForRelationshipOnly = CSV_IndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 300; // minutes before stop to get the datas!
            string sScript = "" +
                "include_once($PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserGuild.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "$tQueryExpired = 'SELECT " + NWDUserGuild.SLQSelect() + " FROM `'.$ENV.'_" + NWDUserGuild.BasisHelper().ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tLimitDayTime + "` < '.$TIME_SYNC.' " +
                "AND `WebModel` <= '.$WSBUILD.' " +
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryExpired : '. $tQueryExpired, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultExpired = $SQL_CON->query($tQueryExpired);\n" +
                "if (!$tResultExpired)\n" +
                    "{\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "error('UTRFx31');\n" +
                    "}\n" +
                "else\n" +
                    "{\n" +
                        "while($tRowExpired = $tResultExpired->fetch_row())\n" +
                            "{\n" +
                                "myLog('tReferences need be cancelled : '. $tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                                "$tRowExpired = Integrity" + NWDUserGuild.BasisHelper().ClassNamePHP + "Replace ($tRowExpired," + tIndex_tGuildStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");\n" +
                                "$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);\n" +
                                "UpdateData" + NWDUserGuild.BasisHelper().ClassNamePHP + " ($tRowExpired, $TIME_SYNC, $uuid, false);\n" +
                            "}\n" +
                        //"mysqli_free_result($tResultExpired);\n" +
                    "}\n" +
                "// start Addon \n" +
                "$tQueryGuild = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserGuild.BasisHelper().ClassNamePHP + "` " +
                // WHERE REQUEST
                "WHERE `AC`= \\'1\\' " +
                "AND `Account` != \\''.$SQL_CON->real_escape_string($uuid).'\\' " +
                "AND `" + tGuildStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' " +
                "AND `" + t_THIS_MaxPropositions + "` > `" + t_THIS_PropositionsCounter + "` ';\n" +
                "if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')\n" +
                    "{\n" +
                        "$tQueryGuild.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';\n" +
                    "}\n" +
                "$tQueryGuild.= '" +
                "AND `" + tGuildPlaceRequest + "` = \\''.$sCsvList[" + tIndex_GuildPlace + "].'\\' " +
                "AND `" + tLimitDayTime + "` > '.($TIME_SYNC+" + (tDelayOfRefresh).ToString() + ").' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                // LIMIT 
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryGuild : '. $tQueryGuild, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultGuild = $SQL_CON->query($tQueryGuild);\n" +
                "$tReferences = \'\';\n" +
                "$tReferencesList = \'\';\n" +
                "if (!$tResultGuild)\n" +
                    "{\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryGuild.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "error('UTRFx31');\n" +
                    "}\n" +
                "else\n" +
                    "{\n" +
                        "while($tRowGuild = $tResultGuild->fetch_assoc())\n" +
                            "{\n" +
                                "myLog('tReferences found : '. $tRowGuild['Reference'], __FILE__, __FUNCTION__, __LINE__);\n" +
                                "$tReferences[]=$tRowGuild['Reference'];\n" +
                            "}\n" +
                        "if (is_array($tReferences))\n" +
                            "{\n" +
                                "$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);\n" +
                                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserGuild.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                                "GetDatas" + NWDUserGuild.BasisHelper().ClassNamePHP + "ByReferences ($tReferences);\n" +
                            "}\n" +
                    "}\n" +
                "myLog('tReferencesList : '. $tReferencesList, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$sCsvList = Integrity" + NWDUserGuildFinder.BasisHelper().ClassNamePHP + "Replace ($sCsvList, " + tIndex_GuildRequestsList.ToString() + ", $tReferencesList);\n" +
                "";
            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif