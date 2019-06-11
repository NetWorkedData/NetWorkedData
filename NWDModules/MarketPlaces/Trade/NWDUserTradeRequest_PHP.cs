// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:3
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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
public partial class NWDUserTradeRequestHelper : NWDHelper<NWDUserTradeRequest>
    {
		//-------------------------------------------------------------------------------------------------------------
        public override string New_AddonPhpPreCalculate(NWDAppEnvironment sAppEnvironment)
		{

            string t_THIS_TradeStatus = NWDToolbox.PropertyName(() => FictiveData().TradeStatus);
            string t_THIS_TradeHash = NWDToolbox.PropertyName(() => FictiveData().TradeHash);
            string t_THIS_WinnerProposition = NWDToolbox.PropertyName(() => FictiveData().WinnerProposition);
            string t_THIS_ItemsProposed = NWDToolbox.PropertyName(() => FictiveData().ItemsProposed);
            string t_THIS_ItemsAsked = NWDToolbox.PropertyName(() => FictiveData().ItemsAsked);

            //         string t_THIS_TradeStatus = FindAliasName("TradeStatus");
            //string t_THIS_TradeHash = FindAliasName("TradeHash");
            //string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            int t_THIS_Index_TradeStatus = New_CSV_IndexOf(t_THIS_TradeStatus);
			int t_THIS_Index_TradeHash = New_CSV_IndexOf(t_THIS_TradeHash);
			int t_THIS_Index_WinnerProposition = New_CSV_IndexOf(t_THIS_WinnerProposition);
			//string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
			int t_THIS_Index_ItemsProposed = New_CSV_IndexOf(t_THIS_ItemsProposed);
			//string t_THIS_ItemsAsked = FindAliasName("ItemsAsked");
			int t_THIS_Index_ItemsAsked = New_CSV_IndexOf(t_THIS_ItemsAsked);
			string sScript = "" +
				"// start Addon \n" +
				// get the actual state
				"$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
				"$tServerHash = '';\n" +
				"$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeHash + "` FROM `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` " +
				"WHERE " +
				"`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\';';\n" +
				"$tResultStatus = "+NWD.K_SQL_CON+"->query($tQueryStatus);\n" +
				"if (!$tResultStatus)\n" +
				"{\n" +
				"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
				"}\n" +
				"else" +
				"{\n" +
				"if ($tResultStatus->num_rows == 1)\n" +
				"{\n" +
				"$tRowStatus = $tResultStatus->fetch_assoc();\n" +
				"$tServerStatut = $tRowStatus['" + t_THIS_TradeStatus + "'];\n" +
				"$tServerHash = $tRowStatus['" + t_THIS_TradeHash + "'];\n" +
				"}\n" +
				"}\n" +
				// change the statut from CSV TO WAITING, ACCEPTED, EXPIRED
				"if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
				" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
				" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
				" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
				"return;\n" +
				"}\n" +
				// change the statut from CSV TO ACTIVE 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && " +
				"$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
				"{\n" +
				"$sReplaces[" + t_THIS_Index_TradeHash + "] = "+NWD.K_PHP_TIME_SYNC+";\n" +
				"$sReplaces[" + t_THIS_Index_TradeStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
				"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
				"$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
				"}\n" +
				// change the statut from CSV TO NONE 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (" +
				"$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
				" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
				" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
				"))\n" +
				"{\n" +
				"$sReplaces[" + t_THIS_Index_TradeHash + "] = "+NWD.K_PHP_TIME_SYNC+";\n" +
				"$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
				"$sReplaces[" + t_THIS_Index_ItemsAsked + "]='';\n" +
				"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
				"$sCsvList = Integrity" + ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
				"}\n" +
				// change the statut from CSV TO CANCEL 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
				"$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
				"{\n" +
				"$tQueryCancelable = 'UPDATE `'."+NWD.K_ENV+".'_" + ClassNamePHP + "` SET " +
				"`DM` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
				"`DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
				"`'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\', " +
				"`" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
				"WHERE " +
				"`Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\' " +
				"AND `" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
				"';" +
				"$tResultCancelable = "+NWD.K_SQL_CON+"->query($tQueryCancelable);\n" +
				"if (!$tResultCancelable)\n" +
				"{\n" +
				"myLog('error in mysqli request : ('. "+NWD.K_SQL_CON+"->errno.')'. "+NWD.K_SQL_CON+"->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
				"}\n" +
				"else" +
				"\n" +
				"{\n" +
				"$tNumberOfRow = 0;\n" +
				"$tNumberOfRow = "+NWD.K_SQL_CON+"->affected_rows;\n" +
				"if ($tNumberOfRow == 1)\n" +
				"{\n" +
				"// I can change data to expired!\n" +
				"Integrity" + ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
				"return;\n" +
			   "}\n" +
				"else\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + ClassNamePHP + "ByReference ($tReference);\n" +
				"//stop the function!\n" +
				"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
				"return;\n" +
				"}\n" +
				"}\n" +
				"}\n" +

				// change the statut from CSV TO FORCE // ADMIN ONLY 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
					"{\n" +
					"//EXECEPTION FOR ADMIN\n" +
					"}\n" +

				// OTHER
				"else\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
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