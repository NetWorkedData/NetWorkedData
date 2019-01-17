//=====================================================================================================================
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
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDUserBarterPropositionConnection : NWDConnection<NWDUserBarterProposition>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBPR")]
    [NWDClassDescriptionAttribute("User Barter Proposition descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Proposition")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterProposition : NWDBasis<NWDUserBarterProposition>
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
        [NWDAlias("BarterPlace")]
        public NWDReferenceType<NWDBarterPlace> BarterPlace
        {
            get; set;
        }
        [NWDAlias("BarterRequest")]
        public NWDReferenceType<NWDUserBarterRequest> BarterRequest
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Barter References", true, true, true)]
        [NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        [NWDAlias("ItemsSend")]
        public NWDReferencesQuantityType<NWDItem> ItemsSend
        {
            get; set;
        }
        [NWDAlias("BarterStatus")]
        public NWDTradeStatus BarterStatus
        {
            get; set;
        }
        [NWDAlias("BarterRequestHash")]
        public NWDDateTimeUtcType BarterRequestHash
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void barterProposalBlock(bool result, NWDOperationResult infos);
        public barterProposalBlock barterProposalBlockDelegate;
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterProposition()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterProposition(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
        public static NWDUserBarterProposition CreateBarterProposalWith(NWDUserBarterRequest sRequest)
        {
            // Create a new Proposal
            NWDUserBarterProposition tProposition = NewData();
#if UNITY_EDITOR
            tProposition.InternalKey = NWDAccountNickname.GetNickname();
#endif
            tProposition.Tag = NWDBasisTag.TagUserCreated;
            tProposition.BarterPlace.SetObject(sRequest.BarterPlace.GetObject());
            tProposition.BarterRequest.SetObject(sRequest);
            tProposition.ItemsProposed.SetReferenceAndQuantity(sRequest.ItemsProposed.GetReferenceAndQuantity());
            //tProposition.ItemsSend.SetReferenceAndQuantity(sRequest.ItemsReceived.GetReferenceAndQuantity());
            tProposition.BarterStatus = NWDTradeStatus.Active;
            tProposition.BarterRequestHash.SetTimeStamp(sRequest.DM);
            tProposition.SaveData();

            return tProposition;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterProposal()
        {
            List<Type> tLists = new List<Type>() {
                typeof(NWDUserBarterProposition),
                typeof(NWDUserBarterRequest),
                typeof(NWDUserBarterFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterProposalBlockDelegate != null)
                {
                    barterProposalBlockDelegate(true, null);
                }

                AddAndRemoveItems();
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterProposalBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterProposalBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Cancel()
        {
            BarterStatus = NWDTradeStatus.Cancel;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Clean()
        {
            BarterPlace = null;
            BarterRequest = null;
            ItemsProposed = null;
            //ItemsAsked = null;
            BarterRequestHash = null;
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AddAndRemoveItems()
        {
            if (BarterStatus == NWDTradeStatus.Accepted)
            {
                // Add NWDItem to NWDUserOwnership
                Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                {
                    NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                }

                // Remove NWDItem to NWDUserOwnership
                //Dictionary<NWDItem, int> tAsked = ItemsAsked.GetObjectAndQuantity();
                //foreach (KeyValuePair<NWDItem, int> pair in tAsked)
                //{
                //    NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                //}

                // Set Barter Proposition to None, so we can reused an old slot for a new transaction
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

            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "fixe date of BarterRequest DM", tMiniButtonStyle))
            {
                Debug.Log("YES ? or Not " + BarterRequest.Value);
                NWDUserBarterRequest tRequest = BarterRequest.GetObjectAbsolute();
                if (tRequest != null)
                {
                    Debug.Log("YES");
                    BarterRequestHash.SetLong(tRequest.DM);
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

            string tBarterStatus = NWDUserBarterRequest.FindAliasName("BarterStatus");
            string tLimitDayTime = NWDUserBarterRequest.FindAliasName("LimitDayTime");
            string tBarterPlace = NWDUserBarterRequest.FindAliasName("BarterPlace");
            string tBarterRequest = NWDUserBarterRequest.FindAliasName("BarterRequest");
            string tWinnerProposition = NWDUserBarterRequest.FindAliasName("WinnerProposition");
            string tPropositions = NWDUserBarterRequest.FindAliasName("Propositions");
            string tMaxPropositions = NWDUserBarterRequest.FindAliasName("MaxPropositions");
            string tPropositionsCounter = NWDUserBarterRequest.FindAliasName("PropositionsCounter");

            string t_THIS_BarterRequestHash = FindAliasName("BarterRequestHash");
            string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            string t_THIS_BarterRequest = FindAliasName("BarterRequest");
            string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            int t_THIS_Index_BarterRequestHash = CSVAssemblyIndexOf(t_THIS_BarterRequestHash);
            int t_THIS_Index_BarterPlace = CSVAssemblyIndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_BarterRequest = CSVAssemblyIndexOf(t_THIS_BarterRequest);
            int t_THIS_Index_BarterStatus = CSVAssemblyIndexOf(t_THIS_BarterStatus);

            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            string t_THIS_ItemsSend = FindAliasName("ItemsSend");
            int t_THIS_Index_ItemsProposed = CSVAssemblyIndexOf(t_THIS_ItemsProposed);
            int t_THIS_Index_ItemsSend = CSVAssemblyIndexOf(t_THIS_ItemsSend);

            string sScript = "" +
                "// debut find \n" +
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserBarterRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +
                // get the actual state
                "$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
                "$tServerHash = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterRequestHash + "` FROM `'.$ENV.'_" + Datas().ClassNamePHP + "` " +
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';" +
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
                "$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];\n" +
                "$tServerHash = $tRowStatus['" + t_THIS_BarterRequestHash + "'];\n" +
                "}\n" +
                "}\n" +
                "\n" +
                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() + ")\n" +
                "{\n" +
                //"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
                "}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                "{\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                "}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && " +
                "($tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() + " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() + "))\n" +
                "{\n" +
                "$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_ItemsSend + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_BarterRequestHash + "]='';\n" +
                "$sReplaces[" + t_THIS_Index_BarterRequest + "]='';\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
                "}\n" +
                // change the statut from CSV TO CANCEL 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
                "{\n" +
                "$tQueryCancelable = 'UPDATE `'.$ENV.'_" + Datas().ClassNamePHP + "` SET " +
                "`DM` = \\''.$TIME_SYNC.'\\', " +
                "`DS` = \\''.$TIME_SYNC.'\\', " +
                "`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
                "`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\' " +
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                "AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
                "';" +
                "$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
                "if (!$tResultCancelable)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "else" +
                "{\n" +
                "$tNumberOfRow = 0;\n" +
                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                "if ($tNumberOfRow == 1)\n" +
                "{\n" +
                "Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
                "GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                "return;\n" +
                "\n" +
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
            string t_THIS_BarterRequest = FindAliasName("BarterRequest");
            int t_THIS_Index_BarterRequest = CSVAssemblyIndexOf(t_THIS_BarterRequest);

            return "// write your php script here to update after sync on server\n " +
                "GetDatas" + NWDUserBarterRequest.Datas().ClassNamePHP + "ByReference ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n";
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