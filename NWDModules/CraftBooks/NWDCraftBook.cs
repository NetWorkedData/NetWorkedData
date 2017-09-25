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
	[NWDClassTrigrammeAttribute ("CBK")]
	[NWDClassDescriptionAttribute ("Craft Book Recipes descriptions Class")]
	[NWDClassMenuNameAttribute ("Craft Book")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDCraftBook :NWDBasis <NWDCraftBook>
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


		[NWDGroupStartAttribute("Description",true, true, true)] // ok
		public NWDReferenceType<NWDItem> ItemToDescribe { get; set; }

		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]


		[NWDGroupStartAttribute("Recipe attribut",true, true, true)] // ok
		public bool OrderIsImportant { get; set; }
		public NWDReferenceType<NWDRecipientGroup> RecipientGroup { get; set; }
//		public NWDReferencesQuantityType<NWDItemGroup> ItemGroupIngredient { get; set; }
		public NWDReferencesArrayType<NWDItemGroup> ItemGroupIngredient { get; set; }
		public NWDReferencesQuantityType<NWDItem> ItemResult { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute("FX (Special Effects)",true, true, true)]
		public NWDPrefabType SuccessParticles { get; set; }
		public NWDPrefabType SuccessSound { get; set; }
		public NWDPrefabType FailParticles { get; set; }
		public NWDPrefabType FailSound { get; set; }

		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute("Development addons",true, true, true)]
		[NWDNotEditableAttribute]
		public string RecipeHash { get; set;}
		//[NWDGroupEndAttribute]

		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDCraftBook()
		{
			//Init your instance here
			OrderIsImportant = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
        public static NWDCraftBook GetCraftBookByItem(NWDItem item)
        {
            NWDCraftBook rReturn = null;

            NWDCraftBook[] tRecipes = GetAllObjects();
            foreach(NWDCraftBook recipe in tRecipes)
            {
                if (recipe.ItemToDescribe.ContainsReference(item.Reference))
                {
                    rReturn = recipe;
                    break;
                }
            }

            return rReturn;
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public void GetItemsRequired()
        {
            //ItemsOne
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


			#if UNITY_EDITOR
			//TODO recalculate all sign possibilities
			// I need test all possibilities .. I use an Hack : if ordered == false I sort by Name before

			if (RecipientGroup == null)
			{
				RecipientGroup= new NWDReferenceType<NWDRecipientGroup>();
			}
			if (ItemGroupIngredient == null)
			{
				ItemGroupIngredient= new NWDReferencesArrayType<NWDItemGroup>();
			}

			string tAssembly = "";
			if (OrderIsImportant == true) {
				tAssembly = OrderIsImportant.ToString () +
					RecipientGroup.ToString () +
					ItemGroupIngredient.ToString ();
			} else {
				tAssembly = OrderIsImportant.ToString () +
					RecipientGroup.ToString () +
					ItemGroupIngredient.ToStringSorted ();
			}
			RecipeHash = BTBSecurityTools.GenerateSha (tAssembly, BTBSecurityShaTypeEnum.Sha1);

			NWDDataManager.SharedInstance.RepaintWindowsInManager(this.GetType());
			NWDDataInspector.ActiveRepaint();
			#endif
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

		public static NWDCraftBook GetCraftBookFor(NWDReferencesListType<NWDRecipientGroup> sRecipientGroup, NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient) {
			NWDCraftBook tReturn = null;

			bool tOrdered = true;
			string tAssemblyA = tOrdered.ToString () + sRecipientGroup.ToString () + sItemGroupIngredient.ToString ();
			tOrdered = false;
			string tAssemblyB = tOrdered.ToString () + sRecipientGroup.ToString () + sItemGroupIngredient.ToStringSorted ();

			string tRecipeHashA = BTBSecurityTools.GenerateSha (tAssemblyA, BTBSecurityShaTypeEnum.Sha1);
			string tRecipeHashB = BTBSecurityTools.GenerateSha (tAssemblyB, BTBSecurityShaTypeEnum.Sha1);
			Debug.Log ("research tRecipeHashA " + tRecipeHashA);
			Debug.Log ("research tRecipeHashB " + tRecipeHashB);
			foreach (NWDCraftBook tCraft in NWDCraftBook.GetAllObjects()) {
				if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB) {
					tReturn = tCraft;
					break;
				}
			}

			return tReturn;
		}

	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================