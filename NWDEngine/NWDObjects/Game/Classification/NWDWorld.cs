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
    public class NWDWorldConnection : NWDConnection<NWDWorld>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWordType: int
    {
        Kingdom,
        Empire,
        Republic,
        Anarchic,

        None
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("WRD")]
    [NWDClassDescriptionAttribute("This class is used to reccord the world/univers/island available in the game")]
    [NWDClassMenuNameAttribute("World")]
    public partial class NWDWorld : NWDBasis<NWDWorld>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        [NWDTooltips("The name of this world")]
        public NWDWordType WordType
        {
            get; set;
        }
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The subname of this world or description tags")]
        public NWDLocalizableStringType SubName
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Geographical", true, true, true)]
        public NWDReferencesListType<NWDSector> SectorList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Political Universe", true, true, true)]
        [NWDNotEditable]
        public NWDReferencesListType<NWDCharacter> CharacterList
        {
            get; set;
        }
        public NWDReferencesListType<NWDWorld> FriendWorldList
        {
            get; set;
        }
        public NWDReferencesListType<NWDWorld> NeutralWorldList
        {
            get; set;
        }
        public NWDReferencesListType<NWDWorld> EnemyWorldList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDWorld()
        {
            //Debug.Log("NWDWorld Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDWorld(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDWorld Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
//            if (ParentWorldList == null)
//            {
//                ParentWorldList = new NWDReferencesListType<NWDWorld>();
//            }
//            if (ChildWorldList == null)
//            {
//                ChildWorldList = new NWDReferencesListType<NWDWorld>();
//            }
//            if (CascadeWorldList == null)
//            {
//                CascadeWorldList = new NWDReferencesListType<NWDWorld>();
//            }
//            // prevent basic circularity
//            if (WorldParent.ContainsObject(this) == true)
//            {
//                WorldParent.Default();
//            }
//            if (ParentWorldList.ContainsObject(this) == true)
//            {
//                ParentWorldList.RemoveObjects(new NWDWorld[] { this });
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        // analyze the cascade of children
//        private void CascadeAnalyze(bool tUpdate = false)
//        {
//            CascadeAnalyzePrevent();
//            bool tTest = true;
//            while (tTest == true)
//            {
//                tTest = false;
//                // restaure the good parents
//                ChildWorldList.Flush();
//                List<NWDWorld> tChildrensDirect = GetChildrensDirect();
//                ChildWorldList.AddObjects(tChildrensDirect.ToArray());

//                CascadeWorldList.Flush();
//                List<NWDWorld> tCascade = GetCascade();
//                CascadeWorldList.AddObjects(tCascade.ToArray());

//                foreach (NWDWorld tP in tCascade)
//                {
//                    if (ParentWorldList.ContainsObject(tP))
//                    {
//                        ParentWorldList.RemoveObjects(new NWDWorld[] { tP });
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
//            List<NWDWorld> tParents = GetParents();
//            foreach (NWDWorld tParent in tParents)
//            {
//                if (tParent.ParentWorldList.ContainsObject(this))
//                {
//                    ParentWorldList.RemoveObjects(new NWDWorld[] { tParent });
//                }
//            }
//            // test de cicrcularité
//            CascadeAnalyze(false);
//            ParentsUpdate();
//#if UNITY_EDITOR
//            // just for improvment
//            RepaintTableEditor();
//            RepaintInspectorEditor();
//#endif
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //// update parent evrywhere
        //private void ParentsUpdate()
        //{
        //    List<NWDWorld> tChildrenFound = new List<NWDWorld>();

        //    // add know children
        //    foreach (NWDWorld tData in ParentWorldList.GetObjects())
        //    {
        //        tChildrenFound.Add(tData);
        //    }
        //    // analyze children lost by change
        //    foreach (NWDWorld tData in BasisHelper().Datas)
        //    {
        //        if (tData != null)
        //        {
        //            if (tData.ChildWorldList.ContainsObject(this))
        //            {
        //                if (tChildrenFound.Contains(tData) == false)
        //                {
        //                    tChildrenFound.Add(tData);
        //                }
        //            }
        //        }
        //    }
        //    foreach (NWDWorld tData in tChildrenFound)
        //    {
        //        tData.CascadeAnalyze(true);
        //    }
        //    foreach (NWDWorld tData in tChildrenFound)
        //    {
        //        tData.ParentsUpdate();
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private List<NWDWorld> GetParentsDirect()
        //{
        //    List<NWDWorld> rReturn = ParentWorldList.GetObjectsList();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private List<NWDWorld> GetParents()
        //{
        //    List<NWDWorld> rReturn = new List<NWDWorld>();
        //    ParentsFinder(rReturn, this);
        //    if (rReturn.Contains(this))
        //    {
        //        rReturn.Remove(this);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private static void ParentsFinder(List<NWDWorld> sList, NWDWorld sWorld)
        //{
        //    if (sList.Contains(sWorld) == false)
        //    {
        //        sList.Add(sWorld);
        //        // analyze children
        //        foreach (NWDWorld tData in sWorld.ParentWorldList.GetObjects())
        //        {
        //            if (tData != null)
        //            {
        //                if (sList.Contains(tData) == false)
        //                {
        //                    ParentsFinder(sList, tData);
        //                }
        //            }
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private List<NWDWorld> GetChildrensDirect()
        //{
        //    List<NWDWorld> rReturn = new List<NWDWorld>();
        //    // analyze children
        //    foreach (NWDWorld tData in BasisHelper().Datas)
        //    {
        //        tData.CascadeAnalyzePrevent();
        //        if (tData != null)
        //        {
        //            if (tData.ParentWorldList.ContainsObject(this))
        //            {
        //                if (rReturn.Contains(tData) == false)
        //                {
        //                    rReturn.Add(tData);
        //                }
        //            }
        //        }
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private List<NWDWorld> GetCascade()
        //{
        //    List<NWDWorld> rReturn = new List<NWDWorld>();
        //    ChildrenFinder(rReturn, this);
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private List<NWDWorld> GetChildrens()
        //{
        //    List<NWDWorld> rReturn = new List<NWDWorld>();
        //    ChildrenFinder(rReturn, this);
        //    if (rReturn.Contains(this))
        //    {
        //        rReturn.Remove(this);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private static void ChildrenFinder(List<NWDWorld> sList, NWDWorld sWorld)
        //{
        //    if (sList.Contains(sWorld) == false)
        //    {
        //        sList.Add(sWorld);
        //        // analyze children
        //        foreach (NWDWorld tData in BasisHelper().Datas)
        //        {
        //            tData.CascadeAnalyzePrevent();
        //            if (tData != null)
        //            {
        //                if (tData.ParentWorldList.ContainsObject(sWorld))
        //                {
        //                    if (sList.Contains(tData) == false)
        //                    {
        //                        ChildrenFinder(sList, tData);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool Containts(NWDWorld sWorld)
        //{
        //    bool rReturn = false;
        //    if (sWorld == this)
        //    {
        //        rReturn = true;
        //    }
        //    else
        //    {
        //        rReturn = CascadeWorldList.ContainsObject(sWorld);
        //    }
        //    return rReturn;
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public static bool CategoryIsContaintedIn(NWDWorld sWorld, List<NWDWorld> sWorldsList)
        //{
        //    bool rReturn = false;
        //    foreach (NWDWorld tCategory in sWorldsList)
        //    {
        //        if (tCategory.Containts(sWorld))
        //        {
        //            rReturn = true;
        //            break;
        //        }
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonUpdateMe()
        //{
        //    UpdateCascade();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void AddonDuplicateMe()
        //{
        //    UpdateCascade();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
