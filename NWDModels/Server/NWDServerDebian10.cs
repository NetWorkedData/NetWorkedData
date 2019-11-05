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
#if UnityEditor
using System;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerDebian10
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServer(string sIP, int sPort, string sRoot, string sRootPassword)
        {

            StringBuilder tScriptServer = new StringBuilder();
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallWebService(string sIP, int sPort, string sRoot, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {

            StringBuilder tScriptServer = new StringBuilder();
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallMysql(string sIP, int sPort, string sRoot, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase, int sMySQLPort, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {

            StringBuilder tScriptServer = new StringBuilder();
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif