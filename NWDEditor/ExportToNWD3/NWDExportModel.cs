using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace NetWorkedData
{
    public class NWDExportObject
    {
        public string ReferenceOld = string.Empty;
        public string ReferenceNew = string.Empty;
        public string JsonObject = string.Empty;

        static public Dictionary<string, string> ReferencesDictionary = new Dictionary<string, string>();
        static public Dictionary<string,NWDExportObject> NWDStringLocalizationList = new Dictionary<string,NWDExportObject>();
        static public Dictionary<string,NWDExportObject> NWDAssetDataList = new Dictionary<string,NWDExportObject>();

        private static ulong tReferenceUnique = (ulong)NWDToolbox.Timestamp() * 10000000000000000;

        public static object ProcessNewLocalizedString(ulong sProjectHub, ulong sProjectId, NWDLocalizableType sLocalizedString)
        {
            NWDExportObject tNewData = new(sProjectHub, sProjectId, sLocalizedString);
            string tNewReference = tNewData.ReferenceNew;
            NWDStringLocalizationList.Add(tNewReference, tNewData);

            var tExport = new {
                Reference = tNewReference,
            };
            return tExport;
        }
        
        public static object ProcessNewAsset(ulong sProjectHub, ulong sProjectId, NWDAssetType sAssetType)
        {
            NWDExportObject tNewData = new(sProjectHub, sProjectId, sAssetType);
            NWDAssetDataList.Add(tNewData.ReferenceNew, tNewData);

            List<object> rObjects = new() { tNewData.ReferenceNew };
            return rObjects;
        }

        public static object ProcessNewAsset(ulong sProjectHub, ulong sProjectId, List<NWDAssetType> sAssetTypes)
        {
            List<object> rObjects = new List<object>();
            foreach(NWDAssetType k in sAssetTypes)
            {
                NWDExportObject tNewData = new(sProjectHub, sProjectId, k);
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

        public static string GetNewReference()
        {
            tReferenceUnique++;
            return tReferenceUnique.ToString();
        }

        public static string ClassName(string sClassName, bool sCustomClass = false)
        {
            if (sCustomClass)
            {
                return "NWDCustomModels.Models." + sClassName + ", NWDCustomModels, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            }
            else
            {
                return "NWDStandardModels.Models." + sClassName + ", NWDStandardModels, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            }
        }

        public NWDExportObject(ulong sProjectHub, ulong sProjectId, NWDAssetType sValue)
        {
            Init(sProjectHub, sProjectId, GetNewReference(), "", "", "{NULL}","NWDAssetData");
        }

        public NWDExportObject(ulong sProjectHub, ulong sProjectId, NWDLocalizableType sValue)
        {
            sValue.BaseVerif();
            Dictionary<string, string> kSplitDico = new Dictionary<string, string>(sValue.kSplitDico);
            string tContent = string.Empty;
            if (kSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev))
            {
                tContent = kSplitDico[NWDDataLocalizationManager.kBaseDev];
                kSplitDico.Remove(NWDDataLocalizationManager.kBaseDev);
            }
        
            StringBuilder tReturn = new StringBuilder();
            tReturn.Append("{");
            foreach (KeyValuePair<string, string> tdico in sValue.kSplitDico)
            {
                tReturn.Append("\"" + tdico.Key + "\":\"" + tdico.Value.Replace("\"", "\\\"") + "\",");
            }
        
            tReturn.Append("\"Context\":\"" + tContent + "\"");
            tReturn.Append("}");
        
            Init(sProjectHub, sProjectId, GetNewReference(), sValue.GetBaseString(), "", tReturn.ToString(), "NWDTranslate");
        }

        public NWDExportObject(ulong sProjectHub, ulong sProjectId, string sReference, string sTitle, string sDescription, string sJson, string sClassName, bool sCustomClass = false)
        {
            Init(sProjectHub, sProjectId, sReference, sTitle, sDescription, sJson, sClassName, sCustomClass);
        }

        public void Init(ulong sProjectHub, ulong sProjectId, string sReference, string sTitle, string sDescription, string sJson, string sClassName, bool sCustomClass = false)
        {
            ReferenceOld = sReference;
            ReferenceNew = NWDToolbox.NumericCleaner(sReference);
            StringBuilder tFile = new StringBuilder();
            string tData = "[{\"TrackId\":100,\"TrackKind\":0,\"State\":1,\"Origin\":0,\"Data\":\"" + sJson.Replace("\"", "\\\\\\\"") + "\",\"Trashed\":false}]";

            tFile.AppendLine("# reference  " + ReferenceOld + " => " + ReferenceNew);
            tFile.AppendLine("INSERT INTO `LOCAL_Dev_NWDMetaData` (" +
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
                             "`NeedTranslate`, " +
                             "`NeedToBeTranslated` ," +
                             //Last line
                             "`Reference`" +
                             ") VALUES "
                             + "("
                             + "" + sProjectId + ", " //project unique ID of babaoo game
                             + "'" + tData.Replace("'", "''") + "', " //DataByDataTrack
                             + "'" + sTitle.Replace("'", "''") + "', " //Title
                             + "'" + sDescription.Replace("'", "''") + "', " //Description
                             + "'" + ClassName(sClassName, sCustomClass = false) + "', " //ClassName
                             + "0, " //IsLocked
                             + "0, " //LockLimit
                             + "'', " //LockerName
                             + "1, " //AvailableForWeb
                             + "1, " //AvailableForGame
                             + "1, " //AvailableForApp
                             + "" + sProjectHub + ", " //project ID of babaoo NWD
                             + "0, " //Creation
                             + "0, " //Modification
                             + "1, " //Active
                             + "0, " //Trashed
                             + "1," //NeedTranslate
                             + "1 ," //NeedToBeTranslated
                             //last line
                             + "" + ReferenceNew + "" //Reference
                             + ");");
            JsonObject = tFile.ToString();
        }
    }
}