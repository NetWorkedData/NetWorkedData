//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGameSave : NWDBasis<NWDGameSave>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave()
        {
            //Debug.Log("NWDGameSave Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGameSave(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDGameSave Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            //Debug.Log("NWDGameSave Initialization()");
            Name = " * GameSave " + DateTime.Today.ToShortDateString();
            Tag = NWDBasisTag.TagAdminCreated;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave NewCurrent()
        {
            NWDGameSave rGameSave = null;
            rGameSave = NWDBasisHelper.NewData<NWDGameSave>();
            //rGameSave.InternalKey = NWDAccount.GetCurrentAccountReference();
            rGameSave.Name = "GameSave " + DateTime.Today.ToShortDateString();
            rGameSave.Tag = NWDBasisTag.TagUserCreated;
            rGameSave.IsCurrent = true;
            //NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos != null)
            {
                tAccountInfos.CurrentGameSave.SetReference(rGameSave.Reference);
                tAccountInfos.SaveData();
            }
            rGameSave.SaveData();
            return rGameSave;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrent()
        {
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == NWDAccount.CurrentReference() && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.TestIntegrity() == true)
                {
                    tParty.IsCurrent = false;
                    tParty.SaveDataIfModified();
                }
            }
            //NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.CurrentGameSave == null)
                {
                    tAccountInfos.CurrentGameSave = new NWDReferenceFreeType<NWDGameSave>();
                }
                tAccountInfos.CurrentGameSave.SetReference(Reference);
                tAccountInfos.SaveData();
            }
            IsCurrent = true;
            SaveDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================