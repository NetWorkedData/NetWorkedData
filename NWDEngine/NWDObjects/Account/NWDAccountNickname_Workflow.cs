//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountNickname : NWDBasis<NWDAccountNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void SyncNicknameBlock(bool error, NWDOperationResult result = null);
        public SyncNicknameBlock SyncNicknameBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountNickname()
        {
            //Debug.Log("NWDAccountNickname Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountNickname(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccountNickname Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            // Get First nickname found and return a new string
            return Enrichment(sText, GetFirstData(), sLanguage, sBold);
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : clean
        public static string Enrichment(string sText, NWDAccountNickname sNickname, string sLanguage = null, bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";

            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }

            // Replace the nickname
            string tNickname = string.Empty;
            string tNicknameID = string.Empty;
            if (sNickname != null)
            {
                tNickname = sNickname.Nickname;
                tNicknameID = sNickname.UniqueNickname;
            }

            // Replace the old tag nickname
            rText = rText.Replace("#AccountNickname#", tBstart + tNickname + tBend);
            rText = rText.Replace("#UniqueAccountNickname#", tBstart + tNicknameID + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountNickname NewNickname(string name)
        {
            NWDAccountNickname rNickname = NewData();
            rNickname.InternalKey = name;
            rNickname.InternalDescription = string.Empty;
            rNickname.Nickname = name;
            rNickname.UpdateData();
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetNickname()
        {
            string rNickname = string.Empty;
            NWDAccountNickname[] tNickname = FindDatas();
            if (tNickname.Length > 0)
            {
                rNickname = tNickname[0].Nickname;
            }
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetUniqueNickname()
        {
            string rUniqueNickname = string.Empty;
            NWDAccountNickname[] tNickname = FindDatas();
            if (tNickname.Length > 0)
            {
                rUniqueNickname = tNickname[0].UniqueNickname;
            }
            return rUniqueNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncNickname()
        {
            // Sync with the server
            List<Type> tList = new List<Type>
            {
                typeof(NWDAccountNickname)
            };

            BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                if (SyncNicknameBlockDelegate != null)
                {
                    SyncNicknameBlockDelegate(false);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bInfos)
            {
                NWDOperationResult tInfos = bInfos as NWDOperationResult;
                if (SyncNicknameBlockDelegate != null)
                {
                    SyncNicknameBlockDelegate(true, tInfos);
                }
            };
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        }
        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================