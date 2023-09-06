using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if UNITY_EDITOR

namespace NetWorkedData.NWDEditor
{
    public class NWDExportToNWDThree
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + NWDEditorMenu.K_TOOLS + "Export to NWD3", false, 201)]
        public static void ExportDataMenu()
        {
            Debug.LogWarning(nameof(ExportDataMenu) + " Start");
            string tExtension = "txt";
            string tPath = EditorUtility.SaveFilePanel(
                "Export Data",
                "~/Desktop/",
                "NWD2ToNWD3",
                tExtension
            );
            if (string.IsNullOrEmpty(tPath) == false)
            {
                StringBuilder tFile = new StringBuilder();
                StringBuilder tClassResume = new StringBuilder();
                Dictionary<string, string> tReferencesTranslation = new Dictionary<string, string>();
                int tLine = 0;

                foreach (Type tType in NWDLauncher.AllNetWorkedDataTypes)
                {
                    if (tType != typeof(NWDVersion) && tType != typeof(NWDError))
                    {
                        NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                        if (tHelper.TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent && tHelper.TemplateHelper.GetSynchronizable() == NWDTemplateClusterDatabase.SynchronizableInClusterAllDatabase)
                        {
                            int tSubLine = 0;
                            tFile.AppendLine("# export " + tHelper.ClassTableName);
                            foreach (KeyValuePair<string, NWDTypeClass> tKeyValue in tHelper.DatasByReference)
                            {
                                foreach (NWDExportObject tExport in tKeyValue.Value.ExportNWD3(51376, 18446744071694964799))
                                {
                                    tReferencesTranslation.Add(tExport.ReferenceOld, tExport.ReferenceNew);
                                    tFile.AppendLine(tExport.JsonObject);
                                }

                                tSubLine++;
                            }
                            tClassResume.AppendLine("# export " + tHelper.ClassTableName + " lines = " + tSubLine);
                        }
                    }
                }

                foreach (NWDExportObject tTranslate in NWDExportObject.NWDStringLocalizationList.Values)
                {
                    tFile.AppendLine(tTranslate.JsonObject);
                }
                foreach (NWDExportObject tAsset in NWDExportObject.NWDAssetDataList.Values)
                {
                    tFile.AppendLine(tAsset.JsonObject);
                }

                string tFileFinal = tFile.ToString();
                File.WriteAllText(tPath, tFileFinal);
                string tClassResumeFinal = tClassResume.ToString();
                File.WriteAllText(tPath.Replace("." + tExtension, "_ClassResume.txt"), tClassResumeFinal);
            }

            Debug.LogWarning(nameof(ExportDataMenu) + " Finish");
        }
    }
}

#endif