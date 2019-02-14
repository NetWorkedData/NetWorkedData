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
        public NWDLocalizableStringType Name { get; set; }
        public NWDReferenceType<NWDCategory> CategoryParent { get; set; }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem { get; set; }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDKeyword> KeywordList { get; set; }
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
