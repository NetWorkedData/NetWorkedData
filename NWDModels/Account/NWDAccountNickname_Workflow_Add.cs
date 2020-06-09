//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNicknameStyle : int
    {
        Humain,
        Dwarf,
        Elf,
        Ork,

        //...
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNicknameGenderStyle : int
    {
        Male,
        Female,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountNickname : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string GenerateName(NWDNicknameStyle sStyle, NWDNicknameGenderStyle sGender, int sLenght)
        {
            string rReturn = string.Empty;
            List<string> tRandom = new List<string>();
            // prepare random
            switch (sStyle)
            {
                case NWDNicknameStyle.Humain:
                    {
                        switch (sGender)
                        {
                            case NWDNicknameGenderStyle.Male: { tRandom.AddRange(new string[] { "", "" }); } break;
                            case NWDNicknameGenderStyle.Female: { tRandom.AddRange(new string[] { "", "" }); } break;
                        }
                    }
                    break;
            }
            // randomize
            if (tRandom.Count > 0)
            {
                for (int i = 0; i < sLenght; i++)
                {
                    tRandom.ShuffleList();
                    rReturn += tRandom[0];
                }
            }
            // post render
            switch (sStyle)
            {
                case NWDNicknameStyle.Humain:
                    {

                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================