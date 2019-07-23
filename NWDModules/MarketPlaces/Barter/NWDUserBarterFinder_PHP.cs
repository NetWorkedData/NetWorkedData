//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterFinderHelper : NWDHelper<NWDUserBarterFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            string tWebModel = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().WebModel);
            string tAC = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().AC);
            string tReference = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().Reference);
            string tAccount = NWDToolbox.PropertyName(() => NWDUserBarterRequest.FictiveData().Account);

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

            int tIndex_tBarterStatus = NWDUserBarterRequest.CSV_IndexOf(tBarterStatus);

            int tIndex_BarterRequestsList =  CSV_IndexOf(t_THIS_BarterRequestsList);
            int tIndex_BarterPlace =  CSV_IndexOf(t_THIS_BarterPlace);
            int tIndex_THIS_ForRelationshipOnly =  CSV_IndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 300; // minutes before stop to get the datas!

            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("include_once(" + NWDUserBarterRequest.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.Append("$tQueryExpired = 'SELECT " + NWDUserBarterRequest.SLQSelect() + " FROM `" + NWDUserBarterRequest.TableNamePHP(sEnvironment) + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append("AND `" + tLimitDayTime + "` < '." + NWD.K_PHP_TIME_SYNC + ".' ");
            rReturn.Append("AND `" + tWebModel + "` <= '.$WSBUILD.' ");
            rReturn.AppendLine("LIMIT 0, 100;';");
            rReturn.AppendLine("$tResultExpired = " + NWD.K_SQL_CON + "->query($tQueryExpired);");
            rReturn.AppendLine("if (!$tResultExpired)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryExpired"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowExpired = $tResultExpired->fetch_row())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tRowExpired = Integrity" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "Replace ($tRowExpired," + tIndex_tBarterStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");");
            rReturn.AppendLine("$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);");
            rReturn.AppendLine("UpdateData" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + " ($tRowExpired, " + NWD.K_PHP_TIME_SYNC + ", $uuid, false);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.Append("$tQueryBarter = 'SELECT `" + tReference + "` FROM `'." + NWD.K_ENV + ".'_" + NWDUserBarterRequest.BasisHelper().ClassNamePHP + "` ");
            rReturn.Append("WHERE `" + tAC + "`= \\'1\\' ");
            rReturn.Append("AND `" + tAccount + "` != \\''." + NWD.K_SQL_CON + "->real_escape_string($uuid).'\\' ");
            rReturn.Append("AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' ");
            rReturn.Append("AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' ");
            rReturn.AppendLine("AND `" + t_THIS_MaxPropositions + "` > `" + t_THIS_PropositionsCounter + "` ';");
            rReturn.AppendLine("if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tQueryBarter.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$tQueryBarter.= ' AND `" + tBarterPlaceRequest + "` = \\''.$sCsvList[" + tIndex_BarterPlace + "].'\\' AND `" + tLimitDayTime + "` > '.(" + NWD.K_PHP_TIME_SYNC + "+" + (tDelayOfRefresh).ToString() + ").' LIMIT 0, 100;';");
            rReturn.AppendLine("$tResultBarter = " + NWD.K_SQL_CON + "->query($tQueryBarter);");
            rReturn.AppendLine("$tReferences = \'\';");
            rReturn.AppendLine("$tReferencesList = \'\';");
            rReturn.AppendLine("if (!$tResultBarter)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tResultBarter"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRowBarter = $tResultBarter->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tReferences[]=$tRowBarter['" + tReference + "'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("if (is_array($tReferences))");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "', $tReferences);");
            rReturn.AppendLine("include_once ( " + NWDUserBarterRequest.BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            rReturn.AppendLine(NWDUserBarterRequest.BasisHelper().PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ($tReferences);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$sCsvList = " + NWDUserBarterFinder.BasisHelper().PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + tIndex_BarterRequestsList.ToString() + ", $tReferencesList);");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif