//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine;
using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if UNITY_IOS
//using UnityEditor.iOS.Xcode;
#endif
#if UNITY_STANDALONE_OSX
//using UnityEditor.iOS.Xcode;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBuildPostProcess : IPostprocessBuildWithReport
    {
        //-------------------------------------------------------------------------------------------------------------
        public int callbackOrder
        {
            get
            {
                return 0;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPostprocessBuild(BuildReport report)
        {
            //Debug.Log ("NWDBuildPostProcess OnPostprocessBuild for target " + target + " at path " + path);
            BuildTarget tBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            switch (tBuildTarget)
            {
                case BuildTarget.StandaloneOSX:
                    {
#if UNITY_STANDALONE_OSX
                       /* // TODO : change project localization 
                        string tProjetcLanguage = NWDAppConfiguration.SharedInstance().ProjetcLanguage;
                        // Get plist
                        string plistPath = path + "/Contents/Info.plist";
                        PlistDocument plist = new PlistDocument();
                        plist.ReadFromString(File.ReadAllText(plistPath));
                        // Get root
                        PlistElementDict rootDict = plist.root;
                        // Change value of CFBundleVersion in Xcode plist
                        var buildKey = "CFBundleDevelopmentRegion";
                        rootDict.SetString(buildKey, NWDAppConfiguration.SharedInstance().DataLocalizationManager.ISOValue(tProjetcLanguage));
                        Debug.LogWarning("plist > " + buildKey + "  = " + NWDAppConfiguration.SharedInstance().DataLocalizationManager.ISOValue(tProjetcLanguage));
                        // Change value of CFBundleDisplayName in Xcode plist
                        var tBundleDisplayNameKey = "CFBundleDisplayName";
                        string tBundleName = null;
                        if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tProjetcLanguage))
                        {
                            tBundleName =  NWDAppConfiguration.SharedInstance().BundleName[tProjetcLanguage];
                        }
                        if (string.IsNullOrEmpty(tBundleName))
                        {
                            tBundleName = Application.productName;
                        }
                        rootDict.SetString(tBundleDisplayNameKey, tBundleName);
                        Debug.LogWarning("plist > " + tBundleDisplayNameKey + "  = " + tBundleName);
                        // Add protocol to plist
                        if (string.IsNullOrEmpty(NWDAppEnvironment.SelectedEnvironment().AppProtocol) == false)
                        {
                            PlistElementArray CFBundleURLTypes = rootDict.CreateArray("CFBundleURLTypes");
                            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                // ForEach Protocol
                                PlistElementDict tURLScheme = CFBundleURLTypes.AddDict();
                                tURLScheme.SetString("CFBundleTypeRole", "Editor");
                                tURLScheme.SetString("CFBundleURLName", tProtocol.Replace("://",""));
                                PlistElementArray CFBundleURLSchemes = tURLScheme.CreateArray("CFBundleURLSchemes");
                                CFBundleURLSchemes.AddString(tProtocol.Replace("://",""));
                            }
                        }
                        Debug.LogWarning("plistPath = " + plistPath +  " finish to write");
                        // Write to file
                        File.WriteAllText(plistPath, plist.WriteToString());



                        // Hum NOT WORKING?

                        Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
                        foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
                        {
                            bool tContains = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Contains(tKeyValue.Value);
                            if (tContains == true)
                            {
                                //string tV = NWDAppConfiguration.SharedInstance().DataLocalizationManager.ISOValue(tKeyValue.Value);
                                string tV = tKeyValue.Value;
                                if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tKeyValue.Value))
                                {
                                    // todo create the InfoPlist.strings and add to project ?!
                                    string tBundleNameLocalized = NWDAppConfiguration.SharedInstance().BundleName[tKeyValue.Value];
                                    if (string.IsNullOrEmpty(tBundleNameLocalized) == false)
                                    {
                                        if (Directory.Exists(path + "/Contents/" + tV + ".lproj") == false)
                                        {
                                            Directory.CreateDirectory(path + "/Contents/" + tV + ".lproj");
                                        }
                                        File.WriteAllText(path + "/Contents/" + tV + ".lproj/InfoPlist.strings", "\n\"CFBundleDisplayName\" = \"" + tBundleNameLocalized + "\";\n");
                                    }
                                }
                            }
                        }
                        */
#endif
                    }
                    break;
                case BuildTarget.iOS:
                    {
#if UNITY_IOS
                        /*
                        // TODO : change project localization 
                        string tProjetcLanguage = NWDAppConfiguration.SharedInstance().ProjetcLanguage;
                        // Get plist
                        string plistPath = path + "/Info.plist";
                        PlistDocument plist = new PlistDocument();
                        plist.ReadFromString(File.ReadAllText(plistPath));
                        // Get root
                        PlistElementDict rootDict = plist.root;
                        // Change value of CFBundleVersion in Xcode plist
                        var buildKey = "CFBundleDevelopmentRegion";
                        rootDict.SetString(buildKey, NWDAppConfiguration.SharedInstance().DataLocalizationManager.ISOValue(tProjetcLanguage));
                        // Change value of CFBundleDisplayName in Xcode plist
                        var tBundleDisplayNameKey = "CFBundleDisplayName";
                        string tBundleName = NWDAppConfiguration.SharedInstance().BundleName[tProjetcLanguage];
                        if (string.IsNullOrEmpty(tBundleName))
                        {
                            tBundleName = Application.productName;
                        }
                        rootDict.SetString(tBundleDisplayNameKey, tBundleName);
                        Debug.LogWarning("plist > " + tBundleDisplayNameKey + "  = " + tBundleName);

                        // Add protocol to plist
                        if (string.IsNullOrEmpty(NWDAppEnvironment.SelectedEnvironment().AppProtocol) == false)
                        {
                            PlistElementArray CFBundleURLTypes = rootDict.CreateArray("CFBundleURLTypes");
                            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                // ForEach Protocol
                                PlistElementDict tURLScheme = CFBundleURLTypes.AddDict();
                                tURLScheme.SetString("CFBundleTypeRole", "Editor");
                                tURLScheme.SetString("CFBundleURLName", tProtocol.Replace("://", ""));
                                PlistElementArray CFBundleURLSchemes = tURLScheme.CreateArray("CFBundleURLSchemes");
                                CFBundleURLSchemes.AddString(tProtocol.Replace("://", ""));
                            }
                        }
                        // Write to file
                        File.WriteAllText(plistPath, plist.WriteToString());

                        



                        // NOT WORKING !!!
                        // get project xcode
                        string tPBXProjectPath = PBXProject.GetPBXProjectPath(path);
                        PBXProject tPBXProject = new PBXProject();
                        tPBXProject.ReadFromString(File.ReadAllText(tPBXProjectPath));

                        // Add localizable string
                        Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
                        foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
                        {
                            bool tContains = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Contains(tKeyValue.Value);
                            if (tContains == true)
                            {
                                //string tV = NWDAppConfiguration.SharedInstance().DataLocalizationManager.ISOValue(tKeyValue.Value);
                                string tV = tKeyValue.Value;
                                if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tKeyValue.Value))
                                {
                                    // todo create the InfoPlist.strings and add to project ?!
                                    string tBundleNameLocalized = NWDAppConfiguration.SharedInstance().BundleName[tKeyValue.Value];
                                    if (string.IsNullOrEmpty(tBundleNameLocalized) == false)
                                    {

                                        if (Directory.Exists(path + "/" + tV + ".lproj") == false)
                                        {
                                            Directory.CreateDirectory(path + "/" + tV + ".lproj");
                                        }
                                        File.WriteAllText(path + "/" + tV + ".lproj/InfoPlist.strings", "\n\"CFBundleDisplayName\" = \"" + tBundleNameLocalized + "\";\n");
                                        tPBXProject.AddFile(path + "/" + tV + ".lproj/InfoPlist.strings", "InfoPlist.strings", PBXSourceTree.Source);
                                    }
                                }
                            }
                        }

                        */
#endif
                    }
                    break;
                case BuildTarget.Android:
                    {
                    }
                    break;
                case BuildTarget.StandaloneWindows:
                    {
                    }
                    break;
                case BuildTarget.StandaloneLinux:
                    {
                    }
                    break;
                case BuildTarget.StandaloneWindows64:
                    {
                    }
                    break;
                case BuildTarget.WebGL:
                    {
                    }
                    break;
                case BuildTarget.WSAPlayer:
                    {
                    }
                    break;
                case BuildTarget.StandaloneLinux64:
                    {
                    }
                    break;
                case BuildTarget.StandaloneLinuxUniversal:
                    {
                    }
                    break;
                //case BuildTarget.Tizen:
                    //{
                    //}
                    //break;
                //case BuildTarget.PSP2:
                    //{
                    //}
                    //break;
                case BuildTarget.PS4:
                    {
                    }
                    break;
                case BuildTarget.XboxOne:
                    {
                    }
                    break;
                //case BuildTarget.N3DS:
                    //{
                    //}
                    //break;
                //case BuildTarget.WiiU:
                    //{
                    //}
                    //break;
                case BuildTarget.tvOS:
                    {
                    }
                    break;
                case BuildTarget.Switch:
                    {
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif