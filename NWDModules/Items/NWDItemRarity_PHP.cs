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
            rReturn.AppendLine("$tQuery = 'SELECT COUNT(`" + NWDToolbox.PropertyName(() => NWDGameSave.FictiveData().Reference) + "`) as TotalUser FROM `" + NWDGameSave.BasisHelper().PHP_TABLENAME(sEnvironment) + "`';");
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
            rReturn.AppendLine("$tQuery = 'SELECT t2." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Reference) + ", t2." + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemReference) + ",';");
            rReturn.AppendLine("$tQuery.= ' SUM(t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Quantity) + ") as Total ,';");
            rReturn.AppendLine("$tQuery.= ' COUNT(t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Reference) + ") as OwnerTotal,';");
            rReturn.AppendLine("$tQuery.= ' MAX(t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Quantity) + ") as ItemMax,';");
            rReturn.AppendLine("$tQuery.= ' MIN(t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Quantity) + ") as ItemMin,';");
            rReturn.AppendLine("$tQuery.= ' AVG(t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Quantity) + ") as ItemAvg';");
            rReturn.AppendLine("$tQuery.= ' FROM `" + NWDUserOwnership.BasisHelper().PHP_TABLENAME(sEnvironment) + "` t1 INNER JOIN `" + NWDItemRarity.BasisHelper().PHP_TABLENAME(sEnvironment) + "` t2 ON t1." + NWDToolbox.PropertyName(() => NWDUserOwnership.FictiveData().Item) + " = t2." + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemReference) + " GROUP BY t2." + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemReference) + ";';");
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
            rReturn.AppendLine("$tUpdate = 'UPDATE `" + NWDItemRarity.BasisHelper().PHP_TABLENAME(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().DM) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().DS) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\', `" + PHP_ENV_SYNC(sEnvironment) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Maximum) + "` = \\''.$tRow['ItemMax'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Minimum) + "` = \\''.$tRow['ItemMin'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().OwnerUserTotal) + "` = \\''.$tRow['OwnerTotal'].'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().UserTotal) + "` = \\''.$tUserCount.'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Average) + "` = \\''.number_format ($tRow['ItemAvg'], $NWD_FLOAT_FORMAT ,'.','').'\\',';");
            rReturn.AppendLine("if ($tRow['ItemAvg']!=0 && $tUserCount!=0)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Frequency) + "` = \\''.number_format (($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg'], $NWD_FLOAT_FORMAT,'.','').'\\',';");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Rarity) + "` = \\''.number_format (1.0/(($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg']), $NWD_FLOAT_FORMAT,'.','').'\\',';");
            rReturn.AppendLine("}");
            rReturn.AppendLine("$tUpdate.=' `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemTotal) + "` = \\''.$tRow['Total'].'\\'';");
            rReturn.AppendLine("$tUpdate.=' WHERE `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Reference) + "` = \\''.$tRow['" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Reference) + "'].'\\' AND `" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemReference) + "` = \\''.$tRow['" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().ItemReference) + "'].'\\';';");
            rReturn.AppendLine(NWDError.PHP_log(sEnvironment, "$tUpdate = '.$tUpdate.'"));
            rReturn.AppendLine("$tUpdateResult = " + NWD.K_SQL_CON + "->query($tUpdate);");
            rReturn.AppendLine("$REP['spc'][$tRow['" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Reference) + "']] = $tUpdate;");
            rReturn.AppendLine("if (!$tUpdateResult)");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tUpdate"));
            rReturn.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SERVER));
            rReturn.AppendLine("}");
            rReturn.AppendLine("else");
            rReturn.AppendLine("{");
            rReturn.AppendLine(NWDItemRarity.BasisHelper().PHP_FUNCTION_INTEGRITY_REEVALUATE() + "($tRow['" + NWDToolbox.PropertyName(() => NWDItemRarity.FictiveData().Reference) + "']);");
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