//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:15
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
using BasicToolBox;
using UnityEditor;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDIPBan.New_PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBan : NWDBasis<NWDIPBan>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string New_PhpEngine(NWDAppEnvironment sEnvironment)
        {
            
            string tEnvSync = BasisHelper().PHP_ENV_SYNC(sEnvironment);

            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + BasisHelper().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + BasisHelper().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            //tFile.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDIPBan.BasisHelper().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function IPBanOk()");
            tFile.AppendLine("{");
            if (sEnvironment.IPBanActive == true)
            {
            tFile.AppendLine("$tIPBanMaxTentative = " + sEnvironment.IPBanMaxTentative.ToString() + ";");
            tFile.AppendLine("$tIPBanTimer = " + sEnvironment.IPBanTimer.ToString() + ";");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Reference) + "` FROM `" + TableNamePHP(sEnvironment) + "` WHERE ");
            tFile.Append("\\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().IP) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Counter) + "` >= `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().CounterMaximum) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Deadline) + "` >= '.$SQL_CON->real_escape_string(" + NWD.K_PHP_TIME_SYNC + ").' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC90));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_log(sEnvironment, "YOU ARE BANNED!!!"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_IPB01));
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            }
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function IPBanAdd()");
            tFile.AppendLine("{");
            if (sEnvironment.IPBanActive == true)
            {
            tFile.AppendLine("global $SQL_CON;");
            tFile.AppendLine("global " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine("$tIPBanMaxTentative = " + sEnvironment.IPBanMaxTentative.ToString() + ";");
            tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Reference) + "` FROM `" + TableNamePHP(sEnvironment) + "` WHERE");
            tFile.Append(" \\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().IP) + "` AND");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");

            tFile.Append("$tQuerySelect = 'SELECT `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Reference) + "` FROM `" + TableNamePHP(sEnvironment) + "` WHERE ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Deadline) + "` < '.$SQL_CON->real_escape_string(" + NWD.K_PHP_TIME_SYNC + ").' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1 ");
            tFile.Append("LIMIT 1");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuerySelect '.$tQuerySelect.'"));
            tFile.AppendLine("$tResultSelect = $SQL_CON->query($tQuerySelect);");
            tFile.AppendLine("if (!$tResultSelect)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuerySelect"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResultSelect->num_rows == 0)");
            tFile.AppendLine("{");

            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
                
            tFile.AppendLine("while($tRowSelect = $tResultSelect->fetch_array())");
            tFile.AppendLine("{");
            tFile.Append("$tQueryUpdate = 'UPDATE `" + TableNamePHP(sEnvironment) + "` SET ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().DS) + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + tEnvSync + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().IP) + "` = \\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Deadline) + "` = \\'.$tDeadline.\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Counter) + "` = 1 ");
            tFile.Append("WHERE ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference)+"']).'\\' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1 ");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQueryUpdate = '.$tQueryUpdate.'"));
            tFile.AppendLine("$tResultUpdate = $SQL_CON->query($tQueryUpdate);");
            tFile.AppendLine("if (!$tResultUpdate)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryUpdate"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDIPBan.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE()+" ($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference)+"']);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tDeadline = " + NWD.K_PHP_TIME_SYNC + " + " + sEnvironment.IPBanTimer.ToString() + ";");
            tFile.Append("$tQueryUpdate = 'UPDATE `" + TableNamePHP(sEnvironment) + "` SET ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().DS) + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + tEnvSync + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Deadline) + "` = '.$tDeadline.' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Counter) + "` = `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().Counter) + "` + 1 ");
            tFile.Append("WHERE ");
            tFile.Append("\\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDIPBan.FictiveData().IP) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDAccount.FictiveData().AC) + "` = 1");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQueryUpdate = '.$tQueryUpdate.'"));
            tFile.AppendLine("$tResultUpdate = $SQL_CON->query($tQueryUpdate);");
            tFile.AppendLine("if (!$tResultUpdate)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQueryUpdate"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDIPBan.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE()+" ($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDAccount.FictiveData().Reference)+"']);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("?>");
            return tFile.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif