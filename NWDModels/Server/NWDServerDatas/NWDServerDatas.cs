//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDInternalKeyNotEditable]
    [NWDClassUnityEditorOnlyAttribute]
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SSD")]
    [NWDClassDescriptionAttribute("Server Datas descriptions Class")]
    [NWDClassMenuNameAttribute("Server Datas")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDServerDatas : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        [NWDInspectorGroupStart("Data Account Range")]
        [NWDNotEditable]
        public int Range { get; set; }
        public int UserMax { get; set; }

        //public NWDReferenceType<NWDServerDomain> Server { get; set; }
        [NWDEntitled("Account Range Start")]
        [NWDIntSlider(0,999)]
        [NWDNotEditable]
        [Obsolete]
        public int AccountRangeStart { get; set; }
        [NWDEntitled("Account Range Finish")]
        [NWDIntSlider(0, 999)]
        [NWDNotEditable]
        [Obsolete]
        public int AccountRangeEnd { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Authentification MySQL")]
        //[NWDEntitled("MySQL Port")]
        //public int MySQLPort { get; set; }
        [NWDEntitled("MySQL IP")]
        public NWDIPType MySQLIP { get; set; }
        [NWDEntitled("MySQL Port")]
        public int MySQLPort { get; set; }
        [NWDEntitled("MySQL User")]
        public string MySQLUser { get; set; }
        [NWDEntitled("MySQL Password")]
        public NWDPasswordType MySQLPassword { get; set; }
        [NWDEntitled("MySQL Base")]
        public string MySQLBase { get; set; }
        [NWDEntitled("MySQL Root Password")]
        public NWDPasswordType Root_MysqlPassword { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Acces MySQL")]
        public bool External { get; set; }
        [NWDEntitled("PHP My Admin")]
        public bool PhpMyAdmin { get; set; }
        [NWDInspectorGroupEnd]

        //[NWDInspectorGroupStart("Authentification SSH")]
        //[NWDEntitled("SSH IP")]
        //public NWDIPType IP { get; set; }
        //[NWDEntitled("SSH Port")]
        //public int Port { get; set; }
        ////[NWDEntitled("SSH User")]
        ////public string User { get; set; }
        ////[NWDEntitled("SSH Password")]
        ////public NWDPasswordType Password { get; set; }
        //[NWDEntitled("SSH Admin User")]
        //public string Admin_User { get; set; }
        //[NWDEntitled("SSH Admin Password")]
        //public NWDPasswordType Admin_Password { get; set; }
        //[NWDEntitled("SSH Root User")]
        //public string Root_User { get; set; }
        //[NWDEntitled("SSH Root Password")]
        //public NWDPasswordType Root_Password { get; set; }
        //[NWDInspectorGroupEnd]

        //[NWDInspectorGroupStart("Install Server Options")]
        //public NWDServerDistribution Distribution { get; set; }
        //[NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Copy editor base from ...")]
        public NWDReferenceType<NWDServerDatas> ServerOriginal { get; set; }
        [NWDNotEditable]
        [NWDInspectorGroupStart("Server Environment Actif")]
        [NWDNotEditable]
        public bool Dev { get; set; }
        [NWDNotEditable]
        public bool Preprod { get; set; }
        [NWDNotEditable]
        public bool Prod { get; set; }
        [NWDNotEditable]
        public string Information { get; set; }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================