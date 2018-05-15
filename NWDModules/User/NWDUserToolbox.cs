//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using BasicToolBox;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDUserToolbox
	{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static string Enrichment(string sText,
                                        string sLanguage = null,
                                        bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            // Replace the nickname
            NWDUserNickname tNickNameObject = NWDUserNickname.GetFirstObject();
            string tNickname = "";
            string tNicknameID = "";
            if (tNickNameObject != null)
            {
                tNickname = tNickNameObject.Nickname;
                tNicknameID = tNickNameObject.UniqueNickname;
            }

            rText = rText.Replace("@nickname@", tBstart + tNickname + tBend);
            rText = rText.Replace("@nicknameid@", tBstart + tNicknameID + tBend);

            rText = rText.Replace("#Nickname#", tBstart + tNickname + tBend);
            rText = rText.Replace("#Nicknameid#", tBstart + tNicknameID + tBend);

            return rText;
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
