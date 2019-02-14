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
    public class NWDKeywordConnection : NWDConnection<NWDKeyword>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("KWD")]
    [NWDClassDescriptionAttribute("This class is used to reccord the keyword available in the game")]
    [NWDClassMenuNameAttribute("Keyword")]
    public partial class NWDKeyword : NWDBasis<NWDKeyword>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword()
        {
            //Debug.Log("NWDKeyword Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDKeyword Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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