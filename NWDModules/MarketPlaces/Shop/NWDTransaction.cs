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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRS")]
	[NWDClassDescriptionAttribute ("Transaction descriptions Class")]
	[NWDClassMenuNameAttribute ("Transaction")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTransaction :NWDBasis <NWDTransaction>
	{
        //-----------------------------------------------------------------------------------------------------------------
        public enum TransactionType { None, Daily, Weekly, Monthly }
        //-----------------------------------------------------------------------------------------------------------------
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
        [Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
        public NWDReferenceType<NWDShop> ShopReference { get; set; }
        public NWDReferenceType<NWDRack> RackReference { get; set; }
        public NWDReferenceType<NWDPack> PackReference { get; set; }
        public string Platform { get; set; }
        public NWDReferenceType<NWDInAppPack> InAppReference { get; set; }
        public string InAppTransaction { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDTransaction()
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
        #region Static methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDTransaction> GetTransactionsByShopAndType(NWDShop sShop, List<NWDRack> sRacks, TransactionType sType)
        {
            // Create Transaction array
            List<NWDTransaction> rTransactionList = new List<NWDTransaction>();

            // Init all transactions done by the user for selected shop and type
            NWDTransaction[] tList = NWDTransaction.GetAllObjects();
            foreach (NWDTransaction transaction in tList)
            {
                // Verify we are in the right Shop
                if (transaction.ShopReference.ContainsObject(sShop))
                {
                    // Parse selected Shop Racks
                    foreach (NWDRack tRack in sRacks)
                    {
                        // Verify the Rack
                        if (transaction.RackReference.ContainsObject(tRack))
                        {
                            // Take only transaction filter by ShopType
                            bool isValidate = false;
                            switch (sType)
                            {
                                case TransactionType.Daily:
                                    double tLocalDateStart = BTBDateHelper.ConvertToUnixTimestamp(DateTime.Today);
                                    double tLocalDateEnd = BTBDateHelper.ConvertToUnixTimestamp(DateTime.Today.AddDays(1));
                                    if (transaction.DC >= tLocalDateStart && transaction.DC <= tLocalDateEnd)
                                    {
                                        isValidate = true;
                                    }
                                    break;
                                case TransactionType.Weekly:
                                    isValidate = true;
                                    break;
                                case TransactionType.Monthly:
                                    isValidate = true;
                                    break;
                            }

                            // Transaction found (shop and shop type match) and validate
                            if (isValidate)
                            {
                                rTransactionList.Add(transaction);
                            }
                            break;
                        }
                    }
                }
            }

            return rTransactionList;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }

    //-------------------------------------------------------------------------------------------------------------
    #region Connexion NWDTransaction with Unity MonoBehavior
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWDTransaction connexion.
    /// In your MonoBehaviour Script connect object with :
    /// <code>
    ///	[NWDConnexionAttribut(true,true, true, true)]
    /// public NWDTransactionConnexion MyNWDTransactionObject;
    /// </code>
    /// </summary>
    //-------------------------------------------------------------------------------------------------------------
    // CONNEXION STRUCTURE METHODS
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
	public class NWDTransactionConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDTransaction GetObject ()
		{
			return NWDTransaction.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDTransaction sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDTransaction NewObject ()
		{
			NWDTransaction tObject = NWDTransaction.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDTransactionConnexion))]
	public class NWDTransactionConnexionDrawer : PropertyDrawer
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
			return NWDTransaction.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
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
			NWDTransaction.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endif
	//-------------------------------------------------------------------------------------------------------------
	#endregion
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================