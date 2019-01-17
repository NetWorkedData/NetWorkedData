﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronize(true)]
    [NWDClassTrigramme("UTRR")]
    [NWDClassDescription("User Trade Request descriptions Class")]
    [NWDClassMenuName("User Trade Request")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserTradeRequest : NWDBasis<NWDUserTradeRequest>
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

        [NWDGroupStart("For Relationship Only", true, true, true)]

        [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        [NWDAlias("RelationshipAccountReferences")]
        public string RelationshipAccountReferences
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Trade References", true, true, true)]
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
        [NWDAlias("TradeHash")]
        public string TradeHash
        {
            get; set;
        }
        [NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime
        {
            get; set;
        }
        [NWDAlias("WinnerProposition")]
        public NWDReferenceType<NWDUserTradeProposition> WinnerProposition
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Tags", true, true, true)]
        public NWDReferencesListType<NWDWorld> TagWorlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> TagCategories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> TagFamilies
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> TagKeywords
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void tradeRequestBlock(bool result, NWDOperationResult infos);
        public tradeRequestBlock tradeRequestBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeRequest()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserTradeRequest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
        public static NWDUserTradeRequest CreateTradeRequestWith(NWDTradePlace sTradePlace, Dictionary<string, int> sProposed, Dictionary<string, int> sAsked)
        {
            // Get Request Life time
            int tLifetime = sTradePlace.RequestLifeTime;

            // Create a new Request
            NWDUserTradeRequest tRequest = NewData();
#if UNITY_EDITOR
            tRequest.InternalKey = NWDAccountNickname.GetNickname(); // + " - " + sProposed.Name.GetBaseString();
#endif
            tRequest.Tag = NWDBasisTag.TagUserCreated;
            tRequest.TradePlace.SetObject(sTradePlace);
            tRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            tRequest.ItemsAsked.SetReferenceAndQuantity(sAsked);
            tRequest.TradeStatus = NWDTradeStatus.Active;
            tRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            tRequest.SaveData();

            return tRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTradeRequest[] FindRequestsWith(NWDTradePlace sTradePlace)
        {
            List<NWDUserTradeRequest> tUserTradesRequest = new List<NWDUserTradeRequest>();
            foreach (NWDUserTradeRequest k in FindDatas())
            {
                if (k.TradePlace.GetReference().Equals(sTradePlace.Reference))
                {
                    tUserTradesRequest.Add(k);
                }
            }

            return tUserTradesRequest.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncTradeRequest()
        {
            List<Type> tLists = new List<Type>() {
                typeof(NWDUserTradeProposition),
                typeof(NWDUserTradeRequest),
                typeof(NWDUserTradeFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (tradeRequestBlockDelegate != null)
                {
                    tradeRequestBlockDelegate(true, null);
                }

                AddOrRemoveItems();
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (tradeRequestBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    tradeRequestBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddOrRemoveItems()
        {
            switch (TradeStatus)
            {
                case NWDTradeStatus.Active:
                    {
                        // Remove NWDItem from NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                        }
                    }
                    break;
                case NWDTradeStatus.Expired:
                    {
                        // Add NWDItem to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Trade Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                case NWDTradeStatus.Accepted:
                    {
                        // Add NWDItem Ask to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsAsked.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Trade Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                default:
                    break;
            }

            // Sync NWDUserOwnership
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserOwnership) });
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UserCanBuy()
        {
            bool rCanBuy = false;

            // Check Pack Cost
            foreach (KeyValuePair<NWDItem, int> pair in ItemsAsked.GetObjectAndQuantity())
            {
                // Get Item Cost data
                NWDItem tNWDItem = pair.Key;
                int tItemQte = pair.Value;

                rCanBuy = true;

                if (NWDUserOwnership.OwnershipForItemExists(tNWDItem))
                {
                    if (NWDUserOwnership.OwnershipForItem(tNWDItem).Quantity < tItemQte)
                    {
                        // User don't have enough item
                        rCanBuy = false;
                        break;
                    }
                }
                else
                {
                    // User don't have the selected item
                    rCanBuy = false;
                    break;
                }
            }

            return rCanBuy;
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
            ItemsProposed = null;
            ItemsAsked = null;
            LimitDayTime = null;
            TradeStatus = NWDTradeStatus.None;
            SaveData();
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
        }//-------------------------------------------------------------------------------------------------------------
        public static string AddonPhpPreCalculate()
        {
            string t_THIS_TradeStatus = FindAliasName("TradeStatus");
            string t_THIS_TradeHash = FindAliasName("TradeHash");
            string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            int t_THIS_Index_TradeStatus = CSVAssemblyIndexOf(t_THIS_TradeStatus);
            int t_THIS_Index_TradeHash = CSVAssemblyIndexOf(t_THIS_TradeHash);
            int t_THIS_Index_WinnerProposition = CSVAssemblyIndexOf(t_THIS_WinnerProposition);
            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            int t_THIS_Index_ItemsProposed = CSVAssemblyIndexOf(t_THIS_ItemsProposed);
            string t_THIS_ItemsAsked = FindAliasName("ItemsAsked");
            int t_THIS_Index_ItemsAsked = CSVAssemblyIndexOf(t_THIS_ItemsAsked);
            string sScript = "" +
                "// start Addon \n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeHash + "` FROM `'.$ENV.'_" + Datas().ClassNamePHP + "` " +
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
                "$tServerHash = $tRowStatus['" + t_THIS_TradeHash + "'];\n" +
                "}\n" +
                "}\n" +
                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED
                "if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")\n" +
                "{\n" +
                //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
                "}\n" +
                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                "{\n" +
                "$sReplaces[" + t_THIS_Index_TradeHash + "] = $TIME_SYNC;\n" +
                "$sReplaces[" + t_THIS_Index_TradeStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
                "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                "}\n" +
                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() +" && (" +
                "$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
                //" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() + 
                "))\n" +
                "{\n" +
                "$sReplaces[" + t_THIS_Index_TradeHash + "] = $TIME_SYNC;\n" +
                "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                "$sReplaces["+ t_THIS_Index_ItemsAsked + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                "}\n" +
                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                "{\n" +
                "$tQueryCancelable = 'UPDATE `'.$ENV.'_" + Datas().ClassNamePHP + "` SET " +
                "`DM` = \\''.$TIME_SYNC.'\\', " +
                "`DS` = \\''.$TIME_SYNC.'\\', " +
                "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                "`" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                "AND `" + t_THIS_TradeStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "';" +
                "$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
                "if (!$tResultCancelable)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "else" +
                "\n" +
                "{\n" +
                "$tNumberOfRow = 0;\n" +
                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                "if ($tNumberOfRow == 1)\n" +
                "{\n" +
                "// I can change data to expired!\n" +
                "Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
               "}\n" +
                "else\n" +
                "{\n" +
                //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "//stop the function!\n" +
                "myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                "return;\n" +
                "}\n" +
                "}\n" +
                "}\n" +
                // OTHER
                "else\n" +
                "{\n" +
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