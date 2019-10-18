//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDNS : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDNS()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDNS(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            BalanceLoad = 90;
            InternalKey = "Unused server";
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            InternalKey = "Unused server";
            if (string.IsNullOrEmpty(ServerHTTPS) == false)
            {
                InternalKey = ServerHTTPS;
            }
            InternalDescription = "";
            if (string.IsNullOrEmpty(Name) == false)
            {
                InternalDescription = Name;
            }
            ServerHTTPS = NWDToolbox.TextProtect(NWDToolbox.CleanDNS(NWDToolbox.TextUnprotect(ServerHTTPS)));
        }
        //-------------------------------------------------------------------------------------------------------------
        static void ResetServerHTTPS(NWDAppEnvironment sEnvironment)
        {
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            tAccountInfos.Server.SetData(null); ;
            tAccountInfos.UpdateDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
