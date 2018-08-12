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
    //-------------------------------------------------------------------------------------------------------------
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("OWS")]
    [NWDClassDescriptionAttribute("User Ownership descriptions Class")]
    [NWDClassMenuNameAttribute("User Ownership")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDUserOwnership : NWDBasis<NWDUserOwnership>
    {
        //-------------------------------------------------------------------------------------------------------------
        // Create And Index
        //-------------------------------------------------------------------------------------------------------------
        static NWDWritingMode kWritingMode = NWDWritingMode.MainThread;
        static Dictionary<string, List<NWDUserOwnership>> kIndex = new Dictionary<string, List<NWDUserOwnership>>();
        private List<NWDUserOwnership> kIndexList;
        // lors du changement si kIndexList !=  de kIndexItemReverse[item.ref + gamesave.ref] => on a changer l'item ou le gamesave 
        // je retire de la kIndexList et je cherche la nuvelle kIndexList et je la memorise et la rajoute
        //-------------------------------------------------------------------------------------------------------------
        private void InsertInIndex()
        {
            //Debug.Log("InsertInIndex reference =" + Reference);
            //BTBBenchmark.Start();
            if (Item.GetReference() != null
                && GameSave!=null
                && GameSave.GetReference() != null  // permet aussi d'avoir indirectement l'account
                && IsEnable() == true
                && IsTrashed() == false
                && TestIntegrity() == true)
            {
                string tKey = Item.GetReference() + "*" + GameSave.GetReference();
                if (kIndexList != null)
                {
                    // I have allready index
                    if (kIndex.ContainsKey(tKey))
                    {
                        if (kIndex[tKey] == kIndexList)
                        {
                            // I am in the good index ... do nothing
                        }
                        else
                        {
                            // I Changed index! during update ?!!
                            kIndexList.Remove(this);
                            kIndexList = null;
                            kIndexList = kIndex[tKey];
                            kIndexList.Add(this);
                        }
                    }
                    else
                    {
                        kIndexList.Remove(this);
                        kIndexList = null;
                        kIndexList = new List<NWDUserOwnership>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
                else
                {
                    // I need add in index!
                    if (kIndex.ContainsKey(tKey))
                    {
                        // index exists
                        kIndexList = kIndex[tKey];
                        kIndexList.Add(this);
                    }
                    else
                    {
                        // index must be create
                        kIndexList = new List<NWDUserOwnership>();
                        kIndex.Add(tKey, kIndexList);
                        kIndexList.Add(this);
                    }
                }
            }
            else
            {
                RemoveFromIndex();
            }

            //string tTdbug = "";
            //tTdbug += " ################";
            //tTdbug += "InsertInIndex reference =" + Reference+" for item "+Item.GetReference()+" \n";
            //foreach (KeyValuePair<string, List<NWDUserOwnership>> tkeyvalue in kIndex)
            //{
            //    tTdbug += " key = " + tkeyvalue.Key + " : ";
            //    foreach (NWDUserOwnership tOwner in tkeyvalue.Value)
            //    {
            //        tTdbug += " " + tOwner.Reference + "(" + tOwner.GameSave.GetReference() + ")";
            //    }
            //    tTdbug += "\n";
            //}
            //tTdbug += " ################";
            //Debug.Log(tTdbug);

            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveFromIndex()
        {
            if (kIndexList != null)
            {
                kIndexList.Contains(this);
                {
                    kIndexList.Remove(this);
                }
                kIndexList = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserOwnership> FindByIndex(NWDItem sItem, NWDGameSave sGameSave)
        {
            //BTBBenchmark.Start();
            List<NWDUserOwnership> rReturn = null;
            if (sItem != null && sGameSave != null)
            {
                string tKey = sItem.Reference + "*" + sGameSave.Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public List<NWDUserOwnership> FindByIndex(string sItemreference)
        {
            //BTBBenchmark.Start();
            List<NWDUserOwnership> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDUserOwnership FindFirstByIndex(string sItemreference)
        {
            //BTBBenchmark.Start();
            NWDUserOwnership rObject = null;
            List<NWDUserOwnership> rReturn = null;
            if (sItemreference != null)
            {
                string tKey = sItemreference + "*" + NWDGameSave.Current().Reference;
                if (kIndex.ContainsKey(tKey))
                {
                    rReturn = kIndex[tKey];
                }
            }
            if (rReturn != null)
            {
                if (rReturn.Count > 0)
                {
                    rObject = rReturn[0];
                }
            }
            //BTBBenchmark.Finish();
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Ownership", true, true, true)] // ok
        public bool FirstAcquisition
        {
            get; set;
        }
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> Item
        {
            get; set;
        }
        public int Quantity
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Extensions", true, true, true)]
        public NWDReferencesListType<NWDUserOwnership> OwnershipList
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItemProperty> ItemPropertyQuantity
        {
            get; set;
        }
        [NWDGroupEnd]

        [NWDGroupSeparator]

        [NWDGroupStart("Development addons", true, true, true)]
        public string JSON
        {
            get; set;
        }
        public string KeysValues
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership()
        {
            //Debug.Log("NWDOwnership Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDOwnership Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------

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
            //BTBBenchmark.Start();
            //Debug.Log("NWDUserOwnership OwnershipForItem("+sItemReference+")");
            NWDUserOwnership rOwnership = FindFirstByIndex(sItemReference);

            //if (rOwnership == null)
            //{
            //    foreach (NWDUserOwnership tOwnership in FindDatas())
            //    {
            //        //Debug.Log("NWDUserOwnership OwnershipForItem   test " + tOwnership.Reference + " for item " + tOwnership.Item.GetReference());
            //        if (tOwnership.Item.GetReference() == sItemReference)
            //        {
            //            rOwnership = tOwnership;
            //            break;
            //        }
            //    }
            //}
            if (rOwnership == null)
            {
                //Debug.Log("NWDUserOwnership OwnershipForItem(" + sItemReference + ") NEED NEW OWNERSHIP");
                rOwnership = NewData(kWritingMode);
                //--------------
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
                //--------------
                rOwnership.Item.SetReference(sItemReference);
                rOwnership.Tag = NWDBasisTag.TagUserCreated;
                rOwnership.Quantity = 0;
                rOwnership.UpdateData(true, kWritingMode);
            }
            //BTBBenchmark.Finish();
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
            if (sItem != null)
            {
                return OwnershipForItem(sItem.Reference);
            }
            else
            {
                return null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Quantities for item's reference if exists.
        /// </summary>
        /// <returns><c>true</c>, if for item reference exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItemReference">item reference.</param>
        public static int QuantityForItem(string sItemReference)
        {//BTBBenchmark.Start();
            NWDUserOwnership rOwnership = OwnershipForItem(sItemReference);
            int rQte = 0;
            if (rOwnership != null)
            {
                rQte = rOwnership.Quantity;
            }
            //foreach (NWDUserOwnership tOwnership in FindDatas())
            //{
            //    if (tOwnership.Item.GetReference() == sItemReference)
            //    {
            //        rQte = tOwnership.Quantity;
            //        break;
            //    }
            //}
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            NWDUserOwnership rOwnership = OwnershipForItem(sItemReference);
            //foreach (NWDUserOwnership tOwnership in FindDatas())
            //{
            //    if (tOwnership.Item.GetReference() == sItemReference)
            //    {
            //        rOwnership = tOwnership;
            //        break;
            //    }
            //}
            //BTBBenchmark.Finish();
            return rOwnership != null;
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
        public static NWDUserOwnership SetItemToOwnership(NWDItem sItem, int sQuantity)
        {
            //BTBBenchmark.Start();
            NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity = sQuantity;
            rOwnershipToUse.UpdateData();
            //BTBBenchmark.Finish();
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership AddItemToOwnership(NWDItem sItem, int sQuantity, bool sIsIncrement = true)
        {
            //BTBBenchmark.Start();
            NWDUserOwnership rOwnership = OwnershipForItem(sItem);
            if (sIsIncrement)
            {
                rOwnership.Quantity += sQuantity;
            }
            else
            {
                rOwnership.Quantity = sQuantity;
            }
            rOwnership.UpdateData();
            //BTBBenchmark.Finish();
            return rOwnership;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership RemoveItemToOwnership(NWDItem sItem, int sQuantity)
        {
            //BTBBenchmark.Start();
            NWDUserOwnership rOwnership = OwnershipForItem(sItem);
            rOwnership.Quantity -= sQuantity;
            rOwnership.UpdateData();
            //BTBBenchmark.Finish();
            return rOwnership;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AddItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            //BTBBenchmark.Start();
            //Debug.Log("AddItemToOwnership ("+sItemsReferenceQuantity.Value+");");
            if (sItemsReferenceQuantity != null)
            {
                foreach (KeyValuePair<string, int> tItemQuantity in sItemsReferenceQuantity.GetReferenceAndQuantity())
                {
                    NWDUserOwnership rOwnershipToUse = OwnershipForItem(tItemQuantity.Key);
                    rOwnershipToUse.Quantity += tItemQuantity.Value;
                    rOwnershipToUse.UpdateData();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            //BTBBenchmark.Start();
            if (sItemsReferenceQuantity != null)
            {
                foreach (KeyValuePair<string, int> tItemQuantity in sItemsReferenceQuantity.GetReferenceAndQuantity())
                {
                    NWDUserOwnership rOwnershipToUse = OwnershipForItem(tItemQuantity.Key);
                    rOwnershipToUse.Quantity -= tItemQuantity.Value;
                    rOwnershipToUse.UpdateData();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ContainsItems(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sItemsReferenceQuantity != null)
            {
                if (sItemsReferenceQuantity.IsEmpty())
                {
                    // No test needed
                }
                else
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
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ContainsItem(NWDItem sItem, int sQuantity)
        {
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sItem != null)
            {
                NWDUserOwnership rOwnershipToUse = OwnershipForItem(sItem);
                if (rOwnershipToUse.Quantity < sQuantity)
                {
                    rReturn = false;
                }
                if (sQuantity == 0 && rOwnershipToUse.Quantity > 0)
                {
                    rReturn = false;
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroups(NWDReferencesQuantityType<NWDItemGroup> sItemGroupsReferenceQuantity)
        {
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sItemGroupsReferenceQuantity != null)
            {
                if (sItemGroupsReferenceQuantity.IsEmpty())
                {
                    // No test needed
                }
                else
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
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroup(NWDItemGroup sItemGroup, int sQuantity)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConditionalItems(NWDReferencesConditionalType<NWDItem> sItemsReferenceConditional)
        {
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sItemsReferenceConditional != null)
            {
                if (sItemsReferenceConditional.IsEmpty())
                {
                    // No test needed
                }
                else
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
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            bool rReturn = true;
            if (sItemGroupsReferenceQuantity != null)
            {
                if (sItemGroupsReferenceQuantity.IsEmpty())
                {
                    // No test needed
                }
                else
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
            }//BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ConditionalItemGroup(NWDReferenceConditionalType<NWDItemGroup> sConditional)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            FirstAcquisition = true;
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
            //NWDItem tItem = Item.GetObject();
            //if (tItem != null)
            //{
            //    tItem.SetUserOwnership(this);
            //}
            InsertInIndex();
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
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            InsertInIndex();
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
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
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
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================