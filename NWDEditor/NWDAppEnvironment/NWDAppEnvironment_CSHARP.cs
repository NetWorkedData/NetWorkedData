﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:19
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().DevEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = true;");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Dev) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = false;");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            if (NWDAppConfiguration.SharedInstance().PreprodEnvironment == this)
            {
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = true;");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Preprod) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = false;");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            if (NWDAppConfiguration.SharedInstance().ProdEnvironment == this)
            {
                tPropertyName = NWDToolbox.PropertyName(() => NWDAppConfiguration.SharedInstance().ProdEnvironment);
                rReturn.AppendLine("//" + tPropertyName);
                if (sSelectedEnvironment == this)
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = true; ");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Prod) + "\";");
                }
                else
                {
                    rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Selected) + " = false; ");
                    //rReturn.AppendLine(tPropertyName + ".AccountsForTests = \"\";");
                }
            }
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.Environment) + " = \"" + Environment.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.DataSHAPassword) + " = \"" + NWDToolbox.SaltCleaner(DataSHAPassword) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.DataSHAVector) + " = \"" + NWDToolbox.SaltCleaner(DataSHAVector) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltStart) + " = \"" + NWDToolbox.SaltCleaner(SaltStart) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltEnd) + " = \"" + NWDToolbox.SaltCleaner(SaltEnd) + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WebTimeOut) + " = " + WebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.EditorWebTimeOut) + " = " + EditorWebTimeOut.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltFrequency) + " = " + SaltFrequency.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AddressPing) + " = \"" + AddressPing.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerHTTPS) + " = \"" + ServerHTTPS.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AllwaysSecureData) + " = " + AllwaysSecureData.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.BuildDate) + " = \"" + BuildDate.Replace("\"", "\\\"") + "\";");
            //rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.FacebookAppID) + " = \"" + FacebookAppID.Replace("\"", "\\\"") + "\";");
            //rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.FacebookAppSecret) + " = \"" + FacebookAppSecret.Replace("\"", "\\\"") + "\";");
            //rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.GoogleAppKey) + " = \"" + GoogleAppKey.Replace("\"", "\\\"") + "\";");
            //rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.UnityAppKey) + " = \"" + UnityAppKey.Replace("\"", "\\\"") + "\";");
            //rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.TwitterAppKey) + " = \"" + TwitterAppKey.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AppName) + " = \"" + AppName.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.PreProdTimeFormat) + " = \"" + PreProdTimeFormat.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AppProtocol) + " = \"" + AppProtocol.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SpeedOfGameTime) + " = " + SpeedOfGameTime.ToString() + "F;");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.BuildTimestamp) + " = " + BuildTimestamp.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ThreadPoolForce) + " = " + ThreadPoolForce.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeLocal) + " = "+typeof(NWDWritingMode).Name + "." + WritingModeLocal.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeWebService) + " = "+typeof(NWDWritingMode).Name + "." + WritingModeWebService.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.WritingModeEditor) + " = "+typeof(NWDWritingMode).Name + "." + WritingModeEditor.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.CartridgeColor) + " = new Color(" + NWDToolbox.FloatToString(CartridgeColor.r) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.g) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.b) + "F," +
                                                                NWDToolbox.FloatToString(CartridgeColor.a) + "F);");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.LogMode) + " = " + LogMode.ToString().ToLower() + ";");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanMaxTentative) + " = " + IPBanMaxTentative.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanTimer) + " = " + IPBanTimer.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.IPBanActive) + " = " + IPBanActive.ToString().ToLower() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SFTPHost) + " = \"" + SFTPHost.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SFTPPort) + " = " + SFTPPort.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SFTPFolder) + " = \"" + SFTPFolder.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SFTPUser) + " = \"" + SFTPUser.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SFTPPassword) + " = \"" + SFTPPassword.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.SaltServer) + " = \"" + SaltServer.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerHost) + " = \"" + ServerHost.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerUser) + " = \"" + ServerUser.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerPassword) + " = \"" + ServerPassword.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.ServerBase) + " = \"" + ServerBase.Replace("\"", "\\\"") + "\";");
            if (AdminInPlayer == false)
            {
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminKey) + " = \"" + AdminKey.Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminKeyHash) + " = \"" + AdminKeyHashGenerate().Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminInPlayer) + " = " + AdminInPlayer.ToString().ToLower() + ";");
            }
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.RescueEmail) + " = \"" + RescueEmail.Replace("\"", "\\\"").Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.TokenHistoric) + " = " + TokenHistoric.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailHost) + " =  \"" + MailHost.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailPort) + " = " + MailPort.ToString() + ";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailUserName) + " =  \"" + MailUserName.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailPassword) + " =  \"" + MailPassword.Replace("\"", "\\\"") + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailDomain) + " =  \"" + MailDomain.Trim() + " \";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailAuthentication) + " =  \"" + MailAuthentication.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailEnableStarttlsAuto) + " =  \"" + MailEnableStarttlsAuto.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailOpenSSLVerifyMode) + " =  \"" + MailOpenSSLVerifyMode.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailOpenSSLVerifyMode) + " =  \"" + MailOpenSSLVerifyMode.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailFrom) + " =  \"" + MailFrom.Trim() + "\";");
            rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.MailReplyTo) + " =  \"" + MailReplyTo.Trim() + "\";");
            rReturn.AppendLine("#endif");
            if (AdminInPlayer == true)
            {
                // write bypass in player mode
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminKey) + " = \"" + AdminKey.Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminKeyHash) + " = \"" + AdminKeyHashGenerate().Replace("\"", "\\\"") + "\";");
                rReturn.AppendLine(tPropertyName + "." + NWDToolbox.PropertyName(() => this.AdminInPlayer) + " = " + AdminInPlayer.ToString().ToLower() + ";");
            }
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
