﻿// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:51:1
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
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
public partial class NWDAccountRelationshipHelper : NWDHelper<NWDAccountRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {

            string tRelationStatus = NWDToolbox.PropertyName(() => FictiveData().RelationshipStatus);

            string tRelationPlace = NWDToolbox.PropertyName(() => FictiveData().RelationPlace);
            string tRelationshipHash = NWDToolbox.PropertyName(() => FictiveData().RelationshipHash);
            string tRelationshipCode = NWDToolbox.PropertyName(() => FictiveData().RelationshipCode);
            string tLimitDayTime = NWDToolbox.PropertyName(() => FictiveData().LimitDayTime);
            string tFriendLastSynchronization = NWDToolbox.PropertyName(() => FictiveData().FriendLastSynchronization);

            string tFriendAccount = NWDToolbox.PropertyName(() => FictiveData().FriendAccount);

            string tAccount = NWDToolbox.PropertyName(() => FictiveData().Account);
            string tFriendUserRelationShip = NWDToolbox.PropertyName(() => FictiveData().FriendUserRelationShip);

            //string tRelationStatus = FindAliasName("RelationshipStatus");
            //string tRelationPlace = FindAliasName("RelationPlace");
            //string tRelationshipHash = FindAliasName("RelationshipHash");
            //string tRelationshipCode = FindAliasName("RelationshipCode");
            //string tLimitDayTime = FindAliasName("LimitDayTime");
            //string tFriendLastSynchronization = FindAliasName("FriendLastSynchronization");

            //string tFriendAccount = FindAliasName("FriendAccount");

            //string tAccount = FindAliasName("Account");
            //string tFriendUserRelationShip = FindAliasName("FriendUserRelationShip");

            int t_Index_RelationStatus = New_CSV_IndexOf(tRelationStatus);
            int t_Index_RelationPlace = New_CSV_IndexOf(tRelationPlace);
            int t_Index_RelationshipHash = New_CSV_IndexOf(tRelationshipHash);
            int t_Index_RelationshipCode = New_CSV_IndexOf(tRelationshipCode);
            int t_Index_LimitDayTime = New_CSV_IndexOf(tLimitDayTime);
            int t_Index_FriendLastSynchronization = New_CSV_IndexOf(tFriendLastSynchronization);

            int t_Index_FriendAccount = New_CSV_IndexOf(tFriendAccount);

            int t_Index_Account = New_CSV_IndexOf(tAccount);
            int t_Index_FriendUserRelationShip = New_CSV_IndexOf(tFriendUserRelationShip);

            string tCodeLenght = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().CodeLenght);

            string tExpireTime = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ExpireTime);
            string tClassesSharedToStartRelation = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesSharedToStartRelation);
            string tClassesShared = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesShared);

            //string tCodeLenght = NWDRelationshipPlace.FindAliasName("CodeLenght");

            //string tExpireTime = NWDRelationshipPlace.FindAliasName("ExpireTime");
            //string tClassesSharedToStartRelation = NWDRelationshipPlace.FindAliasName("ClassesSharedToStartRelation");
            //string tClassesShared = NWDRelationshipPlace.FindAliasName("ClassesShared");

            string sScript = "" +
                "// start Addon \n" +
                "include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDAccountNickname.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDAccountAvatar.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                "include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDAccountInfos.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tServerCode = '';\n" +
                "$tServerLimitDayTime = '';\n" +
                "$tFriendLastSynchronization = 0;\n" +
                "$tServerAccount = '';\n" +
                "$tServerFriendAccount = '';\n" +
                "$tServerID = '';\n" +
                "$tServerRelationPlace = '';\n" +
                "$tFriendUserRelationShip = 0;\n" +
                "$tQueryStatus = 'SELECT `ID`, `" + tRelationStatus + "`," +
                " `" + tRelationshipHash + "`," +
                " `" + tRelationshipCode + "`," +
                " `" + tRelationPlace + "`," +
                " `" + tLimitDayTime + "`," +
                " `" + tAccount + "`," +
                " `" + tFriendAccount + "`," +
                " `" + tFriendLastSynchronization + "`," +
                " `" + tFriendUserRelationShip + "" +
                "` FROM `'." + NWD.K_ENV + ".'_" + ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `Reference` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' " +
                "AND `WebModel` <= '.$WSBUILD.' " +
                "';" +
                "$tResultStatus = " + NWD.K_SQL_CON + "->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                    "{\n" +
                        NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryStatus") +
                        NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP)+
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
                                   "$tFriendLastSynchronization = $tRowStatus['" + tFriendLastSynchronization + "'];\n" +
                                   "$tFriendUserRelationShip = $tRowStatus['" + tFriendUserRelationShip + "'];\n" +
                                   "$tServerFriendAccount = $tRowStatus['" + tFriendAccount + "'];\n" +
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

                        //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                        "GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
                        "return;\n" +
                        "}\n" +
                    "}\n" +

                // change the statut from CSV TO GENERATE CODE 
                "else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.GenerateCode).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDRelationshipStatus.None).ToString() + ")\n" +
                    "{\n" +
                            "$tQueryPlace = 'SELECT `" + tCodeLenght + "`, `" + tExpireTime + "` FROM `'."+NWD.K_ENV+".'_" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "` " +
                            "WHERE `AC`= \\'1\\' " +
                            "AND `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                            "AND `WebModel` <= '.$WSBUILD.' " +
                            ";';\n" +
                            "$tResultPlace = "+NWD.K_SQL_CON+"->query($tQueryPlace);\n" +
                            "if (!$tResultPlace)\n" +
                                "{\n" +
                                    NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace") +
                                    NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                                "}\n" +
                            "else\n" +
                                "{\n" +
                                    "if ($tResultPlace->num_rows == 1)\n" +
                                        "{\n" +
                                            "while($tRowPlace = $tResultPlace->fetch_assoc())\n" +
                                                "{\n" +
                                                    "$tCode = $tServerID;\n" +
                                                    "$tCode = $tServerID+CodeRandomSizable($tRowPlace['" + tCodeLenght + "']);\n" +
                                                    //"$tCode = UniquePropertyValueFromValue("+NWD.K_ENV+".'_" + BasisHelper().ClassNamePHP+", $sColumnOrign, $sColumUniqueResult, $tReference, $sNeverEmpty = true);\n" +
                                                    "$tLimitDayTime = "+NWD.K_PHP_TIME_SYNC+"+$tRowPlace['" + tExpireTime + "'];\n" +
                                                    "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + ";\n" +
                                                    "$sReplaces[" + t_Index_RelationshipHash + "]= $tCode ;\n" +
                                                    "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                                    "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                                    "$sReplaces[" + t_Index_FriendAccount + "]= '' ;\n" +
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
                        "$tQueryRequestor = 'SELECT `" + tAccount + "` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
                        "WHERE " +
                        "`" + tRelationshipHash + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                        "AND `" + tRelationPlace + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                        "AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRequestor = "+NWD.K_SQL_CON+"->query($tQueryRequestor);\n" +
                        "if (!$tResultRequestor)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRequestor") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultRequestor->num_rows == 1)\n" +
                                    "{\n" +
                                        "while($tRowRequestor = $tResultRequestor->fetch_assoc())\n" +
                                            "{\n" +
                                                "$tAccountRequestor = $tRowRequestor['" + tAccount + "'];\n" +
                                            "}\n" +
                                        "$tQueryFriend = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
                                        "WHERE " +
                                        "(" +
                                        "`AC`= \\'1\\' " +
                                        "AND `" + tFriendAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tAccountRequestor).'\\' " +
                                        "AND `" + tAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                        "AND `" + tRelationPlace + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                        ")" +
                                        "OR " +
                                        "(" +
                                        "`AC`= \\'1\\' " +
                                        "AND `" + tAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tAccountRequestor).'\\' " +
                                        "AND `" + tFriendAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                        "AND `" + tRelationPlace + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                        ")" +
                                        "';\n" +
                                        "$tResultFriend = "+NWD.K_SQL_CON+"->query($tQueryFriend);\n" +
                                        "if (!$tResultFriend)\n" +
                                            "{\n" +
                                                    NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryFriend") +
                                                    NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
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
                                                        "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                                                        "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                                                        "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                                                    "}\n" +
                                                "else" +
                                                    "{\n" +
                                                        "$tQueryCancelable = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                                                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                                                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + "\\', " +
                                                        "`" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\', " +
                                                        "`" + tRelationshipHash + "` = \\'\\', " +
                                                        "`" + tRelationshipCode + "` = \\'\\', " +
                                                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                                                        "`" + tFriendAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                                        "WHERE " +
                                                        "`" + tRelationshipHash + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' " +
                                                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                                                        "AND `" + tRelationPlace + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' " +
                                                        "AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                                                        "AND `AC` = 1 " +
                                                        "AND `WebModel` <= '.$WSBUILD.' " +
                                                        ";';\n" +
                                                        "$tResultCancelable = "+NWD.K_SQL_CON+"->query($tQueryCancelable);\n" +
                                                        "if (!$tResultCancelable)\n" +
                                                            "{\n" +
                                                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryCancelable") +
                                                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                                                            "}\n" +
                                                        "else" +
                                                            "{\n" +
                                                                "$tNumberOfRow = 0;\n" +
                                                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                                                "if ($tNumberOfRow == 1)\n" +
                                                                    "{\n" +
                                                                        "$tQueryIntegrity = 'SELECT `Reference` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
                                                                        "WHERE " +
                                                                        "`" + tFriendAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                                                                        "AND `" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                                                                        ";';\n" +
                                                                        "$tResultIntegrity = "+NWD.K_SQL_CON+"->query($tQueryIntegrity);\n" +
                                                                        "if (!$tResultIntegrity)\n" +
                                                                            "{\n" +
                                                                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryIntegrity") +
                                                                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
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
                        "$tQueryAccept = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultAccept = "+NWD.K_SQL_CON+"->query($tQueryAccept);\n" +
                        "if (!$tResultAccept)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryAccept") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
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
                        "$tQueryRefuse = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = "+NWD.K_SQL_CON+"->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue($tFriendUserRelationShip);\n" +
                                    "}\n" +
                                "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
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
                        "$tQueryRefuse = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\' " +
                        "WHERE " +
                        "`" + tFriendUserRelationShip + "` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\' " +
                        //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = "+NWD.K_SQL_CON+"->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow == 1)\n" +
                                    "{\n" +
                                        "Integrity" + ClassNamePHP + "Reevalue($tFriendUserRelationShip);\n" +
                                    "}\n" +
                                "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
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
                        "$tQueryRefuse = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
                        "`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
                        "`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', " +
                        "`" + tFriendUserRelationShip + "` = \\'\\', " +
                        "`" + tRelationshipHash + "` = \\'\\', " +
                        "`" + tRelationshipCode + "` = \\'\\', " +
                        "`" + tFriendLastSynchronization + "` = \\'0\\', " +
                        "`" + tFriendAccount + "` = \\'\\' " +
                        "WHERE " +
                        "`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
                        "AND `" + tLimitDayTime + "` < '."+NWD.K_PHP_TIME_SYNC+".' " +
                        "AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' " +
                        "AND `AC` = 1 " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        ";';\n" +
                        "$tResultRefuse = "+NWD.K_SQL_CON+"->query($tQueryRefuse);\n" +
                        "if (!$tResultRefuse)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "$tNumberOfRow = 0;\n" +
                                "$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
                                "if ($tNumberOfRow > 0)\n" +
                                    "{\n" +
                                        "$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";\n" +
                                        "$sReplaces[" + t_Index_RelationshipHash + "] = '';\n" +
                                        "$sReplaces[" + t_Index_RelationshipCode + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';\n" +
                                        "$sReplaces[" + t_Index_FriendAccount + "] = '';\n" +
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
                        "$tQueryPlace = 'SELECT `" + tClassesSharedToStartRelation + "`, `" + tClassesShared + "` FROM `'."+NWD.K_ENV+".'_" + NWDRelationshipPlace.BasisHelper().ClassNamePHP + "` " +
                        "WHERE `AC`= \\'1\\' " +
                        "AND `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tServerRelationPlace).'\\' " +
                        "AND `WebModel` <= '.$WSBUILD.' " +
                        "';\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace") +
                        "$tResultPlace = "+NWD.K_SQL_CON+"->query($tQueryPlace);\n" +
                        "if (!$tResultPlace)\n" +
                            "{\n" +
                                NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace") +
                                NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP) +
                            "}\n" +
                        "else" +
                            "{\n" +
                                "if ($tResultPlace->num_rows == 1)\n" +
                                    "{\n" +
                                        "$tRowPlace = $tResultPlace->fetch_assoc();\n" +
                                        "$tClassesSharedToStartRelation = $tRowPlace['" + tClassesSharedToStartRelation + "'];\n" +
                                        "$tClassesShared = $tRowPlace['" + tClassesShared + "'];\n" +
                                        "$tClasses = array_merge(explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesSharedToStartRelation), explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesShared));" +
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
                                                "include_once ( "+NWD.K_PATH_BASE+".'/'."+NWD.K_ENV+".'/" + NWD.K_DB + "/'.$tClass.'/" + NWD.K_WS_SYNCHRONISATION + "');\n" +
                                                "$tFunction = 'GetDatas'.$tClass;\n" +
                                                "$tFunction($tFriendLastSynchronization, $tServerFriendAccount);\n" +
                                            "}\n" +
                                    "}\n" +
                                "else" +
                                    "{\n" +
                                    "}\n" +
                            "}\n" +
                            "$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Valid).ToString() + ";\n" +
                            "$sReplaces[" + t_Index_FriendLastSynchronization + "]= "+NWD.K_PHP_TIME_SYNC+" ;\n" +
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
                            "$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;\n" +
                            "$sReplaces[" + t_Index_LimitDayTime + "] = 0;\n" +
                            "$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                    "}\n" +

                // OTHER
                "else\n" +
                      "{\n" +
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