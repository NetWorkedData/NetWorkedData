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
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWordPoliticalType : BTBDataTypeEnumGeneric<NWDWordPoliticalType>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDWordPoliticalType Amex = Add(1, "Amex");
        public static NWDWordPoliticalType Tex = Add(2, "Tex");
        public static NWDWordPoliticalType Alerrrr = Add(3, "Alerrrr");
        public static NWDWordPoliticalType Alrtdtdrrrrr = Add(4, "Alrt  dtd  rr  rrr");
        public static NWDWordPoliticalType dfddf = Add(5, "A/dfddf");
        public static NWDWordPoliticalType dg = Add(6, "A/dfgggddf");
        public static NWDWordPoliticalType dffgfgfgdf = Add(7, "A/dfddfdfdf");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWordGovernmentType : BTBDataTypeEnumGeneric<NWDWordGovernmentType>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDWordGovernmentType AmexBB = Add(1, "AmexBB");
        public static NWDWordGovernmentType TexBB = Add(2, "TexBB");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWordType : BTBDataTypeEnumGeneric<NWDWordType>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDWordType Kingdom = Add(1, "Kingdom");
        public static NWDWordType Empire = Add(2, "Empire");
        public static NWDWordType Republic = Add(3, "Republic");
        public static NWDWordType Anarchic = Add(4, "Anarchic");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWordFlag : BTBDataTypeMaskGeneric<NWDWordFlag>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDWordFlag Amex = Add(1, "Amex");
        public static NWDWordFlag Tex = Add(2, "Tex");
        public static NWDWordFlag Alerrrr = Add(3, "Alerrrr");
        public static NWDWordFlag Alrtdtdrrrrr = Add(4, "Alrt  dtd  rr  rrr");
        public static NWDWordFlag dfddf = Add(5, "A/dfddf");
        public static NWDWordFlag dg = Add(6, "A/dfgggddf");
        public static NWDWordFlag dffgfgfgdf = Add(7, "A/dfddfdfdf");
        //-------------------------------------------------------------------------------------------------------------
        public void Test()
        {
            NWDWordFlag tTest = Amex | Tex; // surcharge operateur binaire
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        public NWDWordType WorldType
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
        public NWDWordGovernmentType Government
        {
            get; set;
        }
        public NWDWordPoliticalType Political
        {
            get; set;
        }
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
        //[NWDGroupEndAttribute]

        //[NWDGroupStartAttribute("Flags Universe", true, true, true)]
        //public NWDWordFlag Flag
        //{
        //    get; set;
        //}
        //public NWDWordFlag Mask
        //{
        //    get; set;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
