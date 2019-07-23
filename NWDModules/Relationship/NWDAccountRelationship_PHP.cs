//=====================================================================================================================
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
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tWebModel = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().WebModel);
            string tID = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().ID);
            string tAC = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().AC);
            string tDM = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().DM);
            string tDS = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().DS);
            string tReference = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().Reference);
            string tEnvSync = PHP_ENV_SYNC(sEnvironment);

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

            int t_Index_RelationStatus = CSV_IndexOf(tRelationStatus);
            int t_Index_RelationPlace = CSV_IndexOf(tRelationPlace);
            int t_Index_RelationshipHash = CSV_IndexOf(tRelationshipHash);
            int t_Index_RelationshipCode = CSV_IndexOf(tRelationshipCode);
            int t_Index_LimitDayTime = CSV_IndexOf(tLimitDayTime);
            int t_Index_FriendLastSynchronization = CSV_IndexOf(tFriendLastSynchronization);

            int t_Index_FriendAccount = CSV_IndexOf(tFriendAccount);

            int t_Index_Account = CSV_IndexOf(tAccount);
            int t_Index_FriendUserRelationShip = CSV_IndexOf(tFriendUserRelationShip);

            string tCodeLenght = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().CodeLenght);

            string tExpireTime = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ExpireTime);
            string tClassesSharedToStartRelation = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesSharedToStartRelation);
            string tClassesShared = NWDToolbox.PropertyName(() => NWDRelationshipPlace.FictiveData().ClassesShared);

            //string tCodeLenght = NWDRelationshipPlace.FindAliasName("CodeLenght");
            //string tExpireTime = NWDRelationshipPlace.FindAliasName("ExpireTime");
            //string tClassesSharedToStartRelation = NWDRelationshipPlace.FindAliasName("ClassesSharedToStartRelation");
            //string tClassesShared = NWDRelationshipPlace.FindAliasName("ClassesShared");

            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("include_once ( " + NWDRelationshipPlace.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("include_once ( " + NWDAccountNickname.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("include_once ( " + NWDAccountAvatar.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine("include_once ( " + NWDAccountInfos.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            // get the actual state
            rReturn.AppendLine("$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";");
            rReturn.AppendLine("$tServerHash = '';");
            rReturn.AppendLine("$tServerCode = '';");
            rReturn.AppendLine("$tServerLimitDayTime = '';");
            rReturn.AppendLine("$tFriendLastSynchronization = 0;");
            rReturn.AppendLine("$tServerAccount = '';");
            rReturn.AppendLine("$tServerFriendAccount = '';");
            rReturn.AppendLine("$tServerID = '';");
            rReturn.AppendLine("$tServerRelationPlace = '';");
            rReturn.AppendLine("$tFriendUserRelationShip = 0;");
            rReturn.Append("$tQueryStatus = 'SELECT `" + tID + "`, `" + tRelationStatus + "`,");
            rReturn.Append(" `" + tRelationshipHash + "`,");
            rReturn.Append(" `" + tRelationshipCode + "`,");
            rReturn.Append(" `" + tRelationPlace + "`,");
            rReturn.Append(" `" + tLimitDayTime + "`,");
            rReturn.Append(" `" + tAccount + "`,");
            rReturn.Append(" `" + tFriendAccount + "`,");
            rReturn.Append(" `" + tFriendLastSynchronization + "`,");
            rReturn.Append(" `" + tFriendUserRelationShip + "");
            rReturn.Append("` FROM `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultStatus = " + NWD.K_SQL_CON + "->query($tQueryStatus);");
            rReturn.AppendLine("if (!$tResultStatus)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryStatus"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultStatus->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowStatus = $tResultStatus->fetch_assoc();");
            rReturn.AppendLine("$tServerID = $tRowStatus['" + tID + "'];");
            rReturn.AppendLine("$tServerStatut = $tRowStatus['" + tRelationStatus + "'];");
            rReturn.AppendLine("$tServerHash = $tRowStatus['" + tRelationshipHash + "'];");
            rReturn.AppendLine("$tServerCode = $tRowStatus['" + tRelationshipCode + "'];");
            rReturn.AppendLine("$tServerLimitDayTime = $tRowStatus['" + tLimitDayTime + "'];");
            rReturn.AppendLine("$tServerAccount = $tRowStatus['" + tAccount + "'];");
            rReturn.AppendLine("$tFriendLastSynchronization = $tRowStatus['" + tFriendLastSynchronization + "'];");
            rReturn.AppendLine("$tFriendUserRelationShip = $tRowStatus['" + tFriendUserRelationShip + "'];");
            rReturn.AppendLine("$tServerFriendAccount = $tRowStatus['" + tFriendAccount + "'];");
            rReturn.AppendLine("$tServerRelationPlace = $tRowStatus['" + tRelationPlace + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");

            // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
            rReturn.AppendLine("if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Valid).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.CodeInvalid).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.AllreadyFriend).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.WaitingValidation).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString());
            rReturn.AppendLine(" || $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Expired).ToString());
            rReturn.AppendLine(")");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");

            // change the statut from CSV TO Waiting FRIENDS 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tServerLimitDayTime< time())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");

            // TODO RETURN THE DEFAULT CLASSE TO RELATIONSHIP VISIBLE INFORMATIONS

            //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");

            // change the statut from CSV TO GENERATE CODE 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.GenerateCode).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryPlace = 'SELECT `" + tCodeLenght + "`, `" + tExpireTime + "` FROM `" + NWDRelationshipPlace.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultPlace = " + NWD.K_SQL_CON + "->query($tQueryPlace);");
            rReturn.AppendLine("if (!$tResultPlace)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultPlace->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowPlace = $tResultPlace->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tCode = $tServerID;");
            rReturn.AppendLine("$tCode = $tServerID + CodeRandomSizable($tRowPlace['" + tCodeLenght + "']);");
            //"$tCode = UniquePropertyValueFromValue("+NWD.K_ENV+".'_" + BasisHelper().ClassNamePHP+", $sColumnOrign, $sColumUniqueResult, $tReference, $sNeverEmpty = true);");
            rReturn.AppendLine("$tLimitDayTime = " + NWD.K_PHP_TIME_SYNC + "+$tRowPlace['" + tExpireTime + "'];");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "]= $tCode ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "]= $tLimitDayTime ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");

            // change the statut from CSV TO INSERT CODE 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.InsertCode).ToString() + " && ");
            rReturn.AppendLine("$sCsvList[" + t_Index_RelationshipCode + "] != '' &&");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.None).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryRequestor = 'SELECT `" + tAccount + "` FROM `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tRelationshipHash + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' ");
            rReturn.Append("AND `" + tRelationPlace + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' ");
            rReturn.Append("AND `" + tLimitDayTime + "` > '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultRequestor = " + NWD.K_SQL_CON + "->query($tQueryRequestor);");
            rReturn.AppendLine("if (!$tResultRequestor)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRequestor"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultRequestor->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowRequestor = $tResultRequestor->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tAccountRequestor = $tRowRequestor['" + tAccount + "'];");
            rReturn.AppendLine("}");
            rReturn.Append("$tQueryFriend = 'SELECT `" + tReference + "` FROM `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.Append("(");
            rReturn.Append("`" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tFriendAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tAccountRequestor).'\\' ");
            rReturn.Append("AND `" + tAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' ");
            rReturn.Append("AND `" + tRelationPlace + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.Append(")");
            rReturn.Append("OR ");
            rReturn.Append("(");
            rReturn.Append("`" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tAccountRequestor).'\\' ");
            rReturn.Append("AND `" + tFriendAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' ");
            rReturn.Append("AND `" + tRelationPlace + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.Append(")");
            rReturn.AppendLine("';");
            rReturn.AppendLine("$tResultFriend = " + NWD.K_SQL_CON + "->query($tQueryFriend);");
            rReturn.AppendLine("if (!$tResultFriend)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryFriend"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultFriend->num_rows > 0)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.AllreadyFriend).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryCancelable = 'UPDATE `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + "\\', ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\', ");
            rReturn.Append("`" + tRelationshipHash + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipCode + "` = \\'\\', ");
            rReturn.Append("`" + tFriendLastSynchronization + "` = \\'0\\', ");
            rReturn.Append("`" + tFriendAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tRelationshipHash + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationshipCode + "]).'\\' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' ");
            rReturn.Append("AND `" + tRelationPlace + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_RelationPlace + "]).'\\' ");
            rReturn.Append("AND `" + tLimitDayTime + "` > '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultCancelable = " + NWD.K_SQL_CON + "->query($tQueryCancelable);");
            rReturn.AppendLine("if (!$tResultCancelable)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryCancelable"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryIntegrity = 'SELECT `" + tReference + "` FROM `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tFriendAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' ");
            rReturn.Append("AND `" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultIntegrity = " + NWD.K_SQL_CON + "->query($tQueryIntegrity);");
            rReturn.AppendLine("if (!$tResultIntegrity)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryIntegrity"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowIntegrity = $tResultIntegrity->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tRowIntegrity[0]);");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "]= $tRowIntegrity[0];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.CodeInvalid).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.CodeInvalid).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");

            // change the statut from CSV TO ACCEPT FRIENDS 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.AcceptFriend).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryAccept = 'UPDATE `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\', ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\', ");
            rReturn.Append("`" + tRelationshipHash + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipCode + "` = \\'\\', ");
            rReturn.Append("`" + tFriendLastSynchronization + "` = \\'0\\', ");
            rReturn.Append("`" + tFriendAccount + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + t_Index_Account + "]).'\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' ");
            //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultAccept = " + NWD.K_SQL_CON + "->query($tQueryAccept);");
            rReturn.AppendLine("if (!$tResultAccept)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryAccept"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Valid).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "]= 0 ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tFriendUserRelationShip);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("");
            rReturn.AppendLine("}");

            // change the statut from CSV TO REFUSE FRIENDS 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.RefuseFriend).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.ProposeFriend).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryRefuse = 'UPDATE `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipHash + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipCode + "` = \\'\\', ");
            rReturn.Append("`" + tFriendLastSynchronization + "` = \\'0\\', ");
            rReturn.Append("`" + tFriendAccount + "` = \\'\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingValidation).ToString() + "\\' ");
            //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultRefuse = " + NWD.K_SQL_CON + "->query($tQueryRefuse);");
            rReturn.AppendLine("if (!$tResultRefuse)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tFriendUserRelationShip);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("");
            rReturn.AppendLine("}");

            // change the statut from CSV TO DELETE FRIENDS 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Delete).ToString() + " && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.Valid).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryRefuse = 'UPDATE `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipHash + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipCode + "` = \\'\\', ");
            rReturn.Append("`" + tFriendLastSynchronization + "` = \\'0\\', ");
            rReturn.Append("`" + tFriendAccount + "` = \\'\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Valid).ToString() + "\\' ");
            //"AND `" + tLimitDayTime + "` > '."+NWD.K_PHP_TIME_SYNC+".' " +
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultRefuse = " + NWD.K_SQL_CON + "->query($tQueryRefuse);");
            rReturn.AppendLine("if (!$tResultRefuse)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tFriendUserRelationShip);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("");
            rReturn.AppendLine("}");

            // change the statut from CSV TO NONE 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.None).ToString() + " && (");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.AllreadyFriend).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDRelationshipStatus.CodeInvalid).ToString());
            rReturn.AppendLine(" || $tServerStatut == " + ((int)NWDRelationshipStatus.Expired).ToString());
            rReturn.AppendLine(" || ($tServerStatut == " + ((int)NWDRelationshipStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");


            // change the statut from waiting to expired if sync 
            rReturn.AppendLine("else if (($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Sync).ToString() + " OR $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")  && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.Append("$tQueryRefuse = 'UPDATE `" + NWDAccountRelationship.TableNamePHP(sEnvironment) + "` SET ");
            rReturn.Append("`" + tDM + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tDS + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tEnvSync + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', ");
            rReturn.Append("`" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.Expired).ToString() + "\\', ");
            rReturn.Append("`" + tFriendUserRelationShip + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipHash + "` = \\'\\', ");
            rReturn.Append("`" + tRelationshipCode + "` = \\'\\', ");
            rReturn.Append("`" + tFriendLastSynchronization + "` = \\'0\\', ");
            rReturn.Append("`" + tFriendAccount + "` = \\'\\' ");
            rReturn.Append("WHERE ");
            rReturn.Append("`" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            rReturn.Append("AND `" + tLimitDayTime + "` < '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append("AND `" + tRelationStatus + "` = \\'" + ((int)NWDRelationshipStatus.WaitingFriend).ToString() + "\\' ");
            rReturn.Append("AND `" + tAC + "` = 1 ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine(";';");
            rReturn.AppendLine("$tResultRefuse = " + NWD.K_SQL_CON + "->query($tQueryRefuse);");
            rReturn.AppendLine("if (!$tResultRefuse)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryRefuse"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tNumberOfRow = 0;");
            rReturn.AppendLine("$tNumberOfRow = " + NWD.K_SQL_CON + "->affected_rows;");
            rReturn.AppendLine("if ($tNumberOfRow > 0)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "] = " + ((int)NWDRelationshipStatus.Expired).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "] = 0;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("");
            rReturn.AppendLine("}");


            // SYNC 
            rReturn.AppendLine("else if (($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Sync).ToString() + " OR $sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")  && ");
            rReturn.AppendLine("$tServerStatut == " + ((int)NWDRelationshipStatus.Valid).ToString() + ")");
            rReturn.AppendLine("{");
            // the last sync is  $tFriendLastSynchronization
            rReturn.Append("$tQueryPlace = 'SELECT `" + tClassesSharedToStartRelation + "`, `" + tClassesShared + "` FROM `" + NWDRelationshipPlace.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tReference + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tServerRelationPlace).'\\' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine("';");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace"));
            rReturn.AppendLine("$tResultPlace = " + NWD.K_SQL_CON + "->query($tQueryPlace);");
            rReturn.AppendLine("if (!$tResultPlace)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryPlace"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER, ClassNamePHP));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("if ($tResultPlace->num_rows == 1)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowPlace = $tResultPlace->fetch_assoc();");
            rReturn.AppendLine("$tClassesSharedToStartRelation = $tRowPlace['" + tClassesSharedToStartRelation + "'];");
            rReturn.AppendLine("$tClassesShared = $tRowPlace['" + tClassesShared + "'];");
            rReturn.AppendLine("$tClasses = array_merge(explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesSharedToStartRelation), explode('" + NWDConstants.kFieldSeparatorA + "',$tClassesShared));");
            rReturn.AppendLine("$tClasses[] = '" + NWDAccountAvatar.BasisHelper().ClassNamePHP + "';");
            rReturn.AppendLine("$tClasses[] = '" + NWDAccountNickname.BasisHelper().ClassNamePHP + "';");
            rReturn.AppendLine("$tClasses[] = '" + NWDAccountInfos.BasisHelper().ClassNamePHP + "';");
            rReturn.AppendLine("$tClasses = array_unique ($tClasses);");
            rReturn.AppendLine("if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.SyncForce).ToString() + ")");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tFriendLastSynchronization=0;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("foreach ($tClasses as $tClass)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/'.$tClass.'/" + NWD.K_WS_SYNCHRONISATION + "');");
            //"$tFunction = 'GetDatas'.$tClass;"); 
            rReturn.AppendLine("$tFunction = '" + PHP_FUNCTION_GET_DATAS().Replace(ClassNamePHP, "'.$tClass.'") + "';");
            rReturn.AppendLine("$tFunction($tFriendLastSynchronization, $tServerFriendAccount);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.Valid).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= " + NWD.K_PHP_TIME_SYNC + " ;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");

            // change the statut from CSV TO FORCE // ADMIN ONLY 
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.Force).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//EXECEPTION FOR ADMIN");
            rReturn.AppendLine("}");
            rReturn.AppendLine("else if ($sCsvList[" + t_Index_RelationStatus + "] == " + ((int)NWDRelationshipStatus.ForceNone).ToString() + " && $sAdmin == true)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//EXECEPTION FOR ADMIN");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationStatus + "]=" + ((int)NWDRelationshipStatus.None).ToString() + ";");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipHash + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_RelationshipCode + "]= '' ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendUserRelationShip + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendAccount + "] = '';");
            rReturn.AppendLine("$sReplaces[" + t_Index_FriendLastSynchronization + "]= 0 ;");
            rReturn.AppendLine("$sReplaces[" + t_Index_LimitDayTime + "] = 0;");
            rReturn.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvList, $sReplaces);");
            rReturn.AppendLine("}");

            // OTHER
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($tReference);");
            rReturn.AppendLine("return;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("// finish Addon");

            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif