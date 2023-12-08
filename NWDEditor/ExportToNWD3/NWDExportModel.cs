using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

#if UNITY_EDITOR

namespace NetWorkedData
{
    public enum AssetType
    {
        None = 0,
        Object = 1,
        Scriptable = 2,
        Scene = 4,
        All = ~0,
    }

    public class NWDExportObject
    {
        public const string kTableName = "Local_Reflex_Prod_NWDMetaData";
        public const long kHubProjectId = 86868;
        public const long kProjectID = 17004892179923615;
        public const string kModelVersion = "0.0.0.0";


        public string ReferenceOld = string.Empty;
        public long ReferenceNew = 0;
        public string JsonObject = string.Empty;

        static public Dictionary<string, long> ReferencesDictionary = new Dictionary<string, long>();
        static public Dictionary<long,NWDExportObject> NWDStringLocalizationList = new Dictionary<long,NWDExportObject>();
        static public Dictionary<long, NWDExportObject> NWDAssetLocalizationList = new Dictionary<long, NWDExportObject>();
        static public Dictionary<long,NWDExportObject> NWDAssetDataList = new Dictionary<long,NWDExportObject>();

        private static long tReferenceUnique = NWDToolbox.NumericCleaner(NWDToolbox.Timestamp() * 10000000000000000);

        public static object ProcessNewLocalizedString(NWDLocalizableType sLocalizedString)
        {
            try
            {
                NWDExportObject tNewData = new NWDExportObject(sLocalizedString);
                long tNewReference = tNewData.ReferenceNew;
                NWDStringLocalizationList.Add(tNewReference, tNewData);

                var tExport = new
                {
                    Reference = tNewReference,
                };
                return tExport;
            }
            catch
            {
                return null;
            }
        }

        public static object ProcessNewLocalizedString(Dictionary<NWDLanguageEnum, object> sValues, string sContext)
        {
            try
            {
                NWDExportObject tNewData = new NWDExportObject(sValues, sContext);
                long tNewReference = tNewData.ReferenceNew;
                NWDStringLocalizationList.Add(tNewReference, tNewData);

                var tExport = new
                {
                    Reference = tNewReference,
                };
                return tExport;
            }
            catch
            {
                return null;
            }
        }

        public static object ProcessNewLocalizedAsset(Dictionary<NWDLanguageEnum, object> sValues, string sContext, string sClassName)
        {
            NWDExportObject tNewData = new NWDExportObject(sValues, sContext, sClassName);
            long tNewReference = tNewData.ReferenceNew;
            NWDAssetLocalizationList.Add(tNewReference, tNewData);

            var tExport = new
            {
                Reference = tNewReference,
            };
            return tExport;
        }

        public static object ProcessNewAsset(NWDLocalizableVideoClipType sValue)
        {
            long tNewReference = GetNewReference();
            NWDExportObject tNewData = new NWDExportObject(sValue, tNewReference);
            NWDAssetDataList.Add(tNewReference, tNewData);

            var tExport = new
            {
                Reference = tNewReference,
            };
            return tExport;
        }

        public static object ProcessNewAsset(NWDAssetType sAssetType)
        {
            if (sAssetType == null)
            {
                return null;
            }
            return ProcessNewAsset(sAssetType.Value, AssetType.Object | AssetType.Scriptable);
        }

        public static object ProcessNewAsset(List<NWDAssetType> sAssetTypes, AssetType sType)
        {
            return ProcessNewAsset(sAssetTypes.Select(x => x?.Value ?? "").ToList(), sType);
        }


        public static object ProcessNewAsset(string sAssetType, AssetType sType)
        {
            try
            {
                NWDExportObject tNewData = new NWDExportObject(sAssetType, sType);
                NWDAssetDataList.Add(tNewData.ReferenceNew, tNewData);

                object rObjects = new { Reference = tNewData.ReferenceNew };
                return rObjects;
            }
            catch
            {
                return null;
            }
        }

        public static object ProcessNewAsset(List<string> sAssetTypes, AssetType sType)
        {
            List<object> rObjects = new List<object>();
            foreach (string k in sAssetTypes)
            {
                try
                {
                    NWDExportObject tNewData = new NWDExportObject(k, sType);
                    NWDAssetDataList.Add(tNewData.ReferenceNew, tNewData);
                    rObjects.Add(tNewData.ReferenceNew);
                }
                catch
                {
                    rObjects.Add(0);
                }
            }
            return rObjects;
        }

        public static object ProcessNewColor(List<NWDColorType> sColors)
        {
            List<object> rObjects = new List<object>();
            foreach(NWDColorType k in sColors)
            {
                Color tColor = k.GetColor();
                var tExport = new {
                    Red = tColor.r,
                    Green = tColor.g,
                    Blue = tColor.b,
                    Alpha = tColor.a,
                };
                rObjects.Add(tExport);
            }
            return rObjects;
        }

        public static object GetIntSequence(int sMin, int sMax, int sBehavior = 0)
        {
            var tExport = new {
                Min = sMin,
                Max = sMax,
                Behavior = sBehavior
            };
            return tExport;
        }

        public static object GetFloatSequence(float sMin, float sMax, int sBehavior = 0)
        {
            var tExport = new {
                Min = sMin,
                Max = sMax,
                Behavior = sBehavior
            };
            return tExport;
        }

        public static object ProcessNewRef(object sOldRef)
        {
            return "";
        }

        /*public static string ProcessNewArray(string sValue)
        {
            string rReferences = "";
            string[] tReferencesList = sValue?.Split(new char[] { NWDConstants.kFieldSeparatorA_char });
            if (tReferencesList != null)
            {
                foreach(string k in tReferencesList)
                {
                    if (rReferences.Length > 0) rReferences += ",";
                    rReferences += k;
                }
            }
            return "[" + rReferences + "]";
        }

        public static string ProcessNewArray(Dictionary<string, int> sValue)
        {
            string rReferences = "";
            if (sValue != null)
            {
                foreach(var k in sValue)
                {
                    if (rReferences.Length > 0) rReferences += ",";
                    string tPair = $"{k.Key}:{k.Value}";
                    rReferences += "{" + tPair + "}";
                }
            }
            return "[" + rReferences + "]";
        }*/

        /*public static string ProcessNewReferences(ulong sProjectHub, ulong sProjectId, params string[] sValues)
        {
            NWDExportObject tReturn = new NWDExportObject(sProjectHub, sProjectId, sValue);
            string tNewReference = tReturn.ReferenceNew;
            return "{\"Reference\":" + tNewReference + ", \"Type\":\"NWDAssetData\"}";
        }*/

        public static string GetNewReference(string sOldReference)
        {
            tReferenceUnique++;
            return tReferenceUnique.ToString();
        }

        public static long GetNewReference()
        {
            tReferenceUnique++;
            return tReferenceUnique;
        }

        public static string ClassName(string sClassName, bool sCustomClass = false)
        {
            if (!sClassName.StartsWith("NWD"))
            {
                return "NWDCustomModels.Models." + sClassName + ", NWDCustomModels, Version=" + kModelVersion + ", Culture=neutral, PublicKeyToken=null";
            }
            else
            {
                return "NWDStandardModels.Models." + sClassName + ", NWDStandardModels, Version=" + kModelVersion + ", Culture=neutral, PublicKeyToken=null";
            }
        }

        static public string GetAssetSerialization (string sPath, AssetType sAssetType, out string sTitle, out string sDescription, out string sType)
        {
            string tPath = sPath.Replace("**", "");
            UnityEngine.Object tAsset = null;

            if (!tPath.Contains("/") && !tPath.Contains("\\"))
            {
                tAsset = FindObjectFromAddressable(sPath, sAssetType);

                if (tAsset == null)
                {
                    Debug.LogWarning("Could not find asset: '" + sPath + "'");
                    throw new Exception();
                }
            }
            else
            {
                tAsset = AssetDatabase.LoadMainAssetAtPath(tPath);
            }

            string tGUId;
            long tLocalId;

            if (!tAsset || !AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tAsset, out tGUId, out tLocalId))
            {
                tGUId = AssetDatabase.AssetPathToGUID(tPath);
                tLocalId = 0;
            }

            sType = GetNWDAssetType (tAsset?.GetType());
            sTitle = Path.GetFileNameWithoutExtension(tPath);
            sDescription = tGUId + ":" + tLocalId;

            return sDescription + ":" + sTitle + ":2";
        }

        static private string GetNWDAssetType (Type sType)
        {
            if (sType == typeof(Texture2D))
            {
                return "NWDSpriteAsset";
            }
            if (sType == typeof(Texture))
            {
                return "NWDTextureAsset";
            }
            if (sType == typeof(TextAsset))
            {
                return "NWDTextAsset";
            }
            if (sType == typeof(AudioClip))
            {
                return "NWDAudioAsset";
            }
            if (sType == typeof(VideoClip))
            {
                return "NWDVideoAsset";
            }
            if (sType == typeof(GameObject))
            {
                return "NWDPrefabAsset";
            }
            if (sType?.IsSubclassOf (typeof(ScriptableObject)) ?? false)
            {
                return "NWDScriptableObjectAsset";
            }
            return "NWDAsset";
        }

        static private UnityEngine.Object FindObjectFromAddressable(string sKey, AssetType sType)
        {
            if (sType.HasFlag(AssetType.Object))
            {
                try
                {
                    UnityEngine.Object rResult = Addressables.LoadAsset<UnityEngine.Object>(sKey).Result;
                    if (rResult)
                    {
                        return rResult;
                    }
                }
                catch { }
            }
            string[] tGUIDs;
            if (sType.HasFlag(AssetType.Scriptable))
            {
                tGUIDs = AssetDatabase.FindAssets("t:ScriptableObject " + sKey);
                if (tGUIDs.Length > 0)
                {
                    return AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0]));
                }
            }

            if (sType.HasFlag(AssetType.Scene))
            {
                tGUIDs = AssetDatabase.FindAssets("t:Scene " + sKey);
                if (tGUIDs.Length > 0)
                {
                    return AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(tGUIDs[0]));
                }
            }

            return null;
        }

        static public object GetTextAndAudio(NWDLocalizableLongTextAndAudioType sValue)
        {
            Dictionary<string, string> tDico = sValue.GetDictionary();
            Dictionary<NWDLanguageEnum, object> tTextDico = new Dictionary<NWDLanguageEnum, object>();
            Dictionary<NWDLanguageEnum, object> tAudioDico = new Dictionary<NWDLanguageEnum, object>();

            foreach (var k in tDico)
            {
                string[] split = k.Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);

                if (k.Key.Equals("fr"))
                {
                    tTextDico.Add(NWDLanguageEnum.French, split[0]);
                    tAudioDico.Add(NWDLanguageEnum.French, ProcessNewAsset(split[1], AssetType.Object));
                }
                else if (k.Key.Equals("en"))
                {
                    tTextDico.Add(NWDLanguageEnum.English, split[0]);
                    tAudioDico.Add(NWDLanguageEnum.English, ProcessNewAsset(split[1], AssetType.Object));
                }
            }

            string[] tBases = sValue.GetBaseString().Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);

            if (!tTextDico.ContainsKey(NWDLanguageEnum.French))
            {
                if (!string.IsNullOrWhiteSpace(tBases[0]))
                {
                    tTextDico.Add(NWDLanguageEnum.French, tBases[0]);
                }
            }

            if (!tAudioDico.ContainsKey(NWDLanguageEnum.French))
            {
                if (!string.IsNullOrWhiteSpace(tBases[1]))
                {
                    tAudioDico.Add(NWDLanguageEnum.French, ProcessNewAsset(tBases[1], AssetType.Object));
                }
            }

            string tBase = tBases[0];
            if (string.IsNullOrWhiteSpace(tBase))
            {
                if (tTextDico.TryGetValue(NWDLanguageEnum.French, out object tValue))
                {
                    tBase = (string)tValue;
                }
                else
                {
                    tBase = "No title";
                }
            }

            var tExport = new
            {
                Text = ProcessNewLocalizedString (tTextDico, tBase),
                Audio = ProcessNewLocalizedAsset (tAudioDico, tBase, "NWDAudioLocalization"),
            };
            return tExport;
        }

        object GetVideo(NWDLocalizableVideoClipType sValue)
        {
            Dictionary<string, string> tDico = sValue.GetDictionary();
            Dictionary<NWDLanguageEnum, object> tNewDico = new Dictionary<NWDLanguageEnum, object>();
            foreach (var k in tDico)
            {
                if (k.Key.Equals("fr"))
                {
                    tNewDico.Add(NWDLanguageEnum.French, ProcessNewAsset(k.Value, AssetType.Object));
                }
                else if (k.Key.Equals("en"))
                {
                    tNewDico.Add(NWDLanguageEnum.English, ProcessNewAsset(k.Value, AssetType.Object));
                }
            }

            if (!tNewDico.ContainsKey(NWDLanguageEnum.French))
            {
                string tBase = sValue.GetBaseString();
                if (!string.IsNullOrWhiteSpace(tBase))
                {
                    tNewDico.Add(NWDLanguageEnum.French, ProcessNewAsset(tBase, AssetType.Object));
                }
            }

            var tExport = new
            {
                Value = tNewDico,
            };
            return tExport;
        }

        public NWDExportObject(Dictionary<NWDLanguageEnum, object> sValues, string sContext, string sClassName)
        {
            long sReference = GetNewReference();
            object tExport = null;
            switch (sClassName)
            {
                case "NWDAudioLocalization":
                    tExport = new
                    {
                        Reference = sReference,
                        Audio = new { Value = sValues },
                        Key = "",
                        Context = sContext,
                        NeedToBeTranslated = true,
                    };
                    break;
                case "NWDVideoLocalization":
                    tExport = new
                    {
                        Reference = sReference,
                        Video = new { Value = sValues },
                        Key = "",
                        Context = sContext,
                        NeedToBeTranslated = true,
                    };
                    break;
            }

            Init(sReference.ToString(), sContext, "", JsonConvert.SerializeObject(tExport), sClassName);
        }

        public NWDExportObject(string sValue, AssetType sType)
        {
            long sReference = GetNewReference();

            string tTitle, tDescription, tType;

            var tExport = new
            {
                Reference = sReference,
                Asset = new
                {
                    UnityAsset = GetAssetSerialization(sValue, sType, out tTitle, out tDescription, out tType)
                }
            };

            Init(sReference.ToString(), tTitle, tDescription, JsonConvert.SerializeObject(tExport), tType);
        }

        public NWDExportObject(NWDLocalizableType sValue)
        {
            long tReference = GetNewReference();
            string tTitle;
            var tExport = new
            {
                // Specific
                Reference = tReference,
                Text = NWDLocalization.GetText(sValue, out tTitle),
                Key = "",
                Context = sValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            Init(tReference.ToString(), tTitle, "", JsonConvert.SerializeObject(tExport), "NWDStringLocalization");
        }

        public NWDExportObject(Dictionary<NWDLanguageEnum, object> sValues, string sContext)
        {
            if (sValues.Count == 0)
            {
                throw new Exception();
            }
            long tReference = GetNewReference();
            var tExport = new
            {
                // Specific
                Reference = tReference,
                Text = new { Value = sValues },
                Key = "",
                Context = sContext,
                NeedToBeTranslated = true,
            };

            Init(tReference.ToString(), sContext, "", JsonConvert.SerializeObject(tExport), "NWDStringLocalization");
        }

        public NWDExportObject(NWDLocalizableVideoClipType sValue, long sReference)
        {
            var tExport = new
            {
                // Specific
                Reference = sReference,
                Video = GetVideo(sValue),
                Key = "",
                Context = sValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            Init(sReference.ToString(), sValue.GetBaseString(), "", JsonConvert.SerializeObject (tExport), "NWDVideoLocalization");
        }

        public NWDExportObject(string sReference, string sTitle, string sDescription, string sJson, string sClassName, bool sCustomClass = false)
        {
            Init(sReference, sTitle, sDescription, sJson, sClassName, sCustomClass);
        }

        public void Init(string sReference, string sTitle, string sDescription, string sJson, string sClassName, bool sCustomClass = false)
        {
            ReferenceOld = sReference;
            ReferenceNew = NWDToolbox.NumericCleaner(sReference);
            StringBuilder tFile = new StringBuilder();
            object tData = new
            {
                TrackId = 100,
                TrackKind = 0,
                State = 1,
                Origin = 0,
                Data = sJson,
                Trashed = false,

            };

            tFile.AppendLine("-- reference  " + ReferenceOld + " => " + ReferenceNew);
            tFile.AppendLine("INSERT INTO [" + kTableName + "] (" +
                             "[ProjectUniqueId], " +
                             "[DataByDataTrack], " +
                             "[Title], " +
                             "[Description], " +
                             "[ClassName], " +
                             "[IsLocked], " +
                             "[LockLimit], " +
                             "[LockerName], " +
                             "[AvailableForWeb], " +
                             "[AvailableForGame], " +
                             "[AvailableForApp], " +
                             "[ProjectId], " +
                             "[Creation], " +
                             "[Modification], " +
                             "[Active], " +
                             "[Trashed], " +
                             //"`NeedTranslate`, " +
                             //"`NeedToBeTranslated` ," +
                             //Last line
                             "[Reference]" +
                             ") VALUES "
                             + "("
                             + "" + kProjectID + ", " //project unique ID of babaoo game
                             + "'[" + JsonConvert.SerializeObject(tData)/*.Replace(@"\", @"\\")*/.Replace("'", "''") + "]', " //DataByDataTrack
                             + "'" + CleanString(sTitle).Replace("'", "''") + "', " //Title
                             + "'" + CleanString(sDescription, 100).Replace("'", "''") + "', " //Description
                             + "'" + ClassName(sClassName, sCustomClass = false).Replace("'", "''") + "', " //ClassName
                             + "0, " //IsLocked
                             + "0, " //LockLimit
                             + "'', " //LockerName
                             + "1, " //AvailableForWeb
                             + "1, " //AvailableForGame
                             + "1, " //AvailableForApp
                             + "" + kHubProjectId + ", " //project ID of babaoo NWD
                             + "0, " //Creation
                             + "0, " //Modification
                             + "1, " //Active
                             + "0, " //Trashed
                             //+ "1," //NeedTranslate
                             //+ "1 ," //NeedToBeTranslated
                             //last line
                             + "" + ReferenceNew + "" //Reference
                             + ");");
            JsonObject = tFile.ToString();
        }

        private string CleanString(string sValue, int sTrim = 30)
        {
            bool tTrimed = false;
            if (sValue.Length > sTrim)
            {
                sValue = sValue.Substring(0, sTrim);
                tTrimed = true;
            }

            int tEndOfLine = sValue.IndexOf("\n");

            if (tEndOfLine >= 0)
            {
                sValue = sValue.Substring(0, tEndOfLine);
                sValue.Replace("\r", "");
                tTrimed = true;
            }

            if (tTrimed)
            {
                sValue += "...";
            }
            return sValue;
        }
    }
}
#endif