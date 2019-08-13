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
        public override string AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return NWDIPBan.PhpEngine(sEnvironment);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBan : NWDBasis<NWDIPBan>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PhpEngine(NWDAppEnvironment sEnvironment)
        {
            
            string tEnvSync = NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_ENV_SYNC(sEnvironment);

            StringBuilder tFile = new StringBuilder();
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine("// ENGINE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_CONSTANTS_PATH(sEnvironment) + ");");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_SYNCHRONISATION_PATH(sEnvironment) + ");");
            //tFile.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/'." + NWD.K_ENV + ".'/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDIPBan>().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
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
            tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE ");
            tFile.Append("\\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` >= `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().CounterMaximum) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` >= '.$SQL_CON->real_escape_string(" + NWD.K_PHP_TIME_SYNC + ").' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1");
            tFile.AppendLine(";';");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_ACC90));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_log(sEnvironment, "YOU ARE BANNED!!!"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_IPB01));
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
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
            tFile.Append("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE");
            tFile.Append(" \\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` AND");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1");
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

            tFile.Append("$tQuerySelect = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Reference) + "` FROM `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` WHERE ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` < '.$SQL_CON->real_escape_string(" + NWD.K_PHP_TIME_SYNC + ").' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1 ");
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
            tFile.Append("$tQueryUpdate = 'UPDATE `" +NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` SET ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DS) + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + tEnvSync + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` = \\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` = \\'.$tDeadline.\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` = 1 ");
            tFile.Append("WHERE ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference) + "` = \\''.$SQL_CON->real_escape_string($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference)+"']).'\\' AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1 ");
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
            tFile.AppendLine(NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_FUNCTION_INTEGRITY_REEVALUATE()+" ($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference)+"']);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine("}");
            tFile.AppendLine("else if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tDeadline = " + NWD.K_PHP_TIME_SYNC + " + " + sEnvironment.IPBanTimer.ToString() + ";");
            tFile.Append("$tQueryUpdate = 'UPDATE `" + NWDBasisHelper.TableNamePHP<NWDIPBan>(sEnvironment) + "` SET ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().DS) + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + tEnvSync + "` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Deadline) + "` = '.$tDeadline.' , ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` = `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().Counter) + "` + 1 ");
            tFile.Append("WHERE ");
            tFile.Append("\\''.$SQL_CON->real_escape_string($_SERVER['REMOTE_ADDR']).'\\' LIKE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDIPBan>().IP) + "` AND ");
            tFile.Append("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().AC) + "` = 1");
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
            //tFile.AppendLine(NWDBasisHelper.BasisHelper<NWDIPBan>().PHP_FUNCTION_INTEGRITY_REEVALUATE()+" ($tRowSelect['"+ NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccount>().Reference)+"']);");
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