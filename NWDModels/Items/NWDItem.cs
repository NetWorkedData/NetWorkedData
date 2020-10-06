//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDItemNotification : int
    {
        NoNotification = 0,
        Notification = 1,
        SmallNotification = 2,
        BigNotification = 3,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDItemConnection : NWDConnection<NWDItem>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ITM")]
    [NWDClassDescriptionAttribute("Item descriptions Class")]
    [NWDClassMenuNameAttribute("Item")]
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_FirstAcquisitionNotificationKey = "NWDItem_FirstAcquisition";
        public const string K_AddNotificationKey = "NWDItem_Add";
        public const string K_RemoveNotificationKey = "NWDItem_Remove";
        public const string K_NoMoreNotificationKey = "NWDItem_NoMore";
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        [NWDTooltips("The name usable in game for 0 or 1 object")]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The name usable in game for 2 and more objects")]
        public NWDLocalizableStringType PluralName
        {
            get; set;
        }
        [NWDTooltips("The sub name in game")]
        public NWDLocalizableStringType SubName
        {
            get; set;
        }
        [NWDTooltips("The description of object in game")]
        public NWDLocalizableLongTextType Description
        {
            get; set;
        }
        [NWDInspectorGroupEnd]


        //[NWDInspectorGroupStart("Geograpphical", true, true, true)]
        //public NWDReferencesListType<NWDWorld> WorldList
        //{
        //    get; set;
        //}
        //public NWDReferencesListType<NWDSector> SectorList
        //{
        //    get; set;
        //}
        //public NWDReferencesListType<NWDArea> AreaList
        //{
        //    get; set;
        //}
        //[NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Notifications", true, true, true)]
        public NWDItemNotification FirstAcquisitionNotification
        {
            get; set;
        }
        public NWDItemNotification AddItemNotification
        {
            get; set;
        }
        public NWDItemNotification RemoveItemNotification
        {
            get; set;
        }
        public NWDItemNotification NoMoreItemNotification
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Rarity", true, true, true)]
        [NWDFloatSlider(0.0F, 1.0F)]
        [NWDEntitled("Rarity : float [0,1]")]
        public float Rarity
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Usage", true, true, true)]
        //[NWDNotEditableAttribute]
        [NWDTooltips("Item is never visible by the player")]
        public bool HiddenInGame
        {
            get; set;
        }
        [NWDTooltips("Item is countable or not?")]
        public bool Uncountable
        {
            get; set;
        }
        [NWDTooltips("Item is usable or not?")]
        public bool Usable
        {
            get; set;
        }
        [NWDInDevelopment]
        [NWDNotEditable]
        [Obsolete]
        public float DelayBeforeUse
        {
            get; set;
        }
        [NWDInDevelopment]
        [NWDNotEditable]
        [Obsolete]
        public float DurationOfUse
        {
            get; set;
        }
        [NWDInDevelopment]
        [NWDNotEditable]
        [Obsolete]
        public float DelayBeforeReUse
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Extensions", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemExtensionQuantity
        {
            get; set;
        }
        public NWDReferencesListType<NWDParameter> ParameterList
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Assets", true, true, true)]
        [NWDInspectorHeader("Sprites")]
        public NWDSpriteType PrimarySprite
        {
            get; set;
        }
        public NWDSpriteType SecondarySprite
        {
            get; set;
        }
        public NWDSpriteType TertiarySprite
        {
            get; set;
        }
        [NWDInspectorHeader("Textures")]
        public NWDTextureType PrimaryTexture
        {
            get; set;
        }
        public NWDTextureType SecondaryTexture
        {
            get; set;
        }
        public NWDTextureType TertiaryTexture
        {
            get; set;
        }
        [NWDInspectorHeader("Colors")]
        public NWDColorType PrimaryColor
        {
            get; set;
        }
        public NWDColorType SecondaryColor
        {
            get; set;
        }
        public NWDColorType TertiaryColor
        {
            get; set;
        }
        [NWDInspectorHeader("Prefabs")]
        public NWDPrefabType PrimaryPrefab
        {
            get; set;
        }
        public NWDPrefabType SecondaryPrefab
        {
            get; set;
        }
        public NWDPrefabType TertiaryPrefab
        {
            get; set;
        }

        [NWDTooltips("Particules effect used overlay render (for special item)")]
        public NWDPrefabType EffectPrefab
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Development addons", true, true, true)]
        public string JSON
        {
            get; set;
        }
        public string KeysValues
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
