// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNickname : NWDBasis<NWDUserNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void SyncNicknameBlock(bool error, NWDOperationResult result = null);
        public SyncNicknameBlock SyncNicknameBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname()
        {
            //Debug.Log("NWDUserNickname Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNickname(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserNickname Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, bool sBold = true)
        {
            // Get First nickname found and return a new string
            return Enrichment(sText, GetCorporateFirstData(), sBold);
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : clean
        public static string Enrichment(string sText, NWDUserNickname sNickname, bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }
            /*if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }*/
            // Replace the nickname
            string tNickname = string.Empty;
            string tNicknameID = string.Empty;
            if (sNickname != null)
            {
                tNickname = sNickname.Nickname;
                tNicknameID = sNickname.UniqueNickname;
            }
            // Replace the old tag nickname
            //rText = rText.Replace("@nickname@", tBstart + tNickname + tBend);
            //rText = rText.Replace("@nicknameid@", tBstart + tNicknameID + tBend);
            // Replace the new tag nickname
            rText = rText.Replace("#AccountNickname#", tBstart + tNickname + tBend);
            rText = rText.Replace("#UniqueAccountNickname#", tBstart + tNicknameID + tBend);
            //// Replace the standard tag nickname
            //rText = rText.Replace("{Nickname}", tBstart + tNickname + tBend);
            //rText = rText.Replace("{UniqueNickname}", tBstart + tNicknameID + tBend);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNickname NewNickname(string name)
        {
            NWDUserNickname rNickname = NewData();
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
            NWDUserNickname[] tNickname = GetReachableDatas();
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
            NWDUserNickname[] tNickname = GetReachableDatas();
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
                typeof(NWDUserNickname)
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