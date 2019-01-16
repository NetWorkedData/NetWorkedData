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
    public class NWDUserBarterRequestConnection : NWDConnection<NWDUserBarterRequest>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBRR")]
    [NWDClassDescriptionAttribute("User Barter Request descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Request")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterRequest : NWDBasis<NWDUserBarterRequest>
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

        [NWDGroupStart("Barter References", true, true, true)]
        [NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemsSuggested
        {
            get; set;
        }
        [NWDAlias("ItemsReceived")]
        public NWDReferencesQuantityType<NWDItem> ItemsReceived
        {
            get; set;
        }
        [NWDAlias("BarterStatus")]
        public NWDBarterStatus BarterStatus
        {
            get; set;
        }
        [NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime
        {
            get; set;
        }
        [NWDAlias("MaxPropositions")]
        public int MaxPropositions
        {
            get; set;
        }
        [NWDAlias("PropositionsCounter")]
        public int PropositionsCounter
        {
            get; set;
        }
        [NWDAlias("Propositions")]
        public NWDReferencesListType<NWDUserBarterProposition> Propositions
        {
            get; set;
        } // ON VA LIMITER LES NOMBRE DE PROPOSITION DANS LE RESULTAT DE LA SYNCHRO! ... MAIS COMMENT FAIRE LA SYNCHRO?
        [NWDAlias("WinnerProposition")]
        public NWDReferenceType<NWDUserBarterProposition> WinnerProposition
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
        public delegate void barterRequestBlock(bool result, NWDOperationResult infos);
        public barterRequestBlock barterRequestBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterRequest()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserBarterRequest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            ForRelationshipOnly = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed, Dictionary<string, int> sAsked)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;

            // Create a new Request
            NWDUserBarterRequest tRequest = NewData();
#if UNITY_EDITOR
            tRequest.InternalKey = NWDAccountNickname.GetNickname(); // + " - " + sProposed.Name.GetBaseString();
#endif
            tRequest.Tag = NWDBasisTag.TagUserCreated;
            tRequest.BarterPlace.SetObject(sBarterPlace);
            tRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
            //tRequest.ItemsReceived.SetReferenceAndQuantity(sAsked);
            tRequest.BarterStatus = NWDBarterStatus.Active;
            tRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
            tRequest.SaveData();

            return tRequest;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest[] FindRequestsWith(NWDBarterPlace sBarterPlace)
        {
            List<NWDUserBarterRequest> tUserBartersRequest = new List<NWDUserBarterRequest>();
            foreach (NWDUserBarterRequest k in FindDatas())
            {
                if (k.BarterPlace.GetReference().Equals(sBarterPlace.Reference))
                {
                    tUserBartersRequest.Add(k);
                }
            }

            return tUserBartersRequest.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void SyncBarterRequest()
        {
            List<Type> tLists = new List<Type>() {
                typeof(NWDUserBarterProposition),
                typeof(NWDUserBarterRequest),
                typeof(NWDUserBarterFinder),
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterRequestBlockDelegate != null)
                {
                    barterRequestBlockDelegate(true, null);
                }

                AddOrRemoveItems();
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterRequestBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterRequestBlockDelegate(false, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tLists, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddOrRemoveItems()
        {
            switch (BarterStatus)
            {
                case NWDBarterStatus.Active:
                    {
                        // Remove NWDItem from NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
                        }
                    }
                    break;
                case NWDBarterStatus.Expired:
                    {
                        // Add NWDItem to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsProposed.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                case NWDBarterStatus.Accepted:
                    {
                        // Add NWDItem Ask to NWDUserOwnership
                        Dictionary<NWDItem, int> tProposed = ItemsReceived.GetObjectAndQuantity();
                        foreach (KeyValuePair<NWDItem, int> pair in tProposed)
                        {
                            NWDUserOwnership.AddItemToOwnership(pair.Key, pair.Value);
                        }

                        // Set Barter Proposition to None, so we can reused an old slot for a new transaction
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
            foreach (KeyValuePair<NWDItem, int> pair in ItemsReceived.GetObjectAndQuantity())
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
            BarterStatus = NWDBarterStatus.Cancel;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Clean()
        {
            BarterPlace = null;
            ItemsProposed = null;
            //ItemsAsked = null;
            LimitDayTime = null;
            BarterStatus = NWDBarterStatus.None;
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
            string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            int t_THIS_Index_BarterStatus = CSVAssemblyIndexOf(t_THIS_BarterStatus);
            string sScript = "" +
                "// debut find \n" +
                "\n" +
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Accepted).ToString() + " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Expired).ToString() + ")\n" +
                "{\n" +
                // error ou
                //"error('UTRRx99');\n" +
                //"return;\n" +
                // none ... faudra trancher : none pour avoir une sync 
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", '" + ((int)NWDBarterStatus.None).ToString() + "');\n" +
                "}\n" +
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDBarterStatus.Cancel).ToString() + ")\n" +
                "{\n" +
                "$tQueryCancelable = 'SELECT `Reference` FROM `'.$ENV.'_" + Datas().ClassNamePHP + "` WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
                " AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDBarterStatus.Accepted).ToString() + "\\' " +
                "';" +
                "$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
                "if (!$tResultCancelable)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('UTRRx31');\n" +
                "}\n" +
                "else" +
                "\n" +
                "{\n" +
                "if ($tResultCancelable->num_rows > 0)\n" +
                "{\n" +
                "mysqli_free_result($tResultCancelable);\n" +
                "//stop the function!\n" +
                "myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
                "return;\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "mysqli_free_result($tResultCancelable);\n" +
                "// I can change data to expired!\n" +
                "$sCsvList = Integrity" + Datas().ClassNamePHP + "Replace ($sCsvList, " + t_THIS_Index_BarterStatus + ", '" + ((int)NWDBarterStatus.Expired).ToString() + "');" +
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