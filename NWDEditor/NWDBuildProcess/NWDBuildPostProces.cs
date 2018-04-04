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

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBuildPostProcess : IPostprocessBuild {
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnPostprocessBuild(BuildTarget target, string path)
		{
			Debug.Log ("NWDBuildPostProcess OnPostprocessBuild for target " + target + " at path " + path);
			BuildTarget tBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			switch (tBuildTarget) {
                case BuildTarget.StandaloneOSX: 
				{
                        #if UNITY_IOS
                        // TODO : change project localization 
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
                        // Change value of CFBundleDisplayName in Xcode plist
                        var tBundleDisplayNameKey = "CFBundleDisplayName";
                        string tBundleName = NWDAppConfiguration.SharedInstance().BundleName[tProjetcLanguage];
                        tBundleName = Application.productName;
                        rootDict.SetString(tBundleDisplayNameKey, tBundleName);
                        // Write to file
                        File.WriteAllText(plistPath, plist.WriteToString());
                        #endif
				}
				break;
			case BuildTarget.StandaloneWindows: 
				{
				}
				break;
			case BuildTarget.iOS: 
				{
                        #if UNITY_IOS
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
                        // Write to file
                        File.WriteAllText(plistPath, plist.WriteToString());

                        /*
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
                                    string tBundleName = NWDAppConfiguration.SharedInstance().BundleName[tKeyValue.Value];
                                    if (string.IsNullOrEmpty(tBundleName) == false)
                                    {

                                        if (Directory.Exists(path + "/" + tV + ".lproj") == false)
                                        {
                                            Directory.CreateDirectory(path + "/" + tV + ".lproj");
                                        }
                                        File.WriteAllText(path + "/" + tV + ".lproj/InfoPlist.strings", "\n\"CFBundleDisplayName\" = \"" + tBundleName + "\";\n");
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
			case BuildTarget.Tizen: 
				{
				}
				break;
			case BuildTarget.PSP2: 
				{
				}
				break;
			case BuildTarget.PS4: 
				{
				}
				break;
			case BuildTarget.XboxOne: 
				{
				}
				break;
			case BuildTarget.N3DS: 
				{
				}
				break;
			case BuildTarget.WiiU: 
				{
				}
				break;
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