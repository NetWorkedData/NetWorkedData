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
        // WebModel used by WebServiceBuild

        public bool WebModelChanged = false;
        public bool WebModelDegraded = false;
        public List<string> WebModelDegradationList = new List<string>();

        public Dictionary<int, int> WS_Model = new Dictionary<int, int>(); // => use webmodel for webservicesbuild
        // Properties order by WebModel
        public Dictionary<int, List<string>> PropertiesOrderArrayBBB = new Dictionary<int, List<string>>(); // => PropertiesOrderArray !!!

        public Dictionary<int, string> SQL_Order = new Dictionary<int, string>();  // => K . SLQSelect();   : to select Datas  =  CSV_OrderArray, with rewrite float, double, etc.


        //public Dictionary<int, List<string>> IntegrityOrderArray = new Dictionary<int, List<string>>(); // => IntegrityOrderArray !!!
        //public Dictionary<int, string[]> CSV_OrderArray = new Dictionary<int, string[]>(); // =>CSVAssemblyOrderArray 
        //public Dictionary<int, string[]> SQL_OrderArray = new Dictionary<int, string[]>(); // =>SLQAssemblyOrderArray   : to update(/insert) datas  = CSV_OrderArray - "Reference" TODO : remove?
        //public Dictionary<int, List<string>> SLQ_IntegrityOrder = new Dictionary<int, List<string>>();
        //public Dictionary<int, List<string>> SLQ_IntegrityServerOrder = new Dictionary<int, List<string>>();
        //public Dictionary<int, List<string>> AssemblyPropertiesList = new Dictionary<int, List<string>>();
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
            //rReturn.AppendLine("tDatas.AssemblyPropertiesList.Clear();");
            //foreach (KeyValuePair<int, List<string>> tKeyValue in AssemblyPropertiesList.OrderBy(x => x.Key))
            //{
            //    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
            //    {
            //        if (tApp.WSList[tKeyValue.Key] == true)
            //        {
            //            rReturn.AppendLine("tDatas.AssemblyPropertiesList.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
            //        }
            //    }
            //}

            //rReturn.AppendLine("tDatas.CSV_OrderArray.Clear();");
            //foreach (KeyValuePair<int, string[]> tKeyValue in CSV_OrderArray.OrderBy(x => x.Key))
            //{
            //    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
            //    {
            //        if (tApp.WSList[tKeyValue.Key] == true)
            //        {
            //            rReturn.AppendLine("tDatas.CSV_OrderArray.Add(" + tKeyValue.Key + ", new string[]{\"" + string.Join("\", \"", tKeyValue.Value) + "\"});");
            //        }
            //    }
            //}
            rReturn.AppendLine("#if UNITY_EDITOR");
            //rReturn.AppendLine("tDatas.SQL_OrderArray.Clear();");
            //foreach (KeyValuePair<int, string[]> tKeyValue in SQL_OrderArray.OrderBy(x => x.Key))
            //{
            //    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
            //    {
            //        if (tApp.WSList[tKeyValue.Key] == true)
            //        {
            //            rReturn.AppendLine("tDatas.SQL_OrderArray.Add(" + tKeyValue.Key + ", new string[]{\"" + string.Join("\", \"", tKeyValue.Value) + "\"});");
            //        }
            //    }
            //}

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

            //rReturn.AppendLine("tDatas.SLQ_IntegrityOrder.Clear();");
            //foreach (KeyValuePair<int, List<string>> tKeyValue in SLQ_IntegrityOrder.OrderBy(x => x.Key))
            //{
            //    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
            //    {
            //        if (tApp.WSList[tKeyValue.Key] == true)
            //        {
            //            rReturn.AppendLine("tDatas.SLQ_IntegrityOrder.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
            //        }
            //    }
            //}

            //rReturn.AppendLine("tDatas.SLQ_IntegrityServerOrder.Clear();");
            //foreach (KeyValuePair<int, List<string>> tKeyValue in SLQ_IntegrityServerOrder.OrderBy(x => x.Key))
            //{
            //    if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
            //    {
            //        if (tApp.WSList[tKeyValue.Key] == true)
            //        {
            //            rReturn.AppendLine("tDatas.SLQ_IntegrityServerOrder.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
            //        }
            //    }
            //}

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