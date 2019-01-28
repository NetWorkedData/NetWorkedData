//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
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
    public class NWDUserOwnershipConnection : NWDConnection<NWDUserOwnership>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD ownership. This class connect the item to the account. The item is decripted in NWDItem, but some informations
    /// specific to this ownership are available only here. For example : the quantity of this item in chest, the first 
    /// acquisition statut or some particular values (A, B, C, etc.).
    /// It's a generic class for traditionla game.
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("OWS")]
    [NWDClassDescriptionAttribute("User Ownership descriptions Class")]
    [NWDClassMenuNameAttribute("User Ownership")]
    public partial class NWDUserOwnership : NWDBasis<NWDUserOwnership>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Ownership", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        public NWDReferenceType<NWDItem> Item { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Acquisition", true, true, true)]
        public bool FirstAcquisitionNotify { get; set; }
        public NWDDateTimeType FirstAcquisitionDate { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Extensions", true, true, true)]
        public NWDReferencesArrayType<NWDUserOwnership> OwnershipList { get; set; }
        public NWDReferencesQuantityType<NWDItemProperty> ItemPropertyQuantity { get; set; }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Development addons", true, true, true)]
        public string JSON { get; set; }
        public string KeysValues { get; set; }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDItem) };
        }
        //-------------------------------------------------------------------------------------------------------------
        // OWNERSHIP AND ITEM FOR PLAYER
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item's reference.
        /// </summary>
        /// <returns>The ownership.</returns>
        /// <param name="sItemReference">S item reference.</param>
        public static NWDUserOwnership OwnershipForItem(string sItemReference)
        {
            NWDUserOwnership rOwnership = FindFirstByIndex(sItemReference);

            if (rOwnership == null)
            {
                rOwnership = NewData(kWritingMode);
                #if UNITY_EDITOR
                NWDItem tItem = NWDItem.GetDataByReference(sItemReference);
                if (tItem != null)
                {
                    if (tItem.Name != null)
                    {
                        string tItemNameBase = tItem.Name.GetBaseString();
                        if (tItemNameBase != null)
                        {
                            rOwnership.InternalKey = tItemNameBase;
                        }
                    }
                }
                rOwnership.InternalDescription = NWDAccountNickname.GetNickname();
                #endif
                rOwnership.Item.SetReference(sItemReference);
                rOwnership.Tag = NWDBasisTag.TagUserCreated;
                rOwnership.Quantity = 0;
                rOwnership.UpdateData(true, kWritingMode);
            }
            return rOwnership;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the Ownership for selected item.
        /// </summary>
        /// <returns>The ownership.</returns>
        /// <param name="sItem">selected item.</param>
        public static NWDUserOwnership OwnershipForItem(NWDItem sItem)
        {
            return sItem != null ? OwnershipForItem(sItem.Reference) : null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Quantities for item's reference if exists.
        /// </summary>
        /// <returns><c>true</c>, if for item reference exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItemReference">item reference.</param>
        public static int QuantityForItem(string sItemReference)
        {
            NWDUserOwnership rOwnership = OwnershipForItem(sItemReference);
            int rQte = 0;
            if (rOwnership != null)
            {
                rQte = rOwnership.Quantity;
            }
            return rQte;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item's reference exists.
        /// </summary>
        /// <returns><c>true</c>, if for item reference exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItemReference">item reference.</param>
        public static bool OwnershipForItemExists(string sItemReference)
        {
            NWDUserOwnership rOwnership = OwnershipForItem(sItemReference);
            return rOwnership.FirstAcquisitionNotify;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item exists.
        /// </summary>
        /// <returns><c>true</c>, if for item exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItem">S item.</param>
        public static bool OwnershipForItemExists(NWDItem sItem)
        {
            return OwnershipForItemExists(sItem.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void FirstAcquisitionMethod()
        {
            if (FirstAcquisitionNotify == false)
            {
                FirstAcquisitionDate.SetDateTime(DateTime.Now);
                FirstAcquisitionNotify = true;
                NWDItem tItem = Item.GetObject();
                if (tItem != null && tItem.FirstAcquisitionNotification!= NWDItemNotification.NoNotification)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_FirstAcquisitionNotificationKey);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership SetItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity = sQuantity;
            if (sQuantity != 0)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
            }
            rOwnershipToUse.UpdateData();
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsDiscovered(NWDItem sItem)
        {
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            return rOwnershipToUse.FirstAcquisitionNotify;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetDiscovered(NWDItem sItem, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            if (rOwnershipToUse.FirstAcquisitionNotify == false)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
                rOwnershipToUse.UpdateData(true, sWritingMode);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership AddItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity += sQuantity;
            if (sQuantity > 0)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
            }
            rOwnershipToUse.UpdateData();

            NWDItem tItem = rOwnershipToUse.Item.GetObject();
            if (tItem != null && tItem.AddItemNotification != NWDItemNotification.NoNotification)
            {
                BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_AddNotificationKey);
            }
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsAndQuantity)
        {
            if (sItemsAndQuantity != null)
            {
                foreach (KeyValuePair<string, int> tQte in sItemsAndQuantity.GetReferenceAndQuantity())
                {
                    NWDUserOwnership rOwnershipToUse = OwnershipForItem(tQte.Key);
                    rOwnershipToUse.Quantity += tQte.Value;
                    if (rOwnershipToUse.Quantity > 0)
                    {
                        rOwnershipToUse.FirstAcquisitionMethod();
                    }
                    rOwnershipToUse.UpdateData();
                    NWDItem tItem = rOwnershipToUse.Item.GetObject();
                    if (tItem != null && tItem.AddItemNotification != NWDItemNotification.NoNotification)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_AddNotificationKey);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership RemoveItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity -= sQuantity;
            if (rOwnershipToUse.Quantity < 0)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
            }
            rOwnershipToUse.UpdateData();
            NWDItem tItem = rOwnershipToUse.Item.GetObject();
            if (tItem != null && tItem.RemoveItemNotification != NWDItemNotification.NoNotification)
            {
                BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_RemoveNotificationKey);
            }
            if (tItem != null && tItem.NoMoreItemNotification != NWDItemNotification.NoNotification && rOwnershipToUse.Quantity <= 0)
            {
                BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_NoMoreNotificationKey);
            }
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsAndQuantity)
        {
            if (sItemsAndQuantity != null)
            {
                foreach (KeyValuePair<string, int> tQte in sItemsAndQuantity.GetReferenceAndQuantity())
                {
                    NWDUserOwnership rOwnershipToUse = OwnershipForItem(tQte.Key);
                    rOwnershipToUse.Quantity -= tQte.Value;
                    if (rOwnershipToUse.Quantity < 0)
                    {
                        rOwnershipToUse.FirstAcquisitionMethod();
                    }
                    rOwnershipToUse.UpdateData();
                    NWDItem tItem = rOwnershipToUse.Item.GetObject();
                    if (tItem != null && tItem.RemoveItemNotification != NWDItemNotification.NoNotification)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_RemoveNotificationKey);
                    }
                    if (tItem != null && tItem.NoMoreItemNotification != NWDItemNotification.NoNotification && rOwnershipToUse.Quantity <= 0)
                    {
                        BTBNotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_NoMoreNotificationKey);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ContainsItems(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            bool rReturn = true;
            if (sItemsReferenceQuantity != null)
            {
                if (sItemsReferenceQuantity.IsNotEmpty())
                {
                    foreach (KeyValuePair<NWDItem, int> tItemQuantity in sItemsReferenceQuantity.GetObjectAndQuantity())
                    {
                        if (ContainsItem(tItemQuantity.Key, tItemQuantity.Value) == false)
                        {
                            rReturn = false;
                            break;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ContainsItem(NWDItem sItem, int sQuantity)
        {
            bool rReturn = true;
            if (sItem != null)
            {
                NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
                if (rOwnershipToUse.Quantity < sQuantity)
                {
                    rReturn = false;
                }
                else if (sQuantity == 0 && rOwnershipToUse.Quantity > 0)
                {
                    rReturn = false;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroups(NWDReferencesQuantityType<NWDItemGroup> sItemGroupsReferenceQuantity)
        {
            bool rReturn = true;
            if (sItemGroupsReferenceQuantity != null)
            {
                if (sItemGroupsReferenceQuantity.IsNotEmpty())
                {
                    foreach (KeyValuePair<NWDItemGroup, int> tItemQuantity in sItemGroupsReferenceQuantity.GetObjectAndQuantity())
                    {
                        if (ContainsItemGroup(tItemQuantity.Key, tItemQuantity.Value) == false)
                        {
                            rReturn = false;
                            break;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroup(NWDItemGroup sItemGroup, int sQuantity)
        {
            bool rReturn = true;
            if (sItemGroup != null)
            {
                rReturn = false;
                int tQ = 0;
                foreach (NWDItem tItem in sItemGroup.ItemList.GetObjects())
                {
                    NWDUserOwnership tOwnership = OwnershipForItem(tItem);
                    tQ = tQ + tOwnership.Quantity;
                    if (tQ >= sQuantity)
                    {
                        if (sQuantity >= 0)
                        {
                            rReturn = true;
                            break;
                        }
                    }
                }
                if (sQuantity == 0 && tQ > 0)
                {
                    rReturn = false;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConditionalItems(NWDReferencesConditionalType<NWDItem> sItemsReferenceConditional)
        {
            bool rReturn = true;
            if (sItemsReferenceConditional != null)
            {
                if (sItemsReferenceConditional.IsNotEmpty())
                {
                    foreach (NWDReferenceConditionalType<NWDItem> tTest in sItemsReferenceConditional.GetReferenceQuantityConditional())
                    {
                        if (ConditionalItem(tTest) == false)
                        {
                            rReturn = false;
                            break;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConditionalItem(NWDReferenceConditionalType<NWDItem> sConditional)
        {
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sConditional.Reference != null)
            {
                NWDUserOwnership rOwnershipToUse = OwnershipForItem(sConditional.Reference);
                rReturn = sConditional.isValid(rOwnershipToUse.Quantity);
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ConditionalItemGroups(NWDReferencesConditionalType<NWDItemGroup> sItemGroupsReferenceQuantity)
        {
            bool rReturn = true;
            if (sItemGroupsReferenceQuantity != null)
            {
                if (sItemGroupsReferenceQuantity.IsNotEmpty())
                {
                    foreach (NWDReferenceConditionalType<NWDItemGroup> tTest in sItemGroupsReferenceQuantity.GetReferenceQuantityConditional())
                    {
                        if (ConditionalItemGroup(tTest) == false)
                        {
                            rReturn = false;
                            break;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ConditionalItemGroup(NWDReferenceConditionalType<NWDItemGroup> sConditional)
        {
            bool rReturn = true;
            NWDItemGroup tItemGroup = NWDItemGroup.FindDataByReference(sConditional.Reference);
            if (tItemGroup != null)
            {
                rReturn = false;
                int tQ = 0;
                foreach (NWDItem tItem in tItemGroup.ItemList.GetObjects())
                {
                    NWDUserOwnership tOwnership = OwnershipForItem(tItem);
                    tQ = tQ + tOwnership.Quantity;
                }
                // I Got the quantity
                rReturn = sConditional.isValid(tQ);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            FirstAcquisitionNotify = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool CheckOwnershipAndItemValidity()
        {
            bool rReturn = false;

            NWDItem tNWDItem = Item.GetObject();

            // Check if item is not null
            if (tNWDItem != null)
            {
                // Check if item is enable
                if (tNWDItem.IsEnable() && IsEnable())
                {
                    rReturn = true;
                }
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonLoadedMe()
        {
            // do something when object is loaded
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            //InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // not insert in index because integrity is not reevaluate!
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
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
        /// <summary>
        /// Addon method when untrahs me. Can be ovverride in herited Class.
        /// </summary>
        public override void AddonDeleteMe()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonIndexMe()
        {
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            RemoveFromIndex();
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
                //TODO: do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            // draw line 
            EditorGUI.DrawRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1), NWDConstants.kRowColorLine);
            tY += NWDConstants.kFieldMarge * 2;

            EditorGUI.LabelField(new Rect(tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
            tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
            // Draw the interface addon for editor
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDDatas.FindTypeInfos(tType).m_SearchAccount = Account.GetReference();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY, tWidth, tMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                {
                    NWDDatas.FindTypeInfos(tType).m_SearchGameSave = GameSave.GetReference();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }

            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================