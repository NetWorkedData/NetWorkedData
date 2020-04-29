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

#if UNITY_EDITOR
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
            User = "wsuser" + NWDToolbox.RandomStringAlpha(3).ToLower();
            // no sync please
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerAuthentication GetServerSFTP(NWDAppEnvironment sEnvironment)
        {
            NWDServerAuthentication rReturn = null;
            NWDServerDomain tServerDNS = ServerDomain.GetRawData();
            if (Server != null)
            {
                NWDServer tServer = Server.GetRawData();
                if (tServerDNS != null)
                {

                    if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                    {
                        if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && tServerDNS.Dev == true) ||
                            (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && tServerDNS.Preprod == true) ||
                            (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && tServerDNS.Prod == true))
                        {
                            rReturn = new NWDServerAuthentication(NWDToolbox.TextUnprotect(tServerDNS.ServerDNS), tServer.Port, NWDToolbox.TextUnprotect(Folder), NWDToolbox.TextUnprotect(User), NWDToolbox.TextUnprotect(Password.ToString()));
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDServerAuthentication GetConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        //{
        //    NWDServerAuthentication rReturn = new NWDServerAuthentication(sEnvironment.SFTPHost, sEnvironment.SFTPPort, sEnvironment.SFTPFolder, sEnvironment.SFTPUser, sEnvironment.SFTPPassword);
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerAuthentication[] GetAllConfigurationServerSFTP(NWDAppEnvironment sEnvironment)
        {
            List<NWDServerAuthentication> rReturn = new List<NWDServerAuthentication>();
            //rReturn.Add(GetConfigurationServerSFTP(sEnvironment));
            foreach (NWDServerServices tSFTP in NWDBasisHelper.GetRawDatas<NWDServerServices>())
            {
                NWDServerAuthentication tConn = tSFTP.GetServerSFTP(sEnvironment);
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
#endif
