// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using BasicToolBox;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARPCallLoader()
        {
            //BTBBenchmark.St art();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine(ClassNamePHP + NWD.K_LOADER + "();");
            //BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARP()
        {
            //BTBBenchmark.Start();
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            // Write data ...
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("NWDBasisHelper tBasisHelper = null;");
            rReturn.AppendLine("// = NWDBasisHelper.FindTypeInfos(typeof(" + ClassNamePHP + "));");
            rReturn.AppendLine("tBasisHelper = NWDBasisHelper.FindTypeInfos(\""+ ClassNamePHP + "\");");
            rReturn.AppendLine("if (tBasisHelper!=null)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("tBasisHelper.SaltStart = \"" + SaltStart.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper.SaltEnd = \"" + SaltEnd.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper.SaltValid = true;"); // salt was reccord because loaded :-p
            rReturn.AppendLine("tBasisHelper.LastWebBuild = " + LastWebBuild + ";");
            rReturn.AppendLine("tBasisHelper.WebModelPropertiesOrder.Clear();");
            foreach (KeyValuePair<int, List<string>> tKeyValue in WebModelPropertiesOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper.WebModelPropertiesOrder.Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper.WebServiceWebModel.Clear();");
            foreach (KeyValuePair<int, int> tKeyValue in WebServiceWebModel.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper.WebServiceWebModel.Add(" + tKeyValue.Key + ", " + tKeyValue.Value + ");");
                    }
                }
            }
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("tBasisHelper.WebModelSQLOrder.Clear();");
            foreach (KeyValuePair<int, string> tKeyValue in WebModelSQLOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper.WebModelSQLOrder.Add(" + tKeyValue.Key + ", \"" + tKeyValue.Value.Replace("\"", "\\\"") + "\");");
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper.ModelAnalyze();");
            rReturn.AppendLine("//tBasisHelper.WebModelChanged = " + ClassNamePHP + ".ModelChanged();");
            rReturn.AppendLine("//tBasisHelper.WebModelDegraded = " + ClassNamePHP + ".ModelDegraded();");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            //BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif