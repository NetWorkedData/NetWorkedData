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
    public partial class NWDUserStats : NWDBasis<NWDUserStats>
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
        //public NWDParty UserStatsNewObject()
        //{
        //    NWDParty rResult = NWDParty.NewObject();
        //    rResult.GameSaveTag = GameSaveTag;
        //    rResult.SaveModifications();
        //    return rResult;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void UserStatsTrash()
        {
            foreach (NWDUserStats tObject in UserStatsList())
            {
                tObject.TrashData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDUserStats> UserStatsList()
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            List<NWDUserStats> rResult = new List<NWDUserStats>();
            foreach (NWDUserStats tObject in NWDUserStats.Datas().ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.GameSaveTag == GameSaveTag)
                {
                    rResult.Add(tObject);
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats UserStatsByInternalKey(string sInternalKey, bool sCreateIfNull = false)
        {
            string tPlayerAccountReference = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
            NWDUserStats rResult = null;
            foreach (NWDUserStats tObject in NWDUserStats.Datas().ObjectsList)
            {
                if (tObject.IsReacheableByAccount(tPlayerAccountReference) && tObject.GameSaveTag == GameSaveTag && tObject.InternalKey == sInternalKey)
                {
                    rResult = tObject;
                    break;
                }
            }
            if (rResult == null && sCreateIfNull==true)
            {
                rResult = NWDUserStats.NewData();
                rResult.InternalKey = sInternalKey;
                rResult.Tag = NWDBasisTag.TagUserCreated;
                rResult.GameSaveTag = GameSaveTag;
                rResult.UpdateData();
            }
            return rResult;
        }

        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats NewIntStat(string sInternalKey, int sInt)
        {
            NWDUserStats rReturn = NWDUserStats.NewData();
            rReturn.InternalKey = sInternalKey;
            rReturn.IntValue = sInt;
            rReturn.GameSaveTag = GameSaveTag;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats NewFloatStat(string sInternalKey, float sFloat)
        {
            NWDUserStats rReturn = NWDUserStats.NewData();
            rReturn.InternalKey = sInternalKey;
            rReturn.FloatValue = sFloat;
            rReturn.GameSaveTag = GameSaveTag;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats NewBoolStat(string sInternalKey, bool sBool)
        {
            NWDUserStats rReturn = NWDUserStats.NewData();
            rReturn.InternalKey = sInternalKey;
            rReturn.BoolValue = sBool;
            rReturn.GameSaveTag = GameSaveTag;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats NewStringStat(string sInternalKey, string sString)
        {
            NWDUserStats rReturn = NWDUserStats.NewData();
            rReturn.InternalKey = sInternalKey;
            rReturn.StringValue = sString;
            rReturn.GameSaveTag = GameSaveTag;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats GetStatByInternalKey(string sInternalKey)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats GetStatByInternalKeyOrCreate(string sInternalKey)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
            rReturn.UpdateData();
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats ModifyStat(string sInternalKey, int sInt)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
            rReturn.IntValue = sInt;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats ModifyStat(string sInternalKey, float sFloat)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
            rReturn.FloatValue = sFloat;
            rReturn.UpdateData();
            return rReturn;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDUserStats ModifyStat(string sInternalKey, double sDouble)
        //{
        //    NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
        //    rReturn.DoubleValue = sDouble;
        //    rReturn.SaveModifications();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats ModifyStat(string sInternalKey, bool sBool)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
            rReturn.BoolValue = sBool;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserStats ModifyStat(string sInternalKey, string sString)
        {
            NWDUserStats rReturn = UserStatsByInternalKey(sInternalKey, true);
            rReturn.StringValue = sString;
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================