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
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 40);
            int tI = 0;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Open Terminal"))
            {
                // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                System.Diagnostics.Process.Start(tFileInfo.FullName);
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server MySQL command");
            tI++;
            GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerMySQL(Distribution, IP.GetValue(), Port, Root_User, Root_Password.GetValue(), Root_MysqlPassword.GetValue(), External, PhpMyAdmin));
            tI += 11;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            
            GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Database command");
            tI++;
            GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallDatabase(Distribution, IP.GetValue(), Port, Root_User, Root_MysqlPassword.GetValue(), MySQLUser, MySQLPassword.GetValue(), MySQLBase));
            tI += 11;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force dev editor data"))
            {
                //TODO : push data ...
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force preprod editor data"))
            {
                //TODO : push data ...
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force prod editor data"))
            {
                //TODO : push data ...
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            if (PhpMyAdmin == true)
            {

                if (GUI.Button(tMatrix[0, tI], "http://" + IP + "/PhpMyAmdin/"))
                {
                    Application.OpenURL("http://" + IP + "/PhpMyAmdin/");
                }
                if (GUI.Button(tMatrix[1, tI], "https://" + IP + "/PhpMyAmdin/"))
                {
                    Application.OpenURL("https://" + IP + "/PhpMyAmdin/");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif