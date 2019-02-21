//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Text;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public string CreateAppConfigurationCsharp(NWDAppEnvironment sSelectedEnvironment)
        {
            //BTBBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            string tPropertyName = "null";
            if (NWDAppConfiguration.SharedInstance().DevEnvironment == this)
            {
                tPropertyName = NWDAlias.FindAliasName(typeof(NWDAppConfiguration), NWD.K_DevEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = true;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Dev) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = false;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            if (NWDAppConfiguration.SharedInstance().PreprodEnvironment == this)
            {
                tPropertyName = NWDAlias.FindAliasName(typeof(NWDAppConfiguration), NWD.K_PreprodEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = true;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Preprod) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = false;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            if (NWDAppConfiguration.SharedInstance().ProdEnvironment == this)
            {
                tPropertyName = NWDAlias.FindAliasName(typeof(NWDAppConfiguration), NWD.K_ProdEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = true;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Prod) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + ".Selected = false;");
                    rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            rReturn.AppendLine(tPropertyName + ".Environment = \"" + Environment.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".DataSHAPassword = \"" + NWDToolbox.SaltCleaner(DataSHAPassword) + "\";");
            rReturn.AppendLine(tPropertyName + ".DataSHAVector = \"" + NWDToolbox.SaltCleaner(DataSHAVector) + "\";");
            rReturn.AppendLine(tPropertyName + ".SaltStart = \"" + NWDToolbox.SaltCleaner(SaltStart) + "\";");
            rReturn.AppendLine(tPropertyName + ".SaltEnd = \"" + NWDToolbox.SaltCleaner(SaltEnd) + "\";");
            rReturn.AppendLine(tPropertyName + ".WebTimeOut = " + WebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".EditorWebTimeOut = " + EditorWebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".SaltFrequency = " + SaltFrequency.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".AddressPing = \"" + AddressPing.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".ServerHTTPS = \"" + ServerHTTPS.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".AllwaysSecureData = " + AllwaysSecureData.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + ".BuildDate = \"" + BuildDate.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".FacebookAppID = \"" + FacebookAppID.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".FacebookAppSecret = \"" + FacebookAppSecret.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".GoogleAppKey = \"" + GoogleAppKey.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".UnityAppKey = \"" + UnityAppKey.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".TwitterAppKey = \"" + TwitterAppKey.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".AppName = \"" + AppName.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".PreProdTimeFormat = \"" + PreProdTimeFormat.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".AppProtocol = \"" + AppProtocol.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".SpeedOfGameTime = " + SpeedOfGameTime.ToString() + "F;");
            rReturn.AppendLine(tPropertyName + ".BuildTimestamp = " + BuildTimestamp.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".ThreadPoolForce = " + ThreadPoolForce.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + ".WritingModeLocal = NWDWritingMode." + WritingModeLocal.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".WritingModeWebService = NWDWritingMode." + WritingModeWebService.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".WritingModeEditor = NWDWritingMode." + WritingModeEditor.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".CartridgeColor = new Color(" + NWDToolbox.FloatToString(CartridgeColor.r) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.g) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.b) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.a) + "F);");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine(tPropertyName + ".LogMode = " + LogMode.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + ".SFTPHost = \"" + SFTPHost.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".SFTPPort = " + SFTPPort.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".SFTPFolder = \"" + SFTPFolder.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".SFTPUser = \"" + SFTPUser.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".SFTPPassword = \"" + SFTPPassword.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".SaltServer = \"" + SaltServer.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".ServerHost = \"" + ServerHost.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".ServerUser = \"" + ServerUser.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".ServerPassword = \"" + ServerPassword.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".ServerBase = \"" + ServerBase.Replace("\"", "\\\"") + "\";");
            if (AdminInPlayer == false)
            {
                rReturn.AppendLine(tPropertyName + ".AdminKey = \"" + AdminKey.Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + ".AdminKeyHash = \"" + AdminKeyHashGenerate().Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + ".AdminInPlayer = " + AdminInPlayer.ToString().ToLower() + ";");
            }
            rReturn.AppendLine(tPropertyName + ".RescueEmail = \"" + RescueEmail.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + ".TokenHistoric = " + TokenHistoric.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".MailHost =  \"" + MailHost + " \";");
            rReturn.AppendLine(tPropertyName + ".MailPort = " + MailPort.ToString() + ";");
            rReturn.AppendLine(tPropertyName + ".MailUserName =  \"" + MailUserName + " \";");
            rReturn.AppendLine(tPropertyName + ".MailPassword =  \"" + MailPassword.Replace("\"", "\\\"") + " \";");
            rReturn.AppendLine(tPropertyName + ".MailDomain =  \"" + MailDomain + " \";");
            rReturn.AppendLine(tPropertyName + ".MailAuthentication =  \"" + MailAuthentication + " \";");
            rReturn.AppendLine(tPropertyName + ".MailEnableStarttlsAuto =  \"" + MailEnableStarttlsAuto + " \";");
            rReturn.AppendLine(tPropertyName + ".MailOpenSSLVerifyMode =  \"" + MailOpenSSLVerifyMode + " \";");
            rReturn.AppendLine(tPropertyName + ".MailOpenSSLVerifyMode =  \"" + MailOpenSSLVerifyMode + " \";");
            rReturn.AppendLine(tPropertyName + ".MailFrom =  \"" + MailFrom + " \";");
            rReturn.AppendLine(tPropertyName + ".MailReplyTo =  \"" + MailReplyTo + " \";");
            rReturn.AppendLine("#endif");
            if (AdminInPlayer == true)
            {
                // write bypass in player mode
                rReturn.AppendLine(tPropertyName + ".AdminKey = \"" + AdminKey.Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + ".AdminKeyHash = \"" + AdminKeyHashGenerate().Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + ".AdminInPlayer = " + AdminInPlayer.ToString().ToLower() + ";");
            }
            rReturn.AppendLine(tPropertyName + ".LoadPreferences ();");
            rReturn.AppendLine(tPropertyName + ".FormatVerification ();");
            //BTBBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
