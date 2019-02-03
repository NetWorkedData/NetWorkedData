//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[Serializable]
    public class NWDDistrictConnection : NWDConnection<NWDDistrict>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("DIS")]
    [NWDClassDescriptionAttribute("This class is used to reccord the distric available in the game")]
    [NWDClassMenuNameAttribute("District")]
    public partial class NWDDistrict : NWDBasis<NWDDistrict>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        public NWDLocalizableStringType SubName
        {
            get; set;
        }
        public NWDLocalizableStringType Description
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Universe Arrangement", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList
        {
            get; set;
        }
        public NWDReferencesListType<NWDDistrict> DistrictList
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
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
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Assets", true, true, true)]
        public NWDColorType PrimaryColor
        {
            get; set;
        }
        public NWDColorType SecondaryColor
        {
            get; set;
        }
        public NWDColorType TertiaryColor
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDistrict()
        {
            //Debug.Log("NWDDistrict Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDistrict(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDDistrict Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
