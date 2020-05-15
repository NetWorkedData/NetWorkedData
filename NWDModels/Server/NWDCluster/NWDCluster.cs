//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDClusterHelper : NWDHelper<NWDCluster>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDCluster class. This class is use for (complete description here).
    /// </summary>
    [NWDClassTrigrammeAttribute("CLS")]
    [NWDClassDescriptionAttribute("Cluster")]
    [NWDClassMenuNameAttribute("Cluster")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDCluster : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]

        [NWDInspectorGroupStart("Servers")]
        [NWDNotEditable]
        public NWDReferencesListType<NWDServerDomain> Domains { get; set; }
        public NWDReferencesListType<NWDServerServices> Services { get; set; }
        public NWDReferencesListType<NWDServerDatas> DataBases { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Server Environment Actif")]
        public bool Dev { get; set; }
        public bool Preprod { get; set; }
        public bool Prod { get; set; }
        [NWDNotEditable]
        [NWDVeryLongString]
        public string Information { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("IP of Editor")]
        [NWDNotEditable]
        public bool LimitEditorByIP { get; set; }
        [NWDNotEditable]
        public NWDIPType EditorIP { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCluster()
        {
            //Debug.Log("NWDCluster Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCluster(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCluster Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================