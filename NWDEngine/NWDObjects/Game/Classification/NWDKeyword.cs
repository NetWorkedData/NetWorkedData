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
        [NWDInformation("Use the internal key as keyword. If you need more complex classification use Category or Family!")]
        [NWDGroupStartAttribute("Informations", true, true, true)]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> DescriptionItem
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