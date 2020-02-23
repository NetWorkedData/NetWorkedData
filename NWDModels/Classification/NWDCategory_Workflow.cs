//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:20
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCategory : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDCategory()
        {
            //Debug.Log("NWDCategory Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCategory(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCategory Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Containts(NWDCategory sCategory)
        {
            bool rReturn = false;
            if (sCategory == this)
            {
                rReturn = true;
            }
            else
            {
                rReturn = CascadeCategoryList.ConstaintsData(sCategory);
            }
            return rReturn;
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicatedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;
            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;
            //foreach (NWDItem tItem in NWDItem.FindByCategory(this))
            //{
            //    GUI.Label(new Rect(tX, tY, tWidth, NWDGUI.kLabelStyle.fixedHeight), tItem.InternalKey+" "+ tItem.Reference, NWDGUI.kLabelStyle);
            //    tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        static void ChildrenAssembly(List<NWDCategory> sList, NWDCategory sCat)
        {
            if (sList.Contains(sCat) == false)
            {
                sList.Add(sCat);
                foreach (NWDCategory tData in sCat.ChildrenCategoryList.GetEditorDatas())
                {
                    if (tData != null)
                    {
                        ChildrenAssembly(sList, tData);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static void UpdateAll()
        {
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                tData.ChildrenCategoryList.Flush();
                tData.CascadeCategoryList.Flush();
            }
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                foreach (NWDCategory tDataSub in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
                {
                    if (tDataSub != tData)
                    {
                        if (tDataSub.ParentCategoryList != null)
                        {
                            if (tDataSub.ParentCategoryList.ConstaintsData(tData))
                            {
                                if (tData.ChildrenCategoryList.ConstaintsData(tDataSub) == false)
                                {
                                    tData.ChildrenCategoryList.AddData(tDataSub);
                                }
                            }
                        }
                    }
                }
            }
            foreach (NWDCategory tData in NWDBasisHelper.BasisHelper<NWDCategory>().Datas)
            {
                List<NWDCategory> tList = new List<NWDCategory>();
                ChildrenAssembly(tList, tData);
                tData.CascadeCategoryList.Flush();
                tData.CascadeCategoryList.AddData(tData);
                tData.CascadeCategoryList.AddDatas(tList.ToArray());
                tData.UpdateDataIfModified();
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
