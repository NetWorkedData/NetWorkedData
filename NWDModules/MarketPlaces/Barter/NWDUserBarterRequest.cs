//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BasicToolBox;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBRR")]
    [NWDClassDescriptionAttribute("User Barter Request descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Request")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterRequest : NWDBasis<NWDUserBarterRequest>
    {
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
        [NWDNotEditable]
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
        [NWDNotEditable]
        [NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDIntSlider(NWDBarterPlace.K_BARTER_PROPOSITIONS_PER_REQUEST_MIN, NWDBarterPlace.K_BARTER_PROPOSITIONS_PER_REQUEST_MAX)]
        [NWDAlias("MaxPropositions")]

        public int MaxPropositions
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("PropositionsCounter")]
        public int PropositionsCounter
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("Propositions")]
        public NWDReferencesListType<NWDUserBarterProposition> Propositions
        {
            get; set;
        }
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
        //-------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserBarterRequest CreateBarterRequestWith(NWDBarterPlace sBarterPlace, Dictionary<string, int> sProposed)
        {
            // Get Request Life time
            int tLifetime = sBarterPlace.RequestLifeTime;
            
            // Create a new Request
            NWDUserBarterRequest tRequest = NewData();
            #if UNITY_EDITOR
            tRequest.InternalKey = NWDAccountNickname.GetNickname() + " - " + sBarterPlace.InternalKey;
            #endif
            tRequest.Tag = NWDBasisTag.TagUserCreated;
            tRequest.BarterPlace.SetObject(sBarterPlace);
            tRequest.ItemsProposed.SetReferenceAndQuantity(sProposed);
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
                // Keep TradeStatus before Clean()
                NWDTradeStatus tBarterStatus = BarterStatus;
                
                // Do action with Items & Sync
                AddOrRemoveItems();
                
                if (barterRequestBlockDelegate != null)
                {
                    barterRequestBlockDelegate(true, tBarterStatus, null);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (barterRequestBlockDelegate != null)
                {
                    NWDOperationResult tInfos = bInfos as NWDOperationResult;
                    barterRequestBlockDelegate(false, NWDTradeStatus.None, tInfos);
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
            LimitDayTime = null;
            BarterStatus = NWDTradeStatus.None;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================