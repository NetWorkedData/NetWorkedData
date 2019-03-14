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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
