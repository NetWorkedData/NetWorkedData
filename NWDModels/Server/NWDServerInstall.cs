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
        public static string CommandInstallServer(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
#if UnityEditor
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        return NWDServerDebian9.CommandInstallServer(sIP, sPort, sRoot, sRootPassword);
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        return NWDServerDebian10.CommandInstallServer(sIP, sPort, sRoot, sRootPassword);
                    }
                    break;
            }
#endif
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallWebService(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptAppache = new StringBuilder();
#if UnityEditor
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                        return NWDServerDebian9.CommandInstallWebService(sIP, sPort, sRoot, sRootPassword, sDNS, sUser, sPassword, sFolder, sEmail);
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                        return NWDServerDebian10.CommandInstallWebService(sIP, sPort, sRoot, sRootPassword, sDNS, sUser, sPassword, sFolder, sEmail);
                    }
                    break;
            }
#endif
            return tScriptAppache.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallMysql(NWDServerDistribution sDistribution, string sIP, int sPort, string sRoot, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase, int sMySQLPort, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptAppache = new StringBuilder();
#if UnityEditor
            switch (sDistribution)
            {
                case NWDServerDistribution.debian9:
                    {
                       return NWDServerDebian9.CommandInstallMysql(sIP, sPort, sRoot, sRootPassword, sMySQLUser, sMySQLPassword, sMySQLBase, sMySQLPort, sMySQLExternal, sMySQLPhpMyAdmin);
                    }
                    break;
                case NWDServerDistribution.debian10:
                    {
                       return NWDServerDebian10.CommandInstallMysql(sIP, sPort, sRoot, sRootPassword, sMySQLUser, sMySQLPassword, sMySQLBase, sMySQLPort, sMySQLExternal, sMySQLPhpMyAdmin);
                    }
                    break;
            }
#endif
            return tScriptAppache.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
