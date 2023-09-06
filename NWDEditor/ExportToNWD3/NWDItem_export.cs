using System.Collections;
using System.Collections.Generic;

namespace NetWorkedData
{
    public partial class NWDItem : NWDBasis
    {
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            // Create object for Language!

            NWDExportObject tName = new NWDExportObject(sProjectHub, sProjectId, Name);
            rReturn.Add(tName);
            // Create object for Asset!
            
            // no asset here
            
            // Create root object
            string tJson = "{"+
                             "\""+nameof(Reference)+"\":"+NWDToolbox.NumericCleaner(Reference)+", " +
                             //-------------------------
                             //-------------------------
                             //-------------------------
                             
                             
                             //Insert Here your Object
                
                             "\"Rarity\":"+Rarity.ToString("0.00000")+", " +
                             
                             // if you create object for langague or asset, use it 
                             
                            "\"Name\":"+tName.Link()+", " +
                             
                             //-------------------------
                             //-------------------------
                             //-------------------------
                             //-------------------------
                             "\""+nameof(AC)+"\":1" +
                             "}";

            NWDExportObject tObject = new NWDExportObject(sProjectHub, sProjectId, Reference, InternalKey, InternalDescription, tJson, nameof(NWDItem), false);
            rReturn.Add(tObject);
            return rReturn;
        }
    }
}
