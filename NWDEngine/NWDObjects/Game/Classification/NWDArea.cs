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
    public class NWDAreaConnection : NWDConnection<NWDArea>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AREA")]
    [NWDClassDescriptionAttribute("This class is used to reccord the world/univers/island available in the game")]
    [NWDClassMenuNameAttribute("Area")]
    public partial class NWDArea : NWDBasis<NWDArea>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        [NWDTooltips("The name of this world")]
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

        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FamilyList
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Universe Arrangement", true, true, true)]
        public NWDReferencesListType<NWDArea> ParentAreaList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDArea> ChildAreaList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDArea> CascadeAreaList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDArea()
        {
            //Debug.Log("NWDArea Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDArea(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDArea Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
            if (ParentAreaList == null)
            {
                ParentAreaList = new NWDReferencesListType<NWDArea>();
            }
            if (ChildAreaList == null)
            {
                ChildAreaList = new NWDReferencesListType<NWDArea>();
            }
            if (CascadeAreaList == null)
            {
                CascadeAreaList = new NWDReferencesListType<NWDArea>();
            }
            if (ParentAreaList.ContainsObject(this) == true)
            {
                ParentAreaList.RemoveObjects(new NWDArea[] { this });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // analyze the cascade of children
        private void CascadeAnalyze(bool tUpdate = false)
        {
            CascadeAnalyzePrevent();
            bool tTest = true;
            while (tTest == true)
            {
                tTest = false;
                // restaure the good parents
                ChildAreaList.Flush();
                List<NWDArea> tChildrensDirect = GetChildrensDirect();
                ChildAreaList.AddObjects(tChildrensDirect.ToArray());

                CascadeAreaList.Flush();
                List<NWDArea> tCascade = GetCascade();
                CascadeAreaList.AddObjects(tCascade.ToArray());

                foreach (NWDArea tP in tCascade)
                {
                    if (ParentAreaList.ContainsObject(tP))
                    {
                        ParentAreaList.RemoveObjects(new NWDArea[] { tP });
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
            List<NWDArea> tParents = GetParents();
            foreach (NWDArea tParent in tParents)
            {
                if (tParent.ParentAreaList.ContainsObject(this))
                {
                    ParentAreaList.RemoveObjects(new NWDArea[] { tParent });
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
            List<NWDArea> tChildrenFound = new List<NWDArea>();

            // add know children
            foreach (NWDArea tData in ParentAreaList.GetObjects())
            {
                tChildrenFound.Add(tData);
            }
            // analyze children lost by change
            foreach (NWDArea tData in BasisHelper().Datas)
            {
                if (tData != null)
                {
                    if (tData.ChildAreaList.ContainsObject(this))
                    {
                        if (tChildrenFound.Contains(tData) == false)
                        {
                            tChildrenFound.Add(tData);
                        }
                    }
                }
            }
            foreach (NWDArea tData in tChildrenFound)
            {
                tData.CascadeAnalyze(true);
            }
            foreach (NWDArea tData in tChildrenFound)
            {
                tData.ParentsUpdate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDArea> GetParentsDirect()
        {
            List<NWDArea> rReturn = ParentAreaList.GetObjectsList();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDArea> GetParents()
        {
            List<NWDArea> rReturn = new List<NWDArea>();
            ParentsFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ParentsFinder(List<NWDArea> sList, NWDArea sArea)
        {
            if (sList.Contains(sArea) == false)
            {
                sList.Add(sArea);
                // analyze children
                foreach (NWDArea tData in sArea.ParentAreaList.GetObjects())
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
        private List<NWDArea> GetChildrensDirect()
        {
            List<NWDArea> rReturn = new List<NWDArea>();
            // analyze children
            foreach (NWDArea tData in BasisHelper().Datas)
            {
                tData.CascadeAnalyzePrevent();
                if (tData != null)
                {
                    if (tData.ParentAreaList.ContainsObject(this))
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
        private List<NWDArea> GetCascade()
        {
            List<NWDArea> rReturn = new List<NWDArea>();
            ChildrenFinder(rReturn, this);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDArea> GetChildrens()
        {
            List<NWDArea> rReturn = new List<NWDArea>();
            ChildrenFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ChildrenFinder(List<NWDArea> sList, NWDArea sArea)
        {
            if (sList.Contains(sArea) == false)
            {
                sList.Add(sArea);
                // analyze children
                foreach (NWDArea tData in BasisHelper().Datas)
                {
                    tData.CascadeAnalyzePrevent();
                    if (tData != null)
                    {
                        if (tData.ParentAreaList.ContainsObject(sArea))
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
        public bool Containts(NWDArea sArea)
        {
            bool rReturn = false;
            if (sArea == this)
            {
                rReturn = true;
            }
            else
            {
                rReturn = CascadeAreaList.ContainsObject(sArea);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool CategoryIsContaintedIn(NWDArea sArea, List<NWDArea> sAreasList)
        {
            bool rReturn = false;
            foreach (NWDArea tCategory in sAreasList)
            {
                if (tCategory.Containts(sArea))
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
