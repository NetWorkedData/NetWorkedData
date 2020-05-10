//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
    public partial class NWDAccountNickname : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        //public delegate void SyncNicknameBlock(bool error, NWDOperationResult result = null);
        //public SyncNicknameBlock SyncNicknameBlockDelegate;
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
            return Enrichment(sText, NWDBasisHelper.GetReachableFirstData<NWDAccountNickname>(), sLanguage, sBold);
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
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDAccountNickname NewNickname(string name)
        //{
        //    NWDAccountNickname rNickname = NewData();
        //    rNickname.InternalKey = name;
        //    rNickname.InternalDescription = string.Empty;
        //    rNickname.Nickname = name;
        //    rNickname.UpdateData();
        //    return rNickname;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static string GetNickname()
        {
            NWDAccountNickname tNickname = NWDAccountNickname.CurrentData();
            return tNickname.Nickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetUniqueNickname()
        {
            NWDAccountNickname tNickname = NWDAccountNickname.CurrentData();
            return tNickname.UniqueNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void SyncNickname()
        //{
        //    // Sync with the server
        //    List<Type> tList = new List<Type>
        //    {
        //        typeof(NWDAccountNickname)
        //    };

        //    NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
        //    {
        //        if (SyncNicknameBlockDelegate != null)
        //        {
        //            SyncNicknameBlockDelegate(false);
        //        }
        //    };
        //    NWEOperationBlock tFailed = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bInfos)
        //    {
        //        NWDOperationResult tInfos = bInfos as NWDOperationResult;
        //        if (SyncNicknameBlockDelegate != null)
        //        {
        //            SyncNicknameBlockDelegate(true, tInfos);
        //        }
        //    };
        //    NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(tList, tSuccess, tFailed);
        //}
        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================