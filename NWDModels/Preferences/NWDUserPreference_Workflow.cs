//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserPreference : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserPreference()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserPreference(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = NWEConstants.K_EMPTY_STRING)
        {
            NWDUserPreference rObject = NWDBasisHelper.GetReacheableFirstDataByInternalKey<NWDUserPreference>(sInternalKey);
            if (rObject == null)
            {
                rObject = NWDBasisHelper.NewData<NWDUserPreference>();
                rObject.InternalKey = sInternalKey;
                rObject.InternalDescription = sInternalDescription;
                rObject.Value = sDefaultValue;
                rObject.Tag = NWDBasisTag.TagUserCreated;
                rObject.SaveData();
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(NWDMultiType sValue)
        {
            Value = sValue;
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType GetEnter()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Set(string sValue)
        {
            Value.SetStringValue(sValue);
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString(string sDefault = "")
        {
            return Value.GetStringValue(sDefault);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================