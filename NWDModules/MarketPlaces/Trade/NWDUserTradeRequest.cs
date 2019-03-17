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
	[NWDClassServerSynchronize(true)]
	[NWDClassTrigramme("UTR")]
	[NWDClassDescription("User Trade Request descriptions Class")]
	[NWDClassMenuName("User Trade Request")]
	[NWDForceSecureDataAttribute]
	public partial class NWDUserTradeRequest : NWDBasis<NWDUserTradeRequest>
	{
		//-------------------------------------------------------------------------------------------------------------
		[NWDInspectorGroupStart("Trade Detail", true, true, true)]
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
		[NWDInspectorGroupEnd]

		//[NWDGroupSeparator]

		[NWDInspectorGroupStart("For Relationship Only", true, true, true)]

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
		[NWDInspectorGroupEnd]

		//[NWDGroupSeparator]

		[NWDInspectorGroupStart("Trade References", true, true, true)]
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
        [NWDNotEditable]
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
		[NWDInspectorGroupEnd]

		//[NWDGroupSeparator]

		[NWDInspectorGroupStart("Tags", true, true, true)]
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
		public delegate void tradeRequestBlock(bool error, NWDTradeStatus status, NWDOperationResult result);
		public tradeRequestBlock tradeRequestBlockDelegate;
		//-------------------------------------------------------------------------------------------------------------
		public NWDUserTradeRequest()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDUserTradeRequest(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Initialization()
		{
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDTradePlace), typeof(NWDUserTradeRequest), typeof(NWDUserTradeProposition) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTradeRequest CreateTradeRequestWith(NWDTradePlace sTradePlace, Dictionary<string, int> sProposed, Dictionary<string, int> sAsked)
		{
			// Get Request Life time
			int tLifetime = sTradePlace.RequestLifeTime;

            // Create a new Proposal
            NWDUserTradeRequest rRequest = FindEmptySlot();

            #if UNITY_EDITOR
            rRequest.InternalKey = NWDUserNickname.GetNickname() + " - " + sTradePlace.InternalKey;
            #endif
			rRequest.Tag = NWDBasisTag.TagUserCreated;
			rRequest.TradePlace.SetObject(sTradePlace);
			rRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
			rRequest.ItemsAsked.SetReferenceAndQuantity(sAsked);
			rRequest.TradeStatus = NWDTradeStatus.Submit;
			rRequest.LimitDayTime.SetDateTime(DateTime.UtcNow.AddSeconds(tLifetime));
			rRequest.SaveData();

			return rRequest;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDUserTradeRequest[] FindRequestsWith(NWDTradePlace sTradePlace)
		{
			List<NWDUserTradeRequest> rUserTradesRequest = new List<NWDUserTradeRequest>();
			foreach (NWDUserTradeRequest k in FindDatas())
			{
				if (k.TradePlace.GetReference().Equals(sTradePlace.Reference))
				{
					rUserTradesRequest.Add(k);
				}
			}

			return rUserTradesRequest.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SyncTradeRequest()
		{
            // Sync NWDUserTradeRequest
            SynchronizationFromWebService(TradeRequestSuccessBlock, TradeRequestFailedBlock);
        }
		//-------------------------------------------------------------------------------------------------------------
		public void AddOrRemoveItems()
		{
			switch (TradeStatus)
			{
				case NWDTradeStatus.Waiting:
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
            SynchronizationFromWebService();
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
		public void CancelRequest()
		{
			TradeStatus = NWDTradeStatus.Cancel;
			SaveData();

            // Sync NWDUserTradeRequest
            SynchronizationFromWebService(TradeRequestSuccessBlock, TradeRequestFailedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        void TradeRequestFailedBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            if (tradeRequestBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                tradeRequestBlockDelegate(true, NWDTradeStatus.None, tResult);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void TradeRequestSuccessBlock(BTBOperation sOperation, float sProgress, BTBOperationResult sResult)
        {
            // Keep TradeStatus before Clean()
            NWDTradeStatus tTradeStatus = TradeStatus;

            // Do action with Items & Sync
            AddOrRemoveItems();

            if (tradeRequestBlockDelegate != null)
            {
                NWDOperationResult tResult = sResult as NWDOperationResult;
                tradeRequestBlockDelegate(false, tTradeStatus, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Clean()
		{
            TradePlace.Flush();
            ItemsProposed.Flush();
            ItemsAsked.Flush();
            LimitDayTime.Flush();
			TradeStatus = NWDTradeStatus.None;
			SaveData();
		}
        //-------------------------------------------------------------------------------------------------------------
        static NWDUserTradeRequest FindEmptySlot()
        {
            NWDUserTradeRequest rSlot = null;

            // Search for a empty NWDUserTradeRequest Slot
            foreach (NWDUserTradeRequest k in FindDatas())
            {
                if (k.TradeStatus == NWDTradeStatus.None)
                {
                    rSlot = k;
                    break;
                }
            }

            // Create a new Proposal if null
            if (rSlot == null)
            {
                rSlot = NewData();
            }

            return rSlot;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_AddonPhpPreCalculate)]
        public static string AddonPhpPreCalculate(NWDAppEnvironment sAppEnvironment)
		{
			string t_THIS_TradeStatus = FindAliasName("TradeStatus");
			string t_THIS_TradeHash = FindAliasName("TradeHash");
			string t_THIS_WinnerProposition = FindAliasName("WinnerProposition");
			int t_THIS_Index_TradeStatus = CSV_IndexOf(t_THIS_TradeStatus);
			int t_THIS_Index_TradeHash = CSV_IndexOf(t_THIS_TradeHash);
			int t_THIS_Index_WinnerProposition = CSV_IndexOf(t_THIS_WinnerProposition);
			string t_THIS_ItemsProposed = FindAliasName("ItemsProposed");
			int t_THIS_Index_ItemsProposed = CSV_IndexOf(t_THIS_ItemsProposed);
			string t_THIS_ItemsAsked = FindAliasName("ItemsAsked");
			int t_THIS_Index_ItemsAsked = CSV_IndexOf(t_THIS_ItemsAsked);
			string sScript = "" +
				"// start Addon \n" +
				// get the actual state
				"$tServerStatut = " + ((int)NWDTradeStatus.None).ToString() + ";\n" +
				"$tServerHash = '';\n" +
				"$tQueryStatus = 'SELECT `" + t_THIS_TradeStatus + "`, `" + t_THIS_TradeHash + "` FROM `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` " +
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
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.NoDeal).ToString() +
                " || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancelled).ToString() +
				" || $sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Expired).ToString() + ")\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
				"return;\n" +
				"}\n" +
				// change the statut from CSV TO ACTIVE 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Submit).ToString() + " && " +
				"$tServerStatut == " + ((int)NWDTradeStatus.None).ToString() + ")\n" +
				"{\n" +
				"$sReplaces[" + t_THIS_Index_TradeHash + "] = $TIME_SYNC;\n" +
				"$sReplaces[" + t_THIS_Index_TradeStatus + "]=" + ((int)NWDTradeStatus.Waiting).ToString() + ";\n" +
				"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
				"$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
				"}\n" +
				// change the statut from CSV TO NONE 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.None).ToString() + " && (" +
				"$tServerStatut == " + ((int)NWDTradeStatus.Accepted).ToString() +
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Cancelled).ToString() +  // FOR DEBUG!!!!
				//" || $tServerStatut == " + ((int)NWDTradeStatus.Deal).ToString() + // FOR DEBUG!!!!
				" || $tServerStatut == " + ((int)NWDTradeStatus.Expired).ToString() +
				" || ($tServerStatut == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)" +
				"))\n" +
				"{\n" +
				"$sReplaces[" + t_THIS_Index_TradeHash + "] = $TIME_SYNC;\n" +
				"$sReplaces[" + t_THIS_Index_ItemsProposed + "]='';\n" +
				"$sReplaces[" + t_THIS_Index_ItemsAsked + "]='';\n" +
				"$sReplaces[" + t_THIS_Index_WinnerProposition + "]='';\n" +
				"$sCsvList = Integrity" + BasisHelper().ClassNamePHP + "Replaces ($sCsvList, $sReplaces);\n" +
				"}\n" +
				// change the statut from CSV TO CANCEL 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Cancel).ToString() + " && " +
				"$tServerStatut == " + ((int)NWDTradeStatus.Waiting).ToString() + ")\n" +
				"{\n" +
				"$tQueryCancelable = 'UPDATE `'.$ENV.'_" + BasisHelper().ClassNamePHP + "` SET " +
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
				"Integrity" + BasisHelper().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
				"return;\n" +
			   "}\n" +
				"else\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
				"//stop the function!\n" +
				"myLog('Break!', __FILE__, __FUNCTION__, __LINE__);\n" +
				"return;\n" +
				"}\n" +
				"}\n" +
				"}\n" +

				// change the statut from CSV TO FORCE // ADMIN ONLY 
				"else if ($sCsvList[" + t_THIS_Index_TradeStatus + "] == " + ((int)NWDTradeStatus.Force).ToString() + " && $sAdmin == true)\n" +
					"{\n" +
					"//EXECEPTION FOR ADMIN\n" +
					"}\n" +

				// OTHER
				"else\n" +
				"{\n" +
				//"Integrity" + Datas().ClassNamePHP + "Reevalue ($tReference);\n" +
				"GetDatas" + BasisHelper().ClassNamePHP + "ByReference ($tReference);\n" +
				"return;\n" +
				"}\n" +
				"// finish Addon \n";

			return sScript;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string AddonPhpPostCalculate(NWDAppEnvironment sAppEnvironment)
		{
			return "// write your php script here to update afetr sync on server\n";
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string AddonPhpSpecialCalculate(NWDAppEnvironment sAppEnvironment)
		{
			return "// write your php script here to special operation, example : \n$REP['" + BasisHelper().ClassName + " Special'] ='success!!!';\n";
		}
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================