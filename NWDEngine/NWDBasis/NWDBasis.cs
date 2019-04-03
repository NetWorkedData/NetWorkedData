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
        const string K_BASIS_INDEX = "BasisIndex";
        const string K_INTERNAL_INDEX = "InternalIndex";
        const string K_EDITOR_INDEX = "EditorIndex";
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset()]
        [NWDInspectorGroupStart("Basis")]
        [PrimaryKey, AutoIncrement, NWDNotEditable]
        [NWDCertified]
        public int ID
        {
            get; set;
        }
        [Indexed(K_BASIS_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
        public string Reference
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDFlagsEnum]
        [NWDCertified]
        public NWDBasisCheckList CheckList
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(K_BASIS_INDEX, 4)]
        public int WebModel
        {
            get;set;
        }
        [Indexed(K_INTERNAL_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
        public string InternalKey
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(K_INTERNAL_INDEX, 0)]
        public string InternalDescription
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string Preview
        {
            get; set;
        }
        [Indexed(K_EDITOR_INDEX, 0)]
        [NWDNotEditable]
        [NWDCertified]
        public bool AC
        {
            get; set;
        }
        [Indexed(K_EDITOR_INDEX, 1)]
        [NWDNotEditable]
        [NWDCertified]
        public int DC
        {
            get; set;
        }
        [Indexed(K_EDITOR_INDEX, 2)]
        [NWDNotEditable]
        [NWDCertified]
        public int DM
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DD
        {
            get; set;
        }
        [Indexed(K_EDITOR_INDEX, 3)]
        [NWDNotEditable]
        [NWDCertified]
        public int XX
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string Integrity
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public int DS
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(K_BASIS_INDEX, 3)]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(K_BASIS_INDEX, 2)]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        [Indexed(K_BASIS_INDEX, 1)]
        public int ProdSync
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public NWDBasisTag Tag
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string ServerHash
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public string ServerLog
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDCertified]
        public bool InError
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================