//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using SQLite.Attribute;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDUserBarterPropositionConnection : NWDConnection<NWDUserBarterProposition>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UBP")]
    [NWDClassDescriptionAttribute("User Barter Proposition descriptions Class")]
    [NWDClassMenuNameAttribute("User Barter Proposition")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserBarterProposition : NWDBasis<NWDUserBarterProposition>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Barter Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        //[NWDAlias("BarterPlace")]
        public NWDReferenceType<NWDBarterPlace> BarterPlace
        {
            get; set;
        }
        //[NWDAlias("BarterRequest")]
        public NWDReferenceType<NWDUserBarterRequest> BarterRequest
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

       

        [NWDInspectorGroupStart("Barter References", true, true, true)]
        [NWDNotEditable]
        //[NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        //[NWDAlias("ItemsSend")]
        public NWDReferencesQuantityType<NWDItem> ItemsSend
        {
            get; set;
        }
        //[NWDAlias("BarterStatus")]
        public NWDTradeStatus BarterStatus
        {
            get; set;
        }
        [NWDNotEditable]
       //[NWDAlias("BarterRequestHash")]
        public string BarterRequestHash
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================