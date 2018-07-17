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
    public partial class NWDOwnership : NWDBasis<NWDOwnership>
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
        public NWDOwnership NewOwnershipForItem(NWDItem sItem)
        {
            NWDOwnership rResult = NWDOwnership.NewObject();
            rResult.GameSaveTag = GameSaveTag;
            rResult.Item.SetReference(sItem.Reference);
            rResult.Tag = NWDBasisTag.TagUserCreated;
            rResult.SaveModifications();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OwnershipTrash()
        {
            foreach (NWDOwnership tObject in OwnershipList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDOwnership> OwnershipList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDOwnership> rResult = new List<NWDOwnership>();
            foreach (NWDOwnership tObject in NWDOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOwnership OwnershipForItem(NWDItem sItem, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            NWDOwnership rResult = null;
            foreach (NWDOwnership tObject in NWDOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.Item.GetReference() == sItem.Reference && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null)
            {
                rResult = NWDOwnership.NewObject();
                rResult.Item.SetReference(sItem.Reference);
                rResult.Tag = NWDBasisTag.TagUserCreated;
                rResult.Quantity = 0;
                rResult.SaveModifications();
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDOwnership> OwnershipListForItem(NWDItem sItem, bool sCreateIfNull = false, int sDefaultQuantity = 0)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDOwnership> rResult = new List<NWDOwnership>();
            foreach (NWDOwnership tObject in NWDOwnership.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.Item.GetReference() == sItem.Reference && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            if (rResult.Count == 0)
            {
                NWDOwnership tAddResult = NWDOwnership.NewObject();
                tAddResult.Item.SetReference(sItem.Reference);
                tAddResult.Tag = NWDBasisTag.TagUserCreated;
                tAddResult.Quantity = sDefaultQuantity;
                tAddResult.SaveModifications();
                rResult.Add(tAddResult);
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================