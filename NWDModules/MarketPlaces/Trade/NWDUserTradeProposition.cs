//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using SQLite.Attribute;
using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronize(true)]
    [NWDClassTrigramme("UTRP")]
    [NWDClassDescription("User Trade Proposition descriptions Class")]
    [NWDClassMenuName("User Trade Proposition")]
    public partial class NWDUserTradeProposition : NWDBasis<NWDUserTradeProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
#region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Trade Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set;}
        [NWDAlias("TradePlace")]
        public NWDReferenceType<NWDTradePlace> TradePlace { get; set; }
        [NWDAlias("TradeRequest")]
        public NWDReferenceType<NWDUserTradeRequest> TradeRequest { get; set; }
        [NWDAlias("TradeStatus")]
        public NWDTradeStatus TradeStatus { get; set;}
        [NWDAlias("TradeRequestDM")]
        public NWDDateTimeUtcType TradeRequestDM {get; set;}
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade References", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }
        public NWDReferencesQuantityType<NWDItem> ItemsAsked { get; set; }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeProposition()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeProposition(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
#region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;

            float tYadd = 20.0f;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "fixe date of TradeRequest DM", tMiniButtonStyle))
            {
                Debug.Log("YES ? or Not "+ TradeRequest.Value);
                NWDUserTradeRequest tRequest = TradeRequest.GetObjectAbsolute();
                if (tRequest != null)
                {
                    Debug.Log("YES");
                    TradeRequestDM.SetLong(tRequest.DM);
                }
            }
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 20.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPreCalculate()
        {

            string tTradeStatus = NWDUserTradeRequest.FindAliasName("TradeStatus");
            string tLimitDayTime = NWDUserTradeRequest.FindAliasName("LimitDayTime");
            string tTradePlace = NWDUserTradeRequest.FindAliasName("TradePlace");
            string tTradeRequest = NWDUserTradeRequest.FindAliasName("TradeRequest");

            string t_THIS_TradeRequestDM = FindAliasName("TradeRequestDM");
            string t_THIS_TradePlace = FindAliasName("TradePlace");
            string t_THIS_TradeRequest = FindAliasName("TradeRequest");
            string t_THIS_TradeStatus = FindAliasName("TradeStatus");
            int t_THIS_Index_tTradeRequestDM = CSVAssemblyIndexOf(t_THIS_TradeRequestDM);
            int t_THIS_Index_TradePlace = CSVAssemblyIndexOf(t_THIS_TradePlace);
            int t_THIS_Index_TradeRequest = CSVAssemblyIndexOf(t_THIS_TradeRequest);
            int t_THIS_Index_TradeStatus = CSVAssemblyIndexOf(t_THIS_TradeStatus);
            string sScript = "" +
                "// debut find \n" +
                "\n" +
                "if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + ")\n" +
                "{\n" +
                "$tQueryTrade = 'UPDATE `'.$ENV.'_" + NWDUserTradeRequest.Datas().ClassNamePHP + "` SET " +
                " `DM` = \\''.$TIME_SYNC.'\\'," +
                " `DS` = \\''.$TIME_SYNC.'\\'," +
                " `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\'," +
                " `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\'" +
                // WHERE REQUEST
                " WHERE `AC`= \\'1\\' " +
                " AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Active).ToString() + "\\' " +
                " AND `" + tTradePlace + "` = \\''.$sCsvList[" + t_THIS_Index_TradePlace + "].'\\' " +
                " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_TradeRequest + "].'\\' " +
                " AND `DM` = \\''.$sCsvList[" + t_THIS_Index_tTradeRequestDM + "].'\\' " +
                " AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                "';\n" +
                "myLog('tQueryTrade : '. $tQueryTrade, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultTrade = $SQL_CON->query($tQueryTrade);\n" +
                "$tReferences = \'\';\n" +
                "$tReferencesList = \'\';\n" +
                "if (!$tResultTrade)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryTrade.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRFx31');\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "$tNumberOfRow = 0;\n" +
                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                "if ($tNumberOfRow == 1)\n" +
                "{\n" +
                "// I need update the proposition too !\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_TradeStatus + ", \'" + ((int)NWDTradeStatus.Accepted).ToString() + "\');\n" +
                "myLog('I need update the proposition accept', __FILE__, __FUNCTION__, __LINE__);\n" +
                //"global $PATH_BASE;\n" +
                // YOU MUST REIMPORT THE GLOSBAL ... PHP strange practice?
                //"global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $uuid;\n" +
                //"global $SQL_" + NWDUserTradeRequest.Datas().ClassNamePHP + "_SaltA, $SQL_" + NWDUserTradeRequest.Datas().ClassNamePHP + "_SaltB, $SQL_" + NWDUserTradeRequest.Datas().ClassNamePHP + "_WebService;" +
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserTradeRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +
                "Integrity" + NWDUserTradeRequest.Datas().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_TradeRequest+ "]);" +
                "}\n" +
                "else\n" +
                "{\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_TradeStatus + ", \'" + ((int)NWDTradeStatus.Expired).ToString() + "\');\n" +
                "\tmyLog('I need update the proposition refused ... too late!', __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "}\n" +
                "}\n" +
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() + ")\n" +
                "{\n" +
                "// this case must be cancelled ?\n" +
                "}\n" +
                "// fin find \n";

            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPostCalculate()
        {
            return "// write your php script here to update afetr sync on server\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpSpecialCalculate()
        {
            return "// write your php script here to special operation, example : \n$REP['" + Datas().ClassName + " Special'] ='success!!!';\n";
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================