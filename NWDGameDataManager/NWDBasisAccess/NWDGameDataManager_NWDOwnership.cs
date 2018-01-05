using UnityEngine;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDGameDataManager : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		// OWNERSHIP AND ITEM FOR PLAYER
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership OwnershipForItem (NWDItem sItem)
		{
			NWDOwnership rOwnershipToUse = null;
			foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects()) {
				if (tOwnership.Item.GetObject () == sItem) {
					rOwnershipToUse = tOwnership;
					break;
				}
			}
			if (rOwnershipToUse == null) {
				rOwnershipToUse = NWDOwnership.NewObject ();
				rOwnershipToUse.Item.SetObject (sItem);
				rOwnershipToUse.Quantity = 0;
				rOwnershipToUse.SaveModifications ();
			}
			return rOwnershipToUse;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool OwnershipForItemExists (NWDItem sItem)
		{
			NWDOwnership rOwnershipToUse = null;
			foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects()) {
				if (tOwnership.Item.GetObject () == sItem) {
					rOwnershipToUse = tOwnership;
					break;
				}
			}
			return rOwnershipToUse != null;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership AddItemToOwnership (NWDItem sItem, int sQuantity)
		{
			NWDOwnership rOwnershipToUse = OwnershipForItem (sItem);
#if UNITY_EDITOR
            rOwnershipToUse.InternalKey = NWDPreferences.GetString("NickNameKey", "no nickname");
            rOwnershipToUse.InternalDescription = sItem.Name.GetLocalString();
#endif
            rOwnershipToUse.Quantity += sQuantity;
			rOwnershipToUse.SaveModifications ();
            return rOwnershipToUse;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership RemoveItemToOwnership (NWDItem sItem, int sQuantity)
		{
			NWDOwnership rOwnershipToUse = OwnershipForItem (sItem);
			rOwnershipToUse.Quantity -= sQuantity;
			rOwnershipToUse.SaveModifications ();
			return rOwnershipToUse;
		}
	}
}
//=====================================================================================================================
