////=====================================================================================================================
////
//// ideMobi copyright 2019
//// All rights reserved by ideMobi
////
//// Read License-en or Licence-fr
////
////=====================================================================================================================
//using System;
////=====================================================================================================================
//namespace NetWorkedData
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//	[Serializable]
//    public class NWDDistrictConnection : NWDConnection<NWDDistrict>
//    {
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//	[NWDClassServerSynchronizeAttribute(true)]
//    [NWDClassTrigrammeAttribute("DIS")]
//    [NWDClassDescriptionAttribute("This class is used to reccord the distric available in the game")]
//    [NWDClassMenuNameAttribute("District")]
//    public partial class NWDDistrict : NWDBasis<NWDDistrict>
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        [NWDGroupStartAttribute("Informations", true, true, true)]
//        [NWDTooltips("The name of this district")]
//        public NWDLocalizableStringType Name
//        {
//            get; set;
//        }
//        [NWDTooltips("The subname of this district or description tags")]
//        public NWDLocalizableStringType SubName
//        {
//            get; set;
//        }
//        [NWDTooltips("The description item. Usable to be ownershipped")]
//        public NWDReferenceType<NWDItem> DescriptionItem
//        {
//            get; set;
//        }
//        [NWDGroupEndAttribute]
        
//        [NWDGroupStartAttribute("Universe Arrangement", true, true, true)]
//        public NWDReferenceType<NWDWorld> ParentWorld
//        {
//            get; set;
//        }
//        [NWDGroupEndAttribute]

//        [NWDGroupStartAttribute("Geographical", true, true, true)]
//        [Obsolete]
//        [NWDNotEditable]
//        public NWDReferencesListType<NWDDistrict> BorderDistrictList
//        {
//            get; set;
//        }

//        [NWDGroupStartAttribute("Classification", true, true, true)]
//        public NWDReferencesListType<NWDCategory> CategoryList
//        {
//            get; set;
//        }
//        public NWDReferencesListType<NWDFamily> FamilyList
//        {
//            get; set;
//        }
//        public NWDReferencesListType<NWDKeyword> KeywordList
//        {
//            get; set;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDDistrict()
//        {
//            //Debug.Log("NWDDistrict Constructor");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDDistrict(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
//        {
//            //Debug.Log("NWDDistrict Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void Initialization()
//        {
//        }
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
////=====================================================================================================================
