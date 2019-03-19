//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDShop :NWDBasis <NWDShop>
	{
		//-------------------------------------------------------------------------------------------------------------
        public NWDShop()
        {
            //Debug.Log("NWDShop Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDShop(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
           //Debug.Log("NWDShop Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Buy an InApp Pack :
        /// - Add items to Ownership
        /// - Add a new transaction to Account
        /// </summary>
        /// <param name="sRack">NWDRack from where we buy the NWDPack.</param>
        /// <param name="sPack">NWDPack the pack we just buy.</param>
        public NWDUserTransaction BuyInAppPack(NWDRack sRack, NWDPack sPack)
        {
            foreach (KeyValuePair<NWDItemPack, int> pair in sPack.ItemPackReference.GetObjectAndQuantity())
            {
                // Get Item Pack data
                NWDItemPack tItemPack = pair.Key;
                int tItemPackQte = pair.Value;

                // Find all Items from Item Pack
                Dictionary<NWDItem, int> tItems = tItemPack.Items.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> p in tItems)
                {
                    // Get Item data
                    NWDItem tNWDItem = p.Key;
                    int tItemQte = p.Value;

                    // Add Items to Ownership
                    NWDUserOwnership.AddItemToOwnership(tNWDItem, tItemQte);
                }
            }
            
            // Add a new NWDTransaction to user Account
            NWDItem tItemDescribe = sPack.DescriptionItem.GetObject();
            return NWDUserTransaction.AddTransactionToAccount(tItemDescribe, this, sRack, sPack);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Buy a Pack :
        /// - Remove currency from Ownership
        /// - Add items to Ownership
        /// - Add a new transaction to Account
        /// </summary>
        /// <param name="sShop">NWDShop from where we buy the NWDPack.</param>
        /// <param name="sRack">NWDRack from where we buy the NWDPack.</param>
        /// <param name="sPack">NWDPack the pack we just buy.</param>
        /// <param name="sType">Enum to represente the type of the transaction (Daily, Weekly, Monthly).</param>
        public void BuyPack(NWDRack sRack, NWDPack sPack, NWDTransactionType sType)
        {
            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDUserOwnership),
                typeof(NWDItem),
                typeof(NWDItemPack),
                typeof(NWDPack),
                typeof(NWDUserTransaction)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                // Define a new NWDTransaction
                NWDUserTransaction bTransaction = null;

                // Check if Pack is enable
                BuyPackResult bResult = PackEnable(sPack);

                // Pack is enable
                if (bResult == BuyPackResult.Enable)
                {
                    // Check if there is enough pack to buy
                    bResult = EnoughPackToBuy(this, sRack, sPack, sType);

                    // User can buy if there is enough Pack to buy
                    if (bResult == BuyPackResult.EnoughPackToBuy)
                    {
                        // Check if pack is free
                        Dictionary<NWDItem, int> tCost = new Dictionary<NWDItem, int>();
                        if (sPack.EnableFreePack)
                        {
                            bResult = BuyPackResult.CanBuy;
                        }
                        else
                        {
                            // Check if user have enough currency
                            tCost = sPack.ItemsToPay.GetObjectAndQuantity();
                            bResult = UserCanBuy(tCost);
                        }

                        // User can buy the Pack
                        if (bResult == BuyPackResult.CanBuy)
                        {
                            // Find all Items Pack in Pack
                            foreach (KeyValuePair<NWDItemPack, int> pair in sPack.ItemPackReference.GetObjectAndQuantity())
                            {
                                // Get Item Pack data
                                NWDItemPack tItemPack = pair.Key;
                                int tItemPackQte = pair.Value;

                                // Find all Items from Item Pack
                                Dictionary<NWDItem, int> tItems = tItemPack.Items.GetObjectAndQuantity();
                                foreach (KeyValuePair<NWDItem, int> p in tItems)
                                {
                                    // Get Item data
                                    NWDItem tNWDItem = p.Key;
                                    int tItemQte = p.Value * tItemPackQte;

                                    // Add Items to Ownership
                                    NWDUserOwnership.AddItemToOwnership(tNWDItem, tItemQte);
                                }
                            }

                            // Find all currency to remove from Ownership
                            foreach (KeyValuePair<NWDItem, int> pair in tCost)
                            {
                                // Get Item Cost data
                                NWDItem tNWDItem = pair.Key;
                                int tItemQte = pair.Value;

                                // Remove currency from Ownership
                                NWDUserOwnership.RemoveItemToOwnership(tNWDItem, tItemQte);
                            }

                            // Add a new NWDTransaction to user Account
                            NWDItem tItemDescribe = sPack.DescriptionItem.GetObject();
                            bTransaction = NWDUserTransaction.AddTransactionToAccount(tItemDescribe, this, sRack, sPack);
                        }
                    }
                }

                if (BuyPackBlockDelegate != null)
                {
                    BuyPackBlockDelegate(bResult, bTransaction);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (BuyPackBlockDelegate != null)
                {
                    BuyPackBlockDelegate(BuyPackResult.Failed, null);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        private BuyPackResult PackEnable(NWDPack sPack)
        {
            BuyPackResult rPackEnable = BuyPackResult.NotFound;

            if (sPack != null)
            {
                if (sPack.IsEnable())
                {
                    rPackEnable = BuyPackResult.Enable;
                }
                else
                {
                    rPackEnable = BuyPackResult.Disable;
                }
            }

            return rPackEnable;
        }
        //-------------------------------------------------------------------------------------------------------------
        private BuyPackResult UserCanBuy(Dictionary<NWDItem, int> sPackCost)
        {
            BuyPackResult rUserCanBuy = BuyPackResult.MissingPayCost;

            // Check Pack Cost
            foreach (KeyValuePair<NWDItem, int> pair in sPackCost)
            {
                // Get Item Cost data
                NWDItem tNWDItem = pair.Key;
                int tItemQte = pair.Value;

                rUserCanBuy = BuyPackResult.CanBuy;

                if (NWDUserOwnership.OwnershipForItemExists(tNWDItem))
                {
                    if (NWDUserOwnership.OwnershipForItem(tNWDItem).Quantity < tItemQte)
                    {
                        // User don't have enough item
                        rUserCanBuy = BuyPackResult.NotEnoughCurrency;
                        break;
                    }
                }
                else
                {
                    // User don't have the selected item
                    rUserCanBuy = BuyPackResult.NotEnoughCurrency;
                    break;
                }
            }

            return rUserCanBuy;
        }
        //-------------------------------------------------------------------------------------------------------------
        private BuyPackResult EnoughPackToBuy(NWDShop sShop, NWDRack sRack, NWDPack sPack, NWDTransactionType sType)
        {
            BuyPackResult rEnoughPackToBuy = BuyPackResult.NotFound;

            // Create Transactions array
            List<NWDUserTransaction> tTransactionList = new List<NWDUserTransaction>();

            // Create Racks array
            List<NWDRack> tRackList = new List<NWDRack>();

            // Init all transactions done by the user for selected shop and type
            tRackList.Add(sRack);
            tTransactionList = NWDUserTransaction.GetTransactionsByShopAndType(sShop, tRackList, sType);

            // Search for the right Pack in Rack (for quantities)
            Dictionary<NWDPack, int> tPacks = sRack.PackQuantity.GetObjectAndQuantity();
            foreach (KeyValuePair<NWDPack, int> pair in tPacks)
            {
                NWDPack tPack = pair.Key;
                int tPackQte = pair.Value;

                if (tPack.Equals(sPack))
                {
                    // Verify if there is enough number of pack to buy
                    foreach (NWDUserTransaction transaction in tTransactionList)
                    {
                        if (transaction.RackReference.ContainsObject(sRack) &&
                            transaction.PackReference.ContainsObject(sPack))
                        {
                            tPackQte--;
                        }
                    }

                    if (tPackQte > 0 || sPack.Quantity == 0)
                    {
                        rEnoughPackToBuy = BuyPackResult.EnoughPackToBuy;
                    }
                    else
                    {
                        rEnoughPackToBuy = BuyPackResult.NotEnoughPackToBuy;
                    }

                    break;
                }
            }

            /*}
            else
            {
                // Search for the right Pack in Rack (for missing pack)
                Dictionary<NWDPack, int> tPacks = sRack.PackQuantity.GetObjectAndQuantity();
                if (tPacks.Count > 0)
                {
                    rEnoughPackToBuy = BuyPackResult.EnoughPackToBuy;
                }
            }*/

            return rEnoughPackToBuy;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================