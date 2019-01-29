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
        [NWDAlias("ItemsSuggested")]
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
        public NWDTradeStatus BarterStatus
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("BarterHash")]
        public string BarterHash
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
            tRequest.BarterStatus = NWDTradeStatus.Active;
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

                        // Set Barter Proposition to None, so we can reused an old slot for a new transaction
                        Clean();
                    }
                    break;
                case NWDTradeStatus.Accepted:
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
            BarterStatus = NWDTradeStatus.Cancel;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Clean()
        {
            BarterPlace = null;
            ItemsProposed = null;
            //ItemsAsked = null;
            LimitDayTime = null;
            BarterStatus = NWDTradeStatus.None;
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
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string AddonPhpFunctions(NWDAppEnvironment sAppEnvironment)
		{
			string sScript = "";
			return sScript;
		}
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            string tBarterStatus = NWDUserBarterProposition.FindAliasName("BarterStatus");
            string tBarterRequest = NWDUserBarterProposition.FindAliasName("BarterRequest");
            string tBarterRequestHash = NWDUserBarterProposition.FindAliasName("BarterRequestHash");

            string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
            string t_THIS_Propositions = FindAliasName("Propositions");
			string t_THIS_PropositionsCounter = FindAliasName("PropositionsCounter");
			string t_THIS_BarterStatus = FindAliasName("BarterStatus");
            string t_THIS_BarterHash = FindAliasName("BarterHash");

            int t_THIS_Index_WinnerProposition = CSVAssemblyIndexOf(t_THIS_WinnerProposition);
            int t_THIS_Index_Propositions = CSVAssemblyIndexOf(t_THIS_Propositions);
			int t_THIS_Index_PropositionsCounter = CSVAssemblyIndexOf(t_THIS_PropositionsCounter);
			int t_THIS_Index_BarterStatus = CSVAssemblyIndexOf(t_THIS_BarterStatus);
            int t_THIS_Index_BarterHash = CSVAssemblyIndexOf(t_THIS_BarterHash);

            string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
            int t_THIS_Index_ItemsProposed = CSVAssemblyIndexOf(t_THIS_ItemsProposed);
            string t_THIS_ItemsSuggested = FindAliasName("ItemsSuggested");
            int t_THIS_Index_ItemsSuggested = CSVAssemblyIndexOf(t_THIS_ItemsSuggested);
            string t_THIS_ItemsReceived = FindAliasName("ItemsReceived");
            int t_THIS_Index_ItemsReceived = CSVAssemblyIndexOf(t_THIS_ItemsReceived);

            string sScript = "" +
                "// start Addon \n" +
				"include_once ( $PATH_BASE.'/Environment/'.$ENV.'/Engine/Database/" + NWDUserBarterProposition.Datas().ClassNamePHP + "/synchronization.php');\n" +
				// get the actual state
				"$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() +";\n" +
                "$tServerHash = '';\n" +
                "$tServerPropositions = '';\n" +
                "$tQueryStatus = 'SELECT `" + t_THIS_BarterStatus + "`, `" + t_THIS_BarterHash + "`, `" + t_THIS_Propositions + "` FROM `'.$ENV.'_" + Datas().ClassNamePHP + "` " + 
                "WHERE " +
                "`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';" +
                "$tResultStatus = $SQL_CON->query($tQueryStatus);\n" +
                "if (!$tResultStatus)\n" +
                	"{\n" +
						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultStatus.'', __FILE__, __FUNCTION__, __LINE__);\n" +
						"error('SERVER');\n" +
					"}\n" +
                "else" +
                	"{\n" +
               			"if ($tResultStatus->num_rows == 1)\n" +
                			"{\n" +
               					"$tRowStatus = $tResultStatus->fetch_assoc();\n" +
               					"$tServerStatut = $tRowStatus['" + t_THIS_BarterStatus + "'];\n" +
               					"$tServerHash = $tRowStatus['" + t_THIS_BarterHash + "'];\n" +
                				"$tServerPropositions = $tRowStatus['" + t_THIS_Propositions + "'];\n" +
                			"}\n" +
               		"}\n" +

                // change the statut from CSV TO WAITING, ACCEPTED, EXPIRED, CANCELLED
                "if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Waiting).ToString() + 
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Refresh).ToString() +
                " || $sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() +
                ")\n" +
                	"{\n" +
               			//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
               			"GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
                		"return;\n" +
                	"}\n" +

                // change the statut from CSV TO ACTIVE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Active).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
                	"{\n" +
                		"$sReplaces[" + t_THIS_Index_BarterHash + "] = $TIME_SYNC;\n" +
               			"$sReplaces[" + t_THIS_Index_BarterStatus + "]="+ ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
						"$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
						"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
						"$sCsvList = Integrity" + Datas().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
					"}\n" +

                // change the statut from CSV TO NONE 
                "else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (" +
                "$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
                " || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
				" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
				"))\n" +
					"{\n" +
						"$sReplaces[" + t_THIS_Index_BarterHash + "] = $TIME_SYNC;\n" +
						"$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
						"$sReplaces[" + t_THIS_Index_ItemsSuggested + "]='';\n" +
						"$sReplaces[" + t_THIS_Index_ItemsReceived + "]='';\n" +
						"$sReplaces[" + t_THIS_Index_Propositions + "]='';\n" +
						"$sReplaces[" + t_THIS_Index_PropositionsCounter + "]='0';\n" +
						"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
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
						"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
						"WHERE " +
						"`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
						"AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
						"';" +
						"$tResultCancelable = $SQL_CON->query($tQueryCancelable);\n" +
						"if (!$tResultCancelable)\n" +
							"{\n" +
								"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
								"error('SERVER');\n" +
							"}\n" +
						"else" +
							"{\n" +
								"$tNumberOfRow = 0;\n" +
								"$tNumberOfRow = $SQL_CON->affected_rows;\n" +
								"if ($tNumberOfRow == 1)\n" +
									"{\n" +
										// START CANCEL PUT PROPOSITION TO EXPIRED
										"// I need to put all propositions in Expired\n" +
										"$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.Datas().ClassNamePHP + "` SET " +
										"`DM` = \\''.$TIME_SYNC.'\\', " +
										"`DS` = \\''.$TIME_SYNC.'\\', " +
										"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
										"`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
										"WHERE " +
										"`"+ tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
										"AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
										"`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
										"AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
										"';\n" +
										"$tResultExpired = $SQL_CON->query($tQueryExpired);" +
										"if (!$tResultExpired)\n" +
											"{\n" +
												"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
												"error('SERVER');\n" +
											"}\n" +
										"else" +
											"{\n" +
												"$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterProposition.Datas().ClassNamePHP + "`" +
												"WHERE " +
												"`"+ tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
												"AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
												"AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
												"';\n" +
												"$tResultExpired = $SQL_CON->query($tQueryExpired);" +
												"if (!$tResultExpired)\n" +
													"{\n" +
														"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
														"error('SERVER');\n" +
														"}\n" +
												"else" +
													"{\n" +
														"while ($tRowExpired = $tResultExpired->fetch_row())\n" +
															"{\n" +
																"myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
																"Integrity" + NWDUserBarterProposition.Datas().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
															"}\n" +
													"}\n" +
											"}\n" +
										// FINISH CANCEL PUT PROPOSITION TO EXPIRED
										"// I can integrate data to expired!\n" +
										"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
									"}\n" +
							"}\n" +
						"GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
						"//stop the function!\n" +
						"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
						"return;\n" +
					"}\n" +

				// change the statut from CSV TO DEAL 
				"else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Deal).ToString() + " && " +
                "$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
					"{\n" +
						"$tQueryDeal = 'UPDATE `'.$ENV.'_" + Datas().ClassNamePHP + "` SET " +
						"`DM` = \\''.$TIME_SYNC.'\\', " +
						"`DS` = \\''.$TIME_SYNC.'\\', " +
						"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
						"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\' " +
						"WHERE " +
						"`Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
						"AND `" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' " +
						"';" + 
						"// I need to put winner propositions to Accepted Or it's reject?\n" +
						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
						"$tResultDeal = $SQL_CON->query($tQueryDeal);\n" +
						"if (!$tResultDeal)\n" +
							"{\n" +
								"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tResultCancelable.'', __FILE__, __FUNCTION__, __LINE__);\n" +
								"error('UBRRx31');\n" +
							"}\n" +
						"else" +
							"{\n" +
								"// I need to put Accepted or expired in this request?\n" +
								"$tNumberOfRow = 0;\n" +
								"$tNumberOfRow = $SQL_CON->affected_rows;\n" +
								"if ($tNumberOfRow == 1)\n" +
									"{\n" +
									"// I need to put all propositions in Expired\n" +
										"$tQueryAccepted = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.Datas().ClassNamePHP + "` SET " +
										"`DM` = \\''.$TIME_SYNC.'\\', " +
										"`DS` = \\''.$TIME_SYNC.'\\', " +
										"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
										"`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
										"WHERE " +
										"`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
										"AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\'" +
										"AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
										"AND `Reference` = \\''.$sCsvList[" + t_THIS_Index_WinnerProposition + "].'\\' " +
										"';\n" +
										"$tResultAccepted = $SQL_CON->query($tQueryAccepted);" +
										"if (!$tResultAccepted)\n" +
											"{\n" +
												"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAccepted.'', __FILE__, __FUNCTION__, __LINE__);\n" +
												"error('SERVER');\n" +
											"}\n" +
										"else" +
											"{\n" +
												"// I need to put Accepted or expired in this request?\n" +
												"$tNumberOfRow = 0;\n" +
												"$tNumberOfRow = $SQL_CON->affected_rows;\n" +
												"if ($tNumberOfRow == 1)\n" +
													"{\n" +
														"$tQueryAcceptedDeal = 'UPDATE `'.$ENV.'_" + Datas().ClassNamePHP + "` SET " +
														"`DM` = \\''.$TIME_SYNC.'\\', " +
														"`DS` = \\''.$TIME_SYNC.'\\', " +
														"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
														"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Accepted).ToString() + "\\' " +
														"WHERE " +
														"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
														"AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
														"AND `Reference` = \\''.$tReference.'\\' " +
														"';\n" +
														"$tResultAcceptedDeal = $SQL_CON->query($tQueryAcceptedDeal);" +
														"if (!$tResultAcceptedDeal)\n" +
															"{\n" +
																"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryAcceptedDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
																"error('SERVER');\n" +
															"}\n" +
													"}\n" +
												"else\n" +
													"{\n" +
														"$tQueryExpiredDeal = 'UPDATE `'.$ENV.'_" + Datas().ClassNamePHP + "` SET " +
														"`DM` = \\''.$TIME_SYNC.'\\', " +
														"`DS` = \\''.$TIME_SYNC.'\\', " +
														"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
														"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
														"WHERE " +
														"`" + t_THIS_BarterStatus + "` = \\'" + ((int)NWDTradeStatus.Deal).ToString() + "\\'" +
														"AND `" + t_THIS_BarterHash + "` = \\''.$tServerHash.'\\' " +
														"AND `Reference` = \\''.$tReference.'\\' " +
														"';\n" +
														"$tResultExpiredDeal = $SQL_CON->query($tQueryExpiredDeal);" +
														"if (!$tResultExpiredDeal)\n" +
															"{\n" +
																"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpiredDeal.'', __FILE__, __FUNCTION__, __LINE__);\n" +
																"error('SERVER');\n" +
															"}\n" +
													"}\n" +
											"}\n" +
										// START CANCEL PUT PROPOSITION TO EXPIRED
										"// I need to put all propositions in Expired\n" +
										"$tQueryExpired = 'UPDATE `'.$ENV.'_" + NWDUserBarterProposition.Datas().ClassNamePHP + "` SET " +
										"`DM` = \\''.$TIME_SYNC.'\\', " +
										"`DS` = \\''.$TIME_SYNC.'\\', " +
										"`'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\', " +
										"`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
										"WHERE " +
										"`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
										"AND (`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Waiting).ToString() + "\\' OR " +
										"`" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Cancelled).ToString() + "\\') " +
										"AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
										"';\n" +
										"$tResultExpired = $SQL_CON->query($tQueryExpired);" +
										"if (!$tResultExpired)\n" +
											"{\n" +
												"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
												"error('SERVER');\n" +
											"}\n" +
										"else" +
											"{\n" +
												"$tQueryExpired = 'SELECT `Reference` FROM `'.$ENV.'_" + NWDUserBarterProposition.Datas().ClassNamePHP + "`" +
												"WHERE " +
												"`" + tBarterRequest + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
												"AND `" + tBarterStatus + "` = \\'" + ((int)NWDTradeStatus.Expired).ToString() + "\\' " +
												"AND `" + tBarterRequestHash + "` = \\''.$tServerHash.'\\' " +
												"';\n" +
												"$tResultExpired = $SQL_CON->query($tQueryExpired);" +
												"if (!$tResultExpired)\n" +
													"{\n" +
														"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQueryExpired.'', __FILE__, __FUNCTION__, __LINE__);\n" +
														"error('SERVER');\n" +
														"}\n" +
												"else" +
													"{\n" +
														"while ($tRowExpired = $tResultExpired->fetch_row())\n" +
															"{\n" +
																"myLog('cancel proposition too : ref = '.$tRowExpired[0], __FILE__, __FUNCTION__, __LINE__);\n" +
																"Integrity" + NWDUserBarterProposition.Datas().ClassNamePHP + "Reevalue ($tRowExpired[0]);\n" +
															"}\n" +
													"}\n" +
											"}\n" +
										// FINISH CANCEL PUT PROPOSITION TO EXPIRED
										"// I can integrate data to expired!\n" +
										"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
									"}\n" +
							"}\n" +
						"GetDatas" + Datas().ClassNamePHP + "ByReference ($tReference);\n" +
						"//stop the function!\n" +
						"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
						"return;\n" +
					"}\n" +

				// change the statut from CSV TO FORCE // ADMIN ONLY 
				"else if ($sCsvList[" + t_THIS_Index_BarterStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
					"{\n" +
					"//EXECEPTION FOR ADMIN\n" +
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
        public static string AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            return "// write your php script here to update afetr sync on server\n";
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