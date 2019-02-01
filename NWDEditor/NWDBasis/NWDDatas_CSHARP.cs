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
        public Dictionary<int, string[]> CSV_OrderArray = new Dictionary<int, string[]>();
        public Dictionary<int, string[]> SQL_OrderArray = new Dictionary<int, string[]>();
        public Dictionary<int, string> SQL_Order = new Dictionary<int, string>();
        public Dictionary<int, List<string>> SLQ_IntegrityOrder = new Dictionary<int, List<string>>();
        public Dictionary<int, List<string>> SLQ_IntegrityServerOrder = new Dictionary<int, List<string>>();
        public Dictionary<int, List<string>> AssemblyPropertiesList = new Dictionary<int, List<string>>();
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

            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();

            // TODO : repport actual value...
            CSV_OrderArray.Clear();
            SQL_OrderArray.Clear();
            SQL_Order.Clear();
            SLQ_IntegrityOrder.Clear();
            SLQ_IntegrityServerOrder.Clear();
            AssemblyPropertiesList.Clear();
            foreach (KeyValuePair<int, bool> tKeyValue in tApp.WSList)
            {
                if (tKeyValue.Value == true)
                {
                    if (tApp.kWebBuildkCSVAssemblyOrderArray.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkCSVAssemblyOrderArray[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            CSV_OrderArray.Add(tKeyValue.Key, tApp.kWebBuildkCSVAssemblyOrderArray[tKeyValue.Key][ClassNamePHP]);
                        }

                    }

                    if (tApp.kWebBuildkSLQAssemblyOrderArray.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkSLQAssemblyOrderArray[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            SQL_OrderArray.Add(tKeyValue.Key, tApp.kWebBuildkSLQAssemblyOrderArray[tKeyValue.Key][ClassNamePHP]);
                        }

                    }

                    if (tApp.kWebBuildkSLQAssemblyOrder.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkSLQAssemblyOrder[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            SQL_Order.Add(tKeyValue.Key, tApp.kWebBuildkSLQAssemblyOrder[tKeyValue.Key][ClassNamePHP]);
                        }

                    }

                    if (tApp.kWebBuildkSLQIntegrityOrder.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkSLQIntegrityOrder[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            SLQ_IntegrityOrder.Add(tKeyValue.Key, tApp.kWebBuildkSLQIntegrityOrder[tKeyValue.Key][ClassNamePHP]);
                        }

                    }

                    if (tApp.kWebBuildkSLQIntegrityServerOrder.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkSLQIntegrityServerOrder[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            SLQ_IntegrityServerOrder.Add(tKeyValue.Key, tApp.kWebBuildkSLQIntegrityServerOrder[tKeyValue.Key][ClassNamePHP]);
                        }

                    }

                    if (tApp.kWebBuildkDataAssemblyPropertiesList.ContainsKey(tKeyValue.Key))
                    {
                        if (tApp.kWebBuildkDataAssemblyPropertiesList[tKeyValue.Key].ContainsKey(ClassNamePHP))
                        {
                            AssemblyPropertiesList.Add(tKeyValue.Key, tApp.kWebBuildkDataAssemblyPropertiesList[tKeyValue.Key][ClassNamePHP]);
                        }

                    }
                }
            }

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
            rReturn.AppendLine("tDatas.CSV_OrderArray.Clear();");
            rReturn.AppendLine("tDatas.SQL_OrderArray.Clear();");
            rReturn.AppendLine("tDatas.SQL_Order.Clear();");
            rReturn.AppendLine("tDatas.SLQ_IntegrityOrder.Clear();");
            rReturn.AppendLine("tDatas.SLQ_IntegrityServerOrder.Clear();");
            rReturn.AppendLine("tDatas.AssemblyPropertiesList.Clear();");
            foreach (KeyValuePair<int, string[]> tKeyValue in CSV_OrderArray.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.CSV_OrderArray.Add(" + tKeyValue.Key + ", new string[]{\"" + string.Join("\", \"", tKeyValue.Value) + "\"});");
                    }
                }
            }
            foreach (KeyValuePair<int, string[]> tKeyValue in SQL_OrderArray.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.SQL_OrderArray.Add(" + tKeyValue.Key + ", new string[]{\"" + string.Join("\", \"", tKeyValue.Value) + "\"});");
                    }
                }
            }
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
            foreach (KeyValuePair<int, List<string>> tKeyValue in SLQ_IntegrityOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.SLQ_IntegrityOrder.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }
            foreach (KeyValuePair<int, List<string>> tKeyValue in SLQ_IntegrityServerOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.SLQ_IntegrityServerOrder.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }

            foreach (KeyValuePair<int, List<string>> tKeyValue in AssemblyPropertiesList.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tDatas.AssemblyPropertiesList.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }
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