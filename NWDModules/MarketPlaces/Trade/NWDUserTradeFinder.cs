//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// </summary>
	[NWDClassServerSynchronize(true)]
    [NWDClassTrigramme("UTRF")]
    [NWDClassDescription("User Trade Finder descriptions Class")]
    [NWDClassMenuName("User Trade Finder")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTradeFinder : NWDBasis<NWDUserTradeFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Trade Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDTradePlace> TradePlace
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Filters", true, true, true)]
        public NWDReferencesListType<NWDItem> FilterItems
        {
            get; set;
        }
        public NWDReferencesListType<NWDWorld> FilterWorlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> FilterCategories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FilterFamilies
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> FilterKeywords
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Results", true, true, true)]
        [NWDAlias("TradeRequestsList")]
        public NWDReferencesListType<NWDUserTradeRequest> TradeRequestsList
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeFinder()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeFinder(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
        public static NWDUserTradeRequest[] FindPropositionsList(NWDTradePlace sTradePlace)
        {
            NWDUserTradeFinder[] tUserTradesFinder = FindDatas();
            foreach (NWDUserTradeFinder k in tUserTradesFinder)
            {
                if (k.TradePlace.GetReference().Equals(sTradePlace.Reference))
                {
                    return k.TradeRequestsList.GetObjectsAbsolute();
                }
            }

            // No NWD Finder Object found, we create one
            NWDUserTradeFinder tFinder = NewData();
#if UNITY_EDITOR
            tFinder.InternalKey = NWDAccountNickname.GetNickname();
#endif
            tFinder.Tag = NWDBasisTag.TagUserCreated;
            tFinder.TradePlace.SetObject(sTradePlace);
            tFinder.SaveData();

            return new NWDUserTradeRequest[0];
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
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPreCalculate()
        {

            string tTradeStatus = NWDUserTradeRequest.FindAliasName("TradeStatus");
            string tLimitDayTime = NWDUserTradeRequest.FindAliasName("LimitDayTime");
            string tTradePlaceRequest = NWDUserTradeRequest.FindAliasName("TradePlace");

            string tTradeRequestsList = NWDUserTradeFinder.FindAliasName("TradeRequestsList");
            string tTradePlace = NWDUserTradeFinder.FindAliasName("TradePlace");
            int tIndex_TradeRequestsList = CSVAssemblyIndexOf(tTradeRequestsList);
            int tIndex_TradePlace = CSVAssemblyIndexOf(tTradePlace);

            int tDelayOfRefresh = 5; // minutes before stop to get the datas!
            string sScript = "" +
                "// debut find \n" +
                "$tQueryTrade = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserTradeRequest.Datas().ClassNamePHP + "` " +
                // WHERE REQUEST
                "WHERE `AC`= \\'1\\' " +
                "AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Active).ToString() + "\\' " +
                "AND `" + tTradePlaceRequest + "` = \\''.$sCsvList[" + tIndex_TradePlace + "].'\\' " +
                "AND `" + tLimitDayTime + "` > '.($TIME_SYNC+"+ (tDelayOfRefresh * 60).ToString() + ").' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                // END WHERE REQUEST LIMIT START
                "LIMIT 0, 100;';\n" +
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
                "while($tRowTrade = $tResultTrade->fetch_assoc())\n" +
                "{\n" +
                "myLog('tReferences found : '. $tRowTrade['Reference'], __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tReferences[]=$tRowTrade['Reference'];\n" +
                "}\n" +
                "if (is_array($tReferences))\n" +
                "{\n" +
                "$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);\n" +
                "}\n" +
                "}\n" +
                "myLog('tReferencesList : '. $tReferencesList, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$sCsvList = IntegrityNWDUserTradeFinderReplaceIntegrate ($sCsvList, " + tIndex_TradeRequestsList.ToString() + ", $tReferencesList);\n" +
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