//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================
using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGameSave : NWDBasisAccountDependent
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
#if UNITY_EDITOR
            //Debug.Log("NWDGameSave Initialization()");
            Name = " * GameSave " + DateTime.Today.ToShortDateString();
            InternalKey = "GameSave " + DateTime.Today.ToShortDateString();
            Tag = NWDBasisTag.TagTestForDev;
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave NewCurrent()
        {
            NWDGameSave rGameSave = null;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos != null)
            {
                rGameSave = NWDBasisHelper.NewData<NWDGameSave>();
                //rGameSave.InternalKey = NWDAccount.GetCurrentAccountReference();
                rGameSave.Name = "GameSave " + DateTime.Today.ToShortDateString();
                rGameSave.Tag = NWDBasisTag.TagUserCreated;
                rGameSave.IsCurrent = true;
                //NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
                tAccountInfos.CurrentGameSave.SetReference(rGameSave.Reference);
                tAccountInfos.SaveData();
                rGameSave.SaveData();
            }
            return rGameSave;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrent()
        {
            foreach (NWDGameSave tParty in BasisHelper().Datas)
            {
                if (tParty.Account.GetReference() == NWDAccount.CurrentReference() && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.IntegrityIsValid() == true)
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
                    tAccountInfos.CurrentGameSave = new NWDReferenceType<NWDGameSave>();
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