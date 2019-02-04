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
        /// Generates the C sharp file for all environments' restauration.
        /// </summary>
        /// <param name="sEnvironment">S environment.</param>
        public void GenerateCSharpFile(NWDAppEnvironment sEnvironment)
        {
            BTBBenchmark.Start();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);

            StringBuilder rReturn = new StringBuilder(string.Empty);
            //rReturn.AppendLine("//NWDAppConfiguration");
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
            rReturn.AppendLine("public partial class NWDAppConfiguration");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("public override bool RestaureConfigurations ()");
            rReturn.AppendLine("{");
            rReturn.AppendLine("WebFolder = \"" + WebFolder + "\";");
            rReturn.AppendLine("WebBuild = " + WebBuild + ";");
            rReturn.AppendLine("WebBuildMax = " + WebBuildMax + ";");
            rReturn.AppendLine("DatabasePrefix = \"" + DatabasePrefix + "\";");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("EditorPass = \"" + NWDToolbox.SaltCleaner(EditorPass) + "\";");
            rReturn.AppendLine("EditorPassA = \"" + NWDToolbox.SaltCleaner(EditorPassA) + "\";");
            rReturn.AppendLine("EditorPassB = \"" + NWDToolbox.SaltCleaner(EditorPassB) + "\";");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("AccountHashSalt = \"" + NWDToolbox.SaltCleaner(AccountHashSalt) + "\";");
            rReturn.AppendLine("AccountHashSaltA = \"" + NWDToolbox.SaltCleaner(AccountHashSaltA) + "\";");
            rReturn.AppendLine("AccountHashSaltB = \"" + NWDToolbox.SaltCleaner(AccountHashSaltB) + "\";");
            rReturn.AppendLine("RowDataIntegrity = " + RowDataIntegrity.ToString().ToLower() + ";");
            rReturn.AppendLine("PreloadDatas = " + PreloadDatas.ToString().ToLower() + ";");
            rReturn.AppendLine("ProjetcLanguage = \"" + ProjetcLanguage + "\";");
            foreach (KeyValuePair<string, string> tEntry in BundleName.OrderBy(x => x.Key))
            {
                rReturn.AppendLine("BundleName[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";");
            }
            rReturn.AppendLine("WSList = new Dictionary<int, bool>();");
            foreach (KeyValuePair<int, bool> tWS in WSList.OrderBy(x => x.Key))
            {
                if (tWS.Value == true)
                {
                    rReturn.AppendLine("WSList.Add(" + tWS.Key + "," + tWS.Value.ToString().ToLower() + ");");
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
            rReturn.AppendLine("TagList = new Dictionary<int, string>();");
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumberUser; tI++)
            {
                if (TagList.ContainsKey(tI) == true)
                {
                    rReturn.AppendLine("TagList.Add(" + tI + ",\"" + TagList[tI] + "\");");
                }
            }
            int tTag = TagNumberUser + 1;
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Internal Created\");"); // 11
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"(Reserved)\");"); // 12
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"(Reserved)\");"); // 13
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"(Reserved)\");"); // 14
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Test for Preprod\");"); // 15 
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Test for Dev\");"); // 16
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Admin Created\");"); // 17
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Device Created\");"); // 18
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"Server Created\");"); // 19
            rReturn.AppendLine("TagList.Add(" + (tTag++) + ",\"User Created\");"); // 20
            rReturn.AppendLine("//Environments restaure\n");
            rReturn.Append(DevEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(PreprodEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.Append(ProdEnvironment.CreateAppConfigurationCsharp(sEnvironment));
            rReturn.AppendLine("DataLocalizationManager.LanguagesString = \"" + this.DataLocalizationManager.LanguagesString + "\";");
            rReturn.AppendLine("return true;");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("public override bool RestaureTypesConfigurations()");
            rReturn.AppendLine("{");
            foreach (Type tType in NWDTypeLauncher.AllTypes)
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
            foreach (Type tType in NWDTypeLauncher.AllTypes)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                if (tDatas != null)
                {
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
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif