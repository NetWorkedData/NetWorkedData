//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:2
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
    public partial class NWDGameSave : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave CurrentData()
        {
            NWDGameSave rParty = null;
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.CurrentGameSave != null)
                {
                    NWDGameSave tParty = NWDBasisHelper.GetRawDataByReference<NWDGameSave>(tAccountInfos.CurrentGameSave.GetReference());
                    if (tParty != null)
                    {
                        if (tParty.Account.GetReference() == tAccountInfos.Account.GetReference())
                        {
                            rParty = tParty;
                        }
                    }
                }
            }
            if (rParty == null)
            {
                NWDGameSave[] tParties = NWDBasisHelper.GetCorporateDatas<NWDGameSave>(NWDAccount.CurrentReference());
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
                if (rParty == null && tAccountInfos != null)
                {
                    rParty = NewCurrent();
                }
            }
            return rParty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDGameSave SelectCurrentDataForAccount(string sAccountReference)
        {
            NWDGameSave rParty = null;
            foreach (NWDGameSave tParty in NWDBasisHelper.BasisHelper<NWDGameSave>().Datas)
            {
                if (tParty.Account.GetReference() == sAccountReference)
                {
                    if (tParty.IsCurrent == true && tParty.IsEnable() && tParty.IsTrashed() == false && tParty.IntegrityIsValid() == true)
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