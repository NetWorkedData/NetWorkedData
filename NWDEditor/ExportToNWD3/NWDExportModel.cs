using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace NetWorkedData
{
    public class NWDExportObject
    {
        public const string kTableName = "Local__Prod_NWDMetaData";
        public const long kHubProjectId = 86868;
        public const long kProjectID = 16958220569938382;
        public const string kModelVersion = "0.0.0.0";


        public string ReferenceOld = string.Empty;
        public long ReferenceNew = 0;
        public string JsonObject = string.Empty;

        static public Dictionary<string, string> ReferencesDictionary = new Dictionary<string, string>();
        static public Dictionary<long,NWDExportObject> NWDStringLocalizationList = new Dictionary<long,NWDExportObject>();
        static public Dictionary<long, NWDExportObject> NWDAssetLocalizationList = new Dictionary<long, NWDExportObject>();
        static public Dictionary<long,NWDExportObject> NWDAssetDataList = new Dictionary<long,NWDExportObject>();

        private static long tReferenceUnique = NWDToolbox.Timestamp() * 10000000000000000;

        public static object ProcessNewLocalizedString(NWDLocalizableType sLocalizedString)
        {
            NWDExportObject tNewData = new NWDExportObject(sLocalizedString);
            long tNewReference = tNewData.ReferenceNew;
            NWDStringLocalizationList.Add(tNewReference, tNewData);

            var tExport = new {
                Reference = tNewReference,
            };
            return tExport;
        }

        public static object ProcessNewLocalizedString(Dictionary<NWDLanguageEnum, object> sValues, string sContext)
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
            NWDExportObject tNewData = new NWDExportObject(sValue);
            long tNewReference = tNewData.ReferenceNew;
            NWDAssetDataList.Add(tNewReference, tNewData);

            var tExport = new
            {
                Reference = tNewReference,
            };
            return tExport;
        }

        public static object ProcessNewAsset(NWDAssetType sAssetType, string sClassName)
        {
            if (sAssetType == null)
            {
                return null;
            }
            return ProcessNewAsset(sAssetType.Value, sClassName);
        }

        public static object ProcessNewAsset(List<NWDAssetType> sAssetTypes, string sClassName)
        {
            return ProcessNewAsset(sAssetTypes.Where(x => x != null).Select(x => x.Value).ToList(), sClassName);
        }


        public static object ProcessNewAsset(string sAssetType, string sClassName)
        {
            NWDExportObject tNewData = new(sAssetType, sClassName);
            NWDAssetDataList.Add(tNewData.ReferenceNew, tNewData);

            object rObjects = new { Reference = tNewData.ReferenceNew };
            return rObjects;
        }

        public static object ProcessNewAsset(List<string> sAssetTypes, string sClassName)
        {
            List<object> rObjects = new List<object>();
            foreach (string k in sAssetTypes)
            {
                NWDExportObject tNewData = new(k, sClassName);
                NWDAssetDataList.Add(tNewData.ReferenceNew, tNewData);
                rObjects.Add(tNewData.ReferenceNew);
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
            string rReturn = tReferenceUnique.ToString();
            ReferencesDictionary.Add(sOldReference, rReturn);
            return rReturn;
        }

        public static long GetNewReference()
        {
            tReferenceUnique++;
            return tReferenceUnique;
        }

        public static string ClassName(string sClassName, bool sCustomClass = false)
        {
            if (sCustomClass)
            {
                return "NWDCustomModels.Models." + sClassName + ", NWDCustomModels, Version=" + kModelVersion + ", Culture=neutral, PublicKeyToken=null";
            }
            else
            {
                return "NWDStandardModels.Models." + sClassName + ", NWDStandardModels, Version=" + kModelVersion + ", Culture=neutral, PublicKeyToken=null";
            }
        }

        static public string GetAssetSerialization (string sPath)
        {
            string tPath = sPath.Replace("**", "");
            UnityEngine.Object tAsset = AssetDatabase.LoadMainAssetAtPath(tPath);
            string tGUId;
            long tLocalId;

            if (!tAsset || !AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tAsset, out tGUId, out tLocalId))
            {
                tGUId = AssetDatabase.AssetPathToGUID(tPath);
                tLocalId = 0;
            }

            string tKey = Path.GetFileNameWithoutExtension(tPath);
            return tGUId + ":" + tLocalId + ":" + tKey + ":2";
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
                    tAudioDico.Add(NWDLanguageEnum.French, ProcessNewAsset(split[1], "NWDAudioAsset"));
                }
                else if (k.Key.Equals("en"))
                {
                    tTextDico.Add(NWDLanguageEnum.English, split[0]);
                    tAudioDico.Add(NWDLanguageEnum.English, ProcessNewAsset(split[1], "NWDAudioAsset"));
                }
            }

            if (!tTextDico.ContainsKey(NWDLanguageEnum.French))
            {
                string tBase = sValue.GetBaseString().Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (!string.IsNullOrWhiteSpace(tBase))
                {
                    tTextDico.Add(NWDLanguageEnum.French, tBase);
                }
            }

            if (!tAudioDico.ContainsKey(NWDLanguageEnum.French))
            {
                string tBase = sValue.GetBaseString().Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (!string.IsNullOrWhiteSpace(tBase))
                {
                    tAudioDico.Add(NWDLanguageEnum.French, ProcessNewAsset(tBase, "NWDAudioAsset"));
                }
            }

            var tExport = new
            {
                Text = ProcessNewLocalizedString (tTextDico, ""),
                Audio = ProcessNewLocalizedAsset (tAudioDico, "", "NWDAudioLocalization"),
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
                    tNewDico.Add(NWDLanguageEnum.French, ProcessNewAsset(k.Value, "NWDVideoAsset"));
                }
                else if (k.Key.Equals("en"))
                {
                    tNewDico.Add(NWDLanguageEnum.English, ProcessNewAsset(k.Value, "NWDVideoAsset"));
                }
            }

            if (!tNewDico.ContainsKey(NWDLanguageEnum.French))
            {
                string tBase = sValue.GetBaseString();
                if (!string.IsNullOrWhiteSpace(tBase))
                {
                    tNewDico.Add(NWDLanguageEnum.French, ProcessNewAsset(tBase, "NWDVideoAsset"));
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

            Init(sReference.ToString(), "", "", JsonConvert.SerializeObject(tExport), sClassName);
        }

        public NWDExportObject(string sValue, string sClassName)
        {

            long sReference = GetNewReference();

            var tExport = new
            {
                Reference = sReference,
                Asset = new
                {
                    UnityAsset = GetAssetSerialization(sValue)
                }
            };

            Init(sReference.ToString(), "", "", JsonConvert.SerializeObject(tExport), sClassName);
        }

        public NWDExportObject(NWDLocalizableType sValue)
        {
            long tReference = GetNewReference();
            var tExport = new
            {
                // Specific
                Reference = tReference,
                Text = NWDLocalization.GetText(sValue),
                Key = "",
                Context = sValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            Init(tReference.ToString(), sValue.GetBaseString(), "", JsonConvert.SerializeObject(tExport), "NWDStringLocalization");
        }

        public NWDExportObject(Dictionary<NWDLanguageEnum, object> sValues, string sContext)
        {
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

        public NWDExportObject(NWDLocalizableVideoClipType sValue)
        {
            long tReference = GetNewReference();

            var tExport = new
            {
                // Specific
                Reference = tReference,
                Video = GetVideo(sValue),
                Key = "",
                Context = sValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            Init(tReference.ToString(), sValue.GetBaseString(), "", JsonConvert.SerializeObject (tExport), "NWDVideoLocalization");
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

            tFile.AppendLine("# reference  " + ReferenceOld + " => " + ReferenceNew);
            tFile.AppendLine("INSERT INTO `" + kTableName + "` (" +
                             "`ProjectUniqueId`, " +
                             "`DataByDataTrack`, " +
                             "`Title`, " +
                             "`Description`, " +
                             "`ClassName`, " +
                             "`IsLocked`, " +
                             "`LockLimit`, " +
                             "`LockerName`, " +
                             "`AvailableForWeb`, " +
                             "`AvailableForGame`, " +
                             "`AvailableForApp`, " +
                             "`ProjectId`, " +
                             "`Creation`, " +
                             "`Modification`, " +
                             "`Active`, " +
                             "`Trashed`, " +
                             //"`NeedTranslate`, " +
                             //"`NeedToBeTranslated` ," +
                             //Last line
                             "`Reference`" +
                             ") VALUES "
                             + "("
                             + "" + kProjectID + ", " //project unique ID of babaoo game
                             + "'[" + JsonConvert.SerializeObject(tData).Replace(@"\", @"\\").Replace("'", @"\'") + "]', " //DataByDataTrack
                             + "'" + sTitle.Replace("'", @"\'") + "', " //Title
                             + "'" + sDescription.Replace("'", @"\'") + "', " //Description
                             + "'" + ClassName(sClassName, sCustomClass = false).Replace("'", @"\'") + "', " //ClassName
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
    }
}
#endif