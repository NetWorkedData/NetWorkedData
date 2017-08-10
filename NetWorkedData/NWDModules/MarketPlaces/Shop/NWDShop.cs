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
    //-----------------------------------------------------------------------------------------------------------------
    public enum BuyPackResult { None, Enable, Disable, NotFound, NotEnoughCurrency, NotEnoughItem, CanBuy, Failed }
    //-----------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SHP")]
	[NWDClassDescriptionAttribute ("Shop descriptions Class")]
	[NWDClassMenuNameAttribute ("Shop")]
	//-----------------------------------------------------------------------------------------------------------------
	public partial class NWDShop :NWDBasis <NWDShop>
	{
		//-------------------------------------------------------------------------------------------------------------
		//#warning YOU MUST FOLLOW THIS INSTRUCTIONS
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
		// YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
		//-------------------------------------------------------------------------------------------------------------
		#region Properties
		//-------------------------------------------------------------------------------------------------------------
		// Your properties
		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Opening",true, true, true)]
		public int OpenDateTime { get; set; }
		public int CloseDateTime { get; set; }
		public int Calendar { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Racks",true, true, true)]
		public int RequestLifeTime { get; set; }
		public NWDReferencesListType<NWDRack> DailyRack { get; set; }
		public NWDReferencesListType<NWDRack> WeeklyRack { get; set; }
		public NWDReferencesListType<NWDRack> MonthlyRack { get; set; }
        //[NWDGroupEndAttribute]
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
			//Init your instance here
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
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
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
        //-------------------------------------------------------------------------------------------------------------
        public void BuyPack(NWDPack sPack)
        {
            // Sync with the server
            List<Type> tList = new List<Type>();
            tList.Add(typeof(NWDOwnership));
            tList.Add(typeof(NWDItem));
            tList.Add(typeof(NWDItemPack));
            tList.Add(typeof(NWDPack));

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                // Define a new NWDTransaction
                NWDTransaction bTransaction = null;

                // Check if Pack is enable
                BuyPackResult bResult = PackEnable(sPack.InternalKey);

                // Pack is enable
                if (bResult == BuyPackResult.Enable)
                {
                    // Check if user have enough currency
                    Dictionary<NWDItem, int> tCost = sPack.ItemsToPay.GetObjectAndQuantity();
                    bResult = UserCanBuy(tCost);

                    // User can by the Pack
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
                                NWDOwnership.AddItemToOwnership(tNWDItem, tItemQte);
                            }
                        }

                        // Find all currency to remove from ownership
                        foreach (KeyValuePair<NWDItem, int> pair in tCost)
                        {
                            // Get Item Cost data
                            NWDItem tNWDItem = pair.Key;
                            int tItemQte = pair.Value;

                            // Remove currency from Ownership
                            NWDOwnership.RemoveItemToOwnership(tNWDItem, tItemQte);
                        }

                        // Set a NWDTransaction
                        bTransaction = NWDTransaction.NewObject();
                        bTransaction.PackReference.SetReference(sPack.Reference);
                        bTransaction.InternalKey = sPack.Name.GetBaseString();
                        bTransaction.InternalDescription = NWDPreferences.GetString("NickNameKey", "no nickname");
                        bTransaction.SaveModifications();
                    }
                }

                if (BuyPackBlockDelegate != null)
                {
                    BuyPackBlockDelegate(bResult, bTransaction);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = (NWDOperationResult)bInfos;

                if (BuyPackBlockDelegate != null)
                {
                    BuyPackBlockDelegate(BuyPackResult.Failed, null);
                }
            };
            NWDDataManager.SharedInstance.AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------
        private BuyPackResult PackEnable(string sPackKey)
        {
            BuyPackResult rPackEnable = BuyPackResult.NotFound;

            NWDPack tPack = NWDPack.GetObjectByInternalKey(sPackKey);
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

                if (NWDOwnership.OwnershipForItemExists(tNWDItem))
                {
                    if (NWDOwnership.OwnershipForItem(tNWDItem).Quantity < tItemQte)
                    {
                        // User don't have enough item
                        rUserCanBuy = BuyPackResult.NotEnoughCurrency;
                        break;
                    }
                }
                else
                {
                    // User don't have the selected item
                    rUserCanBuy = BuyPackResult.NotEnoughItem;
                    break;
                }
            }

            return rUserCanBuy;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region override of NetWorkedData addons methods
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

    //-----------------------------------------------------------------------------------------------------------------
    #region Connexion NWDShop with Unity MonoBehavior
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWDShop connexion.
    /// In your MonoBehaviour Script connect object with :
    /// <code>
    ///	[NWDConnexionAttribut(true,true, true, true)]
    /// public NWDShopConnexion MyNWDShopObject;
    /// </code>
    /// </summary>
    //-------------------------------------------------------------------------------------------------------------
    // CONNEXION STRUCTURE METHODS
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
	public class NWDShopConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDShop GetObject ()
		{
			return NWDShop.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDShop sObject)
		{
			Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDShop NewObject ()
		{
			NWDShop tObject = NWDShop.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	// CUSTOM PROPERTY DRAWER METHODS
	//-------------------------------------------------------------------------------------------------------------
	#if UNITY_EDITOR
	//-------------------------------------------------------------------------------------------------------------
	[CustomPropertyDrawer (typeof(NWDShopConnexion))]
	public class NWDShopConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Debug.Log ("GetPropertyHeight");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			return NWDShop.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Debug.Log ("OnGUI");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			NWDShop.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
    //-------------------------------------------------------------------------------------------------------------
    #endif
    //-------------------------------------------------------------------------------------------------------------
    #endregion
    //-----------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================