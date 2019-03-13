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
using System.Linq;
using System.Reflection;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDSectorConnection : NWDConnection<NWDSector>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //public abstract class BTBDataTypeEnum : IComparable
    //{
    //    public string Name
    //    {
    //        get; private set;
    //    }

    //    public int Id
    //    {
    //        get; private set;
    //    }

    //    protected BTBDataTypeEnum()
    //    {
    //    }

    //    protected BTBDataTypeEnum(int id, string name)
    //    {
    //        Id = id;
    //        Name = name;
    //    }

    //    public override string ToString() => Name;

    //    public static IEnumerable<T> GetAll<T>() where T : BTBDataTypeEnum
    //    {
    //        var fields = typeof(T).GetFields(BindingFlags.Public |
    //                                         BindingFlags.Static |
    //                                         BindingFlags.DeclaredOnly);

    //        return fields.Select(f => f.GetValue(null)).Cast<T>();
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        var otherValue = obj as BTBDataTypeEnum;

    //        if (otherValue == null)
    //            return false;

    //        var typeMatches = GetType().Equals(obj.GetType());
    //        var valueMatches = Id.Equals(otherValue.Id);

    //        return typeMatches && valueMatches;
    //    }

    //    public int CompareTo(object other) => Id.CompareTo(((BTBDataTypeEnum)other).Id);

    //    // Other utility methods ... 
    //}
    //public abstract partial class CardType : BTBDataTypeEnum
    //{
    //    public static CardType Amex = new AmexCardType();
    //    public static CardType Visa = new VisaCardType();
    //    public static CardType MasterCard = new MasterCardType();

    //    protected CardType(int id, string name)
    //        : base(id, name)
    //    {
    //    }

    //    private class AmexCardType : CardType
    //    {
    //        public AmexCardType() : base(1, "Amex")
    //        {
    //        }
    //    }

    //    private class VisaCardType : CardType
    //    {
    //        public VisaCardType() : base(2, "Visa")
    //        {
    //        }
    //    }

    //    private class MasterCardType : CardType
    //    {
    //        public MasterCardType() : base(3, "MasterCard")
    //        {
    //        }
    //    }
    //}
    //public abstract partial class CardType : BTBDataTypeEnum
    //{
    //    public static CardType Truc = new TrucCardType(); 
    //    private class TrucCardType : CardType
    //    {
    //        public TrucCardType() : base(4, "Truc")
    //        {
    //        }
    //    }
    //}
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public enum NWDSectorLevel : int
    {
        StellarSystem = -10,
        Planet = 1,
        Satellite,
        Continent,
        Land,
        Country,
        Region,
        County,
        City,
        Village,
        District,
        House,
        WC,

        None
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SCTR")]
    [NWDClassDescriptionAttribute("This class is used to reccord the world/univers/island available in the game")]
    [NWDClassMenuNameAttribute("Sector")]
    public partial class NWDSector : NWDBasis<NWDSector>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        [NWDTooltips("The name of this world")]
        public NWDSectorLevel SectorLevel
        {
            get; set;
        }
        //public CardType CardTypeLevel
        //{
        //    get; set;
        //}
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
        public NWDReferencesListType<NWDSector> ParentSectorList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDSector> ChildSectorList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDSector> CascadeSectorList
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupStartAttribute("Geographical", true, true, true)]
        public NWDReferencesListType<NWDSector> BorderSectorList
        {
            get; set;
        }
        public NWDReferencesListType<NWDArea> AreaList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSector()
        {
            //Debug.Log("NWDSector Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSector(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDSector Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //CardTypeLevel = CardType.Truc;
        }
        //-------------------------------------------------------------------------------------------------------------
        // prevent exec bad
        private void CascadeAnalyzePrevent()
        {
            // upgrade model
            if (ParentSectorList == null)
            {
                ParentSectorList = new NWDReferencesListType<NWDSector>();
            }
            if (ChildSectorList == null)
            {
                ChildSectorList = new NWDReferencesListType<NWDSector>();
            }
            if (CascadeSectorList == null)
            {
                CascadeSectorList = new NWDReferencesListType<NWDSector>();
            }
            // prevent basic circularity
            if (ParentSectorList.ContainsObject(this) == true)
            {
                ParentSectorList.RemoveObjects(new NWDSector[] { this });
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
                ChildSectorList.Flush();
                List<NWDSector> tChildrensDirect = GetChildrensDirect();
                ChildSectorList.AddObjects(tChildrensDirect.ToArray());

                CascadeSectorList.Flush();
                List<NWDSector> tCascade = GetCascade();
                CascadeSectorList.AddObjects(tCascade.ToArray());

                foreach (NWDSector tP in tCascade)
                {
                    if (ParentSectorList.ContainsObject(tP))
                    {
                        ParentSectorList.RemoveObjects(new NWDSector[] { tP });
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
            List<NWDSector> tParents = GetParents();
            foreach (NWDSector tParent in tParents)
            {
                if (tParent.ParentSectorList.ContainsObject(this))
                {
                    ParentSectorList.RemoveObjects(new NWDSector[] { tParent });
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
            List<NWDSector> tChildrenFound = new List<NWDSector>();

            // add know children
            foreach (NWDSector tData in ParentSectorList.GetObjects())
            {
                tChildrenFound.Add(tData);
            }
            // analyze children lost by change
            foreach (NWDSector tData in BasisHelper().Datas)
            {
                if (tData != null)
                {
                    if (tData.ChildSectorList.ContainsObject(this))
                    {
                        if (tChildrenFound.Contains(tData) == false)
                        {
                            tChildrenFound.Add(tData);
                        }
                    }
                }
            }
            foreach (NWDSector tData in tChildrenFound)
            {
                tData.CascadeAnalyze(true);
            }
            foreach (NWDSector tData in tChildrenFound)
            {
                tData.ParentsUpdate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDSector> GetParentsDirect()
        {
            List<NWDSector> rReturn = ParentSectorList.GetObjectsList();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDSector> GetParents()
        {
            List<NWDSector> rReturn = new List<NWDSector>();
            ParentsFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ParentsFinder(List<NWDSector> sList, NWDSector sSector)
        {
            if (sList.Contains(sSector) == false)
            {
                sList.Add(sSector);
                // analyze children
                foreach (NWDSector tData in sSector.ParentSectorList.GetObjects())
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
        private List<NWDSector> GetChildrensDirect()
        {
            List<NWDSector> rReturn = new List<NWDSector>();
            // analyze children
            foreach (NWDSector tData in BasisHelper().Datas)
            {
                tData.CascadeAnalyzePrevent();
                if (tData != null)
                {
                    if (tData.ParentSectorList.ContainsObject(this))
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
        private List<NWDSector> GetCascade()
        {
            List<NWDSector> rReturn = new List<NWDSector>();
            ChildrenFinder(rReturn, this);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<NWDSector> GetChildrens()
        {
            List<NWDSector> rReturn = new List<NWDSector>();
            ChildrenFinder(rReturn, this);
            if (rReturn.Contains(this))
            {
                rReturn.Remove(this);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ChildrenFinder(List<NWDSector> sList, NWDSector sSector)
        {
            if (sList.Contains(sSector) == false)
            {
                sList.Add(sSector);
                // analyze children
                foreach (NWDSector tData in BasisHelper().Datas)
                {
                    tData.CascadeAnalyzePrevent();
                    if (tData != null)
                    {
                        if (tData.ParentSectorList.ContainsObject(sSector))
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
        public bool Containts(NWDSector sSector)
        {
            bool rReturn = false;
            if (sSector == this)
            {
                rReturn = true;
            }
            else
            {
                rReturn = CascadeSectorList.ContainsObject(sSector);
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public static bool CategoryIsContaintedIn(NWDSector sSector, List<NWDSector> sSectorsList)
        {
            bool rReturn = false;
            foreach (NWDSector tCategory in sSectorsList)
            {
                if (tCategory.Containts(sSector))
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
