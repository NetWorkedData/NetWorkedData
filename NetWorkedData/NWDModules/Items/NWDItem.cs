//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("ITM")]
	[NWDClassDescriptionAttribute ("Item descriptions Class")]
	[NWDClassMenuNameAttribute ("Item")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItem :NWDBasis <NWDItem>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[NWDHeaderAttribute("Informations")]
		public NWDReferencesListType<NWDItemGroup> ItemGroupReferencesList { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDFloatSliderAttribute(0.0F,1.0F)]
		public float Rarity { get; set; }
		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }
		public bool Usable { get; set; }
		[NWDHeaderAttribute("Color")]
		public NWDColorType ColorNormal { get; set; }
		public NWDColorType ColorSelected { get; set; }
		[NWDHeaderAttribute("Prefab")]
		public NWDPrefabType NormalPrefab { get; set; }
		public NWDPrefabType SelectedPrefab { get; set; }
		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Is Character in the game",true,false,true)]
		public bool IsCharactersCreation { get; set; }
		public float LevelMin { get; set; }
		public float LevelMax { get; set; }
		public float LifeLevelMin { get; set; }
		public float LifeLevelMax { get; set; }
		public float AttackLevelMin { get; set; }
		public float AttackLevelMax { get; set; }
		public float DefenseLevelMin { get; set; }
		public float DefenseLevelMax { get; set; }
		[NWDGroupEndAttribute]

		[NWDHeaderAttribute("Is Battle active in the game")]
		public bool HasBattleProperties { get; set; }
		public NWDReferencesListType<NWDBattleProperty> BattleProperties { get; set; }

		[NWDHeaderAttribute("Is cook recipe script in the game")]
	
		public NWDReferenceType<NWDCookRecipe> CookRecipeReference { get; set; }

		[NWDHeaderAttribute("For developer a JSON data")]

		[NWDGroupStartAttribute("JSON for this object",true, false, true)]
		public string JSON { get; set; }
		[NWDGroupEndAttribute]

		[NWDHeaderAttribute("Use object delayed")]
		[NWDGroupStartAttribute("Delay of usage",true, false, true)]
		public int DelayToUse { get; set; }
		public int DelayToReUse { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDHeaderAttribute("Automatique values")]
//		[NWDGroupStartAttribute("Value A settings",true, true, true)]

		//[NWDAccountEntitledAttribute("BLABLA BALA bALLAA")]
//		public NWDLocalizableStringType A_NameKey { get; set; }
//
//		public float A_Min { get; set; }
//
//		public float A_Max { get; set; }
//
//		public float A_Value { get; set; }
//
//		public int A_Timer { get; set; }
//
//		public float A_Increment { get; set; }
//
//		public int A_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		public NWDLocalizableStringType B_NameKey { get; set; }
//
//		public float B_Min { get; set; }
//
//		public float B_Max { get; set; }
//
//		public float B_Value { get; set; }
//
//		public int B_Timer { get; set; }
//
//		public float B_Increment { get; set; }
//
//		public int B_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value C settings",true, true, true)]
//
//		public NWDLocalizableStringType C_NameKey { get; set; }
//
//		public float C_Min { get; set; }
//
//		public float C_Max { get; set; }
//
//		public float C_Value { get; set; }
//
//		public int C_Timer { get; set; }
//
//		public float C_Increment { get; set; }
//
//		public int C_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value D settings",true, true, true)]
//
//		public NWDLocalizableStringType D_NameKey { get; set; }
//
//		public float D_Min { get; set; }
//
//		public float D_Max { get; set; }
//
//		public float D_Value { get; set; }
//
//		public int D_Timer { get; set; }
//
//		public float D_Increment { get; set; }
//
//		public int D_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value E settings",true, true, true)]
//
//		public NWDLocalizableStringType E_NameKey { get; set; }
//
//		public float E_Min { get; set; }
//
//		public float E_Max { get; set; }
//
//		public float E_Value { get; set; }
//
//		public int E_Timer { get; set; }
//
//		public float E_Increment { get; set; }
//
//		public int E_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value F settings",true, true, true)]
//
//		public NWDLocalizableStringType F_NameKey { get; set; }
//
//		public float F_Min { get; set; }
//
//		public float F_Max { get; set; }
//
//		public float F_Value { get; set; }
//
//		public int F_Timer { get; set; }
//
//		public float F_Increment { get; set; }
//
//		public int F_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value G settings",true, true, true)]
//
//		public NWDLocalizableStringType G_NameKey { get; set; }
//
//		public float G_Min { get; set; }
//
//		public float G_Max { get; set; }
//
//		public float G_Value { get; set; }
//
//		public int G_Timer { get; set; }
//
//		public float G_Increment { get; set; }
//
//		public int G_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value H settings",true, true, true)]
//
//		public NWDLocalizableStringType H_NameKey { get; set; }
//
//		public float H_Min { get; set; }
//
//		public float H_Max { get; set; }
//
//		public float H_Value { get; set; }
//
//		public int H_Timer { get; set; }
//
//		public float H_Increment { get; set; }
//
//		public int H_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value I settings",true, true, true)]
//
//		public NWDLocalizableStringType I_NameKey { get; set; }
//
//		public float I_Min { get; set; }
//
//		public float I_Max { get; set; }
//
//		public float I_Value { get; set; }
//
//		public int I_Timer { get; set; }
//
//		public float I_Increment { get; set; }
//
//		public int I_LastSynchronization { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Value J settings",true, true, true)]
//
//		public NWDLocalizableStringType J_NameKey { get; set; }
//
//		public float J_Min { get; set; }
//
//		public float J_Max { get; set; }
//
//		public float J_Value { get; set; }
//
//		public int J_Timer { get; set; }
//
//		public float J_Increment { get; set; }
//
//		public int J_LastSynchronization { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		public NWDItem()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
			Usable = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================