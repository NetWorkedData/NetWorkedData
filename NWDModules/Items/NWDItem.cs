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
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
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
        //[NWDGroupEndAttribute]
        //
        //[NWDGroupStartAttribute ("Precalculate", true, true, true)]
        //[NWDNotEditableAttribute]
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
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
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            //CheckMeFromItemGroups();
            //CheckMeFromRecipientGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            //ItemGroupList = new NWDReferencesListType<NWDItemGroup>();
            //RecipientGroupList = new NWDReferencesListType<NWDRecipientGroup>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDeleteMe()
        {
            //Debug.Log("AddonDeleteMe()");
            // do something when object will be delete from local base
            //CheckMeFromItemGroups();
            //CheckMeFromRecipientGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void CheckMeFromItemGroups()
        //{
        //    List<NWDItemGroup> tActualItemGroup = ItemGroupList.GetObjectsList();
        //    foreach (NWDItemGroup tItemGroup in NWDItemGroup.FindDatas())
        //    {
        //        if (tActualItemGroup.Contains(tItemGroup))
        //        {
        //            if (tItemGroup.ItemList.GetObjectsList().Contains(this) == true)
        //            {
        //                // ok It's contains me
        //            }
        //            else
        //            {
        //                // oh item group not contains me! WHYYYYYYYY
        //                tItemGroup.ItemList.AddObject(this);
        //                tItemGroup.UpdateData();
        //            }
        //        }
        //        else
        //        {
        //            if (tItemGroup.ItemList.GetObjectsList().Contains(this))
        //            {
        //                // Oh This ItemGroup contains me but I not refere it ... remove me from it
        //                tItemGroup.ItemList.RemoveObjects(new NWDItem[] { this });
        //                tItemGroup.UpdateData();
        //            }
        //            else
        //            {
        //                // ok i'ts not contains me!
        //            }
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void CheckMeFromRecipientGroup()
        //{
        //    List<NWDRecipientGroup> tActualRecipient = RecipientGroupList.GetObjectsList();
        //    foreach (NWDRecipientGroup tRecipient in NWDRecipientGroup.FindDatas())
        //    {
        //        if (tActualRecipient.Contains(tRecipient))
        //        {
        //            if (tRecipient.ItemList.GetObjectsList().Contains(this) == true)
        //            {
        //                // ok It's contains me
        //            }
        //            else
        //            {
        //                // oh item group not contains me! WHYYYYYYYY
        //                tRecipient.ItemList.AddObject(this);
        //                tRecipient.UpdateData();
        //            }
        //        }
        //        else
        //        {
        //            if (tRecipient.ItemList.GetObjectsList().Contains(this))
        //            {
        //                // Oh This ItemGroup contains me but I not refere it ... remove me from it
        //                tRecipient.ItemList.RemoveObjects(new NWDItem[] { this });
        //                tRecipient.UpdateData();
        //            }
        //            else
        //            {
        //                // ok i'ts not contains me!
        //            }
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipAddValue;
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipSetValue;
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {

            //Debug.Log("AddonEditor");
            // Draw the interface addon for editor
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            NWDUserOwnership tOwnership = NWDUserOwnership.FindFirstByIndex(this.Reference);


            if (tOwnership != null)
            {
                GUI.Label(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "You have " + tOwnership.Quantity + " " + this.InternalKey + "!"); tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Select Ownership", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedData(tOwnership);
                }
            }
            else
            {
                GUI.Label(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "You haven't ownership on " + this.InternalKey + "!");

            }
            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Reset to zero", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.SetItemToOwnership(this, 0);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Add 1 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 1);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Add 10 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 10);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Add 100 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, 100);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Remove 1 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -1);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Remove 10 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -10);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Remove 100 to ownsership", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, -100);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            int tAddWidth = 30;
            kOwnershipAddValue = EditorGUI.IntField(new Rect(tX, tY, tWidth - tAddWidth - NWDGUI.kFieldMarge, NWDGUI.kMiniButtonStyle.fixedHeight), " Value to add", kOwnershipAddValue);
            if (GUI.Button(new Rect(tX + tWidth - tAddWidth, tY, tAddWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Add", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.AddItemToOwnership(this, kOwnershipAddValue);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            tY += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            kOwnershipSetValue = EditorGUI.IntField(new Rect(tX, tY, tWidth - tAddWidth - NWDGUI.kFieldMarge, NWDGUI.kMiniButtonStyle.fixedHeight), " Value to set", kOwnershipSetValue);
            if (GUI.Button(new Rect(tX + tWidth - tAddWidth, tY, tAddWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Set", NWDGUI.kMiniButtonStyle))
            {
                NWDUserOwnership.SetItemToOwnership(this, kOwnershipSetValue);
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tY = 0.0f;
            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            tY += 3 * (NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);
            tY += 3 * (NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 200.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 50.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUI.Label(sRect, InternalDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
#endif

        //-------------------------------------------------------------------------------------------------------------
        //private NWDUserOwnership UserOwnershipReverse;
        //-------------------------------------------------------------------------------------------------------------
        //public void SetUserOwnership(NWDUserOwnership sUserOwnership)
        //{
        //    bool tTest = true;
        //    if (sUserOwnership != null)
        //    {
        //        if (sUserOwnership.Item.GetReference() != Reference)
        //        {
        //            // It's not the good refrence (changed?)
        //            tTest = false;
        //        }
        //        if (sUserOwnership.Account.GetReference() != NWDAccount.GetCurrentAccountReference())
        //        {
        //            // It's not the good refrence (changed?)
        //            tTest = false;
        //        }
        //        if (sUserOwnership.GameSave!=null)
        //        {
        //            if (sUserOwnership.GameSave.GetReference() != NWDGameSave.Current().Reference)
        //            {
        //                // It's not the good refrence (changed?)
        //                tTest = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        tTest = false;
        //    }
        //    if (tTest == true)
        //    {
        //        UserOwnershipReverse = sUserOwnership;
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDUserOwnership GetUserOwnership()
        //{
        //    BTBBenchmark.Start();
        //    NWDUserOwnership rReturn = UserOwnershipReverse;
        //    if (rReturn!=null)
        //    {
        //        if (rReturn.Item.GetReference() != Reference)
        //        {
        //            // It's not the good refrence (changed?)
        //            rReturn = null;
        //        }
        //        if (rReturn.Account.GetReference()!= NWDAccount.GetCurrentAccountReference())
        //        {
        //            // It's not the good refrence (changed?)
        //            rReturn = null;
        //        }
        //        if (rReturn.GameSave.GetReference() != NWDGameSave.Current().Reference)
        //        {
        //            // It's not the good refrence (changed?)
        //            rReturn = null;
        //        }
        //    }
        //    if (rReturn == null)
        //    {
        //        rReturn = NWDUserOwnership.OwnershipForItem(Reference);
        //        UserOwnershipReverse = rReturn;
        //    }
        //    BTBBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================