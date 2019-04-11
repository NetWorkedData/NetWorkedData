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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemRarityHelper : NWDHelper<NWDItemRarity>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
        {
            return "\n" +
        "\t\tglobal $NWD_FLOAT_FORMAT;\n" +
        "\t\t// Count user by gamesave\n" +
        "\t\t$tUserCount = 0;\n" +
        "\t\t$tQuery = 'SELECT COUNT(Reference) as TotalUser FROM `'.$ENV.'_NWDGameSave`';\n" +
        "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
        "\t\tif (!$tResult)\n" +
        "\t\t\t{\n" +
        "\t\t\t\terror('IRYx33');\n" +
        "\t\t\t}\n" +
        "\t\telse\n" +
        "\t\t\t{\n" +
        "\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n" +
        "\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUserCount = $tRow['TotalUser'];\n" +
        "\t\t\t\t\t}\n" +
        "\t\t\t\tmysqli_free_result($tResult);\n" +
        "\t\t\t}\n" +
        "\t\t// calculate rarity and fill the datas\n" +
        "\t\t$tQuery = 'SELECT t2.Reference, t2.ItemReference,';\n" +
        "\t\t$tQuery.= ' SUM(t1.Quantity) as Total ,';\n" +
        "\t\t$tQuery.= ' COUNT(t1.Reference) as OwnerTotal,';\n" +
        "\t\t$tQuery.= ' MAX(t1.Quantity) as ItemMax,';\n" +
        "\t\t$tQuery.= ' MIN(t1.Quantity) as ItemMin,';\n" +
        "\t\t$tQuery.= ' AVG(t1.Quantity) as ItemAvg';\n" +
        "\t\t$tQuery.= ' FROM `'.$ENV.'_NWDUserOwnership` t1 INNER JOIN `'.$ENV.'_NWDItemRarity` t2 ON t1.Item = t2.ItemReference GROUP BY t2.ItemReference;';\n" +
        "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
        //"\t\tmyLog('mysqli request : ('. $tQuery.')', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\tif (!$tResult)\n" +
        "\t\t\t{\n" +
        "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\t\t\terror('IRYx33');\n" +
        "\t\t\t}\n" +
        "\t\telse\n" +
        "\t\t\t{\n" +
        "\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n" +
        "\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUpdate = 'UPDATE `'.$ENV.'_NWDItemRarity` SET `DM` = \\''.$TIME_SYNC.'\\', `DS` = \\''.$TIME_SYNC.'\\', `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Maximum` = \\''.$tRow['ItemMax'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Minimum` = \\''.$tRow['ItemMin'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `OwnerUserTotal` = \\''.$tRow['OwnerTotal'].'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `UserTotal` = \\''.$tUserCount.'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Average` = \\''.number_format ($tRow['ItemAvg'], $NWD_FLOAT_FORMAT ,'.','').'\\',';\n" +
        "\t\t\t\t\t\tif ($tRow['ItemAvg']!=0 && $tUserCount!=0)\n" +
        "\t\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Frequency` = \\''.number_format (($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg'], $NWD_FLOAT_FORMAT,'.','').'\\',';\n" +
        "\t\t\t\t\t\t$tUpdate.=' `Rarity` = \\''.number_format (1.0/(($tRow['OwnerTotal']/$tUserCount)/$tRow['ItemAvg']), $NWD_FLOAT_FORMAT,'.','').'\\',';\n" +
        "\t\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t\t$tUpdate.=' `ItemTotal` = \\''.$tRow['Total'].'\\'';\n" +
        "\t\t\t\t\t\t$tUpdate.=' WHERE `Reference` = \\''.$tRow['Reference'].'\\' AND `ItemReference` = \\''.$tRow['ItemReference'].'\\';';\n" +
        "\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
        //"\t\t\t\t\t\t$REP['spc'][$tRow['Reference']] = $tUpdate;\n" +
        "\t\t\t\t\t\tif (!$tUpdateResult)\n" +
        "\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
        "\t\t\t\t\t\t\terror('IRYx38');\n" +
        "\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t\telse\n" +
        "\t\t\t\t\t\t{\n" +
        "\t\t\t\t\t\tIntegrityNWDItemRarityReevalue($tRow['Reference']);\n" +
        "\t\t\t\t\t\t}\n" +
        "\t\t\t\t\t}\n" +
        "\t\t\t\tmysqli_free_result($tResult);\n" +
        "\t\t\t\t$REP['special'] ='success!';\n" +
        "\t\t\t}" +
        "";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif