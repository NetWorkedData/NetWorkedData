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
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserOwnershipIndexer : NWDIndexer<NWDUserOwnership>
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDIndex<NWDItem, NWDUserOwnership> Index = new NWDIndex<NWDItem, NWDUserOwnership>();
        //-------------------------------------------------------------------------------------------------------------
        public static string KeyToUse(NWDUserOwnership sData)
        {
            return sData.Item.GetReference() + NWDConstants.kFieldSeparatorA + sData.GameSave.GetReference();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string KeyToUse(string sData)
        {
            return sData + NWDConstants.kFieldSeparatorA + NWDGameSave.CurrentData().Reference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper IndexData()</color>");
            NWDUserOwnership tUserOwnership = sData as NWDUserOwnership;
            string tKey = KeyToUse(tUserOwnership);
            Index.UpdateData(tUserOwnership, tKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper DeindexData()</color>");
            NWDUserOwnership tUserOwnership = sData as NWDUserOwnership;
            Index.RemoveData(tUserOwnership);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDUserOwnership> FindDatas(string sKey)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper FindDatas()</color>");
            string tKey = KeyToUse(sKey);
            List<NWDUserOwnership> rReturn = new List<NWDUserOwnership>(Index.RawDatasByKey(tKey));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindFirstData(string sKey)
        {
            string tKey = KeyToUse(sKey);
            //Debug.Log("<color=orange>### NWDBasisHelper FindFirstData()</color> " + tKey);
            NWDUserOwnership rReturn = Index.FirstRawDataByKey(tKey);
            return rReturn;
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
        public static NWDUserOwnership FindReachableByItem(NWDItem sKey, bool sOrCreate = true)
        {
            return FindReachableByItemReference(sKey.Reference, sOrCreate);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserOwnership FindReachableByItemReference(string sReference, bool sOrCreate = true)
        {
            NWDUserOwnership rReturn = NWDUserOwnershipIndexer.FindFirstData(sReference);
            if (rReturn == null)
            {
                if (sOrCreate == true)
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
#if NWD_USER_IDENTITY
                    rReturn.InternalDescription = NWDUserNickname.GetNickname();
#endif
#endif
                    rReturn.Item.SetReference(sReference);
                    rReturn.Tag = NWDBasisTag.TagUserCreated;
                    rReturn.Quantity = 0;
                    rReturn.UpdateData();
#if UNITY_EDITOR
                    rReturn.InternalKey = NWDUserOwnershipIndexer.KeyToUse(sReference);
                    rReturn.InternalDescription = NWDUserOwnershipIndexer.KeyToUse(rReturn);
#endif
                }
            }

#if UNITY_EDITOR
            if (rReturn != null)
            {
                NWDItem tItemForPreview = NWDBasisHelper.GetRawDataByReference<NWDItem>(sReference);
                if (tItemForPreview != null)
                {
                    rReturn.Preview = tItemForPreview.Preview;
                    rReturn.UpdateDataIfModified();
                }
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if NWD_CLASSIFICATION
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
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
