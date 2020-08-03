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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftRecipient : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftRecipient()
        {
            //Debug.Log("NWDRecipientGroup Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftRecipient(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDRecipientGroup Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            CraftOnlyMax = true;
            CraftUnUsedElements = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            //CheckMyItems();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
            //Debug.Log("AddonDeleteMe()");
            //ItemList = new NWDReferencesListType<NWDItem>();
            //CheckMyItems();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckMyItems()
        {
            //List<NWDItem> tActualItems = ItemList.GetObjectsList();
            //foreach (NWDItem tItem in NWDItem.FindDatas())
            //{
            //    if (tActualItems.Contains(tItem))
            //    {
            //        if (tItem.RecipientGroupList.GetObjectsList().Contains(this) == true)
            //        {
            //            // ok It's contains me
            //        }
            //        else
            //        {
            //            // oh item group not contains me! WHYYYYYYYY
            //            tItem.RecipientGroupList.AddObject(this);
            //            tItem.UpdateData();
            //            foreach (NWDCraftBook tCraftbook in NWDBasisHelper.BasisHelper<NWDCraftBook>().Datas)
            //            {
            //                if (tCraftbook.RecipientGroup.ContainsObject(this))
            //                {
            //                    tCraftbook.RecalculMe();
            //                    tCraftbook.UpdateDataIfModified();
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (tItem.RecipientGroupList.GetObjectsList().Contains(this))
            //        {
            //            // Oh This ItemGroup contains me but I not refere it ... remove me from it
            //            tItem.RecipientGroupList.RemoveObjects(new NWDRecipientGroup[] { this });
            //            tItem.UpdateData();
            //            foreach (NWDCraftBook tCraftbook in NWDBasisHelper.BasisHelper<NWDCraftBook>().Datas)
            //            {
            //                if (tCraftbook.RecipientGroup.ContainsObject(this))
            //                {
            //                    tCraftbook.RecalculMe();
            //                    tCraftbook.UpdateDataIfModified();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            // ok i'ts not contains me!
            //        }
            //    }
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================