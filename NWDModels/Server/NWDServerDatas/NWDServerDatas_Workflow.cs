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
    public partial class NWDServerDatas : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            string tRandom = NWDToolbox.RandomStringAlpha(3).ToLower();
            InternalKey = "Unused config";
            MySQLUser = "dbuser" + tRandom;
            MySQLPort = 3306;
            MySQLBase = "NetWorkedData" + tRandom;
            External = true;
            PhpMyAdmin = true;
            // no sync please
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
            AccountRangeStart = 000;
            AccountRangeEnd = 999;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatabaseAuthentication GetServerDatabase(NWDAppEnvironment sEnvironment)
        {
            NWDServerDatabaseAuthentication rReturn = null;
            if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && Dev == true) ||
                (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && Preprod == true) ||
                (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && Prod == true))
            {
                rReturn = new NWDServerDatabaseAuthentication(NWDToolbox.TextUnprotect(MySQLIP.GetValue()), MySQLPort, NWDToolbox.TextUnprotect(MySQLBase), NWDToolbox.TextUnprotect(MySQLUser), NWDToolbox.TextUnprotect(MySQLPassword.ToString()));
            }
            return rReturn;
        }
    //-------------------------------------------------------------------------------------------------------------
    public static NWDServerDatabaseAuthentication GetConfigurationServerDatabase(NWDAppEnvironment sEnvironment)
    {
        NWDServerDatabaseAuthentication rReturn = new NWDServerDatabaseAuthentication(sEnvironment.ServerHost, 555, sEnvironment.ServerBase, sEnvironment.ServerUser, sEnvironment.ServerPassword);
        return rReturn;
    }
    //-------------------------------------------------------------------------------------------------------------
    public static NWDServerDatabaseAuthentication[] GetAllConfigurationServerDatabase(NWDAppEnvironment sEnvironment)
    {
        List<NWDServerDatabaseAuthentication> rReturn = new List<NWDServerDatabaseAuthentication>();
        rReturn.Add(GetConfigurationServerDatabase(sEnvironment));
        foreach (NWDServerDatas tServerDatabase in NWDBasisHelper.GetRawDatas<NWDServerDatas>())
        {
            NWDServerDatabaseAuthentication tConn = tServerDatabase.GetServerDatabase(sEnvironment);
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