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
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDServerDistribution
    {
        debian9,
        debian10,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerInstall
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerApache(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        tScriptServer.Append( NWDServerDebian9.CommandInstallServerApache(sIP, sPort, sRoot, sRootPassword));
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        tScriptServer.Append(NWDServerDebian10.CommandInstallServerApache(sIP, sPort, sRoot, sRootPassword));
                    }
                    break;
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerMySQL(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword, string sRootMySQLPassword, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptServer = new StringBuilder();
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        tScriptServer.Append(NWDServerDebian9.CommandInstallServerMySQL(sIP, sPort, sRoot, sRootPassword, sRootMySQLPassword, sMySQLExternal,  sMySQLPhpMyAdmin));
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        tScriptServer.Append(NWDServerDebian10.CommandInstallServerMySQL(sIP, sPort, sRoot, sRootPassword, sRootMySQLPassword,  sMySQLExternal,  sMySQLPhpMyAdmin));
                    }
                    break;
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallWebService(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptServer = new StringBuilder();
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        tScriptServer.Append(NWDServerDebian9.CommandInstallWebService(sIP, sPort, sRoot, sRootPassword, sDNS, sUser, sPassword, sFolder, sEmail));
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        tScriptServer.Append(NWDServerDebian10.CommandInstallWebService(sIP, sPort, sRoot, sRootPassword, sDNS, sUser, sPassword, sFolder, sEmail));
                    }
                    break;
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallDatabase(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase)
        {
            StringBuilder tScriptServer = new StringBuilder();
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        tScriptServer.Append(NWDServerDebian9.CommandInstallDatabase(sIP, sPort, sRoot, sRootPassword, sMySQLUser, sMySQLPassword, sMySQLBase));
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        tScriptServer.Append(NWDServerDebian10.CommandInstallDatabase(sIP, sPort, sRoot, sRootPassword, sMySQLUser, sMySQLPassword, sMySQLBase));
                    }
                    break;
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandDNS(string sServerHTTPS, string sIP)
        {
            StringBuilder tScriptDNS = new StringBuilder();
            tScriptDNS.AppendLine(sServerHTTPS + " 10800 A " + sIP);
            return tScriptDNS.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif