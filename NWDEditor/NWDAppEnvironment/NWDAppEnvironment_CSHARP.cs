//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use <paramref name="sSelectedEnvironment"/> to define environment
        /// </summary>
        /// <param name="sSelectedEnvironment">sSelectedEnvironment </param>
        /// <returns></returns>
        public string CreateAppConfigurationCsharp(NWDAppEnvironment sSelectedEnvironment)
        {
            NWDBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            string tPropertyName = "null";

           if (NWDAppConfiguration.SharedInstance().DevEnvironment == this)
            {
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().DevEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
            }
            if (NWDAppConfiguration.SharedInstance().PreprodEnvironment == this)
            {
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
            }
            if (NWDAppConfiguration.SharedInstance().ProdEnvironment == this)
            {
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().ProdEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
            }

            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = false;");

            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Environment) + " = \"" + Environment.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.DataSHAPassword) + " = \"" + NWDToolbox.SaltCleaner(DataSHAPassword) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.DataSHAVector) + " = \"" + NWDToolbox.SaltCleaner(DataSHAVector) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltStart) + " = \"" + NWDToolbox.SaltCleaner(SaltStart) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltEnd) + " = \"" + NWDToolbox.SaltCleaner(SaltEnd) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WebTimeOut) + " = " + WebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.EditorWebTimeOut) + " = " + EditorWebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltFrequency) + " = " + SaltFrequency.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AddressPing) + " = \"" + AddressPing.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AlwaysSecureData) + " = " + AlwaysSecureData.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.BuildDate) + " = \"" + BuildDate.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AppName) + " = \"" + AppName.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.PreProdTimeFormat) + " = \"" + PreProdTimeFormat.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AppProtocol) + " = \"" + AppProtocol.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SpeedOfGameTime) + " = " + SpeedOfGameTime.ToString() + "F;");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.BuildTimestamp) + " = " + BuildTimestamp.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ThreadPoolForce) + " = " + ThreadPoolForce.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeLocal) + " = " + typeof(NWDWritingMode).Name + "." + WritingModeLocal.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeWebService) + " = " + typeof(NWDWritingMode).Name + "." + WritingModeWebService.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeEditor) + " = " + typeof(NWDWritingMode).Name + "." + WritingModeEditor.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.CartridgeColor) + " = new Color(" + NWDToolbox.FloatToString(CartridgeColor.r) + "F," +  NWDToolbox.FloatToString(CartridgeColor.g) + "F," + NWDToolbox.FloatToString(CartridgeColor.b) + "F," + NWDToolbox.FloatToString(CartridgeColor.a) + "F);");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.LogMode) + " = " + typeof(NWDEnvironmentLogMode).Name + "." + LogMode.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.LoadBalancingLimit) + " = " + LoadBalancingLimit.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RuntimeDefineDictionary) + ".Clear();");
            foreach (KeyValuePair<long, string> tKeyValue in RuntimeDefineDictionary)
            {
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RuntimeDefineDictionary) + ".Add(" + tKeyValue.Key + ", \"" + tKeyValue.Value.Replace("\"", "\\\"") + "\");");
            }
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.EditorDefineDictionary) + ".Clear();");
            foreach (KeyValuePair<long, string> tKeyValue in EditorDefineDictionary)
            {
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.EditorDefineDictionary) + ".Add("+ tKeyValue.Key+", \""+ tKeyValue.Value.Replace("\"","\\\"")+ "\");");
            }
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanMaxTentative) + " = " + IPBanMaxTentative.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanTimer) + " = " + IPBanTimer.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanActive) + " = " + IPBanActive.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerLanguage) + " = " + ServerLanguage.GetType().Name + "." + ServerLanguage.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RescueDelay) + " = " + RescueDelay.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RescueLoginLength) + " = " + RescueLoginLength.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RescuePasswordLength) + " = " + RescuePasswordLength.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.TokenHistoric) + " = " + TokenHistoric.ToString() + ";");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine(tPropertyName + ".FormatVerification ();");
            rReturn.AppendLine("#endif");
            NWDBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
