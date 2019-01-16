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
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsSend
        {
            get; set;
        }
        [NWDAlias("BarterStatus")]
        public NWDBarterStatus BarterStatus
        {
            get; set;
        }
        [NWDAlias("BarterRequestDM")]
        public NWDDateTimeUtcType BarterRequestDM
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
            tProposition.BarterStatus = NWDBarterStatus.Active;
            tProposition.BarterRequestDM.SetTimeStamp(sRequest.DM);
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
            BarterStatus = NWDBarterStatus.Cancel;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Clean()
        {
            BarterPlace = null;
            BarterRequest = null;
            ItemsProposed = null;
            //ItemsAsked = null;
            BarterRequestDM = null;
            BarterStatus = NWDBarterStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void AddAndRemoveItems()
        {
            if (BarterStatus == NWDBarterStatus.Accepted)
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
                    BarterRequestDM.SetLong(tRequest.DM);
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

            string t_THIS_BarterRequestDM = FindAliasName("BarterRequestDM");
            string t_THIS_BarterPlace = FindAliasName("BarterPlace");
            string t_THIS_BarterRequest = FindAliasName("BarterRequest");
            string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            int t_THIS_Index_tBarterRequestDM = CSVAssemblyIndexOf(t_THIS_BarterRequestDM);
            int t_THIS_Index_BarterPlace = CSVAssemblyIndexOf(t_THIS_BarterPlace);
            int t_THIS_Index_BarterRequest = CSVAssemblyIndexOf(t_THIS_BarterRequest);
            int t_THIS_Index_BarterStatus = CSVAssemblyIndexOf(t_THIS_BarterStatus);
            string sScript = "" +
                "// debut find \n" +
                // YOU MUST REIMPORT THE GLOBAL ... PHP strange practice?
                "include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserBarterRequest.Datas().ClassNamePHP + "/synchronization.php');\n" +

                "\n" +
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Cancel).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Expired).ToString() + ")\n" +
                "{\n" +
                // error ou
                //"error('UTRRx99');\n" +
                //"return;\n" +
                // none ... faudra trancher : none pour avoir une sync 
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", '" + ((int)NWDBarterStatus.None).ToString() + "');\n" +
                "}\n" +
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Active).ToString() + ")\n" +
                "{\n" +
                "$tQueryBarter = 'UPDATE `'.$ENV.'_" + NWDUserBarterRequest.Datas().ClassNamePHP + "` SET " +
                " `DM` = \\''.$TIME_SYNC.'\\'," +
                " `DS` = \\''.$TIME_SYNC.'\\'," +
                " `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\'," +
                " `" + tWinnerProposition + "` = \\''.$sCsvList[0].'\\'," +
                " `" + tBarterStatus + "` = \\'" + ((int)NWDBarterStatus.Accepted).ToString() + "\\'" +
                // WHERE REQUEST
                " WHERE `AC`= \\'1\\' " +
                " AND `" + tBarterStatus + "` = \\'" + ((int)NWDBarterStatus.Active).ToString() + "\\' " +
                " AND `" + tBarterPlace + "` = \\''.$sCsvList[" + t_THIS_Index_BarterPlace + "].'\\' " +
                " AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_BarterRequest + "].'\\' " +
                " AND `DM` = \\''.$sCsvList[" + t_THIS_Index_tBarterRequestDM + "].'\\' " +
                " AND `" + tLimitDayTime + "` > '.$TIME_SYNC.' " +
                // ORDER BY 
                //"ORDER BY `" + tLimitDayTime + "` " +
                "';\n" +
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
                "$tNumberOfRow = 0;\n" +
                "$tNumberOfRow = $SQL_CON->affected_rows;\n" +
                "if ($tNumberOfRow == 1)\n" +
                "{\n" +
                "// I need update the proposition too !\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDBarterStatus.Accepted).ToString() + "\');\n" +
                "myLog('I need update the proposition accept', __FILE__, __FUNCTION__, __LINE__);\n" +
                "Integrity" + NWDUserBarterRequest.Datas().ClassNamePHP + "Reevalue ($sCsvList[" + t_THIS_Index_BarterRequest + "]);\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", \'" + ((int)NWDBarterStatus.Expired).ToString() + "\');\n" +
                "\tmyLog('I need update the proposition refused ... too late!', __FILE__, __FUNCTION__, __LINE__);\n" +
                "}\n" +
                "}\n" +
                "}\n" +
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Accepted).ToString() + ")\n" +
                "{\n" +
                "// this case must be cancelled ?\n" +
                "}\n" +
                "// fin find \n";

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