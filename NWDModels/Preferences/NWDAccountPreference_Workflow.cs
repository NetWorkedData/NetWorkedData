//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================


using System;
using UnityEngine;
using System.Collections.Generic;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("APR")]
    [NWDClassDescriptionAttribute("Account Preference")]
    [NWDClassMenuNameAttribute("Account Preference")]
    public partial class NWDAccountPreference : NWDBasisAccountDependent
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
        public static NWDAccountPreference GetByInternalKeyOrCreate(string sInternalKey, NWDMultiType sDefaultValue, string sInternalDescription = NWEConstants.K_EMPTY_STRING)
        {
            NWDAccountPreference rObject = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDAccountPreference));
            if (tHelper != null)
            {
                if (tHelper.IsLoaded())
                {
                    rObject = NWDBasisHelper.GetReacheableFirstDataByInternalKey<NWDAccountPreference>(sInternalKey);
                    if (rObject == null)
                    {
                        rObject = NWDBasisHelper.NewData<NWDAccountPreference>();
                        rObject.PropertiesPrevent();
                        rObject.InternalKey = sInternalKey;
                        rObject.InternalDescription = sInternalDescription;
                        rObject.Value = sDefaultValue;
                        rObject.SaveData();
                    }
                }
                else
                {
                    Debug.LogWarning("tHelper for "+ tHelper.ClassNamePHP +  " is not loaded!");
                }
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