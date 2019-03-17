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
        [NWDInspectorGroupStartAttribute("Informations", true, true, true)]
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
        [NWDInspectorGroupEndAttribute]

        [NWDInspectorGroupStartAttribute("Classification", true, true, true)]
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
        [NWDInspectorGroupEndAttribute]

        [NWDInspectorGroupStartAttribute("Universe Arrangement", true, true, true)]
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
        [NWDInspectorGroupEndAttribute]

        [NWDInspectorGroupStartAttribute("Geographical", true, true, true)]
        public NWDReferencesListType<NWDSector> BorderSectorList
        {
            get; set;
        }
        public NWDReferencesListType<NWDArea> AreaList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
