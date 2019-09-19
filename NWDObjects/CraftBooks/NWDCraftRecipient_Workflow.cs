//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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