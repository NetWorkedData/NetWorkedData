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
    public partial class AIRUserNotoriety : NWDBasis<AIRUserNotoriety>
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
        public AIRUserNotoriety UserNotorietyNewObject()
        {
            AIRUserNotoriety rResult = AIRUserNotoriety.NewObject();
            rResult.PartyTag = PartyTag;
            rResult.SaveModifications();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UserNotorietyTrash()
        {
            foreach (AIRUserNotoriety tObject in UserNotorietyList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<AIRUserNotoriety> UserNotorietyList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<AIRUserNotoriety> rResult = new List<AIRUserNotoriety>();
            foreach (AIRUserNotoriety tObject in AIRUserNotoriety.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public AIRUserNotoriety UserNotorietyByInternalKey(string sInternalKey, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            AIRUserNotoriety rResult = null;
            foreach (AIRUserNotoriety tObject in AIRUserNotoriety.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag && tObject.InternalKey == sInternalKey)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null && sCreateIfNull == true)
            {
                rResult = AIRUserNotoriety.NewObject();
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