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
    public partial class NWDUserBarterFinderHelper : NWDHelper<NWDUserBarterFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            //string tTradeHash = NWDToolbox.PropertyName(() => NWDUserTradeRequest.FictiveData().TradeHash);

            string tBarterStatus = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterStatus);
            string tLimitDayTime = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().LimitDayTime);
            string tBarterPlaceRequest = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().BarterPlace);
            string tForRelationshipOnly = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().ForRelationshipOnly);
            string tRelationshipAccountReferences = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().UserRelationship);

            string t_THIS_BarterRequestsList = NWDToolbox.PropertyName(() => FictiveData().BarterRequestsList);
            string t_THIS_BarterPlace = NWDToolbox.PropertyName(() => FictiveData().BarterPlace);
            string t_THIS_ForRelationshipOnly = NWDToolbox.PropertyName(() => FictiveData().ForRelationshipOnly);
            string t_THIS_MaxPropositions = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().MaxPropositions);
            string t_THIS_PropositionsCounter = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().PropositionsCounter);


            //string tBarterStatus = NWDUserBarterRequest.FindAliasName("BarterStatus");
            //string tLimitDayTime = NWDUserBarterRequest.FindAliasName("LimitDayTime");
            //string tBarterPlaceRequest = NWDUserBarterRequest.FindAliasName("BarterPlace");
            //string tForRelationshipOnly = NWDUserBarterRequest.FindAliasName("ForRelationshipOnly");
            //string tRelationshipAccountReferences = NWDUserBarterRequest.FindAliasName("RelationshipAccountReferences");

            //string t_THIS_BarterRequestsList = FindAliasName("BarterRequestsList");
            //string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            //string t_THIS_ForRelationshipOnly = FindAliasName("ForRelationshipOnly");
            //string t_THIS_MaxPropositions = FindAliasName("MaxPropositions");
            //string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");

            int tIndex_tBarterStatus = NWDUserBarterRequest.CSV_IndexOf(tBarterStatus);

            int tIndex_BarterRequestsList = New_CSV_IndexOf(t_THIS_BarterRequestsList);
            int tIndex_BarterPlace = New_CSV_IndexOf(t_THIS_BarterPlace);
            int tIndex_THIS_ForRelationshipOnly = New_CSV_IndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 300; // minutes before stop to get the datas!
            string sScript = "" +
                "include_once($PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "$tQueryExpired = 'SELECT " + NWDUserBarterRequest.SLQSelect() + " FROM `'.$ENV.'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
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
                                "$tRowExpired = Integrity" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "Replace ($tRowExpired," + tIndex_tBarterStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");\n" +
                                "$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);\n" +
                                "UpdateData" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + " ($tRowExpired, $TIME_SYNC, $uuid, false);\n" +
                            "}\n" +
                        //"mysqli_free_result($tResultExpired);\n" +
                    "}\n" +
                "// start Addon \n" +
                "$tQueryBarter = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` " +
                // WHERE REQUEST
                "WHERE `AC`= \\'1\\' " +
                "AND `Account` != \\''.$SQL_CON->real_escape_string($uuid).'\\' " +
                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' " +
                "AND `" + t_THIS_MaxPropositions + "` > `" + t_THIS_PropositionsCounter + "` ';\n" +
                "if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')\n" +
                    "{\n" +
                        "$tQueryBarter.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';\n" +
                    "}\n" +
                "$tQueryBarter.= '" +
                "AND `" + tBarterPlaceRequest + "` = \\''.$sCsvList[" + tIndex_BarterPlace + "].'\\' " +
                "AND `" + tLimitDayTime + "` > '.($TIME_SYNC+" + (tDelayOfRefresh).ToString() + ").' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                // LIMIT 
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryBarter : '. $tQueryBarter, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultBarter = $SQL_CON->query($tQueryBarter);\n" +
                "$tReferences = \'\';\n" +
                "$tReferencesList = \'\';\n" +
                "if (!$tResultBarter)\n" +
                    "{\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryBarter.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "error('UTRFx31');\n" +
                    "}\n" +
                "else\n" +
                    "{\n" +
                        "while($tRowBarter = $tResultBarter->fetch_assoc())\n" +
                            "{\n" +
                                "myLog('tReferences found : '. $tRowBarter['Reference'], __FILE__, __FUNCTION__, __LINE__);\n" +
                                "$tReferences[]=$tRowBarter['Reference'];\n" +
                            "}\n" +
                        "if (is_array($tReferences))\n" +
                            "{\n" +
                                "$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);\n" +
                                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                                "GetDatas" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "ByReferences ($tReferences);\n" +
                            "}\n" +
                    "}\n" +
                "myLog('tReferencesList : '. $tReferencesList, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$sCsvList = Integrity" + NWDUserBarterFinder.BasisHelper().ClassNamePHP + "Replace ($sCsvList, " + tIndex_BarterRequestsList.ToString() + ", $tReferencesList);\n" +
                "";
            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif