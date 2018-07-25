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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserOwnership : NWDBasis<NWDUserOwnership>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDNotEditable]
        public int GameSaveTag
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGameSave : NWDBasis<NWDGameSave>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership NewOwnershipForItem(NWDItem sItem)
        {
            NWDUserOwnership rResult = NWDUserOwnership.NewObject();
            rResult.GameSaveTag = GameSaveTag;
            rResult.Item.SetReference(sItem.Reference);
            rResult.Tag = NWDBasisTag.TagUserCreated;
            rResult.SaveModifications();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OwnershipTrash()
        {
            foreach (NWDUserOwnership tObject in OwnershipList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDUserOwnership> OwnershipList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDUserOwnership> rResult = new List<NWDUserOwnership>();
            foreach (NWDUserOwnership tObject in NWDUserOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership OwnershipForItem(NWDItem sItem, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            NWDUserOwnership rResult = null;
            foreach (NWDUserOwnership tObject in NWDUserOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.Item.GetReference() == sItem.Reference && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null)
            {
                rResult = NWDUserOwnership.NewObject();
                rResult.Item.SetReference(sItem.Reference);
                rResult.Tag = NWDBasisTag.TagUserCreated;
                rResult.Quantity = 0;
                rResult.SaveModifications();
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDUserOwnership> OwnershipListForItem(NWDItem sItem, bool sCreateIfNull = false, int sDefaultQuantity = 0)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDUserOwnership> rResult = new List<NWDUserOwnership>();
            foreach (NWDUserOwnership tObject in NWDUserOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.Item.GetReference() == sItem.Reference && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            if (rResult.Count == 0)
            {
                NWDUserOwnership tAddResult = NWDUserOwnership.NewObject();
                tAddResult.Item.SetReference(sItem.Reference);
                tAddResult.Tag = NWDBasisTag.TagUserCreated;
                tAddResult.Quantity = sDefaultQuantity;
                tAddResult.SaveModifications();
                rResult.Add(tAddResult);
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDUserOwnership AddItemToOwnership(NWDItem sItem, int sQuantity, bool sIsIncrement = true)
        //{
        //    NWDUserOwnership rOwnership = OwnershipForItem(sItem);
        //    if (sIsIncrement)
        //    {
        //        rOwnership.Quantity += sQuantity;
        //    }
        //    else
        //    {
        //        rOwnership.Quantity = sQuantity;
        //    }
        //    rOwnership.SaveModifications();
        //    return rOwnership;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================