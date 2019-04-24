// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:24
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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

#if UNITY_EDITOR
using UnityEditor;
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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ITM")]
    [NWDClassDescriptionAttribute("Item descriptions Class")]
    [NWDClassMenuNameAttribute("Item")]
    public partial class NWDItem : NWDBasis<NWDItem>
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

        [NWDInspectorGroupStart("Classification", true, true, true)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FamilyList
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Geograpphical", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList
        {
            get; set;
        }
        public NWDReferencesListType<NWDSector> SectorList
        {
            get; set;
        }
        public NWDReferencesListType<NWDArea> AreaList
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

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
        public NWDItem()
        {
            //Debug.Log("NWDItem Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItem Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Enrichment(string sText, int sCpt = 0, string sLanguage = null, bool sBold = true)
        {
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }

            // Replace Tag by Item Name
            string rText = sText.Replace("#I" + sCpt + BTBConstants.K_HASHTAG, tBstart + Name + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //ForRelationshipOnly = false;
            Uncountable = false;
            HiddenInGame = false;
            Usable = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDiscovered(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            NWDUserOwnership.SetDiscovered(this, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipAddValue;
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipSetValue;
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            return NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 6 );
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrixRect = NWDGUI.DiviseArea(sRect, 3, 6);

            NWDUserOwnership tOwnership = NWDUserOwnership.FindFisrtByItem(this);

            if (tOwnership != null)
            {
                GUI.Label(NWDGUI.AssemblyArea(tMatrixRect[0,0], tMatrixRect[1,0]), "You have " + tOwnership.Quantity + " " + this.InternalKey + " in your ownership!");
                if (GUI.Button(tMatrixRect[0, 1], "Select Ownership", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedData(tOwnership);
                }
            }
            else
            {
                GUI.Label(NWDGUI.AssemblyArea(tMatrixRect[0, 0], tMatrixRect[2, 0]), "You haven't ownership on " + this.InternalKey + "!");
            }
            if (GUI.Button(tMatrixRect[2, 1], "reset to zero", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.SetItemToOwnership(this, 0);
            }

            kOwnershipSetValue = EditorGUI.IntField(NWDGUI.AssemblyArea(tMatrixRect[0, 2], tMatrixRect[1, 2]), "value to set", kOwnershipSetValue);
            if (GUI.Button(tMatrixRect[2, 2], "set", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.SetItemToOwnership(this, kOwnershipSetValue);
            }
            kOwnershipAddValue = EditorGUI.IntField(NWDGUI.AssemblyArea(tMatrixRect[0, 3], tMatrixRect[1, 3]), "value to add", kOwnershipAddValue);
            if (GUI.Button(tMatrixRect[2, 3], "add", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, kOwnershipAddValue);
            }

            if (GUI.Button(tMatrixRect[0, 4], "add 1", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 1);
            }
            if (GUI.Button(tMatrixRect[1, 4], "add 10", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 10);
            }
            if (GUI.Button(tMatrixRect[2,4], "add 100", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 100);
            }
            if (GUI.Button(tMatrixRect[0, 5], "remove 1", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -1);
            }
            if (GUI.Button(tMatrixRect[1, 5], "remove 10", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -10);
            }
            if (GUI.Button(tMatrixRect[2, 5], "remove 100", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -100);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================