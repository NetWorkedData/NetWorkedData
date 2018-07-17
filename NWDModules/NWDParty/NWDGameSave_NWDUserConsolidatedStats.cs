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
    public partial class NWDUserConsolidatedStats : NWDBasis<NWDUserConsolidatedStats>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDNotEditable]
        public int PartyTag
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGameSave : NWDBasis<NWDGameSave>
    {
        //-------------------------------------------------------------------------------------------------------------
        //public NWDUserConsolidatedStats UserConsolidatedStatsNewObject()
        //{
        //    NWDUserConsolidatedStats rResult = NWDUserConsolidatedStats.NewObject();
        //    rResult.GameSaveTag = GameSaveTag;
        //    rResult.SaveModifications();
        //    return rResult;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void UserConsolidatedStatsTrash()
        {
            foreach (NWDUserConsolidatedStats tObject in UserConsolidatedStatsList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDUserConsolidatedStats> UserConsolidatedStatsList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDUserConsolidatedStats> rResult = new List<NWDUserConsolidatedStats>();
            foreach (NWDUserConsolidatedStats tObject in NWDUserConsolidatedStats.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserConsolidatedStats UserConsolidatedStatsByInternalKey(string sInternalKey, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            NWDUserConsolidatedStats rResult = null;
            foreach (NWDUserConsolidatedStats tObject in NWDUserConsolidatedStats.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == GameSaveTag && tObject.InternalKey == sInternalKey)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null && sCreateIfNull == true)
            {
                rResult = NWDUserConsolidatedStats.NewObject();
                rResult.InternalKey = sInternalKey;
                rResult.Tag = NWDBasisTag.TagUserCreated;
                rResult.SaveModifications();
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================