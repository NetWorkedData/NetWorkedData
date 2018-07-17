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
    public partial class AIRUserSurface : NWDBasis<AIRUserSurface>
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
        public AIRUserSurface UserSurfaceNewObject()
        {
            AIRUserSurface rResult = AIRUserSurface.NewObject();
            rResult.PartyTag = PartyTag;
            rResult.SaveModifications();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UserSurfaceTrash()
        {
            foreach (AIRUserSurface tObject in UserSurfaceList())
            {
                tObject.TrashMeLater();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<AIRUserSurface> UserSurfaceList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<AIRUserSurface> rResult = new List<AIRUserSurface>();
            foreach (AIRUserSurface tObject in AIRUserSurface.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public AIRUserSurface UserSurfaceByInternalKey(string sInternalKey, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            AIRUserSurface rResult = null;
            foreach (AIRUserSurface tObject in AIRUserSurface.ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.PartyTag == PartyTag && tObject.InternalKey == sInternalKey)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null && sCreateIfNull == true)
            {
                rResult = AIRUserSurface.NewObject();
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