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
        // prevent exec bad
//        private void CascadeAnalyzePrevent()
//        {
//            // upgrade model
//            if (ParentCategoryList == null)
//            {
//                ParentCategoryList = new NWDReferencesListType<NWDCategory>();
//            }
//            if (ChildrenCategoryList == null)
//            {
//                ChildrenCategoryList = new NWDReferencesListType<NWDCategory>();
//            }
//            if (CascadeCategoryList == null)
//            {
//                CascadeCategoryList = new NWDReferencesListType<NWDCategory>();
//            }
//            if (ParentCategoryList.ConstaintsData(this) == true)
//            {
//                ParentCategoryList.RemoveDatas(new NWDCategory[] { this });
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        // analyze the cascade of children
//        private void CascadeAnalyze(bool tUpdate = false)
//        {
//            bool tTest = true;
//            while (tTest == true)
//            {
//                tTest = false;
//                // restaure the good parents
//                ChildrenCategoryList.Flush();
//                List<NWDCategory> tChildrensDirect = GetChildrensDirect();
//                ChildrenCategoryList.AddDatas(tChildrensDirect.ToArray());

//                CascadeCategoryList.Flush();
//                List<NWDCategory> tCascade = GetCascade();
//                CascadeCategoryList.AddDatas(tCascade.ToArray());

//                foreach (NWDCategory tP in tCascade)
//                {
//                    if (ParentCategoryList.ConstaintsData(tP))
//                    {
//                        ParentCategoryList.RemoveDatas(new NWDCategory[] { tP });
//                        tTest = true;
//                        break;
//                    }
//                }
//            }
//            if (tUpdate == true)
//            {
//                UpdateDataIfModifiedWithoutCallBack();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private void UpdateCascade()
//        {
//            CascadeAnalyzePrevent();
//            //List<NWDCategory> tParentsDirect = GetParentsDirect();
//            List<NWDCategory> tParents = GetParents();
//            foreach (NWDCategory tParent in tParents)
//            {
//                if (tParent.ParentCategoryList.ConstaintsData(this))
//                {
//                    ParentCategoryList.RemoveDatas(new NWDCategory[] { tParent });
//                }
//            }
//            // test de cicrcularité
//            CascadeAnalyze(false);
//            ParentsUpdate();
//#if UNITY_EDITOR
//            // just for improvment
//            BasisHelper().RepaintTableEditor();
//            BasisHelper().RepaintInspectorEditor();
//#endif
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        // update parent evrywhere
//        private void ParentsUpdate()
//        {
//            List<NWDCategory> tChildrenFound = new List<NWDCategory>();

//            // add know children
//            foreach (NWDCategory tData in ParentCategoryList.GetReachableDatas())
//            {
//                tChildrenFound.Add(tData);
//            }
//            // analyze children lost by change
//            foreach (NWDCategory tData in BasisHelper().Datas)
//            {
//                if (tData != null)
//                {
//                    if (tData.ChildrenCategoryList != null)
//                    {
//                        if (tData.ChildrenCategoryList.ConstaintsData(this))
//                        {
//                            if (tChildrenFound.Contains(tData) == false)
//                            {
//                                tChildrenFound.Add(tData);
//                            }
//                        }
//                    }
//                }
//            }
//            foreach (NWDCategory tData in tChildrenFound)
//            {
//                tData.CascadeAnalyze(true);
//            }
//            foreach (NWDCategory tData in tChildrenFound)
//            {
//                tData.ParentsUpdate();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private List<NWDCategory> GetParentsDirect()
//        {
//            List<NWDCategory> rReturn = ParentCategoryList.GetReachableDatasList();
//            return rReturn;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private List<NWDCategory> GetParents()
//        {
//            List<NWDCategory> rReturn = new List<NWDCategory>();
//            ParentsFinder(rReturn, this);
//            if (rReturn.Contains(this))
//            {
//                rReturn.Remove(this);
//            }
//            return rReturn;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private static void ParentsFinder(List<NWDCategory> sList, NWDCategory sCat)
//        {
//            if (sList.Contains(sCat) == false)
//            {
//                sList.Add(sCat);
//                // analyze children
//                foreach (NWDCategory tData in sCat.ParentCategoryList.GetReachableDatas())
//                {
//                    if (tData != null)
//                    {
//                        if (sList.Contains(tData) == false)
//                        {
//                            ParentsFinder(sList, tData);
//                        }
//                    }
//                }
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private List<NWDCategory> GetChildrensDirect()
//        {
//            List<NWDCategory> rReturn = new List<NWDCategory>();
//            // analyze children
//            foreach (NWDCategory tData in BasisHelper().Datas)
//            {
//                if (tData != null)
//                {
//                    if (tData.ParentCategoryList != null)
//                    {
//                        if (tData.ParentCategoryList.ConstaintsData(this))
//                        {
//                            if (rReturn.Contains(tData) == false)
//                            {
//                                rReturn.Add(tData);
//                            }
//                        }
//                    }
//                }
//            }
//            return rReturn;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private List<NWDCategory> GetCascade()
//        {
//            List<NWDCategory> rReturn = new List<NWDCategory>();
//            ChildrenFinder(rReturn, this);
//            return rReturn;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private List<NWDCategory> GetChildrens()
//        {
//            List<NWDCategory> rReturn = new List<NWDCategory>();
//            ChildrenFinder(rReturn, this);
//            if (rReturn.Contains(this))
//            {
//                rReturn.Remove(this);
//            }
//            return rReturn;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        private void ChildrenFinder(List<NWDCategory> sList, NWDCategory sCat)
//        {
//            if (sList.Contains(sCat) == false)
//            {
//                //sList.Add(sCat);
//                // analyze children
//                foreach (NWDCategory tData in BasisHelper().Datas)
//                {
//                    if (tData != null)
//                    {
//                        if (tData.ParentCategoryList != null)
//                        {
//                            if (tData.ParentCategoryList.ConstaintsData(sCat))
//                            {
//                                if (sList.Contains(tData) == false)
//                                {
//                                    ChildrenFinder(sList, tData);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
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
        //-------------------------------------------------------------------------------------------------------------
        //public bool CategoryIsContaintedIn(NWDCategory sCategory, List<NWDCategory> sCategoriesList)
        //{
        //    bool rReturn = false;
        //    foreach (NWDCategory tCategory in sCategoriesList)
        //    {
        //        if (tCategory.Containts(sCategory))
        //        {
        //            rReturn = true;
        //            break;
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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
        public override void AddonDuplicateMe()
        {
            UpdateAll();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ChildrenAssembly(List<NWDCategory> sList, NWDCategory sCat)
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
        private static void UpdateAll()
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
                tData.CascadeCategoryList.AddDatas(tList.ToArray());
                tData.UpdateDataIfModified();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;
            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;
            foreach (NWDItem tItem in NWDItem.FindByCategory(this))
            {
                GUI.Label(new Rect(tX, tY, tWidth, NWDGUI.kLabelStyle.fixedHeight), tItem.InternalKey+" "+ tItem.Reference, NWDGUI.kLabelStyle);
                tY += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
