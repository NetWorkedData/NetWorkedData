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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerServices : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerServices()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerServices(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            InternalKey = "Unused config";
            Port = 22;
            User = NWDAppEnvironment.SelectedEnvironment().SFTPUser;
            Folder = "public_webservice";
            IP.SetValue("192.168.0.1");
            ServerName = "MyServer";
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            InternalKey = "Unused config";
            NWDServerDomain tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                {
                    InternalKey = tServerDNS.InternalKey + " config";
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerAuthentification GetServerSFTP(NWDAppEnvironment sEnvironment)
        {
            NWDServerAuthentification rReturn = null;
            NWDServerDomain tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {

                if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                {
                    if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && tServerDNS.Dev == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && tServerDNS.Preprod == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && tServerDNS.Prod == true))
                    {
                        rReturn = new NWDServerAuthentification(NWDToolbox.TextUnprotect(tServerDNS.ServerDNS), Port, NWDToolbox.TextUnprotect(Folder), NWDToolbox.TextUnprotect(User), NWDToolbox.TextUnprotect(Password.ToString()));
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerAuthentification GetConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        {
            NWDServerAuthentification rReturn = new NWDServerAuthentification(sEnvironment.SFTPHost, sEnvironment.SFTPPort, sEnvironment.SFTPFolder, sEnvironment.SFTPUser, sEnvironment.SFTPPassword);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerAuthentification[] GetAllConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        {
            List<NWDServerAuthentification> rReturn = new List<NWDServerAuthentification>();
            rReturn.Add(GetConfigurationServerSFTP(sEnvironment));
            foreach (NWDServerServices tSFTP in NWDBasisHelper.GetRawDatas<NWDServerServices>())
            {
                NWDServerAuthentification tConn = tSFTP.GetServerSFTP(sEnvironment);
                if (tConn != null)
                {
                    rReturn.Add(tConn);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
