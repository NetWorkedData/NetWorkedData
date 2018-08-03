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
    public class NWDShopConnection : NWDConnection <NWDShop> {}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum BuyPackResult : int { 
        None, 
        Enable, 
        Disable, 
        NotFound, 
        NotEnoughCurrency, 
        NotEnoughPackToBuy, 
        EnoughPackToBuy, 
        CanBuy, 
        Failed 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SHP")]
	[NWDClassDescriptionAttribute ("Shop descriptions Class")]
	[NWDClassMenuNameAttribute ("Shop")]
	public partial class NWDShop :NWDBasis <NWDShop>
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Properties
		//-------------------------------------------------------------------------------------------------------------
		[NWDHeaderAttribute("Description Item")]
        public NWDReferenceType<NWDItem> DescriptionItem { get; set; }
        [NWDGroupSeparatorAttribute]
		[NWDGroupStartAttribute("Racks",true, true, true)]
		public NWDReferencesListType<NWDRack> DailyRack { get; set; }
		public NWDReferencesListType<NWDRack> WeeklyRack { get; set; }
		public NWDReferencesListType<NWDRack> MonthlyRack { get; set; }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Special Racks", true, true, true)]
        public NWDDateTimeType SpecialDateStart { get; set; }
        public NWDDateTimeType SpecialDateEnd { get; set; }
        public NWDReferencesListType<NWDRack> SpecialRack { get; set; }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds { get; set; }
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public delegate void BuyPackBlock(BuyPackResult result, NWDTransaction transaction);
        public BuyPackBlock BuyPackBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
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
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
		//-------------------------------------------------------------------------------------------------------------
		public static void MyClassMethod ()
		{
			// do something with this class
		}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Buy an InApp Pack :
        /// - Add items to Ownership
        /// - Add a new transaction to Account
        /// </summary>
        /// <param name="sRack">NWDRack from where we buy the NWDPack.</param>
        /// <param name="sPack">NWDPack the pack we just buy.</param>
        public NWDTransaction BuyInAppPack(NWDRack sRack, NWDPack sPack)
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
            return NWDTransaction.AddTransactionToAccount(tItemDescribe, this, sRack, sPack);
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
        public void BuyPack(NWDRack sRack, NWDPack sPack, NWDTransaction.TransactionType sType)
        {
            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDUserOwnership),
                typeof(NWDItem),
                typeof(NWDItemPack),
                typeof(NWDPack),
                typeof(NWDTransaction)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                // Define a new NWDTransaction
                NWDTransaction bTransaction = null;

                // Check if Pack is enable
                BuyPackResult bResult = PackEnable(sPack.InternalKey);

                // Pack is enable
                if (bResult == BuyPackResult.Enable)
                {
                    // Check if there is enough pack to buy
                    bResult = EnoughPackToBuy(this, sRack, sPack, sType);

                    // User can buy if there is enough Pack to buy
                    if (bResult == BuyPackResult.EnoughPackToBuy)
                    {
                        // Check if user have enough currency
                        Dictionary<NWDItem, int> tCost = sPack.ItemsToPay.GetObjectAndQuantity();
                        bResult = UserCanBuy(tCost);

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
                                    int tItemQte = p.Value;

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
                            bTransaction = NWDTransaction.AddTransactionToAccount(tItemDescribe, this, sRack, sPack);
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
        private BuyPackResult PackEnable(string sPackKey)
        {
            BuyPackResult rPackEnable = BuyPackResult.NotFound;

            NWDPack tPack = NWDPack.NEW_FirstDatasByInternalKey(sPackKey);
            if (tPack != null)
            {
                if (tPack.IsEnable())
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
            BuyPackResult rUserCanBuy = BuyPackResult.None;

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
        private BuyPackResult EnoughPackToBuy(NWDShop sShop, NWDRack sRack, NWDPack sPack, NWDTransaction.TransactionType sType)
        {
            BuyPackResult rEnoughPackToBuy = BuyPackResult.None;

            // Create Transactions array
            List<NWDTransaction> tTransactionList = new List<NWDTransaction>();

            // Create Racks array
            List<NWDRack> tRackList = new List<NWDRack>();

            // Init all transactions done by the user for selected shop and type
            tRackList.Add(sRack);
            tTransactionList = NWDTransaction.GetTransactionsByShopAndType(sShop, tRackList, sType);

            // Search for the right Pack in Rack (for quantities)
            Dictionary<NWDPack, int> tPacks = sRack.PackQuantity.GetObjectAndQuantity();
            foreach (KeyValuePair<NWDPack, int> pair in tPacks)
            {
                NWDPack tPack = pair.Key;
                int tPackQte = pair.Value;

                if (tPack.Equals(sPack))
                {
                    // Verify if there is enough number of pack to buy
                    foreach (NWDTransaction transaction in tTransactionList)
                    {
                        if (transaction.RackReference.ContainsObject(sRack) &&
                            transaction.PackReference.ContainsObject(sPack))
                        {
                            tPackQte--;
                        }
                    }

                    if (tPackQte > 0)
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

            return rEnoughPackToBuy;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe ()
		{
			// do something when object will be inserted
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
		{
			// Height calculate for the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
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