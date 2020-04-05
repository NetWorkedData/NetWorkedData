//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
    //[NWDClassUnityEditorOnlyAttribute()]
    [NWDClassServerSynchronizeAttribute(false)]
    [NWDClassTrigrammeAttribute("CLS")]
    [NWDClassDescriptionAttribute("Cluster")]
    [NWDClassMenuNameAttribute("Cluster")]
    //[NWDInternalKeyNotEditableAttribute]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDCluster : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        //[NWDInspectorGroupStart("Domains to use")]
        //public NWDReferencesListType<NWDServerDomain> Domains { get; set; }
        //[NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Servers")]
        public NWDReferencesListType<NWDServerServices> Services { get; set; }
        public NWDReferencesListType<NWDServerDatas> DataBases { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Environment Actif")]
        public bool Dev { get; set; }
        public bool Preprod { get; set; }
        public bool Prod { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("IP of Editor")]
        [NWDNotEditable]
        public bool LimitEditorByIP { get; set; }
        [NWDNotEditable]
        public NWDIPType EditorIP { get; set; }
        //-------------------------------------------------------------------------------------------------------------
#endif
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