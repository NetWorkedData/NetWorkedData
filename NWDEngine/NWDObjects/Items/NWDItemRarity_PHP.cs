//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date        2019-4-12 18:48:35
//  Author      Kortex (Jean-François CONTART) 
//  Email       jfcontart@idemobi.com
//  Project     NetWorkedData for Unity3D
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
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemRarityHelper : NWDHelper<NWDItemRarity>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            StringBuilder rReturn = new StringBuilder();
            rReturn.AppendLine("global $NWD_FLOAT_FORMAT;");
            rReturn.AppendLine("// Count user by gamesave");
            rReturn.AppendLine("$tUserCount = 0;");
            rReturn.AppendLine("$tQuery = 'SELECT COUNT(`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDGameSave>().Reference) + "`) as TotalUser FROM `" + NWDBasisHelper.BasisHelper<NWDGameSave>().PHP_TABLENAME(sEnvironment) + "`';");
            rReturn.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));
            rReturn.AppendLine("if (!$tResult)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRow = $tResult->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tUserCount = $tRow['TotalUser'];");
            rReturn.AppendLine("}");
            rReturn.AppendLine("mysqli_free_result($tResult);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("// calculate rarity and fill the datas");
            rReturn.AppendLine("$tQuery = 'SELECT t2." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Reference) + ", t2." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemReference) + ",';");
            rReturn.AppendLine("$tQuery.= ' SUM(t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Quantity) + ") as Total ,';");
            rReturn.AppendLine("$tQuery.= ' COUNT(t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Reference) + ") as OwnerTotal,';");
            rReturn.AppendLine("$tQuery.= ' MAX(t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Quantity) + ") as ItemMax,';");
            rReturn.AppendLine("$tQuery.= ' MIN(t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Quantity) + ") as ItemMin,';");
            rReturn.AppendLine("$tQuery.= ' AVG(t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Quantity) + ") as ItemAvg';");
            rReturn.AppendLine("$tQuery.= ' FROM `" + NWDBasisHelper.BasisHelper<NWDUserOwnership>().PHP_TABLENAME(sEnvironment) + "` t1 INNER JOIN `" + NWDBasisHelper.BasisHelper<NWDItemRarity>().PHP_TABLENAME(sEnvironment) + "` t2 ON t1." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDUserOwnership>().Item) + " = t2." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemReference) + " GROUP BY t2." + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemReference) + ";';");
            rReturn.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "$tQuery = '.$tQuery.'"));
            rReturn.AppendLine("if (!$tResult)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine("while($tRow = $tResult->fetch_assoc())");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tUpdate = 'UPDATE `" + NWDBasisHelper.BasisHelper<NWDItemRarity>().PHP_TABLENAME(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().DM) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().DS) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', `" + PHP_ENV_SYNC(sEnvironment) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Maximum) + "` = \\''.$tRow['ItemMax'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Minimum) + "` = \\''.$tRow['ItemMin'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().OwnerUserTotal) + "` = \\''.$tRow['OwnerTotal'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().UserTotal) + "` = \\''.$tUserCount.'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Average) + "` = \\''.number_format ($tRow['ItemAvg'], $NWD_FLOAT_FORMAT ,'.','').'\\',';");
            rReturn.AppendLine("if ($tRow['ItemAvg']!=0 && $tUserCount!=0)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Frequency) + "` = \\''.number_format (($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg'], $NWD_FLOAT_FORMAT,'.','').'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Rarity) + "` = \\''.number_format (1.0/(($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg']), $NWD_FLOAT_FORMAT,'.','').'\\',';");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemTotal) + "` = \\''.$tRow['Total'].'\\'';");
            rReturn.AppendLine("$tUpdate.=' WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Reference) + "` = \\''.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Reference) + "'].'\\' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemReference) + "` = \\''.$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().ItemReference) + "'].'\\';';");
            rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "$tUpdate = '.$tUpdate.'"));
            rReturn.AppendLine("$tUpdateResult = " + NWD.K_SQL_CON + "->query($tUpdate);");
            rReturn.AppendLine("$REP['spc'][$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Reference) + "']] = $tUpdate;");
            rReturn.AppendLine("if (!$tUpdateResult)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tUpdate"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDBasisHelper.BasisHelper<NWDItemRarity>().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDItemRarity>().Reference) + "']);");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            rReturn.AppendLine("mysqli_free_result($tResult);");
            rReturn.AppendLine("$REP['special'] ='success!';");
            rReturn.AppendLine("}");
            rReturn.AppendLine("");
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif