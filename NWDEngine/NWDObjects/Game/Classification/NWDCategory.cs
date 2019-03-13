﻿//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDCategoryConnection : NWDConnection<NWDCategory>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CAT")]
    [NWDClassDescriptionAttribute("This class is used to reccord the category available in the game")]
    [NWDClassMenuNameAttribute("Category")]
    public partial class NWDCategory : NWDBasis<NWDCategory>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Description", true, true, true)]
        [NWDTooltips("The name of this Category")]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Arrangement", true, true, true)]
        public NWDReferencesListType<NWDCategory> ParentCategoryList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> ChildrenCategoryList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> CascadeCategoryList
        {
            get; set;
        }










        // !!!!!!!!!!!!!
        [NWDGroupEndAttribute]
        [NWDGroupStartAttribute("Old - REMOVE", true, true, true)]
        [NWDNotEditable]
        [Obsolete]
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        [NWDNotEditable]
        [Obsolete]
        public NWDReferenceType<NWDCategory> CategoryParent
        {
            get; set;
        }
        // !!!!!!!!!!!!!






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
        private void CascadeAnalyzePrevent()
        {
            // upgrade model
            if (ParentCategoryList == null)
            {
                ParentCategoryList = new NWDReferencesListType<NWDCategory>();
            }
            if (ChildrenCategoryList == null)
            {
                ChildrenCategoryList = new NWDReferencesListType<NWDCategory>();
            }
            if (CascadeCategoryList == null)
            {
                CascadeCategoryList = new NWDReferencesListType<NWDCategory>();
            }
            if (CategoryParent.GetObject() != null)
            {
                if (ParentCategoryList.ContainsObject(CategoryParent.GetObject()) == false)
                {
                    ParentCategoryList.AddObject(CategoryParent.GetObject());
                }
            }
            // prevent basic circularity
            if (CategoryParent.ContainsObject(this) == true)
            {
                CategoryParent.Default();
            }
            if (ParentCategoryList.ContainsObject(this) == true)
            {
                ParentCategoryList.RemoveObjects(new NWDCategory[] { this });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // analyze the cascade of children
        private void CascadeAnalyze(bool tUpdate = false)
        {
            bool tTest = true;
            while (tTest == true)
            {
                tTest = false;
                // restaure the good parents
                ChildrenCategoryList.Flush();
                List<NWDCategory> tChildrensDirect = GetChildrensDirect();
                ChildrenCategoryList.AddObjects(tChildrensDirect.ToArray());

                CascadeCategoryList.Flush();
                List<NWDCategory> tCascade = GetCascade();
                CascadeCategoryList.AddObjects(tCascade.ToArray());

                foreach (NWDCategory tP in tCascade)
                {
                    if (ParentCategoryList.ContainsObject(tP))
                    {
                        ParentCategoryList.RemoveObjects(new NWDCategory[] { tP });
                        tTest = true;
                        break;
                    }
                }
            }
            if (tUpdate == true)
            {
                UpdateDataIfModifiedWithoutCallBack();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateCascade()
        {
            CascadeAnalyzePrevent();
            //List<NWDCategory> tParentsDirect = GetParentsDirect();
            List<NWDCategory> tParents = GetParents();
            foreach (NWDCategory tParent in tParents)
            {
                if (tParent.ParentCategoryList.ContainsObject(this))
                {
                    ParentCategoryList.RemoveObjects(new NWDCategory[] { tParent });
                }
            }
            // test de cicrcularité
            CascadeAnalyze(false);
            ParentsUpdate();
#if UNITY_EDITOR
            // just for improvment
            RepaintTableEditor();
            RepaintInspectorEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        // update parent evrywhere
        private void ParentsUpdate()
        {
            List<NWDCategory> tChildrenFound = new List<NWDCategory>();

            // add know children
            foreach (NWDCategory tData in ParentCategoryList.GetObjects())
            {
                tChildrenFound.Add(tData);
            }
            // analyze children lost by change
            foreach (NWDCategory tData in BasisHelper().Datas)
            {
                if (tData != null)
                {
                    if (tData.ChildrenCategoryList.ContainsObject(this))
                    {
                        if (tChildrenFound.Contains(tData) == false)
                        {
                            tChildrenFound.Add(tData);
                        }
                    }
                }
            }
            foreach (NWDCategory tData in tChildrenFound)
            {
                tData.CascadeAnalyze(true);
            }
            foreach (NWDCategory tData in tChildrenFound)
            {
                tData.ParentsUpdate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDCategory> GetParentsDirect()
        {
            List<NWDCategory> rReturn = ParentCategoryList.GetObjectsList();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDCategory> GetParents()
        {
            List<NWDCategory> rReturn = new List<NWDCategory>();
            ParentsFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ParentsFinder(List<NWDCategory> sList, NWDCategory sCat)
        {
            if (sList.Contains(sCat) == false)
            {
                sList.Add(sCat);
                // analyze children
                foreach (NWDCategory tData in sCat.ParentCategoryList.GetObjects())
                {
                    if (tData != null)
                    {
                        if (sList.Contains(tData) == false)
                        {
                            ParentsFinder(sList, tData);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDCategory> GetChildrensDirect()
        {
            List<NWDCategory> rReturn = new List<NWDCategory>();
            // analyze children
            foreach (NWDCategory tData in BasisHelper().Datas)
            {
                if (tData != null)
                {
                    if (tData.ParentCategoryList.ContainsObject(this))
                    {
                        if (rReturn.Contains(tData) == false)
                        {
                            rReturn.Add(tData);
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDCategory> GetCascade()
        {
            List<NWDCategory> rReturn = new List<NWDCategory>();
            ChildrenFinder(rReturn, this);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDCategory> GetChildrens()
        {
            List<NWDCategory> rReturn = new List<NWDCategory>();
            ChildrenFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ChildrenFinder(List<NWDCategory> sList, NWDCategory sCat)
        {
            if (sList.Contains(sCat) == false)
            {
                sList.Add(sCat);
                // analyze children
                foreach (NWDCategory tData in BasisHelper().Datas)
                {
                    if (tData != null)
                    {
                        if (tData.ParentCategoryList.ContainsObject(sCat))
                        {
                            if (sList.Contains(tData) == false)
                            {
                                ChildrenFinder(sList, tData);
                            }
                        }
                    }
                }
            }
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
                rReturn = CascadeCategoryList.ContainsObject(sCategory);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool CategoryIsContaintedIn(NWDCategory sCategory, List<NWDCategory> sCategoriesList)
        {
            bool rReturn = false;
            foreach (NWDCategory tCategory in sCategoriesList)
            {
                if (tCategory.Containts(sCategory))
                {
                    rReturn = true;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            UpdateCascade();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            UpdateCascade();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
