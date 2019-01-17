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
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        [NWDAlias("TradePlace")]
        public NWDReferenceType<NWDTradePlace> TradePlace
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade References", true, true, true)]
        [NWDAlias("TradeRequest")]
        public NWDReferenceType<NWDUserTradeRequest> TradeRequest
        {
            get; set;
        }
        [NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        [NWDAlias("ItemsAsked")]
        public NWDReferencesQuantityType<NWDItem> ItemsAsked
        {
            get; set;
        }
        [NWDAlias("TradeStatus")]
        public NWDTradeStatus TradeStatus
        {
            get; set;
        }
        [NWDAlias("TradeRequestHash")]
        public string TradeRequestHash
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void tradeProposalBlock(bool result, NWDOperationResult infos);
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
            tProposition.InternalKey = NWDAccountNickname.GetNickname();
#endif
            tProposition.Tag = NWDBasisTag.TagUserCreated;
            tProposition.TradePlace.SetObject(sRequest.TradePlace.GetObject());
            tProposition.TradeRequest.SetObject(sRequest);
            tProposition.ItemsProposed.SetReferenceAndQuantity(sRequest.ItemsProposed.GetReferenceAndQuantity());
            tProposition.ItemsAsked.SetReferenceAndQuantity(sRequest.ItemsAsked.GetReferenceAndQuantity());
            tProposition.TradeStatus = NWDTradeStatus.Active;
            tProposition.TradeRequestHash = sRequest.TradeHash;
            tProposition.SaveData();

            return tProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncTradeProposal()
        {
            List<Type> tLists = new List<Type>() {
                typeof(NWDUserTradeProposition),
                typeof(NWDUserTradeRequest),
                typeof(NWDUserTradeFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (tradeProposalBlockDelegate != null)
                {
                    tradeProposalBlockDelegate(true, null);
                }

                AddAndRemoveItems();
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (tradeProposalBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    tradeProposalBlockDelegate(false, tInfos);
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
            TradePlace.Flush();
            TradeRequest.Flush();
            ItemsProposed.Flush();
            ItemsAsked.Flush();
            TradeRequestHash = string.Empty;
            TradeStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AddAndRemoveItems(NWDMessage sMessage = null)
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

                // Send Notification to the seller
                if (sMessage != null)
                {
                    string tSellerReference = TradeRequest.GetObject().Account.GetReference();
                    NWDUserInterMessage.SendMessage(sMessage, tSellerReference);
                }

                // Set Trade Proposition to None, so we can reused an old slot for a new transaction
                Clean();

                // Sync NWDUserOwnership
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserOwnership) });
            }
        }
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
                    TradeRequestHash = tRequest.TradeHash;
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

            string tTradeHash = NWDUserTradeRequest.FindAliasName("TradeHash");
            string tTradeStatus = NWDUserTradeRequest.FindAliasName("TradeStatus");
            string tLimitDayTime = NWDUserTradeRequest.FindAliasName("LimitDayTime");
            string tTradePlace = NWDUserTradeRequest.FindAliasName("TradePlace");
            string tTradeRequest = NWDUserTradeRequest.FindAliasName("TradeRequest");
            string tWinnerProposition = NWDUserTradeRequest.FindAliasName("WinnerProposition");

            string t_THIS_TradeRequestHash = FindAliasName("TradeRequestHash");
            string t_THIS_TradePlace = FindAliasName("TradePlace");
            string t_THIS_TradeRequest = FindAliasName("TradeRequest");
            string t_THIS_TradeStatus = FindAliasName("TradeStatus");
            int t_THIS_Index_tTradeRequestHash = CSVAssemblyIndexOf(t_THIS_TradeRequestHash);
            int t_THIS_Index_TradePlace = CSVAssemblyIndexOf(t_THIS_TradePlace);
            int t_THIS_Index_TradeRequest = CSVAssemblyIndexOf(t_THIS_TradeRequest);
            int t_THIS_Index_TradeStatus = CSVAssemblyIndexOf(t_THIS_TradeStatus);
            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            int t_THIS_Index_ItemsProposed = CSVAssemblyIndexOf(t_THIS_ItemsProposed);
            string t_THIS_ItemsAsked = FindAliasName("ItemsAsked");
            int t_THIS_Index_ItemsAsked = CSVAssemblyIndexOf(t_THIS_ItemsAsked);
            string sScript = "" +
                "// start Addon \n" +
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserTradeRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerRequestHash = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeRequestHash + "` FROM `'.$ENV.'_" + Datas().ClassNamePHP + "` " +
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';\n" +
                "$tResultStatus = $SQL_CON->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "else" +
                "{\n" +
                "if ($tResultStatus->num_rows == 1)\n" +
                "{\n" +
                "$tRowStatus = $tResultStatus->fetch_assoc();\n" +
                "$tServerStatut = $tRowStatus['" + t_THIS_TradeStatus + "'];\n" +
                "$tServerRequestHash = $tRowStatus['" + t_THIS_TradeRequestHash + "'];\n" +
                "}\n" +
                "}\n" +
                // change the statut from CSV TO WAITING, ACCEPTED, CANCEL, EXPIRED
                "if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")\n" +
                "{\n" +
                //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
                "}\n" +
                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && " +
                "($tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() + 
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() + "))\n" +
                "{\n" +
                "$sReplaces["+ t_THIS_Index_ItemsProposed + "]='';\n" +
                "$sReplaces["+ t_THIS_Index_ItemsAsked + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_tTradeRequestHash + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_TradeRequest + "]='';\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                "}\n" +
                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                "{\n" +
                "$tQueryTrade = 'UPDATE `'.$ENV.'_" + NWDUserTradeRequest.Datas().ClassNamePHP + "` SET " +
                " `DM` = \\''.$TIME_SYNC.'\\'," +
                " `DS` = \\''.$TIME_SYNC.'\\'," +
                " `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\'," +
                " `" + tWinnerProposition + "` = \\''.$sCsvList[0].'\\'," +
                " `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\'" +
                " WHERE `AC`= \\'1\\' " +
                " AND `" + tTradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                " AND `" + tTradePlace + "` = \\''.$sCsvList[" + t_THIS_Index_TradePlace + "].'\\' " +
                " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_TradeRequest + "].'\\' " +
                " AND `" + tTradeHash + "` = \\''.$sCsvList[" + t_THIS_Index_tTradeRequestHash + "].'\\' " +
                " AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
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
                "GetDatas" + NWDUserTradeRequest.Datas().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_TradeRequest + "]);\n" +
                "}\n" +
                "}\n" +
                // OTHER
                "else\n" +
                "{\n" +
                // not possible return preview value
                //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
                "}\n" +
                "// finish Addon \n";

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