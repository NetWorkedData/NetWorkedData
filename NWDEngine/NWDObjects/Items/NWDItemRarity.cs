//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:30
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDItemRarity class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("IRY")]
    [NWDClassDescriptionAttribute("Rarity of Item")]
    [NWDClassMenuNameAttribute("Item Rarity")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDItemRarity : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDTooltips("The item to check")]
        public NWDReferenceType<NWDItem> ItemReference
        {
            get; set;
        }
        [NWDTooltips("The total of this item in all game save")]
        public long ItemTotal
        {
            get; set;
        }
        [NWDTooltips("The total of user own this item in all game save")]
        public long OwnerUserTotal
        {
            get; set;
        }
        [NWDTooltips("The total of user in all game save")]
        public long UserTotal
        {
            get; set;
        }
        [NWDTooltips("The maximum item in all game save for one game save")]
        public long Maximum
        {
            get; set;
        }
        [NWDTooltips("The minimum item in all game save for one game save")]
        public long Minimum
        {
            get; set;
        }
        [NWDTooltips("The average in all game save")]
        public double Average
        {
            get; set;
        }
        [NWDTooltips("The frequency in all game save (OwnerUserTotal/UserTotal)/Average")]
        public double Frequency
        {
            get; set;
        }
        [NWDTooltips("The rarity in all game save (1/Frequency)")]
        public double Rarity
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemRarity()
        {
            //Debug.Log("NWDItemRarity Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItemRarity(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItemRarity Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================