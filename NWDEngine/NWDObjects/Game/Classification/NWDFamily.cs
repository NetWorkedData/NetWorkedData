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
    public class NWDFamilyConnection : NWDConnection<NWDFamily>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("FAM")]
    [NWDClassDescriptionAttribute("This class is used to reccord the family available in the game")]
    [NWDClassMenuNameAttribute("Family")]
    public partial class NWDFamily : NWDBasis<NWDFamily>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name { get; set; }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem { get; set; }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList { get; set; }
        public NWDReferencesListType<NWDCategory> CategoryList { get; set; }
        public NWDReferencesListType<NWDFamily> FamilyList { get; set; }
        public NWDReferencesListType<NWDKeyword> KeywordList { get; set; }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        public NWDFamily()
        {
            //Debug.Log("NWDFamily Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDFamily(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDFamily Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
