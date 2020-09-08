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
using System.Collections.Generic;
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
    public partial class NWDUserOwnership : NWDBasisGameSaveDependent
    {
#if NWD_EXTENSION
        [NWDInspectorGroupStart("In Extension", true, true, true)]
        [NWDInDevelopment]
        public int QuantityUsed { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Customisation ", true, true, true)]
        public string Name { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("OLD-RENAME ", true, true, true)]
        [Obsolete]
        public NWDReferencesArrayType<NWDUserOwnership> OwnershipList { get; set; }
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD ownership. This class connect the item to the account. The item is decripted in NWDItem, but some informations
    /// specific to this ownership are available only here. For example : the quantity of this item in chest, the first 
    /// acquisition statut or some particular values (A, B, C, etc.).
    /// It's a generic class for traditionla game.
    /// </summary>
    [NWDClassTrigrammeAttribute("UOW")]
    [NWDClassDescriptionAttribute("User Ownership descriptions Class")]
    [NWDClassMenuNameAttribute("User Ownership")]
    public partial class NWDUserOwnership : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
#region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Ownership", true, true, true)]
        public NWDReferenceType<NWDItem> Item { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Quantity ", true, true, true)]
        public int Quantity { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Acquisition", true, true, true)]
        public bool Discovered { get; set; }
        public NWDDateTimeType DiscoveredDate { get; set; }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Quantities for item's reference if exists.
        /// </summary>
        /// <returns><c>true</c>, if for item reference exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItemReference">item reference.</param>
        public static int QuantityForItem(string sItemReference)
        {
            NWDUserOwnership rOwnership = FindReachableByItemReference(sItemReference);
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
            NWDUserOwnership rOwnership = FindReachableByItemReference(sItemReference);
            return rOwnership.Discovered;
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
            if (Discovered == false)
            {
                DiscoveredDate.SetDateTime(DateTime.Now);
                Discovered = true;
                NWDItem tItem = Item.GetRawData();
                if (tItem != null && tItem.FirstAcquisitionNotification != NWDItemNotification.NoNotification)
                {
                    NWENotificationManager.SharedInstance().PostNotification(tItem, NWDItem.K_FirstAcquisitionNotificationKey);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UncountableVerify(NWDItem sItem)
        {
            if (sItem.Uncountable == true)
            {
                if (Quantity > 0)
                {
                    Quantity = 1;
                }
                else
                {
                    Quantity = 0;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> OwnershipIntersection(List<NWDItem> sItemsList, int sQuantity = 1)
        {
            List<NWDItem> rReturn = new List<NWDItem>();
            foreach (NWDItem tItem in sItemsList)
            {
                if (tItem != null)
                {
                    if (ContainsItem(tItem, sQuantity))
                    {
                        rReturn.Add(tItem);
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership SetItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDUserOwnership rOwnershipToUse = FindReachableByItem(sItem);
            int tOldQuantity = rOwnershipToUse.Quantity;
            rOwnershipToUse.Quantity = sQuantity;
            rOwnershipToUse.UncountableVerify(sItem);
            rOwnershipToUse.FirstAcquisitionMethod();
            if (sQuantity != 0)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
            }
            if (sItem != null && sItem.AddItemNotification != NWDItemNotification.NoNotification && sQuantity > tOldQuantity)
            {
                NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_AddNotificationKey);
            }
            if (sItem != null && sItem.RemoveItemNotification != NWDItemNotification.NoNotification && sQuantity < tOldQuantity)
            {
                NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_RemoveNotificationKey);
            }
            if (sItem != null && sItem.NoMoreItemNotification != NWDItemNotification.NoNotification && rOwnershipToUse.Quantity <= 0)
            {
                NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_NoMoreNotificationKey);
            }
            rOwnershipToUse.UpdateData();
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsDiscovered(NWDItem sItem)
        {
            NWDUserOwnership rOwnershipToUse = FindReachableByItem(sItem);
            return rOwnershipToUse.Discovered;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetDiscovered(NWDItem sItem, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            NWDUserOwnership rOwnershipToUse = FindReachableByItem(sItem);
            if (rOwnershipToUse.Discovered == false)
            {
                rOwnershipToUse.FirstAcquisitionMethod();
                rOwnershipToUse.UpdateData(true, sWritingMode);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership AddItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDUserOwnership rOwnershipToUse = null;
            if (sItem != null)
            {
                rOwnershipToUse = FindReachableByItem(sItem);
                rOwnershipToUse.Quantity += sQuantity;
                rOwnershipToUse.UncountableVerify(sItem);
                rOwnershipToUse.FirstAcquisitionMethod();
                if (sItem != null && sItem.AddItemNotification != NWDItemNotification.NoNotification && sQuantity > 0)
                {
                    NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_AddNotificationKey);
                }
                if (sItem != null && sItem.RemoveItemNotification != NWDItemNotification.NoNotification && sQuantity < 0)
                {
                    NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_RemoveNotificationKey);
                }
                if (sItem != null && sItem.NoMoreItemNotification != NWDItemNotification.NoNotification && rOwnershipToUse.Quantity <= 0)
                {
                    NWENotificationManager.SharedInstance().PostNotification(sItem, NWDItem.K_NoMoreNotificationKey);
                }
                rOwnershipToUse.UpdateData();
            }
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership RemoveItemToOwnership(NWDItem sItem, int sQuantity)
        {
            return AddItemToOwnership(sItem, -sQuantity);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsAndQuantity)
        {
            if (sItemsAndQuantity != null)
            {
                foreach (KeyValuePair<string, int> tQte in sItemsAndQuantity.GetReferenceAndQuantity())
                {
                    NWDItem tItem = NWDBasisHelper.GetRawDataByReference<NWDItem>(tQte.Key);
                    AddItemToOwnership(tItem, tQte.Value);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsAndQuantity)
        {
            if (sItemsAndQuantity != null)
            {
                foreach (KeyValuePair<string, int> tQte in sItemsAndQuantity.GetReferenceAndQuantity())
                {
                    NWDItem tItem = NWDBasisHelper.GetRawDataByReference<NWDItem>(tQte.Key);
                    AddItemToOwnership(tItem, -tQte.Value);
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
                    foreach (KeyValuePair<NWDItem, int> tItemQuantity in sItemsReferenceQuantity.GetReachableDatasAndQuantities())
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
                NWDUserOwnership rOwnershipToUse = FindReachableByItem(sItem);
                if (sItem.Uncountable == false)
                {
                    if (rOwnershipToUse.Quantity < sQuantity)
                    {
                        rReturn = false;
                    }
                    else if (sQuantity == 0 && rOwnershipToUse.Quantity > 0)
                    {
                        rReturn = false;
                    }
                }
                else
                {
                    if (rOwnershipToUse.Quantity < 1)
                    {
                        rReturn = false;
                    }
                    else if (sQuantity == 0 && rOwnershipToUse.Quantity > 0)
                    {
                        rReturn = false;
                    }
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
                    foreach (KeyValuePair<NWDItemGroup, int> tItemQuantity in sItemGroupsReferenceQuantity.GetReachableDatasAndQuantities())
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
                foreach (NWDItem tItem in sItemGroup.ItemList.GetReachableDatas())
                {
                    if (tItem.Uncountable == true)
                    {
                        if (sQuantity > 0)
                        {
                            sQuantity = 1;
                        }
                        else
                        {
                            sQuantity = 0;
                        }
                    }
                    NWDUserOwnership tOwnership = FindReachableByItem(tItem);
                    tQ = tQ + tOwnership.Quantity;
                    if (tQ >= sQuantity)
                    {
                        if (sQuantity >= 0)
                        {
                            rReturn = true;
                            //break; // must continue to check uncountable items
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
            //NWDBenchmark.Start();
            bool rReturn = true;
            if (sConditional.Reference != null)
            {
                NWDUserOwnership rOwnershipToUse = FindReachableByItemReference(sConditional.Reference);
                rReturn = sConditional.isValid(rOwnershipToUse.Quantity);
            }
            //NWDBenchmark.Finish();
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
            NWDItemGroup tItemGroup = NWDBasisHelper.GetRawDataByReference<NWDItemGroup>(sConditional.Reference);
            if (tItemGroup != null)
            {
                rReturn = false;
                int tQ = 0;
                foreach (NWDItem tItem in tItemGroup.ItemList.GetReachableDatas())
                {
                    NWDUserOwnership tOwnership = FindReachableByItem(tItem);
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
            Discovered = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool CheckOwnershipAndItemValidity()
        {
            bool rReturn = false;
            NWDItem tNWDItem = Item.GetRawData();
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
#endregion
        //-------------------------------------------------------------------------------------------------------------
#region NetWorkedData addons methods
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
        public override void AddonEditor(Rect sRect)
        {
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;
            //GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            //tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            //GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            //tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            //GUIStyle tLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            //tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            // draw line 

            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;

            EditorGUI.LabelField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), "Tools box", NWDGUI.kLabelStyle);
            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            // Draw the interface addon for editor
            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = Account.GetReference();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_FILTER))
            {
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
                {
                    NWDBasisHelper.FindTypeInfos(tType).m_SearchGameSave = GameSave.GetReference();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                }
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            float tY = 0.0f;
            tY += NWDGUI.kFieldMarge * 2;
            tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
