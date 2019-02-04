//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
            rGameSave = NewData();
            //rGameSave.InternalKey = NWDAccount.GetCurrentAccountReference();
            rGameSave.Name = "GameSave " + DateTime.Today.ToShortDateString();
            rGameSave.Tag = NWDBasisTag.TagUserCreated;
            rGameSave.IsCurrent = true;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            if (tAccountInfos != null)
            {
                tAccountInfos.CurrentGameSave.SetReference(rGameSave.Reference);
                tAccountInfos.SaveData();
            }
            rGameSave.SaveData();
            return rGameSave;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave Current()
        {
            NWDGameSave rParty = null;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.CurrentGameSave != null)
                {
                    NWDGameSave tParty = NWDGameSave.FindDataByReference(tAccountInfos.CurrentGameSave.GetReference());
                    if (tParty != null)
                    {
                        rParty = tParty;
                    }
                }
                else
                {
                }
            }
            if (rParty == null)
            {
                NWDGameSave[] tParties = NWDGameSave.FindDatas(NWDAccount.GetCurrentAccountReference(), null);
                foreach (NWDGameSave tPart in tParties)
                {
                    if (tPart != null)
                    {
                        rParty = tPart;
                        if (tAccountInfos != null)
                        {
                            if (tAccountInfos.CurrentGameSave == null)
                            {
                                tAccountInfos.CurrentGameSave = new NWDReferenceFreeType<NWDGameSave>();
                            }
                            tAccountInfos.CurrentGameSave.SetReference(rParty.Reference);
                            tAccountInfos.SaveData();
                        }
                        break;
                    }
                }
            }
            if (rParty == null)
            {
                rParty = NewCurrent();
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave CurrentForAccount(string sAccountReference)
        {
            NWDGameSave rParty = null;
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == sAccountReference)
                {
                    if (tParty.IsCurrent == true && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.TestIntegrity() == true)
                    {
                        rParty = tParty;
                        break;
                    }
                }
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrent()
        {
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == NWDAccount.GetCurrentAccountReference() && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.TestIntegrity() == true)
                {
                    tParty.IsCurrent = false;
                    tParty.SaveDataIfModified();
                }
            }
            NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
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
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDAccountInfos), typeof(NWDGameSave) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================