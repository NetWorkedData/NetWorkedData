//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQLite4Unity3d;
using BasicToolBox;
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
        public bool WebModelChanged = false;
        public bool WebModelDegraded = false;
        public List<string> WebModelDegradationList = new List<string>();
        public Dictionary<int, int> WS_Model = new Dictionary<int, int>();
        public Dictionary<int, List<string>> PropertiesOrderArrayBBB = new Dictionary<int, List<string>>();
        public Dictionary<int, string> SQL_Order = new Dictionary<int, string>();
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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

            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();

            // Write data ...
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("NWDDatas tDatas = null;");
            rReturn.AppendLine("tDatas = NWDDatas.FindTypeInfos(typeof(" + ClassNamePHP + "));");
            rReturn.AppendLine("if (tDatas!=null)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("tDatas.SaltStart = \"" + SaltStart.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tDatas.SaltEnd = \"" + SaltEnd.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tDatas.SaltValid = true;"); // salt was reccord because loaded :-p
            rReturn.AppendLine("tDatas.LastWebBuild = " + LastWebBuild + ";");

            rReturn.AppendLine("tDatas.PropertiesOrderArrayBBB.Clear();");
            foreach (KeyValuePair<int, List<string>> tKeyValue in PropertiesOrderArrayBBB.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.PropertiesOrderArrayBBB.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }

            rReturn.AppendLine("tDatas.WS_Model.Clear();");
            foreach (KeyValuePair<int, int> tKeyValue in WS_Model.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.WS_Model.Add(" + tKeyValue.Key + ", " + tKeyValue.Value + ");");
                    }
                }
            }
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("tDatas.SQL_Order.Clear();");
            foreach (KeyValuePair<int, string> tKeyValue in SQL_Order.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.SQL_Order.Add(" + tKeyValue.Key + ", \"" + tKeyValue.Value.Replace("\"", "\\\"") + "\");");
                    }
                }
            }
            rReturn.AppendLine("tDatas.WebModelChanged = "+ ClassNamePHP +".ModelChanged();");
            rReturn.AppendLine("tDatas.WebModelDegraded = "+ ClassNamePHP +".ModelDegraded();");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public string CreationIntegrityCSHARP()
        //{
        //    BTBBenchmark.Start();
        //    StringBuilder rReturn = new StringBuilder(string.Empty);
        //    rReturn.AppendLine("public partial class "+ ClassNamePHP+ " : NWDBasis<ClassNamePHP>");
        //    rReturn.AppendLine("{");
        //    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
        //    rReturn.AppendLine("public override string IntegrityEvaluateSpecial ()");
        //    rReturn.AppendLine("{");
        //    rReturn.AppendLine("string rReturn = string.Empty;");
        //    rReturn.AppendLine("return rReturn;");
        //    rReturn.AppendLine("}");
        //    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
        //    rReturn.AppendLine("public override string IntegrityEvaluateSpecial ()");
        //    rReturn.AppendLine("{");
        //    rReturn.AppendLine("string rReturn = string.Empty;");
        //    rReturn.AppendLine("return rReturn;");
        //    rReturn.AppendLine("}");
        //    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
        //    rReturn.AppendLine("}");
        //    rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        //    BTBBenchmark.Finish();
        //    return rReturn.ToString();
        //}
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    }
//=====================================================================================================================