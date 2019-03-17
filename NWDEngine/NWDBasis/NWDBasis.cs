//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        protected bool InDatabase = false;
        protected bool FromDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Basis")]
        [PrimaryKey, AutoIncrement, NWDNotEditable]
        public int ID
        {
            get; set;
        }
        [Indexed("UpdateIndex", 0)]
        [NWDNotEditable]
        public string Reference
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDFlagsEnum]
        public NWDBasisCheckList CheckList
        {
            get; set;
        }
        [NWDNotEditable]
        public int WebModel
        {
            get;set;
        }
        [Indexed("InternalIndex", 0)]
        [NWDNotEditable]
        public string InternalKey
        {
            get; set;
        }
        [NWDNotEditable]
        public string InternalDescription
        {
            get; set;
        }
        [NWDNotEditable]
        public string Preview
        {
            get; set;
        }
        [Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public bool AC
        {
            get; set;
        }
        [Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public int DC
        {
            get; set;
        }
        [Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public int DM
        {
            get; set;
        }
        [NWDNotEditable]
        public int DD
        {
            get; set;
        }
        [NWDNotEditable]
        public int XX
        {
            get; set;
        }
        [NWDNotEditable]
        public string Integrity
        {
            get; set;
        }
        [NWDNotEditable]
        public int DS
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 3)]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 2)]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 1)]
        public int ProdSync
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDBasisTag Tag
        {
            get; set;
        }
        [NWDNotEditable]
        public string ServerHash
        {
            get; set;
        }
        [NWDNotEditable]
        public string ServerLog
        {
            get; set;
        }
        [NWDNotEditable]
        public bool InError
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================