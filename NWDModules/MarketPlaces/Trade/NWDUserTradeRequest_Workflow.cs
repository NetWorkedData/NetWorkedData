// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDUserTradeRequest : NWDBasis<NWDUserTradeRequest>
	{
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
			rRequest.TradePlace.SetData(sTradePlace);
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
			foreach (NWDUserTradeRequest k in GetDatas())
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
						Dictionary<NWDItem, int> tProposed = ItemsProposed.FindDataAndQuantity();
						foreach (KeyValuePair<NWDItem, int> pair in tProposed)
						{
							NWDUserOwnership.RemoveItemToOwnership(pair.Key, pair.Value);
						}
					}
					break;
				case NWDTradeStatus.Expired:
					{
						// Add NWDItem to NWDUserOwnership
						Dictionary<NWDItem, int> tProposed = ItemsProposed.FindDataAndQuantity();
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
						Dictionary<NWDItem, int> tProposed = ItemsAsked.FindDataAndQuantity();
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
			foreach (KeyValuePair<NWDItem, int> pair in ItemsAsked.FindDataAndQuantity())
			{
				// Get Item Cost data
				NWDItem tNWDItem = pair.Key;
				int tItemQte = pair.Value;

				rCanBuy = true;

				if (NWDUserOwnership.OwnershipForItemExists(tNWDItem))
				{
					if (NWDUserOwnership.FindFisrtByItem(tNWDItem).Quantity < tItemQte)
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
            foreach (NWDUserTradeRequest k in GetDatas())
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================