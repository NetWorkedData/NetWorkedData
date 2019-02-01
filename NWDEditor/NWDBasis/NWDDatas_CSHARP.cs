//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDatas
    {
        //-------------------------------------------------------------------------------------------------------------
        public int LastWebBuild = 0;
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARPCallLoader()
        {
            BTBBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine(ClassNamePHP + NWD.K_LOADER + "();");
            BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARP()
        {
            BTBBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("NWDDatas tDatas = null;");
            rReturn.AppendLine("tDatas = NWDDatas.FindTypeInfos(typeof("+ ClassNamePHP + "));");
            rReturn.AppendLine("if (tDatas!=null)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("tDatas.SaltStart = \"" + SaltStart.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine("tDatas.SaltEnd = \"" + SaltEnd.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine("tDatas.SaltValid = true;"); // salt was reccord because loaded :-p
            rReturn.AppendLine("tDatas.LastWebBuild = " + LastWebBuild + ";");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif