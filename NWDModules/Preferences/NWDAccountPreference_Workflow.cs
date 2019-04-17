// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:25
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
        public static NWDAccountPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = BTBConstants.K_EMPTY_STRING)
        {
            NWDAccountPreference rObject = GetReacheableFirstDataByInternalKey(sInternalKey,false);
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