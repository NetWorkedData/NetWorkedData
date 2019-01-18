//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
	[NWDClassServerSynchronize(true)]
    [NWDClassTrigramme("UBRF")]
    [NWDClassDescription("User Barter Finder descriptions Class")]
    [NWDClassMenuName("User Barter Finder")]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserBarterFinder : NWDBasis<NWDUserBarterFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Barter Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDBarterPlace> BarterPlace
        {
            get; set;
        }
        [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
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
        [NWDAlias("BarterRequestsList")]
        public NWDReferencesListType<NWDUserBarterRequest> BarterRequestsList
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void barterFinderBlock(bool result, NWDOperationResult infos);
        public barterFinderBlock barterFinderBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterFinder()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterFinder(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
        public static NWDUserBarterRequest[] FindPropositionsWith(NWDBarterPlace sBarterPlace)
        {
            NWDUserBarterFinder[] tUserBartersFinder = FindDatas();
            foreach (NWDUserBarterFinder k in tUserBartersFinder)
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    return k.BarterRequestsList.GetObjectsAbsolute();
                }
            }

            CreateBarterFinderWith(sBarterPlace);

            return new NWDUserBarterRequest[0];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterFinder GetBarterFinderWith(NWDBarterPlace sBarterPlace)
        {
            NWDUserBarterFinder[] tUserBartersFinder = FindDatas();
            foreach (NWDUserBarterFinder k in tUserBartersFinder)
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    return k;
                }
            }

            return CreateBarterFinderWith(sBarterPlace);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDUserBarterFinder CreateBarterFinderWith(NWDBarterPlace sBarterPlace)
        {
            // No NWD Finder Object found, we create one
            NWDUserBarterFinder tFinder = NewData();
#if UNITY_EDITOR
            tFinder.InternalKey = NWDAccountNickname.GetNickname();
#endif
            tFinder.Tag = NWDBasisTag.TagUserCreated;
            tFinder.BarterPlace.SetObject(sBarterPlace);
            tFinder.SaveData();

            return tFinder;
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterFinder()
        {
            // Clean the Barter Place Finder Result
            CleanResult();

            List<Type> tLists = new List<Type>() {
                typeof(NWDUserBarterProposition),
                typeof(NWDUserBarterRequest),
                typeof(NWDUserBarterFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterFinderBlockDelegate != null)
                {
                    barterFinderBlockDelegate(true, null);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterFinderBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterFinderBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CleanResult()
        {
            BarterRequestsList = null;
            SaveData();

            // Remove stranger data request
            NWDUserBarterRequest.PurgeTable();
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
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {

            string tBarterStatus = NWDUserBarterRequest.FindAliasName("BarterStatus");
            string tLimitDayTime = NWDUserBarterRequest.FindAliasName("LimitDayTime");
            string tBarterPlaceRequest = NWDUserBarterRequest.FindAliasName("BarterPlace");
            string tForRelationshipOnly = NWDUserBarterRequest.FindAliasName("ForRelationshipOnly");
            string tRelationshipAccountReferences = NWDUserBarterRequest.FindAliasName("RelationshipAccountReferences");
            int tIndex_tBarterStatus = NWDUserTradeRequest.CSVAssemblyIndexOf(tBarterStatus);

            string t_THIS_BarterRequestsList = FindAliasName("BarterRequestsList");
            string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            string t_THIS_ForRelationshipOnly = FindAliasName("ForRelationshipOnly");
            string t_THIS_MaxPropositions = FindAliasName("MaxPropositions");
            string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");

            int tIndex_BarterRequestsList = CSVAssemblyIndexOf(t_THIS_BarterRequestsList);
            int tIndex_BarterPlace = CSVAssemblyIndexOf(t_THIS_BarterPlace);
            int tIndex_THIS_ForRelationshipOnly = CSVAssemblyIndexOf(t_THIS_ForRelationshipOnly);

            int tDelayOfRefresh = 5; // minutes before stop to get the datas!
            string sScript = "" +
                "// start Addon \n" +
                "include_once($PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserBarterRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +
                "$tQueryExpired = 'SELECT " + NWDUserBarterRequest.SLQAssemblyOrder() + " FROM `'.$ENV.'_" + NWDUserBarterRequest.Datas().ClassNamePHP + "` " +
                "WHERE `AC`= \\'1\\' " +
                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tLimitDayTime + "` < '.$TIME_SYNC.' " +
                "AND `WebServiceVersion` <= '.$WSBUILD.' " +
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryExpired : '. $tQueryExpired, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultExpired = $SQL_CON->query($tQueryExpired);\n" +
                "if (!$tResultExpired)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRFx31');\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "while($tRowExpired = $tResultExpired->fetch_row())\n" +
                "{\n" +
                "myLog('tReferences need be cancelled : '. $tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tRowExpired = Integrity" + NWDUserBarterRequest.Datas().ClassNamePHP + "Replace ($tRowExpired," + tIndex_tBarterStatus + ", " + ((int)NWDTradeStatus.Cancel).ToString() + ");\n" +
                "$tRowExpired = implode('" + NWDConstants.kStandardSeparator + "',$tRowExpired);\n" +
                "UpdateData" + NWDUserBarterRequest.Datas().ClassNamePHP + " ($tRowExpired, $TIME_SYNC, $uuid, false);\n" +
                "}\n" +
                //"mysqli_free_result($tResultExpired);\n" +
                "}\n" +


                "// start Addon \n" +
                "$tQueryBarter = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterRequest.Datas().ClassNamePHP + "` " +
                // WHERE REQUEST
                "WHERE `AC`= \\'1\\' " +
                "AND `Account` != \\''.$SQL_CON->real_escape_string($uuid).'\\' " +
                "AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "AND `" + tForRelationshipOnly + "` = \\''.$sCsvList[" + tIndex_THIS_ForRelationshipOnly + "].'\\' " +
                "AND `" + t_THIS_MaxPropositions + "` > `" + t_THIS_PropositionsCounter + "` ';\n" +
                "if ($sCsvList[" + tIndex_THIS_ForRelationshipOnly + "] == '1')\n" +
                "{\n" +
                "$tQueryBarter.= 'AND `" + tRelationshipAccountReferences + "` = \\''.$uuid.'\\' ';\n" +
                "}\n" +
                "$tQueryBarter.= '" +
                "AND `" + tBarterPlaceRequest + "` = \\''.$sCsvList[" + tIndex_BarterPlace + "].'\\' " +
                "AND `" + tLimitDayTime + "` > '.($TIME_SYNC+" + (tDelayOfRefresh * 60).ToString() + ").' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                // LIMIT 
                "LIMIT 0, 100;';\n" +
                "myLog('tQueryBarter : '. $tQueryBarter, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tResultBarter = $SQL_CON->query($tQueryBarter);\n" +
                "$tReferences = \'\';\n" +
                "$tReferencesList = \'\';\n" +
                "if (!$tResultBarter)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryBarter.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRFx31');\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "while($tRowBarter = $tResultBarter->fetch_assoc())\n" +
                "{\n" +
                "myLog('tReferences found : '. $tRowBarter['Reference'], __FILE__, __FUNCTION__, __LINE__);\n" +
                "$tReferences[]=$tRowBarter['Reference'];\n" +
                "}\n" +
                "if (is_array($tReferences))\n" +
                "{\n" +
                "$tReferencesList = implode('" + NWDConstants.kFieldSeparatorA + "',$tReferences);\n" +
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserBarterRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +
                "GetDatas" + NWDUserBarterRequest.Datas().ClassNamePHP + "ByReferences ($tReferences);\n" +
                "}\n" +
                "}\n" +
                "myLog('tReferencesList : '. $tReferencesList, __FILE__, __FUNCTION__, __LINE__);\n" +
                "$sCsvList = Integrity" + NWDUserBarterFinder.Datas().ClassNamePHP + "Replace ($sCsvList, " + tIndex_BarterRequestsList.ToString() + ", $tReferencesList);\n" +
                "// finish Addon \n";

            return sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            return "\n" +
                "\n";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
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