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
    public partial class NWDServerSFTP : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerSFTP()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerSFTP(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            InternalKey = "Unused config";
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            InternalKey = "Unused config";
            NWDServerDNS tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {

                if (string.IsNullOrEmpty(tServerDNS.ServerHTTPS) == false)
                {
                    InternalKey = tServerDNS.ServerHTTPS;
                }
            }
            InternalDescription = "";
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.Name) == false)
                {
                    InternalDescription = tServerDNS.Name;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerAuthentification GetServerSFTP(NWDAppEnvironment sEnvironment)
        {
            NWDServerAuthentification rReturn = null;
            NWDServerDNS tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {

                if (string.IsNullOrEmpty(tServerDNS.ServerHTTPS) == false)
                {
                    if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && tServerDNS.Dev == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && tServerDNS.Preprod == true) ||
                        (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && tServerDNS.Prod == true))
                    {
                        rReturn = new NWDServerAuthentification(NWDToolbox.TextUnprotect(tServerDNS.ServerHTTPS), Port, NWDToolbox.TextUnprotect(Folder), NWDToolbox.TextUnprotect(User), NWDToolbox.TextUnprotect(Password), tServerDNS.BalanceLoad);
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerAuthentification GetConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        {
            NWDServerAuthentification rReturn = new NWDServerAuthentification(sEnvironment.SFTPHost, sEnvironment.SFTPPort, sEnvironment.SFTPFolder, sEnvironment.SFTPUser, sEnvironment.SFTPPassword, sEnvironment.SFTPBalanceLoad);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerAuthentification[] GetAllConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        {
            List<NWDServerAuthentification> rReturn = new List<NWDServerAuthentification>();
            rReturn.Add(GetConfigurationServerSFTP(sEnvironment));
            foreach (NWDServerSFTP tSFTP in NWDBasisHelper.GetRawDatas<NWDServerSFTP>())
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
