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
        NoNotification      =   0,
        Notification        =   1,
        SmallNotification   =   2,
        BigNotification     =   3,
    }
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
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDGroupStartAttribute("Description", true, true, true)]
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
        //[NWDTooltips("The description's panel style")]
        //public int DescriptionStyle
        //{
        //    get; set;
        //}
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList
        {
            get; set;
        }
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
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Notifications", true, true, true)]
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
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Rarity", true, true, true)]
        [NWDFloatSliderAttribute(0.0F, 1.0F)]
        [NWDEntitledAttribute("Rarity : float [0,1]")]
        public float Rarity
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Usage", true, true, true)]
        //[NWDNotEditableAttribute]
        [NWDTooltips("Item is countable or not?")]
        public bool Uncountable
        {
            get; set;
        }
        [NWDTooltips("Item is never visible by the player")]
        public bool HiddenInGame
        {
            get; set;
        }
        [NWDTooltips("Item is usable or not?")]
        public bool Usable
        {
            get; set;
        }
        public float DelayBeforeUse
        {
            get; set;
        }
        public float DurationOfUse
        {
            get; set;
        }
        public float DelayBeforeReUse
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Craft Usage", true, true, true)]
        public NWDReferencesListType<NWDItemGroup> ItemGroupList
        {
            get; set;
        }
        //public float DelayBeforeCraft
        //{
        //    get; set;
        //}
        //public float DurationOfCraft
        //{
        //    get; set;
        //}
        //public float DelayOfImmunity
        //{
        //    get; set;
        //}
        public NWDReferencesListType<NWDRecipientGroup> RecipientGroupList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Extensions", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemExtensionQuantity
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItemProperty> ItemPropertyQuantity
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Assets", true, true, true)]
        [NWDHeaderAttribute("Textures")]
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
        [NWDHeaderAttribute("Colors")]
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
        [NWDHeaderAttribute("Prefabs")]
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
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

        [NWDGroupStartAttribute("Development addons", true, true, true)]
        public string JSON
        {
            get; set;
        }
        public string KeysValues
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //[NWDGroupSeparatorAttribute]
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
                tBstart = "";
                tBend = "";
            }

            // Replace Tag by Item Name
            string rText = sText.Replace("#I" + sCpt + "#", tBstart + Name + tBend);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
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
            CheckMeFromItemGroups();
            CheckMeFromRecipientGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            ItemGroupList = new NWDReferencesListType<NWDItemGroup>();
            RecipientGroupList = new NWDReferencesListType<NWDRecipientGroup>();
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
            ItemGroupList = new NWDReferencesListType<NWDItemGroup>();
            CheckMeFromItemGroups();
            CheckMeFromRecipientGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckMeFromItemGroups()
        {
            List<NWDItemGroup> tActualItemGroup = ItemGroupList.GetObjectsList();
            foreach (NWDItemGroup tItemGroup in NWDItemGroup.FindDatas())
            {
                if (tActualItemGroup.Contains(tItemGroup))
                {
                    if (tItemGroup.ItemList.GetObjectsList().Contains(this) == true)
                    {
                        // ok It's contains me
                    }
                    else
                    {
                        // oh item group not contains me! WHYYYYYYYY
                        tItemGroup.ItemList.AddObject(this);
                        tItemGroup.UpdateData();
                    }
                }
                else
                {
                    if (tItemGroup.ItemList.GetObjectsList().Contains(this))
                    {
                        // Oh This ItemGroup contains me but I not refere it ... remove me from it
                        tItemGroup.ItemList.RemoveObjects(new NWDItem[] { this });
                        tItemGroup.UpdateData();
                    }
                    else
                    {
                        // ok i'ts not contains me!
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckMeFromRecipientGroup()
        {
            List<NWDRecipientGroup> tActualRecipient = RecipientGroupList.GetObjectsList();
            foreach (NWDRecipientGroup tRecipient in NWDRecipientGroup.FindDatas())
            {
                if (tActualRecipient.Contains(tRecipient))
                {
                    if (tRecipient.ItemList.GetObjectsList().Contains(this) == true)
                    {
                        // ok It's contains me
                    }
                    else
                    {
                        // oh item group not contains me! WHYYYYYYYY
                        tRecipient.ItemList.AddObject(this);
                        tRecipient.UpdateData();
                    }
                }
                else
                {
                    if (tRecipient.ItemList.GetObjectsList().Contains(this))
                    {
                        // Oh This ItemGroup contains me but I not refere it ... remove me from it
                        tRecipient.ItemList.RemoveObjects(new NWDItem[] { this });
                        tRecipient.UpdateData();
                    }
                    else
                    {
                        // ok i'ts not contains me!
                    }
                }
            }
        }
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
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), tWidth);

            NWDUserOwnership tOwnership = NWDUserOwnership.FindFirstByIndex(this.Reference);
            if (tOwnership != null)
            {
                GUI.Label(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "You have " + tOwnership.Quantity + " " + this.InternalKey + "!");
                tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "Add 1 to ownsership", tMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, 1);
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), "Remove 1 to ownsership", tMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, -1);
                }
                tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tY = 0.0f;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent("A"), 100);

            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
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