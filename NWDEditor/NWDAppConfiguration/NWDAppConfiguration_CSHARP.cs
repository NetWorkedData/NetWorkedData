//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:14
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppConfiguration
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate automatically the C# file of configurations 
        /// </summary>
        /// <param name="sEnvironment"></param>
        public void GenerateCSharpFile(NWDAppEnvironment sEnvironment)
        {
            //BTBBenchmark.Start();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);

            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("using System.Collections;");
            rReturn.AppendLine("using System.Collections.Generic;");
            rReturn.AppendLine("using UnityEngine;");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("namespace NetWorkedData");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("public partial class NWDAppConfiguration : NWDApp");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Restaure the configurations ");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("public override bool RestaureConfigurations ()");
            rReturn.AppendLine("{");

            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebFolder) + " = \"" + WebFolder + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebBuild) + " = " + WebBuild + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WebBuildMax) + " = " + WebBuildMax + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.DatabasePrefix) + " = \"" + DatabasePrefix + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPass) + " = \"" + NWDToolbox.SaltCleaner(EditorPass) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPassA) + " = \"" + NWDToolbox.SaltCleaner(EditorPassA) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorPassB) + " = \"" + NWDToolbox.SaltCleaner(EditorPassB) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSalt) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSalt) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSaltA) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSaltA) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AccountHashSaltB) + " = \"" + NWDToolbox.SaltCleaner(AccountHashSaltB) + "\";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.RowDataIntegrity) + " = " + RowDataIntegrity.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PreloadDatas) + " = " + PreloadDatas.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.EditorTableCommun) + " = " + EditorTableCommun.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.ProjetcLanguage) + " = \"" + ProjetcLanguage + "\";");
            foreach (KeyValuePair<string, string> tEntry in BundleName.OrderBy(x => x.Key))
            {
                rReturn.AppendLine(NWDToolbox.PropertyName(() => this.BundleName) + "[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";");
            }
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WSList) + " = new Dictionary<int, bool>();");
            foreach (KeyValuePair<int, bool> tWS in WSList.OrderBy(x => x.Key))
            {
                if (tWS.Value == true)
                {
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => this.WSList) + ".Add(" + tWS.Key + "," + tWS.Value.ToString().ToLower() + ");");
                }
                else
                {
                    string tWebServiceFolder = NWDAppConfiguration.SharedInstance().OldWebServiceFolder(tWS.Key);
                    string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                    if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == true)
                    {
                        AssetDatabase.DeleteAsset(tOwnerServerFolderPath + "/" + tWebServiceFolder);
                    }
                }
            }
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + " = new Dictionary<int, string>();");
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumberUser; tI++)
            {
                if (TagList.ContainsKey(tI) == true)
                {
                    rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + tI + ",\"" + TagList[tI] + "\");");
                }
            }
            int tTag = TagNumberUser + 1;
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Internal Created\");"); // 11
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 12
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 13
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"(Reserved)\");"); // 14
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Test for Preprod\");"); // 15 
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Test for Dev\");"); // 16
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Admin Created\");"); // 17
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Device Created\");"); // 18
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"Server Created\");"); // 19
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"User Created\");"); // 20

            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.TagList) + ".Add(" + (tTag++) + ",\"TO DELETE\");"); // 21
            rReturn.AppendLine("//Environments restaure\n");
            rReturn.Append(DevEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(PreprodEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(ProdEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.DataLocalizationManager) + "." + NWDToolbox.PropertyName(() => this.DataLocalizationManager.LanguagesString) + " = \"" + this.DataLocalizationManager.LanguagesString + "\";");
            //rReturn.AppendLine("AnonymousPlayerIsLocal = " + AnonymousPlayerIsLocal.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.AnonymousDeviceConnected) + " = " + AnonymousDeviceConnected.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.SurProtected) + " = " + SurProtected.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PinCodeLenghtMin) + " = " + PinCodeLenghtMin.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.PinCodeLenghtMax) + " = " + PinCodeLenghtMax.ToString().ToLower() + ";");
            rReturn.AppendLine(NWDToolbox.PropertyName(() => this.ProtectionTentativeMax) + " = " + ProtectionTentativeMax.ToString() + ";");
            rReturn.AppendLine("return true;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Restaure the configurations Types by Types");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("public override bool RestaureTypesConfigurations()");
            rReturn.AppendLine("{");
            List<Type> tAllTypes = new List<Type>(NWDTypeLauncher.AllTypes);
            tAllTypes.Sort((tA, tB) => string.Compare(tA.Name, tB.Name, StringComparison.Ordinal));
            foreach (Type tType in tAllTypes)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                if (tDatas != null)
                {
                    rReturn.Append(tDatas.CreationCSHARPCallLoader());
                }
            }
            rReturn.AppendLine("return true;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            foreach (Type tType in tAllTypes)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                if (tDatas != null)
                {
                    rReturn.AppendLine("/// <summary>");
                    rReturn.AppendLine("/// Restaure the configurations for " + tDatas.ClassNamePHP + "");
                    rReturn.AppendLine("/// </summary>");
                    rReturn.AppendLine(tDatas.CreationCSHARP());
                    rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
                }
            }
            rReturn.AppendLine("}");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//=====================================================================================================================");
            string tOwnerConfigurationFolderPath = NWDToolbox.FindOwnerConfigurationFolder();
            string tPath = tOwnerConfigurationFolderPath + "/NWDConfigurations.cs";
            string rReturnFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
            File.WriteAllText(tPath, rReturnFormatted);
            AssetDatabase.ImportAsset(tPath);
            AssetDatabase.Refresh();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif