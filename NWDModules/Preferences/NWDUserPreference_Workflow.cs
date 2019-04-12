// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:33
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        public static NWDUserPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = BTBConstants.K_EMPTY_STRING)
        {
            NWDUserPreference rObject = FindFirstDataByInternalKey(sInternalKey);
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
        /*public static NWDUserPreference GetByPreferenceKeyOrCreate(NWDPreferenceKey sPreferenceKey, NWDMultiType sDefaultValue)
        {
            NWDUserPreference rObject = null;
            NWDUserPreference[] tUserPreferences = FindDatas();
            foreach (NWDUserPreference k in tUserPreferences)
            {
                if (k.PreferenceKey.GetObject().Equals(sPreferenceKey))
                {
                    rObject = k;
                    break;
                }
            }
            
            if (rObject == null)
            {
                rObject = NewData();
                #if UNITY_EDITOR
                rObject.InternalKey = sPreferenceKey.Title.GetBaseString();
                rObject.InternalDescription = sPreferenceKey.Description.GetBaseString();
                #endif
                rObject.Value = sDefaultValue;
                rObject.Tag = NWDBasisTag.TagUserCreated;
                rObject.SaveData();
            }
            
            return rObject;
        }*/
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