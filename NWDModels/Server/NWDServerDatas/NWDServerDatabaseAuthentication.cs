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
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using Renci.SshNet;
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerDatabaseAuthentication
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Host;
        public int Port;
        public string Database;
        public string User;
        public string Password;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatabaseAuthentication(string sHost, int sPort, string sDatabase, string sUser, string sPassword)
        {
            Host = NWDToolbox.CleanDNS(sHost);
            Port = sPort;
            Database = sDatabase;
            User = sUser;
            Password = sPassword;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
