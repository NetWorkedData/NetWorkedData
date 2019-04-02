//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("APR")]
    [NWDClassDescriptionAttribute("Account Preference")]
    [NWDClassMenuNameAttribute("Account Preference")]
    public partial class NWDAccountPreference : NWDBasis<NWDAccountPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountPreference()
        {
            //Debug.Log("NWDUserStatKeyValue Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountPreference(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserStatKeyValue Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
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
        public static NWDAccountPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = BTBConstants.K_EMPTY_STRING)
        {
            NWDAccountPreference rObject = FindFirstDatasByInternalKey(sInternalKey);
            if (rObject == null)
            {
                rObject = NewData();
                #if UNITY_EDITOR
                rObject.InternalKey = sInternalKey;
                rObject.InternalDescription = sInternalDescription;
                #endif
                rObject.Value = sDefaultValue;
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================