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
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDIndexerType : int
    {
        InMemory,
        InBase,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        private static void AddIndexMethod()
        {
            //Debug.Log("<color=orange>### NWDLauncher AddIndexMethod()</color>");
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.IndexMethodStart;
            Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllIndexerTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisIndexer)) select type).ToArray();
            foreach (Type tType in tAllIndexerTypes)
            {
                if (tType != typeof(NWDIndexer<>))
                {
                    NWDBasisIndexer tIndexer = (NWDBasisIndexer)Activator.CreateInstance(tType);
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tIndexer.GetDataClass());
                    if (tIndexer.GetIndexerType() == NWDIndexerType.InMemory)
                    {
                        tHelper.IndexerInMemoryList.Add(tIndexer);
                    }
                    else
                    {
                        tHelper.IndexerInBaseList.Add(tIndexer);
                    }
                }
            }
            State = NWDStatut.IndexMethodFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDBasisIndexer> IndexerInMemoryList = new List<NWDBasisIndexer>();
        public List<NWDBasisIndexer> IndexerInBaseList = new List<NWDBasisIndexer>();
        //-------------------------------------------------------------------------------------------------------------
        public void IndexInMemoryData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper IndexInMemoryData()</color>");
            foreach (NWDBasisIndexer tIndexerType in IndexerInMemoryList)
            {
                tIndexerType.IndexData(sData);
                NWDDataManager.SharedInstance().IndexationCounterOp++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeindexInMemoryData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper DeindexInMemoryData()</color>");
            foreach (NWDBasisIndexer tIndexerType in IndexerInMemoryList)
            {
                tIndexerType.DeindexData(sData);
                NWDDataManager.SharedInstance().IndexationCounterOp++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void IndexInBaseData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper IndexInBaseData()</color>");
            foreach (NWDBasisIndexer tIndexerType in IndexerInBaseList)
            {
                tIndexerType.IndexData(sData);
                NWDDataManager.SharedInstance().IndexationCounterOp++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeindexInBaseData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisHelper DeindexInBaseData()</color>");
            foreach (NWDBasisIndexer tIndexerType in IndexerInBaseList)
            {
                tIndexerType.DeindexData(sData);
                NWDDataManager.SharedInstance().IndexationCounterOp++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBasisIndexer
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual Type GetDataClass()
        {
            //Debug.Log("<color=orange>### NWDBasisIndexer GetDataClass()</color>");
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual NWDIndexerType GetIndexerType()
        {
            //Debug.Log("<color=orange>### NWDBasisIndexer GetIndexerType()</color>");
            return NWDIndexerType.InMemory;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void IndexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisIndexer IndexData()</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeindexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDBasisIndexer DeindexData()</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDIndexer<K> : NWDBasisIndexer where K : NWDTypeClass, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public override Type GetDataClass()
        {
            //Debug.Log("<color=orange>### NWDIndexer<K> GetDataClass()</color>");
            return typeof(K);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDItemByCatgeorieIndexer : NWDIndexer<NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDIndex<NWDCategory, NWDItem> kCategoryIndex = new NWDIndex<NWDCategory, NWDItem>();
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDItemByCatgeorieIndexer IndexData()</color>");
            NWDItem tData = sData as NWDItem;
            List<NWDCategory> tCategoriesList = new List<NWDCategory>();
            if (tData.CategoryList != null)
            {
                foreach (NWDCategory tCategories in tData.CategoryList.GetRawDatas())
                {
                    if (tCategoriesList.Contains(tCategories) == false)
                    {
                        tCategoriesList.Add(tCategories);
                    }
                }
            }
            kCategoryIndex.UpdateData(tData, tCategoriesList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            //Debug.Log("<color=orange>### NWDItemByCatgeorieIndexer DeindexData()</color>");
            NWDItem tData = sData as NWDItem;
            kCategoryIndex.RemoveData(tData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindDatas(NWDCategory sCategory)
        {
            //Debug.Log("<color=orange>### NWDItemByCatgeorieIndexer FindDatas()</color>");
            List<NWDItem> rReturn = new List<NWDItem>(kCategoryIndex.RawDatasByKey(sCategory));
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDItem> FindByCategorii(NWDCategory sCategory)
        {
            //Debug.Log("<color=orange>### NWDItem FindByCategorii()</color>");
            List<NWDItem> rReturn = NWDItemByCatgeorieIndexer.FindDatas(sCategory);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================