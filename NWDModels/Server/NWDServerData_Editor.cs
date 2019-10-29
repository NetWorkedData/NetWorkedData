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
using UnityEngine;
using UnityEditor;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDatas : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            float tYadd = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 40);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerNySQL(NWDServerDistribution sDistribution, string sIP, int sPort, string sServerName, string sNewRootPassword, bool sMySQL, bool sPhpMyAdmin, bool sPostfix)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("ssh -l root " + sIP + " -p22");
            if (sPort != 22)
            {
                tScriptServer.AppendLine("# or ssh -l root " + sIP + " -p" + sPort);
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 40);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif