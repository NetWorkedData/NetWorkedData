//=====================================================================================================================
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
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
//using BasicToolBox;
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
            //NWEBenchmark.St art();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine(ClassNamePHP + NWD.K_LOADER + "();");
            //NWEBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreationCSHARP()
        {
            //NWEBenchmark.Start();
            NWDAppConfiguration tApp = NWDAppConfiguration.SharedInstance();
            // Write data ...
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("public void " + ClassNamePHP + NWD.K_LOADER + "()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("" + typeof(NWDBasisHelper).Name + " tBasisHelper = null;");
            //rReturn.AppendLine("// = NWDBasisHelper.FindTypeInfos(typeof(" + ClassNamePHP + "));");
            rReturn.AppendLine("tBasisHelper = " + typeof(NWDBasisHelper).Name + ".FindTypeInfos(\"" + ClassNamePHP + "\");");
            rReturn.AppendLine("if (tBasisHelper!=null)");
            rReturn.AppendLine("{");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltStart) + " = \"" + SaltStart.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltEnd) + " = \"" + SaltEnd.Replace("}", "").Replace("{", "") + "\";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.SaltValid) + " = true;"); // salt was reccord because loaded :-p
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.LastWebBuild) + " = " + LastWebBuild + ";");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelPropertiesOrder) + ".Clear();");
            foreach (KeyValuePair<int, List<string>> tKeyValue in WebModelPropertiesOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelPropertiesOrder) + ".Add(" + tKeyValue.Key + ", new List<string>(){\"" + string.Join("\", \"", tKeyValue.Value.ToArray()) + "\"});");
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Clear();");
            foreach (KeyValuePair<int, int> tKeyValue in WebServiceWebModel.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebServiceWebModel) + ".Add(" + tKeyValue.Key + ", " + tKeyValue.Value + ");");
                    }
                }
            }
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelSQLOrder) + ".Clear();");
            foreach (KeyValuePair<int, string> tKeyValue in WebModelSQLOrder.OrderBy(x => x.Key))
            {
                if (tApp.WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (tApp.WSList[tKeyValue.Key] == true)
                    {
                        rReturn.AppendLine("tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelSQLOrder) + ".Add(" + tKeyValue.Key + ", \"" + tKeyValue.Value.Replace("\"", "\\\"") + "\");");
                    }
                }
            }
            rReturn.AppendLine("tBasisHelper.ModelAnalyze();");
            rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelChanged) + " = " + ClassNamePHP + ".ModelChanged();");
            rReturn.AppendLine("//tBasisHelper." + NWDToolbox.PropertyName(() => this.WebModelDegraded) + " = " + ClassNamePHP + ".ModelDegraded();");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("}");
            rReturn.AppendLine("}");
            //NWEBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif