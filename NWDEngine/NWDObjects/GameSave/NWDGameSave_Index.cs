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
        public static NWDGameSave Current()
        {
            NWDGameSave rParty = null;
            //NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.Current();
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
                NWDGameSave[] tParties = NWDGameSave.FindDatas(NWDAccount.CurrentReference(), null);
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
        public static NWDGameSave CurrentByAccount(string sAccountReference)
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================