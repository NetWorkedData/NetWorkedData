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
    public partial class NWDUserPreference : NWDBasis<NWDUserPreference>
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
    public partial class NWDAccountParty : NWDBasis<NWDAccountParty>
    {
        //-------------------------------------------------------------------------------------------------------------
        //public NWDPreferences UserPreferencesNewObject()
        //{
        //    NWDPreferences rResult = NWDPreferences.NewObject();
        //    rResult.PartyTag = PartyTag;
        //    rResult.SaveModifications();
        //    return rResult;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void UserPreferencesTrash()
        {
            foreach (NWDUserPreference tObject in UserPreferencesList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDUserPreference> UserPreferencesList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDUserPreference> rResult = new List<NWDUserPreference>();
            foreach (NWDUserPreference tObject in NWDUserPreference.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserPreference UserPreferencesByInternalKey(string sInternalKey, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            NWDUserPreference rResult = null;
            foreach (NWDUserPreference tObject in NWDUserPreference.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag && tObject.InternalKey == sInternalKey)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null && sCreateIfNull == true)
            {
                rResult = NWDUserPreference.NewObject();
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