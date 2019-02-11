//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserPreference : NWDBasis<NWDUserPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserPreference()
        {
            //Debug.Log("NWDUserPreferences Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserPreference(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserPreferences Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDPreferenceKey), typeof(NWDAccountPreference), typeof(NWDUserPreference) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = BTBConstants.K_EMPTY_STRING)
        {
            NWDUserPreference rObject = FindFirstDatasByInternalKey(sInternalKey);
            if (rObject == null)
            {
                rObject = NewData();
#if UNITY_EDITOR
                rObject.InternalKey = sInternalKey;
                rObject.InternalDescription = sInternalDescription;
#endif
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