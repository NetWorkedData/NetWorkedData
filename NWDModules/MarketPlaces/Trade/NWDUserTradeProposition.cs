//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using SQLite.Attribute;
using UnityEngine;
using BasicToolBox;
using System.Collections.Generic;
using System;

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
    [NWDForceSecureDataAttribute]
    public partial class NWDUserTradeProposition : NWDBasis<NWDUserTradeProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Trade Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        [NWDAlias("TradePlace")]
        public NWDReferenceType<NWDTradePlace> TradePlace { get; set; }
        [NWDAlias("TradeRequest")]
        public NWDReferenceType<NWDUserTradeRequest> TradeRequest { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade References", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }
        public NWDReferencesQuantityType<NWDItem> ItemsAsked { get; set; }
        [NWDAlias("TradeStatus")]
        public NWDTradeStatus TradeStatus { get; set; }
        [NWDAlias("TradeRequestDM")]
        public NWDDateTimeUtcType TradeRequestDM { get; set; }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void tradeProposalBlock(bool result, NWDTradeStatus status, NWDOperationResult infos);
        public tradeProposalBlock tradeProposalBlockDelegate;
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
        public static NWDUserTradeProposition CreateTradeProposalWith(NWDUserTradeRequest sRequest)
        {
            // Create a new Proposal
            NWDUserTradeProposition tProposition = NewData();
            #if UNITY_EDITOR
            NWDTradePlace tTrade = sRequest.TradePlace.GetObject();
            tProposition.InternalKey = NWDAccountNickname.GetNickname() + " - " + tTrade.InternalKey;
            #endif
            tProposition.Tag = NWDBasisTag.TagUserCreated;
            tProposition.TradePlace.SetObject(sRequest.TradePlace.GetObject());
            tProposition.TradeRequest.SetObject(sRequest);
            tProposition.ItemsProposed.SetReferenceAndQuantity(sRequest.ItemsProposed.GetReferenceAndQuantity());
            tProposition.ItemsAsked.SetReferenceAndQuantity(sRequest.ItemsAsked.GetReferenceAndQuantity());
            tProposition.TradeStatus = NWDTradeStatus.Active;
            tProposition.TradeRequestDM.SetTimeStamp(sRequest.DM);
            tProposition.SaveData();

            return tProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncTradeProposal(NWDMessage sMessage = null)
        {
            List<Type> tLists = new List<Type>() {
                typeof(NWDUserTradeProposition),
                typeof(NWDUserTradeRequest),
                typeof(NWDUserTradeFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                // Keep TradeStatus before Clean()
                NWDTradeStatus tTradeStatus = TradeStatus;

                // Notify the seller with an Inter Message
                if (sMessage != null)
                {
                    string tSellerReference = TradeRequest.GetObjectAbsolute().Account.GetReference();
                    NWDUserInterMessage.SendMessage(sMessage, tSellerReference);
                }

                // Do action with Items & Sync
                AddOrRemoveItems();

                // Notify Callback
                if (tradeProposalBlockDelegate != null)
                {
                    tradeProposalBlockDelegate(true, tTradeStatus, null);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (tradeProposalBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    tradeProposalBlockDelegate(false, NWDTradeStatus.None, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Cancel()
        {
            TradeStatus = NWDTradeStatus.Cancel;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Clean()
        {
            TradePlace = null;
            TradeRequest = null;
            ItemsProposed = null;
            ItemsAsked = null;
            TradeRequestDM = null;
            TradeStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AddOrRemoveItems()
        {
            if (TradeStatus == NWDTradeStatus.Accepted)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                {
                    NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                }

                // Remove NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tAsked = ItemsAsked.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> pair in tAsked)
                {
                    NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                }

                // Set Trade Proposition to None, so we can reused an old slot for a new transaction
                Clean();

                // Sync NWDUserOwnership
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserOwnership) });
            }
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
                Debug.Log("YES ? or Not " + TradeRequest.Value);
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
            //string tWinnerProposition = NWDUserTradeRequest.FindAliasName("WinnerProposition");

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
                // YOU MUST REIMPORT THE GLOBAL ... PHP strange practice?
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserTradeRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +

                "\n" +
                "if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")\n" +
                "{\n" +
                // error ou
                //"error('UTRRx99');\n" +
                //"return;\n" +
                // none ... faudra trancher : none pour avoir une sync 
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_TradeStatus + ", '" + ((int)NWDTradeStatus.None).ToString() + "');\n" +
                "}\n" +
                "if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + ")\n" +
                "{\n" +
                "$tQueryTrade = 'UPDATE `'.$ENV.'_" + NWDUserTradeRequest.Datas().ClassNamePHP + "` SET " +
                " `DM` = \\''.$TIME_SYNC.'\\'," +
                " `DS` = \\''.$TIME_SYNC.'\\'," +
                " `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\'," +
                //" `" + tWinnerProposition + "` = \\''.$sCsvList[0].'\\'," +
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
                "Integrity" + NWDUserTradeRequest.Datas().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_TradeRequest + "]);\n" +
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
            string t_THIS_TradeRequest = FindAliasName("TradeRequest");
            int t_THIS_Index_TradeRequest = CSVAssemblyIndexOf(t_THIS_TradeRequest);

            return "// write your php script here to update after sync on server\n " +
                "GetDatas" + NWDUserTradeRequest.Datas().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_TradeRequest + "]);\n";
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