using System.Collections.Generic;
using System.Text;
using System.Dynamic;
using Unity.Android.Types;

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

        public static string ProcessNewLocalizedString(ulong sProjectHub, ulong sProjectId,NWDLocalizableType sLocalizedString)
        {
            NWDExportObject tReturn = new NWDExportObject(sProjectHub,sProjectId,sLocalizedString);
            string tNewReference = tReturn.ReferenceNew;
            NWDStringLocalizationList.Add(tNewReference,tReturn);
            return "{\"Reference\":" + tNewReference + ", \"Type\":\"NWDTranslate\"}";
        }
        
        public static string ProcessNewAsset(ulong sProjectHub, ulong sProjectId,NWDAssetType sAssetType)
        {
            string tNewReference = GetNewReference();
            // TODO Process creation of new NWDAssetData
            // Create new JSON
            // Add record to file to export ... 
            string tObject = "{bloblo}";
            //NWDAssetDataList.Add(tNewReference, tObject);
            return "{\"Reference\":" + tNewReference + ", \"Type\":\"NWDAssetData\"}";
        }
        public static string GetNewReference(string sOldReference)
        {
            tReferenceUnique++;
            string rReturn = tReferenceUnique.ToString();
            ReferencesDictionary.Add(sOldReference, rReturn);
            return rReturn;
        }

        public static dynamic Merge(object item1, object item2)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (System.Reflection.PropertyInfo fi in item1.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item1, null);
            }
            foreach (System.Reflection.PropertyInfo fi in item2.GetType().GetProperties())
            {
                result[fi.Name] = fi.GetValue(item2, null);
            }
            return result;
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