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
    public partial class NWDServerDomain : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDomain()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDomain(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
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
            if (string.IsNullOrEmpty(ServerDNS) == false)
            {
                InternalKey = ServerDNS;

                if (Dev == true)
                {
                    InternalKey = InternalKey + " dev";
                }
                if (Preprod == true)
                {
                    InternalKey = InternalKey + " prepord";
                }
                if (Prod == true)
                {
                    InternalKey = InternalKey + " prod";
                }
            }
            ServerDNS = NWDToolbox.TextProtect(NWDToolbox.CleanDNS(NWDToolbox.TextUnprotect(ServerDNS)));
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ValidInSelectedEnvironment()
        {
            return ValidInEnvironment(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ValidInEnvironment(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
            if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && Dev == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && Preprod == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && Prod == true))
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static void ResetServerHTTPS(NWDAppEnvironment sEnvironment)
        {
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            tAccountInfos.Server.SetData(null);
            tAccountInfos.UpdateDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
