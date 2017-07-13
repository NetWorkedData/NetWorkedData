using System;

using UnityEngine;

using SQLite4Unity3d;
//=====================================================================================================================
using System.Collections.Generic;

namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TAT")]
	[NWDClassDescriptionAttribute ("Tips And Tricks descriptions Class")]
	[NWDClassMenuNameAttribute ("Tips And Tricks")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTipsAndTricks :NWDBasis <NWDTipsAndTricks>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[NWDGroupStartAttribute("Classification", false , true ,true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Tips and Tricks", false , true ,true)]

		public NWDLocalizableStringType Title { get; set; }

		public NWDLocalizableStringType SubTitle { get; set; }

		public NWDLocalizableTextType Message { get; set; }

		public NWDSpriteType Icon { get; set; }

		[NWDHeaderAttribute("Items required to be visible")]
		public NWDReferencesQuantityType<NWDItem> ItemsRequired{ get; set; }

		[NWDHeaderAttribute("Weighting")]
		[NWDIntSliderAttribute(1,9)]
		public int Weighting { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTipsAndTricks()
		{
			Weighting = 1;
			Title = new NWDLocalizableStringType ();
			SubTitle = new NWDLocalizableStringType ();
			Message = new NWDLocalizableTextType ();
		}
		//-------------------------------------------------------------------------------------------------------------
		static List<NWDTipsAndTricks> ListForRandom;
		//-------------------------------------------------------------------------------------------------------------
		public static List<NWDTipsAndTricks> PrepareListForRandom (NWDWorld sWorld=null, NWDFamily sFamily=null, NWDKeyword sKeyword=null, NWDOwnership sOwnership=null)
		{
			ListForRandom = new List<NWDTipsAndTricks> ();
			foreach (NWDTipsAndTricks tObject in NWDTipsAndTricks.ObjectsList) 
			{
				/* I list the object compatible with request
			 	* I insert in the list  each object (Frequency) times
			 	* I return the List
				*/
				for (int i = 0; i < tObject.Weighting; i++) 
				{
					ListForRandom.Add (tObject);
				}
			}
			return ListForRandom;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDTipsAndTricks SelectRandomTips (bool sAbsoluteRemove = true)
		{
			NWDTipsAndTricks rReturn = null;
			// I select the tick by random 
			int tCount = ListForRandom.Count-1;
			int tIndex  = UnityEngine.Random.Range (0, tCount);
			if (tIndex >=0 && tIndex <= tCount) {
				rReturn = ListForRandom [tIndex];
				if (sAbsoluteRemove == false) {
					ListForRandom.RemoveAt (tIndex);
				} else {
					while (ListForRandom.Contains (rReturn)) {
						ListForRandom.Remove (rReturn);
					}
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================