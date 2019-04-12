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
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
public partial class NWDUserRelationshipHelper : NWDHelper<NWDUserRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {

            string tRelationStatus = NWDToolbox.PropertyName(() => FictiveData().RelationshipStatus);
            string tRelationPlace = NWDToolbox.PropertyName(() => FictiveData().RelationPlace);
            string tRelationshipHash = NWDToolbox.PropertyName(() => FictiveData().RelationshipHash);
            string tRelationshipCode = NWDToolbox.PropertyName(() => FictiveData().RelationshipCode);
            string tLimitDayTime = NWDToolbox.PropertyName(() => FictiveData().LimitDayTime);
            string tFriendLastSynchronization = NWDToolbox.PropertyName(() => FictiveData().FriendLastSynchronization);

            string tFriendAccount = NWDToolbox.PropertyName(() => FictiveData().FriendAccount);
            string tFriendGameSave = NWDToolbox.PropertyName(() => FictiveData().FriendGameSave);

            string tAccount = NWDToolbox.PropertyName(() => FictiveData().Account);
            string tGameSave = NWDToolbox.PropertyName(() => FictiveData().GameSave);
            string tFriendUserRelationShip = NWDToolbox.PropertyName(() => FictiveData().FriendUserRelationShip);

            //string tRelationStatus = FindAliasName("RelationshipStatus");
            //string tRelationPlace = FindAliasName("RelationPlace");
            //string tRelationshipHash = FindAliasName("RelationshipHash");
            //string tRelationshipCode = FindAliasName("RelationshipCode");
            //string tLimitDayTime = FindAliasName("LimitDayTime");
            //string tFriendLastSynchronization = FindAliasName("FriendLastSynchronization");

            //string tFriendAccount = FindAliasName("FriendAccount");
            //string tFriendGameSave = FindAliasName("FriendGameSave");

            //string tAccount = FindAliasName("Account");
            //string tGameSave = FindAliasName("GameSave");
            //string tFriendUserRelationShip = FindAliasName("FriendUserRelationShip");

            int t_Index_RelationStatus = New_CSV_IndexOf(tRelationStatus);
            int t_Index_RelationPlace = New_CSV_IndexOf(tRelationPlace);
            int t_Index_RelationshipHash = New_CSV_IndexOf(tRelationshipHash);
            int t_Index_RelationshipCode = New_CSV_IndexOf(tRelationshipCode);
            int t_Index_LimitDayTime = New_CSV_IndexOf(tLimitDayTime);
            int t_Index_FriendLastSynchronization = New_CSV_IndexOf(tFriendLastSynchronization);

            int t_Index_FriendAccount = New_CSV_IndexOf(tFriendAccount);
            int t_Index_FriendGameSave = New_CSV_IndexOf(tFriendGameSave);

            int t_Index_Account = New_CSV_IndexOf(tAccount);
            int t_Index_GameSave = New_CSV_IndexOf(tGameSave);
            int t_Index_FriendUserRelationShip = New_CSV_IndexOf(tFriendUserRelationShip);

            //string tCodeLenght = NWDRelationshipPlace.FindAliasName("CodeLenght");

            //string tExpireTime = NWDRelationshipPlace.FindAliasName("ExpireTime");
            //string tClassesSharedToStartRelation = NWDRelationshipPlace.FindAliasName("ClassesSharedToStartRelation");
            //string tClassesShared = NWDRelationshipPlace.FindAliasName("ClassesShared");


            string tCodeLenght = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().CodeLenght);

            string tExpireTime = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ExpireTime);
            string tClassesSharedToStartRelation = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesSharedToStartRelation);
            string tClassesShared = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesShared);

            string sScript = "" +
                "// start Addon \n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserNickname.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserAvatar.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDUserInfos.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/" + NWDGameSave.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tServerCode = '';\n" +
                "$tServerLimitDayTime = '';\n" +
                "$tFriendLastSynchronization = 0;\n" +
                "$tServerAccount = '';\n" +
                "$tServerGameSave = '';\n" +
                "$tServerFriendAccount = '';\n" +
                "$tServerFriendGameSave = '';\n" +
                "$tServerID = '';\n" +
                "$tServerRelationPlace = '';\n" +
                "$tFriendUserRelationShip = 0;\n" +
                "$tQueryStatus = 'SELECT `ID`, `" + tRelationStatus + "`," +
                " `" + tRelationshipHash + "`," +
                " `" + tRelationshipCode + "`," +
                " `" + tRelationPlace + "`," +
                " `" + tLimitDayTime + "`," +
                " `" + tAccount + "`," +
                " `" + tGameSave + "`," +
                " `" + tFriendAccount + "`," +
                " `" + tFriendGameSave + "`," +
                " `" + tFriendLastSynchronization + "`," +
                " `" + tFriendUserRelationShip + "" +
                "` FROM `'.$ENV.'_" + ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                "AND `WebModel` <= '.$WSBUILD.' " +
                "';" +
                "$tResultStatus = $SQL_CON->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                    "{\n" +
                        "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "error('SERVER');\n" +
                    "}\n" +
                "else" +
                    "{\n" +
                           "if ($tResultStatus->num_rows == 1)\n" +
                            "{\n" +
                                   "$tRowStatus = $tResultStatus->fetch_assoc();\n" +
                                   "$tServerID = $tRowStatus['ID'];\n" +
                                   "$tServerStatut = $tRowStatus['" + tRelationStatus + "'];\n" +
                                   "$tServerHash = $tRowStatus['" + tRelationshipHash + "'];\n" +
                                   "$tServerCode = $tRowStatus['" + tRelationshipCode + "'];\n" +
                                   "$tServerLimitDayTime = $tRowStatus['" + tLimitDayTime + "'];\n" +
                                   "$tServerAccount = $tRowStatus['" + tAccount + "'];\n" +
                                   "$tServerGameSave = $tRowStatus['" + tGameSave + "'];\n" +
                                   "$tFriendLastSynchronization = $tRowStatus['" + tFriendLastSynchronization + "'];\n" +
                                   "$tFriendUserRelationShip = $tRowStatus['" + tFriendUserRelationShip + "'];\n" +
                                   "$tServerFriendAccount = $tRowStatus['" + tFriendAccount + "'];\n" +
                                   "$tServerFriendGameSave = $tRowStatus['" + tFriendGameSave + "'];\n" +
                                   "$tServerRelationPlace = $tRowStatus['" + tRelationPlace + "'];\n" +
                            "}\n" +
                       "}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
                "if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Valid).ToString() +
                " || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.CodeInvalid).ToString() +
                " || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.AllreadyFriend).ToString() +
                " || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.WaitingValidation).ToString() +
                " || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() +
                " || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Expired).ToString() +
                ")\n" +
                    "{\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                    "}\n" +

                // change the statut from CSV TO Waiting FRIENDS 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")\n" +
                    "{\n" +
                        "if ($tServerLimitDayTime< time())\n" +
                        "{\n" +
                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                        "$sReplaces[" + t_Index_RelationshipHash + "]= '' ;\n" +
                        "$sReplaces[" + t_Index_RelationshipCode + "]= '' ;\n" +
                        "$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;\n" +
                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                        "}\n" +
                        "else\n" +
                        "{\n" +

                        // TODO RETURN THE DEFAULT CLASSE TO RELATIONSHIP VISIBLE INFORMATIONS
                        // use $tServerRelationPlace
                        // get classes 

                        // use $tServerFriendAccount
                        // use $tServerFriendGameSave
                        // get datas from classe with this account 

                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                        "}\n" +
                    "}\n" +

                // change the statut from CSV TO GENERATE CODE 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.GenerateCode).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.None).ToString() + ")\n" +
                    "{\n" +
                            "$tQueryPlace = 'SELECT `" + tCodeLenght + "`, `" + tExpireTime + "` FROM `'.$ENV.'_" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "` " +
                            "WHERE `AC`= \\'1\\' " +
                            "AND `Reference` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                            "AND `WebModel` <= '.$WSBUILD.' " +
                            ";';\n" +
                            "$tResultPlace = $SQL_CON->query($tQueryPlace);\n" +
                            "if (!$tResultPlace)\n" +
                                "{\n" +
                                    "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                    "error('URSx33');\n" +
                                "}\n" +
                            "else\n" +
                                "{\n" +
                                    "if ($tResultPlace->num_rows == 1)\n" +
                                        "{\n" +
                                            "while($tRowPlace = $tResultPlace->fetch_assoc())\n" +
                                                "{\n" +
                                                    "$tCode = $tServerID;\n" +
                                                    "$tCode = $tServerID+CodeRandomSizable($tRowPlace['" + tCodeLenght + "']);\n" +
                                                    //"$tCode = UniquePropertyValueFromValue($ENV.'_" + BasisHelper().ClassNamePHP+", $sColumnOrign, $sColumUniqueResult, $tReference, $sNeverEmpty = true);\n" +
                                                    "$tLimitDayTime = $TIME_SYNC+$tRowPlace['" + tExpireTime + "'];\n" +
                                                    "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + ";\n" +
                                                    "$sReplaces[" + t_Index_RelationshipHash + "]= $tCode ;\n" +
                                                    "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                                    "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                                    "$sReplaces[" + t_Index_FriendAccount + "]= '' ;\n" +
                                                    "$sReplaces[" + t_Index_FriendGameSave + "]= '' ;\n" +
                                                    "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                                    "$sReplaces[" + t_Index_LimitDayTime + "]= $tLimitDayTime ;\n" +
                                                    "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                                "}\n" +
                                        "}\n" +
                                "else\n" +
                                        "{\n" +
                                            "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                            "$sReplaces[" + t_Index_RelationshipHash + "]= '' ;\n" +
                                            "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                            "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                            "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                            "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                            "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                            "$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;\n" +
                                            "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                        "}\n" +
                                "}\n" +
                    "}\n" +

                // change the statut from CSV TO INSERT CODE 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.InsertCode).ToString() + " && " +
                "$sCsvList[" + t_Index_RelationshipCode + "] != '' &&" +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.None).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryRequestor = 'SELECT `" + tAccount + "`, `" + tGameSave + "` FROM `'.$ENV.'_" + ClassNamePHP + "` " +
                        "WHERE " +
                        "`" + tRelationshipHash + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                        "AND `" + tRelationPlace + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                        "AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +

                        //"myLog(' search account and gamesave of this code  : '.$tQueryRequestor.'', __FILE__, __FUNCTION__, __LINE__);\n" +

                        "$tResultRequestor = $SQL_CON->query($tQueryRequestor);\n" +
                        "if (!$tResultRequestor)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryRequestor.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultRequestor->num_rows == 1)\n" +
                                    "{\n" +
                                        "while($tRowRequestor = $tResultRequestor->fetch_assoc())\n" +
                                            "{\n" +
                                                "$tAccountRequestor = $tRowRequestor['" + tAccount + "'];\n" +
                                                "$tGameSaveRequestor = $tRowRequestor['" + tGameSave + "'];\n" +
                                            "}\n" +
                                        "$tQueryFriend = 'SELECT `Reference` FROM `'.$ENV.'_" + ClassNamePHP + "` " +
                                        "WHERE " +
                                        "(" +
                                        "`AC`= \\'1\\' " +
                                        "AND `" + tFriendAccount + "` = \\''.$SQL_CON->real_escape_string($tAccountRequestor).'\\' " +
                                        "AND `" + tFriendGameSave + "` = \\''.$SQL_CON->real_escape_string($tGameSaveRequestor).'\\' " +
                                        "AND `" + tAccount + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                        "AND `" + tGameSave + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_GameSave + "]).'\\' " +
                                        "AND `" + tRelationPlace + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                        ")" +
                                        "OR " +
                                        "(" +
                                        "`AC`= \\'1\\' " +
                                        "AND `" + tAccount + "` = \\''.$SQL_CON->real_escape_string($tAccountRequestor).'\\' " +
                                        "AND `" + tGameSave + "` = \\''.$SQL_CON->real_escape_string($tGameSaveRequestor).'\\' " +
                                        "AND `" + tFriendAccount + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                        "AND `" + tFriendGameSave + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_GameSave + "]).'\\' " +
                                        "AND `" + tRelationPlace + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                        ")" +
                                        "';\n" +

                                        //"myLog(' search all ready friends  : '.$tQueryFriend.'', __FILE__, __FUNCTION__, __LINE__);\n" +

                                        "$tResultFriend = $SQL_CON->query($tQueryFriend);\n" +
                                        "if (!$tResultFriend)\n" +
                                            "{\n" +
                                                    "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryFriend.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                    "error('SERVER');\n" +
                                            "}\n" +
                                        "else" +
                                            "{\n" +
                                                "if ($tResultFriend->num_rows > 0)\n" +
                                                    "{\n" +
                                                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.AllreadyFriend).ToString() + ";\n" +
                                                        "$sReplaces[" + t_Index_RelationshipHash + "]= '' ;\n" +
                                                        "$sReplaces[" + t_Index_RelationshipCode + "]= 0 ;\n" +
                                                        "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                                        "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                                        "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                                        "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                                    "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "$tQueryCancelable = 'UPDATE `'.$ENV.'_" + ClassNamePHP + "` SET " +
                                                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                                                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                                                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                                                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + "\\', " +
                                                        "`" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\', " +
                                                        "`" + tRelationshipHash + "` = \\'\\', " +
                                                        "`" + tRelationshipCode + "` = \\'\\', " +
                                                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                                                        "`" + tFriendAccount + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\', " +
                                                        "`" + tFriendGameSave + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_GameSave + "]).'\\' " +
                                                        "WHERE " +
                                                        "`" + tRelationshipHash + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' " +
                                                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                                                        "AND `" + tRelationPlace + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                                        "AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                                                        "AND `AC` = 1 " +
                                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                                        ";';\n" +
                                                        "$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
                                                        "if (!$tResultCancelable)\n" +
                                                            "{\n" +
                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                "error('SERVER');\n" +
                                                            "}\n" +
                                                        "else" +
                                                            "{\n" +
                                                                "$tNumberOfRow = 0;\n" +
                                                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                                                "if ($tNumberOfRow == 1)\n" +
                                                                    "{\n" +
                                                                        "$tQueryIntegrity = 'SELECT `Reference` FROM `'.$ENV.'_" + ClassNamePHP + "` " +
                                                                        "WHERE " +
                                                                        "`" + tFriendAccount + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                                                        "AND `" + tFriendGameSave + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_GameSave + "]).'\\' " +
                                                                        "AND `" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                                                                        ";';\n" +
                                                                        "$tResultIntegrity = $SQL_CON->query($tQueryIntegrity);\n" +
                                                                        "if (!$tResultIntegrity)\n" +
                                                                            "{\n" +
                                                                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryIntegrity.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                                                "error('SERVER');\n" +
                                                                            "}\n" +
                                                                        "else\n" +
                                                                            "{\n" +
                                                                                "while($tRowIntegrity = $tResultIntegrity->fetch_row())\n" +
                                                                                    "{\n" +
                                                                                        "Integrity" + ClassNamePHP + "Reevalue($tRowIntegrity[0]);\n" +
                                                                                        "$sReplaces[" + t_Index_FriendUserRelationShip + "]= $tRowIntegrity[0];\n" +
                                                                                    "}\n" +
                                                                            "}\n" +
                                                                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + ";\n" +
                                                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                                                    "}\n" +
                                                                "else\n" +
                                                                    "{\n" +
                                                                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.CodeInvalid).ToString() + ";\n" +
                                                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                                                    "}\n" +
                                                            "}\n" +
                                                    "}\n" +
                                            "}\n" +
                                    "}\n" +
                                "else\n" +
                                    "{\n" +
                                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.CodeInvalid).ToString() + ";\n" +
                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                    "}\n" +
                            "}\n" +
                    "}\n" +

                // change the statut from CSV TO ACCEPT FRIENDS 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.AcceptFriend).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryAccept = 'UPDATE `'.$ENV.'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\', " +
                        "`" + tFriendGameSave + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + t_Index_GameSave + "]).'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultAccept = $SQL_CON->query($tQueryAccept);\n" +
                        "if (!$tResultAccept)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAccept.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Valid).ToString() + ";\n" +
                                        "$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue($tFriendUserRelationShip);\n" +
                                    "}\n" +
                                "else" +
                                    "{\n" +
                                        "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                        "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                        "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;\n" +
                                        "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                    "}\n" +
                            "}\n" +
                            "" +
                    "}\n" +

                // change the statut from CSV TO REFUSE FRIENDS 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.RefuseFriend).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryRefuse = 'UPDATE `'.$ENV.'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\', " +
                        "`" + tFriendGameSave + "` = \\'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = $SQL_CON->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryRefuse.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue($tFriendUserRelationShip);\n" +
                                    "}\n" +
                                "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;\n" +
                                "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                            "}\n" +
                        "" +
                    "}\n" +

                // change the statut from CSV TO DELETE FRIENDS 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Delete).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.Valid).ToString() + ")\n" +
                    "{\n" +
                        "$tQueryRefuse = 'UPDATE `'.$ENV.'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\', " +
                        "`" + tFriendGameSave + "` = \\'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = $SQL_CON->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryRefuse.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue($tFriendUserRelationShip);\n" +
                                    "}\n" +
                                "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;\n" +
                                "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                            "}\n" +
                        "" +
                    "}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.None).ToString() + " && (" +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.AllreadyFriend).ToString() +
                " || $tServerStatut == " + ((int)NWDRelationshipStatus.CodeInvalid).ToString() +
                " || $tServerStatut == " + ((int)NWDRelationshipStatus.Expired).ToString() +
                " || ($tServerStatut == " + ((int)NWDRelationshipStatus.Force).ToString() + " && $sAdmin == true)" +
                "))\n" +
                    "{\n" +
                        "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                        "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +


                // change the statut from waiting to expired if sync 
                "else if (($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Sync).ToString() + " OR $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")  && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + ")\n" +
                    "{\n" +
                        "myLog('###OK IS EXPIRED?###', __FILE__, __FUNCTION__, __LINE__);\n" +

                        "$tQueryRefuse = 'UPDATE `'.$ENV.'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''.$TIME_SYNC.'\\', " +
                        "`DS` = \\''.$TIME_SYNC.'\\', " +
                        "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\', " +
                        "`" + tFriendGameSave + "` = \\'\\' " +
                        "WHERE " +
                        "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                        "AND `" + tLimitDayTime + "` < '.$TIME_SYNC.' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = $SQL_CON->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryRefuse.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                                "if ($tNumberOfRow > 0)\n" +
                                    "{\n" +
                                        "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                        "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                        "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;\n" +
                                        "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                    "}\n" +
                                "else" +
                                    "{\n" +
                                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                                        "return;\n" +
                                    "}\n" +
                            "}\n" +
                        "" +
                    "}\n" +
                

                    // SYNC 
                "else if (($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Sync).ToString() + " OR $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")  && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.Valid).ToString() + ")\n" +
                    "{\n" +
                        // the last sync is  $tFriendLastSynchronization
                        "$tQueryPlace = 'SELECT `" + tClassesSharedToStartRelation + "`, `" + tClassesShared + "` FROM `'.$ENV.'_" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "` " +
                        "WHERE `AC`= \\'1\\' " +
                        "AND `Reference` = \\''.$SQL_CON->real_escape_string($tServerRelationPlace).'\\' " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        "';\n" +
                        "myLog('tQueryPlace : '.$tQueryPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                        "$tResultPlace = $SQL_CON->query($tQueryPlace);\n" +
                        "if (!$tResultPlace)\n" +
                            "{\n" +
                                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryPlace.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                                "error('SERVER');\n" +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultPlace->num_rows == 1)\n" +
                                    "{\n" +
                                        "$tRowPlace = $tResultPlace->fetch_assoc();\n" +
                                        "$tClassesSharedToStartRelation = $tRowPlace['" + tClassesSharedToStartRelation + "'];\n" +
                                        "$tClassesShared = $tRowPlace['" + tClassesShared + "'];\n" +
                                        "$tClasses = array_merge(explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesSharedToStartRelation), explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesShared));" +
                                        "$tClasses[] = '" + NWDGameSave.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDUserAvatar.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDUserNickname.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDUserInfos.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDAccountAvatar.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDAccountNickname.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses[] = '" + NWDAccountInfos.BasisHelper().ClassNamePHP + "';\n" +
                                        "$tClasses = array_unique ($tClasses);\n" +
                                        "if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")\n" +
                                        "{\n" +
                                        "$tFriendLastSynchronization=0;\n" +
                                        "}\n" +
                                        "foreach ($tClasses as $tClass)\n" +
                                            "{\n" +
                                                "myLog('will include : '.$tClass.'!', __FILE__, __FUNCTION__, __LINE__);\n" +
                                                "include_once ( $PATH_BASE.'/'.$ENV.'/" + NWD.K_DB + "/'.$tClass.'/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                                                "$tFunction = 'GetDatasByGameSave'.$tClass;\n" +
                                                "$tFunction($tFriendLastSynchronization, $tServerFriendAccount, $tServerFriendGameSave);\n" +
                                            "}\n" +
                                    "}\n" +
                                "else" +
                                    "{\n" +
                                    "}\n" +
                            "}\n" +
                            "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Valid).ToString() + ";\n" +
                            "$sReplaces[" + t_Index_FriendLastSynchronization + "]= $TIME_SYNC ;\n" +
                            "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // change the statut from CSV TO FORCE // ADMIN ONLY 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Force).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                    "//EXECEPTION FOR ADMIN\n" +
                    "}\n" +
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.ForceNone).ToString() + " && $sAdmin == true)\n" +
                    "{\n" +
                    "//EXECEPTION FOR ADMIN\n" +
                            "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.None).ToString() + ";\n" +
                            "$sReplaces[" + t_Index_RelationshipHash + "]= '' ;\n" +
                            "$sReplaces[" + t_Index_RelationshipCode + "]= '' ;\n" +
                            "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                            "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
                            "$sReplaces[" + t_Index_FriendGameSave + "] = '';\n" +
                            "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                            "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                            "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // OTHER
                "else\n" +
                      "{\n" +
                        "myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
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