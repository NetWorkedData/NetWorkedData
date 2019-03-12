//=====================================================================================================================
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
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDHidden]
        public NWDReferenceType<NWDCategory> CategoryParent
        {
            get; set;
        }

        public NWDReferencesListType<NWDCategory> CategoriesParents
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> CategoriesChildren
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> CategoriesCascade
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
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
            if (CategoriesParents == null)
            {
                CategoriesParents = new NWDReferencesListType<NWDCategory>();
            }
            if (CategoriesChildren == null)
            {
                CategoriesChildren = new NWDReferencesListType<NWDCategory>();
            }
            if (CategoriesCascade == null)
            {
                CategoriesCascade = new NWDReferencesListType<NWDCategory>();
            }
            if (CategoryParent.GetObject() != null)
            {
                if (CategoriesParents.ContainsObject(CategoryParent.GetObject()) == false)
                {
                    CategoriesParents.AddObject(CategoryParent.GetObject());
                }
            }
            // prevent basic circularity
            if (CategoryParent.ContainsObject(this) == true)
            {
                CategoryParent.Default();
            }
            if (CategoriesParents.ContainsObject(this) == true)
            {
                CategoriesParents.RemoveObjects(new NWDCategory[] { this });
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
                CategoriesChildren.Flush();
                List<NWDCategory> tChildrensDirect = GetChildrensDirect();
                CategoriesChildren.AddObjects(tChildrensDirect.ToArray());

                CategoriesCascade.Flush();
                List<NWDCategory> tCascade = GetCascade();
                CategoriesCascade.AddObjects(tCascade.ToArray());

                foreach (NWDCategory tP in tCascade)
                {
                    if (CategoriesParents.ContainsObject(tP))
                    {
                        CategoriesParents.RemoveObjects(new NWDCategory[] { tP });
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
                if (tParent.CategoriesParents.ContainsObject(this))
                {
                    CategoriesParents.RemoveObjects(new NWDCategory[] { tParent });
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
            foreach (NWDCategory tData in CategoriesParents.GetObjects())
            {
                tChildrenFound.Add(tData);
            }
            // analyze children lost by change
            foreach (NWDCategory tData in BasisHelper().Datas)
            {
                if (tData != null)
                {
                    if (tData.CategoriesChildren.ContainsObject(this))
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
            List<NWDCategory> rReturn = CategoriesParents.GetObjectsList();
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
                foreach (NWDCategory tData in sCat.CategoriesParents.GetObjects())
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
                    if (tData.CategoriesParents.ContainsObject(this))
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
                        if (tData.CategoriesParents.ContainsObject(sCat))
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
                rReturn = CategoriesCascade.ContainsObject(sCategory);
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
