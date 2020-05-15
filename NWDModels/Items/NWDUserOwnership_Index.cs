//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserOwnershipIndexer : NWDIndexer
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDIndex<NWDItem, NWDUserOwnership> Index = new NWDIndex<NWDItem, NWDUserOwnership>();
        //-------------------------------------------------------------------------------------------------------------
        static public void Install()
        {
            Debug.Log("NWDUserOwnershipIndexer Install()");
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserOwnership));
            tHelper.IndexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("InsertInLevelIndex"));
            tHelper.DeindexInMemoryMethodList.Add(tHelper.ClassType.GetMethod("RemoveFromLevelIndex"));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserOwnership FindReachableUserOwnerShip(bool sOrCreate = true)
        {
            return NWDUserOwnership.FindReachableByItem(this, sOrCreate);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserOwnership : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        //[NWDIndexInMemory]
        public void InsertInLevelIndex()
        {
            Debug.Log(BasisHelper().ClassNamePHP + " InsertInLevelIndex()");
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Item.GetReference() + NWDConstants.kFieldSeparatorA + GameSave.GetReference();
                NWDUserOwnershipIndexer.Index.UpdateData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDDeindexInMemory]
        public void RemoveFromLevelIndex()
        {
            Debug.Log(BasisHelper().ClassNamePHP +" RemoveFromLevelIndex()");
            // Remove from the actual indexation
            NWDUserOwnershipIndexer.Index.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindReachableByItem(NWDItem sKey, bool sOrCreate = true)
        {
            return FindReachableByItemReference(sKey.Reference, sOrCreate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindReachableByItemReference(string sReference, bool sOrCreate = true)
        {
            string tKey = sReference + NWDConstants.kFieldSeparatorA + NWDGameSave.CurrentData().Reference;
            NWDUserOwnership rReturn = NWDUserOwnershipIndexer.Index.FirstRawDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDUserOwnership>();
#if UNITY_EDITOR
                NWDItem tItem = NWDBasisHelper.GetRawDataByReference<NWDItem>(sReference);
                if (tItem != null)
                {
                    if (tItem.Name != null)
                    {
                        string tItemNameBase = tItem.Name.GetBaseString();
                        if (tItemNameBase != null)
                        {
                            rReturn.InternalKey = tItemNameBase;
                        }
                    }
                }
                rReturn.InternalDescription = NWDUserNickname.GetNickname();

#endif
                rReturn.Item.SetReference(sReference);
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.Quantity = 0;
                rReturn.UpdateData();
            }
#if UNITY_EDITOR
            NWDItem tItemForPreview = NWDBasisHelper.GetRawDataByReference<NWDItem>(sReference);
            if (tItemForPreview != null)
            {
                rReturn.Preview = tItemForPreview.Preview;
                rReturn.UpdateDataIfModified();
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership[] FindReachableByItemCategory(NWDCategory sCategory, bool sOrCreate = true)
        {
            List<NWDUserOwnership> rReturn = new List<NWDUserOwnership>();
            foreach (NWDItem tItem in NWDItem.FindByCategoryInverse(sCategory))
            {
                NWDUserOwnership tAdd = FindReachableByItemReference(tItem.Reference, sOrCreate);
                if (tAdd != null)
                {
                    rReturn.Add(tAdd);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================